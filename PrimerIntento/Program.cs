using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;

using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace PrimerIntento
{
    class MainClass
    {
        public static string code = @"
            class SampleClass
            {
                public static void SayHello ()
                {
                    Console.WriteLine (""Hello World!"");
                }
            }";

        /// <summary>
        /// Function to compile .Net C#/VB source codes at runtime
        /// </summary>
        /// <param name="_CodeProvider">Base class for compiler provider</param>
        /// <param name="_SourceCode">C# or VB source code as a string</param>
        /// <param name="_SourceFile">External file containing C# or VB source code</param>
        /// <param name="_ExeFile">File path to create external executable file</param>
        /// <param name="_AssemblyName">File path to create external assembly file</param>
        /// <param name="_ResourceFiles">Required resource files to compile the code</param>
        /// <param name="_Errors">String variable to store any errors occurred during the process</param>
        /// <returns>Return TRUE if successfully compiled the code, else return FALSE</returns>
        private bool CompileCode(System.CodeDom.Compiler.CodeDomProvider _CodeProvider, string _SourceCode, string _SourceFile, string _ExeFile, string _AssemblyName, string[] _ResourceFiles, ref string _Errors)
        {
            // set interface for compilation
            System.CodeDom.Compiler.ICodeCompiler _CodeCompiler = _CodeProvider.CreateCompiler();

            // Define parameters to invoke a compiler
            System.CodeDom.Compiler.CompilerParameters _CompilerParameters =
                new System.CodeDom.Compiler.CompilerParameters();

            if (_ExeFile != null)
            {
                // Set the assembly file name to generate.
                _CompilerParameters.OutputAssembly = _ExeFile;

                // Generate an executable instead of a class library.
                _CompilerParameters.GenerateExecutable = true;
                _CompilerParameters.GenerateInMemory = false; // Esto será lo que haya que cambiar para que se cargue en memoria
            }
            else if (_AssemblyName != null)
            {
                // Set the assembly file name to generate.
                _CompilerParameters.OutputAssembly = _AssemblyName;

                // Generate an executable instead of a class library.
                _CompilerParameters.GenerateExecutable = false;
                _CompilerParameters.GenerateInMemory = false;
            }
            else
            {
                // Generate an executable instead of a class library.
                _CompilerParameters.GenerateExecutable = false;
                _CompilerParameters.GenerateInMemory = true;
            }


            // Generate debug information.
            //_CompilerParameters.IncludeDebugInformation = true;

            // Set the level at which the compiler
            // should start displaying warnings.
            _CompilerParameters.WarningLevel = 3;

            // Set whether to treat all warnings as errors.
            _CompilerParameters.TreatWarningsAsErrors = false;

            // Set compiler argument to optimize output.
            _CompilerParameters.CompilerOptions = "/optimize";

            // Set a temporary files collection.
            // The TempFileCollection stores the temporary files
            // generated during a build in the current directory,
            // and does not delete them after compilation.
            _CompilerParameters.TempFiles = new System.CodeDom.Compiler.TempFileCollection(".", true);

            if (_ResourceFiles != null && _ResourceFiles.Length > 0)
                foreach (string _ResourceFile in _ResourceFiles)
                {
                    // Set the embedded resource file of the assembly.
                    _CompilerParameters.EmbeddedResources.Add(_ResourceFile);
                }


            try
            {
                // Invoke compilation
                System.CodeDom.Compiler.CompilerResults _CompilerResults = null;

                if (_SourceFile != null && System.IO.File.Exists(_SourceFile))
                    // soruce code in external file
                    _CompilerResults = _CodeCompiler.CompileAssemblyFromFile(_CompilerParameters, _SourceFile);
                else
                    // source code pass as a string
                    _CompilerResults = _CodeCompiler.CompileAssemblyFromSource(_CompilerParameters, _SourceCode);

                if (_CompilerResults.Errors.Count > 0)
                {
                    // Return compilation errors
                    _Errors = "";
                    foreach (System.CodeDom.Compiler.CompilerError CompErr in _CompilerResults.Errors)
                    {
                        _Errors += "Line number " + CompErr.Line +
                                                   ", Error Number: " + CompErr.ErrorNumber +
                                                   ", '" + CompErr.ErrorText + ";\r\n\r\n";
                    }

                    // Return the results of compilation - Failed
                    return false;
                }
                else
                {
                    // no compile errors
                    _Errors = null;
                }
            }
            catch (Exception _Exception)
            {
                // Error occurred when trying to compile the code
                _Errors = _Exception.Message;
                return false;
            }

            // Return the results of compilation - Success
            return true;
        }

        public static void Main (string[] args)
        {
            string _Errors = "";

            string _CSharpSourceCode = @"
               using System;
 
               namespace test
               {
                   class Program
                   {
                       static void Main(string[] args)
                       {
                           Console.WriteLine(""Press ENTER key to start ..."");
                           Console.ReadLine();
                           for (int c=0; c<=100; c++)
                               Console.WriteLine(c.ToString());
                       }
                       public Object hola()
                       {
                            Console.WriteLine(""Hola mundo"");
                            return new Object();
                       }
                   }
               }";
 

            // Debes compilar el codigo que hay en code
            // Pero la compilacion debe ser activada justo debajo de esta linea
            // no vale sacar el codigo del string y decirle a vstudio que lo haga :-)
            // Debes llamar al metodo SampleClass.SayHello
           
            //Y debemos ver que sale lo apropiado por pantalla.
            MainClass mc = new MainClass();
            if (mc.CompileCode(new Microsoft.CSharp.CSharpCodeProvider(), _CSharpSourceCode, null, "c:\\temp\\C-Sharp-test.exe", null, null, ref _Errors))
            {
                Console.WriteLine("Code compiled successfully");

                try
                {
                    // Forma 1:
/*
                    var laClase = "test.Program";
                    Type elTipo = Type.GetType(laClase);
                    Object obj = Activator.CreateInstance(elTipo.MakeGenericType()) as Object;
 */
                    // Forma 2:
                   // AppDomain.CurrentDomain.CreateInstance(

                    // Forma 3:
/*
                    ObjectHandle o = Activator.CreateInstance("mscorlib.dll", "System.Int32");
                    Int32 i = (Int32)o.Unwrap();
*/
                    // Forma 4:
                    // http://www.west-wind.com/presentations/DynamicCode/DynamicCode.htm
/*
                    
 */
                    ICodeCompiler loCompiler = new CSharpCodeProvider().CreateCompiler();

                    CompilerParameters loParameters = new CompilerParameters();
                    loParameters.ReferencedAssemblies.Add("System.dll");
                    loParameters.GenerateInMemory = true;

                    // *** Now compile the whole thing
                    CompilerResults loCompiled =
                            loCompiler.CompileAssemblyFromSource(loParameters, _CSharpSourceCode);

                    if (loCompiled.Errors.HasErrors)
                    {
                        string lcErrorMsg = "";

                        lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                        for (int x = 0; x < loCompiled.Errors.Count; x++)
                            lcErrorMsg = lcErrorMsg + "\r\nLine: " +
                                loCompiled.Errors[x].Line.ToString() + " - " +
                                loCompiled.Errors[x].ErrorText;

                        Console.WriteLine(lcErrorMsg + "\r\n\r\n" + _CSharpSourceCode);
                        return;
                    }

                    Assembly loAssembly = loCompiled.CompiledAssembly;
                    // *** Retrieve an obj ref – generic type only
                    Object loObject = loAssembly.CreateInstance("test.Program");

                    if (loObject == null)
                    {
                        Console.WriteLine("Couldn't load class.");
                        return;
                    }

                    Object[] loCodeParms = new Object[1];
                    loCodeParms[0] = "Valor del parámetro";
                    var methods = loObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                    Object loResult = loObject.GetType().InvokeMember(
                                           "hola", BindingFlags.InvokeMethod ,
                                           null, loObject, null);
                    Console.WriteLine(loResult);
 
                    Console.WriteLine("Instancia llamada");
                }
                catch (Exception e)
                {
                    Console.WriteLine("No se ha podido crear el objeto.");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Error occurred during compilation : \r\n" + _Errors);
            }
            String c = Console.ReadLine();

            
        }
    }
}
