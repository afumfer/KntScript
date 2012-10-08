using System;
using System.Collections.Generic;
using System.Text;

namespace AnTScript
{

    /* <stmt> := var <ident> = <expr>
     *  | <ident> = <expr>
     *  | for <ident> = <expr> to <expr> <stmt> end for 
     *  | while <expr> <stmt> end while
     *  | if <expr> <stmt> else <stmt> end if
     *  | read_int <ident>
     *  | print <expr>
     *  | <stmt> ; <stmt>
     *  | <func_call>        
    */

    public abstract class Stmt
    {
    }

    // var <ident> = <expr>
    public class DeclareVar : Stmt
    {        
        public string Ident;        
        public Expr Expr;
    }

    // print <expr>
    public class Print : Stmt
    {
        public Expr Expr;
    }

    // <ident> = <expr>
    public class Assign : Stmt
    {        
        public string Ident;     
        public Expr Expr;
    }

    // for <ident> = <expr> to <expr> <stmt> end for
    public class ForLoop : Stmt
    {        
        public string Ident;     
        public Expr From;
        public Expr To;
        public Stmt Body;
    }

    // foreach <ident> in <expr> <stmt> end foreach
    public class ForEachLoop : Stmt
    {
        public string Ident;
        public Expr Colec;        
        public Stmt Body;
    }

    // if <expr> then <stmt> end if |
    // if <expr> then <stmt> else <stmt> end if 
    public class IfStmt : Stmt
    {        
        public Expr TestExpr;        
        public Stmt BodyIf;
        public Stmt BodyElse;
        public bool DoElse;
    }

    // while <expr> <stmt> end while
    public class WhileStmt : Stmt
    {
        public Expr TestExpr;
        public Stmt Body;
    }

    // break
    public class BreakStmt : Stmt
    {
        public string Tag;        
    }

    // <FunctionStmt(<arg>)
    public class FunctionStmt : Stmt
    {
        public FunctionExpr Function;
    }

    // read_num <ident>
    public class ReadNum : Stmt
    {
        public string Ident;        
    }

    // <stmt> ; <stmt>
    public class Sequence : Stmt
    {
        public Stmt First;
        public Stmt Second;
    }


    /* <expr> := <string>
     *  | <int>
     *  | <float>
     *  | <double>
     *  | <decimal>
     *  | <DateTime>
     *  | <bin_expr>
     *  | <ident>
     *  | <unary_expr>
     *  | <fun_expr>
     *  | <newobject_exp>
     */
    public abstract class Expr
    {
    }

    // <string> := " <string_elem>* "
    public class StringVal : Expr
    {        
        public string Value;     
    }

    // <int> := <digit>+
    public class IntVal : Expr
    {
        public int Value;
    }

    // <float>
    public class FloatVal : Expr
    {        
        public float Value;     
    }

    // <double>
    public class DoubleVal : Expr
    {
        public double Value;
    }

    // <decimal>
    public class DecimalVal : Expr
    {
        public decimal Value;
    }

    // <DateTime>
    public class DateTimeVal : Expr
    {
        public DateTime Value;
    }

    // <bool>
    public class BoolVal : Expr
    {
        public bool Value;
    }

    // <ident> := <char> <ident_rest>*
    // <ident_rest> := <char> | <digit>
    public class Variable : Expr
    {        
        public string Ident;     
    }

    // <func_call> := <ident> (<args>)
    public class FunctionExpr : Expr
    {
        public string FunctionName;
        public List<Expr> Args;

        public FunctionExpr()
        {
            Args = new List<Expr>();
        }
    }

    // <new_object> := new <ident> (<args>)
    public class NewObjectExpr : Expr
    {
        public string ClassName;
        public List<Expr> Args;

        public NewObjectExpr()
        {
            Args = new List<Expr>();
        }
    }

    // <bin_expr> := <expr> <bin_op> <expr>
    public class BinaryExpr : Expr
    {
        public BinOp Op;
        public Expr Left;
        public Expr Right;        
    }

    // <unary_expr> := <bin_op> <expr>
    public class UnaryExpr : Expr
    {
        public BinOp Op;
        public Expr Expression;
    }

    // <bin_op> := + | - | * | / | && | ! | || 
    public enum BinOp
    {
        Add,
        Sub,
        Mul,
        Div,
        Or,
        And, 
        Not,
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual
    }
}

