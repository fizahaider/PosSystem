using System;
using System.Collections.Generic;
using System.Text;

namespace POSSystem.Helpers
{
    public static class TaxHelper
    {
        public static decimal CalculateTax(decimal subtotal)
        {
            return subtotal * 0.15m;
        }
    }
}
