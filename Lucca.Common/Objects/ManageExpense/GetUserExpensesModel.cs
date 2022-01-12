using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Objects.ManageExpense
{
    public class GetUserExpensesModel
    {
        [Required]
        public int UserId { get; set; }       

        public bool DateFilter { get; set; }
        public bool DateDesc { get; set; }
        public bool ValueFilter { get; set; }
        public bool ValueDesc { get; set; }
    }
}
