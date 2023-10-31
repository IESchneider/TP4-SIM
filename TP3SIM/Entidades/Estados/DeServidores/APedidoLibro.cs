using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3SIM.Entidades.Estados
{
    public class APedidoLibro : Estado
    {
        public APedidoLibro() : base("APedidoLibro", false, true, false, false, false,
                                     false, false, false, false, false, false, false) { }
    }
}
