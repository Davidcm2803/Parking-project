using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parking_project.Models.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        [Column("license_plate")]
        public string LicensePlate { get; set; }

        public string VehicleType { get; set; }
        public int OwnerId { get; set; }
        public bool UsesSpecialSpace { get; set; }
        public bool IsParked { get; set; }
        public int ParkingLotId { get; set; } // Relación con el parqueo
        public List<UserViewModel> Usuarios { get; set; }
    }
}
