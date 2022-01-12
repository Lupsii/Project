using Lucca.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Objects.DTO
{
    public class ExpenseDTO
    {
        public int ExpenseId { get; set; }
        public UserDTO User { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public double Value { get; set; }
        public Currency Currency { get; set; }
        public string Comment { get; set; }

    }
}
