using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP3SIM.Entidades.Estados;
using TP3SIM.Formularios;
using TP3SIM.Logica;

namespace TP3SIM.Entidades
{
    public class Simulacion
    {

        // Nuevos atributos globales

        readonly Servidor Empleado1 = new Servidor();
        readonly Servidor Empleado2 = new Servidor();

        // Atributos generales para simulaciones.

        private Fila fila1 = new Fila();
        private Fila fila2 = new Fila();

        public int FilaDesde { get; set; }
        public int FilaHasta { get; set; }
        public FormSimulacion FormularioSimulacion { get; set; }
        public HashSet<int> iteracionesGrilla { get; set; }
        public DataGridView Grilla { get; set; }

        readonly LogSimulacion log = new LogSimulacion();

        public int CantidadSimulaciones { get; set; }
        public int NumeroSimulacionActual { get; set; }

        // Atributos específicos para la simulación

        //private double CantidadClientesNP { get; set; }
        //private double CantidadClientesIM { get; set; }
        //private double CantidadClientesRP { get; set; }

        //private int DiaSimulacion { get; set; } = 1;
        //private double DescansoInicial { get; set; } = 180;
        //private double FinDia { get; set; }

        private List<Temporal> TodosLosClientes = new List<Temporal>();

        // Diccionarios para guardar estados inmutables de los servidores.

        readonly Dictionary<string, Libre> estadosLibre = new Dictionary<string, Libre>();
        readonly Dictionary<string, APedidoLibro> estadosAPedidoLibro = new Dictionary<string, APedidoLibro>();
        readonly Dictionary<string, ADevolucionLibro> estadosADevolucionLibro = new Dictionary<string, ADevolucionLibro>();
        readonly Dictionary<string, AConsulta> estadosAConsulta = new Dictionary<string, AConsulta>();

        // Estados inmutables para los clientes que se van generando.

        readonly SiendoAtendido SiendoAtendido = new SiendoAtendido();
        readonly EPedirLibro EPedirLibro = new EPedirLibro();
        readonly EDevolverLibro EDevolverLibro = new EDevolverLibro();
        readonly EConsultar EConsultar = new EConsultar();
        readonly EnBiblioteca EnBiblioteca = new EnBiblioteca();
        readonly Destruido Destruido = new Destruido();

        public void Simular()
        {

            // Obtener la grilla y prepararla (mejora rendimiento).

            DataGridView grilla = FormularioSimulacion.DevolverGrilla();
            PrepararGrilla(grilla);
            this.Grilla = grilla;

            // Obtener iteraciones a agregar en un HashSet para solo agregar en la grilla los valores deseados.

            iteracionesGrilla = IteracionesParaGrilla();

            // Inicializar el booleano de los servidores para que sepamos cual es cual.

            Empleado1.Empleado1 = true;
            Empleado1.Nombre = "Empleado 1";
            Empleado2.Empleado2 = true;
            Empleado2.Nombre = "Empleado 2";

            // Inicializar diccionarios de estados inmutables, son 4 estados por servidor.

            string[] nombresServidores = {"Empleado 1", "Empleado 2"};

            foreach (string nombre in nombresServidores)
            {
                Libre Libre = new Libre();
                APedidoLibro APedidoLibro = new APedidoLibro();
                ADevolucionLibro ADevolucionLibro = new ADevolucionLibro();
                AConsulta AConsulta = new AConsulta();

                estadosLibre.Add(nombre, Libre);
                estadosAPedidoLibro.Add(nombre, APedidoLibro);
                estadosADevolucionLibro.Add(nombre, ADevolucionLibro);
                estadosAConsulta.Add(nombre, AConsulta);
            }

            // Cargar valores e iniciar simulación.

            Inicializar();

            // Empezar simulación para filas sucesivas.

            CargarFilas(fila1, fila2, iteracionesGrilla);

            // Calcular métricas en base a los atributos generales.

            CalcularMetricasSimulacion();

            // Agregar las columnas de clientes.

            CargarColumnasClientes();

            // Mostrar formulario.

            MostrarFormulario(FormularioSimulacion, Grilla);
        }

        private string CalcularTipoAtencion(double random)
        {
            if (0.00 <= random && random <= 0.29) return "";

            if (0.30 <= random && random <= 0.59) return "";

            if (0.60 <= random && random <= 0.99) return "";

            return "";
        }

        private void Inicializar()
        {
            // Volver a las filas a los valores por defecto.

            fila1.Reiniciar();
            fila2.Reiniciar();

            // Inicializar valores de filas.

            fila1.Evento = "Inicialización";

            // Obtiene los randoms y las próximas llegadas para la fila de inicialización.

            fila1.RND_Llegada = log.GenerarRND();
            fila1.TiempoEntreLlegadas = log.VariableAleatoriaExponencial(4, fila1.RND_Llegada);
            fila1.ProximaLlegada = fila1.TiempoEntreLlegadas;

            // Obtiene el primer próximo reloj solo para mostrarlo en la grilla de otro color.

            //double proximoReloj = Math.Min(fila1.TiempoEntreLlegadas_LlegadaIM, Math.Min(fila1.TiempoEntreLlegadas_LlegadaNP, fila1.TiempoEntreLlegadas_LlegadaRP));
            double proximoReloj = fila1.TiempoEntreLlegadas;

            // Establece los estados a los servidores, los estados son inmutables.

            fila1.EstadoEmpleado_1 = estadosLibre["Empleado 1"];
            fila1.EstadoEmpleado_2 = estadosLibre["Empleado 2"];

            // Inicializar eventos

            //FinDia = 480;
            //DescansoInicial = 180;
            // TODO: Hacer que el descanso se arrastre al principio. fila1.FinDescanso = DescansoInicial;

            // Reiniciar la cantidad de clientes para que el numero (ID) de los clientes empiece de cero de nuevo.
            //CantidadClientesNP = 0;
            //CantidadClientesIM = 0;
            //CantidadClientesRP = 0;

            // Aumentar uno al numero de simulaciones (la inicialización se toma como una simulación).
            if (NumeroSimulacionActual > 0)
            {
                ++NumeroSimulacionActual;
            }

            // Crear un cliente temporal para la inicialización.

            Temporal clienteFalsoInicializacion = new Temporal();
            //clienteFalsoInicializacion.Tipo = "Inicializacion";
            clienteFalsoInicializacion.EnFilaNumero = NumeroSimulacionActual;
            TodosLosClientes.Add(clienteFalsoInicializacion);

            // Cargar inicialización solo si es la primera o alguna dentro de la selección del usuario.

            if (NumeroSimulacionActual == 0 || iteracionesGrilla.Contains(NumeroSimulacionActual))
            {
                AgregarFilaEnGrilla(fila1, proximoReloj);
            }
        }

        private (double, string) ObtenerProximoReloj()
        {
            // Crea una matriz entre todos los posibles eventos. Luego busca en esa matriz cual es el menor tiempo para saber qué evento sigue.

            object[,] posiblesProximoReloj =
                     {
                      { fila1.ProximaLlegada, "Llegada_Persona" },

                      { fila1.ProxFinAtencion_1, "FinAtención_1"},
                      { fila1.ProxFinAtencion_2, "FinAtención_2"},

                      { fila1.ProxFinLectura, "FinLectura" },
                    };

            double proximoReloj = (double)posiblesProximoReloj[0, 0];
            string evento = (string)posiblesProximoReloj[0, 1];

            for (int j = 1; j < posiblesProximoReloj.GetLength(0); j++)
            {
                double valorActual = (double)posiblesProximoReloj[j, 0];

                if (valorActual < proximoReloj && valorActual != 0)
                {
                    proximoReloj = valorActual;
                    evento = (string)posiblesProximoReloj[j, 1];
                }
            }

            return (proximoReloj, evento);
        }

        private void CargarFilas(Fila fila1, Fila fila2, HashSet<int> iteracionesGrilla)
        {

            // Suspender layout para mejorar rendimiento de grilla

            Grilla.SuspendLayout();

            // Cargar filas y actualizar la anterior.

            for (int i = 1; i <= CantidadSimulaciones; ++i)
            {

                ++NumeroSimulacionActual;

                // Obtener la tupla del próximo reloj y evento llamando a la función ObtenerProximoReloj().

                var resultadoProximoReloj = ObtenerProximoReloj();
                double proximoReloj = resultadoProximoReloj.Item1;
                string evento = resultadoProximoReloj.Item2;

                // En caso de ser el próximo reloj el valor '9999' significa que se debe cambiar de día.

                if (proximoReloj == 9999)
                {
                    //++DiaSimulacion;

                    // Generar una nueva fila de inicialización y obtener el próximo reloj.

                    Inicializar();

                    resultadoProximoReloj = ObtenerProximoReloj();
                    proximoReloj = resultadoProximoReloj.Item1;
                    evento = resultadoProximoReloj.Item2;
                }

                fila2.Evento = evento;
                fila2.Reloj = proximoReloj;

                switch (evento)
                {
                    case "Llegada_Persona":

                        // Generar cliente temporal

                        Temporal cliente = new Temporal();
                        //cliente.Numero = ++CantidadClientesIM;
                        cliente.HoraIngreso = fila2.Reloj;
                        //cliente.Tipo = "Interesado en matricula";
                        //cliente.TiempoEspera = 0;

                        // Obtener RNDs y Llegadas

                        fila2.RND_Llegada = log.GenerarRND();
                        fila2.TiempoEntreLlegadas = log.VariableAleatoriaExponencial(4, fila2.RND_Llegada);
                        fila2.ProximaLlegada = fila2.Reloj + fila2.TiempoEntreLlegadas;

                        // Arrastrar otros eventos

                        fila2.ProximaLlegada = fila1.ProximaLlegada;
                        fila2.ProximaLlegada = fila1.ProximaLlegada;

                        
      
                        fila2.Cola = fila1.Cola; 

                        //fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        //fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        //fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        if (fila1.Cola > 0)
                        {
                            // Si la cola es mayor que cero, implica que el servidor está ocupado y por ende se debe incrementar la cola.

                            ++fila2.Cola;

                            fila2.ProxFinAtencion_1 = fila1.ProxFinAtencion_1;
                            fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;

                            fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                            fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;

                            cliente.Estado = EsperandoAtencion;
                            fila2.Cliente.Add(clienteIM);
                            clienteIM.EnFilaNumero = NumeroSimulacionActual;
                            TodosLosClientes.Add(clienteIM.CopiarCliente(clienteIM));
                        }

                        if (fila1.Cola_IM == 0)
                        {

                            if (fila1.Estado_Tomas_FinAtencionIM.Libre)
                            {
                                fila2.RND_FinAtencionIM = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                fila2.FinAtencion_Tomas_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                fila2.Estado_Tomas_FinAtencionIM = estadosOcupado["Tomas"];

                                fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                fila2.Estado_Manuel = fila1.Estado_Manuel;
                                fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                clienteIM.Estado = SiendoAtendido;
                                clienteIM.SiendoAtendidoPor = Tomas;
                                fila2.ClienteIM.Add(clienteIM);
                                clienteIM.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(clienteIM.CopiarCliente(clienteIM));
                            }
                            else
                            {
                                if (fila1.Estado_Alicia_FinAtencionIM.Libre)
                                {
                                    fila2.RND_FinAtencionIM = log.GenerarRND();
                                    fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                    fila2.FinAtencion_Alicia_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                    fila2.Estado_Alicia_FinAtencionIM = estadosOcupado["Alicia"];

                                    fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                    fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                    fila2.Estado_Manuel = fila1.Estado_Manuel;
                                    fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                    clienteIM.Estado = SiendoAtendido;
                                    clienteIM.SiendoAtendidoPor = Alicia;
                                    clienteIM.EnFilaNumero = NumeroSimulacionActual;
                                    fila2.ClienteIM.Add(clienteIM);
                                    TodosLosClientes.Add(clienteIM.CopiarCliente(clienteIM));
                                }
                                else
                                {
                                    if (fila1.Estado_Manuel.Libre)
                                    {
                                        fila2.RND_FinAtencionIM = log.GenerarRND();
                                        fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                        fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                        fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                                        clienteIM.Estado = SiendoAtendido;
                                        clienteIM.SiendoAtendidoPor = Manuel;
                                        fila2.ClienteIM.Add(clienteIM);
                                        clienteIM.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(clienteIM.CopiarCliente(clienteIM));

                                    }
                                    else
                                    {
                                        ++fila2.Cola_IM;

                                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                        clienteIM.Estado = EsperandoAtencion;
                                        fila2.ClienteIM.Add(clienteIM);
                                        clienteIM.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(clienteIM.CopiarCliente(clienteIM));
                                    }
                                }
                            }
                        }

                        break;

                    case "Llegada" && :

                        Temporal clienteRP = new Temporal();
                        clienteRP.Numero = ++CantidadClientesRP;
                        clienteRP.HoraIngreso = fila2.Reloj;
                        clienteRP.Tipo = "Renueva permiso";
                        clienteRP.TiempoEspera = 0;

                        fila2.RND_LlegadaRP = log.GenerarRND();
                        fila2.TiempoEntreLlegadas_LlegadaRP = log.VariableAleatoriaExponencial(4.85, fila2.RND_LlegadaRP);
                        fila2.ProximaLlegada_LlegadaRP = fila2.Reloj + fila2.TiempoEntreLlegadas_LlegadaRP;

                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                        fila2.Cola_IM = fila1.Cola_IM;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        if (fila1.Cola_RP > 0)
                        {
                            ++fila2.Cola_RP;

                            fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;

                            clienteRP.Estado = EsperandoAtencion;
                            fila2.ClienteRP.Add(clienteRP);
                            clienteRP.EnFilaNumero = NumeroSimulacionActual;
                            TodosLosClientes.Add(clienteRP.CopiarCliente(clienteRP));
                        }

                        if (fila1.Cola_RP == 0)
                        {

                            if (fila1.Estado_Lucia_FinAtencionRP.Libre)
                            {
                                fila2.RND_FinAtencionRP = "Convolución";
                                fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                fila2.FinAtencion_Lucia_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                fila2.Estado_Lucia_FinAtencionRP = estadosOcupado["Lucia"];

                                fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                fila2.Estado_Manuel = fila1.Estado_Manuel;
                                fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                clienteRP.Estado = SiendoAtendido;
                                clienteRP.SiendoAtendidoPor = Lucia;
                                fila2.ClienteRP.Add(clienteRP);
                                clienteRP.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(clienteRP.CopiarCliente(clienteRP));
                            }
                            else
                            {
                                if (fila1.Estado_Maria_FinAtencionRP.Libre)
                                {
                                    fila2.RND_FinAtencionRP = "Convolución";
                                    fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                    fila2.FinAtencion_Maria_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                    fila2.Estado_Maria_FinAtencionRP = estadosOcupado["Maria"];

                                    fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                    fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                    fila2.Estado_Manuel = fila1.Estado_Manuel;
                                    fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                    clienteRP.Estado = SiendoAtendido;
                                    clienteRP.SiendoAtendidoPor = Maria;
                                    fila2.ClienteRP.Add(clienteRP);
                                    clienteRP.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(clienteRP.CopiarCliente(clienteRP));
                                }
                                else
                                {
                                    if (fila1.Estado_Manuel.Libre)
                                    {
                                        fila2.RND_FinAtencionRP = "Convolución";
                                        fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                        fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                        fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;

                                        clienteRP.Estado = SiendoAtendido;
                                        clienteRP.SiendoAtendidoPor = Manuel;
                                        fila2.ClienteRP.Add(clienteRP);
                                        clienteRP.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(clienteRP.CopiarCliente(clienteRP));

                                    }
                                    else
                                    {
                                        ++fila2.Cola_RP;

                                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                                        clienteRP.Estado = EsperandoAtencion;
                                        fila2.ClienteRP.Add(clienteRP);
                                        clienteRP.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(clienteRP.CopiarCliente(clienteRP));
                                    }
                                }
                            }
                        }

                        break;

                    case "Llegada_NP":

                        Temporal clienteNP = new Temporal();
                        clienteNP.Numero = ++CantidadClientesNP;
                        clienteNP.HoraIngreso = fila2.Reloj;
                        clienteNP.Tipo = "Nuevo permiso";
                        clienteNP.TiempoEspera = 0;

                        fila2.RND_LlegadaNP = log.GenerarRND();
                        fila2.TiempoEntreLlegadas_LlegadaNP = log.VariableAleatoriaExponencial(20, fila2.RND_LlegadaNP);
                        fila2.ProximaLlegada_LlegadaNP = fila2.Reloj + fila2.TiempoEntreLlegadas_LlegadaNP;

                        fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;
                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                        fila2.Cola_IM = fila1.Cola_IM;

                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                        fila2.Cola_RP = fila1.Cola_RP;

                        fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        if (fila1.Cola_NP > 0)
                        {
                            ++fila2.Cola_NP;

                            fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;

                            clienteNP.Estado = EsperandoAtencion;
                            fila2.ClienteNP.Add(clienteNP);
                            clienteNP.EnFilaNumero = NumeroSimulacionActual;
                            TodosLosClientes.Add(clienteNP.CopiarCliente(clienteNP));
                        }

                        if (fila1.Cola_NP == 0)
                        {

                            if (fila1.Estado_Dario_FinAtencionNP.Libre)
                            {
                                fila2.RND_FinAtencionNP = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionNP = log.VariableAleatoriaUniforme(15, 20, fila2.RND_FinAtencionNP);
                                fila2.FinAtencion_Dario_FinAtencionNP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionNP;

                                fila2.Estado_Dario_FinAtencionNP = estadosOcupado["Dario"];

                                clienteNP.Estado = SiendoAtendido;
                                clienteNP.SiendoAtendidoPor = Dario;
                                fila2.ClienteNP.Add(clienteNP);
                                clienteNP.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(clienteNP.CopiarCliente(clienteNP));
                            }
                            else
                            {
                                ++fila2.Cola_NP;

                                fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                                fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;

                                clienteNP.Estado = EsperandoAtencion;
                                fila2.ClienteNP.Add(clienteNP);
                                clienteNP.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(clienteNP.CopiarCliente(clienteNP));
                            }
                        }

                        break;

                    case "FinAtención_IM_Tomas":

                        if (fila2.Cola_IM > 0 && !fila2.Estado_Tomas_FinAtencionIM.OcupadoEsperandoDescanso)
                        {

                            fila2.RND_FinAtencionIM = log.GenerarRND();
                            fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                            fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;

                            --fila2.Cola_IM;

                            foreach (Temporal cliente in fila2.ClienteIM)
                            {
                                if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                {
                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Tomas;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (fila2.Estado_Tomas_FinAtencionIM.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Tomas_FinAtencionIM = estadosDescansando["Tomas"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Tomas_FinAtencionIM = 0;
                            }
                            else
                            {
                                fila2.Estado_Tomas_FinAtencionIM = estadosLibre["Tomas"];

                                fila2.FinAtencion_Tomas_FinAtencionIM = 0;
                            }
                        }

                        double horaInicioDestruidoTomas = 0;

                        foreach (Temporal cliente in fila2.ClienteIM)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Tomas)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoTomas = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                break;
                            }
                        }

                        fila2.CantidadClientes_IM = ++fila1.CantidadClientes_IM;
                        fila2.AcumuladoTiempoAtencion_IM = fila1.AcumuladoTiempoAtencion_IM + (fila2.Reloj - horaInicioDestruidoTomas);

                        fila2.MaximoTiempoAtencion_IM = ((fila2.Reloj - horaInicioDestruidoTomas) < fila1.MaximoTiempoAtencion_IM) ?
                            fila1.MaximoTiempoAtencion_IM : (fila2.Reloj - horaInicioDestruidoTomas);

                        // Arrastrar valores que no se utilizan.

                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;

                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        break;

                    case "FinAtención_IM_Alicia":

                        if (fila2.Cola_IM > 0 && !fila2.Estado_Alicia_FinAtencionIM.OcupadoEsperandoDescanso)
                        {

                            fila2.RND_FinAtencionIM = log.GenerarRND();
                            fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                            fila2.FinAtencion_Alicia_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                            fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;

                            --fila2.Cola_IM;

                            foreach (Temporal cliente in fila2.ClienteIM)
                            {
                                if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                {
                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Alicia;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (fila2.Estado_Alicia_FinAtencionIM.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Alicia_FinAtencionIM = estadosDescansando["Alicia"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Alicia_FinAtencionIM = 0;
                            }
                            else
                            {
                                fila2.Estado_Alicia_FinAtencionIM = estadosLibre["Alicia"];

                                fila2.FinAtencion_Alicia_FinAtencionIM = 0;
                            }
                        }

                        double horaInicioDestruidoAlicia = 0;

                        foreach (Temporal cliente in fila2.ClienteIM)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Alicia)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoAlicia = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                break;
                            }
                        }

                        fila2.CantidadClientes_IM = ++fila1.CantidadClientes_IM;
                        fila2.AcumuladoTiempoAtencion_IM = fila1.AcumuladoTiempoAtencion_IM + (fila2.Reloj - horaInicioDestruidoAlicia);

                        fila2.MaximoTiempoAtencion_IM = ((fila2.Reloj - horaInicioDestruidoAlicia) < fila1.MaximoTiempoAtencion_IM) ?
                            fila1.MaximoTiempoAtencion_IM : (fila2.Reloj - horaInicioDestruidoAlicia);

                        // Arrastrar valores que no se utilizan.

                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;

                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        break;

                    case "FinAtención_RP_Lucia":

                        if (fila2.Cola_RP > 0 && !fila2.Estado_Lucia_FinAtencionRP.OcupadoEsperandoDescanso)
                        {

                            fila2.RND_FinAtencionRP = "Convolución";
                            fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                            fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;

                            --fila2.Cola_RP;

                            foreach (Temporal cliente in fila2.ClienteRP)
                            {
                                if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                {
                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Lucia;
                                    cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (fila2.Estado_Lucia_FinAtencionRP.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Lucia_FinAtencionRP = estadosDescansando["Lucia"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Lucia_FinAtencionRP = 0;
                            }
                            else
                            {
                                fila2.Estado_Lucia_FinAtencionRP = estadosLibre["Lucia"];

                                fila2.FinAtencion_Lucia_FinAtencionRP = 0;
                            }
                        }

                        double horaInicioDestruidoLucia = 0;

                        foreach (Temporal cliente in fila2.ClienteRP)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Lucia)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoLucia = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                break;
                            }
                        }

                        fila2.CantidadClientes_RP = ++fila1.CantidadClientes_RP;
                        fila2.AcumuladoTiempoAtencion_RP = fila1.AcumuladoTiempoAtencion_RP + (fila2.Reloj - horaInicioDestruidoLucia);

                        fila2.MaximoTiempoAtencion_RP = ((fila2.Reloj - horaInicioDestruidoLucia) < fila1.MaximoTiempoAtencion_RP) ?
                            fila1.MaximoTiempoAtencion_RP : (fila2.Reloj - horaInicioDestruidoLucia);

                        // Arrastrar valores que no se utilizan.

                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        break;

                    case "FinAtención_RP_Maria":

                        if (fila2.Cola_RP > 0 && !fila2.Estado_Maria_FinAtencionRP.OcupadoEsperandoDescanso)
                        {

                            fila2.RND_FinAtencionRP = "Convolución";
                            fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                            fila2.FinAtencion_Maria_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                            fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;

                            --fila2.Cola_RP;

                            foreach (Temporal cliente in fila2.ClienteRP)
                            {
                                if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                {
                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Maria;
                                    cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (fila2.Estado_Maria_FinAtencionRP.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosDescansando["Maria"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Maria_FinAtencionRP = 0;
                            }
                            else
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosLibre["Maria"];

                                fila2.FinAtencion_Maria_FinAtencionRP = 0;
                            }
                        }

                        double horaInicioDestruidoMaria = 0;

                        foreach (Temporal cliente in fila2.ClienteRP)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Maria)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoMaria = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                break;
                            }
                        }

                        fila2.CantidadClientes_RP = ++fila1.CantidadClientes_RP;
                        fila2.AcumuladoTiempoAtencion_RP = fila1.AcumuladoTiempoAtencion_RP + (fila2.Reloj - horaInicioDestruidoMaria);

                        fila2.MaximoTiempoAtencion_RP = ((fila2.Reloj - horaInicioDestruidoMaria) < fila1.MaximoTiempoAtencion_RP) ?
                            fila1.MaximoTiempoAtencion_RP : (fila2.Reloj - horaInicioDestruidoMaria);

                        // Arrastrar valores que no se utilizan.

                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        break;

                    case "FinAtención_Manuel":

                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;
                        fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                        fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;

                        fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.Cola_NP = fila1.Cola_NP;

                        double horaInicioDestruidoManuel = 0;
                        bool clienteEncontrado = false;
                        string tipoClienteADestruir = "";

                        foreach (Temporal cliente in fila2.ClienteIM)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Manuel)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoManuel = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                tipoClienteADestruir = cliente.Tipo;
                                clienteEncontrado = true;
                                break;
                            }
                        }

                        if (!clienteEncontrado)
                        {
                            foreach (Temporal cliente in fila2.ClienteRP)
                            {
                                if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Manuel)
                                {
                                    cliente.Estado = Destruido;
                                    horaInicioDestruidoManuel = cliente.HoraIngreso;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                    tipoClienteADestruir = cliente.Tipo;
                                    break;
                                }
                            }
                        }

                        if (tipoClienteADestruir == "Interesado en matricula")
                        {
                            fila2.CantidadClientes_IM = ++fila1.CantidadClientes_IM;
                            fila2.AcumuladoTiempoAtencion_IM = fila1.AcumuladoTiempoAtencion_IM + (fila2.Reloj - horaInicioDestruidoManuel);

                            fila2.MaximoTiempoAtencion_IM = ((fila2.Reloj - horaInicioDestruidoManuel) < fila1.MaximoTiempoAtencion_IM) ?
                                fila1.MaximoTiempoAtencion_IM : (fila2.Reloj - horaInicioDestruidoManuel);
                        }
                        else
                        {
                            fila2.CantidadClientes_RP = ++fila1.CantidadClientes_RP;
                            fila2.AcumuladoTiempoAtencion_RP = fila1.AcumuladoTiempoAtencion_RP + (fila2.Reloj - horaInicioDestruidoManuel);

                            fila2.MaximoTiempoAtencion_RP = ((fila2.Reloj - horaInicioDestruidoManuel) < fila1.MaximoTiempoAtencion_RP) ?
                                fila1.MaximoTiempoAtencion_RP : (fila2.Reloj - horaInicioDestruidoManuel);
                        }

                        if ((fila2.Cola_IM > 0 || fila2.Cola_RP > 0) && !fila2.Estado_Manuel.OcupadoEsperandoDescanso)
                        {
                            Temporal clienteMayorIM = fila2.ClienteIM.Where(c => c.Estado.EsperandoAtencion)
                                         .OrderByDescending(c => c.TiempoEspera)
                                         .FirstOrDefault();

                            Temporal clienteMayorRP = fila2.ClienteRP.Where(c => c.Estado.EsperandoAtencion)
                                                                     .OrderByDescending(c => c.TiempoEspera)
                                                                     .FirstOrDefault();

                            Temporal clienteMayor = null;

                            if (clienteMayorIM == null || clienteMayorRP == null)
                            {
                                if (clienteMayorRP != null) { clienteMayor = clienteMayorRP; }
                                else { clienteMayor = clienteMayorIM; }
                            }
                            else
                            {
                                clienteMayor = (clienteMayorIM.TiempoEspera > clienteMayorRP.TiempoEspera) ? clienteMayorIM : clienteMayorRP;
                            }

                            if (clienteMayor.Tipo == "Interesado en matricula")
                            {
                                --fila2.Cola_IM;
                                fila2.Cola_RP = fila1.Cola_RP;

                                fila2.RND_FinAtencionIM = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                clienteMayor.Estado = SiendoAtendido;
                                clienteMayor.SiendoAtendidoPor = Manuel;
                                clienteMayor.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(clienteMayor.CopiarCliente(clienteMayor));

                                fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            }
                            else
                            {
                                if (clienteMayor.Tipo == "Renueva permiso")
                                {
                                    --fila2.Cola_RP;
                                    fila2.Cola_IM = fila1.Cola_IM;

                                    fila2.RND_FinAtencionRP = "Convolución";
                                    fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                    fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                    clienteMayor.Estado = SiendoAtendido;
                                    clienteMayor.SiendoAtendidoPor = Manuel;
                                    clienteMayor.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(clienteMayor.CopiarCliente(clienteMayor));

                                    fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                    fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                    fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                    fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                    fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                }
                                else
                                {
                                    fila2.Estado_Manuel = estadosLibre["Manuel"];

                                    fila2.Cola_IM = fila1.Cola_IM;
                                    fila2.Cola_RP = fila1.Cola_RP;

                                    fila2.FinAtencion_Manuel = 0;

                                    fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                    fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                    fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                    fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                    fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                    fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                    fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                    fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                }

                            }
                        }
                        else
                        {
                            if (fila2.Estado_Manuel.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Manuel = estadosDescansando["Manuel"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Manuel = 0;
                            }
                            else
                            {
                                fila2.Estado_Manuel = estadosLibre["Manuel"];

                                fila2.Cola_IM = fila1.Cola_IM;
                                fila2.Cola_RP = fila1.Cola_RP;

                                fila2.FinAtencion_Manuel = 0;

                                fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                                fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            }
                        }

                        break;

                    case "FinAtención_NP_Dario":

                        if (fila2.Cola_NP > 0 && !fila2.Estado_Dario_FinAtencionNP.OcupadoEsperandoDescanso)
                        {

                            fila2.RND_FinAtencionNP = log.GenerarRND();
                            fila2.TiempoAtencion_FinAtencionNP = log.VariableAleatoriaUniforme(15, 20, fila2.RND_FinAtencionNP);
                            fila2.FinAtencion_Dario_FinAtencionNP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionNP;

                            fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;

                            --fila2.Cola_NP;

                            foreach (Temporal cliente in fila2.ClienteNP)
                            {
                                if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                {
                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Dario;
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (fila2.Estado_Dario_FinAtencionNP.OcupadoEsperandoDescanso)
                            {
                                fila2.Estado_Dario_FinAtencionNP = estadosDescansando["Dario"];

                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;

                                fila2.FinAtencion_Dario_FinAtencionNP = 0;
                            }
                            else
                            {
                                fila2.Estado_Dario_FinAtencionNP = estadosLibre["Dario"];

                                fila2.FinAtencion_Dario_FinAtencionNP = 0;
                            }
                        }

                        double horaInicioDestruidoDario = 0;

                        foreach (Temporal cliente in fila2.ClienteNP)
                        {
                            if (cliente.Estado.SiendoAtendido && cliente.SiendoAtendidoPor == Dario)
                            {
                                cliente.Estado = Destruido;
                                horaInicioDestruidoDario = cliente.HoraIngreso;
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.DestruirCliente(cliente));
                                break;
                            }
                        }

                        fila2.CantidadClientes_NP = ++fila1.CantidadClientes_NP;
                        fila2.AcumuladoTiempoAtencion_NP = fila1.AcumuladoTiempoAtencion_NP + (fila2.Reloj - horaInicioDestruidoDario);

                        fila2.MaximoTiempoAtencion_NP = ((fila2.Reloj - horaInicioDestruidoDario) < fila1.MaximoTiempoAtencion_NP) ?
                            fila1.MaximoTiempoAtencion_NP : (fila2.Reloj - horaInicioDestruidoDario);

                        // Arrastrar valores que no se utilizan.

                        fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;
                        fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                        fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;

                        fila2.Estado_Manuel = fila1.Estado_Manuel;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                        fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;

                        break;

                    case "Llegada_Descanso":

                        DescansoInicial = 0;

                        if (fila2.Estado_Tomas_FinAtencionIM.Libre)
                        {
                            fila2.Estado_Tomas_FinAtencionIM = estadosDescansando["Tomas"];
                        }
                        else
                        {
                            fila2.Estado_Tomas_FinAtencionIM = estadosOcupadoEsperandoDescanso["Tomas"];
                        }

                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                        Temporal clienteFalsoDescanso = new Temporal();
                        clienteFalsoDescanso.Tipo = "Descanso";
                        clienteFalsoDescanso.EnFilaNumero = NumeroSimulacionActual;
                        TodosLosClientes.Add(clienteFalsoDescanso);

                        break;

                    case "Fin_Descanso":

                        // Reiniciar el evento de fin de atención para que no vuelva a ser elegido.

                        fila2.FinDescanso = 0;

                        if (fila2.Estado_Tomas_FinAtencionIM.Descansando)
                        {

                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Tomas";

                            // Ponerle el estado ocupado esperando descanso a la siguiente.

                            if (fila2.Estado_Lucia_FinAtencionRP.Libre)
                            {
                                fila2.Estado_Lucia_FinAtencionRP = estadosDescansando["Lucia"];
                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;
                            }
                            else
                            {
                                fila2.Estado_Lucia_FinAtencionRP = estadosOcupadoEsperandoDescanso["Lucia"];
                            }

                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                            // Actuar igual que un fin de atención

                            if (fila2.Cola_IM > 0)
                            {
                                // Cambiar estado a ocupado.

                                fila2.Estado_Tomas_FinAtencionIM = estadosOcupado["Tomas"];

                                fila2.RND_FinAtencionIM = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                fila2.FinAtencion_Tomas_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                --fila2.Cola_IM;

                                foreach (Temporal cliente in fila2.ClienteIM)
                                {
                                    if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                    {
                                        cliente.Estado = SiendoAtendido;
                                        cliente.SiendoAtendidoPor = Tomas;
                                        cliente.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                fila2.Estado_Tomas_FinAtencionIM = estadosLibre["Tomas"];

                                fila2.FinAtencion_Tomas_FinAtencionIM = 0;
                            }

                            // Arrastrar valores que no se utilizan.

                            fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                            fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;

                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;

                            fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.Cola_NP = fila1.Cola_NP;

                            fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.Estado_Manuel = fila1.Estado_Manuel;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                            fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                            fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;
                        }
                        else if (fila2.Estado_Lucia_FinAtencionRP.Descansando)
                        {
                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Lucia";

                            // Ponerle el estado ocupado esperando descanso a la siguiente.

                            if (fila2.Estado_Manuel.Libre)
                            {
                                fila2.Estado_Manuel = estadosDescansando["Manuel"];
                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;
                            }
                            else
                            {
                                fila2.Estado_Manuel = estadosOcupadoEsperandoDescanso["Manuel"];
                            }

                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                            if (fila2.Cola_RP > 0)
                            {

                                // Cambiar estado a ocupado.

                                fila2.Estado_Lucia_FinAtencionRP = estadosOcupado["Lucia"];

                                fila2.RND_FinAtencionRP = "Convolución";
                                fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                fila2.FinAtencion_Lucia_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                --fila2.Cola_RP;

                                foreach (Temporal cliente in fila2.ClienteRP)
                                {
                                    if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                    {
                                        cliente.Estado = SiendoAtendido;
                                        cliente.SiendoAtendidoPor = Lucia;
                                        cliente.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                fila2.Estado_Lucia_FinAtencionRP = estadosLibre["Lucia"];

                                fila2.FinAtencion_Lucia_FinAtencionRP = 0;
                            }

                            // Arrastrar valores que no se utilizan.

                            fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                            fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                            fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                            fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                            fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.Cola_NP = fila1.Cola_NP;

                            fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                            fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                            fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        }
                        else if (fila2.Estado_Manuel.Descansando)
                        {
                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Manuel";

                            // Ponerle el estado ocupado esperando descanso a la siguiente.

                            if (fila2.Estado_Alicia_FinAtencionIM.Libre)
                            {
                                fila2.Estado_Alicia_FinAtencionIM = estadosDescansando["Alicia"];
                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;
                            }
                            else
                            {
                                fila2.Estado_Alicia_FinAtencionIM = estadosOcupadoEsperandoDescanso["Alicia"];
                            }

                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                            if ((fila2.Cola_IM > 0 || fila2.Cola_RP > 0))
                            {
                                // Cambiar estado a ocupado.

                                fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                Temporal clienteMayorIM = fila2.ClienteIM.Where(c => c.Estado.EsperandoAtencion)
                                             .OrderByDescending(c => c.TiempoEspera)
                                             .FirstOrDefault();

                                Temporal clienteMayorRP = fila2.ClienteRP.Where(c => c.Estado.EsperandoAtencion)
                                                                         .OrderByDescending(c => c.TiempoEspera)
                                                                         .FirstOrDefault();

                                Temporal clienteMayor = null;

                                if (clienteMayorIM == null || clienteMayorRP == null)
                                {
                                    if (clienteMayorRP != null) { clienteMayor = clienteMayorRP; }
                                    else { clienteMayor = clienteMayorIM; }
                                }
                                else
                                {
                                    clienteMayor = (clienteMayorIM.TiempoEspera > clienteMayorRP.TiempoEspera) ? clienteMayorIM : clienteMayorRP;
                                }

                                if (clienteMayor.Tipo == "Interesado en matricula")
                                {
                                    --fila2.Cola_IM;
                                    fila2.Cola_RP = fila1.Cola_RP;

                                    fila2.RND_FinAtencionIM = log.GenerarRND();
                                    fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                    fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                    clienteMayor.Estado = SiendoAtendido;
                                    clienteMayor.SiendoAtendidoPor = Manuel;
                                    clienteMayor.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(clienteMayor.CopiarCliente(clienteMayor));

                                    fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                    fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                    fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                    fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                }
                                else
                                {
                                    if (clienteMayor.Tipo == "Renueva permiso")
                                    {
                                        --fila2.Cola_RP;
                                        fila2.Cola_IM = fila1.Cola_IM;

                                        fila2.RND_FinAtencionRP = "Convolución";
                                        fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                        fila2.FinAtencion_Manuel = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                        clienteMayor.Estado = SiendoAtendido;
                                        clienteMayor.SiendoAtendidoPor = Manuel;
                                        clienteMayor.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(clienteMayor.CopiarCliente(clienteMayor));

                                        fila2.Estado_Manuel = estadosOcupado["Manuel"];

                                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                    }
                                    else
                                    {
                                        fila2.Estado_Manuel = estadosLibre["Manuel"];

                                        fila2.Cola_IM = fila1.Cola_IM;
                                        fila2.Cola_RP = fila1.Cola_RP;

                                        fila2.FinAtencion_Manuel = 0;

                                        fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                        fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                        fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                                    }

                                }
                            }
                            else
                            {
                                fila2.Estado_Manuel = estadosLibre["Manuel"];
                                fila2.Cola_IM = fila1.Cola_IM;
                                fila2.Cola_RP = fila1.Cola_RP;

                                fila2.FinAtencion_Manuel = 0;

                                fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                                fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                                fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                                fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                                fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                                fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                                fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            }
                        }
                        else if (fila2.Estado_Alicia_FinAtencionIM.Descansando)
                        {
                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Alicia";

                            // Ponerle el estado ocupado esperando descanso a la siguiente.

                            if (fila2.Estado_Maria_FinAtencionRP.Libre)
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosDescansando["Maria"];
                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;
                            }
                            else
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosOcupadoEsperandoDescanso["Maria"];
                            }

                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                            if (fila2.Cola_IM > 0)
                            {

                                fila2.Estado_Alicia_FinAtencionIM = estadosOcupado["Alicia"];

                                fila2.RND_FinAtencionIM = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionIM = log.VariableAleatoriaUniforme(8.7, 15.2, fila2.RND_FinAtencionIM);
                                fila2.FinAtencion_Alicia_FinAtencionIM = fila2.Reloj + fila2.TiempoAtencion_FinAtencionIM;

                                --fila2.Cola_IM;

                                foreach (Temporal cliente in fila2.ClienteIM)
                                {
                                    if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                    {
                                        cliente.Estado = SiendoAtendido;
                                        cliente.SiendoAtendidoPor = Alicia;
                                        cliente.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                fila2.Estado_Alicia_FinAtencionIM = estadosLibre["Alicia"];

                                fila2.FinAtencion_Alicia_FinAtencionIM = 0;
                            }

                            // Arrastrar valores que no se utilizan.

                            fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                            fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;

                            fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;

                            fila2.Estado_Dario_FinAtencionNP = fila1.Estado_Dario_FinAtencionNP;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.Cola_NP = fila1.Cola_NP;

                            fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                            fila2.Estado_Manuel = fila1.Estado_Manuel;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                            fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                            fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;
                        }
                        else if (fila2.Estado_Maria_FinAtencionRP.Descansando)
                        {
                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Maria";

                            // Ponerle el estado ocupado esperando descanso a la siguiente.

                            if (fila2.Estado_Dario_FinAtencionNP.Libre)
                            {
                                fila2.Estado_Dario_FinAtencionNP = estadosDescansando["Dario"];
                                fila2.FinDescanso = fila2.Reloj + fila2.TiempoDescanso;
                            }
                            else
                            {
                                fila2.Estado_Dario_FinAtencionNP = estadosOcupadoEsperandoDescanso["Dario"];
                            }

                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                            if (fila2.Cola_RP > 0)
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosOcupado["Maria"];

                                fila2.RND_FinAtencionRP = "Convolución";
                                fila2.TiempoAtencion_FinAtencionRP = log.VariableAleatoriaConvolucion(19.7, 5);
                                fila2.FinAtencion_Maria_FinAtencionRP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionRP;

                                --fila2.Cola_RP;

                                foreach (Temporal cliente in fila2.ClienteRP)
                                {
                                    if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                    {
                                        cliente.Estado = SiendoAtendido;
                                        cliente.SiendoAtendidoPor = Maria;
                                        cliente.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                fila2.Estado_Maria_FinAtencionRP = estadosLibre["Maria"];

                                fila2.FinAtencion_Maria_FinAtencionRP = 0;
                            }

                            // Arrastrar valores que no se utilizan.

                            fila2.ProximaLlegada_LlegadaNP = fila1.ProximaLlegada_LlegadaNP;
                            fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                            fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                            fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                            fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                            fila2.Cola_NP = fila1.Cola_NP;

                            fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                            fila2.Estado_Manuel = fila1.Estado_Manuel;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                            fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                            fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;
                        }
                        else if (fila2.Estado_Dario_FinAtencionNP.Descansando)
                        {
                            // Cambiar evento especifico.

                            fila2.Evento = "Fin_Descanso_Dario";

                            // Reiniciar descanso por ser el último en tener descanso.

                            fila2.TiempoDescanso = 0;

                            if (fila2.Cola_NP > 0)
                            {

                                fila2.Estado_Dario_FinAtencionNP = estadosOcupado["Dario"];

                                fila2.RND_FinAtencionNP = log.GenerarRND();
                                fila2.TiempoAtencion_FinAtencionNP = log.VariableAleatoriaUniforme(15, 20, fila2.RND_FinAtencionNP);
                                fila2.FinAtencion_Dario_FinAtencionNP = fila2.Reloj + fila2.TiempoAtencion_FinAtencionNP;

                                --fila2.Cola_NP;

                                foreach (Temporal cliente in fila2.ClienteNP)
                                {
                                    if (cliente.Estado.EsperandoAtencion && cliente.SiendoAtendidoPor == null)
                                    {
                                        cliente.Estado = SiendoAtendido;
                                        cliente.SiendoAtendidoPor = Dario;
                                        cliente.EnFilaNumero = NumeroSimulacionActual;
                                        TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                fila2.Estado_Dario_FinAtencionNP = estadosLibre["Dario"];

                                fila2.FinAtencion_Dario_FinAtencionNP = 0;
                            }

                            // Arrastrar valores que no se utilizan.

                            fila2.ProximaLlegada_LlegadaRP = fila1.ProximaLlegada_LlegadaRP;
                            fila2.ProximaLlegada_LlegadaIM = fila1.ProximaLlegada_LlegadaIM;

                            fila2.Estado_Tomas_FinAtencionIM = fila1.Estado_Tomas_FinAtencionIM;
                            fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;
                            fila2.Estado_Alicia_FinAtencionIM = fila1.Estado_Alicia_FinAtencionIM;
                            fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;

                            fila2.Estado_Maria_FinAtencionRP = fila1.Estado_Maria_FinAtencionRP;
                            fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                            fila2.Estado_Lucia_FinAtencionRP = fila1.Estado_Lucia_FinAtencionRP;
                            fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;

                            fila2.Estado_Manuel = fila1.Estado_Manuel;
                            fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;

                            fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                            fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        }

                        break;

                    case "Fin_Dia":

                        FinDia = 0;

                        fila2.ProximaLlegada_LlegadaIM = 9999;
                        fila2.ProximaLlegada_LlegadaNP = 9999;
                        fila2.ProximaLlegada_LlegadaRP = 9999;

                        fila2.FinAtencion_Alicia_FinAtencionIM = fila1.FinAtencion_Alicia_FinAtencionIM;
                        fila2.FinAtencion_Dario_FinAtencionNP = fila1.FinAtencion_Dario_FinAtencionNP;
                        fila2.FinAtencion_Lucia_FinAtencionRP = fila1.FinAtencion_Lucia_FinAtencionRP;
                        fila2.FinAtencion_Manuel = fila1.FinAtencion_Manuel;
                        fila2.FinAtencion_Maria_FinAtencionRP = fila1.FinAtencion_Maria_FinAtencionRP;
                        fila2.FinAtencion_Tomas_FinAtencionIM = fila1.FinAtencion_Tomas_FinAtencionIM;

                        Temporal clienteFalsoFinDia = new Temporal();
                        clienteFalsoFinDia.Tipo = "FinDia";
                        clienteFalsoFinDia.EnFilaNumero = NumeroSimulacionActual;
                        TodosLosClientes.Add(clienteFalsoFinDia);

                        break;
                }

                foreach (Temporal cliente in fila2.ClienteIM)
                {
                    if (cliente.Estado.EsperandoAtencion)
                    {
                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                    }
                }

                foreach (Temporal cliente in fila2.ClienteRP)
                {
                    if (cliente.Estado.EsperandoAtencion)
                    {
                        cliente.TiempoEspera = fila2.Reloj - cliente.HoraIngreso;
                    }
                }

                // La fila anterior pasa a tener los nuevos valores para repetir el proceso.

                fila1.Evento = fila2.Evento;
                fila1.Reloj = fila2.Reloj;
                fila1.RND_LlegadaIM = fila2.RND_LlegadaIM;
                fila1.TiempoEntreLlegadas_LlegadaIM = fila2.TiempoEntreLlegadas_LlegadaIM;
                fila1.ProximaLlegada_LlegadaIM = fila2.ProximaLlegada_LlegadaIM;
                fila1.RND_LlegadaRP = fila2.RND_LlegadaRP;
                fila1.TiempoEntreLlegadas_LlegadaRP = fila2.TiempoEntreLlegadas_LlegadaRP;
                fila1.ProximaLlegada_LlegadaRP = fila2.ProximaLlegada_LlegadaRP;
                fila1.RND_LlegadaNP = fila2.RND_LlegadaNP;
                fila1.TiempoEntreLlegadas_LlegadaNP = fila2.TiempoEntreLlegadas_LlegadaNP;
                fila1.ProximaLlegada_LlegadaNP = fila2.ProximaLlegada_LlegadaNP;
                fila1.RND_FinAtencionIM = fila2.RND_FinAtencionIM;
                fila1.TiempoAtencion_FinAtencionIM = fila2.TiempoAtencion_FinAtencionIM;
                fila1.FinAtencion_Tomas_FinAtencionIM = fila2.FinAtencion_Tomas_FinAtencionIM;
                fila1.Estado_Tomas_FinAtencionIM = fila2.Estado_Tomas_FinAtencionIM;
                fila1.FinAtencion_Alicia_FinAtencionIM = fila2.FinAtencion_Alicia_FinAtencionIM;
                fila1.Estado_Alicia_FinAtencionIM = fila2.Estado_Alicia_FinAtencionIM;
                fila1.Cola_IM = fila2.Cola_IM;
                fila1.RND_FinAtencionRP = fila2.RND_FinAtencionRP;
                fila1.TiempoAtencion_FinAtencionRP = fila2.TiempoAtencion_FinAtencionRP;
                fila1.FinAtencion_Lucia_FinAtencionRP = fila2.FinAtencion_Lucia_FinAtencionRP;
                fila1.Estado_Lucia_FinAtencionRP = fila2.Estado_Lucia_FinAtencionRP;
                fila1.FinAtencion_Maria_FinAtencionRP = fila2.FinAtencion_Maria_FinAtencionRP;
                fila1.Estado_Maria_FinAtencionRP = fila2.Estado_Maria_FinAtencionRP;
                fila1.Cola_RP = fila2.Cola_RP;
                fila1.FinAtencion_Manuel = fila2.FinAtencion_Manuel;
                fila1.Estado_Manuel = fila2.Estado_Manuel;
                fila1.RND_FinAtencionNP = fila2.RND_FinAtencionNP;
                fila1.TiempoAtencion_FinAtencionNP = fila2.TiempoAtencion_FinAtencionNP;
                fila1.FinAtencion_Dario_FinAtencionNP = fila2.FinAtencion_Dario_FinAtencionNP;
                fila1.Estado_Dario_FinAtencionNP = fila2.Estado_Dario_FinAtencionNP;
                fila1.Cola_NP = fila2.Cola_NP;
                fila1.TiempoDescanso = fila2.TiempoDescanso;
                fila1.FinDescanso = fila2.FinDescanso;
                fila1.CantidadClientes_IM = fila2.CantidadClientes_IM;
                fila1.CantidadClientes_RP = fila2.CantidadClientes_RP;
                fila1.CantidadClientes_NP = fila2.CantidadClientes_NP;
                fila1.AcumuladoTiempoAtencion_IM = fila2.AcumuladoTiempoAtencion_IM;
                fila1.AcumuladoTiempoAtencion_RP = fila2.AcumuladoTiempoAtencion_RP;
                fila1.AcumuladoTiempoAtencion_NP = fila2.AcumuladoTiempoAtencion_NP;
                fila1.MaximoTiempoAtencion_IM = fila2.MaximoTiempoAtencion_IM;
                fila1.MaximoTiempoAtencion_RP = fila2.MaximoTiempoAtencion_RP;
                fila1.MaximoTiempoAtencion_NP = fila2.MaximoTiempoAtencion_NP;
                fila1.ClienteIM = fila2.ClienteIM;
                fila1.ClienteNP = fila2.ClienteNP;
                fila1.ClienteRP = fila2.ClienteRP;

                // Carga la fila en la grilla. Revisa que sólo se cargue lo seleccionado.

                if (iteracionesGrilla.Contains(i))
                {
                    AgregarFilaEnGrilla(fila2, proximoReloj);
                }
            }

            // Reactivar layout de la grilla al finalizar de actualizar.

            Grilla.ResumeLayout(false);
        }

        private HashSet<int> IteracionesParaGrilla()
        {
            // Agrega a un HashSet los valores de iteraciones que la grilla debería mostrar
            // (la inicialización que se añade arriba, valores desde y hasta del usuario y el último).

            HashSet<int> iteracionesGrilla = new HashSet<int>();

            for (int i = FilaDesde; i <= FilaHasta; ++i)
            {
                iteracionesGrilla.Add(i);
            }

            iteracionesGrilla.Add(CantidadSimulaciones);

            //MessageBox.Show(string.Join(", ", iteracionesGrilla));

            return iteracionesGrilla;
        }

        private void PrepararGrilla(DataGridView grilla)
        {
            // Mejorar el rendimiento de la grilla.

            grilla.Rows.Clear();
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grilla.RowHeadersVisible = false;
            grilla.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(grilla, true, null);
            grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        }

        private void MostrarFormulario(Form formulario, DataGridView grilla)
        {
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grilla.RowHeadersVisible = true;
            formulario.Show();
        }

        private void AgregarFilaEnGrilla(Fila fila, double proximoReloj)
        {
            string reloj = Math.Round(proximoReloj, 2).ToString();

            int indiceFila = Grilla.Rows.Add
                (
                fila.Evento,
                Math.Round(fila.Reloj, 2),
                DiaSimulacion,
                fila.RND_LlegadaIM,
                Math.Round(fila.TiempoEntreLlegadas_LlegadaIM, 2),
                _ = (fila.ProximaLlegada_LlegadaIM == 9999) ? 0 : Math.Round(fila.ProximaLlegada_LlegadaIM, 2),
                fila.RND_LlegadaRP,
                Math.Round(fila.TiempoEntreLlegadas_LlegadaRP, 2),
                _ = (fila.ProximaLlegada_LlegadaRP == 9999) ? 0 : Math.Round(fila.ProximaLlegada_LlegadaRP, 2),
                fila.RND_LlegadaNP,
                Math.Round(fila.TiempoEntreLlegadas_LlegadaNP, 2),
                _ = (fila.ProximaLlegada_LlegadaNP == 9999) ? 0 : Math.Round(fila.ProximaLlegada_LlegadaNP, 2),
                fila.RND_FinAtencionIM,
                Math.Round(fila.TiempoAtencion_FinAtencionIM, 2),
                Math.Round(fila.FinAtencion_Tomas_FinAtencionIM, 2),
                fila.Estado_Tomas_FinAtencionIM.Nombre,
                Math.Round(fila.FinAtencion_Alicia_FinAtencionIM, 2),
                fila.Estado_Alicia_FinAtencionIM.Nombre,
                fila.Cola_IM,
                fila.RND_FinAtencionRP,
                Math.Round(fila.TiempoAtencion_FinAtencionRP, 2),
                Math.Round(fila.FinAtencion_Lucia_FinAtencionRP, 2),
                fila.Estado_Lucia_FinAtencionRP.Nombre,
                Math.Round(fila.FinAtencion_Maria_FinAtencionRP, 2),
                fila.Estado_Maria_FinAtencionRP.Nombre,
                fila.Cola_RP,
                Math.Round(fila.FinAtencion_Manuel, 2),
                fila.Estado_Manuel.Nombre,
                fila.RND_FinAtencionNP,
                Math.Round(fila.TiempoAtencion_FinAtencionNP, 2),
                Math.Round(fila.FinAtencion_Dario_FinAtencionNP, 2),
                fila.Estado_Dario_FinAtencionNP.Nombre,
                fila.Cola_NP,
                fila.TiempoDescanso,
                fila.FinDescanso,
                fila.CantidadClientes_IM,
                fila.CantidadClientes_RP,
                fila.CantidadClientes_NP,
                Math.Round(fila.AcumuladoTiempoAtencion_IM, 2),
                Math.Round(fila.AcumuladoTiempoAtencion_RP, 2),
                Math.Round(fila.AcumuladoTiempoAtencion_NP, 2),
                Math.Round(fila.MaximoTiempoAtencion_IM, 2),
                Math.Round(fila.MaximoTiempoAtencion_RP, 2),
                Math.Round(fila.MaximoTiempoAtencion_NP, 2)
                );

            // Colorea de rojo el valor del próximo reloj.

            if (indiceFila >= 1)
            {
                DataGridViewRow filaAgregada = Grilla.Rows[indiceFila - 1];

                foreach (DataGridViewCell cell in filaAgregada.Cells)
                {
                    if (cell.ColumnIndex > 2 && cell.ColumnIndex < 35 && cell.Value != null && cell.Value.ToString() == reloj)
                    {
                        cell.Style.ForeColor = Color.Red;
                        break;
                    }
                }
            }
        }

        private void CargarColumnasClientes()
        {

            foreach (Temporal cliente in TodosLosClientes)
            {
                if (cliente.Tipo == "Renueva permiso") { cliente.TipoResumido = "RP"; }

                if (cliente.Tipo == "Nuevo permiso") { cliente.TipoResumido = "NP"; }

                if (cliente.Tipo == "Interesado en matricula") { cliente.TipoResumido = "IM"; }
            }

            foreach (Temporal cliente in TodosLosClientes)
            {

                int fila = cliente.EnFilaNumero;

                // Si se diese el caso de que se intenta modificar una fila pasado el 'hasta' mostrar la grilla directamente.

                if (cliente.EnFilaNumero > FilaHasta && cliente.EnFilaNumero >= Grilla.RowCount - 1)
                {
                    return;
                }

                // Si el cliente es de tipo especial "Inicializacion" entonces llenar la fila de espacios vacios.

                if (cliente.Tipo == "Inicializacion")
                {
                    for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                    {
                        Grilla.Rows[fila].Cells[indiceColumna].Value = null;
                    }
                    continue;
                }

                // Si el cliente es de tipo especial "descanso" o "fin de dia" simplemente copiar todo como estaba.

                if (cliente.Tipo == "Descanso" || cliente.Tipo == "FinDia")
                {
                    for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                    {
                        string valor = Grilla.Rows[fila - 1].Cells[indiceColumna].Value?.ToString() ?? string.Empty;

                        Grilla.Rows[fila].Cells[indiceColumna].Value = Grilla.Rows[fila - 1].Cells[indiceColumna].Value;
                    }
                    continue;
                }

                if (cliente.Estado.Nombre == "Esperando Atención" || cliente.Estado.Nombre == "Siendo Atendido")
                {

                    bool existente = false;
                    int indiceDeHallazgo = 0;

                    // Recorrer todas las columnas de clientes a ver si el cliente ya había sido añadido antes.

                    for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                    {
                        string valor = Grilla.Rows[fila - 1].Cells[indiceColumna].Value?.ToString() ?? string.Empty;

                        if (valor.Contains("Número: (" + cliente.Numero.ToString() + ")") && valor.Contains("(" + cliente.TipoResumido.ToString() + ")"))
                        {

                            // Si ya había sido añadido antes, entonces agregarlo en la columna encontrada, en la fila donde debería reflejarse la actualización de estado.

                            existente = true;

                            if (cliente.Estado.Nombre == "Siendo Atendido")
                            {
                                Grilla.Rows[fila].Cells[indiceColumna].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                                indiceDeHallazgo = indiceColumna;
                            }
                            else
                            {
                                Grilla.Rows[fila].Cells[indiceColumna].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + "Nadie" + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                                indiceDeHallazgo = indiceColumna;
                            }
                            break;
                        }
                    }

                    // Si lo encontró y colocó en su lugar, entonces tiene que arrastrar todos los demás a excepción del que encontró.

                    if (existente)
                    {
                        for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                        {
                            string valor = Grilla.Rows[fila - 1].Cells[indiceColumna].Value?.ToString() ?? string.Empty;

                            if (indiceColumna != indiceDeHallazgo)
                            {
                                Grilla.Rows[fila].Cells[indiceColumna].Value = Grilla.Rows[fila - 1].Cells[indiceColumna].Value;
                            }

                        }
                    }

                    // Si no lo encontró entonces el cliente no había sido añadido anteriormente, significa que es la primera vez y hay que agregarlo.

                    if (!existente)
                    {
                        bool añadido = false;

                        // Buscar el primer lugar con cliente destruido y agregarlo ahí.

                        for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                        {
                            string valor = Grilla.Rows[fila - 1].Cells[indiceColumna].Value?.ToString() ?? string.Empty;

                            if (valor.Contains("Destruido") && !añadido)
                            {
                                if (cliente.Estado.Nombre == "Siendo Atendido")
                                {
                                    Grilla.Rows[fila].Cells[indiceColumna].Value =
                                                "Número: ("
                                                + cliente.Numero.ToString()
                                                + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                                + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                                + cliente.HoraIngreso.ToString();
                                    añadido = true;
                                }
                                else
                                {
                                    Grilla.Rows[fila].Cells[indiceColumna].Value =
                                                "Número: ("
                                                + cliente.Numero.ToString()
                                                + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                                + "Nadie" + ") HI: "
                                                + cliente.HoraIngreso.ToString();
                                    añadido = true;
                                }
                            }
                            else
                            {
                                // Como no es destruido, significa que ese lugar pertenece a otro cliente, entonces arrastrar el valor de la fila anterior a la fila actual.

                                Grilla.Rows[fila].Cells[indiceColumna].Value = Grilla.Rows[fila - 1].Cells[indiceColumna].Value;
                            }
                        }

                        // Si no fue añadido hasta ahora, es porque no había clientes destruidos, entonces debe crear una nueva columna y añadirse al final.

                        if (!añadido && Grilla.Rows[fila].Cells[2].Value.ToString() == "1")
                        {
                            DataGridViewColumn nuevaColumna = new DataGridViewTextBoxColumn();
                            nuevaColumna.Name = "Cliente";
                            Grilla.Columns.Add(nuevaColumna);

                            int indiceColumnaNueva = Grilla.Columns.Count - 1;

                            if (cliente.Estado.Nombre == "Siendo Atendido")
                            {
                                Grilla.Rows[fila].Cells[indiceColumnaNueva].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                            }
                            else
                            {
                                Grilla.Rows[fila].Cells[indiceColumnaNueva].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + "Nadie" + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                            }
                        }
                        else if (!añadido && Convert.ToInt32(Grilla.Rows[fila].Cells[2].Value.ToString()) > 1)
                        {

                            int indiceColumnaCliente = 0;

                            for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                            {
                                if (Grilla.Rows[fila].Cells[indiceColumna].Value == null)
                                {
                                    indiceColumnaCliente = indiceColumna;
                                    break;
                                }
                            }

                            if (indiceColumnaCliente == 0)
                            {
                                DataGridViewColumn nuevaColumna = new DataGridViewTextBoxColumn();
                                nuevaColumna.Name = "Cliente";
                                Grilla.Columns.Add(nuevaColumna);

                                int indiceColumnaNueva = Grilla.Columns.Count - 1;


                                if (cliente.Estado.Nombre == "Siendo Atendido")
                                {
                                    Grilla.Rows[fila].Cells[indiceColumnaNueva].Value =
                                                "Número: ("
                                                + cliente.Numero.ToString()
                                                + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                                + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                                + cliente.HoraIngreso.ToString();
                                }
                                else
                                {
                                    Grilla.Rows[fila].Cells[indiceColumnaNueva].Value =
                                                "Número: ("
                                                + cliente.Numero.ToString()
                                                + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                                + "Nadie" + ") HI: "
                                                + cliente.HoraIngreso.ToString();
                                }

                                continue;
                            }

                            if (cliente.Estado.Nombre == "Siendo Atendido")
                            {
                                Grilla.Rows[fila].Cells[indiceColumnaCliente].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                            }
                            else
                            {
                                Grilla.Rows[fila].Cells[indiceColumnaCliente].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + "Nadie" + ") HI: "
                                            + cliente.HoraIngreso.ToString();
                            }
                        }
                    }
                }

                // Si el cliente que hay en lista es un cambio de estado a destruido, debe buscar donde estaba.

                if (cliente.Estado.Nombre == "Destruido")
                {
                    for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                    {
                        string valor = Grilla.Rows[fila - 1].Cells[indiceColumna].Value?.ToString() ?? string.Empty;

                        if (valor.Contains("Número: (" + cliente.Numero.ToString() + ")") && valor.Contains("(" + cliente.TipoResumido.ToString() + ")"))
                        {
                            Console.WriteLine("Estado del cliente: " + cliente.Estado.Nombre);
                            Grilla.Rows[fila].Cells[indiceColumna].Value =
                                            "Número: ("
                                            + cliente.Numero.ToString()
                                            + ") (" + cliente.TipoResumido + ") (" + cliente.Estado.Nombre + ") ("
                                            + cliente.SiendoAtendidoPor.Nombre + ") HI: "
                                            + cliente.HoraIngreso.ToString();

                            // Cambiar el color a rojo.

                            Grilla.Rows[fila].Cells[indiceColumna].Style.BackColor = System.Drawing.Color.Red;

                        }
                        else
                        {
                            // A todos los que no fueron destruidos los debe arrastrar igual.

                            if (Grilla.Rows[fila].Cells[indiceColumna].Value == null)
                            {
                                Grilla.Rows[fila].Cells[indiceColumna].Value = Grilla.Rows[fila - 1].Cells[indiceColumna].Value;
                            }
                        }
                    }
                }

                // Si sucede que el objeto se destruye pero se muestra con un estado que no es, cambiarlo a destruido.

                if (true)
                {
                    for (int indiceColumna = 44; indiceColumna < Grilla.Columns.Count; ++indiceColumna)
                    {
                        if (Grilla.Rows[fila].Cells[indiceColumna].Style.BackColor == System.Drawing.Color.Red)
                        {
                            string textoActual = Grilla.Rows[fila].Cells[indiceColumna].Value.ToString();
                            string textoModificado = "";

                            if (textoActual.Contains("Siendo Atendido"))
                            {
                                textoModificado = textoActual.Replace("Siendo Atendido", "Destruido");
                            }
                            else
                            {
                                textoModificado = textoActual.Replace("Esperando Atención", "Destruido");
                            }

                            Grilla.Rows[fila].Cells[indiceColumna].Value = textoModificado;
                        }

                    }
                }

            }
        }

        private void CalcularMetricasSimulacion()
        {
            // Maximo tiempo de espera: El máximo tiempo de espera entre todos los días de simulación. 

            double maximoTiempoEsperaCliente = Math.Max(Math.Max(fila2.MaximoTiempoAtencion_IM, fila2.MaximoTiempoAtencion_NP), fila2.MaximoTiempoAtencion_RP);

            // Promedio clientes atendidos por dia: Sumatoria de clientes atendidos dividido la cantidad de días.

            double promedioClientesAtendidosPorDia = (fila2.CantidadClientes_IM + fila2.CantidadClientes_NP + fila2.CantidadClientes_RP) / DiaSimulacion;

            // Promedio de permanencia de clientes: Sumatoria de tiempo en el sistema dividido la cantidad total de clientes entre la cantidad de días.

            double promedioPermanenciaRP = ((fila2.AcumuladoTiempoAtencion_RP) / fila2.CantidadClientes_RP);
            double promedioPermanenciaNP = ((fila2.AcumuladoTiempoAtencion_NP) / fila2.CantidadClientes_NP);
            double promedioPermanenciaIM = ((fila2.AcumuladoTiempoAtencion_IM) / fila2.CantidadClientes_IM);

            // Actualizar atributos del formulario de métricas para ser mostrado cuando se aprete el botón de más métricas.

            FormularioSimulacion.maximoTiempoEsperaCliente = Math.Round(maximoTiempoEsperaCliente, 2).ToString() + " minutos";
            FormularioSimulacion.promedioClientesAtendidosPorDia = Math.Truncate(promedioClientesAtendidosPorDia).ToString() + " clientes";
            FormularioSimulacion.promedioPermanenciaIM = Math.Round(promedioPermanenciaIM, 2).ToString() + " minutos";
            FormularioSimulacion.promedioPermanenciaNP = Math.Round(promedioPermanenciaNP, 2).ToString() + " minutos";
            FormularioSimulacion.promedioPermanenciaRP = Math.Round(promedioPermanenciaRP, 2).ToString() + " minutos";
        }
    }
}
