///
/// Простой класс для работы с аргументами запуска программы
/// 
/// Linxon - http://www.linxon.ru
/// email@linxon.ru
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSBL_Checker
{
    class ARGSParserClass
    {
        public int count = -1;
        public string argument = null;

        // Для подгрузки аргументов
        public string[] GetArgs()
        {
            string[] ARGS = Environment.GetCommandLineArgs();
            int c = ARGS.Length;
            if (c > 1)
            {
                string[] Data = ARGS;
                for (int i = 0; i < c; i++)
                {
                    Data = ARGS;
                } return Data;
            } else return new string[] { };
        }

        // Для проверки аргумента (string[] Args - список аргументов, int Num - Номер проверяемого на доступность аргумента)
        public bool CheckArg(string[] Args, int NumArg = 1, int MaxLength = 1)
        {
            int c = Args.Length;
            if (c > NumArg && Args[NumArg].Length > MaxLength)
                return true;
            else
                return false;
        }
    }
}
