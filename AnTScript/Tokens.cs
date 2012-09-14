using System;
using System.Collections.Generic;
using System.Text;

namespace AnTScript
{

    public sealed class Tokens
    {
        // Constants to represent arithmitic tokens. 
        public static readonly OperatorToken Add = new OperatorToken("ADD", 1);
        public static readonly OperatorToken Sub = new OperatorToken("SUB", 1);
        public static readonly OperatorToken Mul = new OperatorToken("MUL", 2);
        public static readonly OperatorToken Div = new OperatorToken("DIV", 2);
        
        // ....
        public static readonly OperatorToken LessThan = new OperatorToken("LESSTHAN", 0);
        public static readonly OperatorToken GreaterThan = new OperatorToken("GREATERTHAN", 0);
        public static readonly OperatorToken LessThanOrEqual = new OperatorToken("LESSTHANOREQUAL", 0);
        public static readonly OperatorToken GreaterThanOrEqual = new OperatorToken("GREATERTHANOREQUAL", 0);
        public static readonly OperatorToken Equal = new OperatorToken("EQUAL", 0);
        public static readonly OperatorToken NotEqual = new OperatorToken("NOTEQUAL", 2);

        public static readonly OperatorToken And = new OperatorToken("AND", 1);
        public static readonly OperatorToken AndBit = new OperatorToken("ANDBIT", 1);
        public static readonly OperatorToken Or = new OperatorToken("OR", 0);
        public static readonly OperatorToken OrBit = new OperatorToken("ORBIT", 0);
        public static readonly OperatorToken Not = new OperatorToken("NOT", 2);
        
        public static readonly SymbolToken Semi = new SymbolToken("SEMI");
        public static readonly SymbolToken Assignment = new SymbolToken("ASSIGNMENT");

        public static readonly SymbolToken LeftParentesis = new SymbolToken("LEFTPARENTESIS");
        public static readonly SymbolToken RightParentesis = new SymbolToken("RIGHTPARENTESIS");

        public static readonly SymbolToken Comma = new SymbolToken("COMMA");
        //public static readonly SymbolToken Dot = new SymbolToken("DOT");
        //public static readonly SymbolToken Colon = new SymbolToken("COLON");

        public static readonly SymbolToken BeginBlock = new SymbolToken("BEGINBLOCK");
        public static readonly SymbolToken EndBlock = new SymbolToken("ENDBLOCK");

        public static readonly KeywordToken Print = new KeywordToken("PRINT");
        public static readonly KeywordToken Var = new KeywordToken("VAR");
        public static readonly KeywordToken Read_num = new KeywordToken("READ_NUM");
        public static readonly KeywordToken For = new KeywordToken("FOR");
        public static readonly KeywordToken To = new KeywordToken("TO");
        public static readonly KeywordToken If = new KeywordToken("IF");
        public static readonly KeywordToken Then = new KeywordToken("THEN");
        public static readonly KeywordToken Else = new KeywordToken("ELSE");
        public static readonly KeywordToken While = new KeywordToken("WHILE");
        public static readonly KeywordToken Break = new KeywordToken("BREAK");
    }

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

    public class KeywordToken : Token
    {        
        public KeywordToken(string value)
            : base(value) {}
    }
    
    public class IdentifierToken : Token
    {        
        public IdentifierToken(string value)
            : base(value) { }
    }

    public class StringToken : Token
    {
        private string _value;

        public StringToken(string value)
            : base(value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value.ToString(); }
        }
    }
    
    public class NumberToken : Token
    {
        private float _value;

        public NumberToken(string value)
            : base(value)
        {
            _value = float.Parse(value.ToString(),
                            System.Globalization.CultureInfo.InvariantCulture);
        }

        public float Value
        {
            get
            {
                return _value;
            }
        }
    }

    //#dd/mm/yyyy#
    public class DateTimeToken : Token
    {
        private DateTime _value;

        public DateTimeToken(string value)
            : base(value)
        {
            _value = DateTime.Parse(value);

            //_value = float.Parse(value.ToString(),
            //                System.Globalization.CultureInfo.InvariantCulture);
        }

        public DateTime Value
        {
            get
            {
                return _value;
            }
        }
    }

    // @ObjectX@
    public class ObjectToken : Token
    {
        private object _value;

        public ObjectToken(string value)
            : base(value)
        {            
            string idObj = value;
            Type t = Type.GetType(idObj, false, true);            
            _value = Activator.CreateInstance(t);   
        }

        public object Value
        {
            get { return _value; }
        }
    }

    public class SymbolToken : Token
    {
        public SymbolToken(string value)
            : base(value) { }
    }

    public abstract class Token
    {
        private string _name = "";

        public Token(string s)
        {
            _name = s;
        }

        public string Name
        {
            get { return _name; }            
        }

        public override string ToString()
        {
            return _name;
        }

    }

}