﻿using CapaEntidad.Util;
using Models.Crystal.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Data.Crystal.Reporte
{
    public class Data_Planilla
    {
        public List<Models_ReglaCab> get_reglamed_cab()
        {
            string sqlquery = "[USP_Planilla_ReglaCab]";
            List<Models_ReglaCab> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;                          
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_ReglaCab>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_ReglaCab()
                                         {
                                             med = dr["rmed"].ToString(),
                                             _00 = dr["00"].ToString(),
                                             _01 = dr["01"].ToString(),
                                             _02 = dr["02"].ToString(),
                                             _03 = dr["03"].ToString(),
                                             _04 = dr["04"].ToString(),
                                             _05 = dr["05"].ToString(),
                                             _06 = dr["06"].ToString(),
                                             _07 = dr["07"].ToString(),
                                             _08 = dr["08"].ToString(),
                                             _09 = dr["09"].ToString(),
                                             _10 = dr["10"].ToString(),
                                             _11 = dr["11"].ToString(),
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }
    
        public List<Models_Planilla> get_planilla(string cod_tda, string grupo, string categoria, string subcategoria, string estado, string tipo)
        {
            string sqlquery = "[USP_ReportePlanilla]";
            List<Models_Planilla> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                            cmd.Parameters.AddWithValue("@Grupo", grupo);
                            cmd.Parameters.AddWithValue("@Categoria", categoria);
                            cmd.Parameters.AddWithValue("@SubCategoria", subcategoria);
                            cmd.Parameters.AddWithValue("@Estado", estado);
                            cmd.Parameters.AddWithValue("@Tipo", tipo);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Planilla>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Planilla()
                                         {
                                             semana_str = dr["semana_str"].ToString(),
                                             store_name = dr["store_name"].ToString(),
                                             des_entid = dr["des_entid"].ToString(),
                                             cate3_des = dr["cate3_des"].ToString(),
                                             subcat3_des = dr["subcat3_des"].ToString(),
                                             articulo = dr["articulo"].ToString(),                                             
                                             calidad = dr["calidad"].ToString(),
                                             grupo = dr["grupo"].ToString(),
                                             cate3 = dr["cate3"].ToString(),
                                             subcat3 = dr["subcat3"].ToString(),
                                             pventa =string.IsNullOrEmpty(dr["pventa"].ToString())?0:Convert.ToDecimal(dr["pventa"].ToString()),
                                             tcant = string.IsNullOrEmpty(dr["tcant"].ToString()) ? 0 : Convert.ToDecimal(dr["tcant"].ToString()), //Convert.ToDecimal(dr["tcant"]),
                                             valor = string.IsNullOrEmpty(dr["valor"].ToString()) ? 0 : Convert.ToDecimal(dr["valor"].ToString()), //Convert.ToDecimal(dr["valor"]),
                                             med = dr["rmed"].ToString(),
                                             _00 = dr["00"].ToString(),
                                             _01 = dr["01"].ToString(),
                                             _02 = dr["02"].ToString(),
                                             _03 = dr["03"].ToString(),
                                             _04 = dr["04"].ToString(),
                                             _05 = dr["05"].ToString(),
                                             _06 = dr["06"].ToString(),
                                             _07 = dr["07"].ToString(),
                                             _08 = dr["08"].ToString(),
                                             _09 = dr["09"].ToString(),
                                             _10 = dr["10"].ToString(),
                                             _11 = dr["11"].ToString(),
                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }

        public List<Models_Vendedor> get_reporteVendedor(string cod_tda, string fecIni, string fecFin)
        {
            string sqlquery = "USP_XSTORE_REPORTE_VENDEDORES";
            List<Models_Vendedor> lista = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionPosPeru))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@codtda", cod_tda);
                            cmd.Parameters.AddWithValue("@FEC_INI", fecIni);
                            cmd.Parameters.AddWithValue("@FEC_FIN", fecFin);                          

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                lista = new List<Models_Vendedor>();
                                lista = (from DataRow dr in dt.Rows
                                         select new Models_Vendedor()
                                         {
                                             cod_entid = dr["COD_ENTID"].ToString(),
                                             des_entid = dr["DES_ENTID"].ToString(),
                                             store_name = dr["STORE_NAME"].ToString(),
                                             semana_str = dr["SEMANA_STR"].ToString(),
                                             dni = dr["DNI"].ToString(),
                                             pares = string.IsNullOrEmpty(dr["PARES"].ToString()) ? 0 : Convert.ToDecimal(dr["PARES"].ToString()),
                                             ropa = string.IsNullOrEmpty(dr["ROPA"].ToString()) ? 0 : Convert.ToDecimal(dr["ROPA"].ToString()), //Convert.ToDecimal(dr["tcant"]),
                                             acc = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["ACC"].ToString()), //Convert.ToDecimal(dr["valor"]),
                                             cant_total = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["CANT_TOTAL"].ToString()),
                                             neto = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["NETO"].ToString()),
                                             igv = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["IGV"].ToString()),
                                             total = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["TOTAL"].ToString()),
                                             upt = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["UPT"].ToString()),
                                             ntk = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["NTK"].ToString()),
                                             mayor_1 = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["MAYOR_1"].ToString()),
                                             pormay1 = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["PORMAY1"].ToString()),
                                             ticket_prom = string.IsNullOrEmpty(dr["ACC"].ToString()) ? 0 : Convert.ToDecimal(dr["TICKET_PROM"].ToString()),

                                         }).ToList();
                            }
                        }
                    }
                    catch (Exception exc)
                    {


                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }
    }
}