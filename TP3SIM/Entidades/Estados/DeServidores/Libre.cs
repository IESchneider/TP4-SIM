using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Libre : Estado
    {
        public Libre() : base("Libre", true, false, false, false, false, 
                              false, false, false, false, false, false, false) { }
    }
}
