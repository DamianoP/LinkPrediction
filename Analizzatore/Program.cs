using System;
using System.Windows.Forms;

namespace Analizzatore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try {
                Application.Run(new Form1());
            }catch(Exception e) {
                System.Diagnostics.Debug.WriteLine("Errore fatale: "+e.Message);
            }
        }
    }
}
