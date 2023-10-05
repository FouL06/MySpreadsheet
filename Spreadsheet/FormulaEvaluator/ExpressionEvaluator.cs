/// <summary>
/// Author: Ashton Foulger, CS 3500 - 001 Fall 2021, Aug 30th 2021
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Allows for the computation of complex expressions once passed in and produces a final integer result for the user to use.
/// </summary>
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        //Variable lookup delegate
        public delegate int Lookup(String v);

        //Variable and Operator Stacks
        public static Stack<int> variables = new Stack<int>();
        public static Stack<String> operators = new Stack<String>();

        /// <summary>
        /// Evaluates the given expression by spliting the expression into substrings based on whether they are operators or variables,
        /// making use of the variableEvaluator delegate to grab and return an int from the Variable Cell.
        /// </summary>
        /// <param name="exp">Expression text passed in to be evaluated</param>
        /// <param name="variableEvaluator">Delegate to determin the value of the variable if found in the expression</param>
        /// <returns>Integer value of final result from given expression</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //Check if the expression is an empty string
            if (exp.Equals(""))
            {
                throw new ArgumentException("expression is empty...");
            }

            //Trim whitespace from expression
            exp = String.Concat(exp.Where(c => !Char.IsWhiteSpace(c)));

            //Split expression into tokens
            String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //Sort each value into its proper stack and simplify the expression
            foreach (String s in substrings)
            {
                //Check for blank or empty strings
                if(s == "" || s == " ")
                {
                    continue;
                }

                //Check if s is an integer
                if (int.TryParse(s, out int num))
                {
                    IntegerExpressionHelper(num);
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
                    BraceExpressionHelper(s);
                }
                else //s is a variable
                {
                    VariableExpressionHelper(s, variableEvaluator);
                }
            }

            int result;

            //Check if there is one more expression left to evaluate
            if(variables.Count == 2 && operators.Count == 1)
            {
                int var1 = variables.Pop();
                int var2 = variables.Pop();
                String op1 = operators.Pop();

                if (op1.Equals("+"))
                {
                    result = var1 + var2;
                    return result;
                }
                else if (op1.Equals("-"))
                {
                    result = var2 - var1;
                    return result;
                }
                else if (op1.Equals("*"))
                {
                    result = var1 * var2;
                    return result;
                }
                else
                {
                    result = var2 / var1;
                    return result;
                }
            }
            else if (operators.Count == 0 && variables.Count == 1) //If we only have one variable just return it otherwise throw an error
            {
                return variables.Pop();
            }
            else
            {
                throw new ArgumentException("Final variable for expression could not be found...");
            }
        }

        /// <summary>
        /// Helper method designed for when the expression contains an integer value. 
        /// If a value is not found it will push it to the variables stack, 
        /// if an operator is avaiable it will compute the expression with the popped operator from the stack.
        /// </summary>
        /// <param name="num"></param>
        private static void IntegerExpressionHelper(int num)
        {
            //check if op stack is empty
            if(operators.Count != 0)
            {
                if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                {
                    //Check if variables is empty
                    if(variables.Count == 0)
                    {
                        throw new ArgumentException("Variables stack is empty...");
                    }

                    //Pop variables and operators once
                    int var1 = variables.Pop();
                    String op1 = operators.Pop();

                    //Apply operator and push result to variables stack
                    if (op1.Equals("*"))
                    {
                        int result = var1 * num;
                        variables.Push(result);
                    }
                    else
                    {
                        //Check for divide by zero
                        if(num == 0)
                        {
                            throw new ArgumentException("Division by zero error...");
                        }

                        int result = var1 / num;
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
        private static void OperatorExpressionHelper(string s)
        {
            if(operators.Count != 0)
            {
                if(operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                {
                    //Check if variables stack has 2 values that can be evaluated
                    if(variables.Count < 2)
                    {
                        throw new ArgumentException("Variables stack contains less than 2 values...");
                    }

                    //Pop the variables stack and operators stack for expression
                    int var1 = variables.Pop();
                    int var2 = variables.Pop();
                    String op1 = operators.Pop();

                    if (op1.Equals("+"))
                    {
                        int result = var1 + var2;
                        variables.Push(result);
                    }
                    else
                    {
                        int result = var2 - var1;
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
        private static void BraceExpressionHelper(string s)
        {
            if (operators.Count != 0)
            {
                if (operators.Peek().Equals("+") || operators.Peek().Equals("-"))
                {
                    //Check if variables stack has two values
                    if(variables.Count < 2)
                    {
                        throw new ArgumentException("Variables stack contains less than 2 values...");
                    }

                    //Pop variables stack and operators stack for expresssion
                    int var1 = variables.Pop();
                    int var2 = variables.Pop();
                    String op1 = operators.Pop();

                    if (op1.Equals("+"))
                    {
                        int result = var1 + var2;
                        variables.Push(result);
                    }
                    else
                    {
                        int result = var2 - var1;
                        variables.Push(result);
                    }
                }
                else if (operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                {
                    //Check if variables stack has two values
                    if(variables.Count < 2)
                    {
                        throw new ArgumentException("Variables stack contains less than 2 values...");
                    }

                    //Pop variables stack and operators stack for expression
                    int var1 = variables.Pop();
                    int var2 = variables.Pop();
                    String op1 = operators.Pop();

                    if (operators.Equals("*"))
                    {
                        int result = var1 * var2;
                        variables.Push(result);
                    }
                    else
                    {
                        //Check for divide by zero
                        if(var2 == 0)
                        {
                            throw new ArgumentException("Division by zero error...");
                        }
                        int result = var2 / var1;
                        variables.Push(result);
                    }
                }

                //Remove the opening brace if found, then preform Order of Operation if needed
                if(operators.Count != 0)
                {
                    if (operators.Peek().Equals("("))
                    {
                        operators.Pop();

                        //Check if an operator preceeds the brace
                        if(operators.Count != 0)
                        {
                            if(operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                            {
                                int var1 = variables.Pop();
                                int var2 = variables.Pop();
                                String op1 = operators.Pop();

                                if (op1.Equals("*"))
                                {
                                    int result = var1 * var2;
                                    variables.Push(result);
                                }
                                else
                                {
                                    int result = var2 / var1;
                                    variables.Push(result);
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Missing '(' on operator stack...");
                }
            }
            else
            {
                throw new ArgumentException("Missing '(' on operator stack...");
            }
        }

        /// <summary>
        /// Helper method designed for evalutating the current substring if its a variables, 
        /// making use of the variablesEvaluator to grab the contents of the cell and preform the calculation with the current operators.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="variableEvaluator"></param>
        private static void VariableExpressionHelper(string s, Lookup variableEvaluator)
        {
            try
            {
                //Verify that variable meets formating requirements
                if (!Regex.IsMatch(s, "([A-Z]|[a-z])+[0-9]+"))
                {
                    throw new ArgumentException("Invalid Variable...");
                }

                //Get variable is contents
                int num = variableEvaluator(s);

                if (operators.Count != 0)
                {
                    if(operators.Peek().Equals("*") || operators.Peek().Equals("/"))
                    {
                        //Check if variables stack is empty
                        if(variables.Count == 0)
                        {
                            throw new ArgumentException("Variables stack is empty...");
                        }

                        //Pop variables and operators once
                        int var1 = variables.Pop();
                        int var2 = variableEvaluator(s);
                        String op1 = operators.Pop();

                        //Apply operator and push result to variables stack
                        if (op1.Equals("*"))
                        {
                            int result = var1 * var2;
                            variables.Push(result);
                        }
                        else
                        {
                            //Check for divide by zero
                            if(var2 == 0)
                            {
                                throw new ArgumentException("Division by zero error...");
                            }

                            int result = var1 / var2;
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
            catch (ArgumentException e)
            {
                //throw error if variable could not be expressed
                throw new ArgumentException("variable could not be evaluated...");
            }
        }
    }
}