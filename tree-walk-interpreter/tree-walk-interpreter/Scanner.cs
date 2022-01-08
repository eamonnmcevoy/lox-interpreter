using System;
using System.Collections.Generic;

namespace tree_walk_interpreter
{
  public class Scanner
  {
    private Dictionary<string, TokenType> _keywords = new()
    {
      {"and", TokenType.AND},
      {"class",  TokenType.CLASS},
      {"else",   TokenType.ELSE},
      {"false",  TokenType.FALSE},
      {"for",    TokenType.FOR},
      {"fun",    TokenType.FUN},
      {"if",     TokenType.IF},
      {"nil",    TokenType.NIL},
      {"or",     TokenType.OR},
      {"print",  TokenType.PRINT},
      {"return", TokenType.RETURN},
      {"super",  TokenType.SUPER},
      {"this",   TokenType.THIS},
      {"true",   TokenType.TRUE},
      {"var",    TokenType.VAR},
      {"while",  TokenType.WHILE},
    };
    
    private readonly string _source;
    private readonly List<Token> _tokens;
    
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Scanner(string source)
    {
      _source = source;
      _tokens = new List<Token>();
    }
    public List<Token> ScanTokens()
    {
      while (!IsAtEnd())
      {
        _start = _current;
        ScanToken();
      }
      _tokens.Add(new Token(TokenType.EOF, "", null, _line));
      return _tokens;
    }

    private void ScanToken()
    {
      var c = Advance();
      switch (c) {
        case '(': AddToken(TokenType.LEFT_PAREN); break;
        case ')': AddToken(TokenType.RIGHT_PAREN); break;
        case '{': AddToken(TokenType.LEFT_BRACE); break;
        case '}': AddToken(TokenType.RIGHT_BRACE); break;
        case ',': AddToken(TokenType.COMMA); break;
        case '.': AddToken(TokenType.DOT); break;
        case '-': AddToken(TokenType.MINUS); break;
        case '+': AddToken(TokenType.PLUS); break;
        case ';': AddToken(TokenType.SEMICOLON); break;
        case '*': AddToken(TokenType.STAR); break;
        case '!':
          AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
          break;
        case '=':
          AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
          break;
        case '<':
          AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
          break;
        case '>':
          AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
          break;
        case '/':
          if (Match('/')) {
            // A comment goes until the end of the line.
            while (Peek() != '\n' && !IsAtEnd()) Advance();
          } else {
            AddToken(TokenType.SLASH);
          }
          break;
        case ' ':
        case '\r':
        case '\t':
          // Ignore whitespace.
          break;
        case '\n':
          _line++;
          break;
        case '"':
          HandleString();
          break;
        default:
          if (IsAlpha(c))
          {
            HandleIdentifier();
          }
          else if (IsDigit(c))
          {
            HandleNumber();
          }
          else
          {
            Program.Error(_line, "Unexpected character.");  
          }
          break;
      }
    }

    private void HandleIdentifier()
    {
      while (IsAlphaNumeric(Peek())) Advance();
      var text = _source.Substring(_start, _current-_start);
      var type = _keywords.ContainsKey(text)
        ? _keywords[text]
        : TokenType.IDENTIFIER;
      AddToken(type);
    }
    
    private void HandleNumber()
    {
      while (IsDigit(Peek())) Advance();
      
      if(Peek() == '.' && IsDigit(PeekNext()))
      {
        Advance();
        while (IsDigit(Peek())) Advance();
      }

      var substr = _source.Substring(_start, _current-_start);
      var value = Double.Parse(substr);
      AddToken(TokenType.NUMBER, value);
    }

    private void HandleString()
    {
      while (Peek() != '"' && !IsAtEnd())
      {
        if (Peek() == '\n')
          _line++;
        Advance();
      }

      if (IsAtEnd())
      {
        Program.Error(_line, "unterminated string");
        return;
      }

      Advance();

      var value = _source.Substring(_start + 1, _current - _start-1);
      AddToken(TokenType.STRING, value);
    }

    private char Peek()
    {
      return IsAtEnd()
        ? '\0'
        : _source[_current];
    }
    
    private char PeekNext()
    {
      return _current + 1 >= _source.Length 
        ? '\0' : 
        _source[_current+1];
    } 

    private bool IsDigit(char c)
    {
      return c is >= '0' and <= '9';
    }
    
    private bool IsAlpha(char c) {
      return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }

    private bool IsAlphaNumeric(char c)
    {
      return IsAlpha(c) || IsDigit(c);
    }

    private bool IsAtEnd()
    {
      return _current >= _source.Length;
    }

    private char Advance()
    {
      return _source[_current++];
    }

    private bool Match(char expected)
    {
      if (IsAtEnd()) return false;
      if (_source[_current] != expected) return false;
      _current++;
      return true;
    }

    private void AddToken(TokenType type, object literal = null)
    {
      var text = _source.Substring(_start, _current-_start);
      _tokens.Add(new Token(type, text, literal, _line));
    }
  }
}