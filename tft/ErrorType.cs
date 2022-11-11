public class ErrorType
{
    public static void Error(string message, int position)
    {
        throw new Exception($"{message} в позиции: {position}");
    }
}