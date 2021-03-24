using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DayGetIn { get; set; }

        public double SalScale1 { get; set; }
    }
}
