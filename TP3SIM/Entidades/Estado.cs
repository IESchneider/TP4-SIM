using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Estado
    {
        public string Nombre { get; }
        public bool Libre { get; }
        public bool APedidoLibro { get; }
        public bool ADevolucionLibro { get; }
        public bool AConsulta { get; }
        public bool SiendoAtendido { get; }
        public bool EPedirLibro { get; }
        public bool EDevolverLibro { get; }
        public bool EConsultar { get; }
        public bool EnBiblioteca { get; }
        public bool Destruido { get; }
        public bool Abierta { get; }
        public bool Cerrada { get; }

        protected Estado(string nombre, bool libre, bool aPedidoLibro, bool aDevolucionLibro, bool aConsulta, 
                         bool siendoAtendido, bool ePedirLibro, bool eDevolverLibro, bool eCosultar, bool enBiblioteca, bool destruido, 
                         bool abierta, bool cerrada)
        {
            Nombre = nombre;

            Libre = libre;
            APedidoLibro = aPedidoLibro;
            ADevolucionLibro = aDevolucionLibro;
            AConsulta = aConsulta;

            SiendoAtendido = siendoAtendido;
            EPedirLibro = ePedirLibro;
            EDevolverLibro = eDevolverLibro;
            EConsultar = eCosultar;
            EnBiblioteca = enBiblioteca;
            Destruido = destruido;

            Abierta = abierta;
            Cerrada = cerrada;

        }
    }
}
