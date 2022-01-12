using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Objects.ManageExpense
{
    public class ExpenseApiRequestModel
    {
        
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ExpenseType { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
