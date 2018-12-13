﻿using CapaEntidad.Util;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CapaPresentacion
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            #region<CONFIGURACION DE BASE DE DATOS CONFIG>

            Ent_Conexion.conexion = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PE"].ConnectionString); //ConfigurationManager.ConnectionStrings["SQL_PE"].ConnectionString;
            Ent_Conexion.conexionPosPeru = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PE"].ConnectionString); //ConfigurationManager.ConnectionStrings["SQL_PE"].ConnectionString;
            #endregion

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Control/Login"),
            });
        }
    }
}