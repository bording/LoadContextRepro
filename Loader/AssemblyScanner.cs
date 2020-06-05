using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Loader
{
    public class AssemblyScanner
    {
        const string assemblyPath = @"..\..\..\..\Handlers\bin\Debug\netcoreapp3.1\Handlers.dll";

        public void ScanFail()
        {
            var assembly = Assembly.LoadFrom(Path.GetFullPath(assemblyPath));

            var handlerType = assembly.GetTypes().Single(t => t.Name == "TestHandler");
            var interfaceType = handlerType.GetInterface("IHandler");

            var equal = interfaceType.Equals(typeof(IHandler)); //false
        }

        public void ScanWorks()
        {
            var context = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(assemblyPath));

            var handlerType = assembly.GetTypes().Single(t => t.Name == "TestHandler");
            var interfaceType = handlerType.GetInterface("IHandler");

            var equal = interfaceType.Equals(typeof(IHandler)); //true
        }
    }
}
