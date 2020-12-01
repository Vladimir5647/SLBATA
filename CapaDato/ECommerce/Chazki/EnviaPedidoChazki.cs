﻿using System;
using System.Collections.Generic;
using System.Linq;
using CapaEntidad.ECommerce;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad.Util;

namespace CapaDato.ECommerce.Chazki
{
    public class EnviaPedidoChazki
    {
        public Ent_Ecommerce_Chazki get_Ventas_por_Chazki(string ven_id)
        {
            Ent_Ecommerce_Chazki ven = null;
            string sqlquery = "USP_ECOMMERCE_LISTA_CHAZKI";
            try
            {
                //Ent_Conexion.conexion = "Server=192.168.1.242;Database=BDPOS;User ID=sa;Password=1;Trusted_Connection=False;";
                using (SqlConnection cn = new SqlConnection(Ent_Conexion.conexionEcommerce))
                {
                    if (cn.State == 0) cn.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlquery, cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@serie_numero", noDoc);
                        cmd.Parameters.AddWithValue("@ven_id", ven_id);
                        //cmd.Parameters.AddWithValue("@fc_nint", CodInterno);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            //DataTable dtC = ds.Tables[0];
                            DataTable dtD = ds.Tables[0]; //data articulos
                            DataTable dtIC = ds.Tables[1]; //data chaski
                            DataTable dtID = ds.Tables[2]; //data direccion cliente 
                            ven = new Ent_Ecommerce_Chazki();
                            /*ARTICULOS*/
                            List<Ent_DetallesVentaCanal_E> listVenD = new List<Ent_DetallesVentaCanal_E>();
                            foreach (DataRow item in dtD.Rows)
                            {
                                Ent_DetallesVentaCanal_E venD = new Ent_DetallesVentaCanal_E();
                                venD.codigoProducto = item["codigo_articulo"].ToString();
                                venD.nombreProducto = item["descripcion_articulo"].ToString();
                                //venD.precioUnitario = item["FD_PREF"].ToString();
                                //venD.descuento = item["FD_DREF"].ToString();
                                venD.total = item["precio_total"].ToString();
                                venD.cantidad = Convert.ToInt32(Convert.ToDouble(item["cantidad"].ToString()));
                                //venD.talla = item["FD_REGL"].ToString();
                                //venD.fd_colo = item["FD_COLO"].ToString();
                                listVenD.Add(venD);
                            }
                            ven.detalles2 = listVenD;
                            /*TIENDA ORIGEN*/
                            if (dtIC.Rows.Count > 0)
                            {
                                Ent_Informacion_Tienda_envio_E ic = new Ent_Informacion_Tienda_envio_E();
                                ic.id = Convert.ToInt32(dtIC.Rows[0]["id"]);
                                //ic.cod_entid = dtIC.Rows[0]["cod_entid"].ToString();
                                //ic.courier = dtIC.Rows[0]["courier"].ToString();
                                //ic.cx_nroDocProveedor = dtIC.Rows[0]["cx_nroDocProveedor"].ToString();
                                //ic.cx_codTipoDocProveedor = dtIC.Rows[0]["cx_codTipoDocProveedor"].ToString();
                                //ic.cx_codDireccionProveedor = dtIC.Rows[0]["cx_codDireccionProveedor"].ToString();
                                //ic.cx_codCliente = dtIC.Rows[0]["cx_codCliente"].ToString();
                                //ic.cx_codCtaCliente = dtIC.Rows[0]["cx_codCtaCliente"].ToString();
                                //ic.id_usuario = dtIC.Rows[0]["id_usuario"].ToString();
                                //ic.de_terminal = dtIC.Rows[0]["de_terminal"].ToString();
                                ic.chaski_storeId = dtIC.Rows[0]["chazki_store_id"].ToString();
                                ic.chaski_branchId = dtIC.Rows[0]["chazki_branch_id"].ToString();
                                ic.chaski_api_key = dtIC.Rows[0]["chaski_api_key"].ToString();
                                ic.deliveryTrack_Code = dtIC.Rows[0]["deliveryTrack_Code"].ToString();
                                ic.mode = dtIC.Rows[0]["mode"].ToString();
                                ic.tiempo = dtIC.Rows[0]["tiempo"].ToString();
                                ic.payment_method = dtIC.Rows[0]["payment_method"].ToString();
                                ven.informacionTiendaEnvio = ic;
                            }
                            /*CLENTE DESTINO*/
                            if (dtID.Rows.Count > 0) //des
                            {
                                Ent_Informacion_Tienda_Destinatario_E id = new Ent_Informacion_Tienda_Destinatario_E();

                                //id.id = Convert.ToInt32(dtID.Rows[0]["id"]);
                                id.nroDocumento = dtID.Rows[0]["dni_ruc"].ToString();
                                id.email = dtID.Rows[0]["correo"].ToString();
                                id.cliente = dtID.Rows[0]["cliente"].ToString();
                                id.referencia = "";
                                id.telefono = dtID.Rows[0]["telefono"].ToString();
                                id.direccion_entrega = dtID.Rows[0]["direccion_cliente"].ToString();
                                //id.cod_entid = dtID.Rows[0]["COD_ENTID"].ToString();
                                id.ubigeo = dtID.Rows[0]["ubigeo"].ToString();
                                ven.informacionTiendaDestinatario = id;
                            }
                            //ven.historialEstados = listHist;
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







    }
}
