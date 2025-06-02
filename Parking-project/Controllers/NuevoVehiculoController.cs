using Parking_project.Models;
using Parking_project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parking_project.Controllers
{
    public class NuevoVehiculoController : Controller
    {
        private readonly ParkingDatabaseEntities _db = new ParkingDatabaseEntities();



        // Acción GET para mostrar el formulario de creación
        public ActionResult Index()
        {
            // Verificamos el dominio del correo del usuario logueado
            /*var admin = (Users)Session["AdminLogged"];
            if (admin != null && admin.email.EndsWith("@guarda.com"))
            {
                // Si el usuario es de @guarda.com, lo redirigimos a una vista diferente
                return RedirectToAction("Index", "AdminInicio");
            }*/

            // Obtener los usuarios para la vista
            var usuarios = _db.Users.Select(u => new UserViewModel
            {
                Id = u.id,
                Name = u.name
            }).ToList();

            var viewModel = new VehicleViewModel
            {
                Usuarios = usuarios
            };

            return View(viewModel);
        }

        // Acción POST para crear un nuevo vehículo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleViewModel newVehicle)
        {
            // Verificamos el dominio del correo del usuario logueado
            var admin = (Users)Session["AdminLogged"];
            if (admin != null && admin.email.EndsWith("@guarda.com"))
            {
                // Si el usuario es de @guarda.com, no permitimos agregar el vehículo
                TempData["ErrorMessage"] = "No tienes permisos para agregar vehículos.";
                return RedirectToAction("Index", "AdminInicio");
            }

            if (ModelState.IsValid)
            {
                // Validar que el usuario no tenga más de 2 vehículos registrados
                int vehiclesCount = _db.Vehicles.Count(v => v.owner_id == newVehicle.OwnerId);

                if (vehiclesCount >= 2)
                {
                    // Si tiene 2 o más vehículos, mostrar mensaje de error
                    TempData["ErrorMessage"] = "Este usuario ya tiene 2 vehículos registrados y no puede agregar más.";
                    return RedirectToAction("Index");  // Redirigir a la vista de creación
                }

                // Validación para asegurar que la placa no se repita (excepto para diferentes tipos de vehículos)
                var existingVehicle = _db.Vehicles
                    .FirstOrDefault(v => v.license_plate == newVehicle.LicensePlate && v.vehicle_type == newVehicle.VehicleType);

                if (existingVehicle != null)
                {
                    TempData["ErrorMessage"] = "Ya existe un vehículo con esta placa y tipo de vehículo.";
                    return RedirectToAction("Index");  // Redirigir a la vista de creación
                }

                // Asignar si se utiliza un espacio especial
                string usesSpecialSpaceValue = newVehicle.UsesSpecialSpace ? "Y" : "N";

                // Crear un nuevo vehículo
                var vehicle = new Vehicles
                {
                    brand = newVehicle.Brand,
                    color = newVehicle.Color,
                    license_plate = newVehicle.LicensePlate,
                    vehicle_type = newVehicle.VehicleType,
                    owner_id = newVehicle.OwnerId,
                    uses_special_space = newVehicle.UsesSpecialSpace,
                    is_active = "1",
                    is_parked = false
                };

                // Agregar el vehículo a la base de datos
                _db.Vehicles.Add(vehicle);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Vehículo registrado exitosamente.";
                return RedirectToAction("Index");  // Redirigir a la vista principal
            }

            // Si el modelo no es válido, recargar los usuarios y volver a la vista
            var usuarios = _db.Users.Select(u => new UserViewModel
            {
                Id = u.id,
                Name = u.name
            }).ToList();

            newVehicle.Usuarios = usuarios;
            return View("Index", newVehicle);
        }
    }
}