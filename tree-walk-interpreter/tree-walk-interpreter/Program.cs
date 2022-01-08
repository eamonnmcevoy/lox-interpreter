using System;
using System.Collections.Generic;
using System.IO;

namespace tree_walk_interpreter
{
  class Program
  {
    private static bool _hadError = false;
    
    static void Main(string[] args)
    {
      switch (args.Length)
      {
        case > 1:
          Console.WriteLine("Usage: jlox [script]");
          Environment.Exit(64);
          break;
        case 1:
          RunFile(args[0]);
          break;
        default:
          RunPrompt();
          break;
      }
    }

    private static void RunFile(string path)
    {
      var content = File.ReadAllText(path);
      Run(content);
      if (_hadError) Environment.Exit(65);
    }

    private static void RunPrompt()
    {
      for (;;)
      {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line))
          break;
        Run(line);
        _hadError = false;
      }
    }

    private static void Run(string content)
    {
      var scanner = new Scanner(content);
      List<Token> tokens = scanner.ScanTokens();
      foreach (var token in tokens)
      {
        Console.WriteLine(token);
      }
    }

    public static void Error(int line, string message)
    {
      Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
      Console.WriteLine($"[line {line.ToString()}] Error {where}: {message}");
      _hadError = true;
    }
  }
}