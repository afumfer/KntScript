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

    public sealed class ParserTree
    {
        TreeView treeAst;

        public ParserTree(Stmt stmt, TreeView treeAst)
        {

            this.treeAst = treeAst;

            TreeNode nodeRoot = treeAst.Nodes.Add("Program");

            // Go gen tree Ast!
            GenTreeStmt(stmt, nodeRoot);

        }

        #region Gen Tree

        private void GenTreeStmt(Stmt stmt, TreeNode node)
        {

            if (stmt is Sequence)
            {
                TreeNode seqParent = node.Nodes.Add("Sequence");
                Sequence seq = (Sequence)stmt;

                TreeNode seqFirst = seqParent.Nodes.Add("Seq.First");
                GenTreeStmt(seq.First, seqFirst);

                TreeNode seqSecond = seqParent.Nodes.Add("Seq.Second");
                GenTreeStmt(seq.Second, seqSecond);
            }

            else if (stmt is DeclareVar)
            {
                // declare a local            
                DeclareVar declare = (DeclareVar)stmt;
                TreeNode seqDeclare = node.Nodes.Add("Declare: " + declare.Ident);

                // set the initial value
                Assign assign = new Assign();
                assign.Ident = declare.Ident;
                assign.Expr = declare.Expr;
                TreeNode seqAssign = seqDeclare.Nodes.Add(" - With Assign: " + assign.Ident);
                GenTreeStmt(assign, seqAssign);
            }

            else if (stmt is Assign)
            {
                TreeNode seqAssign2 = node.Nodes.Add("Assign");
                Assign assign = (Assign)stmt;
                GenTreeExpr(assign.Expr, this.TypeOfExpr(assign.Expr), seqAssign2);
                GenTreeStore(assign.Ident, this.TypeOfExpr(assign.Expr), seqAssign2);
            }

            else if (stmt is Print)
            {
                TreeNode print = node.Nodes.Add("Print");
                GenTreeExpr(((Print)stmt).Expr, typeof(string), node);
            }

            else if (stmt is ReadNum)
            {
                TreeNode readInt = node.Nodes.Add("Read");
                GenTreeStore(((ReadNum)stmt).Ident, typeof(int), readInt);
            }

            else if (stmt is ForLoop)
            {

                //// example: 
                //// for x = 0 to 100 do
                ////   print "hello";
                //// end;

                // x = 0
                ForLoop forLoop = (ForLoop)stmt;
                TreeNode forLoopNode = node.Nodes.Add("ForLoop: ");

                Assign assignFrom = new Assign();
                assignFrom.Ident = forLoop.Ident;
                assignFrom.Expr = forLoop.From;

                TreeNode assingNodeFrom = forLoopNode.Nodes.Add("From: ");
                this.GenTreeStmt(assignFrom, assingNodeFrom);

                TreeNode loopBody = forLoopNode.Nodes.Add("LoopBody: ");
                this.GenTreeStmt(forLoop.Body, loopBody);

                forLoopNode.Nodes.Add("* From Ident (Increment) *");
                this.GenTreeStore(forLoop.Ident, typeof(int), forLoopNode);

                forLoopNode.Nodes.Add("* To (Test Loop) *");
                this.GenTreeExpr(forLoop.To, typeof(int), forLoopNode);
            }

            else
            {
                throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
            }

        }

        private void GenTreeExpr(Expr expr, System.Type expectedType, TreeNode node)
        {
            System.Type deliveredType;
            string sValue;

            if (expr is StringLiteral)
            {
                deliveredType = typeof(string);

                sValue = ((StringLiteral)expr).Value.ToString();
                node.Nodes.Add("Expr: StringLiteral " + sValue);

            }
            else if (expr is NumericLiteral)
            {
                deliveredType = typeof(int);

                sValue = ((NumericLiteral)expr).Value.ToString();
                node.Nodes.Add("Expr: NumLiteral " + sValue);
            }
            else if (expr is Variable)
            {
                string ident = ((Variable)expr).Ident;
                deliveredType = this.TypeOfExpr(expr);

                sValue = ident + " " + deliveredType.ToString();
                node.Nodes.Add("Expr: ?? Variable " + sValue);
            }
            else if (expr is BinExpr)
            {
                deliveredType = typeof(BinExpr);
                BinExpr be = (BinExpr)expr;
                TreeNode n = node.Nodes.Add("BinExp: " + be.Op.ToString());
                GenTreeExpr(be.Left, deliveredType, n);
                GenTreeExpr(be.Right, deliveredType, n);
            }
            else if (expr is UnaryExpr)
            {
                deliveredType = typeof(UnaryExpr);
                UnaryExpr ue = (UnaryExpr)expr;
                TreeNode n = node.Nodes.Add("UnaryExp: " + ue.Op.ToString());
                GenTreeExpr(ue.Expression, deliveredType, n);
            }
            else
            {
                throw new System.Exception("don't know how to generate " + expr.GetType().Name);
            }

        }

        private void GenTreeStore(string name, System.Type type, TreeNode node)
        {
            node.Nodes.Add("Store: " + name + " - " + type.ToString());
        }

        #endregion

        #region Utils

        private Type TypeOfExpr(Expr expr)
        {
            if (expr is StringLiteral)
            {
                return typeof(StringLiteral);
            }
            else if (expr is NumericLiteral)
            {
                return typeof(NumericLiteral);
            }
            else if (expr is Variable)
            {
                return typeof(object);
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

        #endregion

    }

}