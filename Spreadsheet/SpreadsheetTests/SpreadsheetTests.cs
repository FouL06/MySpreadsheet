///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version: 0.1 - (9/27/21)
/// </summary>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContentsInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("3C");
        }

        [TestMethod]
        public void TestGetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5.0");
            Assert.AreEqual(5.0, s.GetCellContents("A1"));

            s.SetContentsOfCell("A1", "");
            Assert.AreEqual("", s.GetCellContents("A1"));
        }

        [TestMethod]
        public void TestGetNamesOfAllNonEmptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Name");
            s.SetContentsOfCell("A2", "1.0");
            s.SetContentsOfCell("A3", "=2+3");
            List<string> keys = new List<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(keys.Contains("A1"));
            Assert.IsTrue(keys.Contains("A2"));
            Assert.IsTrue(keys.Contains("A3"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsDoubleNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "1.0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsDoubleInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3C", "1.0");
        }

        [TestMethod]
        public void TestSetCellContentsDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.0");
            s.SetContentsOfCell("A1", "5.0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsTextNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsTextInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3C", "Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsTextNull()
        {
            Spreadsheet s = new Spreadsheet();
            string text = null;
            s.SetContentsOfCell("A1", text);
        }

        [TestMethod]
        public void TestSetCellContentsText()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Name");
            s.SetContentsOfCell("A1", "Title");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaNameNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "=10+2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("3C", "=10+2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetCellContentsFormulaNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", null);
        }

        [TestMethod]
        public void TestSetCellContentsFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=5+10");
            s.SetContentsOfCell("A1", "=2+3");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellContentsFormulaCatchCircularExceptionBasic()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A1 + A2");
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetCellContentsFormulaCatchCircularExceptionAdvanced()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("E1", "6");
            s.SetContentsOfCell("E3", "8");
            s.SetContentsOfCell("A1", "=B2 + C3");
            s.SetContentsOfCell("B2", "=E3 * A4");
            s.SetContentsOfCell("C3", "=A4 + 7");
            s.SetContentsOfCell("A4", "=E1 + E3");
            s.SetContentsOfCell("E3", "=B2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValueNameIsNull()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string name = null;
            s.GetCellValue(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValueNameIsNotValid()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string name = "1A_7";
            s.GetCellValue(name);
        }

        [TestMethod]
        public void TestGetCellValue()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10");
            Assert.AreEqual(10.0, s.GetCellValue("A1"));
        }

        [TestMethod]
        public void TestGetCellValueEmpty()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "");
            Assert.AreEqual("", s.GetCellValue("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveFilenameIsNull()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            String filename = null;
            s.Save(filename);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveFilenameIsEmpty()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            String filename = "";
            s.Save(filename);
        }

        [TestMethod]
        public void TestSave()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A2", "1");
            s.SetContentsOfCell("A3", "=A2 + 4");
            Assert.IsTrue(s.Changed);
            s.Save("save.txt");
            Assert.IsFalse(s.Changed);
            s = new Spreadsheet("save.txt", s => true, s => s, "default");
            Assert.AreEqual("Hello", s.GetCellContents("A1"));
            Assert.AreEqual(1.0, s.GetCellContents("A2"));
            Assert.AreEqual(5.0, s.GetCellContents("A3"));
        }

        [TestMethod]
        public void TestSave2()
        {
            AbstractSpreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A2", "1");
            s.SetContentsOfCell("A3", "=A2 + 4");
            Assert.IsTrue(s.Changed);
            s.Save("save1.txt");
            Assert.IsFalse(s.Changed);
            Assert.AreEqual("False", s.Changed.ToString());
            Assert.AreEqual("Hello", s.GetCellContents("A1"));
            Assert.AreEqual(1.0, s.GetCellContents("A2"));
            Assert.AreEqual(5.0, s.GetCellContents("A3"));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSave3()
        {
            AbstractSpreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "v1");
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A2", "1");
            s.SetContentsOfCell("A3", "=A2 + 4");
            Assert.IsTrue(s.Changed);
            s.Save("save1.txt");
            AbstractSpreadsheet s1 = new Spreadsheet("save1.txt", s => true, s => s.ToUpper(), "v2");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestFileConstructorNullFilepath()
        {
            AbstractSpreadsheet s = new Spreadsheet(null, s => true, s => s, "v1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestFileConstructorEmptyFilepath()
        {
            AbstractSpreadsheet s = new Spreadsheet("", s => true, s => s, "v1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestFileConstructorFalseFile()
        {
            AbstractSpreadsheet s = new Spreadsheet("dummy.txt", s => true, s => s, "v1");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestFileConstructorFalseFilepath()
        {
            AbstractSpreadsheet s = new Spreadsheet(s => true, s => s, "v1");
            s.Save("thkjfdshgvjhfdskjhgklfdsjhiklgfjoigf/dummy.txt");
        }
    }
}
