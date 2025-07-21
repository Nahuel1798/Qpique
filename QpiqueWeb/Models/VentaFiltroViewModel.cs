using System;
using System.Collections.Generic;
using QpiqueWeb.Models;

namespace QpiqueWeb.ViewModels
{
    public class VentaFiltroViewModel
    {
        public IEnumerable<Venta> Ventas { get; set; }

        public DateTime? FechaFiltro { get; set; }
        public string ProductoFiltro { get; set; }
        public string ClienteFiltro { get; set; }
    }
}
