using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Enums
{
    public enum ExpenseType
    {
        [Description("Restaurant")]
        Restaurant,
        [Description("Hotel")]
        Hotel,
        [Description("Misc")]
        Misc,
    }
}
