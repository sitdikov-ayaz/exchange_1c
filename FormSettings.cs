using ExchangeOS;
using System;
using System.Data;
using System.Windows.Forms;


namespace ExchangeVS
{
    public partial class FormSettings : Form
    {
        //System.Data.DataTable DT = GlobalVars.Nodes;
        //public delegate void AddLogMethodContainer(string Data);
        //public event AddLogMethodContainer AddLog;
        //public static System.Data.DataTable Nodes = GM.InitalizeNodes();
        GlobalVars.SettingsBaseListClass SettingsBase;

        public FormSettings()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DownloadNodesFrom1CtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
            GM newGM = new GM();
            //newGM.AddLog += AddLogG;//AddLogEvent;


            string TNC;
            TNC = newGM.LoadNodesFromBase(SettingsBase, SettingsBase.NodesLists.Nodes);
            if (TNC != null)
            {
                tabControl1.SelectedIndex = 1;
                ThisNodeCode.Text = TNC;
            }
        }

        //public void AddLogG(string Data)
        //{
        //    AddLog(Data);
        //}
        void SetHeadersDataGridView(DataGridView DataGridOfNodes)
        {
            foreach (DataGridViewColumn DGVC in DataGridOfNodes.Columns)
            {
                if (DGVC.Name == "Code") DGVC.HeaderText = "Код";
                if (DGVC.Name == "Name") DGVC.HeaderText = "Наименовние";
                if (DGVC.Name == "Dir") DGVC.HeaderText = "Каталог обмена";
                if (DGVC.Name == "Checked") DGVC.Visible = false;
                if (DGVC.Name == "Status") DGVC.Visible = false;
                if (DGVC.Name == "StatusFlag") DGVC.Visible = false;
            }

        }

        public void FillControlsFromGlobalVars()
        {
            if (SettingsBase == null) return;

            DataGridOfNodes.DataSource = SettingsBase.NodesLists.Nodes;
            DataGridOfNodes.MultiSelect = false;
            SetHeadersDataGridView(DataGridOfNodes);

            Cluster.Text = SettingsBase.Settings.Cluster;
            Base.Text = SettingsBase.Settings.Base;
            User.Text = SettingsBase.Settings.User;
            Password.Text = SettingsBase.Settings.Password;
            Version1C.Text = SettingsBase.Settings.Version1C;
            TranElCount.Text = SettingsBase.Settings.TranElCount.ToString();
            DirUpd.Text = SettingsBase.Settings.DirUpd;

            StartAutoExchangeAfterStarting.Checked = SettingsBase.Settings.StartAutoExchangeAfterStarting;
            KillUserSessionBeforeAutoExchange.Checked = SettingsBase.Settings.KillUserSessionBeforeAutoExchange;

            ExchangeDir.Text = SettingsBase.Settings.ExchangeDir;
            ExchangeTemporaryDir1C.Text = SettingsBase.Settings.ExchangeTemporaryDir1C;
            UploadingAutoExchange.Checked = SettingsBase.Settings.UploadingAutoExchange;

            ExchangePlan.Text = SettingsBase.Settings.ExchangePlan;
            ThisNodeCode.Text = SettingsBase.Settings.ThisNodeCode;

            TimeExchange.Text = SettingsBase.Settings.TimeExchange;
            TimeKillUsers.Text = SettingsBase.Settings.TimeKillUsers;
            TimeUpLoading.Text = SettingsBase.Settings.TimeUpLoading;
            TimeOut.Text = (SettingsBase.Settings.TimeOut).ToString();
            TimePause.Text = (SettingsBase.Settings.TimePause / 1000).ToString();
            TimePauseDnUp.Text = (SettingsBase.Settings.TimePauseDnUp / 1000).ToString();
            TimePauseStatus.Text = (SettingsBase.Settings.TimePauseStatus / 1000).ToString();
        }

        public void FillGlobalVarsFromControls()
        {
            bool NotOk = true;

            string SettingsName = BaseList.SelectedItem.ToString().Trim();

            //SettingsBase = new GlobalVars.SettingsBaseListClass();
            SettingsBase.SettingsName = SettingsName;
            //SettingsBase.Settings = new GlobalVars.Settings();
            //SettingsBase.NodesLists = new GlobalVars.NodesLists();

            SettingsBase.Settings.Cluster = Cluster.Text;
            SettingsBase.Settings.Base = Base.Text;
            SettingsBase.Settings.User = User.Text;
            SettingsBase.Settings.Password = Password.Text;
            SettingsBase.Settings.Version1C = Version1C.Text;
            SettingsBase.Settings.TranElCount = Convert.ToInt16(TranElCount.Text);
            SettingsBase.Settings.DirUpd = DirUpd.Text;

            SettingsBase.Settings.StartAutoExchangeAfterStarting = StartAutoExchangeAfterStarting.Checked;
            SettingsBase.Settings.KillUserSessionBeforeAutoExchange = KillUserSessionBeforeAutoExchange.Checked;

            SettingsBase.Settings.ExchangeDir = ExchangeDir.Text;
            SettingsBase.Settings.ExchangeTemporaryDir1C = ExchangeTemporaryDir1C.Text;
            SettingsBase.Settings.UploadingAutoExchange = UploadingAutoExchange.Checked;

            SettingsBase.Settings.ExchangePlan = ExchangePlan.Text;
            SettingsBase.Settings.ThisNodeCode = ThisNodeCode.Text;

            SettingsBase.Settings.TimeExchange = TimeExchange.Text;
            SettingsBase.Settings.TimeKillUsers = TimeKillUsers.Text;
            SettingsBase.Settings.TimeUpLoading = TimeUpLoading.Text;

            SettingsBase.Settings.TimeOut = Convert.ToInt16(TimeOut.Text);
            SettingsBase.Settings.TimePause = Convert.ToInt16(TimePause.Text) * 1000;
            SettingsBase.Settings.TimePauseDnUp = Convert.ToInt16(TimePauseDnUp.Text) * 1000;
            SettingsBase.Settings.TimePauseStatus = Convert.ToInt16(TimePauseStatus.Text) * 1000;

            if (GlobalVars.SettingsBaseList == null) GlobalVars.SettingsBaseList = new System.Collections.Generic.List<GlobalVars.SettingsBaseListClass>();

            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
                if (CurrS.SettingsName == SettingsName)
                {
                    CurrS.Settings = SettingsBase.Settings;
                    CurrS.NodesLists = SettingsBase.NodesLists;
                    NotOk = false;
                    break;
                }
            }

            if (NotOk)
            {
                GlobalVars.SettingsBaseListClass NewS = new GlobalVars.SettingsBaseListClass();
                NewS.SettingsName = SettingsBase.SettingsName;
                NewS.Settings = SettingsBase.Settings;
                NewS.NodesLists = SettingsBase.NodesLists;

                GlobalVars.SettingsBaseList.Add(NewS);
            }
            
        }

        System.Data.DataTable InitalizeNodes()
        {
            System.Data.DataTable Nodes = new System.Data.DataTable("data");
            System.Data.DataColumn Column;

            Column = Nodes.Columns.Add();
            Column.ColumnName = "Code";

            Column = Nodes.Columns.Add();
            Column.ColumnName = "Name";

            Column = Nodes.Columns.Add();
            Column.ColumnName = "Dir";

            return Nodes;
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {


            DataGridOfNodes.DataSource = InitalizeNodes();
            DataGridOfNodes.MultiSelect = false;
            SetHeadersDataGridView(DataGridOfNodes);

            if (GlobalVars.SettingsBaseList == null) return;
            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
                BaseList.Items.Add(CurrS.SettingsName);
            }

            BaseList.SelectedIndex = BaseList.Items.Count - 1;

        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
           GlobalVars.VarFormSettings = null;
            if (GlobalVars.SettingsBaseList == null) return;
            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
                GM.SetHeaderDGV(CurrS.DGV);
            }
        }

        void SaveSettings()
        {

            FillGlobalVarsFromControls();
            
            GM.ProgramSaveSettings();
            comboBox1_SelectedIndexChanged(null, null);

            //GM.ProgramReadSettings();
        }

        private void сохранитьНастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void NodeItemDown_Click(object sender, EventArgs e)
        {
            if (DataGridOfNodes.CurrentCell != null)
            {
                int SelectedRow = DataGridOfNodes.CurrentCell.RowIndex;
                System.Data.DataTable dt = (System.Data.DataTable)DataGridOfNodes.DataSource;
                if (SelectedRow < dt.Rows.Count - 1)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Rows[SelectedRow].ItemArray.Length; i++)
                        dr[i] = dt.Rows[SelectedRow][i];
                    dt.Rows.RemoveAt(SelectedRow);
                    dt.Rows.InsertAt(dr, SelectedRow + 1);
                    dt.AcceptChanges();
                }
                if ((SelectedRow + 1) < DataGridOfNodes.RowCount)
                {
                    DataGridViewCell NCCell = DataGridOfNodes.Rows[SelectedRow + 1].Cells[0];
                    DataGridOfNodes.CurrentCell = NCCell;
                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (DataGridOfNodes.CurrentCell != null)
            {
                int index = DataGridOfNodes.CurrentCell.RowIndex;
                DataGridViewRow CurrentRow = DataGridOfNodes.Rows[index];
                DataGridOfNodes.Rows.Remove(DataGridOfNodes.Rows[index]);
            }
        }

        private void NodeItemUp_Click(object sender, EventArgs e)
        {
            if (DataGridOfNodes.CurrentCell != null)
            {
                int SelectedRow = DataGridOfNodes.CurrentCell.RowIndex;
                if (SelectedRow > 0)
                {
                    System.Data.DataTable dt = (System.Data.DataTable)DataGridOfNodes.DataSource;
                    System.Data.DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Rows[SelectedRow].ItemArray.Length; i++)
                        dr[i] = dt.Rows[SelectedRow][i];
                    dt.Rows[SelectedRow].Delete();
                    dt.Rows.InsertAt(dr, SelectedRow - 1);
                    dt.AcceptChanges();
                }
                if ((SelectedRow - 1) > -1)
                {
                    DataGridViewCell NCCell = DataGridOfNodes.Rows[SelectedRow - 1].Cells[0];
                    DataGridOfNodes.CurrentCell = NCCell;
                }
            }
        }

        private void SaveAndCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ICronSchedule _cron_schedule;
            string schedule = scheduleForTest.Text;
            _cron_schedule = new CronSchedule(schedule);
            bool result = _cron_schedule.isTime(DateTime.Now);
            string message;
            if (result) message = "Текст по текущему времени прошел.";
            else message = "Текст по текущему времени не прошел.";
            string caption = "Тест расписания по Cron формату";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SettingsName = BaseList.SelectedItem.ToString();
            this.Text = BaseList.SelectedItem.ToString().Trim();
           // bool NotFound = true;
            if (SettingsName != "")
            {
                if (GlobalVars.SettingsBaseList != null)
                    foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)

                        if (CurrS.SettingsName == SettingsName)
                        {
                            SettingsBase = CurrS;
                            //NotFound = true;
                            break;
                        }
                //if (NotFound)
                //{
                //    SettingsBase = new GlobalVars.SettingsBaseListClass();
                //    SettingsBase.Settings = new GlobalVars.Settings();
                //    SettingsBase.Settings.SettingsName = SettingsName;
                //    SettingsBase.NodesLists.SettingsName = SettingsName;
                //    if (GlobalVars.SettingsBaseList == null)
                //        GlobalVars.SettingsBaseList = new System.Collections.Generic.List<GlobalVars.SettingsBaseListClass>();
                    
                //    GlobalVars.SettingsBaseList.Add(SettingsBase);
                //}
                FillControlsFromGlobalVars();
            }
        }

        private void buttonBaseListItemAdd_Click(object sender, EventArgs e)
        {
            CreateSettings();
        }

        private void buttonBaseListItemAddCopy_Click(object sender, EventArgs e)
        {
            CreateSettings(true);
        }

        private void CreateSettings(bool copied = false)
        {
            InputText InText = new InputText();
            InText.Text = "Введите название настройки.";

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (InText.ShowDialog(this) == DialogResult.OK)
            {
                String SourceSettingsName = "";
                if (BaseList.SelectedItem != null) SourceSettingsName = BaseList.SelectedItem.ToString();
                if (copied) NewSettings(InText.textBox1.Text.Trim(), SourceSettingsName);
                else NewSettings(InText.textBox1.Text.Trim());
            }
            InText.Dispose();
        }


        private void NewSettings(String NewSettingsName, String SourceSettingsName = "")
        {
            GlobalVars.SettingsBaseListClass NewSettingsBase = new GlobalVars.SettingsBaseListClass();
            GlobalVars.Settings NewSettings = new GlobalVars.Settings();
            GlobalVars.NodesLists NewBN = new GlobalVars.NodesLists();

            BaseList.Items.Add(NewSettingsName);

            NewSettingsBase.SettingsName = NewSettingsName;
            NewBN.SettingsName = NewSettingsName;
            NewSettings.SettingsName = NewSettingsName;

            NewBN.Nodes = InitalizeNodes();

            NewSettingsBase.NodesLists = NewBN;
            NewSettingsBase.Settings = NewSettings;



            if (SourceSettingsName != "")
            {
                if (GlobalVars.SettingsBaseList == null) GlobalVars.SettingsBaseList = new System.Collections.Generic.List<GlobalVars.SettingsBaseListClass>();
                foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)

                    if (CurrS.SettingsName == SourceSettingsName)
                    {
                        NewSettingsBase.Settings.AutoExchange = CurrS.Settings.AutoExchange;
                        NewSettingsBase.Settings.Base = CurrS.Settings.Base;
                        NewSettingsBase.Settings.Cluster = CurrS.Settings.Cluster;
                        NewSettingsBase.Settings.DirUpd = CurrS.Settings.DirUpd;
                        NewSettingsBase.Settings.ExchangeDir = CurrS.Settings.ExchangeDir;
                        NewSettingsBase.Settings.ExchangeTemporaryDir1C = CurrS.Settings.ExchangeTemporaryDir1C;
                        NewSettingsBase.Settings.Password = CurrS.Settings.Password;
                        NewSettingsBase.Settings.StartAutoExchangeAfterStarting = CurrS.Settings.StartAutoExchangeAfterStarting;
                        NewSettingsBase.Settings.ThisNodeCode = "";
                        NewSettingsBase.Settings.TimeExchange = CurrS.Settings.TimeExchange;
                        NewSettingsBase.Settings.TimeKillUsers = CurrS.Settings.TimeKillUsers;
                        NewSettingsBase.Settings.TimeOut = CurrS.Settings.TimeOut;
                        NewSettingsBase.Settings.TimePause = CurrS.Settings.TimePause;
                        NewSettingsBase.Settings.TimePauseDnUp = CurrS.Settings.TimePauseDnUp;
                        NewSettingsBase.Settings.TimePauseStatus = CurrS.Settings.TimePauseStatus;
                        NewSettingsBase.Settings.TimeUpLoading = CurrS.Settings.TimeUpLoading;
                        NewSettingsBase.Settings.User = CurrS.Settings.User;
                        NewSettingsBase.Settings.Version1C = CurrS.Settings.Version1C;
                        NewSettingsBase.Settings.TranElCount = CurrS.Settings.TranElCount;
                        break;
                    }
            }
            GlobalVars.SettingsBaseList.Add(NewSettingsBase);
            SettingsBase = NewSettingsBase;
            BaseList.SelectedIndex = BaseList.Items.Count - 1;
            //FillControlsFromGlobalVars();



        }

        private void ExchangePlan_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string SettingsName = BaseList.SelectedItem.ToString().Trim();

            lock (this)
            {
                foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
                {
                    if (CurrS.SettingsName == SettingsName)
                    {
                        GlobalVars.SettingsBaseList.Remove(CurrS);
                        break;
                    }
                }
                BaseList.Items.Remove(SettingsName);
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (DataGridOfNodes.SelectedCells.Count > 0)
            {
                int SelectedRow = DataGridOfNodes.SelectedCells[0].RowIndex;
                System.Data.DataTable DT = (System.Data.DataTable)DataGridOfNodes.DataSource;
                DT.Rows[SelectedRow].Delete();
            }
        }

        private void NodeItemDown_Click_1(object sender, EventArgs e)
        {
            if (DataGridOfNodes.SelectedCells.Count>0) 
            {
                int SelectedRow = DataGridOfNodes.SelectedCells[0].RowIndex;
                DataTable dt = (DataTable)DataGridOfNodes.DataSource;
                if (SelectedRow < dt.Rows.Count - 1)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Rows[SelectedRow].ItemArray.Length; i++)
                        dr[i] = dt.Rows[SelectedRow][i];
                    dt.Rows.RemoveAt(SelectedRow);
                    dt.Rows.InsertAt(dr, SelectedRow + 1);
                    dt.AcceptChanges();
                    DataGridOfNodes.ClearSelection();
                    DataGridOfNodes.Rows[SelectedRow + 1].Cells[0].Selected = true;
                    DataGridOfNodes.CurrentCell = DataGridOfNodes.Rows[SelectedRow + 1].Cells[0];

                }
            }
        }

        private void NodeItemUp_Click_1(object sender, EventArgs e)
        {
            if (DataGridOfNodes.SelectedCells.Count > 0)
            {
                int SelectedRow = DataGridOfNodes.SelectedCells[0].RowIndex;
                if (SelectedRow > 0)
                {
                    DataTable dt = (DataTable)DataGridOfNodes.DataSource;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Rows[SelectedRow].ItemArray.Length; i++)
                        dr[i] = dt.Rows[SelectedRow][i];
                    dt.Rows[SelectedRow].Delete();
                    dt.Rows.InsertAt(dr, SelectedRow - 1);
                    dt.AcceptChanges();

                    DataGridOfNodes.ClearSelection();
                    DataGridOfNodes.Rows[SelectedRow - 1].Cells[0].Selected = true;
                    DataGridOfNodes.CurrentCell = DataGridOfNodes.Rows[SelectedRow - 1].Cells[0];
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ICronSchedule _cron_schedule;
            string schedule = "";
             schedule = scheduleForTest.Text;
            _cron_schedule = new CronSchedule(schedule);

            string message = _cron_schedule.isTime(DateTime.Now).ToString();
            string caption = "Тест";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            // Displays the MessageBox.

            MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DirUpd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

