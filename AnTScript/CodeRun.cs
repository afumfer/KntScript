using System.Collections.Generic;
using System.Reflection;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AnTScript;

public sealed class CodeRun
{
    Dictionary<string, object> symbolTable;
    TextBox textOut;
    
    //public CodeRun(Stmt stmt, string moduleName, TextBox textOut)
    public CodeRun(Stmt stmt, TextBox textOut)
    {
        this.textOut = textOut;     
        symbolTable = new Dictionary<string, object>();

        // Go Run!
        RunStmt(stmt);                
    }

	private void RunStmt(Stmt stmt)
	{
		
        if (stmt is Sequence)
		{
			Sequence seq = (Sequence)stmt;
			RunStmt(seq.First);
            RunStmt(seq.Second);
		}		
        
        else if (stmt is DeclareVar)
		{
			// declare
			DeclareVar declare = (DeclareVar)stmt;

            CodeDeclare(declare);

            //// set the initial value, no hace falta, ya se hace en CodDeclare
            //Assign assign = new Assign();
            //assign.Ident = declare.Ident;
            //assign.Expr = declare.Expr;
            //this.RunStmt(assign);
		}        
        
        else if (stmt is Assign)
        {
	        Assign assign = (Assign)stmt;
            CodeStore(assign);
        }			

        else if (stmt is Print)
		{
            Print print = (Print)stmt;
            CodePrint(print);            
		}	

		else if (stmt is ReadNum)
		{
            ReadNum read = (ReadNum)stmt;
                                  
            Assign assign = new Assign();
            assign.Ident = read.Ident;

            NumericLiteral numLiteral = new NumericLiteral();
            numLiteral.Value = CodeReadInt();
            
            assign.Expr = numLiteral;
            RunStmt(assign);
		}

		else if (stmt is ForLoop)
		{
			// example: 
			// for x = 0 to 100 do
			//   print "hello";
			// end;
		
            ForLoop forLoop = (ForLoop)stmt;
            
            NumericLiteral intFrom = new NumericLiteral();
            NumericLiteral intTo = new NumericLiteral();
           				
            Assign assignFrom = new Assign();
            
            assignFrom.Ident = forLoop.Ident;
            assignFrom.Expr = forLoop.From;
            RunStmt(assignFrom);

            //intFrom.Value = (int)GenExpr(forLoop.From);
            //intTo.Value = (int)GenExpr(forLoop.To);

            intFrom.Value = (float)GenExpr(forLoop.From);
            intTo.Value = (float)GenExpr(forLoop.To);

            while (intFrom.Value <= intTo.Value)
            {
                RunStmt(forLoop.Body);
                intFrom.Value++;
                assignFrom.Expr = intFrom;
                RunStmt(assignFrom);
            }

		}

        // 
		else
		{
			throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
		}

	}

    private void CodeDeclare(DeclareVar declare)
    {
        if (!this.symbolTable.ContainsKey(declare.Ident))
            symbolTable.Add(declare.Ident, GenExpr(declare.Expr));
        else
            throw new System.Exception(" variable '" + declare.Ident + "' already declared");
    }

    private void CodeStore(Assign assign)
    {
        if (this.symbolTable.ContainsKey(assign.Ident))
            symbolTable[assign.Ident] = GenExpr(assign.Expr);
        else
            throw new System.Exception(" undeclared variable '" + assign.Ident);
    }

    private void CodePrint(Print print)
    {
        string s = GenExpr(print.Expr).ToString();
        if (s == @"\")
            textOut.AppendText("\r\n");
        else
            textOut.AppendText(@s);
        
    }

    private int CodeReadInt()
    {
        // TODO: Esto es para pruebas
        int value = 0; 
        InputBoxResult test = InputBox.Show(
                         "Read int  " + "\n" + " .... " +
                         "  Prompt " + "\n" + "...."
                         , "Title", "0", 100, 100);

        if (test.ReturnCode == DialogResult.OK)
            value = int.Parse(test.Text);
        
        return value;    
    }

    private object GenExpr(Expr expr) // , System.Type expectedType)
    {
        Type deliveredType;

        if (expr is StringLiteral)
        {
            deliveredType = typeof(string);                        
            return ((StringLiteral)expr).Value;
        }
        else if (expr is NumericLiteral)
        {
            deliveredType = typeof(float);                        
            return ((NumericLiteral)expr).Value;
        }
        else if (expr is Variable)
        {
            string ident = ((Variable)expr).Ident;
            deliveredType = this.TypeOfExpr(expr);
            if (this.symbolTable.ContainsKey(ident))
            {
                return symbolTable[ident];
            }                        
        }
        else if (expr is BinExpr)
        {
            BinExpr be = (BinExpr)expr;

            object left = GenExpr(be.Left);
            object right = GenExpr(be.Right);
            object res;
                        
            switch (be.Op)
            {
                case BinOp.Add:
                    res = (float)left + (float)right;
                    break;
                case BinOp.Sub:
                    res = (float)left - (float)right;
                    break;
                case BinOp.Mul:
                    res = (float)left * (float)right;
                    break;
                case BinOp.Div:
                    res = (float)left / (float)right;
                    break;

                //
                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported", be.Op));
            }

            return res;
        }
        else if (expr is UnaryExpr)
        {
            UnaryExpr ue = (UnaryExpr)expr;

            object ex = GenExpr(ue.Expression);            
            object res;

            switch (ue.Op)
            {
                case BinOp.Add:
                    res = (float)ex;
                    break;
                case BinOp.Sub:
                    res = -1 * (float)ex;
                    break;
                //
                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported", ue.Op));
            }

            return res;
        }


        // Para Boxing o para deover error ?? 
        //if (deliveredType != expectedType)
        //{
        //    if (deliveredType == typeof(int) &&
        //        expectedType == typeof(string))
        //    {
        //        this.il.Emit(Emit.OpCodes.Box, typeof(int));
        //        this.il.Emit(Emit.OpCodes.Callvirt, typeof(object).GetMethod("ToString"));
        //    }
        //    else
        //    {
        //        throw new System.Exception("can't coerce a " + deliveredType.Name + " to a " + expectedType.Name);
        //    }
        //} 

        return null;        
    }


    private Type TypeOfExpr(Expr expr)
	{
		if (expr is StringLiteral)
		{
			return typeof(string);
		}
		else if (expr is NumericLiteral)
		{
			return typeof(int);
		}
		else if (expr is Variable)
		{
            Variable var = (Variable)expr;
		    if (this.symbolTable.ContainsKey(var.Ident))
		    {
                //Emit.LocalBuilder locb = this.symbolTable[var.Ident];
                //return locb.LocalType;
                // TODO: !!! arreglar esto, tiene que devolver el tipo real.
                return typeof(object);
		    }
		    else
		    {                
			    throw new System.Exception("undeclared variable '" + var.Ident + "'");                
		    }
		}
        else if (expr is BinExpr)
        {
            return typeof(BinExpr);
        }
		else
		{
			throw new System.Exception("don't know how to calculate the type of " + expr.GetType().Name);
		}
    }

}
