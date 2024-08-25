using Qurigo.Interfaces;

namespace Qurigo.Simulator;

public enum TokenType
{
    Comment,
    Keyword,
    Identifier,
    Subroutine,
    If,
    Else,
    While,
    For,
    In,
    Return,
    Const,
    Qubit,
    Bit,
    Gate,
    Measure,
    Reset,
    Number,
    Symbol,
    String,
    Operator,
    Assignment,
    Arrow,

    LeftParenthesis,
    RightParenthesis,
    LeftBracket,
    RightBracket,
    LeftBrace,
    RightBrace,
    SemiColon,
    Period,
    Comma,
    At,

    EndOfLine,
    EndOfFile,

    OpenQASM
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public GateNames GateType{ get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
        // Console.WriteLine(Type + " : " + Value);
    }

    public Token(TokenType type, GateNames gateType)
    {
        Type = type;
        GateType = gateType;
        // Console.WriteLine(Type + " : " + GateType);
    }

    public Token(TokenType type)
    {
        Type = type;
        // Console.WriteLine(Type);
    }
}

public class Tokenizer
{
    private string[] _programLines;
    private int _currentLine;
    private int _linePosition;

    public Tokenizer(string program)
    {
        _currentLine = 0;
        _programLines = program.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }

    public void ReadUntilSemicolon()
    {
        Token token = NextToken();
        while (token.Type != TokenType.SemiColon)
        {
            token = NextToken();
        }
    }

    public Token NextToken(TokenType type)
    {
        Token token = NextToken();
        if (token.Type != type)
        {
            throw new Exception($"Expected {type.ToString()} but was {token.Type}, {token.Value}.");
        }

        return token;
    }

    public Token NextToken()
    {
        if (_currentLine >= _programLines.Length)
        {
            return new Token(TokenType.EndOfFile);
        }

        string token = string.Empty;
        string line = _programLines[_currentLine].Trim();

        while (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
        {
            _currentLine++;
            if (_currentLine >= _programLines.Length)
            {
                return new Token(TokenType.EndOfFile);
            }

            line = _programLines[_currentLine].Trim();
        }

        while (_linePosition < line.Length)
        {
            if (line[_linePosition] == '"')
            {
                _linePosition++;
                while (line[_linePosition] != '"')
                {
                    token += line[_linePosition];
                    _linePosition++;
                }

                _linePosition++;
                return new Token(TokenType.String, token);
            }

            if (line[_linePosition] == '-' && line[_linePosition + 1] == '>')
            {
                _linePosition++;
                _linePosition++;
                return new Token(TokenType.Arrow);
            }

            if (line[_linePosition] == '=' && line[_linePosition + 1] == '=')
            {
                _linePosition++;
                _linePosition++;
                return new Token(TokenType.Operator, "==");
            }

            if (line[_linePosition] == '!' && line[_linePosition + 1] == '=')
            {
                _linePosition++;
                _linePosition++;
                return new Token(TokenType.Operator, "!=");
            }

            Token? t = line[_linePosition] switch
            {
                '+' => new Token(TokenType.Operator, "+"),
                '-' => new Token(TokenType.Operator, "-"),
                '*' => new Token(TokenType.Operator, "*"),
                '/' => new Token(TokenType.Operator, "/"),
                '%' => new Token(TokenType.Operator, "%"),
                '>' => new Token(TokenType.Operator, ">"),
                '<' => new Token(TokenType.Operator, "<"),
                '=' => new Token(TokenType.Assignment, "="),
                _ => null
            };

            if (t != null)
            {
                _linePosition++;
                return t;
            }

            if (line[_linePosition] == '#')
            {
                _currentLine++;
                _linePosition = 0;
                return new Token(TokenType.Comment);
            }

            if (line[_linePosition] == '@')
            {
                if (token.Length > 0)
                {
                    return GetTokenFromString(token);
                }
                else
                {
                    _linePosition++;
                    return new Token(TokenType.At);
                }
            }

            if (line[_linePosition] == ';')
            {
                if (token.Length > 0)
                {
                    return GetTokenFromString(token);
                }
                else
                {
                    _currentLine++;
                    _linePosition = 0;
                    return new Token(TokenType.SemiColon);
                }
            }

            if (line[_linePosition] == '(')
            {
                if (token.Length > 0)
                {
                    return GetTokenFromString(token);
                }
                else
                {
                    _linePosition++;
                    return new Token(TokenType.LeftParenthesis);
                }
            }

            if (line[_linePosition] == '.')
            {
                if (token.Length > 0)
                {
                    return GetTokenFromString(token);
                }
                else
                {
                    _linePosition++;
                    return new Token(TokenType.Period);
                }
            }

            if (line[_linePosition] == ',')
            {
                if (token.Length > 0)
                {
                    return GetTokenFromString(token);
                }
                else
                {
                    _linePosition++;
                    return new Token(TokenType.Comma);
                }
            }

            if (line[_linePosition] == ')')
            {
                if(token.Length == 0)
                {
                    _linePosition++;
                    return new Token(TokenType.RightParenthesis);
                }
                else
                {
                    return GetTokenFromString(token);
                }
            }

            if (line[_linePosition] == '[')
            {
                if (token.Length == 0)
                {
                    _linePosition++;
                    return new Token(TokenType.LeftBracket);
                }
                else
                {
                    return GetTokenFromString(token);
                }
            }

            if (line[_linePosition] == ']')
            {
                if (token.Length == 0)
                {
                    _linePosition++;
                    return new Token(TokenType.RightBracket);
                }
                else
                {
                    return GetTokenFromString(token);
                }
            }

            if (line[_linePosition] == '{')
            {
                _currentLine++;
                _linePosition = 0;
                return new Token(TokenType.LeftBrace);
            }

            if (line[_linePosition] == '}')
            {
                _currentLine++;
                _linePosition = 0;
                return new Token(TokenType.RightBrace);
            }

            if (line[_linePosition] == ' ' || line[_linePosition] == '\t')
            {
                if (token.Length > 0)
                {
                    _linePosition++;

                    if (token == "def")
                    {
                        return new Token(TokenType.Subroutine);
                    }

                    return GetTokenFromString(token);
                }
            }
            else
            {
                token += line[_linePosition];
            }

            _linePosition++;
        }

        _currentLine++;
        _linePosition = 0;

        if(token == "\r")
        {
            // Unexpetced empty line, get next token
            return NextToken();
        }

        return GetTokenFromString(token);
    }

    private Token GetTokenFromString(string token)
    {
        Token? t = token switch
        {
            "def" => new Token(TokenType.Subroutine),
            "if" => new Token(TokenType.If),
            "else" => new Token(TokenType.Else),
            "while" => new Token(TokenType.While),
            "for" => new Token(TokenType.For),
            "in" => new Token(TokenType.In),
            "return" => new Token(TokenType.Return),
            "const" => new Token(TokenType.Const),
            "qubit" => new Token(TokenType.Qubit),
            "bit" => new Token(TokenType.Bit),
            "measure" => new Token(TokenType.Measure),
            "reset" => new Token(TokenType.Reset),
            "OPENQASM" => new Token(TokenType.OpenQASM),
            _ => null
        };

        if (t != null)
        {
            return t;
        }

        if(int.TryParse(token, out int number))
        {
            return new Token(TokenType.Number, number.ToString());
        }

        if (Enum.TryParse<GateNames>(token.ToUpper(), out GateNames enumName))
        {
            return new Token(TokenType.Gate, enumName);
        }
        else
        {
            return new Token(TokenType.Identifier, token);
        }
    }
}
