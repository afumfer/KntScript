using System.Collections.Generic;
using System.Reflection;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace AnTScript
{
    public sealed class CodeRun
    {
        Dictionary<string, object> symbolTable;
        TextBox textOut;

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

                //// set the initial value, no hace falta, ya se hace en CodDeclare,
                //// esto es otra forma de hacerlo.
                //Assign assign = new Assign();
                //assign.Ident = declare.Ident;
                //assign.Expr = declare.Expr;
                //RunStmt(assign);
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
                //assign.Identifier = read.Identifier;
                assign.Ident = read.Ident;

                NumericLiteral numLiteral = new NumericLiteral();
                numLiteral.Value = CodeReadNum();

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

                NumericLiteral numFrom = new NumericLiteral();
                NumericLiteral numTo = new NumericLiteral();

                Assign assignFrom = new Assign();
                //assignFrom.Identifier = forLoop.Identifier;
                assignFrom.Ident = forLoop.Ident;

                assignFrom.Expr = forLoop.From;
                RunStmt(assignFrom);

                numFrom.Value = (float)GenExpr(forLoop.From);
                numTo.Value = (float)GenExpr(forLoop.To);

                while (numFrom.Value <= numTo.Value)
                {
                    RunStmt(forLoop.Body);
                    numFrom.Value++;
                    assignFrom.Expr = numFrom;
                    RunStmt(assignFrom);
                }
            }

            else
            {
                throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
            }

        } // RunStmt

        // TODO: pendiente de implmentar la comprobación de tipos.
        // Apadir el parámetro, ... System.Type expectedType)
        private object GenExpr(Expr expr) 
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
                bool boolLeft;
                bool boolRight;
                object res;

                if ((float)left != 0)
                    boolLeft = true;
                else
                    boolLeft = false;

                if ((float)right != 0)
                    boolRight = true;
                else
                    boolRight = false;

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
                    case BinOp.Or:
                        if (boolLeft || boolRight)
                            res = 1.0f;
                        else
                            res = 0.0f;                        
                        break;
                    case BinOp.And:
                        if (boolLeft && boolRight)
                            res = 1.0f;
                        else
                            res = 0.0f;                         
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
                    case BinOp.Not:
                        if ((float)ex == 0)
                            res = 1.0f;
                        else
                            res = 0.0f;                       
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

        } // GenExpr

        #region Utils

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

        private float CodeReadNum()
        {
            // TODO: Esto es para pruebas
            float value = 0;
            InputBoxResult test = InputBox.Show(
                             "Read int  " + "\n" + " .... " +
                             "  Prompt " + "\n" + "...."
                             , "Title", "0", 100, 100);

            if (test.ReturnCode == DialogResult.OK)
                value = float.Parse(test.Text);

            return value;
        }


        private Type TypeOfExpr(Expr expr)
        {
            if (expr is StringLiteral)
            {
                return typeof(string);
            }
            else if (expr is NumericLiteral)
            {
                return typeof(float);
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
            else if (expr is UnaryExpr)
            {
                return typeof(UnaryExpr);
            }
            else
            {
                throw new System.Exception("don't know how to calculate the type of " + expr.GetType().Name);
            }
        }

        #endregion

    } // CodeRun class

} // namespace