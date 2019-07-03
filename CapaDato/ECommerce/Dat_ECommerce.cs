﻿using CapaEntidad.Util;
using CapaEntidad.ECommerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDato.ECommerce
{
    public class Dat_ECommerce
    {
        public DataTable get_tienda_origenes()
        {
            string sqlquery = "[sp_get_tiendas_origen]";
            DataTable origenes = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
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

        //public void insertar_historial_estados_cv(Ent_HistorialEstadosCV historial)
        //{
        //    string sqlquery = "usp_insertar_historial_estados_cv";
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
        //        {
        //            if (cn.State == 0) cn.Open();
        //            using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@cod_entid", historial.cod_entid );
        //                cmd.Parameters.AddWithValue("@fc_nint", historial.fc_nint );
        //                cmd.Parameters.AddWithValue("@id_estado", historial.id_estado );
        //                cmd.Parameters.AddWithValue("@descripcion", historial.descripcion );
        //                cmd.Parameters.AddWithValue("@cod_usuario", historial.cod_usuario );
        //                cmd.Parameters.AddWithValue("@cod_vendedor", historial.cod_vendedor);
        //                cmd.ExecuteNonQuery();
        //            }

        //        }
        //    }catch (Exception ex)
        //    {

        //    }
        //}

        public List<Ent_ECommerce> get_Ventas(DateTime fdesde , DateTime fhasta, string noDocCli , string noDoc )
        {
            List<Ent_ECommerce> list = null;
            string sqlquery = "USP_ECOM_Lista_Ventas";
            string _tienda = "50" + Environment.GetEnvironmentVariable("codtda").ToString();
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Fec_Ini", fdesde.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@Fec_Fin", fhasta.ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@NDoc_Cli", noDocCli);
                        cmd.Parameters.AddWithValue("@Nro_Doc", noDoc);
                        cmd.Parameters.AddWithValue("@Tda_Id", _tienda);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            list = new List<Ent_ECommerce>();

                            while (dr.Read())
                            {
                                Ent_ECommerce ven = new Ent_ECommerce();
                                ven.idPedido = dr["Pedido_Id"].ToString();
                                ven.RefPedido = dr["Pedido_Ref"].ToString();
                                ven.fechaPedido = Convert.ToDateTime(dr["Pedido_Fecha"]);
                                ven.tipoComprobante = dr["Doc_Tipo"].ToString();
                                ven.SerieDoc = dr["Doc_Serie"].ToString();
                                ven.NroDoc = dr["Doc_Nro"].ToString();
                                ven.CodSeguimiento = dr["Doc_Seguimiento"].ToString();
                                ven.nom_courier = dr["Nom_courier"].ToString();
                                ven.estado = dr["Estado"].ToString();
                                ven.cliente = dr["Cliente_Id"].ToString();
                                ven.direccionCliente = dr["Cliente_Direccion"].ToString();
                                ven.referenciaCliente = dr["Cliente_Direccion"].ToString();
                                ven.TpDocCli = dr["Cliente_Tp_Doc"].ToString();
                                ven.noDocCli = dr["Cliente_No_Doc"].ToString();
                                ven.nombreCliente = dr["Cliente_Nombre"].ToString();
                                ven.apePatCliente = dr["Cliente_ApePat"].ToString();
                                ven.apeMatCliente = dr["Cliente_ApeMat"].ToString();
                                ven.nombreCompletoCliente = dr["Cliente_NombreCompleto"].ToString();
                                ven.cod_entid = dr["Entidad_Id"].ToString();
                                ven.nombreEstado = dr["Nombre_Estado"].ToString();
                                list.Add(ven);
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


        public Ent_ECommerce get_Ventas_por_sn(string noDoc , string cod_entid )
        {
            Ent_ECommerce ven = null;
            string sqlquery = "USP_Select_Ventas_x_Tda";
            try
            {
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NroDoc", noDoc);
                        cmd.Parameters.AddWithValue("@Tda_Id", cod_entid);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        if (ds.Tables.Count>0)
                        {
                            DataTable dtC = ds.Tables[0];
                            DataTable dtD = ds.Tables[1];
                            //DataTable dtH = ds.Tables[2];
                            ven = new Ent_ECommerce();                            
                            ven.idPedido = dtC.Rows[0]["Pedido_Id"].ToString();
                            //ven.tiendaOrigen = dtC.Rows[0]["COD_ENTID"].ToString() + " - " + dtC.Rows[0]["des_entida"].ToString();
                            //ven.tiendaDestino = dtC.Rows[0]["FC_ID_TDACVTA"].ToString() + " - " + dtC.Rows[0]["des_entidb"].ToString();
                            ven.fechaPedido = Convert.ToDateTime(dtC.Rows[0]["Pedido_Fecha"]);
                            ven.tipoComprobante = dtC.Rows[0]["Doc_Tipo"].ToString();
                            ven.SerieDoc = dtC.Rows[0]["Doc_Serie"].ToString();
                            ven.NroDoc = dtC.Rows[0]["Doc_Nro"].ToString();
                            ven.CodSeguimiento = dtC.Rows[0]["Doc_Seguimiento"].ToString();
                            ven.CodSeguimiento = dtC.Rows[0]["Nom_Courier"].ToString();
                            ven.estado = dtC.Rows[0]["Estado"].ToString();
                            ven.cliente = dtC.Rows[0]["Cliente_Id"].ToString();
                            ven.direccionA = "";//dtC.Rows[0]["direccion_a"].ToString();
                            ven.direccionB = "";//dtC.Rows[0]["direccion_b"].ToString();
                            ven.direccionCliente = dtC.Rows[0]["Cliente_Direccion"].ToString();
                            ven.referenciaCliente = "";//dtC.Rows[0]["FC_REFERE"].ToString();
                            //ven.hora = dtC.Rows[0]["FC_HORA"].ToString();
                            ven.TpDocCli = dtC.Rows[0]["Cliente_Tp_Doc"].ToString();
                            ven.noDocCli = dtC.Rows[0]["Cliente_No_Doc"].ToString();
                            ven.nombreCliente = dtC.Rows[0]["Cliente_Nombre"].ToString();
                            ven.apePatCliente = dtC.Rows[0]["Cliente_ApePat"].ToString();
                            ven.apeMatCliente = dtC.Rows[0]["Cliente_ApeMat"].ToString();
                            ven.nombreCompletoCliente = dtC.Rows[0]["Cliente_NombreCompleto"].ToString();
                            ven.cod_entid = dtC.Rows[0]["Entidad_Id"].ToString();
                            //ven.idVendedor = dtC.Rows[0]["FC_VEND"].ToString();
                            //ven.nomVendedor = dtC.Rows[0]["V_NOMB"].ToString();
                            ven.nombreEstado = dtC.Rows[0]["nombreEstado"].ToString();
                            //ven.descripcionEstado = dtC.Rows[0]["descripcionEstado"].ToString();
                            //ven.colorEstado = dtC.Rows[0]["colorEstado"].ToString();
                            //ven.importeTotal = Convert.ToDecimal(dtC.Rows[0]["FC_TOTAL"].ToString());
                            //ven.nombreTipoCV = dtC.Rows[0]["nombre_tipo_cv"].ToString();

                            List<Ent_DetallesECommerce> listVenD = new List<Ent_DetallesECommerce>();                            
                            foreach (DataRow item in dtD.Rows)
                            {
                                Ent_DetallesECommerce venD = new Ent_DetallesECommerce();
                                venD.codigoProducto = item["Producto_Id"].ToString();
                                venD.nombreProducto = item["Producto_Desc"].ToString();
                                venD.cantidad = item["Producto_Cant"].ToString();
                                venD.precioUnitario = item["Producto_Prec_Unit"].ToString();
                                venD.descuento = item["Producto_Dcto"].ToString();
                                venD.total = item["Producto_Total"].ToString();
                                venD.talla = item["Producto_Talla"].ToString();
                                listVenD.Add(venD);
                            }
                            ven.detalles = listVenD;
                            //List<Ent_HistorialEstadosCV> listHist = new List<Ent_HistorialEstadosCV>();
                            //foreach (DataRow item in dtH.Rows)
                            //{
                            //    Ent_HistorialEstadosCV _hist = new Ent_HistorialEstadosCV();
                            //    _hist.cod_entid = item["cod_entid"].ToString();
                            //    _hist.fc_nint = item["fc_nint"].ToString();
                            //    _hist.id_estado = item["id_estado"].ToString();
                            //    _hist.fecha =(DateTime)item["fecha"];
                            //    _hist.cod_usuario = item["usu_id"].ToString();
                            //    _hist.descripcion = item["descripcion"].ToString();
                            //    _hist.usu_nombre = item["usu_nombre"].ToString();
                            //    _hist.nombreEstado = item["nombreEstado"].ToString();
                            //    _hist.colorEstado = item["colorEstado"].ToString();
                            //    _hist.descripcionEstado = item["descripcionEstado"].ToString();
                            //    _hist.cod_vendedor = item["cod_vendedor"].ToString();
                            //    _hist.nomVendedor = item["v_nomb"].ToString();
                            //    listHist.Add(_hist);
                            //}
                            //ven.historialEstados = listHist;
                        }
                    }
                }
            }
            catch (Exception)
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
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
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
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
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