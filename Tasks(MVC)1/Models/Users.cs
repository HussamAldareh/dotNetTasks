using Microsoft.AspNetCore.Identity;

namespace Tasks_MVC_1.Models
{
     

      using Microsoft.AspNetCore.Identity;

public class Users : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? NationalId { get; set; }
        public string? Nationality { get; set; }
        public string? MaritalStatus { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? EntryDateToCompany { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }


};

