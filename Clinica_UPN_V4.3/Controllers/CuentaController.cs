using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Clinica_UPN_V4._3.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Clinica_UPN_V4._3.Controllers
{
    public class CuentaController : Controller
    {
        private IConfiguration _config;
        public CuentaController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Consultorios");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Administrador u)
        {
            try
            {
                String connectionString = _config["ConnectionStrings:conexion"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new("sp_validar_Administrador", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@UsuarioAdmin", System.Data.SqlDbType.VarChar).Value = u.UsuarioAdmin;
                        cmd.Parameters.Add("@Contraseña", System.Data.SqlDbType.VarChar).Value = u.Contraseña;
                        con.Open();
                        var dr = cmd.ExecuteReader();
                        bool validUser = false;
                        while (dr.Read())
                        {
                            if (dr["UsuarioAdmin"] != null && u.UsuarioAdmin != null)
                            {
                                validUser = true;
                                List<Claim> c = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, u.UsuarioAdmin)
                                };
                                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                                AuthenticationProperties p = new();
                                p.AllowRefresh = true;
                                p.IsPersistent = u.MantenerActivo;
                                if (!u.MantenerActivo)
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30); // Cambia la sesión temporal a 30 minutos
                                else
                                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7); // Cambia la sesión persistente a 7 días
                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);

                                // Almacenar el nombre de usuario en TempData
                                TempData["UserName"] = u.UsuarioAdmin;

                                return RedirectToAction("Index", "Home"); // Redirige a la página principal
                            }
                        }
                        con.Close();
                        if (!validUser)
                        {
                            ViewBag.Error = "Credenciales incorrectas o cuenta no registrada.";
                        }
                    }
                    return View();
                }
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
