using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWinBackGrnd.IO.GraphicFile.Exceptions;
using TestWinBackGrnd.IO.GraphicFile.Models;
using TestWinBackGrnd.IO.GraphicFile.Symbols;
using TestWinBackGrnd.IO.GraphicFile.Symbols.Scopes;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees;
using TestWinBackGrnd.IO.GraphicFile.SyntaxTrees.PrimNodes;
using TestWinBackGrnd.IO.GraphicFile.Visitors;

namespace TestWinBackGrnd.IO.GraphicFile.Interpreters
{
    public class Interpreter : IZULVisitor
    {

        //Reference symbol table of defined symbols
        public MonolithicSymbolTable SymbolTable;
        //Stores values of symbols in symbol table
        public Dictionary<string, Object> values;

        private double version_supported = 0.1;

        public const string UnsupportedVersionMessage = "VER field specifies a version unsupported by this interpreter. Convert the script to a supported version or change the interpreter to a matching supported version.";

        public double VERSION_SUPPORTED { get => version_supported; protected set => version_supported = value; }

        public Interpreter(SymbolTable symbolTable)
        {
            SymbolTable = (MonolithicSymbolTable) symbolTable;
            values = new Dictionary<string, object>();
        }

        public void Visit(OldArrayNode node)
        {
            VisitChildren(node);
        }

        public void Visit(ArrNode node)
        {
            //Stores the final elements of the array
            List<object> elements = new List<object>(node.ChildCount());

            //Get all children from the node and iterate over them
            List<AST> elmNodes = node.ChildrenToList();
            foreach(AST element in elmNodes)
            {
                //Parse each element in the list of children and add it to elements
                elements.Add(ParseValue((ExprNode) element));
            }

            //Create a new array symbol and store it in the values table
            string name = node.GetTokenType() == TokenType.NAME ? node.GetTokenName() : "array";
            ArrayType array = new ArrayType(name, SymbolTable.ResolveType("arr"), SymbolTable.ResolveType("point"), elements);

            AssignArray(array);
        }

        public void Visit(ArrRetNode node)
        {
            VisitChildren(node);
        }

        public void Visit(AssignNode node)
        {
            Assign(node);
            VisitChildren(node);
        }

        public void Visit(DeclNode node)
        {
            VisitChildren(node);
        }

        public void Visit(FuncNode node)
        {
            ExecFunction(node);
        }

        public void Visit(NameNode node)
        {
            return;
        }

        public void Visit(NumNode node)
        {
            return;
        }

        public void Visit(RootNode node)
        {
            VisitChildren(node);
        }

        /// <summary>
        /// Load a value from the values table using a provided name.
        /// </summary>
        /// <param name="name">The key of the value to lookup. Typically it's a name to a symbol stored in the symbol table.</param>
        /// <returns>The value associated to the provided name or null if no value was found.</returns>
        public Object LoadValue(string name)
        {
            //Attempts to retrieve a value from the table without throwing an exception.
            values.TryGetValue(name, out object val);
            if(val == null)
            {
                //If no value was found, attempt to find it as an enum in the symbol table.
                val = (ConstantSymbol) SymbolTable.ResolveEnum(name);
                if (val != null) val = ((ConstantSymbol)val).value;
            }
            return val;
        }

        /// <summary>
        /// Load and return the value of an element in an array.
        /// </summary>
        /// <param name="node">The root of the syntax sub-tree describing the array access statement.</param>
        /// <returns>The element located in the array at the specified index.</returns>
        public object ParseArrayElement(ArrRetNode node)
        {
            //Acquire the name of the array and the index of the desired element
            NameNode nameNode = (NameNode)node.Child(ArrRetNode.ARR_NAME);
            int intIndex = (int)ParseValue(node.Child(ArrRetNode.INDEX));

            //Resolve the array and load it into a generic object
            object array = LoadValue(nameNode.GetName());

            object element = null;

            //Check if the array is not null to proceed with accessing the elements
            if (array != null)
            {
                try
                {
                    //Try to cast the array as a list of objects and use the ElementAt function to safely return the element.
                    element = ((List<object>)array).ElementAt(intIndex);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }

            return element;
        }

        /// <summary>
        /// Writes/updates a value in the value table using the root node of an assignment subtree.
        /// </summary>
        /// <param name="node">The root node in the assignment subtree. Must contain a NameNode and a not-null ExprNode as the new value.</param>
        public void Assign(AssignNode node)
        {
            ExprNode subject = (ExprNode) node[AssignNode.SUBJECT];

            if(subject is NameNode)
            {
                //Get the name of the value to be assigned
                string name = ((NameNode)subject).GetName();

                //Try to parse the new value of the object
                object value = ParseValue((ExprNode)node[AssignNode.NEW_VALUE]);

                CheckForSystemVar(name, value);

                values[name] = value;
            } else if(subject is ArrRetNode)
            {
                //Get the name of the array to be updated
                string name = ((NameNode)subject).GetName();
                //Try get the index to be used in updating the array
                NumNode indexNode = ((NumNode)subject);
                int.TryParse(indexNode.getValue(), out int index);

                //Try to parse the new value of the array element
                List<object> array = (List<object>) ParseValue((ExprNode)node[AssignNode.NEW_VALUE]);

                //Update the array element
                array[index] = ParseValue((ExprNode) node[AssignNode.NEW_VALUE]);

            }
        }

        /// <summary>
        /// Writes/updates an array to the value table using an ArrayType symbol
        /// </summary>
        /// <param name="array">The array symbol with populated Elements list.</param>
        public void AssignArray(ArrayType array)
        {
            values[array.Name] = array.Elements;
        }

        /// <summary>
        /// Attempts to parse a value from the provided syntax tree node based on its type.
        /// </summary>
        /// <param name="node">The node to be parsed.</param>
        /// <returns>A generic object corresponding to the expected return type of the node.</returns>
        private object ParseValue(ExprNode node)
        {
            switch (node.EvalType)
            {
                case NodeType.ARRRET:
                    //Treat the node as an array access statement and return a value as such
                    return ParseArrayElement((ArrRetNode) node);
                case NodeType.DECIMAL:
                    //Converts the node to a decimal value
                    return Convert.ToDouble(((NumNode)node).getValue());
                case NodeType.FLOAT:
                    //Get the string value of the float node
                    string sFloat = ((NumNode)node).getValue();
                    //Assume an 'f' suffix is present. Remove it from the string
                    sFloat = sFloat.Remove(sFloat.Length - 1);
                    return float.Parse(sFloat);
                case NodeType.INTEGER:
                    //Convert the node to a 32-bit integer
                    return Convert.ToInt32(((NumNode)node).getValue());
                case NodeType.RUPE:
                    //Get the string value of the rupe node
                    string sRupe = ((NumNode)node).getValue();
                    //Assume an 'rp' suffix is present. Remove it from the string
                    sRupe = sRupe.Remove(sRupe.Length - 2);
                    //Create a new Rupe object by parsing in the string as a float
                    Rupe rupe = new Rupe(float.Parse(sRupe));
                    return rupe;
                case NodeType.NAME:
                    //Get the name from the node as a string
                    string name = ((NameNode)node).GetName();
                    //Attempt to load the value of the string by name
                    Object val = LoadValue(name);
                    //If null was returned, return the contents of the nodes token instead
                    if (val == null)
                        return node.GetTokenName();
                    return val;
                case NodeType.FUNC:
                    //Use the node to execute a function and return the results
                    return ExecFunction((FuncNode) node);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Parses and executes a function syntax tree, returning the outcome of the executed function.
        /// </summary>
        /// <param name="node">The root node of the function subtree, containing a NameNode and argument nodes if needed.</param>
        /// <returns>An object of varying type according to the executed function.</returns>
        private object ExecFunction(FuncNode node)
        {

            //Get the name of the function to be executed
            string fName = ((NameNode)node[FuncNode.FUNC_NAME]).GetName();
            
            List<ExprNode> args = node.ArgsToList();

            //Build a syntax tree node to resolve the function from the symbol table
            ZULAst funcSymbol = new ZULAst(SymbolTable.Resolve(fName), SymbolTable);
            FunctionSymbol func = SymbolTable.Call(funcSymbol, args);

            //Process each argument node from the tree by parsing it and adding it to the list args
            foreach(ExprNode arg in args)
            {
                func.PushArg(ParseValue(arg));
            }
            
            //Call the virtual function ProcessFunction to obtain the 
            object result =  ProcessFunction(func);

            //To resolve persisting arguments due to passing symbols by reference, the args in the function symbol are cleared.
            func.ClearArgs();

            return result;
        }

        /// <summary>
        /// Processes a variable to identify if it is a system variable or not, and executes instructions related to the variable.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="val">The value of the variable.</param>
        /// <returns>Returns whether the variable is a system variable or not.</returns>
        public bool CheckForSystemVar(string name, object val)
        {
            switch(name)
            {
                //Version-checking subsection.
                case "VER":
                    double verId;
                    if(val is double)
                    {
                        verId = (double) val;
                        //Check if the version number equals this interpreters version number.
                        if (verId != VERSION_SUPPORTED)
                        {
                            //If it doesn't, throw an exception
                            throw new UnsupportedVersionException(UnsupportedVersionMessage, VERSION_SUPPORTED.ToString(), verId.ToString());
                        }
                    }
                    break;

                default:
                    //Call a dispatch method to check if the variable is defined in an inheriting child interpreter.
                    ProcessSpecificSystemVar(name, val);
                    break;
            }

            return false;
        }

        /// <summary>
        /// Dispatch method for inheriting interpreter classes to override.
        /// Used to check for system-specific predeclared fields which may be important to initialising the interpreter environmtent.
        /// </summary>
        /// <param name="name">The name of the field to be parsed.</param>
        /// <param name="val">The value of the field to be parsed.</param>
        /// <returns></returns>
        public virtual bool ProcessSpecificSystemVar(string name, object val)
        {
            return false;
        }

        /// <summary>
        /// Virtual function to be used by inheriting subclasses to process language-specific methods.
        /// </summary>
        /// <param name="function">The function symbol to be processed,
        ///  containing the name and arguments if any of the function to be executed.</param>
        /// <returns></returns>
        public virtual object ProcessFunction(FunctionSymbol function)
        {
            return null;
        }

        /// <summary>
        /// Recursively visit children nodes in the provided node.
        /// </summary>
        /// <param name="node">The root node to begin visiting.</param>
        private void VisitChildren(ExprNode node)
        {
            if (node.IsChildrenNull()) return;
            int childrenCount = node.ChildCount();
            for (int i = 0; i < childrenCount; i++)
            {
                node.Child(i).Visit(this);
            }
        }

        /// <summary>
        /// Helper function for validating argument signatures.
        /// This test is explicitely for one method signature and must be called multiple times for multiple accepted signatures.
        /// </summary>
        /// <param name="symbol">The function symbol containing the arguments to be validated.</param>
        /// <param name="types">An ordered array of Types corresponding to the desired order of argument types and amount of arguments.</param>
        /// <returns></returns>
        public bool ValidateFuncArgs(FunctionSymbol symbol, Type[] types)
        {
            bool isValid = true;
            //Check if the number of expected arguments matches the number of provided arguments
            if(symbol.ArgCount == types.Count())
            {
                for (int i = 0; i < symbol.ArgCount; i++)
                {
                    //Determine if the argument is a generic Type, used in identifying collections later on
                    bool argGeneric = symbol.ArgAt(i).GetType().IsGenericType;

                    //If the symbol does not match the expected type and is not a generic type, the signature is invalid.
                    //This check is performed since List/array-type objects are considered generic types and must be compared by genericTypeDefinition
                    //but non-generic objects/primitives do not have this field, resulting in an exception.
                    if (symbol.ArgAt(i).GetType() != types[i] && !argGeneric)
                    {
                        return false;
                    } else if (argGeneric)
                    {
                        //Check if the arguments generic type defenition is equal to the expected generic type.
                        //If not, the signature is invalid.
                        if(symbol.ArgAt(i).GetType().GetGenericTypeDefinition() != types[i])
                        {
                            return false;
                        }
                    }
                }
            } else
            {
                return false;
            }

            //If this path is reached, the arguments match the expected signature; the arguments are valid.
            return isValid;
        }

    }
}
