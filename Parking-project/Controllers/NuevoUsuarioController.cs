using Parking_project.Models;
using Parking_project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions; // ← Necesario para transacciones en EF5

namespace Parking_project.Controllers
{
    public class NuevoUsuarioController : Controller
    {
        private readonly ParkingDatabaseEntities _db = new ParkingDatabaseEntities();

        // GET: NuevoUsuario
        [HttpGet]
        public ActionResult Index()
        {
            /*var admin = (Users)Session["AdminLogged"];
            if (admin != null)
            {
                if (admin.email.EndsWith("@guarda.com"))
                {
                    return RedirectToAction("index", "AdminInicio");
                }
            }
            else
            {
                return RedirectToAction("index", "AdminInicio");
            }*/

            // Aquí solo traemos la lista completa de Roles
            var rolesList = _db.Roles.ToList();

            var model = new UserViewModel
            {
                Roles = rolesList
            };

            return View(model);
        }


        // POST: Agregar Usuario
        [HttpPost]
        public JsonResult AgregarUsuario(string nombre, string cedula, string email, DateTime fechaNacimiento, string password, int rol)
        {
            try
            {
                var rolEntity = _db.Roles.FirstOrDefault(r => r.id == rol);
                if (rolEntity == null)
                {
                    return Json(new { success = false, message = "El rol seleccionado no es válido." });
                }

                var usuarioExistente = _db.Users.FirstOrDefault(u => u.email == email);
                if (usuarioExistente != null)
                {
                    return Json(new { success = false, message = "El correo electrónico ya está registrado." });
                }

                var usuarioExistentePorCedula = _db.Users.FirstOrDefault(u => u.identification == cedula);
                if (usuarioExistentePorCedula != null)
                {
                    return Json(new { success = false, message = "La identificación ya está registrada." });
                }

                var nuevoUsuario = new Users
                {
                    name = nombre,
                    email = email,
                    date_of_birth = fechaNacimiento,
                    identification = cedula,
                    role_id = rol,
                    password = password,
                    first_login = "1",
                    password_changed = "0"
                };

                using (var scope = new TransactionScope())
                {
                    _db.Users.Add(nuevoUsuario);
                    _db.SaveChanges();

                    scope.Complete(); // Confirmar la transacción

                    return Json(new { success = true, message = "Usuario agregado exitosamente." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al agregar el usuario: {ex.Message}" });
            }
        }
    }
}

