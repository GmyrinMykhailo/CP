using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorsApplication
{
    
    internal class Error
    {
        public string message;
        public string detail;

        public Error(string message, string detail)
        {
            this.message = message;
            this.detail = detail;
        }
    }
}
