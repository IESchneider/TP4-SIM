using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Temporal
    {

        public double Numero { get; set; }
        public string Tipo { get; set; }
        public string TipoResumido { get; set; }
        public Estado Estado { get; set; }
        public double HoraIngreso { get; set; }
        //public double TiempoEspera { get; set; }
        public int EnFilaNumero { get; set; }
        public Servidor SiendoAtendidoPor { get; set; }
        public double HoraFinLectura { get; set; }

        public Temporal CopiarCliente(Temporal cliente)
        {
            Temporal clienteCopiado = new Temporal();

            clienteCopiado.Numero = cliente.Numero;
            clienteCopiado.Tipo = cliente.Tipo;
            clienteCopiado.Estado = cliente.Estado;
            clienteCopiado.HoraIngreso = cliente.HoraIngreso;
            clienteCopiado.EnFilaNumero = cliente.EnFilaNumero;
            //clienteCopiado.TiempoEspera = cliente.TiempoEspera; // Esto no va a ser lo mismo porque se calcula al final.
            clienteCopiado.SiendoAtendidoPor = cliente.SiendoAtendidoPor;

            return clienteCopiado;
        }

        public Temporal DestruirCliente(Temporal cliente)
        {
            Temporal clienteDestruido = new Temporal();

            clienteDestruido.Numero = cliente.Numero;
            clienteDestruido.Tipo = cliente.Tipo;
            clienteDestruido.Estado = new Destruido();
            clienteDestruido.HoraIngreso = cliente.HoraIngreso;
            clienteDestruido.EnFilaNumero = cliente.EnFilaNumero;
            //clienteDestruido.TiempoEspera = cliente.TiempoEspera; // Esto no va a ser lo mismo porque se calcula al final.
            clienteDestruido.SiendoAtendidoPor = cliente.SiendoAtendidoPor;
            cliente.Estado = new Destruido();
            return clienteDestruido;
        }
    }
}
