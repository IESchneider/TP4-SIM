﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3SIM.Entidades
{
    public class EPedirLibro : Estado
    {
        public EPedirLibro() : base("EPedirLibro", false, false, false, false, false, 
                                    true, false, false, false, false, false, false) { }
    }
}