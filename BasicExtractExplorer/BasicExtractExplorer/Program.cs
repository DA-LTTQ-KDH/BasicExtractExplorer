using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicExtractExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                if(args.Length == 0)
                    Application.Run(new MainForm());
                else
                {
                    if (args[0] == "compress")
                    {
                        List<string> ls = new List<string>();
                        ls.Add(args[1]);
                        AddToArchive addToArchive = new AddToArchive(ls);
                        Application.Run(addToArchive);
                    }
                    else if(args[0] == "extract")
                    {

                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
