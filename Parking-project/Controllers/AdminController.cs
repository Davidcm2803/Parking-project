using Parking_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parking_project.Controllers
{
    public class AdminController : Controller
    {
        private ParkingDatabaseEntities db = new ParkingDatabaseEntities();

        // GET: Admin/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {
                // Buscar el usuario en la base de datos por su correo electrónico
                var admin = db.Users.FirstOrDefault(u => u.email == email);

                // Verificar si el usuario existe y la contraseña es correcta
                if (admin == null)
                {
                    ViewBag.ErrorMessage = "El correo electrónico no está registrado.";
                    return View();
                }

                // Verificar si la contraseña coincide
                if (admin.password != password)
                {
                    ViewBag.ErrorMessage = "La contraseña es incorrecta.";
                    return View();
                }

                // Redirigir según el dominio del correo
                if (admin.email.EndsWith("@admin.com") || admin.email.EndsWith("@guarda.com"))
                {
                    Session["AdminLogged"] = admin;
                    TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                    return RedirectToAction("Index", "AdminInicio");
                }
                else
                {
                    // Si es un usuario normal, redirigir a su inicio
                    Session["UserLogged"] = admin;
                    TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View();
            }
        }

        // Método para Logout
        public ActionResult Logout()
        {
            Session["AdminLogged"] = null;
            Session["UserLogged"] = null;
            return RedirectToAction("Login");
        }

        // GET: Admin/ChangePassword
        public ActionResult ChangePassword()
        {
            // Comprobar si el usuario está logueado y si es administrador
            var admin = (Users)Session["AdminLogged"];
            if (admin == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Admin/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            var admin = (Users)Session["AdminLogged"];
            if (admin == null)
            {
                return RedirectToAction("Login");
            }

            var user = db.Users.Find(admin.id);

            if (user == null || user.password != currentPassword)
            {
                ViewBag.ErrorMessage = "La contraseña actual es incorrecta.";
                return View();
            }

            // Actualizar la contraseña
            user.password = newPassword;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Contraseña actualizada exitosamente.";
            return RedirectToAction("Index", "AdminInicio");
        }

        // Asegurar que los usuarios con "guarda.com" no puedan acceder a ciertas vistas
        [Authorize]
        public ActionResult AdminView()
        {
            var admin = (Users)Session["AdminLogged"];
            if (admin == null || admin.email.EndsWith("@guarda.com"))
            {
                return RedirectToAction("Index", "User"); // Redirige a la vista del usuario normal
            }

            return View();
        }

        // Redirigir a la vista de User para cualquier acción de Admin que no se deba ver
        [Authorize]
        public ActionResult AdminRestrictedAction()
        {
            var admin = (Users)Session["AdminLogged"];
            if (admin == null || admin.email.EndsWith("@guarda.com"))
            {
                return RedirectToAction("Index", "User");
            }

            return View();
        }
    }
}
