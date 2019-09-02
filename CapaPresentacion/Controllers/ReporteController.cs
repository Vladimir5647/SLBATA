﻿using CapaPresentacion.Bll;
using CapaDato.Reporte;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using CapaEntidad.Util;
using System.Linq;
using ClosedXML;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using CapaEntidad.Control;
using Data.Crystal.Reporte;
using Models.Crystal.Reporte;
using CapaEntidad.General;
using CapaDato.ReportsValeCompra;
using CapaDato.Maestros;
using CapaEntidad.Maestros;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using CapaEntidad.ValeCompra;

namespace CapaPresentacion.Controllers
{
    public class ReporteController : Controller
    {
       
        public string reportServerUrl = Ent_Conexion.servidorReporte;
        public ReportCredentials reportCredential = new ReportCredentials(Ent_Conexion.usuarioReporte, Ent_Conexion.passwordReporte, Ent_Conexion.dominioReporte);
        public string reportFolder = Ent_Conexion.CarpetaPlanillaReporte;
        private string gcodTda = "";
        private DataTable dtPromociones = new DataTable();
        private string _session_listcomparativo_private = "_session_listcomparativo_private";
        private string _session_listguia_private = "_session_listguia_private";
        private Dat_Combo datCbo = new Dat_Combo();
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult ReportePlanilla()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                List<Ent_Combo> list = new List<Ent_Combo>();
                Ent_Combo entCombo = new Ent_Combo();
                entCombo.cbo_codigo = "0";
                entCombo.cbo_descripcion = "----Todos----";
                list.Add(entCombo);
                ViewBag.Categoria = list;

                list = new List<Ent_Combo>();
                 entCombo = new Ent_Combo();
                entCombo.cbo_codigo = "-1";
                entCombo.cbo_descripcion = "OPERACIONES";
                list.Add(entCombo);
                entCombo = new Ent_Combo();
                entCombo.cbo_codigo = "1";
                entCombo.cbo_descripcion = "AUDITORIA";
                list.Add(entCombo);
                ViewBag.TipoReporte = list;


                ViewBag.Title = "Reporte de Planilla";
       
                ViewBag.Tipo = datCbo.get_ListaTipoCategoria();              

                ViewBag.Estado = datCbo.get_ListaEstado();

                if (Session["Tienda"]!=null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
                }

                
                

                string strJson = "";
                JsonResult jRespuesta = null;
                var serializer = new JavaScriptSerializer();

                ViewBag.listCalidad = datCbo.get_ListaCalidad().Where(d => d.cbo_codigo != "0");

                strJson = datCbo.listarStr_ListaGrupoTipo();
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClGrupo = jRespuesta;
                
                strJson = datCbo.listarStr_ListaCategoria("");
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClCategoria = jRespuesta;                          

                strJson = datCbo.listarStr_ListaSubCategoria("");
                jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClSBCategoria = jRespuesta;

                return View();
            }

           

            //Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            //string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            //string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            //string return_view = actionName + "|" + controllerName;

            //if (_usuario == null)
            //{
            //    return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            //}
            //else
            //{
            //    List<Ent_Combo> list = new List<Ent_Combo>();
            //    Ent_Combo entCombo = new Ent_Combo();
            //    entCombo.cbo_codigo = "0";
            //    entCombo.cbo_descripcion = "----Todos----";
            //    list.Add(entCombo);
            //    ViewBag.Title = "Reporte de Planilla";
            //    ViewBag.Grupo = datCbo.get_ListaGrupo();
            //    ViewBag.Estado = datCbo.get_ListaEstado();
            //    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
            //    ViewBag.Categoria = list;

            //    return View();
            //}
        }
        //public ActionResult ReportePlanillaTdaView()
        //{

        //    Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
        //    string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
        //    string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
        //    string return_view = actionName + "|" + controllerName;

        //    if (_usuario == null)
        //    {
        //        return RedirectToAction("Login", "Control", new { returnUrl = return_view });
        //    }
        //    else
        //    {
        //        gcodTda = (String)Session["Tienda"];
        //        List<Ent_Combo> list = new List<Ent_Combo>();
        //        Ent_Combo entCombo = new Ent_Combo();
        //        entCombo.cbo_codigo = "0";
        //        entCombo.cbo_descripcion = "----Todos----";
        //        list.Add(entCombo);
        //        ViewBag.Title = "Reporte de Planilla";
        //        ViewBag.Grupo = datCbo.get_ListaGrupo();
        //        ViewBag.Estado = datCbo.get_ListaEstado();
        //        ViewBag.Categoria = list;
        //        return View();
        //    }

        //}
        //public JsonResult GenerarCombo(int Numsp, string Params)
        //{
        //    string strJson = "";
        //    JsonResult jRespuesta = null;
        //    var serializer = new JavaScriptSerializer();


        //    switch (Numsp)
        //    {
        //        case 1:
        //            strJson = datCbo.listarStr_ListaCategoria(Params);
        //            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
        //            break;
        //        case 2:
        //            strJson = datCbo.listarStr_ListaSubCategoria(Params);
        //            jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
        //            break;
        //        default:
        //            Console.WriteLine("Default case");
        //            break;
        //    }
        //    return jRespuesta;
        //}

        //public ActionResult ReportePlanilla(string tda, string grupo, string cate, string subcate, string estado)
        //{

        //    Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
        //    string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
        //    string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
        //    string return_view = actionName + "|" + controllerName;

        //    string gcodTda = (String)Session["Tienda"];
        //    if (gcodTda != "" && gcodTda != null)
        //    {
        //        tda = gcodTda;
        //    }

        //    if (_usuario == null)
        //    {
        //        return RedirectToAction("Login", "Control", new { returnUrl = return_view });
        //    }
        //    else
        //    {
        //        string Param_tienda = (tda == "") ? "" : tda;
        //        string Param_grupo = (grupo == "") ? "" : grupo;
        //        string Param_cate = (cate == "") ? "0" : cate;
        //        string Param_subcate = (subcate == "") ? "0" : subcate;
        //        string Param_estado = (estado == "") ? "0" : estado;

        //        gcodTda = (String)Session["Tienda"];

        //        if (gcodTda != "" && gcodTda != null)
        //        {
        //            Param_tienda = gcodTda;
        //        }
        //        //Param_tienda = "50143";
        //        List<ReportParameter> paramList = new List<ReportParameter>();
        //        paramList.Add(new ReportParameter("cod_tda", Param_tienda, false));
        //        paramList.Add(new ReportParameter("Grupo", Param_grupo, false));
        //        paramList.Add(new ReportParameter("Categoria", Param_cate, false));
        //        paramList.Add(new ReportParameter("SubCategoria", Param_subcate, false));
        //        paramList.Add(new ReportParameter("Estado", Param_estado, false));

        //        // ** Procesar Reporte en Servidor
        //        ReportViewer reportViewer = new ReportViewer();
        //        reportViewer.ProcessingMode = ProcessingMode.Remote;
        //        reportViewer.SizeToReportContent = true;
        //        reportViewer.ZoomMode = ZoomMode.FullPage;
        //        reportViewer.Width = Unit.Percentage(300);
        //        reportViewer.Height = Unit.Percentage(300);
        //        reportViewer.AsyncRendering = true;

        //        reportViewer.ShowPrintButton = true;

        //        reportViewer.ServerReport.ReportServerCredentials = reportCredential;
        //        reportViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);
        //        reportViewer.ServerReport.ReportPath = "/" + reportFolder + "/Rpt_PlanillaWeb";
        //        reportViewer.ShowParameterPrompts = false;
        //        reportViewer.ServerReport.SetParameters(paramList);
        //        reportViewer.ServerReport.Refresh();
        //        ViewBag.ReportViewer = reportViewer;
        //        ViewBag.Title = "Reporte de Planilla";

        //        return View();
        //    }

        //}
        public JsonResult GenerarCombo(int Numsp, string Params)
        {
            string strJson = "";
            JsonResult jRespuesta = null;
            var serializer = new JavaScriptSerializer();


            switch (Numsp)
            {
                case 1:
                    strJson = datCbo.listarStr_ListaCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                case 2:
                    strJson = datCbo.listarStr_ListaSubCategoria(Params);
                    jRespuesta = Json(serializer.Deserialize<List<Ent_Combo>>(strJson), JsonRequestBehavior.AllowGet);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return jRespuesta;
        }
        [HttpPost]
        public ActionResult ShowGenericReportInNewWin(string cod_tda, string grupo, string categoria, string subcategoria, string estado,string tipo,string tipoReport, string calidad)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
         //   tipoReport = "1";
            Data_Planilla pl = new Data_Planilla();
            string nombreReporte = tipoReport == "-1" ? "Planilla.rpt" : "Planilladet.rpt";
            this.HttpContext.Session["ReportName"] = nombreReporte;

            //string tipo_rep = "1";

            List<Models_Planilla> model_planilla= pl.get_planilla(cod_tda, grupo, categoria, subcategoria, estado, tipo, tipoReport, calidad);

            this.HttpContext.Session["rptSource"] = model_planilla;
            this.HttpContext.Session["rptSource_sub"] = pl.get_reglamed_cab();

            /*error=0;exito=1*/
            string _estado = (model_planilla==null)?"0":"1";

            //if (model_planilla==null)

            return Json(new
            {                
                estado = _estado
            });
        }
        public ActionResult ReporteObs()
        {
         
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Title = "Reporte Obselescensia";

                Dat_ArticuloStock distrito_list = new Dat_ArticuloStock();
                Dat_ListaTienda list_tda = new Dat_ListaTienda();

                List<Ent_Combo> listD = new List<Ent_Combo>();
                Ent_Combo entComboD = new Ent_Combo();
                entComboD.cbo_codigo = "0";
                entComboD.cbo_descripcion = "----Todos----";
                listD.Add(entComboD);
                ViewBag.Categoria = listD;

                if (Session["Tienda"] != null)
                {
                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(t => t.cod_entid == Session["Tienda"].ToString()), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                    ViewBag.tda = "0";


                    List<Ent_ListaTienda> listar_tda = serializer.Deserialize<List<Ent_ListaTienda>>(strJson);
                    var tda = listar_tda.Where(t => t.cod_entid == Session["Tienda"].ToString()).ToList();
                    ViewBag.Tienda = tda;



                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis == tda[0].cod_distri);
                }
                else
                {
                    ViewBag.tda = "1";
                    /*ViewBag.Tienda = list_tda.get_tienda("PE");*/ //datCbo.get_ListaTiendaXstoreActivo("");
                    List<Ent_ListaTienda> list = new List<Ent_ListaTienda>();
                    Ent_ListaTienda entCombo = new Ent_ListaTienda();
                    entCombo.cod_entid = "-1";
                    entCombo.des_entid = "----Todos----";
                    list.Add(entCombo);
                    ViewBag.Tienda = list;

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis != "-1");

                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(d => d.cod_entid != "0"), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;

                }

                ViewBag.Tipo = datCbo.get_ListaTipoCategoria();

                string strJson2 = "";
                JsonResult jRespuesta2 = null;
                var serializer2 = new JavaScriptSerializer();


                strJson2 = datCbo.listarStr_ListaGrupoTipo();

                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClGrupo = jRespuesta2;

                strJson2 = datCbo.listarStr_ListaCategoria("");
                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClCategoria = jRespuesta2;
                Ent_ComboList filtros = datCbo.Listar_Filtros_OBS();


                ViewBag.listCalidad  = filtros.Lista_1.Where(d => d.cbo_codigo != "-1");
                ViewBag.listTipoObs = filtros.Lista_2;
                ViewBag.lisRango = filtros.Lista_3;

                return View();
            }
        }
        public ActionResult ReporteVendedor()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {              

                ViewBag.Title = "Reporte Vendedor";

                Dat_ArticuloStock distrito_list = new Dat_ArticuloStock();
                Dat_ListaTienda list_tda = new Dat_ListaTienda();


                if (Session["Tienda"] != null)
                {
                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(t => t.cod_entid == Session["Tienda"].ToString()), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                    ViewBag.tda = "0";


                    List<Ent_ListaTienda> listar_tda = serializer.Deserialize<List<Ent_ListaTienda>>(strJson);
                    var tda = listar_tda.Where(t => t.cod_entid == Session["Tienda"].ToString()).ToList();
                    ViewBag.Tienda = tda;

                    

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis == tda[0].cod_distri);
                }
                else
                {
                    ViewBag.tda = "1";
                    /*ViewBag.Tienda = list_tda.get_tienda("PE");*/ //datCbo.get_ListaTiendaXstoreActivo("");
                    List<Ent_ListaTienda> list = new List<Ent_ListaTienda>();
                    Ent_ListaTienda entCombo = new Ent_ListaTienda();
                    entCombo.cod_entid= "-1";
                    entCombo.des_entid = "----Todos----";
                    list.Add(entCombo);
                    ViewBag.Tienda = list;

                    ViewBag.Distrito = distrito_list.listar_distrito();

                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(d => d.cod_entid != "0"), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;

                }

                ViewBag.listCalidad = datCbo.get_ListaCalidad().Where(d => d.cbo_codigo != "0");



                return View();
            }
           
        }
      

        [HttpPost]
        public ActionResult ShowGenericReportVendedorInNewWin(string coddis,string cod_tda, string fecIni, string FecFin, string calidad)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Planilla pl = new Data_Planilla();
            this.HttpContext.Session["ReportName"] = "Vendedor.rpt";

            List<Models_Vendedor> model_vendedor = pl.get_reporteVendedor(coddis, cod_tda, fecIni, FecFin, calidad);

            this.HttpContext.Session["rptSource"] = model_vendedor;
           

            /*error=0;exito=1*/
            string _estado = (model_vendedor == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }

        public ActionResult ReporteArticuloSinMov()
        {
            Dat_ArticuloStock distrito_list = new Dat_ArticuloStock();

            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            List<Ent_Combo> listD = new List<Ent_Combo>();
            Ent_Combo entComboD = new Ent_Combo();
            entComboD.cbo_codigo = "0";
            entComboD.cbo_descripcion = "----Todos----";
            listD.Add(entComboD);
            ViewBag.Categoria = listD;

            List<Ent_Combo> list = new List<Ent_Combo>();
            Ent_Combo entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "0";
            entCombo.cbo_descripcion = "----Todos----";
            list.Add(entCombo);

            entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "1";
            entCombo.cbo_descripcion = "    Con Venta   ";
            list.Add(entCombo);

            entCombo = new Ent_Combo();
            entCombo.cbo_codigo = "2";
            entCombo.cbo_descripcion = "    Sin venta   ";

            list.Add(entCombo);
            ViewBag.Estado = list;



            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Title = "Reporte Articulo sin movimiento";

                if (Session["Tienda"] != null)
                {
                    //ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo(Session["Tienda"].ToString());
                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(t => t.cod_entid == Session["Tienda"].ToString()), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                    ViewBag.tda = "0";


                    List<Ent_ListaTienda> listar_tda = serializer.Deserialize<List<Ent_ListaTienda>>(strJson);
                    var tda = listar_tda.Where(t => t.cod_entid == Session["Tienda"].ToString()).ToList();
                    ViewBag.Tienda = tda;

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis == tda[0].cod_distri);
                }
                else
                {
                    //ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo("");

                    ViewBag.tda = "1";
                    /*ViewBag.Tienda = list_tda.get_tienda("PE");*/ //datCbo.get_ListaTiendaXstoreActivo("");
                    List<Ent_ListaTienda> list_tda = new List<Ent_ListaTienda>();
                    Ent_ListaTienda entCombo_tda = new Ent_ListaTienda();
                    entCombo_tda.cod_entid = "-1";
                    entCombo_tda.des_entid = "----Todos----";
                    list_tda.Add(entCombo_tda);
                    ViewBag.Tienda = list_tda;

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis != "-1");

                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();


                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(d => d.cod_entid != "0"), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                }

                ViewBag.Tipo = datCbo.get_ListaTipoCategoria();

                string strJson2 = "";
                JsonResult jRespuesta2 = null;
                var serializer2 = new JavaScriptSerializer();


                strJson2 = datCbo.listarStr_ListaGrupoTipo();

                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClGrupo = jRespuesta2;

                strJson2 = datCbo.listarStr_ListaCategoria("");
                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2).Where(d => d.cbo_codigo != "0"), JsonRequestBehavior.AllowGet);
                ViewBag.ClCategoria = jRespuesta2;

                //ViewBag.Cadena = datCbo.get_ListaCadena();

                return View();
            }

        }

        [HttpPost]
        public ActionResult ShowGenericReportArtSinMovInNewWin(string cod_cadena,string cod_dis, string cod_tda, Int32 nsemana, Int32 maxpares, string estado, string grupo, string categoria, string tipo,string resumen)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Bata pl = new Data_Bata();
            this.HttpContext.Session["ReportName"] = "ReportArtSinMov.rpt";

            List<Models_Art_Sin_Mov> model_Art_sn_mov = pl.list_art_sin_mov(cod_cadena, cod_dis, cod_tda, nsemana, maxpares, estado, grupo, categoria, tipo);

            this.HttpContext.Session["rptSource"] = model_Art_sn_mov;
            this.HttpContext.Session["obs_resumen"] = resumen;

            /*error=0;exito=1*/
            string _estado = (model_Art_sn_mov == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }

        public ActionResult ReporteComparativoVenta()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Title = "Reporte Comparativo Venta";

                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo(Session["Tienda"].ToString());
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo("");
                }



                return View();
            }

        }


        public PartialViewResult ListaComparativo(string dwtienda, string fecini, string fecfinc, string fecini2, string fecfinc2, string idcomparativo)
        {
            if (idcomparativo == null) { idcomparativo = "0"; };
            return PartialView(lista(dwtienda, fecini, fecfinc, fecini2, fecfinc2, idcomparativo));
        }

        public List<Models_Comparativo_Venta> lista(string dwtienda, string fecini, string fecfinc, string fecini2, string fecfinc2, string idcomparativo)
        {
            Data_Bata pl = new Data_Bata();
         
            List<Models_Comparativo_Venta> model_vent_comp = pl.list_comparativo_venta(dwtienda, fecini, fecfinc, fecini2, fecfinc2, idcomparativo);

            Session[_session_listcomparativo_private] = model_vent_comp;
            return model_vent_comp;
        }

        public ActionResult getComparativo(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listcomparativo_private] == null)
            {
                List<Models_Comparativo_Venta> liscomp = new List<Models_Comparativo_Venta>();
                Session[_session_listcomparativo_private] = liscomp;
            }

            //Traer registros
            IQueryable<Models_Comparativo_Venta> membercol = ((List<Models_Comparativo_Venta>)(Session[_session_listcomparativo_private])).AsQueryable(); 

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Models_Comparativo_Venta> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.cod_entid.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.cod_entid.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Models_Comparativo_Venta, string> orderingFunction =
            (
            //m => sortIdx == 0 ? m.orden :
             m => m.orden
            );
            var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart);
              
            var result = from a in displayMembers
                         select new
                         {
                             a.des_entid,
                             a.rango,
                             a.pares,
                             a.ropa,
                             a.acc,
                             a.cant_total,
                             a.neto
                         } ;
            //Se devuelven los resultados por json
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCount,
                iTotalDisplayRecords = filteredMembers.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowGenericReportObsolescenciaInNewWin(string coddis, string cod_tda, string tipo_cat, string cod_linea, string cod_categ, string calidad, string precio1, string precio2, string tipoObs,string rangoObs , string resumen)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Bata da = new Data_Bata();
            this.HttpContext.Session["ReportName"] = "Vendedor.rpt";

            List<Models_Obs> model_obs = da.list_obs(coddis, cod_tda, tipo_cat, cod_linea, cod_categ, calidad, Convert.ToDecimal(precio1), Convert.ToDecimal(precio2), tipoObs, rangoObs );

            this.HttpContext.Session["data"] = model_obs;
            this.HttpContext.Session["obs_resumen"] = resumen;


            /*error=0;exito=1*/
            string _estado = (model_obs == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }

        public ActionResult ReporteConsultaGuia()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Title = "Reporte Guia por tienda";

                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo(Session["Tienda"].ToString());
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstoreActivo("");
                }

                ViewBag.Tipo = datCbo.get_ListaTipoCategoria();

                string strJson2 = "";
                JsonResult jRespuesta2 = null;
                var serializer2 = new JavaScriptSerializer();


                strJson2 = datCbo.listarStr_ListaGrupoTipo();

                ViewBag.listCalidad = datCbo.get_ListaCalidad();

                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2), JsonRequestBehavior.AllowGet);
                ViewBag.ClGrupo = jRespuesta2;

                strJson2 = datCbo.listarStr_ListaCategoria("");
                jRespuesta2 = Json(serializer2.Deserialize<List<Ent_Combo>>(strJson2), JsonRequestBehavior.AllowGet);
                ViewBag.ClCategoria = jRespuesta2;

                List<Ent_Combo> listD = new List<Ent_Combo>();
                Ent_Combo entComboD = new Ent_Combo();
                entComboD.cbo_codigo = "0";
                entComboD.cbo_descripcion = "----Todos----";
                listD.Add(entComboD);
                ViewBag.Categoria = listD;

                return View();
            }

        }

        public PartialViewResult ListaGuiaTienda(string dwtienda, string dwTipo, string dwGrupo, string dwCate, string txtarticulo, string dwCalidad)
        {
            dwTipo= dwTipo =="01"? "S" : "R";         
            txtarticulo = txtarticulo.Trim();
            dwGrupo = dwGrupo == "0" ? "-1" : dwGrupo;
            dwCate = dwCate == "0" ? "-1" : dwCate;
            txtarticulo = txtarticulo == "" ? "-1" : txtarticulo;
            dwCalidad = dwCalidad==null ? "0" : dwCalidad;

            Models_GuiaConten model_vent_comp = listaGuia(dwtienda, dwTipo, dwGrupo, dwCate, txtarticulo, dwCalidad);

          ViewBag.GuiaDetalle = model_vent_comp.strDetalle;
          Session[_session_listguia_private] = model_vent_comp.listGuia;

            return PartialView(model_vent_comp.listGuia);
        }

        public Models_GuiaConten listaGuia(string dwtienda, string tipo_cat, string cod_linea, string cod_categ, string articulo, string calidad)
        {
            Data_Bata pl = new Data_Bata();

            Models_GuiaConten model_vent_comp = pl.list_Guia_Tienda(dwtienda, tipo_cat, cod_linea, cod_categ, articulo, calidad);

            return model_vent_comp;
        }

        public ActionResult getGuias(Ent_jQueryDataTableParams param)
        {

            /*verificar si esta null*/
            if (Session[_session_listguia_private] == null)
            {
                List<Models_Guia> lisguia = new List<Models_Guia>();
                Session[_session_listguia_private] = lisguia;
            }

            //Traer registros
            IQueryable<Models_Guia> membercol = ((List<Models_Guia>)(Session[_session_listguia_private])).AsQueryable();

            //Manejador de filtros
            int totalCount = membercol.Count();
            IEnumerable<Models_Guia> filteredMembers = membercol;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredMembers = membercol
                    .Where(m => m.NUMERO.ToUpper().Contains(param.sSearch.ToUpper()) ||
                     m.NUMERO.ToUpper().Contains(param.sSearch.ToUpper()));
            }
            //Manejador de orden
            var sortIdx = Convert.ToInt32(Request["iSortCol_0"]);
            Func<Models_Comparativo_Venta, string> orderingFunction =
            (
            //m => sortIdx == 0 ? m.orden :
             m => m.orden
            );
            var sortDirection = Request["sSortDir_0"];
            //if (sortDirection == "asc")
            //    filteredMembers = filteredMembers.OrderBy(orderingFunction);
            //else
            //    filteredMembers = filteredMembers.OrderByDescending(orderingFunction);
            var displayMembers = filteredMembers
                .Skip(param.iDisplayStart);

            var result = from a in displayMembers
                         select new
                         {
                             a.NUMERO,
                             a.FECHA,
                             a.PARES,
                             a.VCALZADO,
                             a.NOCALZADO,
                             a.VNOCALZADO,
                             a.ESTADO
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

        public ActionResult ReporteRendCateg()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            //Session["Tienda"] = "50143";

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                ViewBag.Title = "Reporte Rendiniento por Categoria";

                Dat_ArticuloStock distrito_list = new Dat_ArticuloStock();
                Dat_ListaTienda list_tda = new Dat_ListaTienda();

                if (Session["Tienda"] != null)
                {
                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();

                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(t => t.cod_entid == Session["Tienda"].ToString()), JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                    ViewBag.tda = "0";

                    List<Ent_ListaTienda> listar_tda = serializer.Deserialize<List<Ent_ListaTienda>>(strJson);
                    var tda = listar_tda.Where(t => t.cod_entid == Session["Tienda"].ToString()).ToList();
                    ViewBag.Tienda = tda;

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(d => d.cod_dis == tda[0].cod_distri);
                }
                else
                {
                    ViewBag.tda = "1";
               
                    List<Ent_ListaTienda> list = new List<Ent_ListaTienda>();
                    Ent_ListaTienda entCombo = new Ent_ListaTienda();
                    entCombo.cod_entid = "-1";
                    entCombo.des_entid = "----Todos----";
                    list.Add(entCombo);
                    ViewBag.Tienda = list;

                    ViewBag.Distrito = distrito_list.listar_distrito().Where(a => a.cod_dis != "-1");

                    string strJson = "";
                    JsonResult jRespuesta = null;
                    var serializer = new JavaScriptSerializer();

                    strJson = datCbo.listarStr_ListaTienda("PE");
                    jRespuesta = Json(serializer.Deserialize<List<Ent_ListaTienda>>(strJson).Where(a => a.cod_entid != "0" ) , JsonRequestBehavior.AllowGet);
                    ViewBag.ClTienda = jRespuesta;
                }

                List<Ent_Combo> listcbo = new List<Ent_Combo>();
                Ent_Combo entCombocbo = new Ent_Combo();
                entCombocbo.cbo_codigo = "S";
                entCombocbo.cbo_descripcion = "STANDAR";
                listcbo.Add(entCombocbo);
                entCombocbo = new Ent_Combo();
                entCombocbo.cbo_codigo = "R";
                entCombocbo.cbo_descripcion = "RIMS";
                listcbo.Add(entCombocbo);
                ViewBag.Tipo = listcbo;
                entCombocbo = new Ent_Combo();
                entCombocbo.cbo_codigo = "01";
                entCombocbo.cbo_descripcion = "SEMESTRAL";
                List<Ent_Combo> listEvaluacion = new List<Ent_Combo>();
                listEvaluacion.Add(entCombocbo);
                entCombocbo = new Ent_Combo();
                entCombocbo.cbo_codigo = "02";
                entCombocbo.cbo_descripcion = "ANUAL";
                listEvaluacion.Add(entCombocbo);

                ViewBag.Semana = datCbo.get_ListaSemana();
                ViewBag.Evaluacion = listEvaluacion;

                return View();
            }

        }


        [HttpPost]
        public ActionResult ShowGenericReportRendimientoCateg(string tip_Categ, string cod_Dis, string codEntid, string cod_Semana , string evalua)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Bata pl = new Data_Bata();
            this.HttpContext.Session["ReportName"] = "ReporteRendCateg.rpt";

            List<Models_Rendimiento_Categ> model_Art_rend_categ = pl.list_RendimientoxCategoria(tip_Categ, cod_Dis, codEntid, cod_Semana, evalua);

            this.HttpContext.Session["rptSource"] = model_Art_rend_categ;


            /*error=0;exito=1*/
            string _estado = (model_Art_rend_categ == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }

        public ActionResult ReportePromociones()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;

            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {

                ViewBag.Title = "Reporte Promociones";
                List<Ent_ListaTienda> list = new List<Ent_ListaTienda>();
                Ent_ListaTienda entCombo = new Ent_ListaTienda();
                entCombo.cod_entid = "BA";
                entCombo.des_entid = "BA";
                list.Add(entCombo);
                entCombo = new Ent_ListaTienda();
                entCombo.cod_entid = "BG";
                entCombo.des_entid = "BG";
                list.Add(entCombo);
                entCombo = new Ent_ListaTienda();
                entCombo.cod_entid = "WB";
                entCombo.des_entid = "WB";
                list.Add(entCombo);

                ViewBag.Cadena = list;

                /*filtro de busqueda*/
                Dat_Prom_Filtro prom_filtro = new Dat_Prom_Filtro();

                ViewBag.Filtro = prom_filtro.lista_prom_fltro();


                return View();
            }

        }

        public ActionResult ExportDataPromociones(string cod_cadena, string fecIni, string FecFin,string filtro)
        {
            var oJRespuesta = new JsonResponse();
            DataTable dt = new DataTable();
            Data_Planilla pl = new Data_Planilla();
            dt = pl.get_reportePromociones(cod_cadena, fecIni, FecFin,filtro);
            dt.TableName = "Promociones";
            Session["Promociones"] = dt;
       
            oJRespuesta.Success = true;
            oJRespuesta.Message = "1";

            return Json(oJRespuesta, JsonRequestBehavior.AllowGet);
        }
    
        public FileResult DescargarPromociones()
        {
            DataTable dt = new DataTable();
           

            var oJRespuesta = new JsonResponse();
            dt = Session["Promociones"] as DataTable;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Promociones.xlsx");
                }
            }
        }
        public ActionResult ReporteTablaProsp()
        {
            Ent_Usuario _usuario = (Ent_Usuario)Session[Ent_Constantes.NameSessionUser];
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            string controllerName = this.ControllerContext.RouteData.GetRequiredString("controller");
            string return_view = actionName + "|" + controllerName;
            if (_usuario == null)
            {
                return RedirectToAction("Login", "Control", new { returnUrl = return_view });
            }
            else
            {
                if (Session["Tienda"] != null)
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore().Where(t => t.cbo_codigo == Session["Tienda"].ToString()).ToList();
                }
                else
                {
                    ViewBag.Tienda = datCbo.get_ListaTiendaXstore();
                    
                }
                ViewBag.anios = datCbo.get_lista_anios(2015);
            }
            return View();
        }
        [HttpPost]
        public ActionResult ShowGenericReportTablaPropInNewWin(string tienda , string anio)
        {
            //grupo = "0";categoria = "0";subcategoria = "0";estado = "0";
            Data_Bata pl = new Data_Bata();
            this.HttpContext.Session["ReportName"] = "TablaProsperidad.rpt";

            List<CapaPresentacion.Models.Crystal.Reporte.Models_Tab_Pros> model_Art_sn_mov = pl.list_tab_pros(tienda , anio);

            this.HttpContext.Session["rptSource"] = model_Art_sn_mov;


            /*error=0;exito=1*/
            string _estado = (model_Art_sn_mov == null) ? "0" : "1";

            //if (model_planilla==null)

            return Json(new
            {
                estado = _estado
            });
        }
    }
}