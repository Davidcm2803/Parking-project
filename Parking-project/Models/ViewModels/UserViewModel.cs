using System;
using System.Collections.Generic;

namespace Parking_project.Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public List<Roles> Roles { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}