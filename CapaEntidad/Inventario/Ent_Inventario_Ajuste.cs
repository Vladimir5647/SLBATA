﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Inventario
{
    public class Ent_Inventario_Ajuste
    {
        
    }
    public class Ent_Inv_Ajuste_Articulos
    {
        public string ARTICULO { get; set; }
        public string CALIDAD { get; set; }
        public string MEDIDA { get; set; }
        public decimal TEORICO { get; set; }
        public decimal STOCK { get; set; }
        public decimal DIFERENCIA { get; set; }
    }
}
