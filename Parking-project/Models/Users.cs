//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Parking_project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public Users()
        {
            this.ParkingAssignments = new HashSet<ParkingAssignments>();
            this.Vehicles = new HashSet<Vehicles>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public string identification { get; set; }
        public int role_id { get; set; }
        public string password { get; set; }
        public string first_login { get; set; }
        public string password_changed { get; set; }
    
        public virtual ICollection<ParkingAssignments> ParkingAssignments { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }
    }
}
