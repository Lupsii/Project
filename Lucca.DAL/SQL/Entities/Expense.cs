using Lucca.Common.Enums;
using Lucca.Common.Objects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.DAL.SQL.Entities
{
    public class Expense
    {

        public int ExpenseId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public double Value { get; set; }
        public Currency Currency { get; set; }
        public string Comment { get; set; }

        public ExpenseDTO ToDto()
        {
            var dto = new ExpenseDTO()
            {
                ExpenseId = this.ExpenseId,
                Date = this.Date,
                ExpenseType = this.ExpenseType,
                Value = this.Value,
                Currency = this.Currency,
                Comment = this.Comment
            };

            if (this.User is not null)
            {
                dto.User = this.User.ToDto();
            }

            return dto;
        }
    }
}
