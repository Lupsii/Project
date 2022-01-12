using Lucca.Common.Enums;
using Lucca.Common.Interface.DAL;
using Lucca.Common.Objects.DTO;
using Lucca.DAL.SQL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.DAL.SQL
{
    public class SQLService : ISQLService
    {
        private readonly ExpenseContext _expenseContext;

        public SQLService(ExpenseContext expenseContext)
        {
            _expenseContext = expenseContext;
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            var user = await _expenseContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            UserDTO userDTO = new UserDTO();            

            return userDTO = user.ToDto();

        }

        public async Task<bool> CreateExpense(ExpenseDTO expenseDTO)
        {
            Expense newExpense = new Expense()
            {
                User = await _expenseContext.Users.FirstOrDefaultAsync(x => x.UserId == expenseDTO.User.UserId),
                ExpenseType = expenseDTO.ExpenseType,
                Value = expenseDTO.Value,
                Currency = expenseDTO.Currency,
                Comment = expenseDTO.Comment,
                Date = expenseDTO.Date
            };

            await _expenseContext.Expenses.AddAsync(newExpense);

            await _expenseContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ExpenseDTO>> GetAllExpensesByUserId(int userId)
        {
            List<ExpenseDTO> listExpenseDTOs = new List<ExpenseDTO>();

            var expenses = await _expenseContext.Expenses.Include(e => e.User).Where(e => e.User.UserId == userId).ToListAsync();

            if(expenses is not null && expenses.Count > 0) 
            { 
                foreach (var expense in expenses)
                {
                    var expenseDTO = new ExpenseDTO();
                    expenseDTO = expense.ToDto();
                    listExpenseDTOs.Add(expenseDTO);
                }
            }

            return listExpenseDTOs;
        }

        public async Task<ExpenseDTO> GetExpenseByDateValueCurrency(DateTime date, double value, Currency currency, ExpenseType expenseType)
        {
            ExpenseDTO existingExpense = new ExpenseDTO();

            var expense = await _expenseContext.Expenses.Where(e => e.Date == date && e.Value == value && e.Currency == currency && e.ExpenseType == expenseType).SingleOrDefaultAsync();

            if (expense is not null)
                return expense.ToDto();
            else
                return null;
        }
    }
}
