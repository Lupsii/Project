using Lucca.Business.Managers;
using Lucca.Common.Enums;
using Lucca.Common.Objects;
using Lucca.Common.Objects.DTO;
using Lucca.Common.Objects.ManageExpense;
using Lucca.DAL.SQL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        List<UserDTO> Users = new List<UserDTO>();
        List<ExpenseDTO> Expenses = new List<ExpenseDTO>();

        public void SeedDatabase()
        {            
            Users.Add(new UserDTO()
            {
                UserId = 1,
                FirstName = "Anthony",
                LastName = "Stark",
                Currency = Currency.USD
            });
            Users.Add(new UserDTO { 
                UserId = 2, 
                FirstName = "Natasha", 
                LastName = "Romanova",
                Currency = Currency.RUB 
            });
                
        }

        public void FeedExpensesTable()
        {
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 1,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Restaurant,
                Date = new DateTime(2022, 01, 01), ///////////
                Value = 18.00,
                Currency = Currency.USD,
                Comment = "Resto 1"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 2,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Hotel,
                Date = new DateTime(2022, 01, 02), ///////////
                Value = 63.00,
                Currency = Currency.USD,
                Comment = "Hotel 1"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 3,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Misc,
                Date = new DateTime(2022, 01, 12), ///////////
                Value = 23.00,
                Currency = Currency.USD,
                Comment = "Taxi 2"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 4,
                User = GetUserById(2),
                ExpenseType = ExpenseType.Misc,
                Date = new DateTime(2022, 01, 04), ///////////
                Value = 70.20,
                Currency = Currency.RUB,
                Comment = "Natasha Misc"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 5,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Misc,
                Date = new DateTime(2022, 01, 05), ///////////
                Value = 53.00,
                Currency = Currency.USD,
                Comment = "Taxi 1"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 6,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Hotel,
                Date = new DateTime(2022, 01, 06), ///////////
                Value = 70.20,
                Currency = Currency.USD,
                Comment = "Hotel 2"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 7,
                User = GetUserById(1),
                ExpenseType = ExpenseType.Restaurant,
                Date = new DateTime(2022, 01, 12), ///////////
                Value = 25.90,
                Currency = Currency.USD,
                Comment = "Resto 2"
            });
            Expenses.Add(new ExpenseDTO()
            {
                ExpenseId = 8,
                User = GetUserById(2),
                ExpenseType = ExpenseType.Restaurant,
                Date = new DateTime(2022, 01, 12), ///////////
                Value = 25.90,
                Currency = Currency.RUB,
                Comment = "Natasha Resto"
            });
        }


        #region Controller
        public List<GetUserExpensesResponseModel> SortData(GetUserExpensesModel getUserExpensesApiRequestModel)
        {
            var expenseDTOs = GetAllExpensesByUserId(getUserExpensesApiRequestModel.UserId);

            if (expenseDTOs == null)
            {
                return null;
            }

            // My Choice => Date over Value
            if (getUserExpensesApiRequestModel.DateFilter && getUserExpensesApiRequestModel.ValueFilter)
            {

                // ASC - ASC
                if (!getUserExpensesApiRequestModel.DateDesc && !getUserExpensesApiRequestModel.ValueDesc)
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
                if (!getUserExpensesApiRequestModel.ValueDesc)
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
            foreach (ExpenseDTO expense in expenseDTOs)
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

            return response;
        }

        #endregion


        #region ExpenseManager 
        public ValidationModel CreateExpense(CreateExpenseModel createExpenseModel)
        {
            var newExpenseDTO = new ExpenseDTO();

            var user = GetUserById(createExpenseModel.UserId);

            if (user == null)
            {
                new ValidationModel()
                {
                    Errors = new List<string>() { "Invalid user. User doesn't exist." },
                    Succeeded = false
                };
            }

            newExpenseDTO.User = user;

            var existingExpense = GetExpenseByDateValueCurrency(createExpenseModel.Date
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


            if (createExpenseModel.Date <= DateTime.Now && createExpenseModel.Date >= DateTime.Now.AddMonths(-3))
            {
                newExpenseDTO.Date = createExpenseModel.Date;
            }
            else
            {
                return new ValidationModel()
                {
                    Errors = new List<string>() { "Invalid Expense date. Date is older than 3 months." },
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
                    Errors = new List<string>() { "Invalid currency. Expense currency doesn't exist." },
                    Succeeded = false
                };
            }

            if (newExpenseDTO.Currency != user.Currency)
            {
                return new ValidationModel()
                {
                    Errors = new List<string>() { "Invalid currency . This user doesn't use this currency." },
                    Succeeded = false
                };
            }

            newExpenseDTO.Comment = createExpenseModel.Comment;
            if (createExpenseModel.Comment is null || newExpenseDTO.Comment == "")
            {
                return new ValidationModel()
                {
                    Errors = new List<string>() { "Comment is required." },
                    Succeeded = false
                };
            }

            newExpenseDTO.Value = createExpenseModel.Value;

            var result = CreateExpense(newExpenseDTO);

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




        #endregion

        #region SQLService 
        public UserDTO GetUserById(int userId)
        {
            return Users.SingleOrDefault(x => x.UserId == userId);
        }

        public ExpenseDTO GetExpenseByDateValueCurrency(DateTime date, double value, Currency currency, ExpenseType expenseType)
        {
            return Expenses.SingleOrDefault(e => e.Date == date && e.Value == value && e.Currency == currency && e.ExpenseType == expenseType);
        }

        public bool CreateExpense(ExpenseDTO expenseDTO)
        {
            Expenses.Add(expenseDTO);
            return true;
        }

        public List<ExpenseDTO> GetAllExpensesByUserId(int userId)
        {
            List<ExpenseDTO> listExpenseDTOs = new List<ExpenseDTO>();

            listExpenseDTOs = Expenses.Where(e => e.User.UserId == userId).ToList();            

            return listExpenseDTOs;
        }

        #endregion





        [TestMethod]
        public void TestSeed()
        {

            SeedDatabase();

            Assert.IsTrue(Users.Count>0);
            Assert.IsTrue(Users.Single(u => u.UserId == 1).FirstName == "Anthony");
            Assert.IsTrue(Users.Single(u => u.UserId == 2).FirstName == "Natasha");
        }

        [TestMethod]
        public void NewExpense()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = new DateTime(2022,01,12),
                Value = 153.00,
                Currency = "USD",
                Comment = "Berlinstrasse"
            };

            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);
            Assert.IsTrue(validationModel.Succeeded, "Expense correctly added");
        }

        [TestMethod]
        public void AddSameExpenseTwice()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = new DateTime(2022,01,12),
                Value = 153.00,
                Currency = "USD",
                Comment = "Berlinstrasse"
            };

            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);
            Assert.IsTrue(validationModel.Succeeded, "Expense correctly added");

            var validationModel2 = CreateExpense(createExpenseModel);
            Assert.IsFalse(validationModel2.Succeeded, "Expense blocked");
        }

        [TestMethod]
        public void AddExpenseFromTheFuture()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = new DateTime(2024,01,12), ///////////
                Value = 153.00,
                Currency = "USD",
                Comment = "Berlinstrasse"
            };

            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);

            Assert.IsFalse(validationModel.Succeeded, "Expense blocked");

        }

        [TestMethod]
        public void AddExpenseOlderThan3Months()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = DateTime.Now.AddMonths(-3).AddMinutes(-1), ///////////
                Value = 153.00,
                Currency = "USD",
                Comment = "Berlinstrasse"
            };

            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);

            Assert.IsFalse(validationModel.Succeeded, "Expense blocked");

        }

        [TestMethod]
        public void AddExpenseWithoutComment()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = DateTime.Now.AddMonths(-3).AddMinutes(-1), ///////////
                Value = 153.00,
                Currency = "USD"
            };


            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);

            Assert.IsFalse(validationModel.Succeeded, "Expense blocked");

        }

        [TestMethod]
        public void AddExpenseWithDifferentCurrencyThanUser()
        {

            SeedDatabase();

            var createExpenseModel = new CreateExpenseModel()
            {
                UserId = 1,
                ExpenseType = "Hotel",
                Date = DateTime.Now.AddMonths(-3).AddMinutes(-1), ///////////
                Value = 153.00,
                Currency = "EUR",
                Comment = "Berlinstrasse"
            };


            // Working Create Expense 
            var validationModel = CreateExpense(createExpenseModel);

            Assert.IsFalse(validationModel.Succeeded, "Expense blocked");

        }

        [TestMethod]
        public void ListUserExpenses()
        {
            GetUserExpensesModel model = new GetUserExpensesModel()
            {
                UserId=1,
                DateFilter = false,
                DateDesc = false,
                ValueFilter = false,
                ValueDesc = false
            };

            SeedDatabase();
            FeedExpensesTable();

            var listResponses = SortData(model);

            Assert.IsTrue(listResponses.Count == 6);

            foreach (var response in listResponses)
            {
                Assert.IsTrue(response.User == "Anthony Stark");         
            }



        }

        [TestMethod]
        public void ListUserExpensesSortByValue()
        {

            GetUserExpensesModel model = new GetUserExpensesModel()
            {
                UserId = 1,
                DateFilter = false,
                DateDesc = false,
                ValueFilter = true,
                ValueDesc = false
            };

            SeedDatabase();
            FeedExpensesTable();

            var listResponses = SortData(model);

            Assert.IsTrue(listResponses.Count == 6);

            var listValue = new List<double>();

            var desiredValue = new List<double>();
            desiredValue.Add(18);
            desiredValue.Add(23);
            desiredValue.Add(25.90);
            desiredValue.Add(53);
            desiredValue.Add(63);
            desiredValue.Add(70.20);

            foreach (var response in listResponses)
            {
                Assert.IsTrue(response.User == "Anthony Stark");
                listValue.Add(response.Value);
            }

            Assert.AreEqual(listValue.Count, desiredValue.Count);
            Assert.IsTrue(desiredValue.SequenceEqual(listValue));

        }

        [TestMethod]
        public void ListUserExpensesSortByDate()
        {

            GetUserExpensesModel model = new GetUserExpensesModel()
            {
                UserId = 1,
                DateFilter = true,
                DateDesc = false,
                ValueFilter = false,
                ValueDesc = false
            };

            SeedDatabase();
            FeedExpensesTable();

            var listResponses = SortData(model);

            Assert.IsTrue(listResponses.Count == 6);

            var listDate = new List<string>();

            var desiredDate = new List<string>();
            desiredDate.Add(new DateTime(2022, 01, 01).ToString("dd/MM/yyyy"));
            desiredDate.Add(new DateTime(2022, 01, 02).ToString("dd/MM/yyyy"));
            desiredDate.Add(new DateTime(2022, 01, 05).ToString("dd/MM/yyyy"));
            desiredDate.Add(new DateTime(2022, 01, 06).ToString("dd/MM/yyyy"));
            desiredDate.Add(new DateTime(2022, 01, 12).ToString("dd/MM/yyyy"));
            desiredDate.Add(new DateTime(2022, 01, 12).ToString("dd/MM/yyyy"));

            foreach (var response in listResponses)
            {
                Assert.IsTrue(response.User == "Anthony Stark");
                listDate.Add(response.Date);
            }

            Assert.AreEqual(listDate.Count, desiredDate.Count);
            Assert.IsTrue(desiredDate.SequenceEqual(listDate));

        }

        [TestMethod]
        public void ListUserExpensesSortByDateAndValue()
        {

            GetUserExpensesModel model = new GetUserExpensesModel()
            {
                UserId = 1,
                DateFilter = true,
                DateDesc = false,
                ValueFilter = true,
                ValueDesc = false
            };

            SeedDatabase();
            FeedExpensesTable();

            var listResponses = SortData(model);

            Assert.IsTrue(listResponses.Count == 6);

            var listExpenses = new List<(double,string)>();

            var desiredExpenses = new List<(double, string)>
            {
                (18,new DateTime(2022, 01, 01).ToString("dd/MM/yyyy")),
                (63,new DateTime(2022, 01, 02).ToString("dd/MM/yyyy")),
                (53,new DateTime(2022, 01, 05).ToString("dd/MM/yyyy")),
                (70.20,new DateTime(2022, 01, 06).ToString("dd/MM/yyyy")),
                (23,new DateTime(2022, 01, 12).ToString("dd/MM/yyyy")),
                (25.90,new DateTime(2022, 01, 12).ToString("dd/MM/yyyy"))
            };

            foreach (var response in listResponses)
            {
                Assert.IsTrue(response.User == "Anthony Stark");
                listExpenses.Add((response.Value, response.Date));
            }

            Assert.AreEqual(listExpenses.Count, desiredExpenses.Count);
            Assert.IsTrue(desiredExpenses.SequenceEqual(listExpenses));

        }
    }
}
