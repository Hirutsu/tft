using System.Text;

namespace tft
{
    public class Avtomat
    {
        private Dictionary<string, Dictionary<string, string>> _avtomat;
        private string _start;
        private string _temp;
        private string _end;
        public Avtomat()
        {
            _avtomat = new Dictionary<string, Dictionary<string, string>>();
        }

        //конструктор добавления из файла
        public Avtomat(string filePath)
        {
            _avtomat = new Dictionary<string, Dictionary<string, string>>();
            using (StreamReader file = new StreamReader(@filePath))
            {
                string[] startAndEnd = file.ReadLine().Split();
                _start = startAndEnd[0];
                _end = startAndEnd[1];

                string tempStr;
                string[] nodeStr;
                string[] nodesStr;
                while ((tempStr = file.ReadLine()) != null)
                {
                    nodesStr = tempStr.Split();
                    Dictionary<string, string> tempNextNodes = new Dictionary<string, string>();
                    for (int i = 1; i < nodesStr.Length; i++)
                    {
                        nodeStr = nodesStr[i].Split(':');
                        tempNextNodes.Add(nodeStr[0],nodeStr[1]);
                    }
                    _avtomat.Add(nodesStr[0], tempNextNodes);
                }
            }
        }

        public void ShowLstVertex()
        {
            Console.WriteLine("Start: {0}", _start);
            Console.WriteLine("End: {0}", _end);
            Console.WriteLine("-------------------");
            foreach (var keyValue in _avtomat)
            {
                Console.Write(keyValue.Key + "->");
                foreach (var keyValue2 in keyValue.Value)
                {
                   Console.Write(keyValue2.Key + ":" + keyValue2.Value + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Passage(string sequence)
        {
            _temp = _start;

            for (int i = 0; i < sequence.Length; i++)
            {
                try
                {
                    _temp = _avtomat[_temp].First(x => x.Value == sequence[i].ToString()).Key;
                }
                catch(Exception)
                {
                    Console.WriteLine("The automaton did not completed at the end point, not found");
                    return;
                }
            }
            if (_temp == _end)
            {
                Console.WriteLine("The automaton completed at the end point");
            }
            else
            {
                Console.WriteLine("The automaton did not completed at the end point");
            }
        }

        public void OutPutInFile(string str)
        {
            string oneNode;
            using (StreamWriter FileOut = new StreamWriter(str, false, Encoding.GetEncoding(1251)))
            {
                foreach (var keyValue in _avtomat)
                {
                    oneNode = keyValue.Key + " ";
                    foreach (var keyValue2 in keyValue.Value)
                    {
                        oneNode += (keyValue2.Key + ":" + keyValue2.Value + " ");
                    }
                    FileOut.WriteLine(oneNode);
                }
            }
        }
    }
}
