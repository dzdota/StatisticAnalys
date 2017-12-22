namespace testgistogr
{
    partial class VisualisationMultiDataForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.CorelMatrxtabPage = new System.Windows.Forms.TabPage();
            this.CorelAreachart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.HeatMaptabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HeatMappictureBox = new System.Windows.Forms.PictureBox();
            this.RadarDiogrtabPage = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.RadarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ParalKoordGrafiktabPage = new System.Windows.Forms.TabPage();
            this.ParalCoordchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1.SuspendLayout();
            this.CorelMatrxtabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CorelAreachart)).BeginInit();
            this.HeatMaptabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HeatMappictureBox)).BeginInit();
            this.RadarDiogrtabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadarChart)).BeginInit();
            this.ParalKoordGrafiktabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParalCoordchart)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.CorelMatrxtabPage);
            this.tabControl1.Controls.Add(this.HeatMaptabPage);
            this.tabControl1.Controls.Add(this.RadarDiogrtabPage);
            this.tabControl1.Controls.Add(this.ParalKoordGrafiktabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(735, 542);
            this.tabControl1.TabIndex = 0;
            // 
            // CorelMatrxtabPage
            // 
            this.CorelMatrxtabPage.Controls.Add(this.CorelAreachart);
            this.CorelMatrxtabPage.Location = new System.Drawing.Point(4, 22);
            this.CorelMatrxtabPage.Name = "CorelMatrxtabPage";
            this.CorelMatrxtabPage.Padding = new System.Windows.Forms.Padding(3);
            this.CorelMatrxtabPage.Size = new System.Drawing.Size(727, 516);
            this.CorelMatrxtabPage.TabIndex = 0;
            this.CorelMatrxtabPage.Text = "Матриця кореляційних полів";
            this.CorelMatrxtabPage.UseVisualStyleBackColor = true;
            // 
            // CorelAreachart
            // 
            this.CorelAreachart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CorelAreachart.Location = new System.Drawing.Point(3, 3);
            this.CorelAreachart.Name = "CorelAreachart";
            this.CorelAreachart.Size = new System.Drawing.Size(721, 510);
            this.CorelAreachart.TabIndex = 0;
            this.CorelAreachart.Text = "chart1";
            // 
            // HeatMaptabPage
            // 
            this.HeatMaptabPage.Controls.Add(this.panel1);
            this.HeatMaptabPage.Location = new System.Drawing.Point(4, 22);
            this.HeatMaptabPage.Name = "HeatMaptabPage";
            this.HeatMaptabPage.Padding = new System.Windows.Forms.Padding(3);
            this.HeatMaptabPage.Size = new System.Drawing.Size(727, 516);
            this.HeatMaptabPage.TabIndex = 1;
            this.HeatMaptabPage.Text = "Теплова карта";
            this.HeatMaptabPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.HeatMappictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 510);
            this.panel1.TabIndex = 1;
            // 
            // HeatMappictureBox
            // 
            this.HeatMappictureBox.Location = new System.Drawing.Point(3, 3);
            this.HeatMappictureBox.Name = "HeatMappictureBox";
            this.HeatMappictureBox.Size = new System.Drawing.Size(100, 50);
            this.HeatMappictureBox.TabIndex = 0;
            this.HeatMappictureBox.TabStop = false;
            // 
            // RadarDiogrtabPage
            // 
            this.RadarDiogrtabPage.Controls.Add(this.button4);
            this.RadarDiogrtabPage.Controls.Add(this.button3);
            this.RadarDiogrtabPage.Controls.Add(this.button2);
            this.RadarDiogrtabPage.Controls.Add(this.button1);
            this.RadarDiogrtabPage.Controls.Add(this.RadarChart);
            this.RadarDiogrtabPage.Location = new System.Drawing.Point(4, 22);
            this.RadarDiogrtabPage.Name = "RadarDiogrtabPage";
            this.RadarDiogrtabPage.Size = new System.Drawing.Size(727, 516);
            this.RadarDiogrtabPage.TabIndex = 3;
            this.RadarDiogrtabPage.Text = "Радарна діограма";
            this.RadarDiogrtabPage.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(162, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(47, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = ">>";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(109, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(47, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(56, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(47, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "<";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "<<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RadarChart
            // 
            this.RadarChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.RadarChart.ChartAreas.Add(chartArea1);
            this.RadarChart.Location = new System.Drawing.Point(8, 28);
            this.RadarChart.Name = "RadarChart";
            this.RadarChart.Size = new System.Drawing.Size(719, 485);
            this.RadarChart.TabIndex = 0;
            this.RadarChart.Text = "chart1";
            // 
            // ParalKoordGrafiktabPage
            // 
            this.ParalKoordGrafiktabPage.Controls.Add(this.ParalCoordchart);
            this.ParalKoordGrafiktabPage.Location = new System.Drawing.Point(4, 22);
            this.ParalKoordGrafiktabPage.Name = "ParalKoordGrafiktabPage";
            this.ParalKoordGrafiktabPage.Size = new System.Drawing.Size(727, 516);
            this.ParalKoordGrafiktabPage.TabIndex = 5;
            this.ParalKoordGrafiktabPage.Text = "Графік паралельних координат";
            this.ParalKoordGrafiktabPage.UseVisualStyleBackColor = true;
            // 
            // ParalCoordchart
            // 
            chartArea2.Name = "ChartArea1";
            this.ParalCoordchart.ChartAreas.Add(chartArea2);
            this.ParalCoordchart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParalCoordchart.Location = new System.Drawing.Point(0, 0);
            this.ParalCoordchart.Name = "ParalCoordchart";
            this.ParalCoordchart.Size = new System.Drawing.Size(727, 516);
            this.ParalCoordchart.TabIndex = 0;
            this.ParalCoordchart.Text = "chart1";
            // 
            // VisualisationMultiDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 542);
            this.Controls.Add(this.tabControl1);
            this.Name = "VisualisationMultiDataForm";
            this.Text = "VisualisationMultiDataForm";
            this.Load += new System.EventHandler(this.VisualisationMultiDataForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.CorelMatrxtabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CorelAreachart)).EndInit();
            this.HeatMaptabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HeatMappictureBox)).EndInit();
            this.RadarDiogrtabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RadarChart)).EndInit();
            this.ParalKoordGrafiktabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ParalCoordchart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage CorelMatrxtabPage;
        private System.Windows.Forms.TabPage HeatMaptabPage;
        private System.Windows.Forms.TabPage RadarDiogrtabPage;
        private System.Windows.Forms.TabPage ParalKoordGrafiktabPage;
        private System.Windows.Forms.DataVisualization.Charting.Chart CorelAreachart;
        private System.Windows.Forms.PictureBox HeatMappictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart ParalCoordchart;
        private System.Windows.Forms.DataVisualization.Charting.Chart RadarChart;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}