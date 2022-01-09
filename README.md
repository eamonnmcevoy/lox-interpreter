# Lox interpreter

An interpreter for lox created using the [craftinginterpreters](https://craftinginterpreters.com/) book.

## tree-walk-interpreter

This project is the interpreter for the lox language.

Usage:

Enter REPL

```sh
 dotnet tree-walk-interpreter.dll 
```


Run source file

```sh
 dotnet tree-walk-interpreter.dll $filename
```

## GenerateAst

Tool to generate abstract syntax tree classes according to the provided grammar. Grammer file can be found at `./GenerateAst/grammar.txt`

Usage:

```sh
 dotnet GenerateAst.dll $grammar_file $output_dir
```
