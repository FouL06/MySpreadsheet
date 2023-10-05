using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod()]
        public void TestDefaultConstructorNoErrors()
        {
            Formula f = new Formula("5+3");
            Assert.AreEqual("5+3", f.ToString());
            Assert.AreEqual(8.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorEmptyExpression()
        {
            Formula f = new Formula("");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorBraceExpressionError()
        {
            Formula f = new Formula("())");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorBraceClosingExpressionError()
        {
            Formula f = new Formula("((5)");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorFirstNumberMissingError()
        {
            Formula f = new Formula("+5");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorLastNumberMissingError()
        {
            Formula f = new Formula("10+");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorDoubleOperatorError()
        {
            Formula f = new Formula("10+5/*2");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorMissingOperatorError()
        {
            Formula f = new Formula("8*2 2");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestDefaultConstructorMissingOperatorWithBraceError()
        {
            Formula f = new Formula("(8*2) 2");
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestMultipleVariableConstructorForIsValidVariable()
        {
            Formula f = new Formula("10+a2", s => s, s => (s == "a"));
        }

        [TestMethod()]
        [ExpectedExceptionAttribute(typeof(FormulaFormatException))]
        public void TestMultipleVariableConstructorForIsValidVariableMissingOperator()
        {
            Formula f = new Formula("10 a", s => s, s => (s == "a"));
        }

        [TestMethod()]
        public void TestMultipleVariableConstructorForIsValidVariableNoError()
        {
            Formula f = new Formula("10+a", s => s, s => (s == "a"));
        }

        [TestMethod()]
        public void TestEvaluateThrowDivideByZeroForDouble()
        {
            Formula f = new Formula("10/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
            String s = ((FormulaError)f.Evaluate(s => 0)).Reason;
        }

        [TestMethod()]
        public void TestEvaluateThrowDivideByZeroForVariable()
        {
            Formula f = new Formula("(10*5)/x1");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod()]
        public void TestEvaluateThrowDivideByZeroForBraceHandler()
        {
            Formula f = new Formula("10/(5-5)");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod()]
        public void TestEvaluateThrowDivideByZeroForBraceHandler2()
        {
            Formula f = new Formula("(2/5/0)");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod()]
        public void TestGetVariables()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            HashSet<String> tokens = new HashSet<string>() { "x1", "x2", "x3", "x4", "x5", "x6" };
            List<String> final = new List<String>(f.GetVariables());
            Assert.AreEqual(tokens.Count, final.Count);
        }

        [TestMethod()]
        public void TestHashCodeAndEqualsMethod()
        {
            Formula f1 = new Formula("5*5");
            Formula f2 = new Formula("5*5");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void TestHashCodeAndEqualsMethodFalse()
        {
            Formula f1 = new Formula("5*5");
            Formula f2 = new Formula("5*6");
            Assert.IsFalse(f1.GetHashCode() == f2.GetHashCode());
            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod()]
        public void TestEqualsMethodNull()
        {
            Formula f1 = new Formula("5*5");
            Assert.IsFalse(f1.Equals(null));
        }

        [TestMethod()]
        public void TestEqualsOperatorNull()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
            f1 = null;
            f2 = null;
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod()]
        public void TestNotEqualsOperator()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(new Formula(f1.ToString()) != new Formula(f2.ToString()));
        }

        [TestMethod()]
        public void TestMultipleBraceEvaluation()
        {
            Formula f = new Formula("(5+0) * (1/1) / (1/1)");
            Assert.AreEqual(5.0, f.Evaluate(z => 0));
        }

        [TestMethod()]
        public void TestComplexExpressionEvaluation()
        {
            Formula f = new Formula("y1 * 3 - 8 / 2 + 4 * (8 - 9 * 2) / 14 * x7");
            Assert.AreEqual(5.142857142857142, f.Evaluate(s => (s == "x7") ? 1 : 4));
        }

        [TestMethod()]
        public void TestBasicVariableExpression()
        {
            Formula f = new Formula("x1/x1/2");
            Assert.AreEqual(0.5, f.Evaluate(s => 4));
        }

        [TestMethod()]
        public void TestBasicOperatorExpression()
        {
            Formula f = new Formula("1+2+3");
            Assert.AreEqual(6.0, f.Evaluate(s => 4));
        }

        [TestMethod()]
        public void TestBasicOperatorExpression2()
        {
            Formula f = new Formula("3-2");
            Assert.AreEqual(1.0, f.Evaluate(s => 4));
        }

        [TestMethod()]
        public void TestBasicSingleExpression()
        {
            Formula f = new Formula("3");
            Assert.AreEqual(3.0, f.Evaluate(s => 0));
        }

        [TestMethod()]
        public void TestRepeatedVariableEvaluatorWithSameVariable()
        {
            Formula f = new Formula("x1-x1*x1/x1+(x1)");
            Assert.AreEqual(1.0, (double)f.Evaluate(s => 1));
        }
    }
}
