using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Threading;

namespace testgistogr
{
    public partial class VisualisationMultiDataForm : Form
    {
        InitialAnalysMultidimensionalData IAM;
        int NumRadaraData = 0;
        int n = 25;
        internal VisualisationMultiDataForm(InitialAnalysMultidimensionalData IAM)
        {
            this.IAM = IAM;
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            HeatMappictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        private void VisualisationMultiDataForm_Load(object sender, EventArgs e)
        {
            if (IAM.ISA.Count > 10)
                tabControl1.Controls.Remove(RadarDiogrtabPage);
            else
                RadarchartPaint(IAM, Color.Red, n, RadarChart);
            CorelAreachart.ChartAreas.Clear();
            CorelAreachartPaint(IAM, Color.DarkBlue, Color.DarkBlue, CorelAreachart, Color.Red);
            //HeatMappictureBoxPaint(IAM, Color.DarkRed, HeatMappictureBox);
            Thread HeatMapThread = new Thread(new ThreadStart(HeatThr));
            HeatMapThread.Start();
            ParalCoordchartPaint(IAM, ParalCoordchart);
        }
        void HeatThr()
        {
            HeatMappictureBoxPaint(IAM, Color.DarkRed, HeatMappictureBox);
        }
        private void CorelAreachartPaint(InitialAnalysMultidimensionalData IAM, Color CorelCol, Color GistCol, Chart CorelChart, Color ColLine)
        {
            for (int i = 0; i < IAM.n; i++)
            {
                for (int j = 0; j < IAM.n; j++)
                {
                    string ChartAreaName = j.ToString() + "_" + i.ToString();
                    ChartArea ChartArea1 = new ChartArea(ChartAreaName);
                    ChartArea1.AxisX.LabelStyle.Enabled = false;
                    ChartArea1.AxisY.LabelStyle.Enabled = false;
                    CorelChart.ChartAreas.Add(ChartArea1);
                    var po = CorelAreachart.ChartAreas[0].Position;


                    Series ser;
                    if (i != j)
                    {
                        InitialStatisticalAnalys ISAX = new InitialStatisticalAnalys(InitialStatisticalAnalys.StandData(IAM.ISA[i].unsortl, IAM.ISA[i].Gx.Q, IAM.ISA[i].Mx.Q));
                        InitialStatisticalAnalys ISAY = new InitialStatisticalAnalys(InitialStatisticalAnalys.StandData(IAM.ISA[j].unsortl, IAM.ISA[j].Gx.Q, IAM.ISA[j].Mx.Q));
                        ser = PaintData.CorelPaint(ISAX.unsortl, ISAY.unsortl, CorelCol, ChartAreaName);
                        ser.MarkerStyle = MarkerStyle.Circle;
                        CorelChart.Series.Add(ser);

                        double Sz = 0;
                        if (Correlation_RegressionAnalysis.KorelationZnach(IAM.K[i,j],IAM.N,IAM.ISA[0].alf.Q))
                        {
                            List<Data> Param = Correlation_RegressionAnalysis.RegresParamFound(
                            ISAX,
                            ISAY,
                            IAM.K[i, j], RegresTypeName.LineRegresion, ref Sz);
                            Series serReg = PaintData.LineRegresPaint(ISAX.Min.Q,
                            ISAX.Max.Q,
                            Param[0].Q,
                            Param[1].Q,
                            ColLine,
                            ChartAreaName + "Line");
                            serReg.ChartArea = ChartAreaName;
                            CorelChart.Series.Add(serReg);
                        }
                    }
                    else
                    {
                        ser = PaintData.GistogPaint(IAM.ISA[i], GistCol, ChartAreaName);
                        CorelChart.Series.Add(ser);
                    }
                    ser.ChartArea = ChartAreaName;
                }
            }

        }

        private void RadarchartPaint(InitialAnalysMultidimensionalData IAM,Color RadarCol,int n,Chart RadarChart)
        {
            Font font = new Font("Times New Roman", 14.0f);
            RadarChart.ChartAreas.Clear();
            RadarChart.Series.Clear();
            RadarChart.Titles.Clear();
            for (int i = 0; i < n && i + NumRadaraData<IAM.N; i++)
            {
                string ChartAreaName = i.ToString();
                Title titl = new Title((i + 1 + NumRadaraData).ToString(),Docking.Bottom,font,Color.DarkBlue);
                titl.DockedToChartArea = ChartAreaName;
                RadarChart.Titles.Add(titl);
                ChartArea ChartArea1 = new ChartArea(ChartAreaName);
                ChartArea1.AxisY.Maximum = 1;

                ChartArea1.AxisX.LabelStyle.Enabled = false;
                ChartArea1.AxisY.LabelStyle.Enabled = false;
                ChartArea1.AxisY.MajorGrid.Enabled = false;
                RadarChart.ChartAreas.Add(ChartArea1);




                Series ser = new Series(ChartAreaName+"_Ser");
                ser.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
                ser["PointWidth"] = "1";
                ser.BorderWidth = 1;
                ser.Color = RadarCol;
                ser.BorderColor = Color.Black;
                for (int col = 0; col < IAM.n; col++)
                    ser.Points.AddXY(col, (IAM.ISA[col].unsortl[i + NumRadaraData] - IAM.ISA[col].Min.Q) / IAM.ISA[col].Len.Q );
                ser.ChartArea = ChartAreaName;
                RadarChart.Series.Add(ser);
            }
            
        }
        private void HeatMappictureBoxPaint(InitialAnalysMultidimensionalData IAM,Color MaxValueCol,PictureBox HeatMapPuctureBox)
        {
            Font font = new Font("Times New Roman", 14.0f);
            int width = panel1.Width - 20;
            int height = 25* IAM.N;
            double stepcol = width / IAM.n;
            double steprow = height / IAM.N;
            Bitmap BM = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(BM))
            {
                for (int row = 0; row < IAM.N; row++)
                {
                    for (int col = 0; col < IAM.n; col++)
                    {
                        double s = (IAM.ISA[col].unsortl[row] - IAM.ISA[col].Min.Q) / IAM.ISA[col].Len.Q;
                        if (s == 0)
                        {
                        }
                        SolidBrush f = new SolidBrush(
                            Color.FromArgb(
                                255 - (int)((255 - MaxValueCol.R) * s),
                                255 - (int)((255 - MaxValueCol.G) * s),
                                255 - (int)((255 - MaxValueCol.B) * s)));
                        gr.FillRectangle(f,
                            (float)(col * stepcol),
                            (float)(row * steprow),
                            (float)((col + 1) * stepcol),
                            (float)((row + 1) * steprow));
                    }
                    gr.DrawString((row + 1).ToString(), font, Brushes.Green, 10, (float)(row * steprow));
                }
                gr.Dispose();
            }
            HeatMapPuctureBox.Image = BM;
        }

        private void ParalCoordchartPaint(InitialAnalysMultidimensionalData IAM, Chart ParalCoordchart)
        {
            ParalCoordchart.Series.Clear();
            for (int row = 0; row < IAM.N; row++)
            {
                Color color = Color.Red;//ColorFromHSV((360.0 * row / IAM.N),1,1);
                Series Line = new Series(row.ToString());
                Line.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Line.Color = color;
                for (int col = 0; col < IAM.n; col++)
                    Line.Points.AddXY(col + 1, 2 * (IAM.ISA[col].unsortl[row] - IAM.ISA[col].Min.Q)/ IAM.ISA[col].Len.Q - 1) ;
                ParalCoordchart.Series.Add(Line);
            }
            ParalCoordchart.ChartAreas[0].AxisY.Minimum = -1;
            ParalCoordchart.ChartAreas[0].AxisY.Maximum = 1;
            ParalCoordchart.ChartAreas[0].AxisX.Minimum = 1;
            ParalCoordchart.ChartAreas[0].AxisX.Maximum = IAM.n;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            NumRadaraData += n;
            if (NumRadaraData > IAM.N - n)
                NumRadaraData = IAM.N - n;
            RadarchartPaint(IAM, Color.Red, n, RadarChart);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NumRadaraData -= n;
            if (NumRadaraData <0)
                NumRadaraData = 0;
            RadarchartPaint(IAM, Color.Red, n, RadarChart);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NumRadaraData = 0;
            RadarchartPaint(IAM, Color.Red, n, RadarChart);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NumRadaraData = IAM.N - n;
            RadarchartPaint(IAM, Color.Red, n, RadarChart);
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
