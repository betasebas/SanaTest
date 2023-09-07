using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanaTest.Domain.Models
{
    public class Customer : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CellPhone { get; set; }

        public string Mail { get; set; }

    }
}