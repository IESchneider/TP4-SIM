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

        public int CantidadSimulaciones { get; set; } = 0;
        public int NumeroSimulacionActual { get; set; } = 0;
        public int cantidadPersonasEnBiblioteca { get; set; } = 0;
        public int numeroCliente { get; set; } = 0;
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

            string[] nombresServidores = { "Empleado 1", "Empleado 2" };

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


            // Mostrar formulario.

            MostrarFormulario(FormularioSimulacion, Grilla);
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
                      { fila1.ProximaLlegada, "Llegada_Persona"},
                      { fila1.ProxFinAtencion_1, "FinAtención_1"},
                      { fila1.ProxFinAtencion_2, "FinAtención_2"},
                      { fila1.ProxFinLectura, "FinLectura"},
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

                        numeroCliente++;

                        // Generar cliente temporal

                        Temporal cliente = new Temporal();
                        cliente.Numero = numeroCliente;
                        //cliente.Numero = ++CantidadClientesIM;
                        cliente.HoraIngreso = fila2.Reloj;
                        //cliente.Tipo = "Interesado en matricula";
                        //cliente.TiempoEspera = 0;

                        // Obtener RNDs y Llegadas

                        fila2.RND_Llegada = log.GenerarRND();
                        fila2.TiempoEntreLlegadas = log.VariableAleatoriaExponencial(4, fila2.RND_Llegada);
                        fila2.ProximaLlegada = fila2.Reloj + fila2.TiempoEntreLlegadas;

                        // Obtener RNDS y tipo atencion

                        fila2.RND_TipoAtencion = log.GenerarRND();
                        fila2.TipoAtencion = log.CalcularTipoAtencion(fila2.RND_TipoAtencion);
                        cliente.Tipo = fila2.TipoAtencion;

                        // Arrastrar otros eventos

                        //fila2.ProximaLlegada = fila1.ProximaLlegada;
                        //fila2.ProximaLlegada = fila1.ProximaLlegada;

                        fila2.CantTotalPersonas = fila1.CantTotalPersonas + 1;

                        fila2.Cola = fila1.Cola;

                        //fila2.CantidadClientes_IM = fila1.CantidadClientes_IM;
                        //fila2.CantidadClientes_RP = fila1.CantidadClientes_RP;
                        //fila2.CantidadClientes_NP = fila1.CantidadClientes_NP;

                        if (fila1.Cola > 0)
                        {
                            // Si la cola es mayor que cero, implica que el servidor está ocupado y por ende se debe incrementar la cola.
                            cantidadPersonasEnBiblioteca = log.CantidadPersonasBiblioteca(TodosLosClientes);

                            if (cantidadPersonasEnBiblioteca < 20)
                            {
                                ++fila2.Cola;
                                fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                                fila2.ProxFinAtencion_1 = fila1.ProxFinAtencion_1;
                                fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;

                                fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                                fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;

                                fila2.Persona.Add(cliente);
                                cliente.EnFilaNumero = NumeroSimulacionActual;

                                fila2.RND_TipoAtencion = log.GenerarRND();
                                cliente.Tipo = log.CalcularTipoAtencion(fila2.RND_TipoAtencion);
                                if (cliente.Tipo == "Pedir libro")
                                {
                                    cliente.Estado = EPedirLibro;
                                }
                                if (cliente.Tipo == "Devolver libro")
                                {
                                    cliente.Estado = EDevolverLibro;
                                }
                                if (cliente.Tipo == "Consulta")
                                {
                                    cliente.Estado = EConsultar;
                                }
                                fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                            }
                            else
                            {
                                cliente = cliente.DestruirCliente(cliente);
                                fila2.CantPersonasQueNoIngresanBiblio = fila1.CantPersonasQueNoIngresanBiblio+1;
                            }
                            TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                        }

                        if (fila1.Cola == 0)
                        {

                            if (fila1.EstadoEmpleado_1.Libre)
                            {
                                fila2.RND_TipoAtencion = log.GenerarRND();
                                cliente.Tipo = log.CalcularTipoAtencion(fila2.RND_TipoAtencion);

                                if (cliente.Tipo == "Pedir libro")
                                {
                                    fila2.TiempoAtencion = log.VariableAleatoriaExponencial(6, fila2.RND_FinAtencion);
                                    fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                    fila2.EstadoEmpleado_1 = estadosAPedidoLibro["Empleado 1"];
                                }
                                if (cliente.Tipo == "Devolver libro")
                                {
                                    fila2.TiempoAtencion = log.VariableAleatoriaConvolucion(2, 0.5);
                                    fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                    fila2.EstadoEmpleado_1 = estadosADevolucionLibro["Empleado 1"];
                                }
                                if (cliente.Tipo == "Consulta")
                                {
                                    fila2.TiempoAtencion = log.VariableAleatoriaUniforme(2, 5, fila2.RND_FinAtencion);
                                    fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                    fila2.EstadoEmpleado_1 = estadosAConsulta["Empleado 1"];
                                }

                                fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;
                                fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;
                                fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;
                                fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;

                                cliente.Estado = SiendoAtendido;
                                cliente.SiendoAtendidoPor = Empleado1;
                                fila2.Persona.Add(cliente);
                                cliente.EnFilaNumero = NumeroSimulacionActual;
                                TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                            }
                            else
                            {
                                if (fila1.EstadoEmpleado_2.Libre)
                                {
                                    fila2.RND_TipoAtencion = log.GenerarRND();
                                    cliente.Tipo = log.CalcularTipoAtencion(fila2.RND_TipoAtencion);

                                    if (cliente.Tipo == "Pedir libro")
                                    {
                                        fila2.TiempoAtencion = log.VariableAleatoriaExponencial(6, fila2.RND_TipoAtencion);
                                        fila2.ProxFinAtencion_2 = fila2.Reloj + fila2.TiempoAtencion;
                                        fila2.EstadoEmpleado_2 = estadosAPedidoLibro["Empleado 2"];
                                    }
                                    if (cliente.Tipo == "Devolver libro")
                                    {
                                        fila2.TiempoAtencion = log.VariableAleatoriaConvolucion(2, 0.5);
                                        fila2.ProxFinAtencion_2 = fila2.Reloj + fila2.TiempoAtencion;
                                        fila2.EstadoEmpleado_2 = estadosADevolucionLibro["Empleado 2"];
                                    }
                                    if (cliente.Tipo == "Consulta")
                                    {
                                        fila2.TiempoAtencion = log.VariableAleatoriaUniforme(2, 5, fila2.RND_TipoAtencion);
                                        fila2.ProxFinAtencion_2 = fila2.Reloj + fila2.TiempoAtencion;
                                        fila2.EstadoEmpleado_2 = estadosAConsulta["Empleado 2"];
                                    }

                                    fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                                    fila2.ProxFinAtencion_1 = fila1.ProxFinAtencion_1;
                                    fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                                    fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;

                                    cliente.Estado = SiendoAtendido;
                                    cliente.SiendoAtendidoPor = Empleado1;
                                    fila2.Persona.Add(cliente);
                                    cliente.EnFilaNumero = NumeroSimulacionActual;
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                    fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                                }
                                else
                                {
                                    cantidadPersonasEnBiblioteca = log.CantidadPersonasBiblioteca(TodosLosClientes);

                                    if (cantidadPersonasEnBiblioteca < 20)
                                    {
                                        ++fila2.Cola;
                                        fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                                        fila2.ProxFinAtencion_1 = fila1.ProxFinAtencion_1;
                                        fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;

                                        fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                                        fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;

                                        fila2.Persona.Add(cliente);
                                        cliente.EnFilaNumero = NumeroSimulacionActual;

                                        fila2.RND_TipoAtencion = log.GenerarRND();
                                        cliente.Tipo = log.CalcularTipoAtencion(fila2.RND_TipoAtencion);

                                        if (cliente.Tipo == "Pedir libro")
                                        {
                                            cliente.Estado = EPedirLibro;
                                        }
                                        if (cliente.Tipo == "Devolver libro")
                                        {
                                            cliente.Estado = EDevolverLibro;
                                        }
                                        if (cliente.Tipo == "Consulta")
                                        {
                                            cliente.Estado = EConsultar;
                                        }
                                        fila2.CantPersonasQueIngresanBiblio = fila1.CantPersonasQueIngresanBiblio+1;
                                    }
                                    else
                                    {
                                        cliente = cliente.DestruirCliente(cliente);
                                        fila2.CantPersonasQueNoIngresanBiblio = fila1.CantPersonasQueNoIngresanBiblio+1;
                                    }
                                    TodosLosClientes.Add(cliente.CopiarCliente(cliente));
                                }
                            }

                            fila2.TiempoPermanenciaBiblioteca = fila1.TiempoPermanenciaBiblioteca + (fila2.Reloj - fila1.Reloj) * fila1.CantPersonasBiblioteca;
                            fila2.PromTiempoPermanenciaBiblio = fila2.TiempoPermanenciaBiblioteca / fila2.CantPersonasBiblioteca;

                            fila2.PromPersonasQueNoIngresanBiblio = fila2.CantPersonasQueNoIngresanBiblio / fila2.CantTotalPersonas;

                            // Arrastrar valores que no se utilizan.
                            fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;
                            //fila2.ProximaLlegada = fila1.ProximaLlegada;
                            fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;
                        }

                        break;

                    case "FinAtención_1":

                        // Se destruye cliente ya atendido
                        foreach (Temporal client in TodosLosClientes)
                        {
                            if (client.Estado == SiendoAtendido && client.SiendoAtendidoPor == Empleado1)
                            {
                                if(client.Tipo == "Pedir libro")
                                {
                                    // Calculamos si se queda a leer si pidio un libro prestado
                                    fila2.RND_FinLectura = log.GenerarRND();
                                    fila2.Se_queda = log.CalcularSiSeQueda(fila2.RND_FinLectura);

                                    if(fila2.Se_queda == "Si")
                                    {
                                        fila2.RND_TiempoLectura = log.GenerarRND();
                                        fila2.ProxFinLectura = log.VariableAleatoriaExponencial(30, fila2.RND_TiempoLectura);

                                        client.HoraFinLectura = fila2.ProxFinLectura;
                                        client.Estado = EnBiblioteca;
                                        break;
                                    }
                                    else
                                    {
                                        client.DestruirCliente(client);
                                        break;
                                    }
                                }
                                else
                                {
                                    client.DestruirCliente(client);
                                    break;
                                }
                                
                            }
                        }

                        // Si hay cola se atiende a la siguiente persona
                        if (fila1.Cola > 0)
                        {
                            var proxCliente = log.proximoCliente(TodosLosClientes);
                            fila2.RND_FinAtencion = log.GenerarRND();
                            if (proxCliente.Tipo == "Pedir libro")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaExponencial(6, fila2.RND_FinAtencion);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosAPedidoLibro["Empleado 1"];
                            }
                            if (proxCliente.Tipo == "Devolver libro")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaConvolucion(2, 0.5);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosADevolucionLibro["Empleado 1"];
                            }
                            if (proxCliente.Tipo == "Consulta")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaUniforme(2, 5, fila2.RND_FinAtencion);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosAConsulta["Empleado 1"];
                            }

                            --fila2.Cola;


                        }
                        else
                        {

                            fila2.EstadoEmpleado_1 = estadosLibre["Empleado 1"];

                            fila2.ProxFinAtencion_1 = 0;

                        }


                        fila2.CantPersonasBiblioteca = --fila1.CantPersonasBiblioteca;
                        fila2.TiempoPermanenciaBiblioteca = fila1.TiempoPermanenciaBiblioteca + (fila2.Reloj - fila1.Reloj) * fila1.CantPersonasBiblioteca;
                        fila2.PromTiempoPermanenciaBiblio = fila2.TiempoPermanenciaBiblioteca / fila2.CantPersonasBiblioteca;



                        // Arrastrar valores que no se utilizan.
                        fila2.EstadoEmpleado_2 = fila1.EstadoEmpleado_2;
                        fila2.ProximaLlegada = fila1.ProximaLlegada;
                        fila2.ProxFinAtencion_2 = fila1.ProxFinAtencion_2;
                        fila2.PromPersonasQueNoIngresanBiblio = fila1.PromPersonasQueNoIngresanBiblio;

                        break;

                    case "FinAtención_2":

                        // Se destruye cliente ya atendido
                        foreach (Temporal client in TodosLosClientes)
                        {
                            if (client.Estado == SiendoAtendido && client.SiendoAtendidoPor == Empleado2)
                            {
                                if (client.Tipo == "Pedir libro")
                                {
                                    // Calculamos si se queda a leer si pidio un libro prestado
                                    fila2.RND_FinLectura = log.GenerarRND();
                                    fila2.Se_queda = log.CalcularSiSeQueda(fila2.RND_FinLectura);

                                    if (fila2.Se_queda == "Si")
                                    {
                                        fila2.RND_TiempoLectura = log.GenerarRND();
                                        fila2.ProxFinLectura = log.VariableAleatoriaExponencial(30, fila2.RND_TiempoLectura);

                                        client.HoraFinLectura = fila2.ProxFinLectura;
                                        client.Estado = EnBiblioteca;
                                        break;
                                    }
                                    else
                                    {
                                        client.DestruirCliente(client);
                                        break;
                                    }
                                }
                                else
                                {
                                    client.DestruirCliente(client);
                                    break;
                                }

                            }
                        }

                        // Si hay cola se atiende a la siguiente persona
                        if (fila1.Cola > 0)
                        {
                            var proxCliente = log.proximoCliente(TodosLosClientes);

                            if (proxCliente.Tipo == "Pedir libro")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaExponencial(6, fila2.RND_FinAtencion);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosAPedidoLibro["Empleado 2"];
                            }
                            if (proxCliente.Tipo == "Devolver libro")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaConvolucion(2, 0.5);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosADevolucionLibro["Empleado 2"];
                            }
                            if (proxCliente.Tipo == "Consulta")
                            {
                                fila2.TiempoAtencion = log.VariableAleatoriaUniforme(2, 5, fila2.RND_FinAtencion);
                                fila2.ProxFinAtencion_1 = fila2.Reloj + fila2.TiempoAtencion;
                                fila2.EstadoEmpleado_1 = estadosAConsulta["Empleado 2"];
                            }

                            --fila2.Cola;


                        }
                        else
                        {

                            fila2.EstadoEmpleado_2 = estadosLibre["Empleado 2"];

                            fila2.ProxFinAtencion_2 = 0;

                        }


                        fila2.CantPersonasBiblioteca = --fila1.CantPersonasBiblioteca;
                        fila2.TiempoPermanenciaBiblioteca = fila1.TiempoPermanenciaBiblioteca + (fila2.Reloj - fila1.Reloj) * fila1.CantPersonasBiblioteca;
                        fila2.PromTiempoPermanenciaBiblio = fila2.TiempoPermanenciaBiblioteca / fila2.CantPersonasBiblioteca;



                        // Arrastrar valores que no se utilizan.
                        fila2.EstadoEmpleado_1 = fila1.EstadoEmpleado_1;
                        fila2.ProximaLlegada = fila1.ProximaLlegada;
                        fila2.ProxFinAtencion_1 = fila1.ProxFinAtencion_1;
                        fila2.PromPersonasQueNoIngresanBiblio = fila1.PromPersonasQueNoIngresanBiblio;

                        break;

                    case "FinLectura":

                        // Se destruye cliente ya atendido
                        foreach (Temporal client in TodosLosClientes)
                        {
                            if(client.HoraFinLectura == fila2.Reloj)
                            {
                                client.DestruirCliente(client);
                                break;
                            }
                        }

                        fila2.CantPersonasBiblioteca = --fila1.CantPersonasBiblioteca;
                        fila2.TiempoPermanenciaBiblioteca = fila1.TiempoPermanenciaBiblioteca + (fila2.Reloj - fila1.Reloj) * fila1.CantPersonasBiblioteca;
                        fila2.PromTiempoPermanenciaBiblio = fila2.TiempoPermanenciaBiblioteca / fila2.CantPersonasBiblioteca;

                        fila2.CantTotalPersonas = fila1.CantTotalPersonas;
                        fila2.PromPersonasQueNoIngresanBiblio = fila1.PromPersonasQueNoIngresanBiblio;
                        fila2.ProximaLlegada = fila1.ProximaLlegada;

                        break;
                }


                // La fila anterior pasa a tener los nuevos valores para repetir el proceso.

                fila1.Evento = fila2.Evento;
                fila1.Reloj = fila2.Reloj;
                fila1.RND_Llegada = fila2.RND_Llegada;
                fila1.TiempoEntreLlegadas = fila2.TiempoEntreLlegadas;
                fila1.ProximaLlegada = fila2.ProximaLlegada;
                fila1.RND_TipoAtencion = fila2.RND_TipoAtencion;
                fila1.TipoAtencion = fila2.TipoAtencion;
                fila1.RND_FinAtencion = fila2.RND_FinAtencion;
                fila1.TiempoAtencion = fila2.TiempoAtencion;
                fila1.ProxFinAtencion_1 = fila2.ProxFinAtencion_1;
                fila1.ProxFinAtencion_2 = fila2.ProxFinAtencion_2;
                fila1.EstadoEmpleado_1 = fila2.EstadoEmpleado_1;
                fila1.EstadoEmpleado_2 = fila2.EstadoEmpleado_2;
                fila1.Cola = fila2.Cola;
                fila1.RND_FinLectura = fila2.RND_FinLectura;
                fila1.Se_queda = fila2.Se_queda;
                fila1.RND_TiempoLectura = fila2.RND_TiempoLectura;
                fila1.TiempoLectura = fila2.TiempoLectura;
                fila1.ProxFinLectura = fila2.ProxFinLectura;
                fila1.EstadoBiblioteca = fila2.EstadoBiblioteca;
                fila1.TiempoPermanenciaBiblioteca = fila2.TiempoPermanenciaBiblioteca;
                fila1.CantPersonasBiblioteca = fila2.CantPersonasBiblioteca;
                fila1.PromTiempoPermanenciaBiblio = fila2.PromTiempoPermanenciaBiblio;
                fila1.CantPersonasQueIngresanBiblio = fila2.CantPersonasQueIngresanBiblio;
                fila1.CantPersonasQueNoIngresanBiblio = fila2.CantPersonasQueNoIngresanBiblio;
                fila1.PromPersonasQueNoIngresanBiblio = fila2.PromPersonasQueNoIngresanBiblio;
                fila1.CantTotalPersonas = fila2.CantTotalPersonas;
                fila1.Persona = fila2.Persona;

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
                fila.RND_Llegada,
                Math.Round(fila.TiempoEntreLlegadas, 2),
                _ = (fila.ProximaLlegada == 9999) ? 0 : Math.Round(fila.ProximaLlegada, 2),
                _ = (fila.RND_TipoAtencion == 9999) ? 0 : Math.Round(fila.RND_TipoAtencion, 2),
                fila.TipoAtencion,
                Math.Round(fila.RND_FinAtencion, 2),
                _ = (fila.TiempoAtencion == 9999) ? 0 : Math.Round(fila.TiempoAtencion, 2),
                Math.Round(fila.ProxFinAtencion_1, 2),
                Math.Round(fila.ProxFinAtencion_2, 2),
                fila.EstadoEmpleado_1.Nombre,
                fila.EstadoEmpleado_2.Nombre,
                fila.Cola,
                Math.Round(fila.RND_FinLectura, 2),
                fila.Se_queda,
                Math.Round(fila.RND_TiempoLectura, 2),
                Math.Round(fila.TiempoLectura, 2),
                Math.Round(fila.ProxFinLectura, 2),
                Math.Round(fila.TiempoPermanenciaBiblioteca, 2),
                fila.CantPersonasBiblioteca,
                Math.Round(fila.PromTiempoPermanenciaBiblio, 2),
                fila.CantPersonasQueIngresanBiblio,
                fila.CantPersonasQueNoIngresanBiblio,
                Math.Round(fila.PromPersonasQueNoIngresanBiblio, 2),
                fila.CantTotalPersonas
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

    }
}
