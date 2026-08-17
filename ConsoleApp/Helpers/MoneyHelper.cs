using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Models;

namespace POSSystem.Helpers
{
    public static class MoneyHelper
    {
        public static string Format(decimal amount)
        {
            return amount.ToString("C");
        }
    }
}
