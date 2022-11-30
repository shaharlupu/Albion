using System;
using System.Collections.Generic;
using System.Linq;
using Albion.Engine;
using Albion.Google;
using Albion.Native;

namespace Albion.Code
{
    public class Program
    {
        FilteringEngine engine = new FilteringEngine();

        public static void Main()
        {
            Program program = new Program();
            try
            {
                program.Run();
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                program.Dispose();
            }
        }

        private void Run()
        {
            engine.Initialize();

            // Test();
            
            // PrintAllFilters();
            // PrintLayers();
            // PrintSubLayers();
            
            using (FiltersContext context = new FiltersContext())
            {
                AddTestFilters(context);
            }
            
            PrintFilters(engine.GetFiltersForProvider());

            // Console.WriteLine("Added RPC filters. Press enter to delete them...");
            //
            Console.ReadLine();
            //
            engine.Clear();
        }

        private void Test()
        {
            try
            {
                var filter = engine.GetFilterById(72559);

                Console.WriteLine($"sublayer: {filter.subLayerKey}, name: {filter.displayData.name}, description: {filter.displayData.description}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PrintFilters(List<FWPM_FILTER0> filters)
        {
            Console.WriteLine($"filters ({filters.Count})");
            
            foreach (var filter in filters)
            {
                Console.WriteLine($"{filter.filterId} {filter.displayData.name}");
            }
            
            Console.WriteLine();
        }

        private void PrintLayers()
        {
            var layers = engine.GetLayers().ToList();
            Console.WriteLine($"layers ({layers.Count})");
            
            foreach (var layer in layers)
            {
                Console.WriteLine($"{layer.layerId} {layer.displayData.name}");
            }
            
            Console.WriteLine();
        }

        private void PrintSubLayers()
        {
            var sublayers = engine.GetSubLayers().ToList();
            Console.WriteLine($"sublayers ({sublayers.Count})");
        }

        private void AddTestFilters(FiltersContext context)
        {
            Guid[] rpcInterfaces =
            {
                new Guid("86d35949-83c9-4044-b424-db363231fd0c"), // task scheduler
                new Guid("e3514235-4b06-11d1-ab04-00c04fc2dcd2"), //drsuapi
                new Guid("367abb81-9844-35f1-ad32-98f038001003"), //svcctl
                new Guid("12345778-1234-abcd-ef00-0123456789ac"), //samr
            };

            foreach (Guid rpcInterface in rpcInterfaces)
            {
                engine.AddRpcFilter(context, FWP_ACTION_TYPE.BLOCK, rpcInterface);
            }
        }

        private void Dispose()
        {
            engine.Dispose();
        }
    }
}