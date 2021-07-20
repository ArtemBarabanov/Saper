using Saper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GameForm form = new GameForm();
            new GamePresenter(form);
            FileRecords FRecords = new FileRecords("Records.txt");
            Records.GetInstance(FRecords);
            Application.Run(form);
        }
    }
}
