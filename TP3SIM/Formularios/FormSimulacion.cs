using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace TP4SIM.Formularios
{
    public partial class FormSimulacion : Form
    {

        public string maximoTiempoEsperaCliente { get; set; }
        public string promedioClientesAtendidosPorDia { get; set; }
        public string promedioPermanenciaRP { get; set; }
        public string promedioPermanenciaIM { get; set; }
        public string promedioPermanenciaNP { get; set; }

        public FormSimulacion()
        {
            InitializeComponent();
        }

        public DataGridView DevolverGrilla()
        {
            return this.dgvGrillaSimulacion;
        }

        private void btnGenerarMetrica_Click(object sender, EventArgs e)
        {
            // Crea el formulario de métricas, le envía todos los datos y muestra el formulario de métricas.

            FormMetricas formMetricas = new FormMetricas();

            formMetricas.ActualizarLblMaximoTiempoEsperaCliente(maximoTiempoEsperaCliente);

            formMetricas.ActualizarLblPromedioClientesAtendidosPorDia(promedioClientesAtendidosPorDia);

            formMetricas.ActualizarLblPromedioPermanenciaIM(promedioPermanenciaIM);

            formMetricas.ActualizarLblPromedioPermanenciaRP(promedioPermanenciaRP);

            formMetricas.ActualizarLblPromedioPermanenciaNP(promedioPermanenciaNP);

            formMetricas.Show();
        }
    }
}
