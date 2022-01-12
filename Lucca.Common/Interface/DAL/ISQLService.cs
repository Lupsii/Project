using Lucca.Common.Enums;
using Lucca.Common.Objects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Common.Interface.DAL
{
    public interface ISQLService
    {
        Task<UserDTO> GetUserById(int userId);
        Task<bool> CreateExpense(ExpenseDTO expenseDTO);
        Task<List<ExpenseDTO>> GetAllExpensesByUserId(int userId);
        Task<ExpenseDTO> GetExpenseByDateValueCurrency(DateTime date, double value, Currency currency, ExpenseType expenseType);
    }
}
