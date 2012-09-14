using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnTScript
{

    public sealed class Scanner
    {

        #region Constructors

        public Scanner(TextReader input)
        {
            _result = new List<Token>();
            Scan(input);
        }

        public Scanner(string input) :
            this(new StreamReader(new MemoryStream(Encoding.Default.GetBytes(input))))
        {

        }

        #endregion

        #region Properties

        private readonly IList<Token> _result;
        public IList<Token> TokensList
        {
            get { return _result; }
        }

        #endregion

        #region Private Methods

        private void Scan(TextReader input)
        {
            while (input.Peek() != -1)
            {
                char ch = (char)input.Peek();

                //
                // Scan individual tokens
                //

                // eat the current char and skip ahead!
                if (char.IsWhiteSpace(ch))
                {                    
                    input.Read();
                }
                //Comment
                else if (ch.ToString() == "'")
                {
                    
                    ch = ReadComment(input, ch);
                }
                // keyword or identifier
                else if (char.IsLetter(ch) || ch == '_')
                {                                        
                    _result.Add(ReadKeywordOrIdentifier(input, ch));

                }
                // string literal
                else if (ch == '"')
                {
                    _result.Add(ReadStringLiteral(input, ch));
                }
                // numeric literal
                else if (char.IsDigit(ch))
                {
                    _result.Add(ReadNumericLiteral(input, ch));
                }
                // node literal
                else if (ch == '$')
                {
                    _result.Add(ReadObjectLiteral(input, ch));
                }
                // node literal
                else if (ch == '#')
                {
                    _result.Add(ReadDateTimeLiteral(input, ch));
                }
                // operator or symbol 
                else
                    _result.Add(ReadOperatorOrSymbol(input, ch));
            }
        }

        #endregion

        #region Utils

        private char ReadComment(TextReader input, char ch)
        {
            // Comment, eat full line
            while ((ch = (char)input.Peek()) != '\n')  
            {
                if (input.Read() == -1)
                {
                    break;
                }
            }
            // eat newline character
            if (ch == '\n')
            {
                input.Read();
            }

            return ch;
        }

        private Token ReadKeywordOrIdentifier(TextReader input, char ch)
        {
            StringBuilder accum = new StringBuilder();
            Token token;

            while (char.IsLetter(ch) || ch == '_' || ch == '.')
            {
                accum.Append(ch);
                input.Read();

                if (input.Peek() == -1)
                {
                    break;
                }
                else
                {
                    ch = (char)input.Peek();
                }
            }

            // literal operators 
            if (accum.ToString().ToLower() == "or")
                token = Tokens.Or;
            else if (accum.ToString().ToLower() == "and")
                token = Tokens.And;
            else if (accum.ToString().ToLower() == "not")
                token = Tokens.Not;

            // block 
            else if (accum.ToString().ToLower() == "do")
                token = Tokens.BeginBlock;
            else if (accum.ToString().ToLower() == "end")
                token = Tokens.EndBlock;

            // keywords
            else if (accum.ToString().ToLower() == "print")
                token = Tokens.Print;
            else if (accum.ToString().ToLower() == "var")
                token = Tokens.Var;
            else if (accum.ToString().ToLower() == "read_num")
                token = Tokens.Read_num;
            else if (accum.ToString().ToLower() == "for")
                token = Tokens.For;
            else if (accum.ToString().ToLower() == "to")
                token = Tokens.To;
            else if (accum.ToString().ToLower() == "if")
                token = Tokens.If;
            else if (accum.ToString().ToLower() == "then")
                token = Tokens.Then;
            else if (accum.ToString().ToLower() == "else")
                token = Tokens.Else;
            else if (accum.ToString().ToLower() == "while")
                token = Tokens.While;
            else if (accum.ToString().ToLower() == "break")
                token = Tokens.Break;

            // identifier
            else
            {
                IdentifierToken identifier = new IdentifierToken(accum.ToString());
                token = identifier;
            }

            return token;

        }

        private StringToken ReadStringLiteral(TextReader input, char ch)
        {
            StringBuilder accum = new StringBuilder();
            
            input.Read(); // skip "

            if (input.Peek() == -1)
            {
                throw new System.Exception("unterminated string literal");
            }

            while ((ch = (char)input.Peek()) != '"')
            {
                accum.Append(ch);
                input.Read();

                if (input.Peek() == -1)
                {
                    throw new System.Exception("unterminated string literal");
                }
            }

            // skip the terminating "
            input.Read();

            return new StringToken(accum.ToString());
        }

        private NumberToken ReadNumericLiteral(TextReader input, char ch)
        {
            StringBuilder accum = new StringBuilder();
            // TODO: controlar ..
            while (char.IsDigit(ch) || ch == '.')
            {
                accum.Append(ch);
                input.Read();

                if (input.Peek() == -1)
                    break;
                else
                    ch = (char)input.Peek();
            }

            if (!accum.ToString().Contains("."))
                accum.Append(".0");
            
            return new NumberToken(accum.ToString());

        }

        private ObjectToken ReadObjectLiteral(TextReader input, char ch)
        {
            StringBuilder accum = new StringBuilder();

            input.Read(); // skip $

            if (input.Peek() == -1)
            {
                throw new System.Exception("unterminated string literal");
            }

            while ((ch = (char)input.Peek()) != '$')
            {
                accum.Append(ch);
                input.Read();

                if (input.Peek() == -1)
                {
                    throw new System.Exception("unterminated string literal");
                }
            }

            // skip the terminating $
            input.Read();

            return new ObjectToken(accum.ToString());
        }

        private DateTimeToken ReadDateTimeLiteral(TextReader input, char ch)
        {
            StringBuilder accum = new StringBuilder();

            input.Read(); // skip #

            if (input.Peek() == -1)
            {
                throw new System.Exception("unterminated datetime literal");
            }

            while ((ch = (char)input.Peek()) != '#')
            {
                accum.Append(ch);
                input.Read();

                if (input.Peek() == -1)
                {
                    throw new System.Exception("unterminated datetime literal");
                }
            }

            // skip the terminating #
            input.Read();

            return new DateTimeToken(accum.ToString());
        }


        private Token ReadOperatorOrSymbol(TextReader input, char ch)
        {
            char chTmp;
            Token token;

            switch (ch)
            {
                case '+':
                    input.Read();
                    token = Tokens.Add;
                    break;

                case '-':
                    input.Read();
                    token = Tokens.Sub;
                    break;

                case '*':
                    input.Read();
                    token = Tokens.Mul;
                    break;

                case '/':
                    input.Read();
                    token = Tokens.Div;
                    break;

                case '=':
                    input.Read(); // eat =
                    chTmp = (char)input.Peek();
                    if (chTmp == '=')
                    {
                        input.Read(); // eat =
                        token = Tokens.Equal;
                    }
                    else
                    {
                        token = Tokens.Assignment;
                    }                    
                    break;

                case ';':
                    input.Read();
                    token = Tokens.Semi;
                    break;

                case ',':
                    input.Read();
                    token = Tokens.Comma;
                    break;

                case '(':
                    input.Read();
                    token = Tokens.LeftParentesis;
                    break;

                case ')':
                    input.Read();
                    token = Tokens.RightParentesis;
                    break;

                case '{':
                    input.Read();
                    token = Tokens.BeginBlock;
                    break;

                case '}':
                    input.Read();
                    token = Tokens.EndBlock;
                    break;

                case '<':
                    input.Read(); // eat <
                    chTmp = (char)input.Peek();
                    if (chTmp == '>')
                    {
                        input.Read(); // eat >
                        token = Tokens.NotEqual;
                    }
                    else if (chTmp == '=')
                    {
                        input.Read(); // eat =
                        token = Tokens.LessThanOrEqual;
                    }
                    else
                    {
                        token = Tokens.LessThan;
                    }                    
                    break;

                case '>':
                    input.Read(); // eat >
                    chTmp = (char)input.Peek();
                    if (chTmp == '=')
                    {
                        input.Read(); // eat =
                        token = Tokens.GreaterThanOrEqual;
                    }
                    else
                    {
                        token = Tokens.GreaterThan;
                    }                    
                    break;

                case '!':
                    input.Read(); // eat !
                    chTmp = (char)input.Peek();
                    if (chTmp == '=')
                    {
                        input.Read(); // eat =
                        token = Tokens.NotEqual;
                    }
                    else
                    {
                        token = Tokens.Not;
                    }                    
                    break;

                case '&':
                    input.Read(); // eat &
                    chTmp = (char)input.Peek();
                    if (chTmp == '&')
                    {
                        input.Read(); // eat &
                        token = Tokens.And;
                    }
                    else
                    {
                        token = Tokens.AndBit;
                    }                    
                    break;

                case '|':
                    input.Read(); // eat |
                    chTmp = (char)input.Peek();
                    if (chTmp == '|')
                    {
                        input.Read(); // eat |
                        token = Tokens.Or;
                    }
                    else
                    {
                        token = Tokens.OrBit;
                    }                    
                    break;

                default:
                    throw new System.Exception("Scanner encountered unrecognized character '" + ch + "'");
            }

            return token;

        }

        #endregion

    }

}