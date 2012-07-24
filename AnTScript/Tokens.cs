using System;
using System.Collections.Generic;
using System.Text;

namespace AnTScript
{

    public sealed class Tokens
    {
        // Constants to represent arithmitic tokens. This could
        // be alternatively written as an enum.
        public static readonly Token Add = new Token("ADD");
        public static readonly Token Sub = new Token("SUB");
        public static readonly Token Mul = new Token("MUL");
        public static readonly Token Div = new Token("DIV");
        public static readonly Token Semi = new Token("SEMI");
        public static readonly Token Assignment = new Token("ASSIGNMENT");
        public static readonly Token Equal = new Token("EQUAL");

        // ....
        public static readonly Token LessThan = new Token("LESSTHAN");
        public static readonly Token GreaterThan = new Token("GREATERTHAN");
        public static readonly Token LessThanOrEqual = new Token("LESSTHANOREQUAL");
        public static readonly Token GreaterThanOrEqual = new Token("GREATERTHANOREQUAL");

        public static readonly Token LeftParentesis = new Token("LEFTPARENTESIS");
        public static readonly Token RightParentesis = new Token("RIGHTPARENTESIS");

        public static readonly Token Comma = new Token("COMMA");
        //public static readonly Token Dot = new Token("DOT");
        //public static readonly Token Colon = new Token("COLON");

        public static readonly Token And = new Token("AND");
        public static readonly Token AndBit = new Token("ANDBIT");
        public static readonly Token Or = new Token("OR");
        public static readonly Token OrBit = new Token("ORBIT");
        public static readonly Token Not = new Token("NOT");
        public static readonly Token NotEqual = new Token("NOTEQUAL");

        public static readonly Token BeginBlock = new Token("BEGINBLOCK");
        public static readonly Token EndBlock = new Token("ENDBLOCK");

    }

    public class Token : TokenBase
    {
        public Token(string value)
            : base(value)
        { }
    }

    public class OperatorToken : TokenBase
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