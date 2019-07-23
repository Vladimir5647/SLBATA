﻿using CapaDato.Inventario;
using CapaEntidad.General;
using CapaEntidad.Inventario;
using CapaPresentacion.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class InventarioController : Controller
    {
        private Dat_Inventario_Consulta datInv = new Dat_Inventario_Consulta(); //gft
        private string _session_tabla_inventCons_private = "_session_tabla_inventCons_private"; //gft
        private string _Inventario_Tienda_Combo = "_Inventario_Tienda_Combo"; 
        private string _Inventario_TiendaFecha_Combo = "_Inventario_TiendaFecha_Combo";

        // GET: Inventario
        //Index
        public ActionResult Consulta_Inv( )
        {
            string cod_entid="";

            //Combo Tienda
            if (Session["_Inventario_Tienda_Combo"] == null)
            {
                ViewBag.Tienda = datInv.get_ListaTienda();
               // ((datInv.get_ListaTienda()).Items[0]).cod_entid
                //List<Ent_Inventario_Tienda> list1 = new List<Ent_Inventario_Tienda>()
                //{
                //    new Ent_Inventario_Tienda(){  cod_entid = "-1", des_entid="Seleccione Tienda" },
                //};
                Session["_Inventario_Tienda_Combo"] = ViewBag.Tienda;
                cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
                ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
            }
            else
            { ViewBag.Tienda = Session["_Inventario_Tienda_Combo"];
                cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
                ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
            }

            ////Combo Fechas según selección de tienda
            //if (Session["_Inventario_TiendaFecha_Combo"] == null)
            //{
            //    //List<Ent_Inventario_Fecha> listc= List;
            //    // listc.Insert(0, new SelectListItem { Text = "--Select Customer--", Value = "" });
            //  //  cod_entid = (ViewBag.Tienda).Items[0].cod_entid;
            //    // List<Ent_Inventario_Tienda> listi =(ViewBag.Tienda).Items[0].cod_entid;
            //    //cod_entid = listi.ToString();
            //    //----------------------------------------------
            //    cod_entid = ViewBag.Tienda[0].cod_entid.ToString();
            //   ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
            //    /*------------------------*/
            //    //List<Ent_Inventario_Fecha> list1 = new List<Ent_Inventario_Fecha>()
            //    //{
            //    //    new Ent_Inventario_Fecha(){ xst_inv_fec_aud = "Seleccione tienda" },
            //    //};
            //    //ViewBag.Fecha = list1;
            //    /*------------------------*/
            //    // ViewBag.Fecha = datInv.get_ListaFecha( cod_entid);
            //    // Session["_Inventario_TiendaFecha_Combo"] = ViewBag.Fecha;
            //}
            //else
            //{ //ViewBag.Fecha = Session["_Inventario_TiendaFecha_Combo"];
            //    ViewBag.Fecha = datInv.get_ListaFecha(cod_entid);
            //    Session["_Inventario_TiendaFecha_Combo"] = ViewBag.Fecha;
            //}

            return View();
        }

        //Table
        public PartialViewResult _TableConsInv( string articulo, string talla, string dwtda, string dwfec)
        {
            //xst_inv_fec_aud
            if (dwtda == null|| dwfec == null)
            { return PartialView(); }
            else
            {  return PartialView(listaTablaConsulta(articulo, talla, dwtda, dwfec)); }
        }

        public List<Ent_Inventario_Consulta> listaTablaConsulta(string articulo, string talla, string dwtda, string dwfec)
        {
            List<Ent_Inventario_Consulta> list = datInv.get_ListaInv_Consulta( dwtda, dwfec, articulo, talla);
            Session[_session_tabla_inventCons_private] = list;
            return list;
        }

        public ActionResult getTableInventAjax(Ent_jQueryDataTableParams param)
        {
            /*verificar si esta null*/
            if (Session[_session_tabla_inventCons_private] == null)
            {
                List<Ent_Inventario_Consulta> listdoc = new List<Ent_Inventario_Consulta>();
                Session[_session_tabla_inventCons_private] = listdoc;
            }

            //Traer registros
            IQueryable<Ent_Inventario_Consulta> membercol = ((List<Ent_Inventario_Consulta>)(Session[_session_tabla_inventCons_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();

            IEnumerable<Ent_Inventario_Consulta> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m =>
                    m.articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.calidad.ToUpper().Contains(param.sSearch.ToUpper()) ||
                  //  m.des_entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.diferencia.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.talla.ToUpper().Contains(param.sSearch.ToUpper()) ||
                  //  m.fecha_inv.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.teorico.ToUpper().Contains(param.sSearch.ToUpper()) ||
                    m.fisico.ToUpper().Contains(param.sSearch.ToUpper()));
            }

            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);

            Func<Ent_Inventario_Consulta, string> orderingFunction =
                   (
                      //m => sortIdx == 0 ? m.fec_doc :
                      // m.fec_doc
                      m => sortIdx == 0 ? m.fecha_inv :
                    sortIdx == 1 ? m.articulo :
                    sortIdx == 2 ? m.talla :
                    sortIdx == 3 ? m.fisico :
                    m.diferencia
                   );

            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredMembers = filteredMembers.OrderBy(orderingFunction);
            else
                filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from a in displayMembers
                         select new
                         {
                             a.articulo,
                             a.calidad,
                             a.des_entid,
                             a.diferencia,
                             a.fecha_inv,
                             a.fisico,
                             a.teorico,
                             a.talla
                         };

           //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getDropdrowlistFecha(Ent_jQueryDataTableParams param, string valor_tienda)
        {

            ViewBag.Fecha = datInv.get_ListaFecha(valor_tienda);

            return Json(new[] {
                ViewBag.Fecha
            }, JsonRequestBehavior.AllowGet);
        }

        //Exportar Excel
        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            List<Ent_Inventario_Consulta> list = (List<Ent_Inventario_Consulta>)Session[_session_tabla_inventCons_private];
            string[] columns = { "des_entid", "articulo", "calidad", "talla", "teorico", "fisico", "diferencia", "nombres_venta", "fecha_inv"};
            byte[] filecontent = ExcelExportHelper.ExportExcel(list, "Inventario_Consulta", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Inventario_Consulta.xlsx");
        }

    }
}