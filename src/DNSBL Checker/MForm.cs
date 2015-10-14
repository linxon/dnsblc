using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNSBL_Checker
{
    public partial class MForm : Form
    {
        // Имя файлов, для чтения и построения списков
        const string BLFilename = "servers.dat";
        const string CFilename = "scan-history.cache";
        const string RFilename = "result.log";

        // Путь к лог-результатам
        string RFileFolder = "logs/";

        // Необходимые поля
        string MFormTitle = "DNSBL Checker: (by Linxon http://www.linxon.ru)";
        string[] BLS, ARGS;
        int bads, goods = 0;

        bool StopBtnStat, ScanBtnStat, UpdateLinkStat = false;

        // Делегаты для обновления информации на экране
        public delegate void HelpToUpdate(
            ListViewItem NewIp, 
            int PGMax, 
            int PGValue, 
            string labelAll, 
            string labelBack, 
            string labelWhite
        );
        public delegate void HelpToReset();

        // Необходимо для доступа извне метода
        HelpToUpdate InfoUpdater;
        HelpToReset InfoReset;

        Thread BootThread, UList;
        FileRenderClass CFileRender;

        public object CheckBtn { get; private set; }

        // Конструктор главной формы программы
        public MForm()
        {
            InitializeComponent();

            this.InfoUpdater = new HelpToUpdate(UpdateForm);
            this.InfoReset = new HelpToReset(ResetForm);

            ARGSParserClass AParser = new ARGSParserClass();
            ARGS = AParser.GetArgs();  // Получаем список аргументов

            // Проверяем на доступность 1-го агумента с минимальным числом символов 2 (учитывается домен с 3-х значными символами)
            if (AParser.CheckArg(ARGS, 1, 2))
            {
                if(AParser.CheckArg(ARGS, 2, 3))
                {
                    this.RFileFolder = ARGS[2];
                }

                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;

                // Запускаем в отдельном потоке
                this.BootThread = new Thread(() => RunScan(ARGS[1]));
                BootThread.IsBackground = true;
                BootThread.Start();

            } else
                this.ResetForm();   // Устанавливаем умолчания
        }

        #region Необходимые методы для обработки

        private void RunScan(string Address)
        {
            FileRenderClass FileRender = new FileRenderClass(BLFilename);
            this.BLS = FileRender.LoadBLS();    //  Загружаем список серверов
            
            int c = this.BLS.Length;
            for (int i = 0; i < c; i++)
            {
                VerifyAddressClass IP = new VerifyAddressClass(Address, this.BLS[i].Trim());

                if (IP.isInit != false && IP.IPAddr.Valid && this.StopBtnStat == false)
                {
                    ListViewItem NewIp = new ListViewItem(IP.IPAddr.AsString);

                    // Формируем готовый список проверенных данных
                    string Listed;
                    if (IP.BlackList.IsListed) {
                        Listed = "Warning";
                        NewIp.ForeColor = Color.Red;
                        NewIp.SubItems.Add(Listed);
                        NewIp.SubItems.Add(IP.BlackList.VerifiedOnServer);
                        this.bads++;
                    } else {
                        Listed = "OK";
                        NewIp.ForeColor = Color.Green;
                        NewIp.SubItems.Add(Listed);
                        NewIp.SubItems.Add(IP.BlackList.VerifiedOnServer);
                        this.goods++;
                    }

                    FileRenderClass SFileRender = new FileRenderClass(Address + "_" + RFilename);
                    SFileRender.SaveResult(RFileFolder, new string[] {
                        IP.IPAddr.AsString + " - " + Listed + " - " + IP.BlackList.VerifiedOnServer
                    }, true);

                    // Отправляем делегату информацию о результатов сканирования
                    if(InvokeRequired)
                    {
                        Invoke(
                            InfoUpdater,

                            NewIp,
                            c * 100,
                            i * 100,

                            "Всего: " + Convert.ToString(this.bads + this.goods),
                            "В черном списке: " + Convert.ToString(this.bads),
                            "Нейтральные: " + Convert.ToString(this.goods)
                        );
                    }

                    Application.DoEvents();
                } else
                    break;
            }

            // Маркуем конечные строки
            FileRenderClass RFileRender = new FileRenderClass(Address + "_" + RFilename);
            RFileRender.SaveResult(RFileFolder, new string[] {
                Environment.NewLine,
                bads + "/" + goods + "/" + Convert.ToString(goods+bads),
                "DONE!",
            }, true);

            if(InvokeRequired)
                Invoke(InfoReset);
        }

        // Для обновления информации на главной форме
        private void UpdateForm(ListViewItem NewIp, int PGMax, int PGValue, string labelAll, string labelBack, string labelWhite)
        {
            ResultBox.Items.Add(NewIp);
            ResultBox.Refresh();

            toolStripProgressBar.Maximum = PGMax;
            toolStripProgressBar.Value = PGValue;

            labelAllCount.Text = labelAll;
            labelBackCount.Text = labelBack;
            labelWhiteCount.Text = labelWhite;

            // Автоскролл вниз формы
            ResultBox.EnsureVisible(ResultBox.Items.Count -1);
        }

        // Для установки умолчаний и очистки информации на главной форме
        private void ResetForm()
        {
            this.Text = MFormTitle;
            this.toolStripProgressBar.Value = 0;
            this.StopBtnStat = false;
            this.StopBtn.Enabled = false;
            this.AddressEdit.Enabled = true;
            this.ScanBtnStat = false;
            UpdateLink.Enabled = true;
            this.ScanBtn.Enabled = true;
            UpdateLinkStat = false;
            UpdateLink.Enabled = true;
            this.toolStripProgressBar.Visible = false;

            // Обнуляем счетчики
            goods = 0;
            bads = 0;

            // Автозагрузка данных сканирования в форму поиска
            if (ScanBtnStat == false)
            {
                this.CFileRender = new FileRenderClass(CFilename);
                string[] SearchCache = CFileRender.LoadCahce();
                AddressEdit.Items.Clear();

                if (SearchCache.Length > 0)
                {
                    foreach (string dataCh in SearchCache)
                        AddressEdit.Items.Add(dataCh);
                }
            }

            if (ResultBox.Items.Count > 0)
                this.SaveAsToolStripMenuItem.Enabled = true;

            // Если пришел аргумент - выполняем и закрываем программу
            if (ARGS.Length > 1)
                Environment.Exit(0);
        }

        // Вывод сообщений о статусе обновлений списка серверов
        private void UpdateServerList()
        {
            FileRenderClass FileRender = new FileRenderClass(BLFilename);

            switch(FileRender.UpdateBLS()) {
                case -1:
                    // Прочие ошибки
                    break;
                case 1:
                    MessageBox.Show("Список серверов был успешно обновлен!");
                    break;
                case 2:
                    MessageBox.Show("Обновления отсутствуют!");
                    break;
            }

            if(InvokeRequired)
                Invoke(InfoReset);
        }

        // Действие выхода приложения...
        private void CloseWindowNow()
        {
            if (UpdateLinkStat == true)
                UList.Abort();

            Close();
            Application.Exit(); // Чао!
        }

        // Действие запуска сканирования
        private void runThread()
        {
            if (this.ScanBtnStat == false)
            {
                this.Text = "DNSBL Checker: Проверка...";
                this.ResultBox.Items.Clear();
                this.ScanBtnStat = true;
                this.AddressEdit.Enabled = false;
                UpdateLink.Enabled = false;
                this.StopBtn.Enabled = true;
                this.ScanBtn.Enabled = false;
                this.toolStripProgressBar.Visible = true;

                if (this.AddressEdit.Text.Length > 2)
                {
                    string Address = this.AddressEdit.Text.Trim();

                    CFileRender.UpdateCahce(Address);

                    // Запускаем в отдельном потоке
                    this.BootThread = new Thread(() => RunScan(Address));
                    BootThread.IsBackground = true;
                    BootThread.Start();
                }
                else
                {
                    MessageBox.Show("Введите IP адрес или домен!");
                    ResetForm();
                }
            }
        }

        #endregion

        #region Обработка событий

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            this.runThread();
        }

        private void AddressEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
                this.runThread();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.CloseWindowNow();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            this.StopBtnStat = true;
            this.ScanBtnStat = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Text = "DNSBL Checker: Обновление списка серверов...";
            this.ScanBtnStat = true;
            this.AddressEdit.Enabled = false;
            UpdateLink.Enabled = false;
            UpdateLinkStat = true;
            this.ScanBtn.Enabled = false;

            this.UList = new Thread(UpdateServerList);
            UList.Start();
        }

        /* Контекстное меню
        -----------------------------*/
        private void ClearRBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ResultBox.Items.Count > 0)
            {
                this.SaveAsToolStripMenuItem.Enabled = false;
                this.ResultBox.Items.Clear();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CloseWindowNow();
        }

        // Чуть поже переделаю
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная операция недоступна!");

            /*
            saveFileDialog.Filter = "Log file|*.log";
            saveFileDialog.FileName = RFilename;
            saveFileDialog.ShowDialog();

            FileRenderClass FileRender = new FileRenderClass(saveFileDialog.FileName);
            MessageBox.Show(saveFileDialog.FileName);
            //FileRender.SaveResult(saveFileDialog.FileName, "", );
            */
        }

        #endregion
    }
}
