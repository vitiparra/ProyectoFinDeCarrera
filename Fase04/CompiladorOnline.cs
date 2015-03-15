using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Fase04
{
    class CompiladorOnline
    {
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
        public bool CompileCode(System.CodeDom.Compiler.CodeDomProvider _CodeProvider, string _SourceCode, string _SourceFile, string _ExeFile, string _AssemblyName, string[] _ResourceFiles, ref string _Errors)
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
                _CompilerParameters.GenerateExecutable = false;
                _CompilerParameters.GenerateInMemory = true; // Esto será lo que haya que cambiar para que se cargue en memoria
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
    }
}
