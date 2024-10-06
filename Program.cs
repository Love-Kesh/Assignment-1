using System;
using System.Collections.Generic;

namespace Assignment1
{
    public class Program
    {
        public static string ProcessCommand(string input)
        {
            try
            {
                var evaluator = new ExpressionEvaluator();
                double result = evaluator.Evaluate(input);
                return result.ToString();
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e.Message;
            }
        }

        public class ExpressionEvaluator
        {
            private readonly Dictionary<char, int> precedence = new Dictionary<char, int>
            {
                { '+', 1 },
                { '-', 1 },
                { '*', 2 },
                { '/', 2 },
                { '(', 0 }
            };

            public double Evaluate(string expression)
            {
                var outputQueue = new Queue<string>();
                var operatorStack = new Stack<char>();
                var tokens = Tokenize(expression);

                foreach (var token in tokens)
                {
                    if (double.TryParse(token, out _))
                    {
                        outputQueue.Enqueue(token);
                    }
                    else if (token == "(")
                    {
                        operatorStack.Push('(');
                    }
                    else if (token == ")")
                    {
                        while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                        {
                            outputQueue.Enqueue(operatorStack.Pop().ToString());
                        }

                        if (operatorStack.Count == 0)
                        {
                            throw new InvalidOperationException("Mismatched parentheses in expression!");
                        }

                        operatorStack.Pop();
                    }
                    else if (precedence.ContainsKey(token[0]))
                    {
                        char op = token[0];

                        while (operatorStack.Count > 0 && precedence[operatorStack.Peek()] >= precedence[op])
                        {
                            outputQueue.Enqueue(operatorStack.Pop().ToString());
                        }
                        operatorStack.Push(op);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unknown token: {token}");
                    }
                }

                while (operatorStack.Count > 0)
                {
                    char top = operatorStack.Pop();
                    if (top == '(')
                    {
                        throw new InvalidOperationException("Mismatched parentheses in expression.");
                    }
                    outputQueue.Enqueue(top.ToString());
                }

                return EvaluateRPN(outputQueue);
            }

            private List<string> Tokenize(string expression)
            {
                var tokens = new List<string>();
                string number = "";

                for (int i = 0; i < expression.Length; i++)
                {
                    char ch = expression[i];

                    if (char.IsWhiteSpace(ch)) continue;

                    if (char.IsDigit(ch) || ch == '.')
                    {
                        number += ch;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(number))
                        {
                            tokens.Add(number);
                            number = "";
                        }

                        if (ch == '-' && (i == 0 || expression[i - 1] == '(' || expression[i - 1] == '+' || expression[i - 1] == '-' || expression[i - 1] == '*' || expression[i - 1] == '/'))
                        {
                            number = "-";
                        }
                        else
                        {
                            tokens.Add(ch.ToString());
                        }
                    }
                }

                if (!string.IsNullOrEmpty(number)) tokens.Add(number);

                return tokens;
            }

            private double EvaluateRPN(Queue<string> rpnQueue)
            {
                var stack = new Stack<double>();

                while (rpnQueue.Count > 0)
                {
                    var token = rpnQueue.Dequeue();

                    if (double.TryParse(token, out double number))
                    {
                        stack.Push(number);
                    }
                    else
                    {
                        if (stack.Count < 2)
                        {
                            throw new InvalidOperationException("Invalid expression.");
                        }

                        double right = stack.Pop();
                        double left = stack.Pop();
                        double result;

                        switch (token)
                        {
                            case "+":
                                result = left + right;
                                break;
                            case "-":
                                result = left - right;
                                break;
                            case "*":
                                result = left * right;
                                break;
                            case "/":
                                if (right == 0)
                                {
                                    throw new InvalidOperationException("Division by zero!");
                                }
                                result = left / right;
                                break;
                            default:
                                throw new InvalidOperationException($"Unknown operator: {token}! \nUse the basic ones ( +, -, *, /)");
                        }

                        stack.Push(result);
                    }
                }

                if (stack.Count != 1)
                {
                    throw new InvalidOperationException("Invalid expression.");
                }

                return stack.Pop();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter your expression ('exit' to close):");
            int i = 1;
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine(ProcessCommand(input));
                i++;
                Console.WriteLine($"Enter expression number: {i}");
            }
        }
    }
}
