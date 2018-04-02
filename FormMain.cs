using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Serialization;
//using System.Runtime.InteropServices;


namespace ExchangeVS
{
    public partial class FormMain : Form
    {
        private delegate void TheadCloseDelegate(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName);
        private delegate void EditDGVDelegate(int row, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase);
        private delegate void EditTabPageTextDelegate(GlobalVars.SettingsBaseListClass SettingsBase);
        private delegate void AddLogDelegate(RichTextBox RTB, string Data);

        //AutoExchangeThread MTAutoExchangeThread;
        UpLoadThread UpLoadThreadMT;
        DownLoadThread DownLoadThreadMT;

        //public static class ScrollAPIs
        //{
        //    [DllImport("user32.dll")]
        //    internal static extern int GetScrollPos(IntPtr hWnd, int nBar);

        //    [DllImport("user32.dll")]
        //    internal static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        //    [DllImport("user32.dll")]
        //    internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        //    public enum ScrollbarDirection
        //    {
        //        Horizontal = 0,
        //        Vertical = 1,
        //    }

        //    private enum Messages
        //    {
        //        WM_HSCROLL = 0x0114,
        //        WM_VSCROLL = 0x0115
        //    }

        //    public static int GetScrollPosition(IntPtr hWnd, ScrollbarDirection direction)
        //    {
        //        return GetScrollPos(hWnd, (int)direction);
        //    }

        //    public static void GetScrollPosition(IntPtr hWnd, out int horizontalPosition, out int verticalPosition)
        //    {
        //        horizontalPosition = GetScrollPos(hWnd, (int)ScrollbarDirection.Horizontal);
        //        verticalPosition = GetScrollPos(hWnd, (int)ScrollbarDirection.Vertical);
        //    }

        //    public static void SetScrollPosition(IntPtr hwnd, int hozizontalPosition, int verticalPosition)
        //    {
        //        SetScrollPosition(hwnd, ScrollbarDirection.Vertical, verticalPosition);
        //        SetScrollPosition(hwnd, ScrollbarDirection.Horizontal, hozizontalPosition);
        //    }

        //    public static void SetScrollPosition(IntPtr hwnd, ScrollbarDirection direction, int position)
        //    {
        //        //move the scroll bar
        //        SetScrollPos(hwnd, (int)direction, position, true);

        //        //convert the position to the windows message equivalent
        //        IntPtr msgPosition = new IntPtr((position << 16) + 4);
        //        Messages msg = (direction == ScrollbarDirection.Horizontal) ? Messages.WM_HSCROLL : Messages.WM_VSCROLL;
        //        SendMessage(hwnd, (int)msg, msgPosition, IntPtr.Zero);
        //    }
        //}

        void LoadFormSize()
        {
            //return;
            string file = GlobalVars.ExeDir + @"MainFormSize.txt";

            if (File.Exists(file))
            {
                Size nSize;
                using (Stream stream = new FileStream(file, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Size));
                    nSize = new Size();
                    nSize = (Size)serializer.Deserialize(stream);
                    this.Size = nSize;
                }
            }
        }

        void SaveFormSize()
        {
            string file = GlobalVars.ExeDir + @"MainFormSize.txt";

            using (Stream writer = new FileStream(file, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Size));
                serializer.Serialize(writer, this.Size);
            }

        }


        public FormMain()
        {
            InitializeComponent();

            //StatusThr = null;

        }

        private void ExitStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVars.VarFormSettings == null)
            {
                GlobalVars.VarFormSettings = new FormSettings();
            }
            GlobalVars.VarFormSettings.Show();
            GlobalVars.VarFormSettings.Focus();

        }

        private void DataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name == "StatusFlag")
            {
                string dir = dgv.Rows[dgv.CurrentCell.RowIndex].Cells["Dir"].Value.ToString();
                if (dir == "") dir = GlobalVars.CurrentSettingsBase.Settings.ExchangeDir;
                Process.Start("explorer", @dir);
                //AddLog(dgv.Rows[dgv.CurrentCell.RowIndex].Cells["Checked"].Value.ToString());
            }
        }
        private void DataGrid_DefaultValuesNeeded(object sender,
            System.Windows.Forms.DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Checked"].Value = true;
        }

        private void DataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgv.EndEdit();
            }
        }

        public enum LightStatus
        {
            //Unknown,
            TurnedOn,
            TurnedOff
        }

        //private void DataGridV_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (DataGridV.Columns[DataGridV.CurrentCell.ColumnIndex].Name == "StatusFlag")
        //    {
        //        string dir = DataGridV.Rows[DataGridV.CurrentCell.RowIndex].Cells["Dir"].Value.ToString();
        //        if (dir == "") dir = GlobalVars.ExchangeDir;
        //        System.Diagnostics.Process.Start("explorer", @dir);
        //    }
        //}


        private void SetupDataGridView(TabPage NewTab, GlobalVars.SettingsBaseListClass CurrB)
        {

            DataGridView DGV = new DataGridView();
            DGV.Name = NewTab + "_DGV";
            DGV.Dock = System.Windows.Forms.DockStyle.Top;
            DGV.Location = new System.Drawing.Point(3, 3);
            DGV.Size = new System.Drawing.Size(499, 343);
            DGV.TabIndex = 0;
            DGV.RowTemplate.Height = 18;

            Splitter Spl = new Splitter();
            Spl.Name = NewTab + "_Spl";
            Spl.BackColor = System.Drawing.SystemColors.MenuHighlight;
            Spl.Dock = System.Windows.Forms.DockStyle.Top;
            Spl.Location = new System.Drawing.Point(3, 346);
            Spl.Size = new System.Drawing.Size(499, 12);
            Spl.TabIndex = 1;
            Spl.TabStop = false;

            RichTextBox RTB = new RichTextBox();
            RTB.Name = NewTab + "_RTB";

            RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            RTB.Location = new System.Drawing.Point(3, 358);
            RTB.Size = new System.Drawing.Size(499, 29);
            RTB.TabIndex = 2;
            RTB.Text = "";

            NewTab.Controls.Add(RTB);
            NewTab.Controls.Add(Spl);
            NewTab.Controls.Add(DGV);

            NewTab.Location = new System.Drawing.Point(4, 22);
            NewTab.Padding = new System.Windows.Forms.Padding(3);
            NewTab.Size = new System.Drawing.Size(505, 532);
            NewTab.TabIndex = 0;
            NewTab.UseVisualStyleBackColor = true;

            CurrB.DGV = DGV;
            CurrB.RTB = RTB;

            DGV.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(DataGrid_MouseDoubleClick);
            DGV.CurrentCellDirtyStateChanged += new System.EventHandler(DataGrid_CurrentCellDirtyStateChanged);
            DGV.ShowEditingIcon = false;
            DGV.AllowUserToAddRows = false;

            GlobalVars.SettingsBaseListClass CurrSet = GM.GetSettings(CurrB.SettingsName);
            if (CurrSet.SettingsName == "") return ;

            if (CurrSet.NodesLists != null)
                if (CurrSet.NodesLists.Nodes != null)
                    lock (this)
                    {

                        DGV.Columns.Clear();
                        DGV.DataSource = CurrSet.NodesLists.Nodes;

                        DGV.MultiSelect = false;
                        GM.SetHeaderDGV(DGV);

                    }
        }

        void CreateTabSheets(GlobalVars.SettingsBaseListClass CurrB)
        {

            string SettingsName = CurrB.SettingsName;

            foreach (TabPage CurTab in tabControl1.TabPages)
            {
                if (CurTab.Text == SettingsName) return;
            }
            //"tab"+tabControl1.TabPages.Count.ToString();;
            string tabName = "tab" + tabControl1.TabPages.Count.ToString();
            TabPage NewTab = new TabPage(tabName);
            NewTab.Text = SettingsName;
            tabControl1.TabPages.Add(NewTab);
            CurrB.TabPage = NewTab;

            SetupDataGridView(NewTab, CurrB);

            CurrB.StatusThr = new StatusThread();
            CurrB.StatusThr.SettingsBase = CurrB;
            CurrB.StatusThr.EditDGV += EditDGVEvent;
            //CurrB.StatusThr.EditTabPageText += EditTabPageTextEvent;
            CurrB.StatusThr.Start();


        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            LoadFormSize();
            lock (this)
            {
                tabControl1.TabPages.Clear();

                foreach (GlobalVars.SettingsBaseListClass CurrB in GlobalVars.SettingsBaseList)
                {
                    CreateTabSheets(CurrB);
                    foreach (DataGridViewColumn CurrColumn in CurrB.DGV.Columns)
                    {
                        if (CurrColumn.Name == "Checked")
                            CurrColumn.ReadOnly = false;
                        else
                            CurrColumn.ReadOnly = true;
                    }
                    CheckedOnOff(CurrB, true);
                    if (CurrB.Settings.StartAutoExchangeAfterStarting)
                    {
                        AutoExchangeThreadStart(CurrB);
                    }
                }

                tabControl1.SelectedIndex = 0;
                tabControl1_SelectedIndexChanged(null, null);
                SetTabText();
            }

        }

        private void CheckedOnOff(GlobalVars.SettingsBaseListClass CurrentSettingsBase,  bool checkedMode = true)
        {
            lock (this)
            {
                if (CurrentSettingsBase.DGV != null)
                    foreach (DataGridViewRow element in CurrentSettingsBase.DGV.Rows)
                    {
                        element.Cells["Checked"].Value = checkedMode;
                    }
            }

        }

        private void TagsOnStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckedOnOff(GlobalVars.CurrentSettingsBase, true);
        }

        private void TagsOffStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckedOnOff(GlobalVars.CurrentSettingsBase, false);
        }

        private void CloseAllSessionsStripMenuItem_Click(object sender, EventArgs e)
        {
            //VisibleMenuItem(true);
            //// GM.KillUserSession();
            //GM newGM = new GM();
            //newGM.AddLog += AddLogEvent;
            //newGM.KillSessions(GlobalVars.CurrentSettingsBase);
            GM.KillUserSession(GlobalVars.CurrentSettingsBase, false, false);

        }

        void AutoExchangeThreadStart(GlobalVars.SettingsBaseListClass SettingsBase)
        {
            SettingsBase.AutoThr = new AutoExchangeThread();
            SettingsBase.AutoThr.ThreadName = "AUTO";
            SettingsBase.AutoThr.ClosingThread += ClosingThreadDelegate;
            SettingsBase.AutoThr.SettingsBase = SettingsBase;
            SettingsBase.AutoThr.AddLog += AddLogEvent;
            SettingsBase.AutoThr.EditDGV += EditDGVEvent;
            SettingsBase.AutoThr.EditTabPageText += EditTabPageTextEvent;
            SettingsBase.AutoThr.Start();
        }

        void ClosingThreadDelegate(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName)
        {
            this.BeginInvoke(new TheadCloseDelegate(ClosingThread), new object[] { SettingsBase, Mode ,ThreadName});
        }

        void AddLogEvent(RichTextBox RTB, string Data)
        {
            this.BeginInvoke(new AddLogDelegate(AddLog), new object[] { RTB, Data });
        }

        void EditDGV(int row, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase)
        {
            //GlobalVars.CurrentSettingsBase.DGV
            try
            {
                if (row > -1)
                {
                    string CompoundState = "";
                    string Status = "";
                    string StatusFlag = "";
                    if (DGV.Rows[row].Cells[column].Value == null)
                    {
                        lock (this) DGV.Rows[row].Cells[column].Value = Data;
                    }
                    else
                    {
                        if (!(DGV.Rows[row].Cells[column].Value.ToString().Trim() == Data)) lock (this) DGV.Rows[row].Cells[column].Value = Data;
                    }
                    if (DGV.Rows[row].Cells["StatusFlag"].Value != null) StatusFlag = DGV.Rows[row].Cells["StatusFlag"].Value.ToString().Trim();
                    if (DGV.Rows[row].Cells["Status"].Value != null) Status = DGV.Rows[row].Cells["Status"].Value.ToString().Trim();
                    if ((Status != "")&&(StatusFlag != ""))
                        CompoundState = Status + " - " + StatusFlag;
                    else
                        CompoundState = Status != "" ? Status : "" + StatusFlag != "" ? StatusFlag : "";

                    //TabPage.Text
                    try
                    {
                        if (Status =="Загрузка" || Status == "Выгрузка")
                        {
                            string TabPageText;
                            if (SettingsBase.Settings.AutoExchange) TabPageText = SettingsBase.SettingsName + " Автообмен";
                            else TabPageText = SettingsBase.SettingsName;
                            TabPageText = TabPageText + " - " + Status;
                            if (SettingsBase.TabPage.Text.Trim() != TabPageText)
                                SettingsBase.TabPage.Text = TabPageText;
                        }
                    }
                    catch (Exception) { }
                    //TabPage.Text

                    if (DGV.Rows[row].Cells["CompoundState"].Value != null)
                        if (DGV.Rows[row].Cells["CompoundState"].Value.ToString().Trim() != CompoundState) lock (this) DGV.Rows[row].Cells["CompoundState"].Value = CompoundState;

                }
            }
            catch (ArgumentException){ }
            catch (Exception){ }
        }

        void EditTabPageText(GlobalVars.SettingsBaseListClass SettingsBase)
        {
            try
            {
            string Data;
            if (SettingsBase.Settings.AutoExchange) Data = SettingsBase.SettingsName + " Автообмен";
            else Data = SettingsBase.SettingsName;

                if (SettingsBase.TabPage.Text.Trim() != Data.Trim())
                    SettingsBase.TabPage.Text = Data;
            }
            catch (Exception) { }
        }


        void EditDGVEvent(int row, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase)
        {
            this.BeginInvoke(new EditDGVDelegate(EditDGV), new object[] { row, column, Data, DGV, SettingsBase });
        }

        void EditTabPageTextEvent(GlobalVars.SettingsBaseListClass SettingsBase)
        {
            this.BeginInvoke(new EditTabPageTextDelegate(EditTabPageText), new object[] { SettingsBase });
        }

        string ToUTF8Win1251(string str)
        {
            //System.Text.Encoding.UTF32
            Encoding encBef = Encoding.UTF32;//.GetEncoding("UTF-8");//Encoding.Unicode;
            Encoding encAft = Encoding.GetEncoding("UTF-8");//.GetEncoding("UTF-8");//Encoding.Unicode;
            byte[] byteBef = encBef.GetBytes(str);
            byte[] byteAft = Encoding.Convert(encBef, encAft, byteBef);
            return encAft.GetString(byteAft);
            
            //Encoding utf8 = Encoding.GetEncoding("UTF-16");
            //Encoding windows1251 = Encoding.GetEncoding("UTF-8");
            ////Encoding windows1251 = Encoding.GetEncoding("Windows-1251");

            //byte[] utf8Byte = utf8.GetBytes(str);
            //byte[] windows125Byte = Encoding.Convert(utf8, windows1251, utf8Byte);
            //str = windows1251.GetString(windows125Byte);
            //return str;
        }

        void AddLog(RichTextBox RTB, string Data)
        {
            string sData;

            sData = DateTime.Now.ToString() + " : " + Data;
            if (RTB.Lines.Count() > 600) RTB.Text = "";

            RTB.AppendText(sData);
            //this.richTextBox1.AppendText(ToUTF8Win1251(sData));
            RTB.AppendText(Environment.NewLine);

            string logdir, fname;

            logdir = @Application.StartupPath.ToString() + "\\log\\";
            fname = logdir + DateTime.Now.ToString("MM_dd_yyyy") + ".txt";

            try
            {

                if (!Directory.Exists(logdir))
                {
                    DirectoryInfo di = Directory.CreateDirectory(logdir);
                }

                if (File.Exists(fname))
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname, true))
                    {
                        file.WriteLine(sData);
                    }
                else
                    System.IO.File.WriteAllText(fname, sData);
            }
            catch (Exception) { }

        }

        void VisibleMenuItem(bool Mode)
        {
            if (FlagFileStripMenuItem.Enabled != Mode) FlagFileStripMenuItem.Enabled = Mode;
            //if (SettingsStripMenuItem.Enabled != Mode) SettingsStripMenuItem.Enabled = Mode;
            if (CloseAllSessionsStripMenuItem.Enabled != Mode) CloseAllSessionsStripMenuItem.Enabled = Mode;
            if (DownloadStripMenuItem.Enabled != Mode) DownloadStripMenuItem.Enabled = Mode;
            if (DownloadAndAutoExchangeOnStripMenuItem.Enabled != Mode) DownloadAndAutoExchangeOnStripMenuItem.Enabled = Mode;
            if (UploadStripMenuItem.Enabled != Mode) UploadStripMenuItem.Enabled = Mode;
            if (UploadAndAutoExchangeOnStripMenuItem.Enabled != Mode) UploadAndAutoExchangeOnStripMenuItem.Enabled = Mode;
        }

        void UnsubscribeThreadEvents(string VarName)
        {
            if (VarName == "UpLoadThreadMT")
            {
                UpLoadThreadMT.ClosingThread -= ClosingThreadDelegate;
                UpLoadThreadMT.AddLog -= AddLogEvent;
                UpLoadThreadMT.EditDGV -= EditDGVEvent;
                UpLoadThreadMT.EditTabPageText -= EditTabPageTextEvent;
            }

        }

        void ClosingThread(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName)
        {
            SettingsBase.Settings.Exchange_launched = false;
            if (ThreadName == "AUTO")
            {
                SettingsBase.AutoThr.ClosingThread -= ClosingThreadDelegate;
                SettingsBase.AutoThr.AddLog -= AddLogEvent;
                SettingsBase.AutoThr.EditDGV -= EditDGVEvent;
                SettingsBase.AutoThr.EditTabPageText -= EditTabPageTextEvent;
            }
            else
            UnsubscribeThreadEvents(ThreadName);

            EditTabPageText(SettingsBase);


            switch (Mode)
            {
                case 1:
                    break;// загрузка
                case 11:// после загрузки надо включить автообмен
                    SettingsBase.Settings.Exchange_launched = true;
                    SettingsBase.Settings.AutoExchange = true;
                    AutoExchangeThreadStart(SettingsBase);

                    break;
                case 2: 
                    break;// выгрузка
                case 22:// после выгрузки надо включить автообмен
                    SettingsBase.Settings.Exchange_launched = true;
                    SettingsBase.Settings.AutoExchange = true;
                    AutoExchangeThreadStart(SettingsBase);
                    break;
                case 0:
                default:
                    break;
            }
            SetTabText();


        }

        private void DataGridV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGrid_CurrentCellDirtyStateChanged(sender, e);
        }

        

        private void DataGridV_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataGrid_MouseDoubleClick(sender, e);
        }

        private void DownloadStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleMenuItem(false);
            if (GlobalVars.CurrentSettingsBase.Settings.Exchange_launched) return;

            DownLoadThreadMT = new DownLoadThread();
            DownLoadThreadMT.ThreadName = "DownLoadThreadMT"; 
            DownLoadThreadMT.SettingsBase = GlobalVars.CurrentSettingsBase; 
            DownLoadThreadMT.Mode = 1;
            DownLoadThreadMT.ClosingThread += ClosingThreadDelegate;
            DownLoadThreadMT.AddLog += AddLogEvent;
            DownLoadThreadMT.EditDGV += EditDGVEvent;
            DownLoadThreadMT.EditTabPageText += EditTabPageTextEvent;
            DownLoadThreadMT.Start();
            GM.V8Null(GlobalVars.CurrentSettingsBase);
        }

        private void DownloadAndAutoExchangeOnStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleMenuItem(false);
            if (GlobalVars.CurrentSettingsBase.Settings.Exchange_launched) return;

            DownLoadThreadMT = new DownLoadThread();
            DownLoadThreadMT.ThreadName = "DownLoadThreadMT";
            DownLoadThreadMT.SettingsBase = GlobalVars.CurrentSettingsBase; 
            DownLoadThreadMT.Mode = 11;
            DownLoadThreadMT.ClosingThread += ClosingThreadDelegate;
            DownLoadThreadMT.AddLog += AddLogEvent;
            DownLoadThreadMT.EditDGV += EditDGVEvent;
            DownLoadThreadMT.EditTabPageText += EditTabPageTextEvent;
            DownLoadThreadMT.Start();

        }

        private void UploadStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleMenuItem(false);
            if (GlobalVars.CurrentSettingsBase.Settings.Exchange_launched) return;

            UpLoadThreadMT = new UpLoadThread();
            UpLoadThreadMT.ThreadName = "UpLoadThreadMT";
            UpLoadThreadMT.SettingsBase = GlobalVars.CurrentSettingsBase; ;
            UpLoadThreadMT.Mode = 2;
            UpLoadThreadMT.ClosingThread += ClosingThreadDelegate;
            UpLoadThreadMT.AddLog += AddLogEvent;
            UpLoadThreadMT.EditDGV += EditDGVEvent;
            UpLoadThreadMT.EditTabPageText += EditTabPageTextEvent;
            UpLoadThreadMT.Start();
            GM.V8Null(GlobalVars.CurrentSettingsBase);
        }

        private void UploadAndAutoExchangeOnStripMenuItem_Click(object sender, EventArgs e)
        {
            VisibleMenuItem(false);
            if (GlobalVars.CurrentSettingsBase.Settings.Exchange_launched) return;

            UpLoadThreadMT = new UpLoadThread();
            UpLoadThreadMT.ThreadName = "UpLoadThreadMT";
            UpLoadThreadMT.SettingsBase = GlobalVars.CurrentSettingsBase;
            UpLoadThreadMT.Mode = 22;
            UpLoadThreadMT.ClosingThread += ClosingThreadDelegate;
            UpLoadThreadMT.AddLog += AddLogEvent;
            UpLoadThreadMT.EditDGV += EditDGVEvent;
            UpLoadThreadMT.EditTabPageText += EditTabPageTextEvent;
            UpLoadThreadMT.Start();
        }

        private void AutoExchangeButton_Click(object sender, EventArgs e)
        {

            if (GlobalVars.CurrentSettingsBase == null) return;

            lock (this)
            {

                GlobalVars.CurrentSettingsBase.Settings.AutoExchange = !GlobalVars.CurrentSettingsBase.Settings.AutoExchange;
                if (GlobalVars.CurrentSettingsBase.Settings.AutoExchange)
                {
                    AutoExchangeThreadStart(GlobalVars.CurrentSettingsBase);
                }
            }

            SetTabText();
        }

        private void timerAutoExchangeStart_Tick(object sender, EventArgs e)
        {
            timerAutoExchangeStart.Enabled = false;

            //lock (this)
            //foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            //    {
            //        CheckedOnOff(CurrS, true);
            //        if (CurrS.Settings.StartAutoExchangeAfterStarting)
            //        {
            //            AutoExchangeThreadStart(CurrS);
            //        }
            //    }
            //SetTabText();

        }

        private void deleteFlag(DataGridViewRow Row)
        {
            string Code, ExchangeDir, NodeName, FileFlagDn, FileFlagUp;
            try
            {
                if (Row.Cells["Code"].Value == null) return;
                if (!Convert.ToBoolean(Row.Cells["Checked"].Value)) return;



                Code = Row.Cells["Code"].Value.ToString().Trim();
                if (Code == "") return;

                ExchangeDir = Row.Cells["Dir"].Value.ToString().Trim();

                if (ExchangeDir == "") ExchangeDir = GlobalVars.CurrentSettingsBase.Settings.ExchangeDir;

                if (!Directory.Exists(ExchangeDir))
                {
                    AddLog(GlobalVars.CurrentSettingsBase.RTB, "Каталог недоступен. " + ExchangeDir);
                    return;
                }

                NodeName = Code + " " + Row.Cells["Name"].Value.ToString();

                FileFlagDn = Code + "-" + GlobalVars.CurrentSettingsBase.Settings.ThisNodeCode + ".txt";
                FileFlagUp = GlobalVars.CurrentSettingsBase.Settings.ThisNodeCode + "-" + Code + ".txt";

                if (File.Exists(ExchangeDir + FileFlagUp))
                    File.Delete(ExchangeDir + FileFlagUp);
                {
                }
            }
            catch (Exception) { }

        }

        private void FlagFileDeleteAllNodesStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow Row in GlobalVars.CurrentSettingsBase.DGV.Rows)
            {
                deleteFlag(Row);
            }
        }

        private void FlagFileDeleteCurrentNodeStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteFlag(GlobalVars.CurrentSettingsBase.DGV.CurrentRow);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormSize();

            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
                try
                {
                    if (CurrS.AutoThr.thread.IsAlive)
                    {
                        CurrS.AutoThr.thread.Abort();
                    }
                }
                catch (Exception) { }

                try
                {
                    if (CurrS.StatusThr.thread.IsAlive)
                    {
                        CurrS.StatusThr.thread.Abort();
                    }
                }
                catch (Exception) { }

                GM.V8Null(CurrS);

            }
            //try
            //{
            //    GM.V8Null(GlobalVars.CurrentSettingsBase, true);
            //}
            //catch (Exception){ }
        }

        private void SetTabText()
        {
            if (GlobalVars.SettingsBaseList == null) return;

            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
                if (tabControl1.SelectedTab != CurrS.TabPage) continue;

                GlobalVars.CurrentSettingsBase = CurrS;

                if (CurrS.Settings.AutoExchange)
                {
                    try
                    {
                        if (AutoExchangeButton.Image != Exchange.Properties.Resources.g1)
                            AutoExchangeButton.Image = Exchange.Properties.Resources.g1;
                    }
                    catch (Exception) { }

                    VisibleMenuItem(false);
                    if (!AutoExchangeButton.Checked)
                        AutoExchangeButton.Checked = true;
                }
                else
                {
                    try
                    {
                        if (AutoExchangeButton.Image != Exchange.Properties.Resources.g2)
                            AutoExchangeButton.Image = Exchange.Properties.Resources.g2;
                    }
                    catch (Exception) { }
                    VisibleMenuItem(true);
                    if (AutoExchangeButton.Checked)
                        AutoExchangeButton.Checked = false;
                }
            }
        }
        //private void SetTabText()
        //{
        //    if (GlobalVars.SettingsBaseList == null) return;

        //    string SettingsName;

        //    foreach (TabPage CurrTab in tabControl1.TabPages)
        //    {
        //        SettingsName = CurrTab.Text.Replace(" - Автообмен", "");
        //        foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
        //        {
        //            if (CurrS.SettingsName != SettingsName) continue;

        //            if (tabControl1.SelectedTab == CurrTab)

        //                if (CurrS.Settings.AutoExchange)
        //                {
        //                    try
        //                    {
        //                        AutoExchangeButton.Image = Exchange.Properties.Resources.g1;
        //                    }
        //                    catch (Exception) { }

        //                    VisibleMenuItem(false);
        //                    AutoExchangeButton.Checked = true;
        //                    }
        //                else
        //                {
        //                    try
        //                    {
        //                        AutoExchangeButton.Image = Exchange.Properties.Resources.g2;
        //                    }
        //                    catch (Exception) { }
        //                    VisibleMenuItem(true);
        //                    AutoExchangeButton.Checked = false;
        //                }


        //            if (CurrS.Settings.AutoExchange)
        //            {
        //                CurrTab.Text = CurrTab.Text.Replace(" - Автообмен", "") + " - Автообмен";
        //            }
        //            else
        //            {
        //                CurrTab.Text = CurrTab.Text.Replace(" - Автообмен", "");
        //            }
        //        }
        //    }
        //}
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null) return;
            GlobalVars.CurrentSettingsBase = GM.GetSettings(tabControl1.SelectedTab.Text.Replace(" - Автообмен", ""));
            //GlobalVars.CurrentSettingsBase = GM.GetSettings(tabControl1.SelectedTab.Text.Replace(" - Автообмен", ""));
            this.Text = GlobalVars.CurrentSettingsBase.SettingsName;
            SetTabText();
        }


        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GM.CheckUpdate(true);
        }
    }
}
