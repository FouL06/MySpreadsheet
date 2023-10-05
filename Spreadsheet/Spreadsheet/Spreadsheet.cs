///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version: 0.1 - (9/19/21)
/// 
/// Version: 0.2 - (9/26/21) : Implemented Skeleton Code and Cell Class
/// 
/// Version 0.3 - (9/27/21) : Added saving and writing of XML files to Spreadsheet
/// </summary>

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    /// <summary>
    /// Represents the Spreadsheet Object is the MVC architecture.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //Private Data Structures for Spreadsheet
        private DependencyGraph dg;
        private Dictionary<string, Cell> cells;

        /// <summary>
        ///  True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Default Constructor for Spreadsheet to create and initilize cells.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            dg = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            this.Changed = false;
        }

        /// <summary>
        /// Constructor for Spreadsheet, with the addition of allowing the user to pass in a validity delgate for valid variables.
        /// </summary>
        /// <param name="isValid">boolean</param>
        /// <param name="normalize">normalize variable</param>
        /// <param name="version">file version</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            dg = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
            this.Changed = false;
        }

        /// <summary>
        /// Constructor for Spreadsheet, with the addition of allowing the user to pass in a file,
        /// validity delgate, and string normilization delgate.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            dg = new DependencyGraph();
            cells = new Dictionary<string, Cell>();

            //Check if the file being passed in is the same version
            if (!GetSavedVersion(filepath).Equals(version))
            {
                throw new SpreadsheetReadWriteException("File versions do not match, or are missing changes...");
            }

            Read_XML_File(filepath, false);
            this.Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>cell contents</returns>
        public override object GetCellContents(string name)
        {
            //Check if cell is null or not a valid cell name
            if (name is null || !(IsValidCellName(name)))
            {
                throw new InvalidNameException();
            }

            //Normalize the name based on constructor
            name = this.Normalize(name);

            //Use TryGetValue to get contents of Cell
            if (cells.TryGetValue(name, out Cell c))
            {
                if (c.contents is Formula)
                {
                    return "=" + c.contents;
                }
                return c.contents;
            }
            else
            {
                return ""; //Special case for if the cell contains and empty string
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            //Check if cell is null or not a valid cell name
            if (name is null || !(IsValidCellName(name)))
            {
                throw new InvalidNameException();
            }

            //Normalize name based on constructor
            name = this.Normalize(name);

            //Get the contents of the cell
            if (cells.TryGetValue(name, out Cell cell))
            {
                return cell.value;
            }
            else
            {
                return ""; //Return empty if no value was found
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">name of file</param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            return Read_XML_File(filename, true);
        }

        public override void Save(string filename)
        {
            //Check if filename is null
            if (filename is null)
            {
                throw new SpreadsheetReadWriteException("Filename cannot be null...");
            }

            //Check if filename is empty
            if (filename == "")
            {
                throw new SpreadsheetReadWriteException("Filename cannot be empty...");
            }

            //Save the document
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();

                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    //Start XML document with opening tag and version
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", null, Version);

                    //Write all current cells into the xml document and parse in their contents
                    foreach (string cell in cells.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell);

                        //Cell Contents String
                        string Cell_Contents;

                        //Cell is contents are a double, string, or formula write to document
                        if (cells[cell].contents is double)
                        {
                            Cell_Contents = cells[cell].contents.ToString();
                        }
                        else if (cells[cell].contents is Formula)
                        {
                            Cell_Contents = "=" + cells[cell].contents.ToString();
                        }
                        else
                        {
                            Cell_Contents = cells[cell].contents.ToString();
                        }

                        //Write cell contents to the file
                        writer.WriteElementString("contents", Cell_Contents);
                        writer.WriteEndElement();
                    }

                    //Write ending spreadsheet tag and close document
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }

            //Set changed to false after a save
            this.Changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <param name="number">cell contents</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, double number)
        {
            //Create cell if it does not exsist within the spreadsheet
            Cell cell = new Cell(number);

            //Normalize name based on spreadsheet constructor
            name = this.Normalize(name);

            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }

            //Clear the current cell set to allow the spreadsheet to update with any changes made to its dependees list
            dg.ReplaceDependees(name, new HashSet<string>());

            //Checks to see if any cells changed an recalculates the dependees list
            List<string> All_Dependees_List = new List<string>(GetCellsToRecalculate(name));
            return All_Dependees_List;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <param name="text">cell contents</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            //Initialize cell
            Cell cell;

            //Check for if the cells contents contains the beginning of a formula
            if (text != "" && text.Substring(0, 1) == "=")
            {
                Formula f = new Formula(text.Substring(1), Normalize, IsValid);
                cell = new Cell(f, CellLookupContents);
            }
            else
            {
                cell = new Cell(text);
            }

            //Normalize name based on spreadsheet constructor
            name = this.Normalize(name);

            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }

            //Check for special case of cell is valid but contains an empty string
            if (cells[name].contents.Equals(""))
            {
                cells.Remove(name);
            }

            //Clear current cell set to allow the spreadsheet to update any changes made to its dependees list
            dg.ReplaceDependees(name, new HashSet<string>());

            //Checks to see if any cells changed and recalcualtes the dependees list
            List<string> All_Dependees_List = new List<string>(GetCellsToRecalculate(name));
            return All_Dependees_List;
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <param name="formula">cell formula</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            //Normalize name based on constructor
            name = this.Normalize(name);

            //Store current dependees in a temporary list for error checking
            IEnumerable<string> Temporary_Dependees_List = dg.GetDependees(name);

            //Replace dependees of the current cell that is being found using the formula
            dg.ReplaceDependees(name, formula.GetVariables());

            //Check if cell has a circular exception within the spreadsheet if so throw an exception
            try
            {
                //Create a new cell and create dependees list found for the cell
                List<string> All_Dependees_List = new List<string>(GetCellsToRecalculate(name));
                Cell cell = new Cell(formula, CellLookupContents);

                //Check if cell object already exsists if not create a new cell
                if (cells.ContainsKey(name))
                {
                    cells[name] = cell;
                }
                else
                {
                    cells.Add(name, cell);
                }

                return All_Dependees_List;
            }
            catch (CircularException e)
            {
                //if a circular exception is found keep the old dependees so that the cells graph is not modified
                dg.ReplaceDependees(name, Temporary_Dependees_List);
                throw new CircularException();
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <param name="content">cell contents</param>
        /// <returns></returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            //Check if name is null or not a vail cell name
            if (name is null || !(IsValidCellName(name)))
            {
                throw new InvalidNameException();
            }

            //Check if contents of the cell in null
            if (content is null)
            {
                throw new InvalidNameException();
            }

            //Normalize name based on constructor
            name = this.Normalize(name);

            //Initialize dependees list for later cell setting
            List<string> All_Dependees_List;

            //Check if cells contents are empty, double, text, or a formula
            if (content == "")
            {
                All_Dependees_List = new List<string>(SetCellContents(name, content));
            }
            else if (double.TryParse(content, out double num))
            {
                All_Dependees_List = new List<string>(SetCellContents(name, num));
            }
            else if (content.Substring(0, 1) == "=")
            {
                //Get the formula after the "="
                string Formula_String = this.Normalize(content.Substring(1));

                //Check to see if the formula is valid and does not contain a circular exception
                Formula f = new Formula(Formula_String, Normalize, IsValid);

                All_Dependees_List = new List<string>(SetCellContents(name, f));
            }
            else
            {
                All_Dependees_List = new List<string>(SetCellContents(name, content));
            }

            //Spreadsheet has been changed update bool
            this.Changed = true;

            //Check if cell is dependees have changed if so re-evaluate to keep dependencies updated
            foreach (string Cell_Dependee in All_Dependees_List)
            {
                if (cells.TryGetValue(Cell_Dependee, out Cell cell))
                {
                    cell.ReEvaluateContents(CellLookupContents);
                }
            }

            //Return Dependees List for Spreadsheet to use if no errors
            return All_Dependees_List;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dg.GetDependents(name);
        }

        /// <summary>
        /// Check if cell object is name is valid so data can be parsed from cell.
        /// </summary>
        /// <param name="name">cell name</param>
        /// <returns>boolean</returns>
        private bool IsValidCellName(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z]+[\d]+$", RegexOptions.Singleline) && IsValid(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Finds the cell within the spreadsheet and returns its value of the cell,
        /// to which the cell can then be evaluated by the formula class.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private double CellLookupContents(string s)
        {
            //Check if the cell contains a double
            if (cells.TryGetValue(s, out Cell cell))
            {
                //If the cell is contents are a double return it
                if (cell.value is double)
                {
                    return (double)cell.value;
                }
                else
                {
                    throw new ArgumentException("Cell does not contain a double value...");
                }
            }
            else
            {
                throw new ArgumentException("Spreadsheet does not contain " + s);
            }
        }

        /// <summary>
        /// Reads an XML based on the filepath given only if the file is of the correct version.
        /// Checking all XML tags within the file created by the save method.
        /// </summary>
        /// <param name="filename">string for filename</param>
        /// <param name="version">file is version</param>
        /// <returns></returns>
        private string Read_XML_File(string filename, bool version)
        {
            //Check if filename is null
            if (filename is null)
            {
                throw new SpreadsheetReadWriteException("Filename was not found...");
            }

            //Check if filename is empty
            if (filename == "")
            {
                throw new SpreadsheetReadWriteException("Filename was not found...");
            }

            //Read File if it was found and is of the correct type
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    string name = ""; //Cell Name
                    string contents = ""; //Cell Contents

                    //Read the file and check for any errors
                    while (reader.Read())
                    {
                        //Find opening XML Tag of spreadsheet
                        if (reader.IsStartElement())
                        {
                            bool Missing_Contents = false; //check if cell is missing contents or empty

                            //Check for the various xml tags and preforms checks
                            switch (reader.Name)
                            {
                                case "spreadsheet": //Check spreadsheet tag and its file version
                                    if (version)
                                    {
                                        return reader["version"];
                                    }
                                    else
                                    {
                                        Version = reader["version"];
                                    }
                                    break;
                                case "cell": //Check cell tag and its version
                                    break;
                                case "name": //Check cells name tag and version
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents": //Check cells contents and preform version check
                                    reader.Read();
                                    contents = reader.Value;
                                    Missing_Contents = true;
                                    break;
                            }

                            //Check if cell has missing contents if so then set the cell contents
                            if (Missing_Contents)
                            {
                                SetCellContents(name, contents);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }

            return Version;
        }

        /// <summary>
        /// An Cell object used to represent individual cell objects created by this class,
        /// for the purpose of storing and maintaining data for individual cells in the spreadsheet.
        /// </summary>
        private class Cell
        {
            //Cell Contents Object
            public object contents;
            public object value;

            /// <summary>
            /// Constructor for when a string is passed into the Cell object.
            /// </summary>
            /// <param name="s"></param>
            public Cell(string s)
            {
                if (double.TryParse(s, out double num))
                {
                    contents = num;
                    value = contents;
                }
                else
                {
                    contents = s;
                    value = contents;
                }
            }

            /// <summary>
            /// Constructor for when a double is passed into the Cell object.
            /// </summary>
            /// <param name="num"></param>
            public Cell(double num)
            {
                contents = num;
                value = contents;
            }

            /// <summary>
            /// Constructor for when a Formula is passed into the Cell object.
            /// </summary>
            /// <param name="f"></param>
            public Cell(Formula f, Func<string, double> lookup)
            {
                contents = f;
                value = f.Evaluate(lookup);
            }

            /// <summary>
            /// Contents are protected by scope, method will check if the type is of Formula for the cell,
            /// checking if the cell contains a circular dependancy within the cells call,
            /// allowing us to evaluate the cells contents as they are passed in.
            /// </summary>
            /// <param name="lookup"></param>
            public void ReEvaluateContents(Func<string, double> lookup)
            {
                if (contents is Formula)
                {
                    Formula f = (Formula)contents;
                    value = f.Evaluate(lookup);
                }
            }
        }
    }
}