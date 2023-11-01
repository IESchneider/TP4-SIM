using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Cerrada : Estado
    {
        public Cerrada() : base("Cerrada", true, false, false, false, false, 
                                false, false, false, false, false, false, true) { }
    }
}
