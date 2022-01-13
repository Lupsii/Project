using Lucca.Common.Enums;
using Lucca.Common.Interface.Business;
using Lucca.Common.Objects.DTO;
using Lucca.Common.Objects.ManageExpense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lucca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        private readonly IExpenseManager _expenseManager;

        public ExpensesController(IExpenseManager expenseManager)
        {
            _expenseManager = expenseManager;
        }


        // Create Expense
        [HttpPost("Create")]
        public async Task<IActionResult> Post(ExpenseApiRequestModel expenseApiRequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createExpenseModel = new CreateExpenseModel()
                    {
                        UserId = expenseApiRequestModel.UserId,
                        ExpenseType = expenseApiRequestModel.ExpenseType,
                        Date = expenseApiRequestModel.Date,
                        Value = expenseApiRequestModel.Value,
                        Currency = expenseApiRequestModel.Currency,
                        Comment = expenseApiRequestModel.Comment
                    };

                    var Result = await _expenseManager.CreateExpense(createExpenseModel);

                    if (!Result.Succeeded)
                        return BadRequest(Result.Errors);

                    return Ok("The expense has been created !");
                }
                else
                {
                    return BadRequest("Invalid Payload");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the create of an Expense : " + ex.Message);
            }
        }

        // All expenses by user
        [HttpGet("AllUserExpenses")]
        public async Task<IActionResult> GetAllUserExpenses(GetUserExpensesModel getUserExpensesApiRequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var expenseDTOs = await _expenseManager.GetAllUserExpenses(getUserExpensesApiRequestModel.UserId);

                    if(expenseDTOs == null)
                    {
                        return NoContent();
                    }

                    // My Choice => Date over Value
                    if(getUserExpensesApiRequestModel.DateFilter && getUserExpensesApiRequestModel.ValueFilter)
                    {

                        // ASC - ASC
                        if( !getUserExpensesApiRequestModel.DateDesc && !getUserExpensesApiRequestModel.ValueDesc)
                            expenseDTOs = expenseDTOs.OrderBy(e => e.Date).ThenBy(v => v.Value).ToList();

                        // ASC - DESC
                        else if (!getUserExpensesApiRequestModel.DateDesc && getUserExpensesApiRequestModel.ValueDesc)
                                expenseDTOs = expenseDTOs.OrderBy(e => e.Date).ThenByDescending(v => v.Value).ToList();

                        // DESC - ASC
                        else if (getUserExpensesApiRequestModel.DateDesc && !getUserExpensesApiRequestModel.ValueDesc)
                                expenseDTOs = expenseDTOs.OrderByDescending(e => e.Date).ThenBy(v => v.Value).ToList();

                        // DESC - DESC
                        else if (getUserExpensesApiRequestModel.DateDesc && getUserExpensesApiRequestModel.ValueDesc)
                                expenseDTOs = expenseDTOs.OrderByDescending(e => e.Date).ThenByDescending(v => v.Value).ToList();

                    }

                    else if (getUserExpensesApiRequestModel.ValueFilter)
                    {
                        // ASC
                        if(!getUserExpensesApiRequestModel.ValueDesc)
                            expenseDTOs = expenseDTOs.OrderBy(v => v.Value).ToList();

                        // DESC
                        else 
                            expenseDTOs = expenseDTOs.OrderByDescending(v => v.Value).ToList();
                    }

                    else if (getUserExpensesApiRequestModel.DateFilter)
                    {
                        // ASC
                        if (!getUserExpensesApiRequestModel.DateDesc)
                            expenseDTOs = expenseDTOs.OrderBy(v => v.Date).ToList();

                        // DESC
                        else
                            expenseDTOs = expenseDTOs.OrderByDescending(v => v.Date).ToList();
                    }


                    List<GetUserExpensesResponseModel> response = new List<GetUserExpensesResponseModel>();
                    // Json Creation
                    foreach(ExpenseDTO expense in expenseDTOs)
                    {
                        GetUserExpensesResponseModel newItem = new GetUserExpensesResponseModel()
                        {
                            ExpenseId = expense.ExpenseId,
                            User = expense.User.FirstName + " " + expense.User.LastName,
                            Date = expense.Date.ToString("dd/MM/yyyy"),
                            ExpenseType = Enum.GetName(typeof(ExpenseType), expense.ExpenseType),
                            Currency = Enum.GetName(typeof(Currency), expense.Currency),
                            Value = expense.Value,
                            Comment = expense.Comment
                        };
                        response.Add(newItem);
                    }


                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid Payload");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured during the create of an Expense : " + ex.Message);
            }
        }



        // Get Expenses by user

        // Get Expense by User

        // Get all Expenses + filters

        // Get Expense by Id
    }
}
