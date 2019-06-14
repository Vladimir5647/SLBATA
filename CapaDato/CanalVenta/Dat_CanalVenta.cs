﻿using CapaEntidad.Util;
using CapaEntidad.CanalVenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDato.CanalVenta
{
    public class Dat_CanalVenta
    {
        public DataTable get_tienda_origenes()
        {
            string sqlquery = "[sp_get_tiendas_origen]";
            DataTable origenes = new DataTable();
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                   // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;                        
                        SqlDataAdapter da = new SqlDataAdapter(cmd);                          
                        da.Fill(origenes);
                    }
                }
            }
            catch (Exception ex)
            {
                origenes = null;
            }
            return origenes;
        }

        public int insertar_historial_estados_cv(Ent_HistorialEstadosCV historial)
        {
            string sqlquery = "usp_insertar_historial_estados_cv";
            int f = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_entid", historial.cod_entid );
                        cmd.Parameters.AddWithValue("@fc_nint", historial.fc_nint );
                        cmd.Parameters.AddWithValue("@id_estado", historial.id_estado );
                        cmd.Parameters.AddWithValue("@descripcion", historial.descripcion );
                        cmd.Parameters.AddWithValue("@cod_usuario", historial.cod_usuario );
                        cmd.Parameters.AddWithValue("@cod_vendedor", historial.cod_vendedor);
                        f = cmd.ExecuteNonQuery();
                    }

                }
            }catch (Exception ex)
            {
                f = 0;
            }
            return f;
        }

        public void insertar_ge_cv( string cod_entid , string fc_nint , string serie_numero , string ge)
        {
            string sqlquery = "usp_cv_insertar_ge";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_entid", cod_entid);
                        cmd.Parameters.AddWithValue("@fc_nint", fc_nint);
                        cmd.Parameters.AddWithValue("@serie_numero", serie_numero);
                        cmd.Parameters.AddWithValue("@ge", ge);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<Ent_VentaCanal> get_Ventas(DateTime fdesde , DateTime fhasta, string noDocCli , string noDoc, string tiendaOrigen , string tiendaDestino, string tipo, string estado)
        {
            List<Ent_VentaCanal> list = null;
            string sqlquery = "usp_select_canal_ventas";
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fdesde", fdesde);
                        cmd.Parameters.AddWithValue("@fhasta", fhasta);
                        cmd.Parameters.AddWithValue("@noDocCli", noDocCli);
                        cmd.Parameters.AddWithValue("@noDoc", noDoc);
                        cmd.Parameters.AddWithValue("@tiendaOrigen", tiendaOrigen);
                        cmd.Parameters.AddWithValue("@tiendaDestino", tiendaDestino);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@estado", estado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_VentaCanal>();

                            while (dr.Read())
                            {
                                Ent_VentaCanal ven = new Ent_VentaCanal();
                                // rol.rol_id = dr["rol_id"].ToString();
                                ven.serieNumero = dr["FC_SFAC"].ToString() + "-" + dr["FC_NFAC"].ToString();
                                ven.tiendaOrigen = dr["COD_ENTID"].ToString() + " - " + dr["des_entida"].ToString();
                                ven.tiendaDestino = dr["FC_ID_TDACVTA"].ToString() + " - " + dr["des_entidb"].ToString(); ;
                                ven.tipo = dr["FC_ID_TIP_CVTA"].ToString();
                                ven.estado = dr["FC_ID_ESTADO_CVTA"].ToString();
                                ven.cliente = (dr["FC_RUC"].ToString() + " - " + dr["FC_NOMB"].ToString() + " " + dr["FC_APEP"].ToString() + " " + dr["FC_APEM"].ToString()).Trim() ;
                                ven.fechaVenta = Convert.ToDateTime(dr["FC_FFAC"]);
                                ven.fc_nint = dr["FC_NINT"].ToString();
                                ven.cod_entid = dr["COD_ENTID"].ToString();
                                ven.nombreEstado = dr["nombreEstado"].ToString();
                                ven.descripcionEstado = dr["descripcionEstado"].ToString();
                                ven.colorEstado = dr["colorEstado"].ToString();
                                ven.importeTotal = Convert.ToDecimal( dr["FC_TOTAL"].ToString());
                                ven.nombreTipoCV = dr["nombre_tipo_cv"].ToString();
                                list.Add(ven);
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


        public Ent_VentaCanal get_Ventas_por_sn(string noDoc , string cod_entid , string fc_nint)
        {
            Ent_VentaCanal ven = null;
            string sqlquery = "usp_select_canal_ventas_por_sn";
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@serie_numero", noDoc);
                        cmd.Parameters.AddWithValue("@cod_entid", cod_entid);
                        cmd.Parameters.AddWithValue("@fc_nint", fc_nint);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables.Count>0)
                        {
                            DataTable dtC = ds.Tables[0];
                            DataTable dtD = ds.Tables[1];
                            DataTable dtH = ds.Tables[2];
                            ven = new Ent_VentaCanal();
                            ven.cod_entid = dtC.Rows[0]["COD_ENTID"].ToString();
                            ven.serieNumero = dtC.Rows[0]["FC_SFAC"].ToString() + "-" + dtC.Rows[0]["FC_NFAC"].ToString();
                            ven.tiendaOrigen = dtC.Rows[0]["COD_ENTID"].ToString() + " - " + dtC.Rows[0]["des_entida"].ToString();
                            ven.tiendaDestino = dtC.Rows[0]["FC_ID_TDACVTA"].ToString() + " - " + dtC.Rows[0]["des_entidb"].ToString();
                            ven.tipo = dtC.Rows[0]["FC_ID_TIP_CVTA"].ToString().Trim();
                            ven.estado = dtC.Rows[0]["FC_ID_ESTADO_CVTA"].ToString();
                            ven.cliente = (dtC.Rows[0]["FC_RUC"].ToString() + " - " + dtC.Rows[0]["FC_NOMB"].ToString() + " " + dtC.Rows[0]["FC_APEP"].ToString() + " " + dtC.Rows[0]["FC_APEM"].ToString()).Trim();
                            ven.fechaVenta = Convert.ToDateTime(dtC.Rows[0]["FC_FFAC"]);
                            ven.direccionA = dtC.Rows[0]["direccion_a"].ToString();
                            ven.direccionB = dtC.Rows[0]["direccion_b"].ToString();
                            ven.direccionCliente = dtC.Rows[0]["FC_DCLI"].ToString();
                            ven.referenciaCliente = dtC.Rows[0]["FC_REFERE"].ToString();
                            ven.hora = dtC.Rows[0]["FC_HORA"].ToString();
                            ven.noDocCli = dtC.Rows[0]["FC_RUC"].ToString();
                            ven.nombreCliente = dtC.Rows[0]["FC_NOMB"].ToString();
                            ven.apeMatCliente = dtC.Rows[0]["FC_APEM"].ToString();
                            ven.apePatCliente = dtC.Rows[0]["FC_APEP"].ToString();
                            ven.nombreCompletoCliente = (dtC.Rows[0]["FC_NOMB"].ToString() + " " + dtC.Rows[0]["FC_APEP"].ToString() + " " + dtC.Rows[0]["FC_APEM"].ToString()).Trim();
                            ven.tipoComprobante = dtC.Rows[0]["FC_SUNA"].ToString();
                            ven.fc_nint = dtC.Rows[0]["FC_NINT"].ToString();
                            ven.idVendedor = dtC.Rows[0]["FC_VEND"].ToString();
                            ven.nomVendedor = dtC.Rows[0]["V_NOMB"].ToString();
                            ven.nombreEstado = dtC.Rows[0]["nombreEstado"].ToString();
                            ven.descripcionEstado = dtC.Rows[0]["descripcionEstado"].ToString();
                            ven.colorEstado = dtC.Rows[0]["colorEstado"].ToString();
                            ven.importeTotal = Convert.ToDecimal(dtC.Rows[0]["FC_TOTAL"].ToString());
                            ven.nombreTipoCV = dtC.Rows[0]["nombre_tipo_cv"].ToString();
                            ven.cod_entid_b = dtC.Rows[0]["FC_ID_TDACVTA"].ToString();
                            ven.guia_electronica = dtC.Rows[0]["ge"].ToString();
                            ven.ubigeoCliente = dtC.Rows[0]["fc_ubi"].ToString();
                            ven.ubigeoTienda = dtC.Rows[0]["cod_ubigeo"].ToString();
                            ven.telefonoCliente = dtC.Rows[0]["fc_lcon"].ToString();
                            List<Ent_DetallesVentaCanal> listVenD = new List<Ent_DetallesVentaCanal>();                            
                            foreach (DataRow item in dtD.Rows)
                            {
                                Ent_DetallesVentaCanal venD = new Ent_DetallesVentaCanal();
                                venD.codigoProducto = item["FD_ARTI"].ToString();
                                venD.nombreProducto = item["des_artic"].ToString(); 
                                venD.precioUnitario = item["FD_PREF"].ToString();
                                venD.descuento = item["FD_DREF"].ToString();
                                venD.total = item["FD_TOTAL"].ToString();
                                venD.cantidad = Convert.ToInt32( Convert.ToDouble(item["FD_QFAC"].ToString()));
                                venD.talla = item["FD_REGL"].ToString();
                                listVenD.Add(venD);
                            }
                            ven.detalles = listVenD;
                            List<Ent_HistorialEstadosCV> listHist = new List<Ent_HistorialEstadosCV>();
                            foreach (DataRow item in dtH.Rows)
                            {
                                Ent_HistorialEstadosCV _hist = new Ent_HistorialEstadosCV();
                                _hist.cod_entid = item["cod_entid"].ToString();
                                _hist.fc_nint = item["fc_nint"].ToString();
                                _hist.id_estado = item["id_estado"].ToString();
                                _hist.fecha =(DateTime)item["fecha"];
                                _hist.cod_usuario = item["usu_id"].ToString();
                                _hist.descripcion = item["descripcion"].ToString();
                                _hist.usu_nombre = item["usu_nombre"].ToString();
                                _hist.nombreEstado = item["nombreEstado"].ToString();
                                _hist.colorEstado = item["colorEstado"].ToString();
                                _hist.descripcionEstado = item["descripcionEstado"].ToString();
                                _hist.cod_vendedor = item["cod_vendedor"].ToString();
                                _hist.nomVendedor = item["v_nomb"].ToString();
                                listHist.Add(_hist);
                            }
                            ven.historialEstados = listHist;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ven = null;
            }
            return ven;
        }

        public DataTable get_vendedores_tda(string cod_tda)
        {
            string sqlquery = "[sp_get_vendedores_tda_cv]";
            DataTable dt_vendedores = new DataTable();
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cod_tda", cod_tda);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt_vendedores);
                    }
                }
            }
            catch (Exception ex)
            {
                dt_vendedores = null;
            }
            return dt_vendedores;
        }

        public DataTable get_tiendas_destino(string tiendaOrigen)
        {
            string sqlquery = "[sp_get_tiendas_destino]";
            DataTable destinos = new DataTable();
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexion))
                {
                    // if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codTdaOri", tiendaOrigen);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(destinos);
                    }
                }
            }
            catch (Exception ex)
            {
                destinos = null;
            }
            return destinos;
        }
    }
  
}