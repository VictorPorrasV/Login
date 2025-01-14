﻿using Login.Models;
using Login.Repositorio.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Login.Controllers
{
    public class InicioController : Controller
    {

        private readonly IUsuarioService _usuarioServicio;

        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }



        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM modelo)
        {
            modelo.Clave =  Repositorio.Utilidades.Encriptar.EncriptarClave(modelo.Clave);

            var usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if (usuario_creado.IdUsuario > 0)
                return RedirectToAction("IniciarSesion", "Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {

            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(correo, Repositorio.Utilidades.Encriptar.EncriptarClave(clave));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto),
                new Claim(ClaimTypes.Email,usuario_encontrado.Correo),
                new Claim("AdminType",usuario_encontrado.IdRol.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
