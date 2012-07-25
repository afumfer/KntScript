using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnTScript
{

    public sealed class Scanner
    {

        public Scanner(TextReader input)
        {
            _result = new List<object>();
            Scan(input);
        }

        public Scanner(string input) :
            this(new StreamReader(new MemoryStream(Encoding.Default.GetBytes(input))))
        {

        }

        private readonly IList<object> _result;
        public IList<object> TokensList
        {
            get { return _result; }
        }

        private void Scan(TextReader input)
        {
            while (input.Peek() != -1)
            {
                char ch = (char)input.Peek();
                char chTmp;
                Token token;

                // Scan individual tokens
                if (char.IsWhiteSpace(ch))
                {
                    // eat the current char and skip ahead!
                    input.Read();
                }

                else if (ch.ToString() == "'")
                {
                    //Comment
                    ch = ReadComment(input, ch);
                }

                else if (char.IsLetter(ch) || ch == '_')
                {
                    // keyword or identifier
                    StringBuilder accum = new StringBuilder();
                    ReadKeywordOrIdentifier(input, ch, accum);

                    if (accum.ToString().ToLower() == "or")
                        _result.Add(Tokens.Or);
                    else if (accum.ToString().ToLower() == "and")
                        _result.Add(Tokens.And);
                    else if (accum.ToString().ToLower() == "not")
                        _result.Add(Tokens.Not);
                    else if (accum.ToString().ToLower() == "do")
                        _result.Add(Tokens.BeginBlock);
                    else if (accum.ToString().ToLower() == "end")
                        _result.Add(Tokens.EndBlock);
                    else
                        _result.Add(accum.ToString());
                }
                else if (ch == '"')
                {
                    // string literal
                    StringBuilder accum = new StringBuilder();
                    ReadStringLiteral(input, ch, accum);
                    _result.Add(accum);
                }
                else if (char.IsDigit(ch))
                {
                    // numeric literal
                    StringBuilder accum = new StringBuilder();
                    ReadNumericLiteral(input, ch, accum);
                    _result.Add(float.Parse(accum.ToString(),
                        System.Globalization.CultureInfo.InvariantCulture));
                }
                else switch (ch)
                    {
                        case '+':
                            input.Read();
                            _result.Add(Tokens.Add);
                            break;

                        case '-':
                            input.Read();
                            _result.Add(Tokens.Sub);
                            break;

                        case '*':
                            input.Read();
                            _result.Add(Tokens.Mul);
                            break;

                        case '/':
                            input.Read();
                            _result.Add(Tokens.Div);
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
                            _result.Add(token);
                            break;

                        case ';':
                            input.Read();
                            _result.Add(Tokens.Semi);
                            break;

                        case ',':
                            input.Read();
                            _result.Add(Tokens.Comma);
                            break;

                        case '(':
                            input.Read();
                            _result.Add(Tokens.LeftParentesis);
                            break;

                        case ')':
                            input.Read();
                            _result.Add(Tokens.RightParentesis);
                            break;

                        case '{':
                            input.Read();
                            _result.Add(Tokens.BeginBlock);
                            break;

                        case '}':
                            input.Read();
                            _result.Add(Tokens.EndBlock);
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
                            _result.Add(token);
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
                            _result.Add(token);
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
                            _result.Add(token);
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
                            _result.Add(token);
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
                            _result.Add(token);
                            break;


                        default:
                            throw new System.Exception("Scanner encountered unrecognized character '" + ch + "'");
                    }

            }
        }

        #region Utils

        private char ReadComment(TextReader input, char ch)
        {
            // Comment, eat full line
            while ((ch = (char)input.Peek()) != '\n')// || input.Peek() == -1)
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

        private void ReadKeywordOrIdentifier(TextReader input, char ch, StringBuilder accum)
        {
            while (char.IsLetter(ch) || ch == '_')
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
        }

        private void ReadStringLiteral(TextReader input, char ch, StringBuilder accum)
        {
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
        }

        private void ReadNumericLiteral(TextReader input, char ch, StringBuilder accum)
        {
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
        }

        #endregion

    }

}