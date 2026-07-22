using System;
using System.Collections.Generic;
using System.Text;

namespace POSSystem.Helpers
{

    public static class StockHelper
    {
        public static void ReduceStock(ref int stock, int quantity)
        {
            stock = stock - quantity;
        }
    }
}
