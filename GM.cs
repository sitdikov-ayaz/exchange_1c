//789

using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Data;
using System.Diagnostics;

namespace ExchangeVS
{

    public class GlobalVars
    {
        public class Settings
        {

            public string SettingsName = "";
            //public string Name = "";
            public string Cluster = "  ";
            public string Base = "";
            public string User = "";
            public string Password = "";
            public string Version1C = "3";
            public int TranElCount = 0;
            public string DirUpd = @"";
            public string ExchangePlan = "";
            public string ThisNodeCode = "";
            public string ExchangeDir = @"";
            public int TimeOut = 99999;
            public int TimePause = 2;
            public int TimePauseDnUp = 3;
            public int TimePauseStatus = 2;
            public string ExchangeTemporaryDir1C = Application.StartupPath.ToString()+@"\temp\";
            public string TimeKillUsers = "99";
            public string TimeUpLoading = "99";
            public string TimeExchange = "* *";
            public bool StartAutoExchangeAfterStarting = false;
            public bool KillUserSessionBeforeAutoExchange = false;
            public bool AutoExchange = false;
            public bool Exchange_launched = false;
            public bool UploadingAutoExchange = false;

        }
        public class NodesLists
        {
            public string SettingsName = "";
            public System.Data.DataTable Nodes;
        }

        public class SettingsBaseListClass
        {
            public StatusThread StatusThr;
            public AutoExchangeThread AutoThr;
            public dynamic v8;
            //public V82.COMConnector v8Connector;
            public dynamic v8Connector;

            public TabPage TabPage;
            public string SettingsName = "";
            public DataGridView DGV;
            public RichTextBox RTB;
            public Settings Settings;
            public NodesLists NodesLists;
        }


        public static SettingsBaseListClass CurrentSettingsBase;

        public static List<SettingsBaseListClass> SettingsBaseList;

        public static string Dir1c = "";
        public const string SettingsFile = "settings.xml";


        public static Form VarFormSettings;
        public static string CheckingUpdate = "";

        public const string UpdateDir = @"\\192.168.0.13\Visitings\Exchange\install\";
        //public static string ExeName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToUpper() + ".EXE";
        public static string ExeName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string ExeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\";

    }

    public class StatusThread
    {
        public Thread thread;
        public delegate void EditDGVMethodContainer(int index, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase);
        public delegate void EditTabPageTextMethodContainer(GlobalVars.SettingsBaseListClass SettingsBase);
        //public event EditTabPageTextMethodContainer EditTabPageText;
        public event EditDGVMethodContainer EditDGV;
        public GlobalVars.SettingsBaseListClass SettingsBase = null;
        bool terminateStatus = false;

        public StatusThread()
        {
        }

        public void Start()
        {
            thread = new Thread(this.func);
            thread.IsBackground = true;
            thread.Start();
        }

        void func(object Val)
        {
            string FileFlag, FileFlagR, StatusFlag, FileFlagRZ, FileFlagZ, ExchangeDir;
            while (true)
            {
                if (terminateStatus) return;
                //try
                //{
                //    if (SettingsBase != null)
                //        if (SettingsBase.Settings != null)
                //            if (SettingsBase.TabPage != null)
                //                    EditTabPageText(SettingsBase);
                //}
                //catch (Exception) { }


                if (SettingsBase == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (SettingsBase.DGV == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                try
                {

                    foreach (DataGridViewRow Row in SettingsBase.DGV.Rows)
                    {
                        if (terminateStatus) return;


                        if (Row.Cells["Code"].Value == null) continue;
                        if (Row.Cells["Code"].Value.ToString().Trim() == "") continue;
                        ExchangeDir = Row.Cells["Dir"].Value.ToString().Trim();
                        if (ExchangeDir == "") ExchangeDir = SettingsBase.Settings.ExchangeDir;

                        FileFlag = ExchangeDir + SettingsBase.Settings.ThisNodeCode + "-" + Row.Cells["Code"].Value.ToString() + ".txt";
                        FileFlagZ = FileFlag.Replace(".txt", ".zip");

                        FileFlagR = ExchangeDir + Row.Cells["Code"].Value.ToString() + "-" + SettingsBase.Settings.ThisNodeCode + ".txt";
                        FileFlagRZ = FileFlagR.Replace(".txt", ".zip");

                        StatusFlag = "";
                        if (File.Exists(FileFlag))
                            if (File.Exists(FileFlagZ)) StatusFlag = "Выгружено"; else StatusFlag = "Загрузка в приемнике";
                        if (File.Exists(FileFlagR))
                        {
                            if (StatusFlag != "") StatusFlag = "куча мала";
                            else StatusFlag = "Можно загружать";
                        }
                        //EditDGV(Row.Index, "Status", StatusFlag, SettingsBase.DGV); //Row.Cells["StatusFlag"].Value = StatusFlag;
                        EditDGV(Row.Index, "StatusFlag", StatusFlag, SettingsBase.DGV, SettingsBase);
                    }

                }
                catch (Exception)
                { }
                if (SettingsBase != null) Thread.Sleep(SettingsBase.Settings.TimePauseStatus);
            }
        }

        public void Stop()
        {
            terminateStatus = true;
        }

    }


    class DownLoadThread
    {
        public delegate void EditTabPageTextMethodContainer(GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditTabPageTextMethodContainer EditTabPageText;
        public delegate void MethodContainer(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName);
        public event MethodContainer ClosingThread;
        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;
        public delegate void EditDGVMethodContainer(int index, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditDGVMethodContainer EditDGV;
        public int Mode;
        public string ThreadName;
        public GlobalVars.SettingsBaseListClass SettingsBase;

        Thread thread;
        bool thisthread = false;

        public DownLoadThread()
        {
        }

        public void Start()
        {
            try
            {
                SettingsBase.Settings.Exchange_launched = true;
                thisthread = true;
                thread = new Thread(this.DownLoad);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (InvalidCastException e)
            {
                AddLog(SettingsBase.RTB, e.Message.ToString());
            }
            catch (Exception e)
            {
                AddLog(SettingsBase.RTB, e.Message.ToString());
            }
            
           
            SettingsBase.Settings.Exchange_launched = false;
        }

        void DownLoad()
        {
            string FileZipDn, FileFlagDn, FileXMLDn, FileFlagUp, ExchangeDir, Code, NodeName;
            try
            {
                EditTabPageText(SettingsBase);
            }
            catch (Exception) { }
            try
            {
                foreach (DataGridViewRow Row in SettingsBase.DGV.Rows)
                {
                    try
                    {
                        if (Row.Cells["Code"].Value == null) continue;
                        if (!Convert.ToBoolean(Row.Cells["Checked"].Value)) continue;

                        Code = Row.Cells["Code"].Value.ToString().Trim();

                        if (Code == "") continue;

                        NodeName = Row.Cells["Code"].Value.ToString() + " - " + Row.Cells["Name"].Value.ToString();

                        ExchangeDir = Row.Cells["Dir"].Value.ToString().Trim();
                        if (ExchangeDir == "") ExchangeDir = SettingsBase.Settings.ExchangeDir;
                        if (!Directory.Exists(ExchangeDir))
                        {
                            AddLog(SettingsBase.RTB, "Каталог недоступен. Узел: " + NodeName + ", Каталог: " + ExchangeDir);
                            continue;
                        }

                        FileFlagDn = Code + "-" + SettingsBase.Settings.ThisNodeCode + ".txt";
                        FileFlagUp = SettingsBase.Settings.ThisNodeCode + "-" + Code + ".txt";

                        if (File.Exists(ExchangeDir + FileFlagDn))
                        {

                            EditDGV(Row.Index, "Status", "Загрузка", SettingsBase.DGV, SettingsBase); //Row.Cells["Status"].Value = "Загрузка";
                            //EditDGV(Row.Index, "StatusFlag", "", SettingsBase.DGV); //Row.Cells["StatusFlag"].Value = "";
                            
                            AddLog(SettingsBase.RTB, "Загрузка узла " + NodeName);

                            FileZipDn = FileFlagDn.Replace(".txt", ".zip");
                            FileXMLDn = FileFlagDn.Replace(".txt", ".xml");
                            
                            GM newGM = new GM();
                            newGM.AddLog += AddLogG;//AddLogEvent;

                            GM.VoidResult ReadMessageResult = newGM.ReadMessage(SettingsBase, FileXMLDn, FileZipDn, FileFlagDn, ExchangeDir, Code);//, "DownLoadThread", this);
                            

                            if (ReadMessageResult.Code == 0)
                            {
                                AddLog(SettingsBase.RTB, "Загрузка узла завершено.. Узел:" + Code + "; " + ExchangeDir + FileZipDn);
                                File.Delete(ExchangeDir + FileFlagDn);
                            }
                            else
                            {
                                AddLog(SettingsBase.RTB, "Ошибка при загрузке узла. " + ReadMessageResult.Message + " Узел:" + NodeName);
                                //GM.V8Null(SettingsBase);
                                (new GM()).ConnectTo1c(SettingsBase);
                            }
                            Thread.Sleep(SettingsBase.Settings.TimePauseDnUp);
                            //AddLog(SettingsBase.RTB, SettingsBase.Settings.TimePauseDnUp.ToString());
                            newGM.AddLog -= AddLogG;
                        }

                        EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase); //Row.Cells["Status"].Value = "";
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
            if (thisthread) ClosingThread(SettingsBase, Mode, ThreadName);
            try
            {
                EditTabPageText(SettingsBase);
            }
            catch (Exception) { }
            
        }

        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(RTB, Data);
        }

    }

    class UpLoadThread
    {
        public delegate void EditTabPageTextMethodContainer(GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditTabPageTextMethodContainer EditTabPageText;
        public delegate void MethodContainer(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName);
        public event MethodContainer ClosingThread;
        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;
        public delegate void EditDGVMethodContainer(int index, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditDGVMethodContainer EditDGV;
        public int Mode;
        public string ThreadName;
        public GlobalVars.SettingsBaseListClass SettingsBase;

        Thread thread;
        bool thisthread = false;

        public UpLoadThread()
        {
        }

        public void Start()
        {
            try
            {
                SettingsBase.Settings.Exchange_launched = true;
                thisthread = true;
                thread = new Thread(this.UpLoad);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (InvalidCastException e)
            {
                AddLog(SettingsBase.RTB, e.Message.ToString());
            }
            catch (Exception e)
            {
                AddLog(SettingsBase.RTB, e.Message.ToString());
            }

           SettingsBase.Settings.Exchange_launched = false;
           
        }

        public void UpLoad()
        {
            string FileZip, FileFlag, FileXML, FileFlagReverse, ExchangeDir, Code, NodeName;
            GM.VoidResult WriteMessageResult;
            try
            {
                EditTabPageText(SettingsBase);
            }
            catch (Exception) { }
            try
            {
                foreach (DataGridViewRow Row in SettingsBase.DGV.Rows)
                {
                    if (Mode == 3) if (!SettingsBase.Settings.AutoExchange) break;
                    try
                    {
                        if (Row.Cells["Code"].Value == null) continue;
                        if (!Convert.ToBoolean(Row.Cells["Checked"].Value)) continue;

                        Code = Row.Cells["Code"].Value.ToString().Trim();

                        if (Code == "") continue;

                        NodeName = Code + " - " + Row.Cells["Name"].Value.ToString();

                        ExchangeDir = Row.Cells["Dir"].Value.ToString().Trim();
                        if (ExchangeDir == "") ExchangeDir = SettingsBase.Settings.ExchangeDir;
                        if (!Directory.Exists(ExchangeDir))
                        {
                            //GM.AddActionStatus(SettingsBase, SettingsBase.SettingsName + " " + "Каталог недоступен. Узел: " + NodeName + ", Каталог: " + ExchangeDir);
                            AddLog(SettingsBase.RTB, "Каталог недоступен. Узел: " + NodeName + ", Каталог: " + ExchangeDir);
                            continue;
                        }

                        FileFlag = SettingsBase.Settings.ThisNodeCode + "-" + Code + ".txt";
                        FileFlagReverse = Code + "-" + SettingsBase.Settings.ThisNodeCode + ".txt";

                        if (File.Exists(ExchangeDir + FileFlag) || File.Exists(ExchangeDir + FileFlagReverse)) continue;

                        FileZip = SettingsBase.Settings.ThisNodeCode + "-" + Code + ".zip";
                        FileXML = SettingsBase.Settings.ThisNodeCode + "-" + Code + ".xml";

                        //GM.AddActionStatus(SettingsBase, SettingsBase.SettingsName + " " + "Выгрузка узла " + NodeName);
                        AddLog(SettingsBase.RTB, "Выгрузка узла " + NodeName);
                        EditDGV(Row.Index, "Status", "Выгрузка", SettingsBase.DGV, SettingsBase); //Row.Cells["Status"].Value = "Выгрузка";
                        Thread.Sleep(1000);
                        GM newGM = new GM();
                        newGM.AddLog += AddLogG;//AddLogEvent;

                        WriteMessageResult = newGM.WriteMessage(SettingsBase, FileXML, FileZip, FileFlag, ExchangeDir, Code);
                        //WriteMessageResult = GM.WriteMessage(FileXML, FileZip, FileFlag, ExchangeDir, Code, "UpLoadThread", this);

                        if (WriteMessageResult.Code == 0)
                        {
                            //GM.AddActionStatus(SettingsBase, SettingsBase.SettingsName + " " + "Выгрузка узла завершено. Размер файла: " + WriteMessageResult.Message + " Узел:" + NodeName + "; " + ExchangeDir + FileZip);
                            AddLog(SettingsBase.RTB, "Выгрузка узла завершено. Размер файла: " + WriteMessageResult.Message + " Узел:" + NodeName + "; " + ExchangeDir + FileZip);
                        }
                        else
                        {
                            AddLog(SettingsBase.RTB, "Ошибка при выгрузке узла 1. " + WriteMessageResult.Message + " Узел:" + NodeName);
                            //GM.V8Null(SettingsBase);
                            (new GM()).ConnectTo1c(SettingsBase);
                        }

                        EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase);// Row.Cells["Status"].Value = "";
                        newGM.AddLog -= AddLogG;//AddLogEvent;
                   }
                    catch (Exception) { }
                    //catch (InvalidCastException e) { AddLog(e.Message.ToString()); }
                }        
            }
            catch (Exception) { }
            if (thisthread) ClosingThread(SettingsBase, Mode, ThreadName);
            try
            {
                EditTabPageText(SettingsBase);
            }
            catch (Exception) { }
        }

        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(RTB, Data);
        }

    }

    class v8WriteMessageThread
    {
        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;

        public string FileZip, FileXML, ExchangeDir, FileFlag, Code;
        //public DataGridViewRow Row;
        //public object thread;
        public GM.VoidResult ThisResult = new GM.VoidResult();
        public bool aborting;
        public GlobalVars.SettingsBaseListClass SettingsBase = null;


        public v8WriteMessageThread()
        {
        }

        public GM.MsgErr v8WriteMessage()
        {
            GM.MsgErr MsgErr = new GM.MsgErr();
            
            MsgErr.Err = "";
            try
            {
                if (SettingsBase.Settings.TranElCount == 0 )
                    MsgErr.ReturnMessage = SettingsBase.v8.ПроцедурыОбменаДаннымиДоп.ЗаписатьСообщение(
                                                                SettingsBase.Settings.ExchangePlan,
                                                                Code,
                                                                SettingsBase.Settings.ExchangeTemporaryDir1C,
                                                                FileXML,   //xml
                                                                @"1");
                else
                MsgErr.ReturnMessage = SettingsBase.v8.ПроцедурыОбменаДаннымиДоп.ЗаписатьСообщение(
                                                            SettingsBase.Settings.ExchangePlan,
                                                            Code,
                                                            SettingsBase.Settings.ExchangeTemporaryDir1C,
                                                            FileXML,   //xml
                                                            @"1",
                                                            SettingsBase.Settings.TranElCount);
            }
            catch (AccessViolationException E)
            {
                MsgErr.ReturnMessage = null;
                MsgErr.Err = E.Message;
            }
            catch (Exception E)
            {
                MsgErr.ReturnMessage = null;
                MsgErr.Err = E.Message;
            }

            return MsgErr;

        }

        public void Start()
        {
            try
            {
                aborting = false;
                Thread threadt = new Thread(this.WriteMessage);

                threadt.IsBackground = true;
                threadt.Start();
                threadt.Join(SettingsBase.Settings.TimeOut * 60 * 1000);

                if (threadt.IsAlive)
                {
                    threadt.Abort();
                    AddLog(SettingsBase.RTB, "Превышен лимит выполнения операции.");
                    //GM.V8Null(SettingsBase);
                    (new GM()).ConnectTo1c(SettingsBase);
                }
            }
            catch (System.AccessViolationException E)
            {
                AddLog(SettingsBase.RTB, E.Message);
            }



        }

        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(RTB, Data);
        }

        public void WriteMessage(object Val)
        {
            ThisResult.Code = 99;

            GM newGM = new GM();
            newGM.AddLog += AddLogG;//AddLogEvent;

            if (!newGM.ConnectTo1c(SettingsBase)) return;

            GM.MsgErr MsgErr = v8WriteMessage();

            newGM.AddLog -= AddLogG;

            if (MsgErr.ReturnMessage != null)
            {
                try
                {
                    if (MsgErr.ReturnMessage.Результат > 0)
                    {
                        ThisResult.Code = (int)MsgErr.ReturnMessage.Результат;
                        ThisResult.Message = (string)MsgErr.ReturnMessage.Инфо;

                        return;
                    }
                }
                catch (Exception)
                {
                    ThisResult.Code = 99;
                    ThisResult.Message = "Произошла непредвиденная ошибка при получении результата выгрузки";

                    return;
                }

            }
            else
            {
                if (MsgErr.Err != "")
                {
                    ThisResult.Code = 99;
                    ThisResult.Message = MsgErr.Err;
                }
                else
                {
                    ThisResult.Code = 99;
                    ThisResult.Message = "Произошла непредвиденная ошибка";
                }
                return;
            }

            if (!File.Exists(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip))
            {
                ThisResult.Code = 10;
                ThisResult.Message = "Ошибка? файл выгрузился и исчез :( " + SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip;
                return;
            }

            if (File.Exists(ExchangeDir + FileZip))
            {
                try
                {
                    File.Delete(ExchangeDir + FileZip);
                }
                catch (Exception) { }
                if (File.Exists(ExchangeDir + FileZip))
                {
                    ThisResult.Code = 10;
                    ThisResult.Message = "Ошибка при удалении файла1: " + ExchangeDir + FileZip;
                    return;
                }

            }

            FileInfo FI = new FileInfo(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip);
            string ZipSize = GM.GetFileSizeNormal(FI.Length);
            FI = null;

            try
            {
                File.Copy(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip, ExchangeDir + FileZip);
            }
            catch (Exception) { }

            if (!File.Exists(ExchangeDir + FileZip))
            {
                ThisResult.Code = 10;
                ThisResult.Message = "Ошибка при копировании файла из " + SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip + " в " + ExchangeDir + FileZip;
                return;
            }

            string filePath = @ExchangeDir + @FileFlag;
            string filePathZIP = @ExchangeDir + FileZip;

            try
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception) { }
                }

                try
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(ZipSize);
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                    }
                }
                catch (Exception) { }

                ThisResult.Code = 0;
                ThisResult.Message = ZipSize;
            }

            catch (Exception E)
            {
                ThisResult.Code = 10;
                ThisResult.Message = "Ошибка при оздании флаг файла. " + filePath + "\n" + E.Message;
                return;
            }

            if (!File.Exists(filePathZIP) || !File.Exists(filePath) )
            {
                ThisResult.Code = 10;
                ThisResult.Message = "Ошибка при создании файла выгрузки";
                return;
            }

            return;

        }

    }

    class v8ReadMessageThread
    {
        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;

        public string FileZip, FileXML, ExchangeDir, FileFlag, Code;
        //public object thread;
        public GM.VoidResult ThisResult = new GM.VoidResult();
        public GlobalVars.SettingsBaseListClass SettingsBase = null;

        public v8ReadMessageThread()
        {

        }

        public GM.MsgErr v8ReadMessage()
        {
            GM.MsgErr MsgErr = new GM.MsgErr();
            MsgErr.ReturnMessage = null;
            MsgErr.Err = "";

            try
            {
                if (SettingsBase.Settings.TranElCount == 0)
                    MsgErr.ReturnMessage = SettingsBase.v8.ПроцедурыОбменаДаннымиДоп.ПрочитатьСообщение(
                                            SettingsBase.Settings.ExchangeTemporaryDir1C,
                                            FileZip);
                else
                    MsgErr.ReturnMessage = SettingsBase.v8.ПроцедурыОбменаДаннымиДоп.ПрочитатьСообщение(
                                            SettingsBase.Settings.ExchangeTemporaryDir1C,
                                            FileZip,
                                            SettingsBase.Settings.TranElCount);

            }
            catch (Exception E)
            {
                MsgErr.Err = E.Message;
            }

            return MsgErr;
        }

        public void Start()
        {
            try
            {
                Thread threadt = new Thread(this.ReadMessage);
                threadt.IsBackground = true;
                threadt.Start();
                threadt.Join(SettingsBase.Settings.TimeOut * 60 * 1000);
                if (threadt.IsAlive)
                {
                    threadt.Abort();
                    AddLog(SettingsBase.RTB, "Превышен лимит выполнения операции.");
                    //GM.V8Null(SettingsBase);
                    (new GM()).ConnectTo1c(SettingsBase);
                }
            }
            catch (System.AccessViolationException E)
            {
                AddLog(SettingsBase.RTB, E.Message);
            }
        }

        public void ReadMessage(object Val)
        {
            ThisResult.Code = 99;
            ThisResult.NotLoaded = true;
            string NotError;
            NotError = "";
            try
            {
                string line = "";
                var encoding = Encoding.GetEncoding("UTF-8");//Encoding.GetEncoding(1251);
                using (var src = new StreamReader(ExchangeDir + FileFlag, encoding: encoding))
                {
                    while ((line = src.ReadLine()) != null) AddLog(SettingsBase.RTB, line);
                }
            }
            catch (Exception e){ NotError = e.Message; }

            if (!(NotError=="")) { ThisResult.Message = "Файл недоступен " + NotError +" "+ ExchangeDir + FileFlag; return; }

            // копирнем из каталога обмена в каталог 1с
            if (File.Exists(ExchangeDir + FileZip))
            {
                if (File.Exists(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip))
                    try
                    {
                        File.Delete(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip);
                    }
                    catch (Exception) { }
                if (File.Exists(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip))
                {
                    ThisResult.Code = 10;
                    ThisResult.Message = "Ошибка при удалении файла2: " + SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip;
                    return;
                }

                try
                {
                    File.Copy(ExchangeDir + FileZip, SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip);
                }
                catch (Exception) { }

                if (File.Exists(SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip))
                {
                    try
                    {
                        File.Delete(ExchangeDir + FileZip);
                    }
                    catch (Exception)
                    {
                        ThisResult.Code = 10;
                        ThisResult.Message = "Ошибка при удалении файла3: " + ExchangeDir + FileZip;
                        return;
                    }
                }
                else
                {
                    ThisResult.Code = 10;
                    ThisResult.Message = "Ошибка копирования файла из " + ExchangeDir + FileZip + " в " + SettingsBase.Settings.ExchangeTemporaryDir1C + FileZip;
                    return;
                }

                if (File.Exists(ExchangeDir + FileZip))
                {
                    ThisResult.Code = 10;
                    ThisResult.Message = "Ошибка при удалении файла4: " + ExchangeDir + FileZip;
                    return;
                }
            }

            GM newGM = new GM();
            newGM.AddLog += AddLogG;//AddLogEvent;

            if (!newGM.ConnectTo1c(SettingsBase)) return;//threadName, thread)) return;

            newGM.AddLog -= AddLogG;//AddLogEvent;

            GM.MsgErr MsgErr = v8ReadMessage();

            if (MsgErr.Err != "")
            {
                if (MsgErr.ReturnMessage != null)
                {
                    ThisResult.Code = (int)MsgErr.ReturnMessage.Результат;
                    ThisResult.Message = (string)MsgErr.ReturnMessage.Инфо;
                }
                else
                {
                    ThisResult.Code = 0;
                    ThisResult.Message = "Удаляем входящий файл. " + MsgErr.Err;
                    ThisResult.NotLoaded = false;
                }
            }
            else
            {
                ThisResult.Code = (int)MsgErr.ReturnMessage.Результат;
                ThisResult.Message = (string)MsgErr.ReturnMessage.Инфо;
            }

            if (ThisResult.Code != 3) // усе загрузили
            {
                if (ThisResult.Message.Contains("Конфликт блокировок"))
                {
                    AddLog(SettingsBase.RTB, ThisResult.Message);
                    return;
                }

                ThisResult.NotLoaded = false;
                try
                {
                    File.Delete(SettingsBase.Settings.ExchangeTemporaryDir1C + FileXML);
                    File.Delete(ExchangeDir + FileFlag);
                }
                catch (Exception) { }

                return;
            }

            //// if (ReturnMessage.Результат == 3 //Обновление может быть выполнено в режиме Конфигуратор.
            GM.V8Null(SettingsBase);
            AddLog(SettingsBase.RTB, "Получена конфигурация...");

            if (GM.TaskWork(1, SettingsBase))
            {
                AddLog(SettingsBase.RTB, "Закрываем сеансы...");
                GM.KillUserSession(SettingsBase, false, false);
                //new GM().KillSessions(SettingsBase);
            }

            newGM = new GM();
            newGM.AddLog += AddLogG;//AddLogEvent;
            newGM.UpdateCfg(SettingsBase);// обновим конфу
            newGM.AddLog -= AddLogG;//AddLogEvent;

            ThisResult.NotLoaded = false;

            AddLog(SettingsBase.RTB, "Повторная загрузка... ");


            newGM = new GM();
            newGM.AddLog += AddLogG;//AddLogEvent;
            if (!newGM.ConnectTo1c(SettingsBase)) return;
            newGM.AddLog -= AddLogG;//AddLogEvent;

            MsgErr = v8ReadMessage();

            if (MsgErr.Err != "")
            {
                if (MsgErr.ReturnMessage != null)
                {
                    ThisResult.Code = (int)MsgErr.ReturnMessage.Результат;
                    ThisResult.Message = (string)MsgErr.ReturnMessage.Инфо;
                }
                else
                {
                    ThisResult.Code = 99;
                    ThisResult.Message = MsgErr.Err;
                    //GM.V8Null(SettingsBase);
                }

            }
            else
            {
                ThisResult.Code = (int)MsgErr.ReturnMessage.Результат;
                ThisResult.Message = (string)MsgErr.ReturnMessage.Инфо;
                if (ThisResult.Code == 0)
                {
                    ThisResult.NotLoaded = false;
                    try
                    {
                        File.Delete(SettingsBase.Settings.ExchangeTemporaryDir1C + FileXML);
                        File.Delete(ExchangeDir + FileFlag);
                    }
                    catch (Exception) { }
                }
            }
            return;

        }
        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(RTB, Data);
        }

    }

    public class AutoExchangeThread
    {
        public delegate void EditTabPageTextMethodContainer(GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditTabPageTextMethodContainer EditTabPageText;

        public delegate void MethodContainer(GlobalVars.SettingsBaseListClass SettingsBase, int Mode, string ThreadName);
        public event MethodContainer ClosingThread;

        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;
        public delegate void EditDGVMethodContainer(int index, string column, string Data, DataGridView DGV, GlobalVars.SettingsBaseListClass SettingsBase);
        public event EditDGVMethodContainer EditDGV;
        public bool StopThread = false;
        public Thread thread;
        public string ThreadName;
        
        public GlobalVars.SettingsBaseListClass SettingsBase;

        public AutoExchangeThread()
        {
            
        }
        
        public void Start()
        {

            try
            {
                SettingsBase.Settings.AutoExchange = true;
                SettingsBase.Settings.Exchange_launched = true;
                thread = new Thread(this.AutoExchange);
                thread.IsBackground = true;
                thread.Start();
                //AddLog("Старт thread.Start(DGV);");
            }
            catch (System.AccessViolationException E)
            {
                AddLog(SettingsBase.RTB, E.Message);
                SettingsBase.Settings.Exchange_launched = false;
                SettingsBase.Settings.AutoExchange = false;
                ClosingThread(SettingsBase, 0, ThreadName);
            }

        }

        void AutoExchange(object Val)
        {
            try
            {
                if (SettingsBase.Settings.KillUserSessionBeforeAutoExchange)
                {
                    AddLog(SettingsBase.RTB, "Завершение сеансов пользователя " + SettingsBase.Settings.User);
                    GM.KillUserSession(SettingsBase, true, false);
                }
            }
            catch (Exception)
            {
            }

            string Code, ExchangeDir, FileFlagDn, FileFlagUp, NodeName, FileZipDn, FileXMLDn, FileZipUp, FileXMLUp;
            GM.VoidResult ReadMessageResult, WriteMessageResult;

            AddLog(SettingsBase.RTB, "Начало автообмена.");

            while (true)
            {
                if (!SettingsBase.Settings.AutoExchange)
                {
                    break;
                }

                if (!GM.TaskWork(0, SettingsBase))
                {
                    Thread.Sleep(1000);
                    try
                    {
                        EditTabPageText(SettingsBase);
                    }
                    catch (Exception) { }
                    continue;
                }
                try
                {
                    EditTabPageText(SettingsBase);
                }
                catch (Exception) { }
                if (GM.TaskWork(2, SettingsBase))
                {
                    //UpLoadThread UpLoadThreadMT = new UpLoadThread();
                    //UpLoadThreadMT.SettingsBase = SettingsBase;
                    //UpLoadThreadMT.Mode = 3;
                    //UpLoadThreadMT.AddLog += AddLogG;
                    //UpLoadThreadMT.EditDGV += EditDGVG;
                    //UpLoadThreadMT.UpLoad();
                    UpLoadThread UpLoadThread = new UpLoadThread();
                    UpLoadThread.SettingsBase = GlobalVars.CurrentSettingsBase; ;
                    UpLoadThread.Mode = 3;
                    
                    UpLoadThread.AddLog += AddLogG;
                    UpLoadThread.EditDGV += EditDGVG;
                    UpLoadThread.EditTabPageText += EditTabPageTextG;
                    UpLoadThread.UpLoad();
                    UpLoadThread.AddLog -= AddLogG;
                    UpLoadThread.EditDGV -= EditDGVG;
                    UpLoadThread.EditTabPageText -= EditTabPageTextG;

                    continue;
                }

                // try
                //{
                // EditTabPageText(SettingsBase);
                try
                {
                    if (!File.Exists(SettingsBase.Settings.ExchangeTemporaryDir1C)) Directory.CreateDirectory(SettingsBase.Settings.ExchangeTemporaryDir1C);
                }
                catch (Exception) { }

                foreach (DataGridViewRow Row in SettingsBase.DGV.Rows)
                {
                    if (!SettingsBase.Settings.AutoExchange)
                    {
                        //AddLog(" !GlobalVars.AutoExchange; break");
                        break;
                    }
                    if (!GM.TaskWork(0, SettingsBase))
                    {
                        //AddLog(" !GM.TaskWork(0); break");
                        break;
                    }
                    //try
                    //{
                    if (Row.Cells["Code"].Value == null)
                    {
                        //AddLog(" Row.Cells[Code].Value == null; continue");
                        continue;
                    }
                    if (Row.Cells["Status"].Value == null)
                    {
                        //EditDGV(Row.Index, "Status", "", SettingsBase.DGV);
                    }
                    else
                        if (!(Row.Cells["Status"].Value.ToString() == ""))
                            EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase);//Row.Cells["Status"].Value = "";
                    if (!Convert.ToBoolean(Row.Cells["Checked"].Value))
                    {
                        //AddLog(" Row.Cells[Checked].Value == null; continue");
                        continue;
                    }

                    Code = Row.Cells["Code"].Value.ToString().Trim();
                    if (Code == "")
                    {
                        //AddLog(" Code == ; continue");
                        continue;
                    };

                    ///Кусок кода обмена начало
                    /// 

                    ExchangeDir = Row.Cells["Dir"].Value.ToString().Trim();
                    if (ExchangeDir == "") ExchangeDir = SettingsBase.Settings.ExchangeDir;

                    if (!Directory.Exists(ExchangeDir))
                    {
                        AddLog(SettingsBase.RTB, "Каталог недоступен. " + ExchangeDir);
                        continue;
                    }

                    NodeName = Code + " " + Row.Cells["Name"].Value.ToString();
                    //AddLog(NodeName);

                    FileFlagDn = Code + "-" + SettingsBase.Settings.ThisNodeCode + ".txt";
                    FileFlagUp = SettingsBase.Settings.ThisNodeCode + "-" + Code + ".txt";

                    FileZipDn = FileFlagDn.Replace(".txt", ".zip");
                    FileXMLDn = FileFlagDn.Replace(".txt", ".xml");

                    FileZipUp = FileFlagUp.Replace(".txt", ".zip");
                    FileXMLUp = FileFlagUp.Replace(".txt", ".xml");



                    /// Загрузка
                    /// 
                    //AddLog(SettingsBase.RTB, "Существует  " + ExchangeDir + FileFlagDn + " " + File.Exists(ExchangeDir + FileFlagDn).ToString());
                    //AddLog(SettingsBase.RTB, "Существует  " + ExchangeDir + FileZipDn + " " + File.Exists(ExchangeDir + FileZipDn).ToString());
                    if (File.Exists(ExchangeDir + FileFlagDn))
                    {
                        EditDGV(Row.Index, "Status", "Загрузка", SettingsBase.DGV, SettingsBase);//Row.Cells["Status"].Value = "Загрузка";
                        //EditDGV(Row.Index, "StatusFlag", "", SettingsBase.DGV); //Row.Cells["StatusFlag"].Value = "";
                        AddLog(SettingsBase.RTB, "Загрузка узла " + NodeName);

                        try
                        {


                            GM newGM = new GM();
                            newGM.AddLog += AddLogG;//AddLogEvent;
                            ReadMessageResult = newGM.ReadMessage(SettingsBase, FileXMLDn, FileZipDn, FileFlagDn, ExchangeDir, Code);//, "AutoExchangeThread", this);
                            newGM.AddLog -= AddLogG;//AddLogEvent;
                      }
                        catch (Exception)
                        {
                            ReadMessageResult = new GM.VoidResult();
                        }

                        

                        if (ReadMessageResult.Code == 0)
                        {
                            AddLog(SettingsBase.RTB, "Загрузка узла завершено. Узел:" + Code + "; " + ExchangeDir + FileZipDn);
                            File.Delete(ExchangeDir + FileFlagUp);

                            Thread.Sleep(SettingsBase.Settings.TimePauseDnUp);
                            //AddLog(SettingsBase.RTB, SettingsBase.Settings.TimePauseDnUp.ToString());

                            /// Выгрузка
                            /// 
                            if (!File.Exists(ExchangeDir + FileFlagUp))
                            {
                                AddLog(SettingsBase.RTB, "Выгрузка узла " + NodeName);
                                EditDGV(Row.Index, "Status", "Выгрузка", SettingsBase.DGV, SettingsBase); //Row.Cells["Status"].Value = "Выгрузка";

                                GM newGM = new GM();
                                newGM.AddLog += AddLogG;//AddLogEvent;

                                WriteMessageResult = newGM.WriteMessage(SettingsBase, FileXMLUp, FileZipUp, FileFlagUp, ExchangeDir, Code);
                                newGM.AddLog -= AddLogG;//AddLogEvent;

                                if (WriteMessageResult.Code == 0)
                                {
                                    AddLog(SettingsBase.RTB, "Выгрузка узла завершено. Размер файла: " + WriteMessageResult.Message + " Узел:" + NodeName + "; " + ExchangeDir + FileZipUp);
                                }
                                else
                                {
                                    AddLog(SettingsBase.RTB, "Ошибка при выгрузке узла 2. " + WriteMessageResult.Message + " Узел:" + NodeName);
                                    //GM.V8Null(SettingsBase);
                                    (new GM()).ConnectTo1c(SettingsBase);
                                    if (WriteMessageResult.Message.Contains("Внешний компонент создал исключение"))
                                    {
                                        continue;
                                    }
                                }
                                EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase);
                            }
                        }
                        else
                        {
                            AddLog(SettingsBase.RTB, "Ошибка при загрузке узла.. " + ReadMessageResult.Message + " Узел:" + NodeName);
                            EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase);
                            Thread.Sleep(SettingsBase.Settings.TimePauseDnUp);
                            if (!File.Exists(ExchangeDir + FileFlagDn)) continue;
                        }

                    }
                    else
                    {//upd
                        if (SettingsBase.Settings.UploadingAutoExchange)
                        {
                            if (!File.Exists(ExchangeDir + FileFlagUp))
                            {
                                AddLog(SettingsBase.RTB, "Выгрузка узла " + NodeName);
                                EditDGV(Row.Index, "Status", "Выгрузка", SettingsBase.DGV, SettingsBase);//Row.Cells["Status"].Value = "Выгрузка";

                                GM newGM = new GM();
                                newGM.AddLog += AddLogG;//AddLogEvent;

                                WriteMessageResult = newGM.WriteMessage(SettingsBase, FileXMLUp, FileZipUp, FileFlagUp, ExchangeDir, Code);
                                newGM.AddLog -= AddLogG;//AddLogEvent;

                                //WriteMessageResult = GM.WriteMessage(FileXMLUp, FileZipUp, FileFlagUp, ExchangeDir, Code, "AutoExchangeThread", this);

                                if (WriteMessageResult.Code == 0)
                                {
                                    AddLog(SettingsBase.RTB, "Выгрузка узла завершено. Размер файла: " + WriteMessageResult.Message + " Узел:" + NodeName + "; " + ExchangeDir + FileZipUp);
                                }
                                else
                                {
                                    AddLog(SettingsBase.RTB, "Ошибка при выгрузке узла 3. " + WriteMessageResult.Message + " Узел:" + NodeName);
                                    //GM.V8Null(SettingsBase);
                                    (new GM()).ConnectTo1c(SettingsBase);
                                }
                                EditDGV(Row.Index, "Status", "", SettingsBase.DGV, SettingsBase);
                            }
                        }

                    }//upd

                    //EditDGV(Row.Index, "Status", "", SettingsBase.DGV); //Row.Cells["Status"].Value = "";

                    /// 
                    ///Кусок кода обмена конец
                    //}
                    //catch (Exception)  { }
                    ////catch (InvalidCastException e) { AddLog(e.Message.ToString()); }
                }

                //}
                //catch (Exception)  { }
                ////catch (InvalidCastException e) { AddLog(e.Message.ToString()); }

                //int pause=0;

                GM.V8Null(SettingsBase);
                try
                {
                    EditTabPageText(SettingsBase);
                }
                catch (Exception) { }


                DateTime pause = new DateTime();
                pause = DateTime.Now;
                pause = pause.AddMilliseconds(SettingsBase.Settings.TimePause);

                while (true)
                {
                    if (!SettingsBase.Settings.AutoExchange) break;
                    if (pause <= DateTime.Now) break;
                    //if (pause >= GlobalVars.TimePause) break;
                    //pause = pause + 1000;
                    Thread.Sleep(1000);
                }

            }

            AddLog(SettingsBase.RTB, "Автообмен завершен.");
            ClosingThread(SettingsBase, 0, ThreadName);
            GM.V8Null(SettingsBase);

        }

        private void UpLoadThreadMT_EditDGV(int index, string column, string Data, DataGridView DGV)
        {
            throw new NotImplementedException();
        }

        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(SettingsBase.RTB, Data);
        }

        public void EditDGVG(int index, string column, string Data, DataGridView DGVs, GlobalVars.SettingsBaseListClass SettingsBase)
        {
            EditDGV(index, column, Data, DGVs, SettingsBase);
        }
        public void EditTabPageTextG(GlobalVars.SettingsBaseListClass SettingsBase)
        {
            EditTabPageText( SettingsBase);
        }
        public void Stop()
        {
            SettingsBase.Settings.AutoExchange = false; 
        }

    }

    public class GM
    {
        public delegate void AddLogMethodContainer(RichTextBox RTB, string Data);
        public event AddLogMethodContainer AddLog;


        public GM()
        {
        }

        public static void CheckUpdate(bool Manual = false)
        {
            string Dir = GlobalVars.UpdateDir;

            if (GlobalVars.SettingsBaseList != null)
                foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
                {
                    if (CurrS.Settings != null)
                    {
                        Dir = CurrS.Settings.DirUpd;
                        break;
                    }
                }

            if (!Directory.Exists(Dir))
            {
                if (Manual) MessageBox.Show("Каталог обновления недоступен. " + Dir);
                return;
            }

            string CurrVer = "";
            string UpdVer = "";

            string CurrVerfile = GlobalVars.ExeDir + "Ver.txt";
            string UpdVerfile = Dir + "Ver.txt";

            string line = "";

            var encoding = Encoding.GetEncoding("UTF-8");//Encoding.GetEncoding(1251);

            if (File.Exists(CurrVerfile))
                using (var src = new StreamReader(CurrVerfile, encoding: encoding))
                {
                    while ((line = src.ReadLine()) != null) CurrVer = line;
                }

            if (File.Exists(UpdVerfile))
                using (var src = new StreamReader(UpdVerfile, encoding: encoding))
                {
                    while ((line = src.ReadLine()) != null) UpdVer = line;
                }

            if (CurrVer == UpdVer)
            {
                if (Manual) MessageBox.Show("Обновление не требуется.");
                return;
            }

            string ExeRename = GlobalVars.ExeDir + "tmp.tmp";
            string ExeNewInst = Dir + GlobalVars.ExeName;
            string ExeNew = GlobalVars.ExeDir + "NEW.tmp";

            if (File.Exists(ExeRename)) File.Delete(ExeRename);
            //if (File.Exists(ExeNew)) File.Delete(ExeNew);

            if (File.Exists(ExeNewInst))
            {
                File.Move(GlobalVars.ExeDir + GlobalVars.ExeName, ExeRename);
                File.Copy(ExeNewInst, GlobalVars.ExeDir + GlobalVars.ExeName);
                //File.Move(ExeNew, GlobalVars.ExeDir + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".EXE");
                //this.Text = "Необходимо перезапустить программу.";

                if (GlobalVars.SettingsBaseList != null)
                    foreach (GlobalVars.SettingsBaseListClass CurrB in GlobalVars.SettingsBaseList)
                    {
                        GM.V8Null(CurrB);
                    }
                try
                {
                    File.Copy(UpdVerfile, CurrVerfile);
                    //MessageBox.Show(GlobalVars.ExeDir);
                    //MessageBox.Show(GlobalVars.ExeName);
                    Process.Start(GlobalVars.ExeDir + "updater.exe", GlobalVars.ExeName);
                    Application.Exit();
                }
                catch (Exception ee) { MessageBox.Show(ee.Message); File.Delete(CurrVerfile); return; }

                //Application.Exit();
            }

            //File.Copy(UpdVerfile, CurrVerfile);
        }

        public static void SetHeaderDGV(DataGridView DGV)
        {
            if (DGV == null) return;
            foreach (DataGridViewColumn DGVC in DGV.Columns)
            {
                if (DGVC.Name == "Checked")
                {
                    DGVC.HeaderText = "";
                    DGVC.DisplayIndex = 0;
                    DGVC.Width = 18;
                }
                if (DGVC.Name == "CompoundState")
                {
                    DGVC.HeaderText = "Статус";
                    DGVC.DisplayIndex = 1;
                    DGVC.Width = 200;
                }
                if (DGVC.Name == "StatusFlag")
                {
                    DGVC.HeaderText = "Статус";
                    DGVC.DisplayIndex = 2;
                    DGVC.Visible = false;
                }
                if (DGVC.Name == "Status")
                {
                    DGVC.HeaderText = "Состояние";
                    DGVC.DisplayIndex = 3;
                    DGVC.Visible = false;
                }
                if (DGVC.Name == "Code")
                {
                    DGVC.HeaderText = "Код";
                    DGVC.DisplayIndex = 4;
                    DGVC.Width = 40;
                }
                if (DGVC.Name == "Name")
                {
                    DGVC.HeaderText = "Наименовние";
                    DGVC.DisplayIndex = 5;
                    DGVC.Width = 200;
                }
                if (DGVC.Name == "Dir")
                {
                    DGVC.Visible = false;
                }
            }
        }

        //static public void AddActionStatus(GlobalVars.SettingsBaseListClass CurrSett, string Data)
        //{
        //    string sData;
        //    if (CurrSett.RTB == null) return;

        //    sData = DateTime.Now.ToString() + " : " + Data;
        //   // if (CurrSett.RTB.TextLength > 600) CurrSett.RTB.Text = "";
        //    if (CurrSett.RTB.Lines.Length > 600) CurrSett.RTB.Text = "";

        //    CurrSett.RTB.AppendText(sData);
        //    //this.richTextBox1.AppendText(ToUTF8Win1251(sData));
        //    CurrSett.RTB.AppendText(Environment.NewLine);

        //    //string logdir, fname;

        //    //logdir = @Application.StartupPath.ToString() + "\\log\\";
        //    //fname = logdir + DateTime.Now.ToString("MM_dd_yyyy") + ".txt";

        //    //try
        //    //{

        //    //    if (!Directory.Exists(logdir))
        //    //    {
        //    //        DirectoryInfo di = Directory.CreateDirectory(logdir);
        //    //    }

        //    //    if (File.Exists(fname))
        //    //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fname, true))
        //    //        {
        //    //            file.WriteLine(sData);
        //    //        }
        //    //    else
        //    //        System.IO.File.WriteAllText(fname, sData);
        //    //}
        //    //catch (Exception)  { }

        //}

        public static GlobalVars.SettingsBaseListClass GetSettings(string SettingsName)
        {
            GlobalVars.SettingsBaseListClass Result = new GlobalVars.SettingsBaseListClass();

            if (GlobalVars.SettingsBaseList != null)
                foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
                {
                    if (CurrS.SettingsName != SettingsName) continue;
                    Result.SettingsName = SettingsName;
                    Result.Settings = CurrS.Settings;
                    Result.DGV = CurrS.DGV;
                    Result.RTB = CurrS.RTB;
                    Result.NodesLists = CurrS.NodesLists;
                    break;
                }
            return Result;
        }

        public struct VoidResult
        {
            public int Code;
            public bool NotLoaded;
            public string Message;
        }

        public struct MsgErr
        {
            public dynamic ReturnMessage;
            public string Err;
        }

        public static string GetFileSizeNormal(long size)
        {
            string RR;
            decimal resultD = size / 1024;
            if (size < 1024)
            {
                resultD = size;
                RR = " байт";
            }
            else

            if (Math.Floor(resultD) < 1024)
            {
                RR = " килобайт";
            }
            else
            {
                resultD = resultD / 1024;
                RR = " мегабайт";
            }
            
            return Convert.ToString(string.Format("{0:0.000}", resultD)) + RR;
            //return string.Format("{0:N3}", resultD) + RR;
        }

        public VoidResult ReadMessage(GlobalVars.SettingsBaseListClass SettingsBase, string FileXML, string FileZip, string FileFlag, string ExchangeDir, string Code)
        {
            v8ReadMessageThread v8ReadMessage = new v8ReadMessageThread();
            v8ReadMessage.SettingsBase = SettingsBase;
            v8ReadMessage.FileXML = FileXML;
            v8ReadMessage.FileZip = FileZip;
            v8ReadMessage.FileFlag = FileFlag;
            v8ReadMessage.ExchangeDir = ExchangeDir;
            v8ReadMessage.Code = Code;
            v8ReadMessage.AddLog += AddLogG;
            v8ReadMessage.Start();
            v8ReadMessage.AddLog -= AddLogG;

            return v8ReadMessage.ThisResult;

        }

        public VoidResult WriteMessage(GlobalVars.SettingsBaseListClass SettingsBase, string FileXML, string FileZip, string FileFlag, string ExchangeDir, string Code)
        {
            //VoidResult Result = new VoidResult();
            v8WriteMessageThread v8WriteMessage = new v8WriteMessageThread();
            v8WriteMessage.AddLog += AddLogG;
            v8WriteMessage.SettingsBase = SettingsBase;
            v8WriteMessage.FileXML = FileXML;
            v8WriteMessage.FileZip = FileZip;
            v8WriteMessage.FileFlag = FileFlag;
            v8WriteMessage.ExchangeDir = ExchangeDir;
            v8WriteMessage.Code = Code;

            v8WriteMessage.Start();
            v8WriteMessage.AddLog -= AddLogG;

            return v8WriteMessage.ThisResult;



        }

        public static string[] HoursToArray(string strIn)
        {
            string[] arrHoure = { };

            if (strIn == "*")
            {
                for (int i = 0; i < 10; i++)
                {
                    Array.Resize(ref arrHoure, i + 1);
                    arrHoure[i] = "0"+i.ToString();

                }
                for (int i = 10; i < 25; i++)
                {
                    Array.Resize(ref arrHoure, i + 1);
                    arrHoure[i] = i.ToString();

                }
            }
            else
            {
                arrHoure = strIn.Split(new char[] { ',' });
                //arrHoure = strIn.Split(new char[] { ',' }, 2);
            }
            return arrHoure;
        }

        public static string[] MinutsToArray(string strIn)
        {
            string[] arrMinuts = { };

            if (strIn == "*")
            {
                for (int i = 0; i < 10; i++)
                {
                    Array.Resize(ref arrMinuts, i + 1);
                    arrMinuts[i] = "0"+i.ToString();

                }
                for (int i = 10; i < 61; i++)
                {
                    Array.Resize(ref arrMinuts, i + 1);
                    arrMinuts[i] = i.ToString();

                }
            }
            else
            {
                arrMinuts = strIn.Split(new char[] { ',' });
                //arrMinuts = strIn.Split(new char[] { ',' }, 2);
            }
            return arrMinuts;
        }

        public static bool TaskWork(int Mode, GlobalVars.SettingsBaseListClass SettingsBase)
        {
            ICronSchedule _cron_schedule;
            string schedule="";

            if (Mode == 0)
            {
                schedule = SettingsBase.Settings.TimeExchange;
            }


            if (Mode == 1)
            {
                schedule = SettingsBase.Settings.TimeKillUsers;
            }

            if (Mode == 2)
            {
                schedule = SettingsBase.Settings.TimeUpLoading;
            }
            _cron_schedule = new CronSchedule(schedule);
            return _cron_schedule.isTime(DateTime.Now);

        }

        public static void ProgramReadSettings(System.Data.DataTable DT = null)
        {
            string AppDir = Application.StartupPath + "\\";
            AppDir = AppDir.ToUpper();
             string m_appName = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper().Replace(AppDir, "");
          //string m_appName = Application.ExecutablePath.ToString().Replace(Application.StartupPath.ToString() + "\\", "");
             //string AppDir = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper().Replace(m_appName, "");

            //MessageBox.Show(m_appName);

            //MessageBox.Show(AppDir);
            string[] allFoundFiles = Directory.GetFiles(AppDir, "*S.xml", SearchOption.TopDirectoryOnly);
            //MessageBox.Show("00");

            GlobalVars.SettingsBaseList = new List<GlobalVars.SettingsBaseListClass>();
            //GlobalVars.Settings BaseList = new List<GlobalVars.Settings>();
            //GlobalVars.NodesLists BaseNodes = new List<GlobalVars.NodesLists>();
            GlobalVars.Settings NewSettings;
            GlobalVars.SettingsBaseListClass SettingsBaseList;
            GlobalVars.NodesLists CurrBaseNodes;
            System.Data.DataTable CurrNodes;
            foreach (string file in allFoundFiles)
            { 
                if (File.Exists(file))
                {
                    using (Stream stream = new FileStream(file, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(GlobalVars.Settings));
                        NewSettings = new GlobalVars.Settings();
                        NewSettings = (GlobalVars.Settings)serializer.Deserialize(stream);
                        //GlobalVars.BaseList.Add(NewSettings);
                        if (File.Exists(AppDir + "\\" + NewSettings.SettingsName + "N.xml"))
                        {
                            CurrBaseNodes = new GlobalVars.NodesLists();
                            CurrNodes = new System.Data.DataTable();


                            CurrNodes.ReadXml(AppDir + "\\" + NewSettings.SettingsName + "N.xml");

                            //if (CurrNodes.Columns.Contains("Code"))
                            //{
                            //    MessageBox.Show(" Code есть");
                            //}
                            //if (CurrNodes.Columns.Contains("Code1"))
                            //{
                            //    MessageBox.Show(" Code1 есть");
                            //}
                            DataColumn DC = new DataColumn("Checked", typeof(bool));
                            DC.DefaultValue = true;
                            DC.Caption = "";
                            if (!CurrNodes.Columns.Contains("Checked"))
                            {
                                CurrNodes.Columns.Add(DC);
                            }

                            DataColumn DCompoundState = new DataColumn("CompoundState", typeof(string));
                            DCompoundState.DefaultValue = "";
                            DCompoundState.Caption = "";
                            if (!CurrNodes.Columns.Contains("CompoundState"))
                            {
                                CurrNodes.Columns.Add(DCompoundState);
                            }
 
                            DataColumn DCStatusFlag = new DataColumn("StatusFlag", typeof(string));
                            DCStatusFlag.DefaultValue = "";
                            DCStatusFlag.Caption = "";
                            if (!CurrNodes.Columns.Contains("StatusFlag"))
                            {
                                CurrNodes.Columns.Add(DCStatusFlag);
                            }

                            DataColumn DCStatus = new DataColumn("Status", typeof(string));
                            DCStatus.DefaultValue = "";
                            DCStatus.Caption = "";
                            if (!CurrNodes.Columns.Contains("Status"))
                            {
                                CurrNodes.Columns.Add(DCStatus);
                            }
                            //DC.

                            CurrBaseNodes.SettingsName = NewSettings.SettingsName;
                            CurrBaseNodes.Nodes = CurrNodes;
                            //GlobalVars.BaseNodes.Add(CurrBaseNodes);

                            NewSettings.Exchange_launched = false;

                            SettingsBaseList = new GlobalVars.SettingsBaseListClass();
                            SettingsBaseList.SettingsName = NewSettings.SettingsName;
                            SettingsBaseList.Settings = NewSettings;
                            SettingsBaseList.NodesLists = CurrBaseNodes;
                            GlobalVars.SettingsBaseList.Add(SettingsBaseList);

                        }
                    }
                }
            }



            //TimeExpanded();
        }

        public static void ProgramSaveSettings(System.Data.DataTable DT = null)
        {
            //string appdir1 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToUpper() + ".EXE";
            //string AppDir = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper().Replace(appdir1, "");
            string AppDir = Application.StartupPath + "\\";
            AppDir = AppDir.ToUpper();
            string m_appName = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper().Replace(AppDir, "");


            foreach (GlobalVars.SettingsBaseListClass CurrS in GlobalVars.SettingsBaseList)
            {
            using (Stream writer = new FileStream(AppDir + CurrS.SettingsName + "S.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(GlobalVars.Settings));
                serializer.Serialize(writer, CurrS.Settings);
                }
                foreach (DataColumn CDC in CurrS.NodesLists.Nodes.Columns)
                {
                    if (CDC.ColumnName == "CompoundState")
                    {
                        CurrS.NodesLists.Nodes.Columns.Remove(CDC);
                        break;
                    }
                }
                foreach (DataColumn CDC in CurrS.NodesLists.Nodes.Columns)
                {
                    if (CDC.ColumnName == "StatusFlag")
                    {
                        CurrS.NodesLists.Nodes.Columns.Remove(CDC);
                        break;
                    }
                }
                foreach (DataColumn CDC in CurrS.NodesLists.Nodes.Columns)
                {
                    if (CDC.ColumnName == "Status")
                    {
                        CurrS.NodesLists.Nodes.Columns.Remove(CDC);
                        break;
                    }
                }
                foreach (DataColumn CDC in CurrS.NodesLists.Nodes.Columns)
                {
                    if (CDC.ColumnName == "Checked")
                    {
                        CurrS.NodesLists.Nodes.Columns.Remove(CDC);
                        break;
                    }
                }
                CurrS.NodesLists.Nodes.WriteXml(AppDir + CurrS.SettingsName + "N.xml", XmlWriteMode.WriteSchema);

                DataColumn DC = new DataColumn("Checked", typeof(bool));
                DC.DefaultValue = true;
                DC.Caption = "";
                CurrS.NodesLists.Nodes.Columns.Add(DC);


                DataColumn DCompoundState = new DataColumn("CompoundState", typeof(string));
                DCompoundState.DefaultValue = "";
                DCompoundState.Caption = "";
                CurrS.NodesLists.Nodes.Columns.Add(DCompoundState);

                DataColumn DCStatusFlag = new DataColumn("StatusFlag", typeof(string));
                DCStatusFlag.DefaultValue = "";
                DCStatusFlag.Caption = "";
                CurrS.NodesLists.Nodes.Columns.Add(DCStatusFlag);

                DataColumn DCStatus = new DataColumn("Status", typeof(string));
                DCStatus.DefaultValue = "";
                DCStatus.Caption = "";
                CurrS.NodesLists.Nodes.Columns.Add(DCStatus);

            }

        }

        public bool ConnectTo1c(GlobalVars.SettingsBaseListClass SettingsBase)
        {
            bool result = true;

            try
            {
                if (SettingsBase.v8 != null) return true;
            }
           // catch(Exception e)
           catch
            {
                V8Null(SettingsBase);
            }
            const string quotes = "\"";
            string ConnectionString =
                "Srvr=" + quotes + SettingsBase.Settings.Cluster + quotes
                + ";Ref=" + quotes + SettingsBase.Settings.Base + quotes
                + ";Usr=" + quotes + SettingsBase.Settings.User + quotes
                + "; Pwd=" + quotes + SettingsBase.Settings.Password + quotes + ";";
            try
            {

                //if (SettingsBase.Settings.Version1C == "2")
                //{
                //    SettingsBase.v8Connector = new V82.COMConnector();
                //    //AddLog(SettingsBase.RTB, "new V82.COMConnector");
                //}
                //else
                //{
                    SettingsBase.v8Connector = new V83.COMConnector();
                    //AddLog(SettingsBase.RTB, "new V83.COMConnector");
                //}
                
                SettingsBase.v8Connector.PoolCapacity = 1;
                SettingsBase.v8Connector.PoolTimeout = 1;
                SettingsBase.v8Connector.MaxConnections = 1;
                //connector.PoolCapacity = 10;
                //connector.PoolTimeout = 60;
                //connector.MaxConnections = 2;
                SettingsBase.v8 = SettingsBase.v8Connector.Connect(ConnectionString);

                //v8Connector = null;

                GlobalVars.Dir1c = SettingsBase.v8.КаталогПрограммы();
                
                //connector.
                //GlobalVars.v8
            }
            catch (Exception e)
            {
                V8Null(SettingsBase);
                result = false;
                AddLog(SettingsBase.RTB, "Ошибка при создании COM объекта.");
                AddLog(SettingsBase.RTB, e.Message.ToString());
            }
            return result;
        }

        public bool KillSessions(GlobalVars.SettingsBaseListClass SettingsBase)
        {

            if (!ConnectTo1c(SettingsBase)) return false;
            try
            {
                AddLog(SettingsBase.RTB, "Закрываем сеансы...");
                SettingsBase.v8.ПроцедурыОбменаДаннымиДоп.ОтключитьСеансыБазы(SettingsBase.Settings.Cluster
                    , SettingsBase.Settings.Base
                    , SettingsBase.Settings.User
                    , SettingsBase.Settings.Password
                    , 0);

            }
            catch (Exception e)
            {
                AddLog(SettingsBase.RTB, e.Message.ToString());
                //GM.MessageWithOutThread(threadName, MC, e.Message.ToString());
                return false;
            }
            //GM.AddActionStatus(SettingsBase, "Закрыли сеансы.");
            AddLog(SettingsBase.RTB, "Закрыли сеансы.");
            return true;

        }


        public void UpdateCfg(GlobalVars.SettingsBaseListClass SettingsBase)//string threadName = "", object thread = null)
        {
            //GM.AddActionStatus(SettingsBase, "Попытка обновления ИБ...");
            AddLog(SettingsBase.RTB, "Попытка обновления ИБ...");

            string FileName = GlobalVars.Dir1c + "1CV8.EXE";
            //Params:= 'CONFIG /S ' + pBaseClaster + '\'+pBaseName+' / N '+pBaseLogin+' / P '+pBasePassword+' / DisableStartupMessages / UpdateDBCfg' ;
            string Params = "CONFIG /S " + SettingsBase.Settings.Cluster.ToUpper() + @"\" + SettingsBase.Settings.Base.ToUpper()
                + " /N " + SettingsBase.Settings.User
                + " /P " + SettingsBase.Settings.Password
                + " /DisableStartupMessages /UpdateDBCfg";
            //ExecAndWait(FileName, Params, SW_SHOWNORMAL);
            AddLog(SettingsBase.RTB, FileName);
            AddLog(SettingsBase.RTB, Params);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = FileName;
                proc.StartInfo.Arguments = Params;
                proc.Start();
                proc.WaitForExit();//ожидаем завершения
        }

        public static void V8Null(GlobalVars.SettingsBaseListClass SettingsBase)
        {

            //try
            //{
            //    Marshal.ReleaseComObject(SettingsBase.v8);
            //}
            //catch (Exception) { }

            //try
            //{
            //    Marshal.ReleaseComObject(SettingsBase.v8Connector);
            //}
            //catch (Exception) { }

            try
            {
                Marshal.FinalReleaseComObject(SettingsBase.v8);
            }
            catch (Exception) { }


            try
            {
                Marshal.FinalReleaseComObject(SettingsBase.v8Connector);
            }
            catch (Exception) { }

            SettingsBase.v8 = null;
            SettingsBase.v8Connector = null;

            try
            {
                GC.Collect();
            }
            catch (Exception) { }
            try
            {
                GC.WaitForPendingFinalizers();
            }
            catch (Exception) { }


        }

        public string LoadNodesFromBase(GlobalVars.SettingsBaseListClass SettingsBase, System.Data.DataTable Nodes)
        {
            string ThisNodeCode = null;
            try {
                GM newGM = new GM();
                newGM.AddLog += AddLogG;//AddLogEvent;

                if (!newGM.ConnectTo1c(SettingsBase)) return ThisNodeCode;
                newGM.AddLog -= AddLogG;//AddLogEvent;

                dynamic refer;
                dynamic zapros = SettingsBase.v8.NewObject("Запрос");
                if (SettingsBase.Settings.ExchangePlan == "Полный")
                    ThisNodeCode = SettingsBase.v8.ПланыОбмена.Полный.ЭтотУзел().Код;
                else
                    if (SettingsBase.Settings.ExchangePlan == "УТМагазин")
                    ThisNodeCode = SettingsBase.v8.ПланыОбмена.УТМагазин.ЭтотУзел().Код;
                else
                    ThisNodeCode = SettingsBase.v8.ПланыОбмена.ОбменЗакакзамиИМ.ЭтотУзел().Код;

                string ss = "ВЫБРАТЬ ПланО.Код, ПланО.Наименование ИЗ ПланОбмена.ИмяПланаОбмена КАК ПланО ГДЕ НЕ ПланО.ПометкаУдаления И ПланО.Код <> &КодУзла ;";
                ss = ss.Replace("ИмяПланаОбмена", SettingsBase.Settings.ExchangePlan);
                zapros.Текст = @ss;
                zapros.УстановитьПараметр("КодУзла", ThisNodeCode);
                refer = zapros.Выполнить().Выбрать();

               Nodes.Rows.Clear();
               System.Data.DataRow NewRow;


                while (refer.следующий())
                {
                    NewRow = Nodes.Rows.Add();
                    NewRow["Code"] = refer.Код;
                    NewRow["Name"] = refer.Наименование;
                    NewRow["Dir"] = "";
                }
            }
            catch (Exception) {  }
            //V8Null(SettingsBase);
            return ThisNodeCode;
        }

        public static void KillUserSession(GlobalVars.SettingsBaseListClass SettingsBase, bool OnlyCurrUser = false, bool Question = true, bool MainStream = false)
        {
            if (SettingsBase == null) return;
            try
            {
                if (!OnlyCurrUser && Question)
                {
                    string message = "Вы хотите закрыть сеансы всех ползователей, продолжить?";
                    string caption = "Закрытие всех сеансов";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (result == System.Windows.Forms.DialogResult.No)
                    {

                        // Closes the parent form.

                        return;

                    }
                }

                dynamic v8ConnectorL;

                //if (SettingsBase.Settings.Version1C == "2")
                //{
                //    v8ConnectorL = new V82.COMConnector();
                //}
                //else
                //{
                    v8ConnectorL = new V83.COMConnector();
                //}

                //v8ConnectorL = new V82.COMConnector();

                //int IDconn;


                dynamic Agent = v8ConnectorL.ConnectAgent(SettingsBase.Settings.Cluster);
                //IDconn = v8ConnectorL.ISessionInfo().AppID;

                dynamic Clusters = Agent.GetClusters();
                dynamic Processes;
                dynamic CurrProcess;
                dynamic Bases;
                int port;
                dynamic InfoBase;
                dynamic Sessions;
                dynamic Connections;
                dynamic InfoBaseInfo;
                string UserNameUpper = SettingsBase.Settings.User.ToUpper();

                int OurConnID = 0;

                foreach (dynamic Cluster in Clusters)
                {
                    Agent.Authenticate(Cluster, null, null);

                    // найдем наш кластер и базу
                    InfoBase = null;

                    Bases = Agent.GetInfoBases(Cluster);
                    foreach (dynamic Base in Bases)
                    {
                        if (Convert.ToString(Base.Name).ToUpper() == SettingsBase.Settings.Base.ToUpper())
                        {
                            InfoBase = Base;
                            break;
                        }
                    }
                    if (InfoBase == null)
                    {
                        continue;
                    }




                    // пробежимся по процессам и закроем нужное соединение 
                    Processes = Agent.GetWorkingProcesses(Cluster);
                    foreach (dynamic Process in Processes)
                    {
                        port = Process.MainPort;
                        // теперь есть адрес и порт для подключения к рабочему процессу 
                        CurrProcess = v8ConnectorL.ConnectWorkingProcess(SettingsBase.Settings.Cluster + ":" + port);
                        CurrProcess.AddAuthentication(SettingsBase.Settings.User, SettingsBase.Settings.Password);

                        InfoBaseInfo = CurrProcess.CreateInfoBaseInfo();
                        InfoBaseInfo.Name = SettingsBase.Settings.Base;
                        Connections = CurrProcess.GetInfoBaseConnections(InfoBaseInfo);

                        int[] arrayConnID = new int[Connections.GetUpperBound(0)+1];
                        int kk = -1;
                        foreach (dynamic Connection in Connections)
                        {
                            kk = kk + 1;
                            arrayConnID[kk] = Connection.ConnID;
                        }
                        Array.Sort(arrayConnID);
                        OurConnID = arrayConnID[kk];
                        foreach (dynamic Connection in Connections)
                        {
                            //if ((Convert.ToString(Connection.UserName).ToUpper() == UserNameUpper))//врем
                            //{
                            //    continue;
                            //}
                            if (OnlyCurrUser && (Convert.ToString(Connection.UserName).ToUpper() != UserNameUpper))
                            {
                                continue;
                            }
                            if (Connection.ConnID == OurConnID)//врем
                            {
                                continue;
                            }
                            CurrProcess.Disconnect(Connection);
                        }
                    }
                    // закроем сеансы
                    Sessions = Agent.GetInfoBaseSessions(Cluster, InfoBase);
                    foreach (dynamic Session in Sessions)
                    {
                        //Если нРег(Сеанс.AppID) = "backgroundjob" ИЛИ нРег(Сеанс.AppID) = "comconsole" Тогда 
                        //	// если это сеансы com-приложения или фонового задания, то не отключаем 
                        //	Продолжить; 
                        //КонецЕсли; 
                        //НомерСоединенияИнформационнойБазы() 

                        if (Session.connection != null)
                        {
                            if (Session.connection.ConnID == OurConnID)//врем
                            {
                                continue;
                            }
                            //if ((Convert.ToString(Session.UserName).ToUpper() == UserNameUpper))//врем
                            //{
                            //    continue;
                            //}
                            if (OnlyCurrUser && (Convert.ToString(Session.UserName).ToUpper() != UserNameUpper))
                            {
                                continue;
                            }
                            Agent.TerminateSession(Cluster, Session);
                        }
                    }



                }
            }
            catch (Exception) { }//(InvalidCastException e) { }
            
        }

        public void AddLogG(RichTextBox RTB, string Data)
        {
            AddLog(RTB, Data);
        }

        public bool Checkschedule(string schedule)
        {
            ICronSchedule _cron_schedule;
            _cron_schedule = new CronSchedule(schedule);
            _cron_schedule.isTime(DateTime.Now);
            return false;
        }

    }

    public interface ICronSchedule
    {
        bool isValid(string expression);
        bool isTime(DateTime date_time);
    }

    public class CronSchedule : ICronSchedule
    {
        #region Readonly Class Members

        readonly static Regex divided_regex = new Regex(@"(\*/\d+)");
        readonly static Regex range_regex = new Regex(@"(\d+\-\d+)\/?(\d+)?");
        readonly static Regex wild_regex = new Regex(@"(\*)");
        readonly static Regex list_regex = new Regex(@"(((\d+,)*\d+)+)");
        readonly static Regex validation_regex = new Regex(divided_regex + "|" + range_regex + "|" + wild_regex + "|" + list_regex);

        #endregion

        #region Private Instance Members

        private readonly string _expression;
        public List<int> minutes;
        public List<int> hours;
        public List<int> days_of_month;
        public List<int> months;
        public List<int> days_of_week;

        #endregion

        #region Public Constructors

        public CronSchedule()
        {
        }

        public CronSchedule(string expressions)
        {
            this._expression = expressions;
            generate();
        }

        #endregion

        #region Public Methods

        private bool isValid()
        {
            return isValid(this._expression);
        }

        public bool isValid(string expression)
        {
            MatchCollection matches = validation_regex.Matches(expression);
            return matches.Count > 0;//== 5;
        }

        public bool isTime(DateTime date_time)
        {
            return minutes.Contains(date_time.Minute) &&
                   hours.Contains(date_time.Hour) &&
                   days_of_month.Contains(date_time.Day) &&
                   months.Contains(date_time.Month) &&
                   days_of_week.Contains((int)date_time.DayOfWeek);
        }

        private void generate()
        {
            if (!isValid()) return;

            MatchCollection matches = validation_regex.Matches(this._expression);

            generate_minutes(matches[0].ToString());

            if (matches.Count > 1)
                generate_hours(matches[1].ToString());
            else
                generate_hours("*");

            if (matches.Count > 2)
                generate_days_of_month(matches[2].ToString());
            else
                generate_days_of_month("*");

            if (matches.Count > 3)
                generate_months(matches[3].ToString());
            else
                generate_months("*");

            if (matches.Count > 4)
                generate_days_of_weeks(matches[4].ToString());
            else
                generate_days_of_weeks("*");
        }

        private void generate_minutes(string match)
        {
            this.minutes = generate_values(match, 0, 60);
        }

        private void generate_hours(string match)
        {
            this.hours = generate_values(match, 0, 24);
        }

        private void generate_days_of_month(string match)
        {
            this.days_of_month = generate_values(match, 1, 32);
        }

        private void generate_months(string match)
        {
            this.months = generate_values(match, 1, 13);
        }

        private void generate_days_of_weeks(string match)
        {
            this.days_of_week = generate_values(match, 0, 7);
        }

        private List<int> generate_values(string configuration, int start, int max)
        {
            if (divided_regex.IsMatch(configuration)) return divided_array(configuration, start, max);
            if (range_regex.IsMatch(configuration)) return range_array(configuration);
            if (wild_regex.IsMatch(configuration)) return wild_array(configuration, start, max);
            if (list_regex.IsMatch(configuration)) return list_array(configuration);

            return new List<int>();
        }

        private List<int> divided_array(string configuration, int start, int max)
        {
            if (!divided_regex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("/".ToCharArray());
            int divisor = int.Parse(split[1]);

            for (int i = start; i < max; ++i)
                if (i % divisor == 0)
                    ret.Add(i);

            return ret;
        }

        private List<int> range_array(string configuration)
        {
            if (!range_regex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("-".ToCharArray());
            int start = int.Parse(split[0]);
            int end = 0;
            if (split[1].Contains("/"))
            {
                split = split[1].Split("/".ToCharArray());
                end = int.Parse(split[0]);
                int divisor = int.Parse(split[1]);

                for (int i = start; i < end; ++i)
                    if (i % divisor == 0)
                        ret.Add(i);
                return ret;
            }
            else
                end = int.Parse(split[1]);

            for (int i = start; i <= end; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> wild_array(string configuration, int start, int max)
        {
            if (!wild_regex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            for (int i = start; i < max; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> list_array(string configuration)
        {
            if (!list_regex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            string[] split = configuration.Split(",".ToCharArray());

            foreach (string s in split)
                ret.Add(int.Parse(s));

            return ret;
        }

        #endregion
    }



}
