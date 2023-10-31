using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3SIM.Entidades
{
    public class Abierta : Estado
    {
        public Abierta() : base("Abierta", false, false, false, false, false, 
                                false, false, false, false, false, true, false) { }
    }
}
