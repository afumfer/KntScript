
/* <stmt> := var <ident> = <expr>
	| <ident> = <expr>
	| for <ident> = <expr> to <expr> do <stmt> end
	| read_int <ident>
	| print <expr>
	| <stmt> ; <stmt>
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

// for <ident> = <expr> to <expr> do <stmt> end
public class ForLoop : Stmt
{
    public string Ident;
    public Expr From;
    public Expr To;
    public Stmt Body;
}

// read_int <ident>
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
 *  | <num>
 *  | <bin_expr>
 *  | <ident>
 *  | <unary_expr>
 */
public abstract class Expr
{
}

// <string> := " <string_elem>* "
public class StringLiteral : Expr
{
	public string Value;
}

// <int> := <digit>+
public class IntLiteral : Expr
{
    public int Value;    
}

public class NumericLiteral : Expr
{    
	public float Value;
}

// <ident> := <char> <ident_rest>*
// <ident_rest> := <char> | <digit>
public class Variable : Expr
{
	public string Ident;
}

// <bin_expr> := <expr> <bin_op> <expr>
public class BinExpr : Expr
{
	public Expr Left;
	public Expr Right;
	public BinOp Op;
}

// <unary_expr> := <bin_op> <expr>
public class UnaryExpr : Expr
{
    public BinOp Op;
    public Expr Expression;    
}


// <bin_op> := + | - | * | /
public enum BinOp
{
	Add,
	Sub,
	Mul,
	Div
}