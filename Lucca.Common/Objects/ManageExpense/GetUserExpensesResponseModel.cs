using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Objects.ManageExpense
{
    public class GetUserExpensesResponseModel
    {
        public int ExpenseId { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public string ExpenseType { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }
        public string Comment { get; set; }
    }
}
