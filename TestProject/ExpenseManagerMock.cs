using Lucca.Common.Interface.Business;
using Lucca.Common.Objects;
using Lucca.Common.Objects.DTO;
using Lucca.Common.Objects.ManageExpense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ExpenseManagerMock : IExpenseManager
    {
        Task<ValidationModel> IExpenseManager.CreateExpense(CreateExpenseModel createExpenseModel)
        {
            throw new NotImplementedException();
        }

        Task<List<ExpenseDTO>> IExpenseManager.GetAllUserExpenses(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
