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

        /// <summary>
        /// Создать файл серверов, если он отсутствует
        /// </summary>
        /// <param name="Filename">Имя файла</param>
        protected void CreateBLS(string Filename)
        {
            try
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
            } catch (IOException err)
            {
                MessageBox.Show(err.Message);
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

        /// <summary>
        /// Загрузить список серверов для проверки
        /// </summary>
        /// <returns>Возвращает данные из файла</returns>
        public string[] LoadBLS()
        {
            try
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
                    this.CreateBLS(this.filename);  // Если файл не существует то применяя рекурсию создаем его и читаем ^_^
                    return this.LoadBLS();
                }
            } catch (IOException err)
            {
                MessageBox.Show(err.Message);
                return new string[] { };
            }
        }

        /// <summary>
        /// Сохранить результат в указанном каталоге
        /// </summary>
        /// <param name="FolderName">Имя каталога</param>
        /// <param name="Data">Данные для записи</param>
        /// <param name="Rewrite">Режим перезаписи файла</param>
        public void SaveResult(string FolderName, string[] Data, bool Rewrite = false)
        {
            try
            {
                if (Directory.Exists(FolderName))
                {
                    using (this.data = new FileStream(FolderName + "/" + this.filename, FileMode.OpenOrCreate))
                    {
                        data.Close();
                        using (this.swriter = new StreamWriter(FolderName + "/" + this.filename, Rewrite))
                        {
                            foreach (string Line in Data)
                                swriter.WriteLine(Line);
                        }

                        this.swriter.Close();
                        this.ifile.Refresh();
                    }
                } else {
                    // Создаем папку
                    Directory.CreateDirectory(FolderName);
                    this.SaveResult(FolderName, Data, Rewrite);
                }
            } catch (IOException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /*
        // Модифицированная копия функции выше, необходима для сохранения файла в отдельный каталог (может быть сделаю все в одну универсальную функцию)
        public void SaveResultAs(string Filename, string[] Data, string FolderName, bool Rewrite = false)
        {

        }
        */

        /// <summary>
        /// Для сравнения хеша списка серверов с удаленным списком
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Обновить список серверов
        /// </summary>
        /// <returns></returns>
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
                    WC.Dispose();
                    return 1;   //  Обновление успешно!
                } else {
                    WC.Dispose();
                    return 2;   //  Обновления отсутствуют!
                }
            } catch (System.Net.WebException err)
            {
                MessageBox.Show("[" + err.Status + "] Ошибка во время обновления списка серверов!");
                return -1;  //  Ошибка во время обновления!
            }
        }

        /// <summary>
        /// Создать кеш файл
        /// </summary>
        protected void CreateCache()
        {
            using (StreamWriter stream = this.ifile.CreateText())
            {
                stream.Close();
                this.ifile.Refresh();
            }
        }

        /// <summary>
        /// Проверить кеш файл
        /// </summary>
        /// <param name="Cache">Имя файла</param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновить кеш файл
        /// </summary>
        /// <param name="Cache">Имя файла</param>
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

        /// <summary>
        /// Загрузить кеш файл
        /// </summary>
        /// <returns>Возвращает данные из файла</returns>
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
