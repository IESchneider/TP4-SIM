namespace TP4SIM.Formularios
{
    partial class FormSimulacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnGenerarMetrica = new System.Windows.Forms.Button();
            this.dgvGrillaSimulacion = new System.Windows.Forms.DataGridView();
            this.Evento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reloj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RND_Llegada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TiempoEntreLlegadas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProximaLlegada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RND_TipoAtencion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoAtencion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RND_FinAtencion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TiempoAtencion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxFinAtencion_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxFinAtencion_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RND_FinLectura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Se_queda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RND_TiempoLectura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TiempoLectura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxFinLectura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstadoEmpleado_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstadoEmpleado_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cola = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstadoBiblioteca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TiempoPermanenciaBiblioteca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantPersonasBiblioteca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromTiempoPermanenciaBiblio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantPersonasQueIngresanBiblio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantPersonasQueNoIngresan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantTotalPersonas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromPersonasQueNoIngresanBiblio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrillaSimulacion)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerarMetrica
            // 
            this.btnGenerarMetrica.Location = new System.Drawing.Point(2195, 1099);
            this.btnGenerarMetrica.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerarMetrica.Name = "btnGenerarMetrica";
            this.btnGenerarMetrica.Size = new System.Drawing.Size(195, 44);
            this.btnGenerarMetrica.TabIndex = 19;
            this.btnGenerarMetrica.Text = "Métricas Adicionales";
            this.btnGenerarMetrica.UseVisualStyleBackColor = true;
            this.btnGenerarMetrica.Click += new System.EventHandler(this.btnGenerarMetrica_Click);
            // 
            // dgvGrillaSimulacion
            // 
            this.dgvGrillaSimulacion.AllowUserToAddRows = false;
            this.dgvGrillaSimulacion.AllowUserToDeleteRows = false;
            this.dgvGrillaSimulacion.AllowUserToResizeColumns = false;
            this.dgvGrillaSimulacion.AllowUserToResizeRows = false;
            this.dgvGrillaSimulacion.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrillaSimulacion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGrillaSimulacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrillaSimulacion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Evento,
            this.Reloj,
            this.RND_Llegada,
            this.TiempoEntreLlegadas,
            this.ProximaLlegada,
            this.RND_TipoAtencion,
            this.TipoAtencion,
            this.RND_FinAtencion,
            this.TiempoAtencion,
            this.ProxFinAtencion_1,
            this.ProxFinAtencion_2,
            this.RND_FinLectura,
            this.Se_queda,
            this.RND_TiempoLectura,
            this.TiempoLectura,
            this.ProxFinLectura,
            this.EstadoEmpleado_1,
            this.EstadoEmpleado_2,
            this.Cola,
            this.EstadoBiblioteca,
            this.TiempoPermanenciaBiblioteca,
            this.CantPersonasBiblioteca,
            this.PromTiempoPermanenciaBiblio,
            this.CantPersonasQueIngresanBiblio,
            this.CantPersonasQueNoIngresan,
            this.CantTotalPersonas,
            this.PromPersonasQueNoIngresanBiblio});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGrillaSimulacion.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGrillaSimulacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvGrillaSimulacion.Location = new System.Drawing.Point(0, 0);
            this.dgvGrillaSimulacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvGrillaSimulacion.Name = "dgvGrillaSimulacion";
            this.dgvGrillaSimulacion.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrillaSimulacion.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGrillaSimulacion.RowHeadersWidth = 51;
            this.dgvGrillaSimulacion.Size = new System.Drawing.Size(1655, 804);
            this.dgvGrillaSimulacion.TabIndex = 0;
            // 
            // Evento
            // 
            this.Evento.HeaderText = "Evento";
            this.Evento.MinimumWidth = 6;
            this.Evento.Name = "Evento";
            this.Evento.ReadOnly = true;
            this.Evento.Width = 125;
            // 
            // Reloj
            // 
            this.Reloj.HeaderText = "Reloj";
            this.Reloj.MinimumWidth = 6;
            this.Reloj.Name = "Reloj";
            this.Reloj.ReadOnly = true;
            this.Reloj.Width = 125;
            // 
            // RND_Llegada
            // 
            this.RND_Llegada.HeaderText = "RND_Llegada";
            this.RND_Llegada.MinimumWidth = 6;
            this.RND_Llegada.Name = "RND_Llegada";
            this.RND_Llegada.ReadOnly = true;
            this.RND_Llegada.Width = 125;
            // 
            // TiempoEntreLlegadas
            // 
            this.TiempoEntreLlegadas.HeaderText = "TiempoEntreLlegadas";
            this.TiempoEntreLlegadas.MinimumWidth = 6;
            this.TiempoEntreLlegadas.Name = "TiempoEntreLlegadas";
            this.TiempoEntreLlegadas.ReadOnly = true;
            this.TiempoEntreLlegadas.Width = 125;
            // 
            // ProximaLlegada
            // 
            this.ProximaLlegada.HeaderText = "ProximaLlegada";
            this.ProximaLlegada.MinimumWidth = 6;
            this.ProximaLlegada.Name = "ProximaLlegada";
            this.ProximaLlegada.ReadOnly = true;
            this.ProximaLlegada.Width = 125;
            // 
            // RND_TipoAtencion
            // 
            this.RND_TipoAtencion.HeaderText = "RND_TipoAtencion";
            this.RND_TipoAtencion.MinimumWidth = 6;
            this.RND_TipoAtencion.Name = "RND_TipoAtencion";
            this.RND_TipoAtencion.ReadOnly = true;
            this.RND_TipoAtencion.Width = 125;
            // 
            // TipoAtencion
            // 
            this.TipoAtencion.HeaderText = "TipoAtencion";
            this.TipoAtencion.MinimumWidth = 6;
            this.TipoAtencion.Name = "TipoAtencion";
            this.TipoAtencion.ReadOnly = true;
            this.TipoAtencion.Width = 125;
            // 
            // RND_FinAtencion
            // 
            this.RND_FinAtencion.HeaderText = "RND_FinAtencion";
            this.RND_FinAtencion.MinimumWidth = 6;
            this.RND_FinAtencion.Name = "RND_FinAtencion";
            this.RND_FinAtencion.ReadOnly = true;
            this.RND_FinAtencion.Width = 125;
            // 
            // TiempoAtencion
            // 
            this.TiempoAtencion.HeaderText = "TiempoAtencion";
            this.TiempoAtencion.MinimumWidth = 6;
            this.TiempoAtencion.Name = "TiempoAtencion";
            this.TiempoAtencion.ReadOnly = true;
            this.TiempoAtencion.Width = 125;
            // 
            // ProxFinAtencion_1
            // 
            this.ProxFinAtencion_1.HeaderText = "ProxFinAtencion_1";
            this.ProxFinAtencion_1.MinimumWidth = 6;
            this.ProxFinAtencion_1.Name = "ProxFinAtencion_1";
            this.ProxFinAtencion_1.ReadOnly = true;
            this.ProxFinAtencion_1.Width = 125;
            // 
            // ProxFinAtencion_2
            // 
            this.ProxFinAtencion_2.HeaderText = "ProxFinAtencion_2";
            this.ProxFinAtencion_2.MinimumWidth = 6;
            this.ProxFinAtencion_2.Name = "ProxFinAtencion_2";
            this.ProxFinAtencion_2.ReadOnly = true;
            this.ProxFinAtencion_2.Width = 125;
            // 
            // RND_FinLectura
            // 
            this.RND_FinLectura.HeaderText = "RND_FinLectura";
            this.RND_FinLectura.MinimumWidth = 6;
            this.RND_FinLectura.Name = "RND_FinLectura";
            this.RND_FinLectura.ReadOnly = true;
            this.RND_FinLectura.Width = 125;
            // 
            // Se_queda
            // 
            this.Se_queda.HeaderText = "Se_queda";
            this.Se_queda.MinimumWidth = 6;
            this.Se_queda.Name = "Se_queda";
            this.Se_queda.ReadOnly = true;
            this.Se_queda.Width = 125;
            // 
            // RND_TiempoLectura
            // 
            this.RND_TiempoLectura.HeaderText = "RND_TiempoLectura";
            this.RND_TiempoLectura.MinimumWidth = 6;
            this.RND_TiempoLectura.Name = "RND_TiempoLectura";
            this.RND_TiempoLectura.ReadOnly = true;
            this.RND_TiempoLectura.Width = 125;
            // 
            // TiempoLectura
            // 
            this.TiempoLectura.HeaderText = "TiempoLectura";
            this.TiempoLectura.MinimumWidth = 6;
            this.TiempoLectura.Name = "TiempoLectura";
            this.TiempoLectura.ReadOnly = true;
            this.TiempoLectura.Width = 125;
            // 
            // ProxFinLectura
            // 
            this.ProxFinLectura.HeaderText = "ProxFinLectura";
            this.ProxFinLectura.MinimumWidth = 6;
            this.ProxFinLectura.Name = "ProxFinLectura";
            this.ProxFinLectura.ReadOnly = true;
            this.ProxFinLectura.Width = 125;
            // 
            // EstadoEmpleado_1
            // 
            this.EstadoEmpleado_1.HeaderText = "EstadoEmpleado_1";
            this.EstadoEmpleado_1.MinimumWidth = 6;
            this.EstadoEmpleado_1.Name = "EstadoEmpleado_1";
            this.EstadoEmpleado_1.ReadOnly = true;
            this.EstadoEmpleado_1.Width = 125;
            // 
            // EstadoEmpleado_2
            // 
            this.EstadoEmpleado_2.HeaderText = "EstadoEmpleado_2";
            this.EstadoEmpleado_2.MinimumWidth = 6;
            this.EstadoEmpleado_2.Name = "EstadoEmpleado_2";
            this.EstadoEmpleado_2.ReadOnly = true;
            this.EstadoEmpleado_2.Width = 125;
            // 
            // Cola
            // 
            this.Cola.HeaderText = "Cola";
            this.Cola.MinimumWidth = 6;
            this.Cola.Name = "Cola";
            this.Cola.ReadOnly = true;
            this.Cola.Width = 125;
            // 
            // EstadoBiblioteca
            // 
            this.EstadoBiblioteca.HeaderText = "EstadoBiblioteca";
            this.EstadoBiblioteca.MinimumWidth = 6;
            this.EstadoBiblioteca.Name = "EstadoBiblioteca";
            this.EstadoBiblioteca.ReadOnly = true;
            this.EstadoBiblioteca.Width = 125;
            // 
            // TiempoPermanenciaBiblioteca
            // 
            this.TiempoPermanenciaBiblioteca.HeaderText = "TiempoPermanenciaBiblioteca";
            this.TiempoPermanenciaBiblioteca.MinimumWidth = 6;
            this.TiempoPermanenciaBiblioteca.Name = "TiempoPermanenciaBiblioteca";
            this.TiempoPermanenciaBiblioteca.ReadOnly = true;
            this.TiempoPermanenciaBiblioteca.Width = 125;
            // 
            // CantPersonasBiblioteca
            // 
            this.CantPersonasBiblioteca.HeaderText = "CantPersonasBiblioteca";
            this.CantPersonasBiblioteca.MinimumWidth = 6;
            this.CantPersonasBiblioteca.Name = "CantPersonasBiblioteca";
            this.CantPersonasBiblioteca.ReadOnly = true;
            this.CantPersonasBiblioteca.Width = 125;
            // 
            // PromTiempoPermanenciaBiblio
            // 
            this.PromTiempoPermanenciaBiblio.HeaderText = "PromTiempoPermanenciaBiblio";
            this.PromTiempoPermanenciaBiblio.MinimumWidth = 6;
            this.PromTiempoPermanenciaBiblio.Name = "PromTiempoPermanenciaBiblio";
            this.PromTiempoPermanenciaBiblio.ReadOnly = true;
            this.PromTiempoPermanenciaBiblio.Width = 125;
            // 
            // CantPersonasQueIngresanBiblio
            // 
            this.CantPersonasQueIngresanBiblio.HeaderText = "CantPersonasQueIngresanBiblio";
            this.CantPersonasQueIngresanBiblio.MinimumWidth = 6;
            this.CantPersonasQueIngresanBiblio.Name = "CantPersonasQueIngresanBiblio";
            this.CantPersonasQueIngresanBiblio.ReadOnly = true;
            this.CantPersonasQueIngresanBiblio.Width = 125;
            // 
            // CantPersonasQueNoIngresan
            // 
            this.CantPersonasQueNoIngresan.HeaderText = "CantPersonasQueNoIngresan";
            this.CantPersonasQueNoIngresan.MinimumWidth = 6;
            this.CantPersonasQueNoIngresan.Name = "CantPersonasQueNoIngresan";
            this.CantPersonasQueNoIngresan.ReadOnly = true;
            this.CantPersonasQueNoIngresan.Width = 125;
            // 
            // CantTotalPersonas
            // 
            this.CantTotalPersonas.HeaderText = "CantTotalPersonas";
            this.CantTotalPersonas.MinimumWidth = 6;
            this.CantTotalPersonas.Name = "CantTotalPersonas";
            this.CantTotalPersonas.ReadOnly = true;
            this.CantTotalPersonas.Width = 125;
            // 
            // PromPersonasQueNoIngresanBiblio
            // 
            this.PromPersonasQueNoIngresanBiblio.MinimumWidth = 6;
            this.PromPersonasQueNoIngresanBiblio.Name = "PromPersonasQueNoIngresanBiblio";
            this.PromPersonasQueNoIngresanBiblio.ReadOnly = true;
            this.PromPersonasQueNoIngresanBiblio.Width = 125;
            // 
            // FormSimulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1655, 798);
            this.Controls.Add(this.btnGenerarMetrica);
            this.Controls.Add(this.dgvGrillaSimulacion);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormSimulacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resultado de la simulación";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrillaSimulacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGenerarMetrica;
        private System.Windows.Forms.DataGridView dgvGrillaSimulacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Evento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reloj;
        private System.Windows.Forms.DataGridViewTextBoxColumn RND_Llegada;
        private System.Windows.Forms.DataGridViewTextBoxColumn TiempoEntreLlegadas;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProximaLlegada;
        private System.Windows.Forms.DataGridViewTextBoxColumn RND_TipoAtencion;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoAtencion;
        private System.Windows.Forms.DataGridViewTextBoxColumn RND_FinAtencion;
        private System.Windows.Forms.DataGridViewTextBoxColumn TiempoAtencion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxFinAtencion_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxFinAtencion_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RND_FinLectura;
        private System.Windows.Forms.DataGridViewTextBoxColumn Se_queda;
        private System.Windows.Forms.DataGridViewTextBoxColumn RND_TiempoLectura;
        private System.Windows.Forms.DataGridViewTextBoxColumn TiempoLectura;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxFinLectura;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstadoEmpleado_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstadoEmpleado_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cola;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstadoBiblioteca;
        private System.Windows.Forms.DataGridViewTextBoxColumn TiempoPermanenciaBiblioteca;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantPersonasBiblioteca;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromTiempoPermanenciaBiblio;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantPersonasQueIngresanBiblio;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantPersonasQueNoIngresan;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantTotalPersonas;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromPersonasQueNoIngresanBiblio;
    }
}