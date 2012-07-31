using System;
using System.Collections.Generic;
using System.Text;

namespace AnTScript
{
    public sealed class Parser
    {

        #region Private fields

        private int index;
        private IList<Token> tokens;

        #endregion

        #region Constructor

        public Parser(IList<Token> tokens)
        {
            this.tokens = tokens;
            index = 0;
            _result = ParseStmt();

            if (index != this.tokens.Count)
                throw new System.Exception("expected EOF");
        }

        #endregion

        #region Properties

        private readonly Stmt _result;
        public Stmt Result
        {
            get { return _result; }
        }

        #endregion 

        #region Private methods

        private Stmt ParseStmt()
        {
            Stmt result;

            if (index == tokens.Count)
            {
                throw new System.Exception("expected statement, got EOF");
            }

            // print
            if (tokens[index] == Tokens.Print)
            {
                MoveNext();
                Print print = new Print();
                print.Expr = ParseExpr();
                result = print;
            }

            // var
            else if (tokens[index] == Tokens.Var)
            {
                MoveNext();
                
                DeclareVar declareVar = new DeclareVar();

                if (index < tokens.Count &&
                    tokens[index] is IdentifierToken)
                {                    
                    declareVar.Ident = ((IdentifierToken)tokens[index]).Name;
                }
                else
                {
                    throw new System.Exception("expected variable name after 'var'");
                }                

                MoveNext();

                if (index == tokens.Count ||
                    tokens[index] != Tokens.Assignment)
                {
                    throw new System.Exception("expected = after 'var ident'");
                }

                MoveNext();

                declareVar.Expr = ParseExpr();
                result = declareVar;
            }

            // read_num
            else if (tokens[index] == Tokens.Read_num)
            {                
                ReadNum readNum = new ReadNum();
                MoveNext();

                if (index < tokens.Count &&
                    tokens[index] is IdentifierToken)
                {                    
                    readNum.Ident = ((IdentifierToken)tokens[index]).Name;
                    MoveNext();
                    result = readNum;
                }
                else
                {
                    throw new System.Exception("expected variable name after 'read_int'");
                }
            }

            // for
            else if (tokens[index] == Tokens.For)
            {                
                ForLoop forLoop = new ForLoop();
                MoveNext();

                if (index < tokens.Count &&
                    tokens[index] is IdentifierToken)
                {                    
                    forLoop.Ident = ((IdentifierToken)tokens[index]).Name;
                }
                else
                {
                    throw new System.Exception("expected identifier after 'for'");
                }

                MoveNext();

                if (index == tokens.Count ||
                    tokens[index] != Tokens.Assignment)
                {
                    throw new System.Exception("for missing '='");
                }

                MoveNext();

                forLoop.From = ParseExpr();

                if (index == tokens.Count || 
                    tokens[index] != Tokens.To)
                {
                    throw new System.Exception("expected 'to' after for");
                }

                MoveNext();

                forLoop.To = ParseExpr();

                if (index == tokens.Count ||
                    tokens[index] != Tokens.BeginBlock)
                {
                    throw new System.Exception("expected 'do' after from expression in for loop");
                }

                MoveNext();

                forLoop.Body = ParseStmt();
                result = forLoop;

                if (index == tokens.Count ||
                    tokens[index] != Tokens.EndBlock)
                {
                    throw new System.Exception("unterminated 'for' loop body");
                }

                MoveNext();

            }

            // if
            else if (tokens[index] == Tokens.If)
            {
                IfStmt ifStmt = new IfStmt();                
                MoveNext();

                ifStmt.TestExpr = ParseExpr();
                
                if (index == tokens.Count ||
                    tokens[index] != Tokens.Then)
                {
                    throw new System.Exception("expected 'then' after if");
                }
                MoveNext();

                ifStmt.BodyIf = ParseStmt();

                // Does else
                ifStmt.DoElse = MaybeEat(Tokens.Else);
                if (ifStmt.DoElse)
                    ifStmt.BodyElse = ParseStmt();

                result = ifStmt;

                if (index == tokens.Count ||
                    tokens[index] != Tokens.EndBlock)
                {
                    throw new System.Exception("unterminated 'if' body");
                }
                MoveNext();
            }


            // assignment
            else if (tokens[index] is IdentifierToken)
            {
                Assign assign = new Assign();
                assign.Ident = ((IdentifierToken)tokens[index]).Name;

                MoveNext();

                if (index == tokens.Count ||
                    tokens[index] != Tokens.Assignment)
                {
                    throw new System.Exception("expected '='");
                }

                MoveNext();

                assign.Expr = ParseExpr();
                result = assign;
            }

            //else if (tokens[index] == Tokens.Else)
            //{

            //}

            else
            {
                throw new System.Exception("parse error at token " + index + ": " + tokens[index].Name);
            }

            if ((index < tokens.Count && tokens[index] == Tokens.Semi))
            {
                MoveNext();

                //if (index < tokens.Count &&
                //    tokens[index] != Tokens.EndBlock)
                if ((index < tokens.Count && tokens[index] != Tokens.EndBlock)
                    && tokens[index] != Tokens.Else)
                {
                    Sequence sequence = new Sequence();
                    sequence.First = result;
                    sequence.Second = ParseStmt();
                    result = sequence;
                }
            }

            return result;
        }

        private Expr ParseExpr()
        {
            return ParseExpr(0);
        }

        private Expr ParseExpr(int precedence)
        {
            Expr left = ParseFactor();

            while (true)
            {
                OperatorToken ot = tokens[index] as OperatorToken;
                if (ot == null)
                    return left;

                int prec = ot.Precedence;
                if (prec >= precedence)
                {
                    BinExpr binExpr = new BinExpr();
                    binExpr.Op = TokenToBinOp(tokens[index]);
                    MoveNext();
                    Expr right = ParseExpr(prec);
                    binExpr.Left = left;
                    binExpr.Right = right;
                    left = binExpr;
                }
                else
                    return left;
            }
        }

        private Expr ParseFactor()
        {
            if (this.index == this.tokens.Count)
            {
                throw new System.Exception("expected expression, got EOF");
            }
            if (this.tokens[this.index] is StringToken)
            {                
                StringLiteral stringLiteral = new StringLiteral();
                stringLiteral.Value = ((StringToken)this.tokens[this.index]).Value;
                MoveNext();
                return stringLiteral;
            }
            else if (this.tokens[this.index] is NumberToken)
            {                
                NumericLiteral numLiteral = new NumericLiteral();
                numLiteral.Value = ((NumberToken)this.tokens[this.index]).Value;
                MoveNext();
                return numLiteral;
            }
            else if (this.tokens[this.index] is IdentifierToken)
            {                
                Variable var = new Variable();
                var.Ident = ((IdentifierToken)this.tokens[this.index]).Name;
                MoveNext();
                return var;
            }
            else if (this.tokens[this.index] == Tokens.LeftParentesis)
            {
                // Eat LeftParenthesis             
                MoveNext();
                Expr result = ParseExpr();
                Eat(Tokens.RightParentesis);
                return result;
            }
            else if (IsUnaryOperator(this.tokens[index]))
            {
                // Unary Expression
                UnaryExpr result = new UnaryExpr();

                if (MaybeEat(Tokens.Sub))
                {
                    result.Op = TokenToBinOp(Tokens.Sub);
                }
                else if (MaybeEat(Tokens.Add))
                {
                    result.Op = TokenToBinOp(Tokens.Add);
                }
                else if (MaybeEat(Tokens.Not))
                {
                    result.Op = TokenToBinOp(Tokens.Not);
                }
                else
                    throw new System.Exception(string.Format(
                        "Operator '{0}' is not supported in unary expressions",
                        tokens[index]));

                result.Expression = ParseFactor();
                return result;
            }
            else
            {
                throw new System.Exception("expected string literal, int literal, or variable");
            }
        }

        #endregion

        #region Utils

        private bool IsUnaryOperator(object token)
        {
            return token == Tokens.Add ||
                token == Tokens.Sub ||
                token == Tokens.Not
                ;
        }

        private BinOp TokenToBinOp(object token)
        {
            if (token == Tokens.Add)
                return BinOp.Add;
            if (token == Tokens.Sub)
                return BinOp.Sub;
            if (token == Tokens.Mul)
                return BinOp.Mul;
            if (token == Tokens.Div)
                return BinOp.Div;
            if (token == Tokens.Or)
                return BinOp.Or;
            if (token == Tokens.And)
                return BinOp.And;
            if (token == Tokens.Not)
                return BinOp.Not;
            if (token == Tokens.Equal)
                return BinOp.Equal;
            if (token == Tokens.NotEqual)
                return BinOp.NotEqual;
            if (token == Tokens.LessThan)
                return BinOp.LessThan;
            if (token == Tokens.GreaterThan)
                return BinOp.GreaterThan;
            if (token == Tokens.LessThanOrEqual)
                return BinOp.LessThanOrEqual;
            if (token == Tokens.GreaterThanOrEqual)
                return BinOp.GreaterThanOrEqual;
            else
                throw new System.Exception("invalid code operator");

        }

        private void MoveNext()
        {
            if (index < tokens.Count)
                index++;
            else
                throw new System.Exception("Token out of range.");
        }

        private bool Eat(SymbolToken token)
        {
            if (tokens[index] != token)
                throw new System.Exception(string.Format(
                    "Syntax error. Expected '{0}' but was '{1}'", token, tokens[index]));

            MoveNext();
            return true;
        }

        private bool MaybeEat(Token token)
        {
            if (tokens[index] == token)
            {
                MoveNext();
                return true;
            }
            else
                return false;
        }

        #endregion

    }
}