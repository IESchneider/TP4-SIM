namespace TP4SIM
{
    partial class Principal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbPrincipal = new System.Windows.Forms.GroupBox();
            this.gbSimular = new System.Windows.Forms.GroupBox();
            this.btnSimular = new System.Windows.Forms.Button();
            this.gbDatosGenerales = new System.Windows.Forms.GroupBox();
            this.txtFilaHasta = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNumeroSimulaciones = new System.Windows.Forms.Label();
            this.txtNumeroSimulaciones = new System.Windows.Forms.MaskedTextBox();
            this.txtFilaDesde = new System.Windows.Forms.MaskedTextBox();
            this.gbPrincipal.SuspendLayout();
            this.gbSimular.SuspendLayout();
            this.gbDatosGenerales.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPrincipal
            // 
            this.gbPrincipal.Controls.Add(this.gbSimular);
            this.gbPrincipal.Controls.Add(this.gbDatosGenerales);
            this.gbPrincipal.Location = new System.Drawing.Point(4, 4);
            this.gbPrincipal.Name = "gbPrincipal";
            this.gbPrincipal.Size = new System.Drawing.Size(294, 382);
            this.gbPrincipal.TabIndex = 0;
            this.gbPrincipal.TabStop = false;
            this.gbPrincipal.Text = "Simulación - Linea de Espera";
            // 
            // gbSimular
            // 
            this.gbSimular.Controls.Add(this.txtFilaDesde);
            this.gbSimular.Controls.Add(this.btnSimular);
            this.gbSimular.Location = new System.Drawing.Point(8, 273);
            this.gbSimular.Name = "gbSimular";
            this.gbSimular.Size = new System.Drawing.Size(269, 100);
            this.gbSimular.TabIndex = 21;
            this.gbSimular.TabStop = false;
            this.gbSimular.Text = "Simular";
            // 
            // btnSimular
            // 
            this.btnSimular.Location = new System.Drawing.Point(81, 39);
            this.btnSimular.Name = "btnSimular";
            this.btnSimular.Size = new System.Drawing.Size(96, 34);
            this.btnSimular.TabIndex = 19;
            this.btnSimular.Text = "Simular";
            this.btnSimular.UseVisualStyleBackColor = true;
            this.btnSimular.Click += new System.EventHandler(this.btnSimular_Click);
            // 
            // gbDatosGenerales
            // 
            this.gbDatosGenerales.Controls.Add(this.txtFilaHasta);
            this.gbDatosGenerales.Controls.Add(this.label1);
            this.gbDatosGenerales.Controls.Add(this.lblNumeroSimulaciones);
            this.gbDatosGenerales.Controls.Add(this.txtNumeroSimulaciones);
            this.gbDatosGenerales.Location = new System.Drawing.Point(8, 35);
            this.gbDatosGenerales.Name = "gbDatosGenerales";
            this.gbDatosGenerales.Size = new System.Drawing.Size(269, 232);
            this.gbDatosGenerales.TabIndex = 18;
            this.gbDatosGenerales.TabStop = false;
            this.gbDatosGenerales.Text = "Datos Generales";
            // 
            // txtFilaHasta
            // 
            this.txtFilaHasta.Location = new System.Drawing.Point(29, 151);
            this.txtFilaHasta.Mask = "9999999";
            this.txtFilaHasta.Name = "txtFilaHasta";
            this.txtFilaHasta.Size = new System.Drawing.Size(57, 20);
            this.txtFilaHasta.TabIndex = 20;
            this.txtFilaHasta.ValidatingType = typeof(int);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Ingrese hasta qué fila desea visualizar:";
            // 
            // lblNumeroSimulaciones
            // 
            this.lblNumeroSimulaciones.AutoSize = true;
            this.lblNumeroSimulaciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroSimulaciones.Location = new System.Drawing.Point(6, 43);
            this.lblNumeroSimulaciones.Name = "lblNumeroSimulaciones";
            this.lblNumeroSimulaciones.Size = new System.Drawing.Size(231, 17);
            this.lblNumeroSimulaciones.TabIndex = 16;
            this.lblNumeroSimulaciones.Text = "Ingrese el número de simulaciones:";
            // 
            // txtNumeroSimulaciones
            // 
            this.txtNumeroSimulaciones.Location = new System.Drawing.Point(29, 67);
            this.txtNumeroSimulaciones.Mask = "9999999";
            this.txtNumeroSimulaciones.Name = "txtNumeroSimulaciones";
            this.txtNumeroSimulaciones.Size = new System.Drawing.Size(57, 20);
            this.txtNumeroSimulaciones.TabIndex = 3;
            this.txtNumeroSimulaciones.ValidatingType = typeof(int);
            // 
            // txtFilaDesde
            // 
            this.txtFilaDesde.Location = new System.Drawing.Point(206, 74);
            this.txtFilaDesde.Mask = "9999999";
            this.txtFilaDesde.Name = "txtFilaDesde";
            this.txtFilaDesde.Size = new System.Drawing.Size(57, 20);
            this.txtFilaDesde.TabIndex = 21;
            this.txtFilaDesde.ValidatingType = typeof(int);
            this.txtFilaDesde.Visible = false;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(304, 392);
            this.Controls.Add(this.gbPrincipal);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trabajo Práctico 4 - Simulacion";
            this.gbPrincipal.ResumeLayout(false);
            this.gbSimular.ResumeLayout(false);
            this.gbSimular.PerformLayout();
            this.gbDatosGenerales.ResumeLayout(false);
            this.gbDatosGenerales.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPrincipal;
        private System.Windows.Forms.Label lblNumeroSimulaciones;
        private System.Windows.Forms.MaskedTextBox txtNumeroSimulaciones;
        private System.Windows.Forms.GroupBox gbDatosGenerales;
        private System.Windows.Forms.MaskedTextBox txtFilaHasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSimular;
        private System.Windows.Forms.GroupBox gbSimular;
        private System.Windows.Forms.MaskedTextBox txtFilaDesde;
    }
}

