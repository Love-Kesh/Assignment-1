using System;
using System.Collections.Generic;

namespace Assignment1
{
    // TODO Add supporting classes here
    public class Program
    {
        public static string ProcessCommand(string input)
        {
            try
            {
                 var evaluator = new ExpressionEvaluator();
                 double result = evaluator.Evaluate(input);
                 return result.ToString();
                // TODO Evaluate the expression and return the result
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e;
            }
        }
        public class ExpressEvaluator
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

        // please insert ur code here Lucky :)

















            
            
            
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
