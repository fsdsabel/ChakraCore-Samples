using ChakraCore.NET;
using ChakraCore.NET.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugAdapter2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader("Data.txt"))
            {

                var hosting = JavaScriptHosting.Default;
                var config = new JavaScriptHostingConfig();


                config.AddModuleFolder("Scripts"); //set script folder
                                
                var app = hosting.GetModuleClass("SimpleProcessor", "App", config); //get javascript class



                var runtimeService = app.ServiceNode.GetService<IRuntimeService>();

                using (var debugger = new ChakraCore.NET.DebugAdapter2.VSCode.Managed.VSCodeChakraDebugger2Managed(runtimeService.Runtime))
                {
                    debugger.Start("test", true, 5000);
                    debugger.WaitForDebugger();



                    InjectDictionaryConverter(app.ServiceNode.GetService<IJSValueConverterService>());

                    int line = 0;
                    dict.Add("Line", null);

                    app.CallMethod<IDictionary<string, string>>("Init", dict);

                    string data = reader.ReadLine();
                    string tmp;
                    while (data != null)
                    {
                        line++;
                        dict["Line"] = line.ToString();
                        tmp = app.CallFunction<string, string>("Process", data);
                        Console.WriteLine(tmp);
                        data = reader.ReadLine();
                        dumpDict(dict);
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("Processing done, press enter to exit");
            Console.ReadLine();



        }

        private static void dumpDict(Dictionary<string, string> dict)
        {
            Console.WriteLine("---Dictionary---");
            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key}->{item.Value}");
            }
        }

        private static void InjectDictionaryConverter(IJSValueConverterService service)
        {
            service.RegisterArrayConverter<string>();

            service.RegisterProxyConverter<IDictionary<string, string>>(
                (binding, dict, node) =>
                {
                    DictionaryWrapper wrapper = new DictionaryWrapper(dict);
                    binding.SetFunction<IEnumerable<string>>("GetKeys", wrapper.GetKeys);
                    binding.SetMethod<string, string>("SetItem", wrapper.SetItem);
                    binding.SetFunction<string, string>("GetItem", wrapper.GetItem);
                    binding.SetFunction<string, bool>("ContainsKey", wrapper.ContainsKey);
                }

                );
        }

    }
}
