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
        #region Fields

        Dictionary<string, object> symbolTable;
        
        bool flagBreak = false;
        
        // TODO: El tipo de objeto para la salida debería ser un objeto abstracto y
        //       la instancia concreta debería venir vía inyección de código.
        //       Ahora está así para salir del paso.
        //       (Arreglar esto en el futuro).
        TextBox textOut;
                
        #endregion

        #region Properties

        private object _defaultFunctionLibrary;
        public object DefaultFunctionLibrary
        {
            get 
            {
                if (_defaultFunctionLibraryType == null || _defaultFunctionLibrary == null)
                {
                    _defaultFunctionLibraryType = Type.GetType("AnTScript.Library", false, true);
                    _defaultFunctionLibrary = Activator.CreateInstance(_defaultFunctionLibraryType);
                }
                return _defaultFunctionLibrary;
            }
            set
            {
                _defaultFunctionLibrary = value;
                _defaultFunctionLibraryType = _defaultFunctionLibrary.GetType();
            }

        }

        private Type _defaultFunctionLibraryType;
        public Type DefaultFunctionLibraryType
        {
            get
            {
                if (_defaultFunctionLibraryType == null || _defaultFunctionLibrary == null)
                {
                    _defaultFunctionLibraryType = Type.GetType("AnTScript.Library", false, true);
                    _defaultFunctionLibrary = Activator.CreateInstance(_defaultFunctionLibraryType);
                }
                return _defaultFunctionLibraryType;
            }
        }

        #endregion

        #region Constructor

        public CodeRun(Stmt stmt, TextBox textOut)
        {
            this.textOut = textOut;
            symbolTable = new Dictionary<string, object>();

            // Go Run!
            RunStmt(stmt);
        }

        #endregion

        #region CodeRun main methods

        private void RunStmt(Stmt stmt)
        {
            if (flagBreak)
                return;

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

                CodeDeclareSymbol(declare);

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
                CodeStoreSymbol(assign);
            }

            else if (stmt is Print)
            {
                Print print = (Print)stmt;
                CodeExecutePrint(print);
            }

            else if (stmt is FunctionStmt)
            {
                FunctionStmt fun = (FunctionStmt)stmt;
                CodeExecuteFunction(fun.Function);
            }

            else if (stmt is ReadNum)
            {
                ReadNum read = (ReadNum)stmt;

                Assign assign = new Assign();
                //assign.Identifier = read.Identifier;
                assign.Ident = read.Ident;

                DecimalLiteral decLiteral = new DecimalLiteral();
                decLiteral.Value = CodeExecuteReadNum();

                assign.Expr = decLiteral;
                RunStmt(assign);
            }

            //flagBreak
            else if (stmt is BreakStmt)
            {
                flagBreak = true;
                return;
            }

            else if (stmt is ForLoop)
            {
                // example: 
                // for x = 0 to 100 do
                //   print "hello";
                // end for;

                ForLoop forLoop = (ForLoop)stmt;

                DecimalLiteral numFrom = new DecimalLiteral();
                DecimalLiteral numTo = new DecimalLiteral();

                Assign assignFrom = new Assign();                
                assignFrom.Ident = forLoop.Ident;

                assignFrom.Expr = forLoop.From;
                RunStmt(assignFrom);

                numFrom.Value = (float)GenExpr(forLoop.From);
                numTo.Value = (float)GenExpr(forLoop.To);

                while (numFrom.Value <= numTo.Value)
                {
                    if (flagBreak)
                        break;
                    RunStmt(forLoop.Body);
                    numFrom.Value++;
                    assignFrom.Expr = numFrom;
                    RunStmt(assignFrom);
                }
                if (flagBreak)
                    flagBreak = false;
            }

            else if (stmt is IfStmt)
            {
                // example: 
                // if a == 10 then 
                //   print "hello";
                // else
                //   print "bye";
                // end if;

                IfStmt ifStmt = (IfStmt)stmt;
                DecimalLiteral ifExp = new DecimalLiteral();

                ifExp.Value = (float)GenExpr(ifStmt.TestExpr);

                if (ifExp.Value != 0)
                {
                    RunStmt(ifStmt.BodyIf);
                }
                else
                {
                    if (ifStmt.DoElse)
                        RunStmt(ifStmt.BodyElse);
                }
            }

            else if (stmt is WhileStmt)
            {
                // example: 
                // while a <= 10
                //   print "hello";
                //   a = a + 1;
                // end while;

                WhileStmt whileStmt = (WhileStmt)stmt;
                DecimalLiteral whileExp = new DecimalLiteral();

                while (true)
                {
                    if (flagBreak)
                        break;
                    whileExp.Value = (float)GenExpr(whileStmt.TestExpr);
                    if (whileExp.Value == 0)
                        break;
                    RunStmt(whileStmt.Body);
                }
                if (flagBreak)
                    flagBreak = false;
            }

            else
            {
                throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
            }

        } 

        private object GenExpr(Expr expr) 
        {
            // ...
            // TODO: pendiente de implmentar la comprobación de tipos.
            // Añadir el parámetro, ... System.Type expectedType como parámetro de entrada
            // de este método.
            // ...

            Type deliveredType;
            object res = null;

            if (expr is StringLiteral)                         
                res = ((StringLiteral)expr).Value;
        
            else if (expr is DecimalLiteral)                            
                res = ((DecimalLiteral)expr).Value;

            else if (expr is DateTimeLiteral)                        
                res = ((DateTimeLiteral)expr).Value;

            else if (expr is Variable)                                        
                res = CodeReadSymbol((Variable)expr);

            else if (expr is FunctionExpr)                        
                res = CodeExecuteFunction((FunctionExpr)expr);

            else if (expr is NewObjectExpr)                       
                res = CodeExecuteNewObject((NewObjectExpr)expr);

            else if (expr is BinaryExpr)                        
                res = CodeExecuteBinaryExpr((BinaryExpr)expr);

            else if (expr is UnaryExpr)                            
                res = CodeExecuteUnaryExpr((UnaryExpr)expr);

                        
            // TODO: Pendiente, para resolver conversión de tipos automárica en versiones futuras 
            deliveredType = res.GetType();
            //if (deliveredType != expectedType)
            //{
            //    if (deliveredType == typeof(int) &&
            //        expectedType == typeof(string))
            //    {
            //        // Aquí hacer el casting
            //        // ....
            //    }
            //    else
            //    {
            //        throw new System.Exception("can't coerce a " + deliveredType.Name + " to a " + expectedType.Name);
            //    }
            //} 

            return res;            

        } 

        #endregion

        #region Code execute region

        private void CodeDeclareSymbol(DeclareVar declare)
        {
            IdentObject id = new IdentObject(declare.Ident);

            if (!string.IsNullOrEmpty(id.Prop))
                throw new System.Exception(" variable declaration '" + declare.Ident + "' incorrect ");

            if (!this.symbolTable.ContainsKey(id.Obj))
                symbolTable.Add(declare.Ident, GenExpr(declare.Expr));
            else
                throw new System.Exception(" variable '" + id.Obj + "' already declared");
        }

        private void CodeStoreSymbol(Assign assign)
        {
            IdentObject id = new IdentObject(assign.Ident);

            if (this.symbolTable.ContainsKey(id.Obj))
                if (string.IsNullOrEmpty(id.Prop))
                    symbolTable[id.Obj] = GenExpr(assign.Expr);
                else
                {
                    try
                    {
                        // TODO: estas variables podría ser un campos dentro de esta clase ???
                        object obj;                        
                        Type t = symbolTable[id.Obj].GetType();
                        PropertyInfo pi = t.GetProperty(id.Prop);

                        obj = GenExpr(assign.Expr);

                        // TODO: Provisional, para el casting a enteros o a decimal hasta que se soporte
                        //       los tipos int, decimal, ... (ahora las expresiones sólo devuelven tipos float cuando 
                        //       el resultado es numérico
                        if (pi.PropertyType == typeof(int) && obj.GetType() == typeof(float))                            
                            obj = (int)Math.Ceiling((float)obj);
                        else if (pi.PropertyType == typeof(decimal) && obj.GetType() == typeof(float))
                            obj = (decimal)(float)obj;                         

                        pi.SetValue(symbolTable[id.Obj], obj, null);                        
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception(" error in assign code  '" + id.Prop + " :" + ex.Message);
                    }
                }
            else
                throw new System.Exception(" undeclared variable '" + id.Obj);

        }

        private object CodeReadSymbol(Variable variable)
        {
            IdentObject id = new IdentObject(variable.Ident);

            if (this.symbolTable.ContainsKey(id.Obj))
                if (string.IsNullOrEmpty(id.Prop))
                    return symbolTable[id.Obj];
                else
                {
                    try
                    {
                        // TODO: estas variables podría ser un campos dentro de esta clase ???
                        object obj;
                        Type t = symbolTable[id.Obj].GetType();
                        PropertyInfo pi = t.GetProperty(id.Prop);                        
                        obj = pi.GetValue(symbolTable[id.Obj], null);

                        // TODO: Provisional, para el casting a enteros o a decimal hasta que se soporte
                        //       los tipos int, decimal, ... (ahora las expresiones sólo devuelven tipos float cuando 
                        //       el resultado es numérico
                        if (obj.GetType() == typeof(int))
                            obj = (float)obj;
                        else if (obj.GetType() == typeof(decimal))                                                
                            obj = (float)Math.Ceiling((decimal)obj);
                        else if (obj.GetType() == typeof(double))
                            obj = (float)Math.Ceiling((double)obj);
                        
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception(" error in read code  '" + id.Prop + " :" + ex.Message);
                    }
                }
            else
                throw new System.Exception(" undeclared variable '" + id.Obj);

        }

        private object CodeExecuteFunction(FunctionExpr function)
        {
            try
            {
                Type t;
                object obj;
                string funName;

                // TODO: arreglar esto (Ident Object tiene que servier para capturar métodos también)
                IdentObject id = new IdentObject(function.FunctionName);
               
                // TODO: Mejorar esto !!! (lo que viene es sólo el nombre del método)
                if (string.IsNullOrEmpty(id.Prop))
                {
                    t = DefaultFunctionLibraryType;
                    obj = DefaultFunctionLibrary;                    
                    funName = id.Obj;
                }
                else
                {
                    t = symbolTable[id.Obj].GetType();
                    obj = symbolTable[id.Obj];
                    funName = id.Prop;
                }

                MethodInfo mi = t.GetMethod(funName);
                
                // Params
                object[] param;
                if (function.Args.Count > 0)
                {
                    param = new object[function.Args.Count];
                    for (int i = 0; i < function.Args.Count; i++)
                    {
                        param[i] = GenExpr(function.Args[i]);
                    }
                }
                else
                    param = null;
                                
                object ret;
                ret = mi.Invoke(obj, param);
                if (ret != null)
                    return ret;
                else
                    return 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private object CodeExecuteNewObject(NewObjectExpr newObject)
        {
            try
            {
                Type t;
                                                
                t = Type.GetType(newObject.ClassName, false, true);
            
                // Params
                object[] param;
                if (newObject.Args.Count > 0)
                {
                    param = new object[newObject.Args.Count];
                    for (int i = 0; i < newObject.Args.Count; i++)
                    {
                        param[i] = GenExpr(newObject.Args[i]);
                    }
                }
                else
                    param = null;

                return Activator.CreateInstance(t, param);   

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private object CodeExecuteBinaryExpr(BinaryExpr be)
        {
            object res = null;
            object left = GenExpr(be.Left);
            object right = GenExpr(be.Right);
            
            if (left.GetType() == typeof(float) && right.GetType() == typeof(float))            
                res = CodeExecuteBinaryExprFloat(left, right, be.Op);            

            else if (left.GetType() == typeof(DateTime) && right.GetType() == typeof(DateTime))            
                res = CodeExecuteBinaryExprDateTime(left, right, be.Op);          

            else            
                res = CodeExecuteBinaryExprGeneric(left, right, be.Op);

            // TODO: Implementar más operadores aquí con tipos de datos distintos
            //       a float (string)
            
            return res;
        }

        private object CodeExecuteUnaryExpr(UnaryExpr ue)
        {
            object res = null;
            object ex = GenExpr(ue.Expression);

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

                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported in this expression ", ue.Op));
            }

            return res;                    
        }

        private object CodeExecuteBinaryExprFloat(object left, object right, BinOp binExpOp)
        {
            object res = null;

            bool boolLeft;
            bool boolRight;

            if ((float)left != 0)
                boolLeft = true;
            else
                boolLeft = false;

            if ((float)right != 0)
                boolRight = true;
            else
                boolRight = false;

            switch (binExpOp)
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
                case BinOp.Equal:
                    if ((float)left == (float)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.NotEqual:
                    if ((float)left == (float)right)
                        res = 0.0f;
                    else
                        res = 1.0f;
                    break;
                case BinOp.LessThan:
                    if ((float)left < (float)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.LessThanOrEqual:
                    if ((float)left <= (float)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThan:
                    if ((float)left > (float)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThanOrEqual:
                    if ((float)left >= (float)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported  in this expression ", binExpOp));
            }

            return res;
        }

        private object CodeExecuteBinaryExprDateTime(object left, object right, BinOp binExpOp)
        {
            object res = null;

            switch (binExpOp)
            {
                case BinOp.Equal:
                    if ((DateTime)left == (DateTime)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.NotEqual:
                    if ((DateTime)left == (DateTime)right)
                        res = 0.0f;
                    else
                        res = 1.0f;
                    break;
                case BinOp.LessThan:
                    if ((DateTime)left < (DateTime)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.LessThanOrEqual:
                    if ((DateTime)left <= (DateTime)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThan:
                    if ((DateTime)left > (DateTime)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThanOrEqual:
                    if ((DateTime)left >= (DateTime)right)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;

                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported in this expression ", binExpOp));
            }

            return res;
        }

        private object CodeExecuteBinaryExprGeneric(object left, object right, BinOp binExpOp)
        {
            object res = null;

            switch (binExpOp)
            {
                case BinOp.Add:
                    res = left.ToString() + right.ToString();
                    break;
                case BinOp.Equal:
                    if (left.ToString() == right.ToString())
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.NotEqual:
                    if (left.ToString() == right.ToString())
                        res = 0.0f;
                    else
                        res = 1.0f;
                    break;
                case BinOp.LessThan:
                    if (string.Compare(left.ToString(), right.ToString()) < 0)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.LessThanOrEqual:
                    if (string.Compare(left.ToString(), right.ToString()) < 0 || left.ToString() == right.ToString())
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThan:
                    if (string.Compare(left.ToString(), right.ToString()) > 0)
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                case BinOp.GreaterThanOrEqual:
                    if (string.Compare(left.ToString(), right.ToString()) > 0 || left.ToString() == right.ToString())
                        res = 1.0f;
                    else
                        res = 0.0f;
                    break;
                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported in this expression ", binExpOp));
            }

            return res;
        }

        private void CodeExecutePrint(Print print)
        {
            string s = GenExpr(print.Expr).ToString();
            if (s == @"\")
                textOut.AppendText("\r\n");
            else
                textOut.AppendText(@s);            
        }

        private float CodeExecuteReadNum()
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

        #endregion

        #region Utils

        private Type TypeOfExpr(Expr expr)
        {
            if (expr is StringLiteral)
            {
                return typeof(string);
            }
            else if (expr is DecimalLiteral)
            {
                return typeof(float);
            }
            else if (expr is DateTimeLiteral)
            {
                return typeof(DateTime);
            }
            else if (expr is Variable)
            {
                Variable var = (Variable)expr;
                IdentObject io = new IdentObject(var.Ident);
                if (this.symbolTable.ContainsKey(io.Obj))
                {
                    return symbolTable[io.Obj].GetType();
                }
                else
                {
                    throw new System.Exception("undeclared variable '" + var.Ident + "'");
                }
            }
            else if (expr is FunctionExpr)
            {
                // TODO: averiguar el valor de retorno de la 
                //       función que viene en expr y devolver ese tipo                
                return typeof(object);
            }
            else if (expr is NewObjectExpr)
            {
                // TODO: averiguar el valor de retorno de la 
                //       objeto que viene en expr y devolver ese tipo                
                return typeof(object);
            }
            else if (expr is BinaryExpr)
            {
                return typeof(BinaryExpr);
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

    #region auxiliary types

    // TODO: Provisional, esto debe ir en en el parser o en el scanner.
    class IdentObject
    { 
        public string Obj {get; set;}
        public string Prop {get; set;}

        public IdentObject (string ident)
        {       
            string[] tmp;
            if (ident != string.Empty)
            {
                tmp = ident.Split('.');
                Obj = tmp[0];
                if (tmp.Length > 1)
                    Prop = tmp[1];
            }
        }        
    }

    #endregion

} // namespace