﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
   public  class Ent_TrazaPedido
    {
        public string ID_PEDIDO { get; set; }
        public string FECHA_PEDIDO { get; set; }
        public string DESPACHO { get; set; }
        public string FECHA_ING_FACTURACION { get; set; }
        public string FECHA_REG_VENTA { get; set; }
        public string CLIENTE { get; set; }
        public string ESTADO { get; set; }
        public string COLOR { get; set; }
        public bool VALOR { get; set; }

    }

  
}
