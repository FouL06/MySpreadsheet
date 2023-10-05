// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

///<summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021
/// Version 1.3 (9/15/21)
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        //Private Data Structures for Formula Tokens
        private HashSet<string> formula_Variables = new HashSet<string>();
        private List<string> formula_Tokens = new List<string>();
        private String formula_Expression = "";

        //Data Structures for Formula Evaluation
        private Stack<double> variables = new Stack<double>();
        private Stack<string> operators = new Stack<string>();

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //Remove whitespace
            formula.Trim();

            //Check if expression is empty
            if (formula == "")
            {
                throw new FormulaFormatException("Expression is either empty or null...");
            }

            //Collect Tokens from Expression
            formula_Tokens = new List<string>(GetTokens(formula));
            StringBuilder sb = new StringBuilder();

            int count = 0;
            string previous_Token = "";

            //Check expression for errors and format exceptions
            foreach (string s in formula_Tokens)
            {
                string token = s.Trim();

                //If token is not empty then continue error checking
                if (s != "")
                {
                    token = normalize(token);

                    //Check if token is an integer or brace
                    if (Double.TryParse(token, out double num))
                    {
                        token = num.ToString();
                    }
                    else if (token == "(")
                    {
                        count++;
                    }
                    else if (token == ")")
                    {
                        count--;

                        //If Count is negative we are missing a brace and should throw an exception
                        if (count < 0)
                        {
                            formula_Variables.Clear();
                            formula_Tokens.Clear();
                            throw new FormulaFormatException("Expression is missing a brace...");
                        }
                    }
                    else if (IsOperator(formula_Tokens.First<string>())) //Check first token to see if valid otherwise throw an error
                    {
                        formula_Variables.Clear();
                        formula_Tokens.Clear();
                        throw new FormulaFormatException("Starting token is not a number, variable, or brace...");
                    }
                    else if (IsOperator(formula_Tokens.Last<string>())) //Check last token to see if valid otherwise throw an error
                    {
                        formula_Variables.Clear();
                        formula_Tokens.Clear();
                        throw new FormulaFormatException("Ending token is not a number, variable, or brace...");
                    }
                    else if (IsOperator(token)) //Check if the token is an operator
                    {
                        //If previous token is not a number, or variable throw an error
                        if (IsOperator(previous_Token))
                        {
                            formula_Variables.Clear();
                            formula_Tokens.Clear();
                            throw new FormulaFormatException("Operator is not followed by a number, or variable...");
                        }
                    }

                    //Check if current expression is missing an operator
                    if (double.TryParse(token, out double num1))
                    {
                        if (double.TryParse(previous_Token, out double num2))
                        {
                            formula_Variables.Clear();
                            formula_Tokens.Clear();
                            throw new FormulaFormatException("Number is not followed by an operator or number...");
                        }

                        if (previous_Token == ")")
                        {
                            formula_Variables.Clear();
                            formula_Tokens.Clear();
                            throw new FormulaFormatException("Missing previous operator...");
                        }
                    }

                    //Check if the token if a valid variable and parse out its data
                    if (!double.TryParse(token, out double result) && !IsOperator(token) && token != "(" && token != ")")
                    {
                        if (isValid(token))
                        {
                            //Check if the previous token is a number if so we are missing an operator
                            if (double.TryParse(previous_Token, out double previous_Result) && previous_Token != ")")
                            {
                                formula_Variables.Clear();
                                formula_Tokens.Clear();
                                throw new FormulaFormatException("Missing previous operator...");
                            }

                            formula_Variables.Add(token);
                        }
                        else //No valid number was able to be parsed from variable
                        {
                            formula_Variables.Clear();
                            formula_Tokens.Clear();
                            throw new FormulaFormatException("Invalid variable or missing data found in expression...");
                        }
                    }

                    //Rebuild expression and set previous token
                    sb.Append(token);
                    previous_Token = token;
                }
            }

            //Check if counter is equal to zero if not we are missing a brace
            if (count != 0)
            {
                formula_Variables.Clear();
                formula_Tokens.Clear();
                throw new FormulaFormatException("Missing a brace in expression...");
            }

            formula_Expression = sb.ToString();
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            //Sort each value into its proper stack and simplify the expression
            foreach (String s in formula_Tokens)
            {
                //Check if s is an integer
                if (double.TryParse(s, out double num))
                {
                    try
                    {
                        IntegerExpressionHelper(num);
                    }
                    catch (Exception e)
                    {
                        variables.Clear();
                        operators.Clear();
                        return new FormulaError("Divide by zero error...");
                    }
                }
                else if (s.Equals("+") || s.Equals("-"))
                {
                    OperatorExpressionHelper(s);
                }
                else if (s.Equals("*") || s.Equals("/"))
                {
                    operators.Push(s);
                }
                else if (s.Equals("("))
                {
                    operators.Push(s);
                }
                else if (s.Equals(")"))
                {
                    try
                    {
                        BraceExpressionHelper(s);
                    }
                    catch (Exception e)
                    {
                        variables.Clear();
                        operators.Clear();
                        return new FormulaError("Divide by Zero Error...");
                    }
                }
                else //s is a variable
                {
                    try
                    {
                        VariableExpressionHelper(lookup(s));
                    }
                    catch (Exception e)
                    {
                        variables.Clear();
                        operators.Clear();
                        return new FormulaError("Divide by zero error, or variable is undefined...");
                    }
                }
            }

            double result = 0.0;

            //Check if there is one more expression left to evaluate
            if (variables.Count == 2 && operators.Count == 1)
            {
                double var1 = variables.Pop();
                double var2 = variables.Pop();
                String op1 = operators.Pop();

                if (op1.Equals("+"))
                {
                    result = var1 + var2;
                }
                else if (op1.Equals("-"))
                {
                    result = var2 - var1;
                }
            }
            else
            {
                result = variables.Pop();
            }

            return result;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return new List<string>(formula_Variables);
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return this.formula_Expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            //Check if obj is null or not a formula
            if (obj is null || !(obj is Formula))
            {
                return false;
            }

            //Check the formula and obj to see if equal
            Formula tempFormula = obj as Formula;

            if (this.formula_Expression.Equals(tempFormula.formula_Expression))
            {
                return true;
            }

            //If none of these pass just return false
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //Check if either are null
            if (f1 is null)
            {
                if (f2 is null)
                {
                    return true;
                }
                return false;
            }
            else //If neither are null check equality
            {
                return f1.Equals(f2);
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return (formula_Expression.GetHashCode());
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Helper method designed for when the expression contains an integer value. 
        /// If a value is not found it will push it to the variables stack, 
        /// if an operator is avaiable it will compute the expression with the popped operator from the stack.
        /// </summary>
        /// <param name="num"></param>
        private void IntegerExpressionHelper(double num)
        {
            //check if op stack is empty
            if (operators.Count != 0)
            {
                if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                {
                    //Pop variables and operators once
                    double var1 = variables.Pop();
                    String op1 = operators.Pop();

                    //Apply operator and push result to variables stack
                    if (op1.Equals("*"))
                    {
                        double result = var1 * num;
                        variables.Push(result);
                    }
                    else
                    {
                        //Check for divide by zero
                        if (num == 0)
                        {
                            throw new Exception("Division by zero error...");
                        }

                        double result = var1 / num;
                        variables.Push(result);
                    }
                }
                else //if neither of them pass just push the variable onto the stack
                {
                    variables.Push(num);
                }
            }
            else //if nothing exsists on the stack push it onto the stack
            {
                variables.Push(num);
            }
        }

        /// <summary>
        /// Helper method designed for when the current substring is an operator of "+" or "-", 
        /// taking such operator and preform a computation if allowed. 
        /// Otherwise will push the operator to the operator stack.
        /// </summary>
        /// <param name="s"></param>
        private void OperatorExpressionHelper(string s)
        {
            if (operators.Count != 0)
            {
                if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                {
                    //Pop the variables stack and operators stack for expression
                    double var1 = variables.Pop();
                    double var2 = variables.Pop();
                    String op1 = operators.Pop();

                    if (op1.Equals("+"))
                    {
                        double result = var1 + var2;
                        variables.Push(result);
                    }
                    else
                    {
                        double result = var2 - var1;
                        variables.Push(result);
                    }

                    //push operator onto the stack
                    operators.Push(s);
                }
                else
                {
                    operators.Push(s);
                }
            }
            else
            {
                operators.Push(s);
            }
        }

        /// <summary>
        /// Helper method designed for when the current substring is a closing brace ")" 
        /// which will preform the proper calculation of the variables contained inside the braces. 
        /// Pushing the final result to the variables stack.
        /// </summary>
        /// <param name="s"></param>
        private void BraceExpressionHelper(string s)
        {
            if (operators.Count != 0)
            {
                if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                {
                    //Pop variables stack and operators stack for expresssion
                    double var1 = variables.Pop();
                    double var2 = variables.Pop();
                    String op1 = operators.Pop();

                    if (op1.Equals("+"))
                    {
                        double result = var1 + var2;
                        variables.Push(result);
                    }
                    else
                    {
                        double result = var2 - var1;
                        variables.Push(result);
                    }
                }

                //Remove the opening brace if found, then preform Order of Operation if needed
                if (operators.Count != 0)
                {
                    if (operators.Peek().Equals("("))
                    {
                        operators.Pop();

                        //Check if an operator preceeds the brace
                        if (operators.Count != 0)
                        {
                            if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                            {
                                double var1 = variables.Pop();
                                double var2 = variables.Pop();
                                String op1 = operators.Pop();

                                if (op1.Equals("*"))
                                {
                                    double result = var1 * var2;
                                    variables.Push(result);
                                }
                                else
                                {
                                    if (var1 == 0)
                                    {
                                        throw new Exception("Divide by zero error...");
                                    }
                                    double result = var2 / var1;
                                    variables.Push(result);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method designed for evalutating the current substring if its a variables, 
        /// making use of the variablesEvaluator to grab the contents of the cell and preform the calculation with the current operators.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="variableEvaluator"></param>
        private void VariableExpressionHelper(double num)
        {
            if (operators.Count != 0)
            {
                if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                {
                    //Pop variables and operators once
                    double var1 = variables.Pop();
                    double var2 = num;
                    String op1 = operators.Pop();

                    //Apply operator and push result to variables stack
                    if (op1.Equals("*"))
                    {
                        double result = var1 * var2;
                        variables.Push(result);
                    }
                    else
                    {
                        //Check for divide by zero
                        if (var2 == 0)
                        {
                            throw new Exception("Division by zero error...");
                        }

                        double result = var1 / var2;
                        variables.Push(result);
                    }
                }
                else //if no conditions apply push the number onto the stack
                {
                    variables.Push(num);
                }
            }
            else //if nothing exsists on the stack push the number onto the stack
            {
                variables.Push(num);
            }
        }

        /// <summary>
        /// Checks if the string being passed in is a valid operator returning a boolean.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool IsOperator(string s)
        {
            if (s == "+" || s == "-" || s == "*" || s == "/")
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}