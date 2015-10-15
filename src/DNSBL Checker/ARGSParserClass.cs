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

        /// <summary>
        /// Выдает список аргументов запуска программы
        /// </summary>
        /// <returns>Возвращает список аргументов</returns>
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

        /// <summary>
        /// Проверить доступные аргументы или отфильтровать ихы
        /// </summary>
        /// <param name="Args">Список аргументов</param>
        /// <param name="NumArg">Идентификатор аргумента в списке</param>
        /// <param name="MaxLength">Максимальное количество символов в аргументе</param>
        /// <returns>Отфильтрованный аргумент</returns>
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
