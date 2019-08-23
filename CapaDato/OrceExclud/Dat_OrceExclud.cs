﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaEntidad.Util;
using System.Data.SqlClient;
using CapaEntidad.OrceExlud;

namespace CapaDato.OrceExclud
{
    public class Dat_OrceExclud
    {
        Ent_Combo entCombo = new Ent_Combo();
        public List<Ent_Combo> get_cadena()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_ORCE_GET_CADENA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();

                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["COD_CADENA"].ToString();
                                combo.cbo_descripcion = dr["DES_CADENA"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Orce_Inter_Cab> get_lista_orce_exlude()
        {
            List<Ent_Orce_Inter_Cab> list = null;
            
            string sqlquery = "[USP_ORCE_GET_INTERFACE_CAB]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Orce_Inter_Cab>();
                            while (dr.Read())
                            {
                                Ent_Orce_Inter_Cab cab = new Ent_Orce_Inter_Cab();

                                cab.ORC_COD = Convert.ToInt32(dr["ORC_COD"]);
                                cab.ORC_DESCRIPCION = dr["ORC_DESCRIPCION"].ToString();
                                cab.ORC_ATRIBUTO = dr["ORC_ATRIBUTO"].ToString();
                                cab.ORC_ENVIADO = Convert.ToBoolean(dr["ORC_ENVIADO"]);
                                cab.ORC_FEC_ENV = (String.IsNullOrEmpty(dr["ORC_FEC_ENV"].ToString()) ? "" : Convert.ToDateTime(dr["ORC_FEC_ENV"]).ToString("dd-MM-yyyy HH:mm"));
                                cab.ORC_EST_ID = dr["ORC_EST_ID"].ToString();
                                cab.ORC_FECHA_ING = (String.IsNullOrEmpty(dr["ORC_FECHA_ING"].ToString()) ? "" : Convert.ToDateTime(dr["ORC_FECHA_ING"]).ToString("dd-MM-yyyy HH:mm")); //Convert.ToDateTime(dr["ORC_FECHA_ING"]);
                                cab.ORC_FECHA_ACT = (String.IsNullOrEmpty(dr["ORC_FECHA_ACT"].ToString()) ? "" : Convert.ToDateTime(dr["ORC_FECHA_ACT"]).ToString("dd-MM-yyyy HH:mm"));  //Convert.ToDateTime(dr["ORC_FECHA_ACT"]);
                                cab.EST_ORC_DES = dr["Est_Orc_Des"].ToString();
                                list.Add(cab);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Orce_Inter_Det_Tda> get_lista_det_tdas(string cod_orce)
        {
            List<Ent_Orce_Inter_Det_Tda> list = null;

            string sqlquery = "[USP_ORCE_GET_INTERFACE_DET_TDA]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_ORCE", cod_orce);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Orce_Inter_Det_Tda>();
                            while (dr.Read())
                            {
                                Ent_Orce_Inter_Det_Tda det_tdas = new Ent_Orce_Inter_Det_Tda();
                                det_tdas.ORC_DET_TDA = dr["ORC_DET_TDA"].ToString();
                                det_tdas.ORC_DET_TDA_DES = dr["des_entid"].ToString();
                                det_tdas.ORC_DET_TDA_CAD = dr["cod_cadena"].ToString();
                                list.Add(det_tdas);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Combo> get_tda_cadena(string cadena)
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_ORCE_GET_TDA_CADENA";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_CADENA", cadena);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            while (dr.Read())
                            {
                                Ent_Combo combo = new Ent_Combo();
                                combo.cbo_codigo = dr["COD_ENTID"].ToString();
                                combo.cbo_descripcion = dr["DES_ENTID"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Orce_Inter_Art> get_lista_det_art(string cod_orce)
        {
            List<Ent_Orce_Inter_Art> list = null;
            string sqlquery = "[USP_ORCE_GET_INTERFACE_DET_ART]";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_ORCE", cod_orce);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Orce_Inter_Art>();
                            while (dr.Read())
                            {
                                Ent_Orce_Inter_Art ART = new Ent_Orce_Inter_Art();
                                ART.ARTICULO = dr["ORC_DET_ART"].ToString();
                                ART.ORC_DET_ART_COD = Convert.ToInt32( dr["ORC_DET_ART_COD"].ToString());
                                ART.VALOR = Convert.ToBoolean(dr["ORC_DET_EST"].ToString());
                                list.Add(ART);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }

        public List<Ent_Orce_Inter_Art> get_articulos_atributo(string atributo)
        {
            List<Ent_Orce_Inter_Art> list = null;
            string sqlquery = "USP_ORCE_GET_ART_ATRIBUTOS";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@COD_ATR", atributo);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Orce_Inter_Art>();
                            while (dr.Read())
                            {
                                Ent_Orce_Inter_Art ART = new Ent_Orce_Inter_Art();
                                ART.ARTICULO = dr["ARTICULO"].ToString();
                                ART.ATRIBUTO = dr["ATRIBUTO"].ToString();
                                ART.VALOR = Convert.ToBoolean(dr["VALOR"].ToString());
                                list.Add(ART);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }



        public List<Ent_Combo> get_atributos()
        {
            List<Ent_Combo> list = null;
            string sqlquery = "USP_ORCE_GET_ATRIBUTOS";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_Combo>();
                            Ent_Combo combo = new Ent_Combo();
                            combo.cbo_codigo = "-1";
                            combo.cbo_descripcion = "---Seleccione---";
                            list.Add(combo);
                            while (dr.Read())
                            {
                                combo = new Ent_Combo();
                                combo.cbo_codigo = dr["COD_ATR"].ToString();
                                combo.cbo_descripcion = dr["DES_ATR"].ToString();
                                list.Add(combo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                list = null;
            }
            return list;
        }
        public int ORCE_INTERFACE_EXCLUD_ACT(int codigo, string descripcion, string atributo, string estado_orce, decimal usu_id, int estado, List<Ent_Orce_Inter_Art> listArticulos, string tdaCadena, ref string mensaje)
        {
            string sqlquery = "USP_ORCE_INTERFACE_EXCLUD_ACT";
            int f = 0;
            DataTable TMP_ORCE_INTERFACE_ART = null;
            DataTable TMP_ORCE_INTERFACE_DET_TDA = null;
            if (estado != 3)
            {
                TMP_ORCE_INTERFACE_ART = new DataTable();
                TMP_ORCE_INTERFACE_ART = _toDTListArti(listArticulos);

                TMP_ORCE_INTERFACE_DET_TDA = new DataTable();
                TMP_ORCE_INTERFACE_DET_TDA = _toDTTdaCdn(tdaCadena);
            }            

            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ORC_COD", codigo);
                        cmd.Parameters.AddWithValue("@ORC_DESCRIPCION", descripcion);
                        cmd.Parameters.AddWithValue("@ORC_ATRIBUTO", atributo);
                        cmd.Parameters.AddWithValue("@ORC_EST_ID", estado_orce);
                        cmd.Parameters.AddWithValue("@USUID_ING", usu_id);
                        cmd.Parameters.AddWithValue("@ESTAOD", estado);
                        cmd.Parameters.AddWithValue("@TMP_ORCE_INTERFACE_ART", TMP_ORCE_INTERFACE_ART);
                        cmd.Parameters.AddWithValue("@TMP_ORCE_INTERFACE_DET_TDA", TMP_ORCE_INTERFACE_DET_TDA);
                        cmd.ExecuteNonQuery();
                        f = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                f = 0;
                mensaje = ex.Message;
            }
            return f;
        }

        private DataTable _toDTTdaCdn(string tdaCadena)
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("ORC_DET_TDA");
            string[] tdas = tdaCadena.Split(',');
            foreach (var item in tdas)
            {
                dtRet.Rows.Add(item);
            }
            return dtRet;
        }

        private DataTable _toDTListArti(List<Ent_Orce_Inter_Art> listArticulos)
        {
            DataTable dtRet = new DataTable();
            dtRet.Columns.Add("ORC_DET_ART");
            dtRet.Columns.Add("ORC_DET_EST");
            foreach (var item in listArticulos)
            {
                dtRet.Rows.Add(item.ARTICULO, ( item.VALOR.ToString().ToUpper() == "TRUE" ? 1 : 0));
            }
            return dtRet;
        }
    }
}
