using Collections = System.Collections.Generic;
using Text = System.Text;

namespace AnTScript
{

    public sealed class Parser
    {
        private int index;
        private Collections.IList<object> tokens;
        private readonly Stmt result;

        public Parser(Collections.IList<object> tokens)
        {
            this.tokens = tokens;
            this.index = 0;
            this.result = this.ParseStmt();

            if (this.index != this.tokens.Count)
                throw new System.Exception("expected EOF");
        }

        public Stmt Result
        {
            get { return result; }
        }

        private Stmt ParseStmt()
        {
            Stmt result;

            if (this.index == this.tokens.Count)
            {
                throw new System.Exception("expected statement, got EOF");
            }

            // <stmt> := print <expr> 

            // <expr> := <string>
            // | <int>
            // | <arith_expr>
            // | <ident>
            if (this.tokens[this.index].Equals("print"))
            {
                MoveNext();
                Print print = new Print();
                print.Expr = this.ParseExpr();
                result = print;
            }
            else if (this.tokens[this.index].Equals("var"))
            {
                MoveNext();
                DeclareVar declareVar = new DeclareVar();

                if (this.index < this.tokens.Count &&
                    this.tokens[this.index] is string)
                {
                    declareVar.Ident = (string)this.tokens[this.index];
                }
                else
                {
                    throw new System.Exception("expected variable name after 'var'");
                }

                MoveNext();

                if (this.index == this.tokens.Count ||
                    this.tokens[this.index] != Tokens.Assignment)
                {
                    throw new System.Exception("expected = after 'var ident'");
                }

                MoveNext();

                declareVar.Expr = this.ParseExpr();
                result = declareVar;
            }
            else if (this.tokens[this.index].Equals("read_num"))
            {
                MoveNext();
                ReadNum readNum = new ReadNum();

                if (this.index < this.tokens.Count &&
                    this.tokens[this.index] is string)
                {
                    readNum.Ident = (string)this.tokens[this.index++];
                    result = readNum;
                }
                else
                {
                    throw new System.Exception("expected variable name after 'read_int'");
                }
            }
            else if (this.tokens[this.index].Equals("for"))
            {
                MoveNext();
                ForLoop forLoop = new ForLoop();

                if (this.index < this.tokens.Count &&
                    this.tokens[this.index] is string)
                {
                    forLoop.Ident = (string)this.tokens[this.index];
                }
                else
                {
                    throw new System.Exception("expected identifier after 'for'");
                }

                MoveNext();

                if (this.index == this.tokens.Count ||
                    this.tokens[this.index] != Tokens.Assignment)
                {
                    throw new System.Exception("for missing '='");
                }

                MoveNext();

                forLoop.From = this.ParseExpr();

                if (this.index == this.tokens.Count ||
                    !this.tokens[this.index].Equals("to"))
                {
                    throw new System.Exception("expected 'to' after for");
                }

                MoveNext();

                forLoop.To = this.ParseExpr();

                if (this.index == this.tokens.Count ||
                    this.tokens[this.index] != Tokens.BeginBlock)
                {
                    throw new System.Exception("expected 'do' after from expression in for loop");
                }

                MoveNext();

                forLoop.Body = this.ParseStmt();
                result = forLoop;

                if (this.index == this.tokens.Count ||
                    this.tokens[this.index] != Tokens.EndBlock)
                {
                    throw new System.Exception("unterminated 'for' loop body");
                }

                MoveNext();

            }
            else if (this.tokens[this.index] is string)
            {
                // assignment

                Assign assign = new Assign();
                assign.Ident = (string)this.tokens[this.index++];

                if (this.index == this.tokens.Count ||
                    this.tokens[this.index] != Tokens.Assignment)
                {
                    throw new System.Exception("expected '='");
                }

                MoveNext();

                assign.Expr = this.ParseExpr();
                result = assign;
            }
            else
            {
                throw new System.Exception("parse error at token " + this.index + ": " + this.tokens[this.index]);
            }

            if ((this.index < this.tokens.Count && this.tokens[this.index] == Tokens.Semi))
            {
                MoveNext();

                if (this.index < this.tokens.Count &&
                    this.tokens[this.index] != Tokens.EndBlock)
                {
                    Sequence sequence = new Sequence();
                    sequence.First = result;
                    sequence.Second = this.ParseStmt();
                    result = sequence;
                }
            }

            return result;
        }

        // TODO: Limpiar
        //   Versión sin la precedencia de operadores
        //private Expr ParseExpr()
        //{
        //    Expr left = ParseFactor();

        //    while (true)
        //    {
        //        if (!IsOperator(tokens[index]))
        //            return left;

        //        else
        //        {
        //            BinExpr binExpr = new BinExpr();
        //            binExpr.Op = TokenToOp(tokens[index]);
        //            index++;
        //            Expr right = ParseExpr();
        //            binExpr.Left = left;
        //            binExpr.Right = right;
        //            return binExpr;
        //        }
        //    }
        //}

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
                    binExpr.Op = TokenToOp(tokens[index]);
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

            if (this.tokens[this.index] is Text.StringBuilder)
            {
                string value = ((Text.StringBuilder)this.tokens[this.index++]).ToString();
                StringLiteral stringLiteral = new StringLiteral();
                stringLiteral.Value = value;
                return stringLiteral;
            }
            else if (this.tokens[this.index] is float)
            {
                float floatValue = (float)this.tokens[this.index++];
                NumericLiteral floatLiteral = new NumericLiteral();
                floatLiteral.Value = floatValue;
                return floatLiteral;
            }
            else if (this.tokens[this.index] is string)
            {
                string ident = (string)this.tokens[this.index++];
                Variable var = new Variable();
                var.Ident = ident;
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
            else if (IsOperator(this.tokens[index]))
            {
                // Unary Expression
                UnaryExpr result = new UnaryExpr();

                if (MaybeEat(Tokens.Sub))
                {
                    result.Op = TokenToOp(Tokens.Sub);
                    result.Expression = ParseFactor();
                    return result;
                }
                else if (MaybeEat(Tokens.Add))
                {
                    result.Op = TokenToOp(Tokens.Add);
                    result.Expression = ParseFactor();
                    return result;
                }
                else
                    throw new System.Exception(string.Format(
                        "Operator '{0}' is not supported in unary expressions",
                        tokens[index]));
            }
            else
            {
                throw new System.Exception("expected string literal, int literal, or variable");
            }
        }

        #region Utils

        private bool IsOperator(object token)
        {
            return token == Tokens.Add ||
                token == Tokens.Sub ||
                token == Tokens.Mul ||
                token == Tokens.Div;
        }

        private BinOp TokenToOp(object token)
        {
            if (token == Tokens.Add)
                return BinOp.Add;
            if (token == Tokens.Sub)
                return BinOp.Sub;
            if (token == Tokens.Mul)
                return BinOp.Mul;
            if (token == Tokens.Div)
                return BinOp.Div;

            else
                throw new System.Exception("invalid code operator");

        }

        private void MoveNext()
        {
            if (this.index < tokens.Count)
                this.index++;
            else
                throw new System.Exception("Token out of range.");
        }

        private bool Eat(Token token)
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