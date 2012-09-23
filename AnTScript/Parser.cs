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
                    throw new System.Exception("expected variable name after 'read_num'");
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

                // TODO: Eliminar el inicio de bloque de la gramática del for
                //if (index == tokens.Count ||
                //    tokens[index] != Tokens.BeginBlock)
                //{
                //    throw new System.Exception("expected 'do' after from expression in for loop");
                //}
                //MoveNext();

                forLoop.Body = ParseStmt();
                result = forLoop;

                if (index == tokens.Count ||
                    tokens[index] != Tokens.EndSequence)
                {
                    throw new System.Exception("unterminated 'end for' loop body");
                }
                MoveNext();
                if (!MaybeEat(Tokens.For))
                    throw new System.Exception("unterminated 'for' body");

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
                    tokens[index] != Tokens.EndSequence)
                {
                    throw new System.Exception("unterminated 'end if' body");
                }
                MoveNext();
                if(!MaybeEat(Tokens.If))
                    throw new System.Exception("unterminated 'if' body");
            }

            // while
            else if (tokens[index] == Tokens.While)
            {
                WhileStmt whileStmt = new WhileStmt();
                MoveNext();

                whileStmt.TestExpr = ParseExpr();
                whileStmt.Body = ParseStmt();
                
                result = whileStmt;

                if (index == tokens.Count ||
                    tokens[index] != Tokens.EndSequence)
                {
                    throw new System.Exception("unterminated 'end while' body");
                }
                MoveNext();
                if (!MaybeEat(Tokens.While))
                    throw new System.Exception("unterminated 'while' body");
            }

            // break
            else if (tokens[index] == Tokens.Break)
            {
                BreakStmt breakStmt = new BreakStmt();
                MoveNext();

                breakStmt.Tag = "_Break_";  // for debug

                result = breakStmt;
            }

            // TODO: Limpiar versión antigua, aquí ahora se debe procesar
            //       la asignación o la llamada a función;
            //// assignment 
            //else if (tokens[index] is IdentifierToken)
            //{
            //    Assign assign = new Assign();
            //    assign.Ident = ((IdentifierToken)tokens[index]).Name;

            //    MoveNext();

            //    if (index == tokens.Count ||
            //        tokens[index] != Tokens.Assignment)
            //    {
            //        throw new System.Exception("expected '='");
            //    }

            //    MoveNext();

            //    assign.Expr = ParseExpr();
            //    result = assign;
            //}

            // assignment or funcionStmt
            else if (tokens[index] is IdentifierToken)
            {
                string ident = ((IdentifierToken)tokens[index]).Name;

                MoveNext();

                if(MaybeEat(Tokens.Assignment))
                {
                    Assign assign = new Assign();
                    assign.Ident = ident;
                    assign.Expr = ParseExpr();
                    result = assign;
                }
                else if(MaybeEat(Tokens.LeftBracket))
                {
                    FunctionStmt functionStmt = new FunctionStmt();                    
                    functionStmt.Function = ParseFunction(ident);
                    result = functionStmt;
                }
                else
                    throw new System.Exception("expected assing or function call");

            }

            else
            {
                throw new System.Exception("parse error at token " + index + ": " + tokens[index].Name);
            }

            if ((index < tokens.Count && tokens[index] == Tokens.Semi))
            {
                MoveNext();

                if ((index < tokens.Count && tokens[index] != Tokens.EndSequence)
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
                DecimalLiteral decLiteral = new DecimalLiteral();
                decLiteral.Value = ((NumberToken)this.tokens[this.index]).Value;
                MoveNext();
                return decLiteral;
            }
            else if (this.tokens[this.index] is DateTimeToken)
            {
                DateTimeLiteral datLiteral = new DateTimeLiteral();
                datLiteral.Value = ((DateTimeToken)this.tokens[this.index]).Value;
                MoveNext();
                return datLiteral;
            }
            // TODO: Basura ahora los objetos se crear con new xxxx
            //else if (this.tokens[this.index] is ObjectToken)
            //{
            //    ObjectLiteral objLiteral = new ObjectLiteral();
            //    objLiteral.Value = ((ObjectToken)this.tokens[this.index]).Value;
            //    objLiteral.ClassName = ((ObjectToken)this.tokens[this.index]).Name;
            //    MoveNext();
            //    return objLiteral;
            //}
            else if (this.tokens[this.index] is IdentifierToken)
            {            
                // TODO: versión antigua, cuando los identificadores eran variables,
                //       ahora pueden ser también llamadas a funciones.
                //Variable var = new Variable();
                //var.Ident = ((IdentifierToken)tokens[index]).Name;
                //MoveNext();
                //return var;

                string ident = ((IdentifierToken)tokens[index]).Name;
                
                MoveNext();

                // function expr
                if (MaybeEat(Tokens.LeftBracket))
                {                                                          
                    FunctionExpr fun = new FunctionExpr();
                    fun = ParseFunction(ident);
                    return fun;
                }
                // variable
                else
                {
                    Variable var = new Variable();
                    var.Ident = ident;                    
                    return var;                
                }
            }
            else if (this.tokens[this.index] == Tokens.New)
            {                
                MoveNext(); // eat new                                             
                return ParseNewObject();
            }
            else if (this.tokens[this.index] == Tokens.LeftBracket)
            {
                // Eat LeftParenthesis             
                MoveNext();
                Expr result = ParseExpr();
                Eat(Tokens.RightBracket);
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

        private FunctionExpr ParseFunction(string name) 
        {             
            FunctionExpr func = new FunctionExpr(); 

            func.FunctionName = name; 

            while ((tokens[index] != Tokens.RightBracket) 
                && (index < tokens.Count)) 
            { 
                func.Args.Add(ParseExpr());
                if (tokens[index] == Tokens.Comma) 
                     MoveNext(); // Skip comma 
                else if (tokens[index] == Tokens.RightBracket) 
                    break; 
                else 
                    throw new System.Exception("unexpected character in arg list"); 
            } 

            if (index == tokens.Count ||
                tokens[index] != Tokens.RightBracket) 
            { 
                throw new System.Exception("expect close bracket after open bracket/args"); 
            } 
            
            MoveNext();   // Skip RightBracket 

            return func; 
        }

        private NewObjectExpr ParseNewObject()
        {
            NewObjectExpr newObj = new NewObjectExpr();

            newObj.ClassName = ((IdentifierToken)tokens[index]).Name;
            
            MoveNext();
            Eat(Tokens.LeftBracket);

            while ((tokens[index] != Tokens.RightBracket)
                && (index < tokens.Count))
            {
                newObj.Args.Add(ParseExpr());
                if (tokens[index] == Tokens.Comma)
                    MoveNext(); // Skip comma 
                else if (tokens[index] == Tokens.RightBracket)
                    break;
                else
                    throw new System.Exception("unexpected character in arg list");
            }

            if (index == tokens.Count ||
                tokens[index] != Tokens.RightBracket)
            {
                throw new System.Exception("expect close bracket after open bracket/args");
            }

            MoveNext();   // Skip RightBracket 

            return newObj;
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