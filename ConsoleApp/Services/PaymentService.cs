using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Models;
using POSSystem.Helpers;

namespace POSSystem.Services
{
    public class PaymentService
    {
        public async Task ProcessAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Processing Payment");
            await Task.Delay(200);
            Console.WriteLine("Payment Successful");
        }

        public async Task LoadCatalogAsync()
        {
            Console.WriteLine("Loading Catalog");
            await Task.Delay(1500);

            Console.WriteLine("Catalog Loaded.");
        }
    }
}
