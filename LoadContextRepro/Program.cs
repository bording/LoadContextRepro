using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Library;

namespace LoadContextRepro
{
    class Program
    {
        const string libraryPath = @"..\..\..\..\Library\bin\Debug\netstandard2.0\Library.dll";
        const string loaderPath = @"..\..\..\..\Loader\bin\Debug\netcoreapp3.1\Loader.dll";

        static void Main(string[] args)
        {
            BasicTypeEquivalence();

            HandlerScenario("ScanFail");
            HandlerScenario("ScanWorks");
        }

        private static void BasicTypeEquivalence()
        {
            var person = new Person();
            Console.WriteLine(person.Name);

            var context = new CustomAssemblyLoadContext();

            var customAssembly = context.LoadFromAssemblyPath(Path.GetFullPath(libraryPath));
            var defaultAssembly = AssemblyLoadContext.Default.Assemblies.Single(a => a.GetName().Name == "Library");

            var assemblyTypesEqual = customAssembly.Equals(defaultAssembly); //false

            var customPerson = customAssembly.GetTypes().Single(t => t.Name == "Person");
            var defaultPerson = defaultAssembly.GetTypes().Single(t => t.Name == "Person");

            var personTypesEqual = customPerson.Equals(defaultPerson); //false
        }

        private static void HandlerScenario(string methodName)
        {
            var context = new CustomAssemblyLoadContext();

            var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(loaderPath));

            var scannerType = assembly.GetTypes().Single(t => t.Name == "AssemblyScanner");
            var instance = Activator.CreateInstance(scannerType);

            var method = scannerType.GetMethod(methodName);

            method.Invoke(instance, null);
        }
    }
}
