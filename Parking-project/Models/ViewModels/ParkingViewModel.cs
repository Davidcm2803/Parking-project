﻿using System;

namespace Parking_project.Models.ViewModels
{
    public class ParkingViewModel
    {
        public int ParkingLotId { get; set; }
        public string LicensePlate { get; set; }
        public string Action { get; set; }
        public int VehicleId { get; set; }
    }
}
