using System;
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
            if (!GlobalVars.ExeDir.Contains("Debug"))  if (!GlobalVars.ExeDir.Contains("Release")) GM.CheckUpdate();

            string m_appName = GlobalVars.ExeName;

            m_appName = m_appName.Replace(".exe", "");
            m_instance = new System.Threading.Mutex(true, m_appName, out flag);
            if (!flag)
            {
                MessageBox.Show(" Программа уже запущена.");
                return;
            }

            //MessageBox.Show(GlobalVars.ExeDir);
            //MessageBox.Show(GlobalVars.ExeName);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MyFormMain = new FormMain();
            Application.Run(MyFormMain);
        }
        //private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9";
    }
}
