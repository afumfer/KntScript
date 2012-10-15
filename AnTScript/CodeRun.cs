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
    internal sealed class CodeRun
    {
        #region Fields
        
        Dictionary<string, object> symbolTable;
                
        bool flagBreak = false;
        
        IInOutDevice InOutDevice;
                
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

        internal CodeRun(Stmt stmt, IInOutDevice outputDevice)
        {
            this.InOutDevice = outputDevice;

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

            #region Sequence

            if (stmt is Sequence)
            {
                Sequence seq = (Sequence)stmt;
                RunStmt(seq.First);
                RunStmt(seq.Second);
            }

            #endregion

            #region DeclareVar

            else if (stmt is DeclareVar)
            {
                // declare
                DeclareVar declare = (DeclareVar)stmt;

                CodeDeclareSymbol(declare);
                
                // TODO: Sustituir lo anterior por esto cuando se
                //       arregle el código de asignación + declaración.
                //Assign assign = new Assign();
                //assign.Ident = declare.Ident;
                //assign.Expr = declare.Expr;
                //RunStmt(assign);
            }

            #endregion

            #region Assign

            else if (stmt is Assign)
            {
                Assign assign = (Assign)stmt;                
                CodeStoreSymbol(assign);
            }

            #endregion

            #region Print

            else if (stmt is Print)
            {
                Print print = (Print)stmt;
                CodeExecutePrint(print);
            }

            #endregion

            #region FunctionStmt

            else if (stmt is FunctionStmt)
            {
                FunctionStmt fun = (FunctionStmt)stmt;
                CodeExecuteFunction(fun.Function);
            }

            #endregion

            #region ReadVar
            
            else if (stmt is ReadVar)
            {
                ReadVar read = (ReadVar)stmt;
                ReadVarItem readVarItem;
                Assign assign;
                List<ReadVarItem> readVarItmes = new List<ReadVarItem>();
                                
                foreach (var pair in read.Vars)
                {
                    readVarItem = new ReadVarItem();
                    readVarItem.VarIdent = pair.Key.Ident;
                    readVarItem.VarValue = CodeReadSymbol(pair.Key);
                    readVarItem.Label = GenExpr(pair.Value);                        
                    readVarItmes.Add(readVarItem);
                }

                if ( CodeExecuteReadVars(readVarItmes) )
                {                 
                    foreach (ReadVarItem vi in readVarItmes)
                    {
                        assign = new Assign();
                        //assign.Ident = vi.Var.Ident;
                        assign.Ident = vi.VarIdent;
                        assign.Expr = ValueToExpr(vi.VarValue.GetType(), vi.VarNewValueText);
                        RunStmt(assign);                        
                    }
                }
            }

            #endregion

            #region BreakStmt

            else if (stmt is BreakStmt)
            {
                flagBreak = true;
                return;
            }

            #endregion

            #region  FoorLoop

            else if (stmt is ForLoop)
            {
                // example: 
                // for x = 0 to 100
                //   print "hello";
                // end for;

                ForLoop forLoop = (ForLoop)stmt;

                IntVal numFrom = new IntVal();
                IntVal numTo = new IntVal();

                Assign assignFrom = new Assign();                
                assignFrom.Ident = forLoop.Ident;

                assignFrom.Expr = forLoop.From;
                RunStmt(assignFrom);

                numFrom.Value = Convert.ToInt32(GenExpr(forLoop.From));
                numTo.Value = Convert.ToInt32(GenExpr(forLoop.To));

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

            #endregion

            #region FoorEachLoop

            // TODO: Provisional implementation of foreach loop
            else if (stmt is ForEachLoop)
            {
                // example: 
                // foreach x in myColec
                //   print "hello";
                //   print x.MyProp;
                // end foreach;

                ForEachLoop forEachLoop = (ForEachLoop)stmt;
                              
                object colec = GenExpr(forEachLoop.Colec);
                
                foreach (object o in (IEnumerable<object>)colec)
                {
                    if (flagBreak)
                        break;
                    
                    // TODO: Pendiente susutiruir CodeSpecialStoreObject por CodeStoreSymbol
                    //       En el futuro, CodeStoreSymbol debe almacenar la variable si 
                    //       previamente no hubiera sido declarada. 
                    CodeSpecialStoreObject(forEachLoop.Ident, o);
                    // CodeStoreSymbol(forEachLoop.Ident, o);

                    RunStmt(forEachLoop.Body);                                        
                }
                if (flagBreak)
                    flagBreak = false;
            }

            #endregion

            #region IfStmt

            else if (stmt is IfStmt)
            {
                // example: 
                // if a == 10 then 
                //   print "hello";
                // else
                //   print "bye";
                // end if;

                IfStmt ifStmt = (IfStmt)stmt;
                IntVal ifExp = new IntVal();

                ifExp.Value = Convert.ToInt32(GenExpr(ifStmt.TestExpr));

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

            #endregion

            #region WhileStmt

            else if (stmt is WhileStmt)
            {
                // example: 
                // while a <= 10
                //   print "hello";
                //   a = a + 1;
                // end while;

                WhileStmt whileStmt = (WhileStmt)stmt;
                IntVal whileExp = new IntVal();

                while (true)
                {
                    if (flagBreak)
                        break;
                    whileExp.Value = Convert.ToInt32(GenExpr(whileStmt.TestExpr));
                    if (whileExp.Value == 0)
                        break;
                    RunStmt(whileStmt.Body);
                }
                if (flagBreak)
                    flagBreak = false;
            }

            #endregion

            else
            {
                throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
            }

        } 

        private object GenExpr(Expr expr) 
        {
            // ...
            // TODO: pendiente de implmentar la comprobación de tipos.
            // Añadir el parámetro, ... "System.Type expectedType" como parámetro de entrada
            // de este método.
            // ...

            Type deliveredType;
            object res = null;

            if (expr is StringVal)                         
                res = ((StringVal)expr).Value;

            else if (expr is IntVal)
                res = ((IntVal)expr).Value;
        
            else if (expr is FloatVal)
                res = ((FloatVal)expr).Value;

            else if (expr is DoubleVal)
                res = ((DoubleVal)expr).Value;

            else if (expr is DecimalVal)
                res = ((DecimalVal)expr).Value;

            else if (expr is DateTimeVal)                        
                res = ((DateTimeVal)expr).Value;
            
            else if (expr is BoolVal)            
                res = ((BoolVal)expr).Value;
            
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
            IdentObject ident = new IdentObject(declare.Ident);

            if (!string.IsNullOrEmpty(ident.Member))
                throw new System.Exception(" variable declaration '" + declare.Ident + "' incorrect ");

            if (!this.symbolTable.ContainsKey(ident.Obj))
                symbolTable.Add(declare.Ident, GenExpr(declare.Expr));
            else
                throw new System.Exception(" variable '" + ident.Obj + "' already declared");
        }

        private void CodeStoreSymbol(Assign assign)
        {            
            CodeStoreSymbol(assign.Ident, GenExpr(assign.Expr));
        }

        private void CodeStoreSymbol(string identString, object varValue)
        {
            IdentObject ident = new IdentObject(identString);

            if (this.symbolTable.ContainsKey(ident.Obj))
                if (string.IsNullOrEmpty(ident.Member))
                    symbolTable[ident.Obj] = varValue;
                else
                {
                    try
                    {
                        SetValue(symbolTable[ident.Obj], ident, varValue);
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception(" error in assign code  '" + ident.Member + " :" + ex.Message);
                    }
                }
            else
                throw new System.Exception(" undeclared variable '" + ident.Obj);

        }

        private object CodeReadSymbol(Variable variable)
        {
            IdentObject ident = new IdentObject(variable.Ident);

            if (symbolTable.ContainsKey(ident.Obj))
                if (string.IsNullOrEmpty(ident.Member))
                    return symbolTable[ident.Obj];
                else
                {
                    try
                    {
                        object resGetValue;
                        GetValue(symbolTable[ident.Obj], ident, out resGetValue);
                        return resGetValue;
                    }
                    catch (Exception ex)
                    {
                        throw new System.Exception(" error in read code  '" + ident.Member + " :" + ex.Message);
                    }
                }
            else
                throw new System.Exception(" undeclared variable '" + ident.Obj);

        }

        // TODO: !!! ojo implementación provisional, sólo para usar en el bucle foreach        
        //           Se debe sustituir por CodeStoreSymbol
        private void CodeSpecialStoreObject(string ident, object value)
        {
            if (!this.symbolTable.ContainsKey(ident))
                symbolTable.Add(ident, value);
            else
                symbolTable[ident] = value;
        }

        private object CodeExecuteFunction(FunctionExpr function)
        {
            try
            {
                Type t;
                object obj;
                object objRoot;
                string funName;
                MethodInfo mi;

                IdentObject ident = new IdentObject(function.FunctionName);
                              
                if (string.IsNullOrEmpty(ident.Member))
                {
                    t = DefaultFunctionLibraryType;
                    obj = DefaultFunctionLibrary;
                    funName = ident.Obj;
                    mi = t.GetMethod(funName);
                }
                else
                {                    
                    objRoot = symbolTable[ident.Obj];                                        
                    GetObjectMethod(objRoot, ident, out obj, out mi);
                }
                                
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

            Type typeBinExpr = TypeOfBinaryExpr(left, right);
           
            if (typeBinExpr == typeof(int))
                res = CodeExecuteBinaryExpr<int>(left, right, be.Op);

            else if (typeBinExpr == typeof(float))
                res = CodeExecuteBinaryExpr<float>(left, right, be.Op);

            else if (typeBinExpr == typeof(double))
                res = CodeExecuteBinaryExpr<double>(left, right, be.Op);

            else if (typeBinExpr == typeof(decimal))
                res = CodeExecuteBinaryExpr<decimal>(left, right, be.Op);

            else if (typeBinExpr == typeof(string))
                res = CodeExecuteBinaryExpr<string>(left, right, be.Op);

            else if (typeBinExpr == typeof(DateTime))
                res = CodeExecuteBinaryExpr<DateTime>(left, right, be.Op);

            else
                res = CodeExecuteBinaryExpr<object>(left, right, be.Op);
            
            return res;
        }

        private object CodeExecuteUnaryExpr(UnaryExpr ue)
        {
            object res = null;
            object ex = GenExpr(ue.Expression);

            switch (ue.Op)
            {
                case BinOp.Add:
                    if (ex.GetType() == typeof(int))
                        res = Convert.ToInt32(ex);
                    else if (ex.GetType() == typeof(float))
                        res = Convert.ToSingle(ex);
                    else if (ex.GetType() == typeof(double))
                        res = Convert.ToDouble(ex);
                    else if (ex.GetType() == typeof(decimal))
                        res = Convert.ToDecimal(ex);

                    break;

                case BinOp.Sub:
                    if (ex.GetType() == typeof(int))
                        res = -1 * Convert.ToInt32(ex);
                    else if (ex.GetType() == typeof(float))
                        res = -1 * Convert.ToSingle(ex);
                    else if (ex.GetType() == typeof(double))
                        res = -1 * Convert.ToDouble(ex);
                    else if (ex.GetType() == typeof(decimal))
                        res = -1 * Convert.ToDecimal(ex);

                    break;

                case BinOp.Not:
                    if (ex.GetType() == typeof(bool))
                        res = (ex.Equals(false)) ? 1 : 0;
                    else
                        res = (ex.Equals(0)) ? 1 : 0;
                    break;

                default:
                    throw new ApplicationException(string.Format(
                        "The operator '{0}' is not supported in this expression ", ue.Op));
            }

            return res;                    
        }

        private object CodeExecuteBinaryExpr<T>(object left, object right, BinOp binExpOp)             
        {
            object res = null;
            bool boolLeft;
            bool boolRight;

            bool notSupport = false;

            if (left.Equals(1) || left.Equals(true))
                boolLeft = true;
            else
                boolLeft = false;

            if (right.Equals(1) || right.Equals(true))
                boolRight = true;
            else
                boolRight = false;

            switch (binExpOp)
            {
                case BinOp.Add:
                    if (typeof(T) == typeof(int))                        
                        res = Convert.ToInt32(left) + Convert.ToInt32(right);
                    else if (typeof(T) == typeof(float))
                        res = Convert.ToSingle(left) + Convert.ToSingle(right);
                    else if (typeof(T) == typeof(double))                        
                        res = Convert.ToDouble(left) + Convert.ToDouble(right);
                    else if (typeof(T) == typeof(decimal))                        
                        res = Convert.ToDecimal(left) + Convert.ToDecimal(right);
                    else if (typeof(T) == typeof(string))
                        res = left.ToString() + right.ToString();                        
                    break;

                case BinOp.Sub:
                    if (typeof(T) == typeof(int))
                        res = Convert.ToInt32(left) - Convert.ToInt32(right);
                    else if (typeof(T) == typeof(float))
                        res = Convert.ToSingle(left) - Convert.ToSingle(right);
                    else if (typeof(T) == typeof(double))
                        res = Convert.ToDouble(left) - Convert.ToDouble(right);
                    else if (typeof(T) == typeof(decimal))
                        res = Convert.ToDecimal(left) - Convert.ToDecimal(right);
                    break;

                case BinOp.Mul:
                    if (typeof(T) == typeof(int))
                        res = Convert.ToInt32(left) * Convert.ToInt32(right);
                    else if (typeof(T) == typeof(float))
                        res = Convert.ToSingle(left) * Convert.ToSingle(right);
                    else if (typeof(T) == typeof(double))
                        res = Convert.ToDouble(left) * Convert.ToDouble(right);
                    else if (typeof(T) == typeof(decimal))
                        res = Convert.ToDecimal(left) * Convert.ToDecimal(right);
                    break;

                case BinOp.Div:
                    if (typeof(T) == typeof(int))
                        res = Convert.ToInt32(left) / Convert.ToInt32(right);
                    else if (typeof(T) == typeof(float))
                        res = Convert.ToSingle(left) / Convert.ToSingle(right);
                    else if (typeof(T) == typeof(double))
                        res = Convert.ToDouble(left) / Convert.ToDouble(right);
                    else if (typeof(T) == typeof(decimal))
                        res = Convert.ToDecimal(left) / Convert.ToDecimal(right);
                    break;

                case BinOp.Or:
                    res = (boolLeft || boolRight) ? 1 : 0;
                    break;

                case BinOp.And:
                    res = (boolLeft && boolRight) ? 1 : 0;
                    break;

                case BinOp.Equal:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) == Convert.ToInt32(right)) ? 1 : 0;                    
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) == Convert.ToSingle(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) == Convert.ToDouble(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) == Convert.ToDecimal(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) == Convert.ToDateTime(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(string))
                        res = (left.ToString() == right.ToString()) ? 1 : 0;
                    else if (typeof(T) == typeof(object))
                        res = (left.GetHashCode() == right.GetHashCode()) ? 1 : 0;
                    break;

                case BinOp.NotEqual:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) == Convert.ToInt32(right)) ? 0 : 1;
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) == Convert.ToSingle(right)) ? 0 : 1;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) == Convert.ToDouble(right)) ? 0 : 1;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) == Convert.ToDecimal(right)) ? 0 : 1;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) == Convert.ToDateTime(right)) ? 0 : 1;
                    else if (typeof(T) == typeof(string))
                        res = (left.ToString() == right.ToString()) ? 0 : 1;
                    else if (typeof(T) == typeof(object))
                        res = (left.GetHashCode() == right.GetHashCode()) ? 0 : 1;
                    break;

                case BinOp.LessThan:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) < Convert.ToInt32(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) < Convert.ToSingle(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) < Convert.ToDouble(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) < Convert.ToDecimal(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) < Convert.ToDateTime(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(string))
                        res = (string.Compare(left.ToString(), right.ToString()) < 0) ? 1 : 0;

                    break;

                case BinOp.LessThanOrEqual:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) <= Convert.ToInt32(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) <= Convert.ToSingle(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) <= Convert.ToDouble(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) <= Convert.ToDecimal(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) <= Convert.ToDateTime(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(string))
                        res = (string.Compare(left.ToString(), right.ToString()) < 0 || left.ToString() == right.ToString()) ? 1 : 0;

                    break;

                case BinOp.GreaterThan:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) > Convert.ToInt32(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) > Convert.ToSingle(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) > Convert.ToDouble(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) > Convert.ToDecimal(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) > Convert.ToDateTime(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(string))
                        res = (string.Compare(left.ToString(), right.ToString()) > 0) ? 1 : 0;

                    break;

                case BinOp.GreaterThanOrEqual:
                    if (typeof(T) == typeof(int))
                        res = (Convert.ToInt32(left) >= Convert.ToInt32(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(float))
                        res = (Convert.ToSingle(left) >= Convert.ToSingle(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(double))
                        res = (Convert.ToDouble(left) >= Convert.ToDouble(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(decimal))
                        res = (Convert.ToDecimal(left) >= Convert.ToDecimal(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(DateTime))
                        res = (Convert.ToDateTime(left) >= Convert.ToDateTime(right)) ? 1 : 0;
                    else if (typeof(T) == typeof(string))
                        res = (string.Compare(left.ToString(), right.ToString()) > 0 || left.ToString() == right.ToString()) ? 1 : 0;

                    break;

                default:
                    notSupport = true;
                    break;
            }

            if (notSupport)
                throw new ApplicationException(string.Format(
                    "The operator '{0}' is not supported  in this expression ", binExpOp));
            else 
                return res;
        }

        private void CodeExecutePrint(Print print)
        {
            string s = GenExpr(print.Expr).ToString();
            InOutDevice.Print(s);
        }

        private bool CodeExecuteReadVars(List<ReadVarItem> readVarItmes)
        {
            return InOutDevice.ReadVars(readVarItmes);
        }

        #endregion

        #region Utils

        private Type TypeOfExpr(Expr expr)
        {
            if (expr is StringVal)
            {
                return typeof(string);
            }
            else if (expr is IntVal)
            {
                return typeof(int);
            }
            else if (expr is FloatVal)
            {
                return typeof(float);
            }
            else if (expr is DoubleVal)
            {
                return typeof(double);
            }
            else if (expr is DecimalVal)
            {
                return typeof(decimal);
            }
            else if (expr is DateTimeVal)
            {
                return typeof(DateTime);
            }
            else if (expr is BoolVal)
            {
                return typeof(bool);
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

        private Type TypeOfBinaryExpr(object left, object right)
        {
            // int
            if (left.GetType() == typeof(int) && right.GetType() == typeof(int))
                return typeof(int);

            // float
            else if (left.GetType() == typeof(float) && right.GetType() == typeof(float))
                return typeof(float);

            else if (  (left.GetType() == typeof(float) || right.GetType() == typeof(float)) &&
                (left.GetType() == typeof(int) || right.GetType() == typeof(int))  
                )
                return typeof(float);

            // double
            else if ( left.GetType() == typeof(double) && right.GetType() == typeof(double) )
                return typeof(double);

            else if ((left.GetType() == typeof(double) || right.GetType() == typeof(double)) &&
                (left.GetType() == typeof(int) || right.GetType() == typeof(int) || left.GetType() == typeof(float) || right.GetType() == typeof(float) )
                )
                return typeof(double);

            // decimal 
            else if (left.GetType() == typeof(decimal) && right.GetType() == typeof(decimal))
                return typeof(decimal);

            else if ((left.GetType() == typeof(decimal) || right.GetType() == typeof(decimal)) &&
                (left.GetType() == typeof(int) || right.GetType() == typeof(int) || left.GetType() == typeof(float) || right.GetType() == typeof(float) || left.GetType() == typeof(double) || right.GetType() == typeof(double) ) 
                )
                return typeof(decimal);

            // string  !!!   ||
            else if (left.GetType() == typeof(string) || right.GetType() == typeof(string))
                return typeof(string);

            // DateTime
            else if (left.GetType() == typeof(DateTime) && right.GetType() == typeof(DateTime))
                return typeof(DateTime);

            // object
            else
                return typeof(object);
            

        }

        private void SetValue(object varObj, IdentObject ident, object newValue, int i = 0)
        {
            Type t;
            PropertyInfo pi;
            string literalObjChild;
            object objChild;

            t = varObj.GetType();

            if (i < ident.ChainObjs.Count)
            {
                literalObjChild = ident.ChainObjs[i];
                pi = t.GetProperty(literalObjChild);
                objChild = pi.GetValue(varObj, null);
                i++;
                SetValue(objChild, ident, newValue, i);
            }
            else
            {
                pi = t.GetProperty(ident.Member);
                pi.SetValue(varObj, newValue, null);
            }

            return;
        }

        private void GetValue(object varObj, IdentObject ident, out object newValue, int i = 0)
        {
            Type t;
            PropertyInfo pi;
            string literalObjChild;
            object objChild;

            t = varObj.GetType();

            if (i < ident.ChainObjs.Count)
            {
                literalObjChild = ident.ChainObjs[i];
                pi = t.GetProperty(literalObjChild);
                objChild = pi.GetValue(varObj, null);
                i++;
                GetValue(objChild, ident, out newValue, i);
            }
            else
            {
                pi = t.GetProperty(ident.Member);                
                newValue = pi.GetValue(varObj, null);
            }

            return;
        }

        private void GetObjectMethod(object objRoot, IdentObject ident, out object objRet, out MethodInfo methodInfo, int i = 0)
        {
            Type t;
            PropertyInfo pi;
            string literalObjChild;
            object objChild;

            t = objRoot.GetType();

            if (i < ident.ChainObjs.Count)
            {
                literalObjChild = ident.ChainObjs[i];
                pi = t.GetProperty(literalObjChild);
                objChild = pi.GetValue(objRoot, null);
                i++;
                GetObjectMethod(objChild, ident, out objRet, out methodInfo, i);
            }
            else
            {                
                objRet = objRoot;
                methodInfo = t.GetMethod(ident.Member);
            }

            return;
        }


        internal Expr ValueToExpr(Type t, object newValue)
        {            
            if (t == typeof(int))
            {
                IntVal intVal = new IntVal();;                
                intVal.Value = Convert.ToInt32(newValue);
                return intVal;
            }
            else if (t == typeof(float))
            {
                FloatVal floatVal = new FloatVal();
                floatVal.Value =  Convert.ToSingle(newValue);
                return floatVal;
            }
            else if (t == typeof(double))
            {
                DoubleVal doubleVal = new DoubleVal();
                doubleVal.Value =  Convert.ToDouble(newValue);
                return doubleVal;
            }
            else if (t == typeof(decimal))
            {
                DecimalVal decimalVal = new DecimalVal();
                decimalVal.Value = Convert.ToDecimal(newValue);
                return decimalVal;
            }
            else if (t == typeof(string))
            {
                StringVal stringVal = new StringVal();
                stringVal.Value = Convert.ToString(newValue);
                return stringVal;
            }
            else if (t == typeof(DateTime))
            {
                DateTimeVal dateTimeVal = new DateTimeVal();
                dateTimeVal.Value = Convert.ToDateTime(newValue);
                return dateTimeVal;
            }
            else if (t == typeof(bool))
            {
                BoolVal boolVal = new BoolVal();
                boolVal.Value = Convert.ToBoolean(newValue);
                return boolVal;
            }
            else
            {
                StringVal stringVal = new StringVal();
                stringVal.Value = Convert.ToString(newValue);
                return stringVal;
            }
            
        }



        #endregion

    } // CodeRun class

} // namespace