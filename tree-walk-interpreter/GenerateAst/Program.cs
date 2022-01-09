using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenerateAst
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 2)
      {
        Console.WriteLine("Usage: generate_ast <grammar file> <output dir>");
        Console.WriteLine(64);
      }
      
      var grammarContent = File.ReadAllLines(args[0]);
      var outputDir = args[1];

      if (!Directory.Exists(outputDir))
      {
        Console.WriteLine($"Creating output dir: {outputDir}");
        Directory.CreateDirectory(outputDir);
      }
      else
      {
        Console.WriteLine($"Output dir: {outputDir}");
      }



      DefineAst(outputDir, "Expression", grammarContent);
      
      Console.WriteLine("Complete!");
    }

    private static void DefineAst(string outputDir, string baseName, IEnumerable<string> types)
    {
      var path = $"{outputDir}/{baseName}.cs";
      using (StreamWriter writer = new StreamWriter(path))
      {
        writer.WriteLine("namespace tree_walk_interpreter");
        writer.WriteLine("{");
        writer.WriteLine($"  abstract class {baseName}");
        writer.WriteLine("  {");
        DefineVisitor(writer, baseName, types);
        writer.WriteLine("");
        writer.WriteLine($"    public abstract void Accept<T>(IVisitor visitor);");
        writer.WriteLine("");

      foreach (var type in types)
      {
        Console.WriteLine(type);
        var parts = type.Split(":").Select(x => x.Trim()).ToArray();
        var className = parts[0];
        var fields = parts[1];
        DefineType(writer, baseName, className, fields);
      }
      writer.WriteLine("");
      
      writer.WriteLine("  }");
      writer.WriteLine("}");
      }
    }

    private static void DefineVisitor(StreamWriter wrtier, string baseName, IEnumerable<string> types)
    {
      wrtier.WriteLine("");
      wrtier.WriteLine("    public interface IVisitor {");
      foreach (var type in types)
      {
        var typeName = type.Split(":")[0].Trim();
        wrtier.WriteLine($"      void Visit({typeName} {baseName.ToLower()});");
      }
      wrtier.WriteLine("    }");
      wrtier.WriteLine("");
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
    {
      writer.WriteLine($"    public class {className}: {baseName}");
      writer.WriteLine("    {");

      //Properties
      var fields = fieldList.Split(",").Select(x => x.Trim()).ToArray();
      foreach (var field in fields)
      {
        var fieldType = field.Split(" ")[0];
        var fieldName = field.Split(" ")[1];
        writer.WriteLine($"      public {fieldType} {CapitalizeFirstChar(fieldName)} {{ get; }}");  
      }
      writer.WriteLine("");
      
      //Constructor
      writer.WriteLine($"      {className} ({fieldList})");
      writer.WriteLine("      {");
      foreach (var field in fields)
      {
        var fieldName = field.Split(" ")[1];
        writer.WriteLine($"        this.{CapitalizeFirstChar(fieldName)} = {fieldName};");
      }
      writer.WriteLine("      }");
      
      //Visitor pattern
      writer.WriteLine($"      public override void Accept<T>(IVisitor visitor) {{");
      writer.WriteLine($"        visitor.Visit(this);");
      writer.WriteLine($"      }}");
      
      writer.WriteLine("    }");
    }

    private static string CapitalizeFirstChar(string str)
    {
      return $"{char.ToUpper(str[0]).ToString()}{str[1..]}";
    }
  }
}