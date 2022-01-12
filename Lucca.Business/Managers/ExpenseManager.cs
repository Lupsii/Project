using Lucca.Common.Enums;
using Lucca.Common.Interface.Business;
using Lucca.Common.Interface.DAL;
using Lucca.Common.Objects;
using Lucca.Common.Objects.DTO;
using Lucca.Common.Objects.ManageExpense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.Business.Managers
{
    public class ExpenseManager: IExpenseManager
    {

        private readonly ISQLService _sql;

        public ExpenseManager(ISQLService sql)
        {
            _sql = sql;
        }

        public async Task<ValidationModel> CreateExpense(CreateExpenseModel createExpenseModel)
        {
            try
            {                
                var newExpenseDTO = new ExpenseDTO();

                var user = await _sql.GetUserById(createExpenseModel.UserId);

                if (user == null)
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid user. User doesn't exist." },
                        Succeeded = false
                    };
                }

                newExpenseDTO.User = user;

                var existingExpense = await _sql.GetExpenseByDateValueCurrency(createExpenseModel.Date
                    , createExpenseModel.Value
                    , (Currency)Enum.Parse(typeof(Currency), createExpenseModel.Currency)
                    , (ExpenseType)Enum.Parse(typeof(ExpenseType), createExpenseModel.ExpenseType)
                    );

                if (existingExpense != null)
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid Expense creation. This expense is already registered." },
                        Succeeded = false
                    };
                }

                if (Enum.IsDefined(typeof(ExpenseType), createExpenseModel.ExpenseType)) 
                {
                    newExpenseDTO.ExpenseType = (ExpenseType)Enum.Parse(typeof(ExpenseType), createExpenseModel.ExpenseType);
                }
                else
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid Expense type. Expense type doesn't exist." },
                        Succeeded = false
                    };
                }
                

                if (createExpenseModel.Date < DateTime.Now && createExpenseModel.Date > DateTime.Now.AddMonths(-3)) 
                {
                    newExpenseDTO.Date = createExpenseModel.Date;
                }
                else
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid Expense type. Expense type doesn't exist." },
                        Succeeded = false
                    };
                }

                if (Enum.IsDefined(typeof(Currency), createExpenseModel.Currency)) 
                {
                    newExpenseDTO.Currency = (Currency)Enum.Parse(typeof(Currency), createExpenseModel.Currency);
                }
                else
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid Expense type. Expense type doesn't exist." },
                        Succeeded = false
                    };
                }

                if(newExpenseDTO.Currency != user.Currency)
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Invalid currency type. This user doesn't use this currency." },
                        Succeeded = false
                    };
                }

                newExpenseDTO.Comment = createExpenseModel.Comment;
                if(createExpenseModel.Comment is null || newExpenseDTO.Comment == "")
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Comment is required." },
                        Succeeded = false
                    };
                }

                newExpenseDTO.Value = createExpenseModel.Value;

                var result = await _sql.CreateExpense(newExpenseDTO);

                if (result) 
                { 
                    return new ValidationModel()
                    {
                        Succeeded = true
                    };
                }
                else
                {
                    return new ValidationModel()
                    {
                        Errors = new List<string>() { "Unable to create expense in database." },
                        Succeeded = false
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during Expense creation in manager : " + ex.Message);
            }
        }

        public async Task<List<ExpenseDTO>> GetAllUserExpenses(int userId)
        {
            try
            {

                var listExpenseDTO = new List<ExpenseDTO>();

                return listExpenseDTO = await _sql.GetAllExpensesByUserId(userId);                

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting All Expenses of this user in manager : " + ex.Message);
            }
        }
    }
}
