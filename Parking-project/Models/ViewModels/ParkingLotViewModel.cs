using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parking_project.Models.ViewModels
{
    public class ParkingLotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegularCapacity { get; set; }
        public int MotorcycleCapacity { get; set; }
        public int SpecialCapacity { get; set; }

        // Propiedades calculadas
        public int RegularOccupied { get; set; }
        public int MotorcycleOccupied { get; set; }
        public int SpecialOccupied { get; set; }
    }

}
