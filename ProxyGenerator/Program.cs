using Server.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProxyGenerator
{
    class Program
    {
        private const string SERVICE_NAME = "{SERVICENAME}";
        private const string METHODS = "{METHODS}";
        private const string RETURN_TYPE = "{RETURNTYPE}";
        private const string METHOD_NAME = "{METHODNAME}";
        private const string PARAMS = "{PARAMS}";
        private const string PARAM_VALUES = "{PARAMVALUES}";
        private static string[] methodsToSkip = { "ToString", "Equals", "GetHashCode", "GetType" };

        static void Main(string[] args)
        {
            string targetDirectory = args.Length > 0 ? args[0] : new DirectoryInfo(@"..\..\..\..\ServerProxy").FullName;

            Console.WriteLine("Deleting old files...");
            foreach (var file in Directory.GetFiles(targetDirectory, "*.cs"))
            {
                File.Delete(file);
            }
            Console.WriteLine("Old files deleted");

            var serverAssembly = Assembly.GetAssembly(typeof(IService));
            string template = Properties.Resources.ServiceTemplateV1;

            foreach (var service in serverAssembly.GetTypes().Where(type => typeof(IService).IsAssignableFrom(type) && type.IsClass))
            {
                Console.WriteLine($"Creating proxy for {service}...");
                template = template.Replace(SERVICE_NAME, service.Name);
                var methods = new List<string>();

                foreach (var method in service.GetMethods())
                {
                    if (methodsToSkip.Contains(method.Name))
                    {
                        continue;
                    }

                    var parameters = method.GetParameters().Select(p => new Parameter
                    {
                        Name = p.Name,
                        Type = p.ParameterType
                    });

                    string methodTemplate = Properties.Resources.MethodTemplateV1
                        .Replace(METHOD_NAME, method.Name)
                        .Replace(RETURN_TYPE, method.ReturnType.FullName)
                        .Replace(PARAMS, string.Join(',', parameters.Select(p => p.ToString())))
                        .Replace(PARAM_VALUES, string.Join(',', parameters.Select(p => p.Name)))
                        .Replace(SERVICE_NAME, service.Name);

                    methods.Add(methodTemplate);
                }

                template = template.Replace(METHODS, string.Join(Environment.NewLine, methods));

                File.WriteAllText(Path.Combine(targetDirectory, service.Name + ".cs"), template);
                Console.WriteLine($"Proxy for {service} created");
            }

            Console.WriteLine("Completed, press any key to exit");
            Console.ReadKey();
        }
    }
}
