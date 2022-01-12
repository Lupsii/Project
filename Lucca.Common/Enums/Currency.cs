using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Enums
{
    public enum Currency
    {
        [Description("US Dollar")]
        USD,
        [Description("Euro")]
        EUR,
        [Description("Russian Ruble")]
        RUB,
    }
}
