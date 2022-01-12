using Lucca.Common.Objects;
using Lucca.Common.Objects.DTO;
using Lucca.Common.Objects.ManageExpense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Interface.Business
{
    public interface IExpenseManager
    {
        Task<ValidationModel> CreateExpense(CreateExpenseModel createExpenseModel);
        Task<List<ExpenseDTO>> GetAllUserExpenses(int userId);
    }
}
