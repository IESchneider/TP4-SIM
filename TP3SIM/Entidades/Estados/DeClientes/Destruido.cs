using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Destruido : Estado
    {
        public Destruido() : base("Destruido", false, false, false, false, false,
                                  false, false, false, false, true, false, false) { }
    }
}
