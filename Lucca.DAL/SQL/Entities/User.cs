using Lucca.Common.Enums;
using Lucca.Common.Objects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucca.DAL.SQL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Currency Currency { get; set; }

        public UserDTO ToDto()
        {
            var dto = new UserDTO()
            {
                UserId = this.UserId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Currency = this.Currency
            };

            return dto;
        }
    }
}
