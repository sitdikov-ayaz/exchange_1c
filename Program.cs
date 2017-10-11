using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExchangeVS
{

    static class Program
    {
        private static System.Threading.Mutex m_instance;
        //private static string m_appName;
        public static FormMain MyFormMain;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool flag;
            GM.ProgramReadSettings();

            string AppDir = Application.StartupPath + "\\";
            AppDir = AppDir.ToUpper();
            string m_appName = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper().Replace(AppDir, "");

            //Application.StartupPath.ToString()
            //m_appName = "Exchange1c";
            m_appName = Application.ExecutablePath.ToString().Replace(Application.StartupPath.ToString() + "\\", "");
            //m_appName = GlobalVars.CurrentSettingsBase.Settings.ExchangePlan + GlobalVars.CurrentSettingsBase.Settings.ThisNodeCode;
            //MessageBox.Show(m_appName);
            //MessageBox.Show("11");
            m_instance = new System.Threading.Mutex(true, m_appName, out flag);
            //MessageBox.Show("22");
            //MessageBox.Show(m_appName);
            if (!flag)
            {
                MessageBox.Show(" Программа уже запущена.");
                return;
            }
            //MessageBox.Show("33");
            Application.EnableVisualStyles();
             //MessageBox.Show("44");
           Application.SetCompatibleTextRenderingDefault(false);
            //MessageBox.Show("55");
            MyFormMain = new FormMain();
            Application.Run(MyFormMain);
            // Application.Run(new FormMain());
        }
        //private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9";
    }
}
