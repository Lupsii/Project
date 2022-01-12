using Lucca.Common.Enums;
using Lucca.DAL.SQL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.DAL.SQL
{
    public class ExpenseContext : DbContext
    {
        public string DbPath { get; }
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Expense>()
                .ToTable("Expenses");

            modelBuilder.Entity<Expense>()
                .Property(c => c.ExpenseType)
                .HasConversion<string>();

            modelBuilder.Entity<Expense>()
                .Property(c => c.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .ToTable("Users");
             
            modelBuilder.Entity<User>()
                .Property(c => c.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FirstName = "Anthony", LastName = "Stark", Currency= Currency.USD },
                new User { UserId = 2, FirstName = "Natasha", LastName = "Romanova", Currency = Currency.RUB }
            );
        }


    }
}
