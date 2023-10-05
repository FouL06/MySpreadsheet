///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version: 0.1 - (10/19/21)
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    class SpreadsheetApplicationContext : ApplicationContext
    {
        //Number of open forms
        private int Form_Count = 0;

        //Singleton ApplicationContext
        private static SpreadsheetApplicationContext Form_Context;

        /// <summary>
        /// Constructor for singleton patterns
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the SpreadsheetApplicationContext
        /// </summary>
        /// <returns></returns>
        public static SpreadsheetApplicationContext GetAppContext()
        {
            if (Form_Context == null)
            {
                Form_Context = new SpreadsheetApplicationContext();
            }
            return Form_Context;
        }

        /// <summary>
        /// Runs the new form
        /// </summary>
        /// <param name="form"></param>
        public void RunForm(Form form)
        {
            //One or more form is running
            Form_Count++;

            //Find out which form closed
            form.FormClosed += (o, e) => { if (--Form_Count <= 0) ExitThread(); };

            //Run the Form
            form.Show();
        }
    }

    /// <summary>
    /// Runs the Spreadsheet Program & GUI
    /// </summary>
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

            //Start new application context and run a new form inside of it
            SpreadsheetApplicationContext Sheet_Context = SpreadsheetApplicationContext.GetAppContext();
            Sheet_Context.RunForm(new Spreadsheet_Form());
            Application.Run(Sheet_Context);
        }
    }
}
