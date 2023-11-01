using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TP4SIM.Formularios
{
    public partial class FormMetricas : Form
    {   

        public FormMetricas()
        {
            InitializeComponent();
        }

        public void ActualizarLblMaximoTiempoEsperaCliente(string valor)
        {
            lblMaximoTiempoEsperaCliente.Text = valor;
        }

        public void ActualizarLblPromedioClientesAtendidosPorDia(string valor)
        {
            lblPromedioClientesAtendidosPorDia.Text = valor;
        }

        public void ActualizarLblPromedioPermanenciaRP(string valor)
        {
            lblPromedioPermanenciaRP.Text = valor;
        }

        public void ActualizarLblPromedioPermanenciaIM(string valor)
        {
            lblPromedioPermanenciaIM.Text = valor;
        }

        public void ActualizarLblPromedioPermanenciaNP(string valor)
        {
            lblPromedioPermanenciaNP.Text = valor;
        }
    }
}
