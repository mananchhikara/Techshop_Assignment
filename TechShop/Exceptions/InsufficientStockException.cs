using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Exceptions
{
    public class InsufficientStockException : Exception
    {
        // Parameterless constructor
        public InsufficientStockException() { }

        // Constructor that accepts a custom error message
        public InsufficientStockException(string message) : base(message) { }

        // Constructor that accepts a custom message and an inner exception
        public InsufficientStockException(string message, Exception inner) : base(message, inner) { }
    }
}
