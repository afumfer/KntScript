using System;
using System.Collections.Generic;
using System.Text;

namespace AnTScript
{

    public sealed class Tokens
    {
        // Constants to represent arithmitic tokens. This could
        // be alternatively written as an enum.
        public static readonly OperatorToken Add = new OperatorToken("ADD", 1);
        public static readonly OperatorToken Sub = new OperatorToken("SUB", 1);
        public static readonly OperatorToken Mul = new OperatorToken("MUL", 2);
        public static readonly OperatorToken Div = new OperatorToken("DIV", 2);
        public static readonly OperatorToken Equal = new OperatorToken("EQUAL", 0);

        // ....
        public static readonly OperatorToken LessThan = new OperatorToken("LESSTHAN", 0);
        public static readonly OperatorToken GreaterThan = new OperatorToken("GREATERTHAN", 0);
        public static readonly OperatorToken LessThanOrEqual = new OperatorToken("LESSTHANOREQUAL", 0);
        public static readonly OperatorToken GreaterThanOrEqual = new OperatorToken("GREATERTHANOREQUAL", 0);

        public static readonly OperatorToken And = new OperatorToken("AND", 1);
        public static readonly OperatorToken AndBit = new OperatorToken("ANDBIT", 1);
        public static readonly OperatorToken Or = new OperatorToken("OR", 0);
        public static readonly OperatorToken OrBit = new OperatorToken("ORBIT", 0);
        public static readonly OperatorToken Not = new OperatorToken("NOT", 2);
        public static readonly OperatorToken NotEqual = new OperatorToken("NOTEQUAL", 2);

        public static readonly Token Semi = new Token("SEMI");
        public static readonly Token Assignment = new Token("ASSIGNMENT");

        public static readonly Token LeftParentesis = new Token("LEFTPARENTESIS");
        public static readonly Token RightParentesis = new Token("RIGHTPARENTESIS");

        public static readonly Token Comma = new Token("COMMA");
        //public static readonly Token Dot = new Token("DOT");
        //public static readonly Token Colon = new Token("COLON");

        public static readonly Token BeginBlock = new Token("BEGINBLOCK");
        public static readonly Token EndBlock = new Token("ENDBLOCK");

    }

    // TODO: Cambiar para heredar de TokenBase
    public class OperatorToken : Token
    {
        private readonly int _precedence = 0;

        public OperatorToken(string value, int precedence)
            : base(value)
        {
            _precedence = precedence;
        }

        public int Precedence
        {
            get { return _precedence; }
        }
    }

    public class Token : TokenBase
    {
        public Token(string value)
            : base(value)
        { }
    }

    public abstract class TokenBase
    {
        private string name = "";

        public TokenBase(string s)
        {
            name = s;
        }

        public override string ToString()
        {
            return name;
        }

    }

}