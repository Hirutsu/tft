using tft;

public class SyntaxAnalyzer
{
	private List<Lexeme> _lexemeList;
	private IEnumerator<Lexeme> _lexemeEnumerator;

	public bool Run(string code)
	{
		Lexical analyser = new();
		var result = analyser.Run(string.Join(Environment.NewLine, code));
		if (!result)
		{
			throw new Exception("Errors were occurred in lexical analyze");
		}

		return IsDoWhileStatement(analyser.Lexemes);
	}

	private bool IsDoWhileStatement(List<Lexeme> lexemeList)
	{
		_lexemeList = lexemeList;
		if (lexemeList.Count == 0) return false;

		_lexemeEnumerator = lexemeList.GetEnumerator();

		if (!_lexemeEnumerator.MoveNext() || _lexemeEnumerator.Current.Type != LexemeType.Do) { ErrorType.Error("Ожидается do", _lexemeList.IndexOf(_lexemeEnumerator.Current)); }
		_lexemeEnumerator.MoveNext();

		if (_lexemeEnumerator.Current == null || _lexemeEnumerator.Current.Type != LexemeType.While) { ErrorType.Error("Ожидается while", _lexemeList.IndexOf(_lexemeEnumerator.Current)); }
		_lexemeEnumerator.MoveNext();

		if (!IsCondition()) return false;

		while (IsStatement()) ;

		if (_lexemeEnumerator.Current == null || _lexemeEnumerator.Current.Type != LexemeType.Loop) { ErrorType.Error("Ожидается loop", _lexemeList.IndexOf(_lexemeEnumerator.Current)); }
		_lexemeEnumerator.MoveNext();

		if (_lexemeEnumerator.MoveNext()) { ErrorType.Error("Лишние символы", _lexemeList.IndexOf(_lexemeEnumerator.Current)); }

		return true;
	}

	private bool IsCondition()
	{
		if (!IsLogicalExpression()) return false;
		while (_lexemeEnumerator.Current != null && _lexemeEnumerator.Current.Type == LexemeType.Or)
		{
			_lexemeEnumerator.MoveNext();
			if (!IsLogicalExpression()) return false;
		}
		return true;
	}

	private bool IsLogicalExpression()
	{
		if (!RelationalExpression()) return false;
		while (_lexemeEnumerator.Current != null && _lexemeEnumerator.Current.Type == LexemeType.And)
		{
			_lexemeEnumerator.MoveNext();
			if (!RelationalExpression()) return false;
		}
		return true;
	}

	private bool RelationalExpression()
	{
		if (!IsOperand()) return false;
		if (_lexemeEnumerator.Current != null && _lexemeEnumerator.Current.Type == LexemeType.Relation)
		{
			_lexemeEnumerator.MoveNext();
			if (!IsOperand()) return false;
		}
		return true;
	}

	private bool IsIdentifier()
	{
		if (_lexemeEnumerator.Current == null || _lexemeEnumerator.Current.Class != LexemeClass.Identifier)
		{
			ErrorType.Error("Ожидается переменная", _lexemeList.IndexOf(_lexemeEnumerator.Current));
			return false;
		}
		_lexemeEnumerator.MoveNext();
		return true;
	}

	private bool IsOperand()
	{
		if (_lexemeEnumerator.Current == null || (_lexemeEnumerator.Current.Class != LexemeClass.Identifier && _lexemeEnumerator.Current.Class != LexemeClass.Constant))
		{
			ErrorType.Error("Ожидается переменная или константа", _lexemeList.IndexOf(_lexemeEnumerator.Current));
			return false;
		}
		_lexemeEnumerator.MoveNext();
		return true;
	}

	private bool IsLogicalOperation()
	{
		if (_lexemeEnumerator.Current == null || (_lexemeEnumerator.Current.Type != LexemeType.And && _lexemeEnumerator.Current.Type != LexemeType.Or))
		{
			ErrorType.Error("Ожидается логическая операция", _lexemeList.IndexOf(_lexemeEnumerator.Current));
			return false;
		}
		_lexemeEnumerator.MoveNext();
		return true;
	}

	private bool IsStatement()
	{
		if (_lexemeEnumerator.Current != null && _lexemeEnumerator.Current.Type == LexemeType.Loop) return false;

		if (_lexemeEnumerator.Current == null || _lexemeEnumerator.Current.Class != LexemeClass.Identifier)
		{
			if (_lexemeEnumerator.Current.Type == LexemeType.Output)
			{
				_lexemeEnumerator.MoveNext();
				if (!IsOperand()) return false;
				return true;
			}

			ErrorType.Error("Ожидается переменная", _lexemeList.IndexOf(_lexemeEnumerator.Current));
			return false;
		}
		_lexemeEnumerator.MoveNext();

		if (_lexemeEnumerator.Current == null || _lexemeEnumerator.Current.Type != LexemeType.Assignment)
		{
			ErrorType.Error("Ожидается присваивание", _lexemeList.IndexOf(_lexemeEnumerator.Current));
			return false;
		}
		_lexemeEnumerator.MoveNext();

		if (!IsArithmeticExpression()) return false;

		return true;
	}

	private bool IsArithmeticExpression()
	{
		if (!IsOperand()) return false;
		while (_lexemeEnumerator.Current.Type == LexemeType.ArithmeticOperation)
		{
			_lexemeEnumerator.MoveNext();
			if (!IsOperand()) return false;
		}
		return true;
	}
}