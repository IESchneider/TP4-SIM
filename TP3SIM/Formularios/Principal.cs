using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP4SIM.Entidades;
using TP4SIM.Formularios;

namespace TP4SIM
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
            LimpiarCampos();
            CargarValoresPorDefecto();
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            // Revisar que los datos ingresados pasen por todas las validaciones.

            txtFilaDesde.Text = 0.ToString();

            if (txtNumeroSimulaciones.Text.Equals("") || txtFilaDesde.Text.Equals("") || txtFilaHasta.Text.Equals(""))
            {
                MessageBox.Show("No ha ingresado todos los datos requeridos, intente nuevamente.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //txtNumeroSimulaciones.Text = 1000000.ToString();
            //txtFilaDesde.Text = 0.ToString();
            //txtFilaHasta.Text = 5000.ToString();

            if (!ValidacionDesdeHasta() || !ValidacionMedia() || !ValidacionAB() || !ValidacionesParametros()) return;

            // Si validó la información, comenzar la simulación.

            ComenzarPrimeraSimulacion();
        }

       

        private void ComenzarPrimeraSimulacion()
        {
            // Enviar los datos de los valores de la primera simulación.

            Simulacion simulacion = new Simulacion();

            simulacion.CantidadSimulaciones = Convert.ToInt32(txtNumeroSimulaciones.Text.Trim());
            simulacion.FilaDesde = Convert.ToInt32(txtFilaDesde.Text.Trim());
            simulacion.FilaHasta = Convert.ToInt32(txtFilaHasta.Text.Trim());
            simulacion.MediaClientes = Convert.ToInt32(TxtMediaClientes.Text.Trim());
            simulacion.MediaLectura = Convert.ToInt32(TxtMediaLectura.Text.Trim());
            simulacion.A = Convert.ToDouble(txtAConsulta.Text.Trim());
            simulacion.B = Convert.ToDouble(txtBConsulta.Text.Trim());
            simulacion.ProbabilidadPedirLibro = Convert.ToDouble(nrcPedirLibro.Value);
            simulacion.ProbabilidadDevolverLibro = Convert.ToDouble(nrcDevolverLibro.Value);
            simulacion.ProbabilidadConsulta = Convert.ToDouble(nrcConsulta.Value);
            simulacion.ProbabilidadNo = Convert.ToDouble(nrcProbabilidadNo.Value);



            simulacion.FormularioSimulacion = new FormSimulacion();

            simulacion.Simular();
        } 
        private bool ValidacionAB()
        {
            double a = Convert.ToDouble(txtAConsulta.Text.Trim());
            double b = Convert.ToDouble(txtBConsulta.Text.Trim());
            if (b < a)
            {
                MessageBox.Show("El valor ingresado en b debe ser menor al de a, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            };
            return true;
        }
        private bool ValidacionMedia()
        {
            
            if (TxtMediaClientes.Text.Equals("") || TxtMediaLectura.Text.Equals("") || txtAConsulta.Text.Equals("") || txtBConsulta.Text.Equals(""))
            {
                MessageBox.Show("No ha ingresado todos los datos requeridos, intente nuevamente.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        
            private bool ValidacionDesdeHasta()
        {
            // Lista de validaciones para los números desde y hasta.

            int numeroSimulaciones = Convert.ToInt32(txtNumeroSimulaciones.Text.Trim());
            int filaDesde = Convert.ToInt32(txtFilaDesde.Text.Trim());
            int filaHasta = Convert.ToInt32(txtFilaHasta.Text.Trim());

            if (filaDesde > numeroSimulaciones || filaHasta > numeroSimulaciones)
            {
                MessageBox.Show("Las filas a mostrar deben estar dentro del número de simulaciones realizadas, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (filaHasta < filaDesde)
            {
                MessageBox.Show("El valor ingresado en la fila de comienzo debe ser menor al de la fila de fin, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validar que la diferencia de filas ingresada por el usuario sea menor a 500.

            int diferencia = filaHasta - filaDesde;

            if (diferencia > 10000)
            {
                MessageBox.Show("El valor de filas a mostrar debe ser de hasta 10000, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private bool ValidacionesParametros()
        {
            // Lista de validaciones de todos los parámetros

            if ((Math.Abs(Convert.ToDouble(nrcDevolverLibro.Value + nrcPedirLibro.Value + nrcConsulta.Value) - 1.0) > 0.001))
            {
                MessageBox.Show("La suma de las probabilidades tiene que sumar 1, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Convert.ToDouble(nrcProbabilidadNo.Value) == 0.00)
            {
                MessageBox.Show("La probabilidad de que una persona se vaya de la biblioteca debe ser mayor a cero, intente nuevamente.", "Datos incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void CargarValoresPorDefecto()
        {
            // Insertar valores por defecto en los numerics.

            nrcPedirLibro.Value = 0.45M;
            nrcDevolverLibro.Value = 0.45M;
            nrcConsulta.Value = 0.10M;

            nrcProbabilidadNo.Value = 0.60M;
            
        }

        private void LimpiarCampos()
        {
            txtFilaDesde.Clear();
            txtFilaHasta.Clear();
            txtNumeroSimulaciones.Clear();
        }
    }
}
