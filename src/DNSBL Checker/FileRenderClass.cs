///
/// Простой класс для работы с файлами
/// 
/// Linxon - http://www.linxon.ru
/// email@linxon.ru
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace DNSBL_Checker
{
    class FileRenderClass
    {

        // Имя текущего файла
        public string filename;

        // Ресурсы
        private FileInfo ifile;
        private DirectoryInfo idir;
        private Stream stream;
        private StreamReader sreader;
        private StreamWriter swriter;
        private FileStream data;

        // Ссылка к списку серверов
        string RemoteBLSAddr = "http://share.linxon.ru/2015/07/bl-servers.txt";

        // Инициализация
        public FileRenderClass(string Filename)
        {
            this.filename = Filename;

            this.ifile = new FileInfo(Filename);
            if (this.stream != null)
            {
                this.data.Close();
                this.stream.Close();
            }
        }

        // Чтобы создать каталог
        public bool CreateFolder(string FolderName)
        {
            if(Directory.Exists(FolderName))
            {

            }
            return true;
        }

        // Метод для создания черного списка, если он не существует
        protected void CreateBLS(string Filename)
        {
            using (StreamWriter stream = this.ifile.CreateText())
            {
                // Записываем список серверов в новый файл
                stream.WriteLine("cbl.abuseat.org");
                stream.WriteLine("bogons.cymru.com");
                stream.WriteLine("dyna.spamrats.com");
                stream.WriteLine("http.dnsbl.sorbs.net");
                stream.WriteLine("dnsbl-3.uceprotect.net"); 
                stream.WriteLine("xbl.spamhaus.org"); 
                stream.WriteLine("zen.spamhaus.org");
                stream.Close();

                this.ifile.Refresh();
            }
        }

        /*
        // Фильтруем список серверов очищая от всякой нечисти (в замен нашлась StringSplitOptions.RemoveEmptyEntries)
        private string[] FilterBLS(string[] Data)
        {
            int c = Data.Length;
            string[] NewData = new string[] { };

            for (int i = 0; i > c; i++)
            {
                if (Data[i].Length > 0)
                {
                    NewData[i] = Data[i];
                    continue;
                }  else break;
            }
            return NewData;
        }
        */

        // Для загрузки черного списка из текстовика
        public string[] LoadBLS()
        {
            if (this.ifile.Exists) {
                using (this.stream = ifile.Open(FileMode.Open, FileAccess.Read))
                {
                    // Чтение файла
                    this.sreader = new StreamReader(this.stream);

                    return this.sreader.ReadToEnd().Split(new string[] {
                        Environment.NewLine
                    }, StringSplitOptions.RemoveEmptyEntries);  // Очищаем пустые строки
                }
            } else {
                this.CreateBLS(this.filename);  // Если файл не существует то применяя рекурсию создаем его и читаем ^_^
                return this.LoadBLS();
            }
        }

        // Записываем результат с формы в текстовик (нужно будет сделать так, чтобы файл перезаписывался)
        public void SaveResult(string Filename, string[] Data, string FolderName, bool Rewrite = false)
        {
            using (this.data = new FileStream(Filename, FileMode.OpenOrCreate))
            {
                data.Close();
                using (this.swriter = new StreamWriter(Filename, Rewrite))
                {
                    foreach (string Line in Data)
                        swriter.WriteLine(Line);
                }

                this.swriter.Close();
                this.ifile.Refresh();
            }
        }

        // Функция проверки хеша одного файла с другом
        private bool CheckHash()
        {
            using (this.data = this.ifile.Open(FileMode.OpenOrCreate, FileAccess.Read))
            {
                MD5 md5hash = new MD5CryptoServiceProvider();

                byte[] RemoteHash = md5hash.ComputeHash(this.stream);
                byte[] LocalHash = md5hash.ComputeHash(this.data);

                for (int i = 0; i < RemoteHash.Length; i++)
                {
                    if (RemoteHash[i] == LocalHash[i])
                        continue;   // Если совпадает - продолжаем
                    else
                        return false;   // Возварщаем false, если хеш не совпадает
                }

                // Очищаем после проверки
                this.stream.Close();
                this.data.Close();

                return true;
            }
        }

        // Для обновления списка серверов
        public int UpdateBLS()
        {
            try
            {
                WebClient WC = new WebClient();

                WC.Headers["user-agent"] = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
                this.stream = WC.OpenRead(RemoteBLSAddr);

                if(CheckHash() == false)
                {
                    WC.DownloadFile(RemoteBLSAddr, this.filename);
                    return 1;   //  Обновление успешно!
                } else
                    return 2;   //  Обновления отсутствуют!

            } catch(System.Net.WebException err)
            {
                MessageBox.Show("[" + err.Status + "] Ошибка во время обновления списка серверов!");
                return -1;  //  Ошибка во время обновления!
            }
        }

        // Кеш
        protected void CreateCache()
        {
            using (StreamWriter stream = this.ifile.CreateText())
            {
                stream.Close();
                this.ifile.Refresh();
            }
        }

        private bool CheckCache(string Cache)
        {
            using (this.sreader = new StreamReader(this.filename))
            {
                string Line;
                int count = 0;

                // Проверяем строки на соответственность
                while ((Line = sreader.ReadLine()) != null)
                {
                    if (Cache != Line && count < 9)
                    {
                        count++;
                        continue;
                    } else
                        return false;
                }
                return true;
            }
        }

        public void UpdateCahce(string Cache)
        {
            if (this.ifile.Exists)
            {
                using (this.data = new FileStream(this.filename, FileMode.OpenOrCreate))
                {
                    this.data.Close();
                    if(this.CheckCache(Cache))
                    {
                        using (this.swriter = new StreamWriter(this.filename, true))
                            swriter.WriteLine(Cache);
                    }
                    
                    this.ifile.Refresh();
                }
            } else {
                CreateCache();
                UpdateCahce(Cache);
            }
        }

        public string[] LoadCahce()
        {
            if (this.ifile.Exists)
            {
                using (this.stream = ifile.Open(FileMode.Open, FileAccess.Read))
                {
                    // Чтение файла
                    this.sreader = new StreamReader(this.stream);

                    return this.sreader.ReadToEnd().Split(new string[] {
                        Environment.NewLine
                    }, StringSplitOptions.RemoveEmptyEntries);  // Очищаем пустые строки
                }
            } else {
                this.CreateCache();
                return this.LoadCahce();
            }
        }
    }
}
