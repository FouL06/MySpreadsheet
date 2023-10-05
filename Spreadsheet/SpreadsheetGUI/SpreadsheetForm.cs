///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version: 0.1 - (10/19/21)
/// </summary>

using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Represents the Spreadsheet GUI by making use of a microsoft forms component.
    /// Allowing for the access of files, file properties, and spreadsheet editing.
    /// </summary>
    public partial class Spreadsheet_Form : Form
    {
        //Private Variables
        private Spreadsheet spreadsheet;
        private string filename;

        /// <summary>
        /// Default constructor to initialize the spreadsheet and set default properties
        /// </summary>
        public Spreadsheet_Form()
        {
            InitializeComponent();
            spreadsheet = new Spreadsheet(isValid, s => s.ToUpper(), "PS6");
            filename = null;
            Update_Spreadsheet();
            spreadsheetPanel1.SelectionChanged += Cell_Selected;
        }

        /// <summary>
        /// Variation of Spreadsheet Constructor that allows the user to input a filepath for opening an existing Spreadsheet Form.
        /// </summary>
        /// <param name="filepath"></param>
        public Spreadsheet_Form(string filepath)
        {
            InitializeComponent();
            spreadsheet = new Spreadsheet(filepath, isValid, s => s.ToUpper(), "PS6");
            filename = null;
            Update_Spreadsheet();
            spreadsheetPanel1.SelectionChanged += Cell_Selected;
        }

        /// <summary>
        /// Updates the spreadsheet when a cell is edited in the panel, 
        /// updating any cell with its proper contents and cell value.
        /// will only update when a new cell is selected or cell is evaluated.
        /// </summary>
        private void Update_Spreadsheet()
        {
            //Get all cell data from currently selected cell
            string Cell_Name = Get_Cell_Name();
            string Cell_Contents_Text = Cell_Contents_Box.Text;
            object Cell_Contents = spreadsheet.GetCellContents(Cell_Name);
            object Cell_Value = spreadsheet.GetCellValue(Cell_Name);

            //Updates selection focus & updates value textbox
            Cell_Name_Box.Text = Cell_Name;
            Cell_Contents_Box.Focus();

            //Updates cells with inputed contents
            if (Cell_Contents_Text != "" && Cell_Contents_Text.Substring(0, 1) == "=")
            {
                Cell_Contents_Box.Text = Cell_Contents.ToString();
                Value_Text_Box.Text = Cell_Value.ToString();
            }
            else
            {
                Cell_Contents_Box.Text = Cell_Contents.ToString();
                Value_Text_Box.Text = Cell_Value.ToString();
            }

            //Checks for Formula Errors
            if (spreadsheet.GetCellValue(Cell_Name) is FormulaError)
            {
                FormulaError error = (FormulaError)spreadsheet.GetCellValue(Cell_Name);
                spreadsheetPanel1.ChangeTextColorErrorOccured(Get_Cell_Col(Cell_Name), Get_Cell_Row(Cell_Name), error.Reason.ToString());
                Cell_Contents_Box.Text = error.Reason.ToString();
            }
            else
            {
                Cell_Contents_Box.Text = Cell_Contents.ToString();
                Value_Text_Box.Text = Cell_Value.ToString();
            }
        }

        /// <summary>
        /// Checks to see if currently selected cell is a valid cell within our spreadsheet constraints.
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private bool isValid(string cellName)
        {
            //Check if cell name is within the A-Z and 99 index limit
            if (Regex.IsMatch(cellName, @"^[a-zA-Z][1-9][0-9]?$"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the spreadsheet with each cell clicked.
        /// </summary>
        /// <param name="s"></param>
        private void Cell_Selected(SpreadsheetPanel panel)
        {
            Update_Spreadsheet();
        }

        /// <summary>
        /// Updates cell dependents with proper data in preperation for file saving or overwriting.
        /// </summary>
        /// <param name="name"></param>
        private void Update_Cell(string name)
        {
            //Cell Data
            int Cell_Col = Get_Cell_Col(name);
            int Cell_Row = Get_Cell_Row(name);
            string Cell_Value;

            //Check cell for any formula errors
            if ((spreadsheet.GetCellValue(name) is FormulaError))
            {
                //Update Cell with Error Message
                FormulaError error = (FormulaError)spreadsheet.GetCellValue(name);
                Cell_Value = error.Reason.ToString();
                spreadsheetPanel1.ChangeTextColorErrorOccured(Cell_Col, Cell_Row, error.Reason.ToString());
            }
            else
            {
                Cell_Value = spreadsheet.GetCellValue(name).ToString();
                Value_Text_Box.Text = Cell_Value.ToString();
            }

            //Update the cells value
            spreadsheetPanel1.SetValue(Cell_Col, Cell_Row, Cell_Value);
        }

        /// <summary>
        /// Gets cell name of currently selected cell.
        /// </summary>
        /// <returns></returns>
        private String Get_Cell_Name()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            int Cell_Row = ++row;
            char Cell_Col = (char)('A' + col);
            return "" + Cell_Col + Cell_Row;
        }

        /// <summary>
        /// Gets the column index of a Cell.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int Get_Cell_Col(string name)
        {
            int col = name.ToCharArray(0, 1)[0] - 'A';
            return col;
        }

        /// <summary>
        /// Gets the row index of the Cell.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int Get_Cell_Row(string name)
        {
            int.TryParse(name.Substring(1), out int row);
            row--;
            return row;
        }

        /// <summary>
        /// Displays a Message Box with the given error or message allowing the user to exit the popup if needed.
        /// </summary>
        /// <param name="message"></param>
        private void Display_Message_Box(string message)
        {
            string caption = "Documentation:";
            MessageBoxButtons ok = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, caption, ok);
        }

        /// <summary>
        /// Saves current spreadsheet after click event prompting user for a filename and option to cancel saving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Setup save dialog prompt
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";
            save.FilterIndex = 2;
            save.RestoreDirectory = true;

            //Return if user cancels saving
            if (save.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            //Write to xml file and change text of current window
            spreadsheet.Save(save.FileName);
            this.Text = save.FileName;
        }

        /// <summary>
        /// Open Spreadsheet from file directory and does a check for any unsaved data on current Spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog Open_File = new OpenFileDialog())
            {
                //Open File Settings
                Open_File.Filter = "sprd files (*.sprd)|*.sprd|All files (*.*)|*.*";
                Open_File.DefaultExt = ".sprd";
                Open_File.FilterIndex = 2;
                Open_File.RestoreDirectory = true;

                if (Open_File.ShowDialog() == DialogResult.OK)
                {
                    //Get Filepath
                    string filepath = Open_File.FileName;

                    //Check if filepath is empty
                    if (filepath == "")
                    {
                        return;
                    }

                    //Default to .sprd incase the user chooses a diffrent file extension
                    if (Open_File.FilterIndex == 1)
                    {
                        Open_File.AddExtension = true;
                    }

                    //Check if Spreadsheet has unsaved changes
                    if (spreadsheet.Changed)
                    {
                        string caption = "You have unsaved changes.";
                        string message = "Would you like to save your changes now?";
                        MessageBoxButtons button = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show(message, caption, button);

                        //Save if yes is selected
                        if (result == DialogResult.Yes)
                        {
                            saveToolStripMenuItem_Click(sender, e);
                        }
                    }

                    //Read the contents of the file into spreadsheet
                    try
                    {
                        Spreadsheet_Form form = new Spreadsheet_Form(filepath);

                        //Grab all cells in file
                        foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                        {
                            int Cell_Col = Get_Cell_Col(s);
                            int Cell_Row = Get_Cell_Row(s);
                            spreadsheetPanel1.SetValue(Cell_Col, Cell_Row, "");
                        }

                        //Update spreadsheet with passed in data & update filepath
                        spreadsheet = new Spreadsheet(filepath, isValid, s => s.ToUpper(), "PS6");
                        filename = filepath;
                        this.Text = filepath;
                        Update_Spreadsheet();

                        //Update Cells in spreadsheetPanel
                        foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
                        {
                            Update_Cell(s);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Prompt user of error reading file
                        string title = "Error Reading File...";
                        string description = "Error reading file, please verify proper spreadsheet file.";
                        MessageBox.Show(description, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Update A1 cell with error message
                        spreadsheetPanel1.ChangeTextColorErrorOccured(1, 1, description);
                        Cell_Contents_Box.Text = description;
                    }
                }
            }
        }

        /// <summary>
        /// Creates new Spreadsheet Form window from the click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.GetAppContext().RunForm(new Spreadsheet_Form());
        }

        /// <summary>
        /// Exits Spreadsheet form after preforming a safty check for any unsaved data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Check if the spradsheet has unsaved data
            if (spreadsheet.Changed == true)
            {
                string caption = "There is unsaved changes in the spreadsheet.";
                string message = "Would you like to save any changes before closing?";
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, button);

                //Save changes if yes
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Handles click event on evaluate button. 
        /// Evaluating the contents stored in the spreadsheet and passing the value to the selected cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Evaluate_Button_Click(object sender, EventArgs e)
        {
            //Variables for Cell Data
            IList<string> dependents = null;
            string Cell_Name = Get_Cell_Name();
            int Cell_Col = Get_Cell_Col(Cell_Name);
            int Cell_Row = Get_Cell_Row(Cell_Name);

            //Place data in cell unless an error is encountered
            try
            {
                dependents = spreadsheet.SetContentsOfCell(Cell_Name_Box.Text, Cell_Contents_Box.Text);
            }
            catch (Exception ex) //Catch any exception
            {
                string error = "Invalid expression for cell...";
                spreadsheetPanel1.ChangeTextColorErrorOccured(Cell_Col, Cell_Row, error);
                Cell_Contents_Box.Text = error;
                MessageBox.Show("Error Occurred:", error);
            }

            //Update Spreadsheet and its dependents
            Update_Spreadsheet();
            foreach (string dep in dependents)
            {
                Update_Cell(dep);
            }
        }

        /// <summary>
        /// Handles click event on Green Text button, allowing the text of the cell to be changed to green.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Text_To_Green_Button_Click(object sender, EventArgs e)
        {
            //Cell Name for parsing
            string Cell_Value = spreadsheet.GetCellValue(Get_Cell_Name()).ToString();

            spreadsheetPanel1.GetSelection(out int col, out int row);
            spreadsheetPanel1.ChangeTextColor(col, row, Cell_Value);
        }

        /// <summary>
        /// Handles click event for Red Text button, allowing the text of the cell to be changed to red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Text_To_Red_Click(object sender, EventArgs e)
        {
            //Cell Name for parsing
            string Cell_Value = spreadsheet.GetCellValue(Get_Cell_Name()).ToString();

            spreadsheetPanel1.GetSelection(out int col, out int row);
            spreadsheetPanel1.ChangeTextColorErrorOccured(col, row, Cell_Value);
        }

        /// <summary>
        /// Handles click event to Black Text button, allowing the text of the cell to be changed back to black.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Black_Text_Color_Button_Click(object sender, EventArgs e)
        {
            //Cell Name for parsing
            string Cell_Value = spreadsheet.GetCellValue(Get_Cell_Name()).ToString();

            spreadsheetPanel1.GetSelection(out int col, out int row);
            spreadsheetPanel1.ChangeTextColorToDefault(col, row, Cell_Value);
        }

        /// <summary>
        /// Updates the cell upon pressing the enter key while editing in the Contents Textbox.
        /// Moving onto the next cell in the column and allowing the user to 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_Contents_Box_KeyDown(object sender, KeyEventArgs e)
        {
            //Event Handler for Enter Key Pressed
            if (e.KeyCode.Equals(Keys.Enter))
            {
                Evaluate_Button_Click(sender, e);
                spreadsheetPanel1.GetSelection(out int col, out int row);
                spreadsheetPanel1.SetSelection(col, ++row);
                Cell_Contents_Box.Text = "";
                Update_Spreadsheet();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Dialog Prompt documentation on how to select a cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cellSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Option 1: Left-click the desired cell in the spreadsheet, allowing you to edit the cells contents \n \n" +
                "Option 2: Pressing the Enter key will move the cell selection down by one in the same column as the already selected cell, " +
                "allowing for editing of the cell.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog Prompt documenatation on how to edit a cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editingACellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "To edit a cell click the cell context box which is the large textbox underneath the menu," +
                " this allows for the editing of the cells contents in the spreadsheet.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog Prompt documenatation on how to evaluate a cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evaluateACellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Option 1: To evaluate a cell enter any desired text, formula, or number into the context box and press enter" +
                " evaluating the cells value and selecting the next cell. \n \n" +
                "Option 2: Click the evaluate button which will repeate the process in from option one.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog prompt documenation on when errors occur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void errorMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Invalid Expression: Data passed into the cell could not be evaluated or expression is not valid. \n \n" +
                "Circular Exception: Circular dependency on the cells was detected, can cause issues with evaluation of cell data. \n \n" +
                "Formula Error: Error computing expression or formula passed into the cell, and could not retreive data.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog prompt documentation on how to create a formula
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formulasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "To create a formula place an '=' sign before adding number, cells, or any arithmic operation to create a formula, " +
                "which will allow for the spreadsheet to evaluate and compute the formula to a single value. \n \n" +
                "Example: =A1+A2, or =A1*(20+3)";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog prompt documentation on how to make the cell text red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeTextToRedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Designed for showing the user that an error occured at a specific cell by changing the text to red." +
                " The user can also modify the text color of a cell to red by pressing the Red Text button.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog prompt documentation on how to make the cell text green.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeTextToGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "To change the color of the text to green for a specific cell," +
                " make sure that the cell you want to edit is selected and click the Green Text button.";
            Display_Message_Box(message);
        }

        /// <summary>
        /// Dialog prompt documentation on how to make the cell text black.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeTextToBlackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "To change the color of the text to black for a specific cell," +
                " make sure that the cell you want to edit is selected and click the Black Text button.";
            Display_Message_Box(message);
        }
    }
}