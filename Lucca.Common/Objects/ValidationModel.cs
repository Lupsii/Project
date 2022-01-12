using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Objects
{
    public class ValidationModel
    {
        public List<string> Errors { get; set; }
        public bool Succeeded { get; set; }
    }
}
