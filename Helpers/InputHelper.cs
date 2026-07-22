using System;
using System.Collections.Generic;
using System.Text;
using POSSystem.Models;

namespace POSSystem.Helpers
{
    public static class InputHelper
    {
        public static bool TryParsePrice(string input, out decimal price)
        {
            return decimal.TryParse(input, out price);
        }
    }
}
