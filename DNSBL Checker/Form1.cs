﻿using System;
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
        // Файлы для чтения и построения списков
        const string BLFilename = "servers.dat";
        const string RFilename = "result.log";
        const string CFilename = "scan-history.cache";

        // Необходимые поля
		string MFormTitle = "DNSBL Checker: (by Linxon http://www.linxon.ru)";
        string[] BLS;
        string[] ARGS;
        int bads = 0;
        int goods = 0;

        bool StopBtnStat = false;
        bool ScanBtnStat = false;
        bool UpdateLinkStat = false;

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

        Thread BootThread;
        Thread UList;
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

            // Проверяем на доступность 1 агумент с максимальным числом символов 2
            if (AParser.CheckArg(ARGS, 1, 2))
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;

                // Запускаем в отдельном потоке
                this.BootThread = new Thread(() => RunScan(ARGS[1]));
                BootThread.IsBackground = true;
                BootThread.Start();
            }

            this.ResetForm();
        }

        /* Необходимые методы
        -----------------------------------*/
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
                    ListViewItem NewIp = new ListViewItem(Address + " [" + IP.IPAddr.AsString + "]");

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
                    SFileRender.SaveResult(Address + "_" + RFilename, new string[] {
                        Listed + " : " + IP.IPAddr.AsString + " : " + IP.BlackList.VerifiedOnServer
                    }, true);

                    // Отправляем делегату информацию о результатов сканирования
                    Invoke(
                        InfoUpdater, 
                        NewIp,
                        c * 100,
                        i * 100,

                        "Всего: " + Convert.ToString(this.bads + this.goods),
                        "В черном списке: " + Convert.ToString(this.bads),
                        "Нейтральные: " + Convert.ToString(this.goods)
                    );

                    Application.DoEvents();
                } else
                    break;
            }

            // Маркуем конечные строки
            FileRenderClass RFileRender = new FileRenderClass(Address + "_" + RFilename);
            RFileRender.SaveResult(Address + "_" + RFilename, new string[] {
                Environment.NewLine,
                bads + "/" + goods + "/" + Convert.ToString(goods+bads),
                "DONE!",
                Environment.NewLine
            }, true);

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
            UpdateLinkStat = false;
            this.toolStripProgressBar.Visible = false;

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

            // Если пришел аргумент - выполняем и закрываем программу
            if (ARGS.Length > 0)
                Environment.Exit(0);
        }

        private void UpdateServerList()
        {
            FileRenderClass FileRender = new FileRenderClass(BLFilename);

            switch(FileRender.UpdateBLS()) {
                case 1:
                    MessageBox.Show("Список серверов был успешно обновлен!");
                    break;
                case 2:
                    MessageBox.Show("Обновления отсутствуют!");
                    break;
            }

            Invoke(InfoReset);
        }

        /* Обработка событий
        --------------------------------*/
        private void CheckBtn_Click(object sender, EventArgs e)
        {
            if (this.ScanBtnStat == false)
            {
                this.Text = "DNSBL Checker: Проверка...";
                this.ResultBox.Items.Clear();
                this.ScanBtnStat = true;
                this.AddressEdit.Enabled = false;
                UpdateLink.Enabled = false;
                this.StopBtn.Enabled = true;
                this.toolStripProgressBar.Visible = true;
                labelAllCount.Text = "Всего: 0";
                labelBackCount.Text = "В черном списке: 0";
                labelWhiteCount.Text = "Нейтральные: 0";
                goods = 0;
                bads = 0;

                if (this.AddressEdit.Text.Length > 2)
                {
                    string Address = this.AddressEdit.Text.Trim();

                    CFileRender.UpdateCahce(Address);

                    // Запускаем в отдельном потоке
                    this.BootThread = new Thread(() => RunScan(Address));
                    BootThread.IsBackground = true;
                    BootThread.Start();

                } else {
                    MessageBox.Show("Введите IP адрес или домен!");
                    ResetForm();
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (UpdateLinkStat == true)
                UList.Abort();
            Close();
            Application.Exit(); // Чао!
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            this.StopBtnStat = true;
            this.ScanBtnStat = false;
            UpdateLink.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Text = "DNSBL Checker: Обновление списка серверов...";
            this.ScanBtnStat = true;
            this.AddressEdit.Enabled = false;
            UpdateLink.Enabled = false;
            UpdateLinkStat = true;

            this.UList = new Thread(UpdateServerList);
            UList.Start();
        }
    }
}