using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4SIM.Entidades
{
    public class Fila
    {
        // Atributos del vector de estado para cada fila. Se utilizarán dos objetos filas.
        
        // Evento, Reloj y llegadas.

        public string Evento { get; set; }
        public double Reloj { get; set; }
        public double RND_Llegada { get; set; }
        public double TiempoEntreLlegadas { get; set; }
        public double ProximaLlegada { get; set; }
        public double RND_TipoAtencion { get; set; }
        public string TipoAtencion { get; set; }

        // Fin atención y empleados.

        public double RND_FinAtencion{ get; set; }
        public double TiempoAtencion { get; set; }
        public double ProxFinAtencion_1 { get; set; }
        public double ProxFinAtencion_2 { get; set; }
        public Estado EstadoEmpleado_1 { get; set; }
        public Estado EstadoEmpleado_2 { get; set; }
        public int Cola { get; set; }

        // Fin lectura y biblioteca.

        public double RND_FinLectura { get; set; }
        public string Se_queda { get; set; }
        public double RND_TiempoLectura { get; set; }
        public double TiempoLectura { get; set; }
        public double ProxFinLectura { get; set; }
        public Estado EstadoBiblioteca { get; set; }

        // Estadísticas.

        public double TiempoPermanenciaBiblioteca { get; set; }
        public int CantPersonasBiblioteca { get; set; }
        public double PromTiempoPermanenciaBiblio { get; set; }
        public int CantPersonasQueIngresanBiblio { get; set; }
        public int CantPersonasQueNoIngresanBiblio { get; set; }
        public double PromPersonasQueNoIngresanBiblio { get; set; }
        public int CantTotalPersonas { get; set; }

        // Lista de todas las personas.

        public List<Temporal> Persona { get; set; } = new List<Temporal>();

        public void Reiniciar()
        {
            Evento = null;
            Reloj = 0.0;

            RND_Llegada = 0.0;
            TiempoEntreLlegadas = 0.0;
            ProximaLlegada = 0.0;
            RND_TipoAtencion = 0.0;
            TipoAtencion = "";
            TiempoAtencion = 0.0;
            ProxFinAtencion_1 = 0.0;
            ProxFinAtencion_2 = 0.0;
            EstadoEmpleado_1 = null;
            EstadoEmpleado_2 = null;
            Cola = 0;

            RND_FinLectura = 0.0;
            Se_queda = null;
            RND_TiempoLectura = 0.0;
            TiempoLectura = 0.0;
            ProxFinLectura = 0.0;
            EstadoBiblioteca = null;

            TiempoPermanenciaBiblioteca = 0.0;
            CantPersonasBiblioteca = 0;
            PromTiempoPermanenciaBiblio = 0.0;
            CantPersonasQueIngresanBiblio = 0;
            CantPersonasQueNoIngresanBiblio = 0;
            PromPersonasQueNoIngresanBiblio = 0.0;
            CantTotalPersonas = 0;

            Persona.Clear();
        }

    }
}
