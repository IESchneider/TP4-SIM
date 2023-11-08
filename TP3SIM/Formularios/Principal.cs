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

            if (!ValidacionDesdeHasta() || !ValidacionMedia()) return;

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

            simulacion.FormularioSimulacion = new FormSimulacion();

            simulacion.Simular();
        }
        private bool ValidacionMedia()
        {
            
            if (TxtMediaClientes.Text.Equals("") || TxtMediaLectura.Text.Equals(""))
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

        private void LimpiarCampos()
        {
            txtFilaDesde.Clear();
            txtFilaHasta.Clear();
            txtNumeroSimulaciones.Clear();
        }
    }
}
