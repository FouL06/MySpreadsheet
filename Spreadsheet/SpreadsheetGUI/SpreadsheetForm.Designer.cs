///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version: 0.1 - (10/19/21)
/// </summary>
namespace SpreadsheetGUI
{
    /// <summary>
    /// Backend data and setting handler for SpreadsheetForm
    /// </summary>
    partial class Spreadsheet_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Spreadsheet_Form));
            this.Cell_Name_Box = new System.Windows.Forms.TextBox();
            this.Cell_Contents_Box = new System.Windows.Forms.TextBox();
            this.Menu_Strip = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editingACellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluateACellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formulasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTextToRedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTextToGreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeTextToBlackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Evaluate_Button = new System.Windows.Forms.Button();
            this.Change_Text_To_Green_Button = new System.Windows.Forms.Button();
            this.Change_Text_To_Red = new System.Windows.Forms.Button();
            this.Black_Text_Color_Button = new System.Windows.Forms.Button();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.Value_Text_Box = new System.Windows.Forms.TextBox();
            this.Menu_Strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Cell_Name_Box
            // 
            resources.ApplyResources(this.Cell_Name_Box, "Cell_Name_Box");
            this.Cell_Name_Box.Name = "Cell_Name_Box";
            this.Cell_Name_Box.ReadOnly = true;
            this.Cell_Name_Box.TabStop = false;
            // 
            // Cell_Contents_Box
            // 
            resources.ApplyResources(this.Cell_Contents_Box, "Cell_Contents_Box");
            this.Cell_Contents_Box.Name = "Cell_Contents_Box";
            this.Cell_Contents_Box.TabStop = false;
            this.Cell_Contents_Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Cell_Contents_Box_KeyDown);
            // 
            // Menu_Strip
            // 
            this.Menu_Strip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.Menu_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.exitToolStripMenuItem1});
            resources.ApplyResources(this.Menu_Strip, "Menu_Strip");
            this.Menu_Strip.Name = "Menu_Strip";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            resources.ApplyResources(this.menuToolStripMenuItem, "menuToolStripMenuItem");
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cellSelectionToolStripMenuItem,
            this.editingACellToolStripMenuItem,
            this.evaluateACellToolStripMenuItem,
            this.errorMessagesToolStripMenuItem,
            this.formulasToolStripMenuItem,
            this.changeTextToRedToolStripMenuItem,
            this.changeTextToGreenToolStripMenuItem,
            this.changeTextToBlackToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // cellSelectionToolStripMenuItem
            // 
            this.cellSelectionToolStripMenuItem.Name = "cellSelectionToolStripMenuItem";
            resources.ApplyResources(this.cellSelectionToolStripMenuItem, "cellSelectionToolStripMenuItem");
            this.cellSelectionToolStripMenuItem.Click += new System.EventHandler(this.cellSelectionToolStripMenuItem_Click);
            // 
            // editingACellToolStripMenuItem
            // 
            this.editingACellToolStripMenuItem.Name = "editingACellToolStripMenuItem";
            resources.ApplyResources(this.editingACellToolStripMenuItem, "editingACellToolStripMenuItem");
            this.editingACellToolStripMenuItem.Click += new System.EventHandler(this.editingACellToolStripMenuItem_Click);
            // 
            // evaluateACellToolStripMenuItem
            // 
            this.evaluateACellToolStripMenuItem.Name = "evaluateACellToolStripMenuItem";
            resources.ApplyResources(this.evaluateACellToolStripMenuItem, "evaluateACellToolStripMenuItem");
            this.evaluateACellToolStripMenuItem.Click += new System.EventHandler(this.evaluateACellToolStripMenuItem_Click);
            // 
            // errorMessagesToolStripMenuItem
            // 
            this.errorMessagesToolStripMenuItem.Name = "errorMessagesToolStripMenuItem";
            resources.ApplyResources(this.errorMessagesToolStripMenuItem, "errorMessagesToolStripMenuItem");
            this.errorMessagesToolStripMenuItem.Click += new System.EventHandler(this.errorMessagesToolStripMenuItem_Click);
            // 
            // formulasToolStripMenuItem
            // 
            this.formulasToolStripMenuItem.Name = "formulasToolStripMenuItem";
            resources.ApplyResources(this.formulasToolStripMenuItem, "formulasToolStripMenuItem");
            this.formulasToolStripMenuItem.Click += new System.EventHandler(this.formulasToolStripMenuItem_Click);
            // 
            // changeTextToRedToolStripMenuItem
            // 
            this.changeTextToRedToolStripMenuItem.Name = "changeTextToRedToolStripMenuItem";
            resources.ApplyResources(this.changeTextToRedToolStripMenuItem, "changeTextToRedToolStripMenuItem");
            this.changeTextToRedToolStripMenuItem.Click += new System.EventHandler(this.changeTextToRedToolStripMenuItem_Click);
            // 
            // changeTextToGreenToolStripMenuItem
            // 
            this.changeTextToGreenToolStripMenuItem.Name = "changeTextToGreenToolStripMenuItem";
            resources.ApplyResources(this.changeTextToGreenToolStripMenuItem, "changeTextToGreenToolStripMenuItem");
            this.changeTextToGreenToolStripMenuItem.Click += new System.EventHandler(this.changeTextToGreenToolStripMenuItem_Click);
            // 
            // changeTextToBlackToolStripMenuItem
            // 
            this.changeTextToBlackToolStripMenuItem.Name = "changeTextToBlackToolStripMenuItem";
            resources.ApplyResources(this.changeTextToBlackToolStripMenuItem, "changeTextToBlackToolStripMenuItem");
            this.changeTextToBlackToolStripMenuItem.Click += new System.EventHandler(this.changeTextToBlackToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            resources.ApplyResources(this.exitToolStripMenuItem1, "exitToolStripMenuItem1");
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // Evaluate_Button
            // 
            this.Evaluate_Button.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.Evaluate_Button, "Evaluate_Button");
            this.Evaluate_Button.Name = "Evaluate_Button";
            this.Evaluate_Button.TabStop = false;
            this.Evaluate_Button.UseVisualStyleBackColor = false;
            this.Evaluate_Button.Click += new System.EventHandler(this.Evaluate_Button_Click);
            // 
            // Change_Text_To_Green_Button
            // 
            this.Change_Text_To_Green_Button.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.Change_Text_To_Green_Button, "Change_Text_To_Green_Button");
            this.Change_Text_To_Green_Button.Name = "Change_Text_To_Green_Button";
            this.Change_Text_To_Green_Button.TabStop = false;
            this.Change_Text_To_Green_Button.UseVisualStyleBackColor = false;
            this.Change_Text_To_Green_Button.Click += new System.EventHandler(this.Change_Text_To_Green_Button_Click);
            // 
            // Change_Text_To_Red
            // 
            this.Change_Text_To_Red.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.Change_Text_To_Red, "Change_Text_To_Red");
            this.Change_Text_To_Red.Name = "Change_Text_To_Red";
            this.Change_Text_To_Red.TabStop = false;
            this.Change_Text_To_Red.UseVisualStyleBackColor = false;
            this.Change_Text_To_Red.Click += new System.EventHandler(this.Change_Text_To_Red_Click);
            // 
            // Black_Text_Color_Button
            // 
            this.Black_Text_Color_Button.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.Black_Text_Color_Button, "Black_Text_Color_Button");
            this.Black_Text_Color_Button.Name = "Black_Text_Color_Button";
            this.Black_Text_Color_Button.TabStop = false;
            this.Black_Text_Color_Button.UseVisualStyleBackColor = false;
            this.Black_Text_Color_Button.Click += new System.EventHandler(this.Black_Text_Color_Button_Click);
            // 
            // spreadsheetPanel1
            // 
            resources.ApplyResources(this.spreadsheetPanel1, "spreadsheetPanel1");
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            // 
            // Value_Text_Box
            // 
            resources.ApplyResources(this.Value_Text_Box, "Value_Text_Box");
            this.Value_Text_Box.Name = "Value_Text_Box";
            this.Value_Text_Box.ReadOnly = true;
            this.Value_Text_Box.TabStop = false;
            // 
            // Spreadsheet_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.Value_Text_Box);
            this.Controls.Add(this.Black_Text_Color_Button);
            this.Controls.Add(this.Change_Text_To_Red);
            this.Controls.Add(this.Change_Text_To_Green_Button);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.Evaluate_Button);
            this.Controls.Add(this.Cell_Contents_Box);
            this.Controls.Add(this.Cell_Name_Box);
            this.Controls.Add(this.Menu_Strip);
            this.Name = "Spreadsheet_Form";
            this.Menu_Strip.ResumeLayout(false);
            this.Menu_Strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel Spreadsheet_Panel;
        private System.Windows.Forms.TextBox Cell_Contents_Box;
        private System.Windows.Forms.MenuStrip Menu_Strip;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Button Evaluate_Button;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ToolStripMenuItem cellSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editingACellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluateACellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorMessagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formulasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeTextToRedToolStripMenuItem;
        private System.Windows.Forms.TextBox Cell_Name_Box;
        private System.Windows.Forms.Button Change_Text_To_Green_Button;
        private System.Windows.Forms.Button Change_Text_To_Red;
        private System.Windows.Forms.ToolStripMenuItem changeTextToGreenToolStripMenuItem;
        private System.Windows.Forms.Button Black_Text_Color_Button;
        private System.Windows.Forms.ToolStripMenuItem changeTextToBlackToolStripMenuItem;
        private System.Windows.Forms.TextBox Value_Text_Box;
    }
}