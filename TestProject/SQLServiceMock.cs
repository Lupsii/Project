using Lucca.Common.Enums;
using Lucca.Common.Interface.DAL;
using Lucca.Common.Objects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class SQLServiceMock : ISQLService
    {
        Task<bool> ISQLService.CreateExpense(ExpenseDTO expenseDTO)
        {
            throw new NotImplementedException();
        }

        Task<List<ExpenseDTO>> ISQLService.GetAllExpensesByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        Task<ExpenseDTO> ISQLService.GetExpenseByDateValueCurrency(DateTime date, double value, Currency currency, ExpenseType expenseType)
        {
            throw new NotImplementedException();
        }

        Task<UserDTO> ISQLService.GetUserById(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
