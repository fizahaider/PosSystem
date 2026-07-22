using Newtonsoft.Json;
using POSSystem.Helpers;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSSystem.Services
{
    public class CatalogService
    {
            public static void SaveCatalog(List<Product> products)
            {
                string json =JsonConvert.SerializeObject(products, Formatting.Indented);

                File.WriteAllText("catalog.json", json);

                Console.WriteLine("Catalog Saved.");
            }

            public static List<Product> LoadCatalog()
            {
                if (File.Exists("catalog.json"))
                {
                    string json = File.ReadAllText("catalog.json");

                    List<Product> products =
                        JsonConvert.DeserializeObject<List<Product>>(json);

                    return products;
                }

                return new List<Product>();
            }
        
    }
}
