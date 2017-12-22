using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace testgistogr
{
    public partial class Form1 : Form
    {
        Point? prevPosition = null; // position
        Point? prevPositionZoom = null; // position zoom
        List<InitialStatisticalAnalys> ML = new List<InitialStatisticalAnalys>();
        List<double[]> undolist = new List<double[]>();
        int Type;
        int SelectGR;
        List<int> MSelectGR = new List<int>();
        InitialAnalysMultidimensionalData Dat;
        VisualisationMultiDataForm VMDF;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.ContextMenuStrip = contextMenuStrip1;
        }
        private void CutDataButton_Click(object sender, EventArgs e)
        {
            SelectGR = WraitData.RefreshChecktTreeView(treeView1)[0];
            if ((String.Compare(CutMaxtextBox.Text, "Max") == 0 && String.Compare(CutMintextBox.Text, "Min") == 0) || ML[SelectGR].l.Count == 0)
            {
                AvtoCut(sender);
                return;
            }
            try
            {
                double dmax, dmin;
                if (String.Compare(CutMaxtextBox.Text, "Max") != 0)
                {
                    if ((dmax = Convert.ToDouble(CutMaxtextBox.Text)) < ML[0].Min.Q)
                        dmax = ML[SelectGR].Max.Q;
                }
                else
                {
                    dmax = ML[SelectGR].Max.Q;
                }
                if (String.Compare(CutMintextBox.Text, "Min") != 0)
                {
                    if ((dmin = Convert.ToDouble(CutMintextBox.Text)) > ML[0].Max.Q)
                        dmin = ML[SelectGR].Min.Q;
                }
                else
                {
                    dmin = ML[SelectGR].Min.Q;
                }
                int i1 = 0, i2 = ML[SelectGR].l.Count - 1;
                for (; ML[SelectGR].l[i1] < dmin; i1++)
                    ;
                for (; ML[SelectGR].l[i2] > dmax; i2--)
                    ;
                double[] HM = new double[i2 - i1 + 1];
                for (int i = 0, o = 0; i < ML[SelectGR].unsortl.Length; i++)
                    if (ML[SelectGR].unsortl[i] >= dmin && ML[SelectGR].unsortl[i] <= dmax)
                        HM[o++] = ML[SelectGR].unsortl[i];
                ML[SelectGR].unsortl = HM;
                ML[SelectGR].l.RemoveRange(i2 + 1, ML[SelectGR].l.Count - i2 - 1);
                ML[SelectGR].l.RemoveRange(0, i1);
                ML[SelectGR].Refresh();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Enter not correct data\n" + ex.Message);
            }
            CutMaxtextBox.Text = "Max";
            CutMintextBox.Text = "Min";
            Colculate(sender, e);
            WraitData.RefreshList(treeView1, ML);
        }
        private void AvtoCut(object sender)
        {
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///CutData_AVTO
            ///++++++++++++++++++++++++++++++++++++++++++++++
            if (((sender.ToString() == "Read") || /*(sender.ToString() == "System.Windows.Forms.Button, Text: Підрахунок") ||*/ (sender.ToString() == "System.Windows.Forms.Button, Text: Обрізання")))
            {
                DialogResult DR = MessageBox.Show("Do you want to automatically edit data?\nMinimal=" + Convert.ToString(ML[SelectGR].a) + "\nMaximum=" + Convert.ToString(ML[SelectGR].b), "Cut Data", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    CutMintextBox.Text = Convert.ToString(ML[SelectGR].a);
                    CutMaxtextBox.Text = Convert.ToString(ML[SelectGR].b);
                    CutDataButton_Click(DR, EventArgs.Empty);
                }
            }
        }
        private void Colculate(object sender, EventArgs e)
        {
            chart3.Visible = true;
            if (ML[SelectGR].l.Count == 0)
            {
                MessageBox.Show("Нет данных");
                return;
            }
            PaintData.Repaint(ML[SelectGR], chart1, chart2);
            WraitData.ReWraitData(MaindatGridView, dataGridView, ML[SelectGR]);
            AvtoFound();
        }
        private void NormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type = 0;
            MaindatGridView[1, 26].Value = "X^2 = " + Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type));
            MaindatGridView[3, 26].Value = "X(a,m)^2 = " + Convert.ToString(Math.Round(Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1), 5));
            MaindatGridView[2, 26].Value = Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type) < Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1));
            MaindatGridView[0, 28].Value = "m";
            MaindatGridView[0, 29].Value = "g";
            MaindatGridView[1, 28].Value = Convert.ToString(ML[SelectGR].Mx.QButton);
            MaindatGridView[3, 28].Value = Convert.ToString(ML[SelectGR].Mx.QUpper);
            MaindatGridView[4, 28].Value = Convert.ToString(Math.Round(ML[SelectGR].Mx.QSigma, 4));
            MaindatGridView[2, 28].Value = Convert.ToString(ML[SelectGR].Mx.Q);
            MaindatGridView[1, 29].Value = Convert.ToString(ML[SelectGR].Dx.QButton);
            MaindatGridView[3, 29].Value = Convert.ToString(ML[SelectGR].Dx.QUpper);
            MaindatGridView[4, 29].Value = Convert.ToString(Math.Round(ML[SelectGR].Dx.QSigma, 4));
            MaindatGridView[2, 29].Value = Convert.ToString(ML[SelectGR].Dx.Q);
            if (ML.Count == 0)
            {
                MessageBox.Show("Нет данных");
                return;
            }
            ChartPrint();
            //double[,] F = Distributions.NormalFRead();
            int i2 = 0;
            for (double i = -0.5; i <= 0.5 && i2 < ML[SelectGR].l.Count; i += (double)1 / ML[SelectGR].l.Count, i2++)
            {
                double dFdMx = -Math.Exp(-Math.Pow((ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q), 2) / (2 * ML[SelectGR].Dx.Q)) / (Math.Sqrt(2 * Math.PI) * ML[SelectGR].Gx.Q);
                double dFdGx = (-(ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q) / (ML[SelectGR].Dx.Q * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow((ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q), 2) / (2 * ML[SelectGR].Dx.Q));
                double DMx = Math.Pow(ML[SelectGR].Gx.Q, 2) / ML[SelectGR].l.Count;
                double DGx = Math.Pow(ML[SelectGR].Gx.Q, 2) / (2 * ML[SelectGR].l.Count);
                double F_G = Math.Sqrt(Math.Pow(dFdMx, 2) * DMx + Math.Pow(dFdGx, 2) * DGx);
                double Y_X = ((ML[SelectGR].l[i2] - ML[SelectGR].Min.Q) / (ML[SelectGR].Len.Q));

                double v1 = Distributions.NormalFobrFound(i);
                chart3.Series["Імовірнісна\n сітка"].Points.AddXY(v1 * ML[SelectGR].Gx.Q + ML[SelectGR].Mx.Q, (ML[SelectGR].l[i2] - ML[SelectGR].Min.Q) / (ML[SelectGR].Len.Q));
                chart3.Series["Y=X"].Points.AddXY(ML[SelectGR].l[i2], (ML[SelectGR].l[i2] - ML[SelectGR].Min.Q) / ML[SelectGR].Len.Q);
                chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].l[i2],
                    Distributions.NormalfFound(ML[SelectGR].l[i2], ML[SelectGR].Mx.Q, ML[SelectGR].Gx.Q) * ML[SelectGR].Step.Q);
                chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].l[i2],
                    /* i + 0.5*/Distributions.NormalFFound((ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q) / ML[SelectGR].Gx.Q));
                double F_low = Distributions.NormalFFound((ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q) / ML[SelectGR].Gx.Q) - Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                double F_upper = Distributions.NormalFFound((ML[SelectGR].l[i2] - ML[SelectGR].Mx.Q) / ML[SelectGR].Gx.Q) + Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                if (F_low < 0)
                    F_low = 0;
                if (F_upper > 1)
                    F_upper = 1;
                chart2.Series["Відтворена F2"].Points.AddXY(ML[SelectGR].l[i2], F_low);
                chart2.Series["Відтворена F3"].Points.AddXY(ML[SelectGR].l[i2], F_upper);

            }
            LinePaint(0);
            chart3.Titles[0].Text = "ψ(x)=F(x,m,g)^(-1)\nобернена функція";
            chart3.Titles[1].Text = "ф(x)=x";
            MaindatGridView[2, 27].Value = Convert.ToString(Math.Round(Kolmagorov.KolmagorovFound(ML[SelectGR].l, ML[SelectGR], Type, ML[SelectGR].Mx.Q, ML[SelectGR].Gx.Q), 4));
        }
        private void ExpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
            {
                MessageBox.Show("Нет данных");
                return;
            }
            Type = 1;
            MaindatGridView[1, 26].Value = "X^2 = " + Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type));
            MaindatGridView[3, 26].Value = "X(a,m)^2 = " + Convert.ToString(Math.Round(Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1), 5));
            MaindatGridView[2, 26].Value = Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type) < Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1));
            MaindatGridView[0, 28].Value = "l";
            double l_G = 1 / (Math.Sqrt(ML.Count) * ML[SelectGR].Mx.Q);
            MaindatGridView[1, 28].Value = Convert.ToString(Math.Round(1 / ML[SelectGR].Mx.Q - l_G, 4));
            MaindatGridView[3, 28].Value = Convert.ToString(Math.Round(1 / ML[SelectGR].Mx.Q + l_G, 4));
            MaindatGridView[4, 28].Value = Convert.ToString(Math.Round(l_G, 5));
            MaindatGridView[2, 28].Value = Convert.ToString(Math.Round(1 / ML[SelectGR].Mx.Q, 4));
            MaindatGridView[0, 29].Value = "";
            MaindatGridView[1, 29].Value = "";
            MaindatGridView[3, 29].Value = "";
            MaindatGridView[4, 29].Value = "";
            MaindatGridView[2, 29].Value = "";
            ChartPrint();
            double l = 1 / (ML[SelectGR].Mx.Q - ML[SelectGR].Min.Q);
            for (int i = 0; i < ML[SelectGR].l.Count; i++)
            {
                double v1 = 0;
                double Y_X = (ML[SelectGR].l[i] / ML[SelectGR].Len.Q);
                if (ML[SelectGR].F[i] != 1)
                {
                    v1 = Math.Log(1 / (1 - ML[SelectGR].F[i]), 2.73) / (ML[SelectGR].Len.Q * l);
                }
                else
                    v1 = 1;
                double F_G = Math.Pow(ML[SelectGR].l[i], 2) * Math.Exp(-2 * l * (ML[SelectGR].l[i])) * l * l / ML[SelectGR].l.Count;
                F_G = Math.Sqrt(F_G);
                //F_G *= Normal.Quantile(alf);
                double f = Math.Exp(-(l) * (ML[SelectGR].l[i])) * ML[SelectGR].Step.Q / ML[SelectGR].Mx.Q;
                if (f > 1)
                    f = 1;
                chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].l[i], f);
                chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].l[i], 1 - Math.Pow(2.73, -(l) * (ML[SelectGR].l[i])));
                chart3.Series["Імовірнісна\n сітка"].Points.AddXY(ML[SelectGR].l[i], v1);
                chart3.Series["Y=X"].Points.AddXY(ML[SelectGR].l[i], Y_X);

                double F_low = 1 - Math.Pow(2.73, -(l) * ML[SelectGR].l[i]) - Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                double F_upper = 1 - Math.Pow(2.73, -(l) * ML[SelectGR].l[i]) + Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                if (F_low < 0)
                    F_low = 0;
                if (F_upper > 1)
                    F_upper = 1;
                chart2.Series["Відтворена F2"].Points.AddXY(ML[SelectGR].l[i], F_low);
                chart2.Series["Відтворена F3"].Points.AddXY(ML[SelectGR].l[i], F_upper);
            }
            chart3.Titles[0].Text = "ψ(x)=x";
            chart3.Titles[1].Text = "ф(x)=Ln(1/(1-F(x,λ)))/((x(max)-x(min))*λ)";
            LinePaint(1);
            MaindatGridView[2, 27].Value = Convert.ToString(Math.Round(Kolmagorov.KolmagorovFound(ML[SelectGR].l, ML[SelectGR], Type, ML[SelectGR].Mx.Q, ML[SelectGR].Gx.Q), 4));
        }
        private void SataticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type = 2;
            double dH1dx = 1 + 3 * ((ML[SelectGR].B + ML[SelectGR].A) / (ML[SelectGR].B - ML[SelectGR].A));
            double dH1dxx = -3 * (1 / (ML[SelectGR].B - ML[SelectGR].A));
            double dH2dx = 1 - 3 * ((ML[SelectGR].B + ML[SelectGR].A) / (ML[SelectGR].B - ML[SelectGR].A));
            double dH2dxx = 3 * (1 / (ML[SelectGR].B - ML[SelectGR].A));
            double DX = Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 2) / (12 * ML[SelectGR].l.Count);
            double covx_xx = (ML[SelectGR].B + ML[SelectGR].A) * Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 2) / (12 * ML[SelectGR].l.Count);
            double DXX = (1.0 / (180.0 * ML[SelectGR].l.Count)) * (Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 4) + 15 * Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 2) * Math.Pow(ML[SelectGR].B + ML[SelectGR].A, 2));
            double A_G = Math.Sqrt(Math.Abs(Math.Pow(dH1dx, 2) * DX + Math.Pow(dH1dxx, 2) * DXX + 2 * dH1dx * dH1dxx * covx_xx));
            double B_G = Math.Sqrt(Math.Abs(Math.Pow(dH2dx, 2) * DX + Math.Pow(dH2dxx, 2) * DXX + 2 * dH2dx * dH2dxx * covx_xx));
            MaindatGridView[1, 26].Value = "X^2 = " + Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type));
            MaindatGridView[3, 26].Value = "X(a,m)^2 = " + Convert.ToString(Math.Round(Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1), 5));
            MaindatGridView[2, 26].Value = Convert.ToString(Hi.HiSqurdFound(ML[SelectGR], Type) < Hi.HIF(ML[SelectGR].alf.Q, (int)ML[SelectGR].m.Q - 1));
            MaindatGridView[0, 28].Value = "a";
            MaindatGridView[1, 28].Value = Convert.ToString(Math.Round(ML[SelectGR].A - A_G, 4));
            MaindatGridView[3, 28].Value = Convert.ToString(Math.Round(ML[SelectGR].A + A_G, 4));
            MaindatGridView[4, 28].Value = Convert.ToString(Math.Round(A_G, 5));
            MaindatGridView[2, 28].Value = Convert.ToString(Math.Round(ML[SelectGR].A, 4));
            MaindatGridView[0, 29].Value = "b";
            MaindatGridView[1, 29].Value = Convert.ToString(Math.Round(ML[SelectGR].B - B_G, 4));
            MaindatGridView[3, 29].Value = Convert.ToString(Math.Round(ML[SelectGR].B + B_G, 4));
            MaindatGridView[4, 29].Value = Convert.ToString(Math.Round(B_G, 5));
            MaindatGridView[2, 29].Value = Convert.ToString(Math.Round(ML[SelectGR].B, 4));
            if (ML.Count == 0)
            {
                MessageBox.Show("Нет данных");
                return;
            }
            ChartPrint();
            for (int i = 0; i < ML[SelectGR].l.Count - 1; i++)
            {
                double Y_X = ((ML[SelectGR].l[i] - ML[SelectGR].Min.Q) / ML[SelectGR].Len.Q);
                chart3.Series["Імовірнісна\n сітка"].Points.AddXY(ML[SelectGR].l[i], ML[SelectGR].F[i]);
                chart3.Series["Y=X"].Points.AddXY(ML[SelectGR].l[i], Y_X);
            }
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].A - 0.1 * (ML[SelectGR].B - ML[SelectGR].A), 0);
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].A, 0);
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].A, (double)1 / ML[SelectGR].m.Q);
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].B, (double)1 / ML[SelectGR].m.Q);
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].B, 0);
            chart1.Series["Відтворена f"].Points.AddXY(ML[SelectGR].B + 0.1 * (ML[SelectGR].B - ML[SelectGR].A), 0);
            chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].A - 0.1 * (ML[SelectGR].B - ML[SelectGR].A), 0);
            chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].A, 0);
            chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].B, 1);
            chart2.Series["Відтворена F"].Points.AddXY(ML[SelectGR].B + 0.1 * (ML[SelectGR].B - ML[SelectGR].A), 1);
            double cov_AB = dH1dx * dH2dx * DX + dH1dxx * dH2dxx * DXX + (dH1dx * dH2dxx + dH2dx * dH1dxx) * covx_xx;
            for (double i = ML[SelectGR].A; i <= ML[SelectGR].B; i += (ML[SelectGR].B - ML[SelectGR].A) / 100)
            {
                double J1 = Math.Pow((A_G * (i - ML[SelectGR].B)) / Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 2), 2);
                double J2 = Math.Pow((B_G * (i - ML[SelectGR].A)) / Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 2), 2);
                double J3 = (double)2 * (i - ML[SelectGR].A) * (i - ML[SelectGR].B) * cov_AB / Math.Pow(ML[SelectGR].B - ML[SelectGR].A, 4);
                double F_G = Math.Sqrt(J1 + J2 - J3);
                double F_low = (i - ML[SelectGR].A) / (ML[SelectGR].B - ML[SelectGR].A) - Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                double F_upper = (i - ML[SelectGR].A) / (ML[SelectGR].B - ML[SelectGR].A) + Distributions.NormalQuantile(1 - ML[SelectGR].alf.Q / 2) * F_G;
                if (F_low < 0)
                    F_low = 0;
                if (F_upper > 1)
                    F_upper = 1;
                chart2.Series["Відтворена F2"].Points.AddXY(i, F_low);
                chart2.Series["Відтворена F3"].Points.AddXY(i, F_upper);
            }

            chart3.Titles[0].Text = "ψ(x)=x";
            chart3.Titles[1].Text = "ф(x)=F(x,a,b)";

            LinePaint(2);
            MaindatGridView[2, 27].Value = Convert.ToString(Math.Round(Kolmagorov.KolmagorovFound(ML[SelectGR].l, ML[SelectGR], Type, ML[SelectGR].Mx.Q, ML[SelectGR].Gx.Q), 4));
        }
        private void ChartPrint()
        {
            if (ML.Count == 0)
            {
                MessageBox.Show("Нет данных");
                return;
            }
            PaintData.Repaint(ML[SelectGR], chart1, chart2);
            chart1.Series.Add("Відтворена f");
            chart1.Series["Відтворена f"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["Відтворена f"]["PointWidth"] = "2";
            chart1.Series["Відтворена f"].BorderWidth = 2;
            chart1.Series["Відтворена f"].Color = Color.Red;
            chart1.Series["Відтворена f"].BorderColor = Color.Red;
            chart2.Series.Add("Відтворена F3");
            chart2.Series["Відтворена F3"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Відтворена F3"]["PointWidth"] = "1";
            chart2.Series["Відтворена F3"].BorderWidth = 1;
            chart2.Series["Відтворена F3"].IsVisibleInLegend = false;
            chart2.Series["Відтворена F3"].Color = Color.Black;
            chart2.Series["Відтворена F3"].BorderColor = Color.Black;
            chart2.Series.Add("Відтворена F2");
            chart2.Series["Відтворена F2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Відтворена F2"]["PointWidth"] = "1";
            chart2.Series["Відтворена F2"].IsVisibleInLegend = false;
            chart2.Series["Відтворена F2"].BorderWidth = 1;
            chart2.Series["Відтворена F2"].Color = Color.Black;
            chart2.Series["Відтворена F2"].BorderColor = Color.Black;
            chart2.Series.Add("Відтворена F");
            chart2.Series["Відтворена F"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Відтворена F"]["PointWidth"] = "2";
            chart2.Series["Відтворена F"].BorderWidth = 2;
            chart2.Series["Відтворена F"].Color = Color.Blue;
            chart2.Series["Відтворена F"].BorderColor = Color.Blue;
            chart3.Series.Clear();
            chart3.Series.Add("Імовірнісна\n сітка");
            chart3.Series["Імовірнісна\n сітка"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart3.Series["Імовірнісна\n сітка"]["PointWidth"] = "1";
            chart3.Series["Імовірнісна\n сітка"].BorderWidth = 1;
            chart3.Series["Імовірнісна\n сітка"].Color = Color.DarkBlue;
            chart3.Series["Імовірнісна\n сітка"].BorderColor = Color.Black;
            chart3.Series.Add("Y=X");
            chart3.Series["Y=X"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Y=X"]["PointWidth"] = "1";
            chart3.Series["Y=X"].BorderWidth = 1;
            chart3.Series["Y=X"].Color = Color.DarkRed;
            chart3.Series["Y=X"].BorderColor = Color.Red;
            chart3.ChartAreas[0].AxisX.Minimum = Math.Round(ML[SelectGR].Min.Q - 0.1 * ML[SelectGR].Len.Q, 5);
            chart3.ChartAreas[0].AxisX.Maximum = Math.Round(ML[SelectGR].Max.Q + 0.1 * ML[SelectGR].Len.Q, 5);
            chart3.ChartAreas[0].AxisY.Interval = 1;
            chart3.ChartAreas[0].AxisY.Minimum = 0;
            chart3.ChartAreas[0].AxisY.Maximum = 1.3;
        }
        private void LinePaint(int l)
        {
            chart3.ChartAreas[0].AxisY.CustomLabels.Clear();
            chart3.ChartAreas[0].AxisY.CustomLabels.Add(-0.1, 0.1, Convert.ToString(0));
            chart3.ChartAreas[0].AxisY.CustomLabels.Add(0.9, 1.1, Convert.ToString(1));
            double[] b = { 0.05, 0.1, 0.25, 0.5, 0.75, 0.9, 0.95 };
            for (int i = 0; i < b.Length; i++)
            {
                chart3.Series.Add(b[i].ToString());
                chart3.Series[b[i].ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series[b[i].ToString()]["PointWidth"] = "1";
                chart3.Series[b[i].ToString()].BorderWidth = 1;
                chart3.Series[b[i].ToString()].Color = Color.Black;
                chart3.Series[b[i].ToString()].BorderColor = Color.Black;
                chart3.Series[b[i].ToString()].IsVisibleInLegend = false;
                if (l == 0)
                {
                    //Normal
                    chart3.ChartAreas[0].AxisY.CustomLabels.Add((Distributions.NormalQuantile(b[i]) * ML[SelectGR].Gx.Q + ML[SelectGR].Mx.Q - ML[SelectGR].Min.Q) / ML[SelectGR].Len.Q - 0.1, (Distributions.NormalQuantile(b[i]) * ML[SelectGR].Gx.Q + ML[SelectGR].Mx.Q - ML[SelectGR].l[0]) / ML[SelectGR].Len.Q + 0.1, b[i].ToString());
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Min.Q - 0.1 * ML[SelectGR].Len.Q, (Distributions.NormalQuantile(b[i]) * ML[SelectGR].Gx.Q + ML[SelectGR].Mx.Q - ML[SelectGR].Min.Q) / ML[SelectGR].Len.Q);
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Max.Q + 0.1 * ML[SelectGR].Len.Q, (Distributions.NormalQuantile(b[i]) * ML[SelectGR].Gx.Q + ML[SelectGR].Mx.Q - ML[SelectGR].Min.Q) / ML[SelectGR].Len.Q);
                }
                else if (l == 1)
                {
                    double v1 = Math.Log(1 / (1 - b[i]), 2.73) / ML[SelectGR].Max.Q * (1 / (ML[SelectGR].Mx.Q));
                    chart3.ChartAreas[0].AxisY.CustomLabels.Add(v1 - 0.1, v1 + 0.1, b[i].ToString());
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Min.Q - 0.1 * ML[SelectGR].Len.Q, v1);
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Max.Q + 0.1 * ML[SelectGR].Len.Q, v1);
                }
                else if (l == 2)
                {
                    chart3.ChartAreas[0].AxisY.CustomLabels.Add(b[i] - 0.1, b[i] + 0.1, b[i].ToString());
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Min.Q - 0.1 * ML[SelectGR].Len.Q, b[i]);
                    chart3.Series[b[i].ToString()].Points.AddXY(ML[SelectGR].Max.Q + 0.1 * ML[SelectGR].Len.Q, b[i]);
                }
            }
        }
        /* private void зберегтиДаніToolStripMenuItem_Click(object sender, EventArgs e)
         {
             OpenFileDialog openFileDialog1 = new OpenFileDialog();
             openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
             openFileDialog1.FilterIndex = 1;
             openFileDialog1.RestoreDirectory = true;
             if (openFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                 StreamWriter streamW = new StreamWriter(fs, Encoding.ASCII);
                 try
                 {
                     for (int i = 0; i < ML.Count; i++)
                     {
                         for (int j = 0; j < ML.Count;j++ )
                             streamW.Write(ML[j].l[i] + " ");
                         streamW.Write("\n");
                     }
                     streamW.Close();
                     fs.Close();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message);
                 }
                 Colculate(sender, e);
             }
         }*/
        private void AvtoFound()
        {
            if (ML[SelectGR].AvtoType == Distributions.Normal)
            {
                MaindatGridView[2, 18].Value = Distributions.Normal;
                NormalToolStripMenuItem_Click(ML[SelectGR].AvtoType, EventArgs.Empty);
            }
            else if (ML[SelectGR].AvtoType == Distributions.Exp)
            {
                MaindatGridView[2, 18].Value = Distributions.Exp;
                ExpToolStripMenuItem1_Click(ML[SelectGR].AvtoType, EventArgs.Empty);
            }
            else if (ML[SelectGR].AvtoType == Distributions.Line)
            {
                MaindatGridView[2, 18].Value = Distributions.Line;
                SataticToolStripMenuItem_Click(ML[SelectGR].AvtoType, EventArgs.Empty);
            }
        }

        #region chart
        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var pos = e.Location;
                if (prevPosition.HasValue && pos == prevPosition.Value)
                    return;
                tooltip.RemoveAll();
                prevPosition = pos;
                var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
                foreach (var result in results)
                {
                    if (result.ChartElementType == ChartElementType.PlottingArea)
                    {
                        var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                        var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                        tooltip.Show("X=" + Math.Round(xVal, 5) + "\nY=" + Math.Round(yVal, 5), this.chart1, 0, this.chart1.Size.Height - 30   /*pos.X, pos.Y - 15*/);
                    }
                }
            }
            catch { }
        }
        private void Chart2_MouseMove(object sender, MouseEventArgs e)
        {
            if (MSelectGR == null || MSelectGR.Count != 1)
                return;
            try
            {
                var pos = e.Location;
                if (prevPosition.HasValue && pos == prevPosition.Value)
                    return;
                tooltip.RemoveAll();
                prevPosition = pos;
                var results = chart2.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
                foreach (var result in results)
                {
                    if (result.ChartElementType == ChartElementType.PlottingArea)
                    {
                        var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                        var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                        tooltip.Show("X=" + Math.Round(xVal, 5) + "\nY=" + Math.Round(yVal, 5), this.chart2, 0, this.chart2.Size.Height - 30);
                    }
                }
            }
            catch
            {

            }
        }
        private void Chart2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && MSelectGR != null && MSelectGR.Count == 1)
            {


                chart2.ChartAreas[0].AxisX.Minimum = Math.Round(ML[MSelectGR[0]].Min.Q - 0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart2.ChartAreas[0].AxisX.Maximum = Math.Round(ML[MSelectGR[0]].Max.Q + 0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart2.ChartAreas[0].AxisX.Interval = Math.Round(0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart2.ChartAreas[0].AxisY.Interval = 0.1;
                chart2.ChartAreas[0].AxisY.Minimum = 0;
                chart2.ChartAreas[0].AxisY.Maximum = 1;
            }
            var pos = e.Location;
            prevPositionZoom = pos;
        }
        private void Chart2_MouseUp(object sender, MouseEventArgs e)
        {

            if (prevPositionZoom != null && MSelectGR.Count <= 1) {
                var pos = e.Location;
                var results = chart2.HitTest(prevPositionZoom.Value.X, prevPositionZoom.Value.Y, false, ChartElementType.PlottingArea);
                double[] xVal = new double[2];
                double[] yVal = new double[2];
                if (results[0].ChartElementType == ChartElementType.PlottingArea)
                {
                    xVal[0] = results[0].ChartArea.AxisX.PixelPositionToValue(prevPositionZoom.Value.X);
                    yVal[0] = results[0].ChartArea.AxisY.PixelPositionToValue(prevPositionZoom.Value.Y);
                }
                var results2 = chart2.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
                if (results2[0].ChartElementType == ChartElementType.PlottingArea)
                {
                    xVal[1] = results2[0].ChartArea.AxisX.PixelPositionToValue(pos.X);
                    yVal[1] = results2[0].ChartArea.AxisY.PixelPositionToValue(pos.Y);
                    if (xVal.Max() - xVal.Min() > 0.03 * (chart2.ChartAreas[0].AxisX.Maximum - chart2.ChartAreas[0].AxisX.Minimum))
                    {
                        if (yVal.Max() - yVal.Min() > 0.03 * (chart2.ChartAreas[0].AxisY.Maximum - chart2.ChartAreas[0].AxisY.Minimum))
                        {
                            chart2.ChartAreas[0].AxisX.Minimum = Math.Round(xVal.Min(), 4);
                            chart2.ChartAreas[0].AxisY.Minimum = Math.Round(yVal.Min(), 4);
                            chart2.ChartAreas[0].AxisX.Maximum = Math.Round(xVal.Max(), 4);
                            chart2.ChartAreas[0].AxisY.Maximum = Math.Round(yVal.Max(), 4);
                        }
                    }
                }
            }
        }
        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            prevPositionZoom = e.Location;
            if (e.Button == MouseButtons.Right && MSelectGR != null && MSelectGR.Count == 1)
            {

                chart1.ChartAreas[0].AxisX.Minimum = Math.Round(ML[MSelectGR[0]].Min.Q - 0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart1.ChartAreas[0].AxisX.Maximum = Math.Round(ML[MSelectGR[0]].Max.Q + 0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart1.ChartAreas[0].AxisX.Interval = Math.Round(0.1 * (ML[MSelectGR[0]].Len.Q), 3);
                chart1.ChartAreas[0].AxisY.Interval = 0.05;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = ML[MSelectGR[0]].f.Max() + 0.07;
            }
            else if (e.Button == MouseButtons.Right && MSelectGR != null)
            {
                chart1.ChartAreas[0].AxisX.Minimum = Math.Round(ML[MSelectGR[0]].Min.Q, 3);
                chart1.ChartAreas[0].AxisX.Maximum = ML[MSelectGR[0]].Max.Q;
                chart1.ChartAreas[0].AxisX.Interval = Math.Round(ML[MSelectGR[0]].Len.Q * 0.1, 3);
                chart1.ChartAreas[0].AxisY.Interval = Math.Round(ML[MSelectGR[1]].Len.Q * 0.1, 3);
                chart1.ChartAreas[0].AxisY.Minimum = ML[MSelectGR[1]].Min.Q;
                chart1.ChartAreas[0].AxisY.Maximum = ML[MSelectGR[1]].Max.Q;
            }
        }
        private void Chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (prevPositionZoom != null)
            {
                var pos = e.Location;
                var results = chart1.HitTest(prevPositionZoom.Value.X, prevPositionZoom.Value.Y, false, ChartElementType.PlottingArea);
                double[] xVal = new double[2];
                double[] yVal = new double[2];
                if (results[0].ChartElementType == ChartElementType.PlottingArea)
                {
                    xVal[0] = results[0].ChartArea.AxisX.PixelPositionToValue(prevPositionZoom.Value.X);
                    yVal[0] = results[0].ChartArea.AxisY.PixelPositionToValue(prevPositionZoom.Value.Y);
                }
                var results2 = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
                if (results2[0].ChartElementType == ChartElementType.PlottingArea)
                {
                    xVal[1] = results2[0].ChartArea.AxisX.PixelPositionToValue(pos.X);
                    yVal[1] = results2[0].ChartArea.AxisY.PixelPositionToValue(pos.Y);

                    if ((e.Button == MouseButtons.Left) &&
                            (xVal.Max() - xVal.Min() > 0.03 * (chart1.ChartAreas[0].AxisX.Maximum - chart1.ChartAreas[0].AxisX.Minimum)) &&
                            (yVal.Max() - yVal.Min() > 0.03 * (chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum)))
                    {
                        chart1.ChartAreas[0].AxisX.Minimum = Math.Round(xVal.Min(), 4);
                        chart1.ChartAreas[0].AxisY.Minimum = Math.Round(yVal.Min(), 4);
                        chart1.ChartAreas[0].AxisX.Maximum = Math.Round(xVal.Max(), 4);
                        chart1.ChartAreas[0].AxisY.Maximum = Math.Round(yVal.Max(), 4);
                        chart1.ChartAreas[0].AxisY.Interval = 0.2 * (chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum);
                        chart1.ChartAreas[0].AxisX.Interval = 0.2 * (chart1.ChartAreas[0].AxisX.Maximum - chart1.ChartAreas[0].AxisX.Minimum);
                    }
                    else if (e.Button == MouseButtons.Middle)
                    {
                        chart1.ChartAreas[0].AxisX.Minimum -= Math.Round(xVal[1] - xVal[0], 4);
                        chart1.ChartAreas[0].AxisY.Minimum -= Math.Round(yVal[1] - yVal[0], 4);
                        chart1.ChartAreas[0].AxisX.Maximum -= Math.Round(xVal[1] - xVal[0], 4);
                        chart1.ChartAreas[0].AxisY.Maximum -= Math.Round(yVal[1] - yVal[0], 4);
                    }
                }
            }
        }
        #endregion

        #region treeview
        private void TreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            treeView1.SelectedNode.Toggle();////CHANGE!!!!!!!!
            if (e.Button != MouseButtons.Left)
                return;
            if (ModifierKeys == Keys.Control)
            {
                ML[treeView1.SelectedNode.Index].Group--;
            }
            else
            {
                ML[treeView1.SelectedNode.Index].Group++;
            }
            this.tabControl1.Select();
            WraitData.RefreshList(treeView1, ML);
        }
        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

            this.treeView1.AfterCheck -= new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterCheck);
            if (ModifierKeys == Keys.Control)
            {
                bool chek = e.Node.Checked;
                string[] tx = Regex.Split(e.Node.Text, "::");
                if (tx.Length < 2)
                    return;
                int group = Convert.ToInt32(tx[1]);
                for (int i = 0; i < ML.Count; i++)
                {
                    if (ML[i].Group == group)
                        treeView1.Nodes[i].Checked = chek;
                }
            }
            //this.treeView1
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterCheck);
        }
        #endregion

        #region ButtonsAndMenuStrip
        #region MenuStrip1
        #region File
        private void ReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs, Encoding.ASCII);
                int i = -1;
                string[] s = null;
                try
                {
                    String str = streamReader.ReadToEnd();
                    str = str.Replace("\r", " ");
                    str = Regex.Replace(str.Replace("\t", " ").Trim(' '), " +", " ");
                    str = str.Replace("  ", " ");
                    str = str.Replace(" \n", "\n");
                    str = str.Replace("\n ", "\n");
                    str = str.Replace(",", ".");
                    str = str.Replace(";", " ");
                    s = Regex.Split(str, "\n");
                    string[] strN = Regex.Split(s[0], " ");
                    List<double>[] A = new List<double>[strN.Length];
                    for (int k = 0; k < strN.Length; k++)
                        A[k] = new List<double>();
                    for (; i < s.Length - 1;)
                    {
                        i++;
                        strN = Regex.Split(s[i], " ");
                        if (strN.Length != 0)
                            for (int j = 0; j < strN.Length; j++)
                                A[j].Add(Convert.ToDouble(strN[j]));
                    }
                    int MaxGroup = -1;
                    for (int j = 0; j < ML.Count; j++)
                        MaxGroup = Math.Max(MaxGroup, ML[j].Group);

                    for (int j = 0; j < A.Length; j++)
                    {
                        ML.Add(new InitialStatisticalAnalys(A[j], MaxGroup + 1));
                        double[] hm = new double[A[j].Count];
                        A[j].CopyTo(hm);
                        undolist.Add(hm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message + "\n" + s[i]
                        + "\n" + ex.Message);
                }
                finally
                {
                    streamReader.Close();
                    fs.Close();
                }
                chart3.Series.Clear();
                WraitData.RefreshList(treeView1, ML);
                SelectGR = 0;
                MSelectGR.Add(SelectGR);
                Colculate(sender, e);
            }
        }
        private void SaveDiogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chart1.SaveImage(openFileDialog1.FileName, ChartImageFormat.Bmp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message);
                }
            }
        }
        private void SaveFuncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chart2.SaveImage(openFileDialog1.FileName, ChartImageFormat.Bmp);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message);
                }
            }
        }
        private void СтворитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            bool Dvom = (MSelectGR.Count == 1);
            bool Mnog = (MSelectGR.Count > 1);
            CreatData crdat = new CreatData();
            object[] ob;
            if (Dvom)
                ob = crdat.SaveData(ML[MSelectGR[0]].unsortl);
            else if (Mnog)
                ob = crdat.SaveData(new InitialAnalysMultidimensionalData(ML, MSelectGR, -1));
            else
                ob = crdat.SaveData();
            if (ob == null)
                return;
            List<double> data = (List<double>)ob[0];
            if ((int)ob[1] == 0)
            {
                ML.Add(new InitialStatisticalAnalys(data));
                double[] hm = new double[data.Count];
                data.CopyTo(hm);
                undolist.Add(hm);
                WraitData.RefreshList(treeView1, ML);
            }
            else if ((int)ob[1] == 1)
            {
                SaveFileDialog openFileDialog1 = new SaveFileDialog()
                {
                    Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    RestoreDirectory = true
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate);
                    StreamWriter streamW = new StreamWriter(fs, Encoding.ASCII);
                    try
                    {
                        if (!Dvom && !Mnog)
                            for (int i = 0; i < data.Count; i++)
                            {
                                streamW.Write(data[i]);
                                if (i < data.Count - 1)
                                    streamW.WriteLine();
                            }
                        else if (Dvom)
                            for (int i = 0; i < data.Count; i++)
                            {
                                streamW.Write(ML[MSelectGR[0]].unsortl[i].ToString() + " " + data[i].ToString());
                                if (i < data.Count - 1)
                                    streamW.WriteLine();
                            }
                        else if (Mnog)
                            for (int i = 0; i < data.Count; i++)
                            {
                                for (int j = 0; j < MSelectGR.Count; j++)
                                    streamW.Write(ML[MSelectGR[j]].unsortl[i].ToString() + " ");
                                streamW.Write(data[i].ToString());
                                if (i < data.Count - 1)
                                    streamW.WriteLine();
                            }
                        streamW.Close();
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Неможливо зберегти дані\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

#region Ttest
        private void TtestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Dat is InitialAnalysMultidimensionalData)
            {
                //12
                double[] Akor = new double[(Dat).n - 1];
                double[] T = new double[(Dat).n - 1];
                try
                {
                    for (int i = 0; i < Akor.Length; i++)
                    {
                        int ireal = i;
                        if (i >= (Dat).k)
                            ireal++;
                        Akor[i] = Convert.ToDouble(MaindatGridView[ireal + 1, 14].Value);
                    }
                }
                catch
                { return; }
                for (int i = 0; i < Akor.Length; i++)
                    T[i] = ((Dat).A[i + 1].Q - Akor[i]) /
                        (
                        (Dat).Szal *
                        Math.Sqrt((Dat).C[i, i])
                        );
                double TQvantile = Distributions.StudentQuantile(
                    (Dat).ISA[0].alf.Q,
                    (Dat).N - (Dat).n);

                MaindatGridView[0, 18].Value = "T = " + Math.Round(TQvantile, 4).ToString();
                for (int i = 0; i < Akor.Length; i++)
                {
                    int ireal = i;
                    if (i >= (Dat).k)
                        ireal++;
                    MaindatGridView[ireal + 1, 15].Value = Math.Round(T[i], 4);
                    MaindatGridView[ireal + 1, 16].Value = Math.Abs(T[i]) < TQvantile;
                }
            }
            else
            {
                MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
                if (MaindatGridView.Columns.Count < 6)
                    return;
                for (int i = 0; i < MaindatGridView.Rows.Count; i++)
                {
                    if (Convert.ToString(MaindatGridView[2, i].Value) != "" && Convert.ToString(MaindatGridView[4, i].Value) != "" && Convert.ToString(MaindatGridView[5, i].Value) != "")
                    {
                        try
                        {
                            double
                                Q = Convert.ToDouble(MaindatGridView[2, i].Value),
                                gQ = Convert.ToDouble(MaindatGridView[4, i].Value),
                                Q0 = Convert.ToDouble(MaindatGridView[5, i].Value);
                            MaindatGridView[6, i].Value = "";
                            if (MSelectGR.Count == 1)
                                MaindatGridView[6, i].Value = Convert.ToString(Math.Abs(Math.Round((Q0 - Q) / gQ, 4)) < ML[MSelectGR[0]].T);
                            else if (i < 20)
                                MaindatGridView[6, i].Value = Convert.ToString(Math.Abs(Math.Round((Q0 - Q) / gQ, 4)) < Distributions.StudentQuantile(1 - ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2));
                            else
                                MaindatGridView[6, i].Value = Convert.ToString(Math.Abs(Math.Round((Q0 - Q) / gQ, 4)) < Distributions.StudentQuantile(1 - ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 3));

                        }
                        catch { }
                    }
                }
            }
        }
        #endregion

#region About
        /// <summary>
        /// About a program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа створена Здотою Дмитром", "Про програму");
        }
        #endregion
        #endregion  

#region contextMenuStrip1

        private void ПідрахуватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VMDF != null)
            {
                MultiThreadClass.StopInNewThread(VMDF);
                VMDF = null;
            }
            chart2.ChartAreas[0].BackImage = "";
            chart2.Images.Clear();
            while (tabControl1.Controls.Count > 2)
                tabControl1.Controls.RemoveAt(2);
            List<List<int>> ind = WraitData.RefreshChecktTreeViewMnog(treeView1);
            if (ind.Count == 0)
                return;
            double LenInd = ind[0].Count;
            for (int i = 1; i < ind.Count; i++)
                if (LenInd != ind[i].Count)
                {
                    LenInd = -1;
                    break;
                }
            if (ind.Count == 1 && ind[0].Count == 1)
            {
                SelectGR = ind[0][0];
                Colculate(sender, EventArgs.Empty);
            }
            else
            {
                chart3.Visible = false;
                SelectGR = -1;
                if (ind.Count == 1 && ind[0].Count == 2)
                {
                    MSelectGR.Clear();
                    MSelectGR.Add(ind[0][0]);
                    MSelectGR.Add(ind[0][1]);

                    AdditionalInformationForm askF = new AdditionalInformationForm("Виберіть тип регресї");
                    string Type = askF.SelectType("Avto\n" + RegresTypeName.TypeRegresion);
                    if (Type == null)
                        return;
                    if (Type == "Avto")
                        Type = Correlation_RegressionAnalysis.TypeRegresFound(ML[MSelectGR[0]], ML[MSelectGR[1]]);
                    Correlation_RegressionAnalysis CRA = new Correlation_RegressionAnalysis(ML, MSelectGR, Type);
                    //SimpleClass SC = new SimpleClass(ML, MSelectGR, BinSim);
                    if (MessageBox.Show("Виділити вибірку е", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        List<double> E = new List<double>();
                        for (int i = 0; i < ML[MSelectGR[0]].unsortl.Length; i++)
                        {
                            E.Add(ML[MSelectGR[1]].unsortl[i] - RegresType.Model(ML[MSelectGR[0]].unsortl[i], CRA.Q, CRA.RegresTypeVib));
                        }

                        ML.Add(new InitialStatisticalAnalys(E));
                        double[] hm = new double[E.Count];
                        E.CopyTo(hm);
                        undolist.Add(hm);
                        WraitData.RefreshList(treeView1, ML);
                    }
                    if (ML[MSelectGR[0]].l.Count == ML[MSelectGR[1]].l.Count)
                    {
                        PaintData.Repaint(ML[MSelectGR[0]], ML[MSelectGR[1]], CRA, chart1, chart2);
                        MainComponentMethod MCM = new MainComponentMethod(new InitialAnalysMultidimensionalData(ML, ind[0],- 1),
                            ref ML, ref undolist, ref treeView1);
                        tabControl1.Controls.Add(MCM.DataTabPage);

                        Classterization classterizatin = new Classterization(new InitialAnalysMultidimensionalData(ML, ind[0], -1),
                            ref ML, ref undolist, ref treeView1);
                        tabControl1.Controls.Add(classterizatin.ClassterizationTabPage);

                        Classification classification = new Classification(new InitialAnalysMultidimensionalData(ML, ind[0], -1), ind[0].Count - 1);
                        tabControl1.Controls.Add(classification.ClassificationTabPage);
                    }
                    else
                    {
                        chart1.Series.Clear();
                        chart2.Series.Clear();
                        chart3.Series.Clear();
                    }
                    WraitData.ReWraitData(MaindatGridView, dataGridView, CRA, ML, MSelectGR);
                }
                else if (ind.Count == 1 && ind[0].Count > 2)
                {
                    chart1.Series.Clear();
                    chart2.Series.Clear();
                    chart3.Series.Clear();
                    AdditionalInformationForm AIF = new AdditionalInformationForm("Введіть к для регресії.");
                    int k;
                    try
                    {
                        k = Convert.ToInt32(AIF.getString());
                    }
                    catch
                    { return; }
                    if (k <= 0 || k > ind[0].Count)
                        return;
                    Dat = new InitialAnalysMultidimensionalData(ML, ind[0], k - 1);
                    VMDF = new VisualisationMultiDataForm((InitialAnalysMultidimensionalData)Dat);

                    MultiThreadClass.RunInNewThread(VMDF, true);

                    SubCorelationClass SCC = new SubCorelationClass((InitialAnalysMultidimensionalData)Dat);
                    tabControl1.Controls.Add(SCC.CorelTabPage);

                    MainComponentMethod MCM = new MainComponentMethod(Dat as InitialAnalysMultidimensionalData,
                            ref ML, ref undolist, ref treeView1);
                    tabControl1.Controls.Add(MCM.DataTabPage);

                    Classterization classterizatin = new Classterization(new InitialAnalysMultidimensionalData(ML, ind[0], -1),
                        ref ML, ref undolist, ref treeView1);
                    tabControl1.Controls.Add(classterizatin.ClassterizationTabPage);

                    Classification classification = new Classification(new InitialAnalysMultidimensionalData(ML, ind[0], -1), ind[0].Count - 1);
                    tabControl1.Controls.Add(classification.ClassificationTabPage);

                    WraitData.ReWraitData(MaindatGridView, dataGridView, (DataGridView)SCC.CorelTabPage.Controls[0], (InitialAnalysMultidimensionalData)Dat, ML, ind[0]);
                }
                else if (LenInd == 1 && ind.Count != 1)
                {
                    chart1.Series.Clear();
                    chart2.Series.Clear();
                    chart3.Series.Clear();

                    MSelectGR.Clear();
                    for (int i = 0; i < ind.Count; i++)
                        for (int j = 0; j < ind[i].Count; j++)
                            MSelectGR.Add(ind[i][j]);

                    UniformityCriteria UC = new UniformityCriteria(ML, MSelectGR);
                    WraitData.ReWraitData(MaindatGridView, dataGridView, UC, ML, MSelectGR);
                }
                else if (ind.Count == 2 && ind[0].Count == ind[1].Count)
                {
                    chart1.Series.Clear();
                    chart2.Series.Clear();
                    chart3.Series.Clear();
                    MaindatGridView.Rows.Clear();
                    UniformityDoubleMultidimensionalData UdMD = new UniformityDoubleMultidimensionalData(ML, ind);
                    WraitData.ReWraitData(MaindatGridView, UdMD.Estimation);
                    dataGridView.Rows.Clear();
                }

                else if (ind.Count > 2)
                {
                    chart1.Series.Clear();
                    chart2.Series.Clear();
                    chart3.Series.Clear();
                    MaindatGridView.Rows.Clear();
                    UniformityMultidimensionalData UMD = new UniformityMultidimensionalData(ML, ind);
                    WraitData.ReWraitData(MaindatGridView, UMD.Estimation);
                    dataGridView.Rows.Clear();
                }
            }
        }

        private void ЗберегтиДаніToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool Nezal = true;
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                return;
            double NezCount = ML[MSelectGR[0]].l.Count;
            for (int i = 0; i < MSelectGR.Count; i++)
                if (ML[MSelectGR[i]].l.Count != NezCount)
                    Nezal = false;
            SaveFileDialog openFileDialog1 = new SaveFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            if (Nezal && openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate);
                StreamWriter streamW = new StreamWriter(fs, Encoding.ASCII);
                try
                {
                    for (int i = 0; i < ML[MSelectGR[0]].unsortl.Length; i++)
                    {
                        for (int j = 0; j < MSelectGR.Count; j++)
                            streamW.Write(ML[MSelectGR[j]].unsortl[i] + "\t");
                        if (i < ML[MSelectGR[0]].unsortl.Length- 1)
                            streamW.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message);
                }
                finally
                {
                    streamW.Close();
                    fs.Close();
                }
            }

        }

        private void ВидалитиВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ML.Clear();
            undolist.Clear();
            WraitData.RefreshList(treeView1, ML);
        }

        private void ВидалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            for (int i = MSelectGR.Count - 1; i >= 0; i--)
            {
                ML.RemoveAt(MSelectGR[i]);
                undolist.RemoveAt(MSelectGR[i]);
            }
            if (MSelectGR.Count == 0 && treeView1.SelectedNode != null && treeView1.SelectedNode.Parent == null)
            {
                int TI = treeView1.SelectedNode.Index;
                ML.RemoveAt(TI);
                undolist.RemoveAt(TI);
            }
            WraitData.RefreshList(treeView1, ML);
        }
        #region DataChange

        private void CoefToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть коефіціент");
            double N = Convert.ToDouble(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                {
                    ML[SelectGR].l[i] = ML[SelectGR].l[i] + N;
                    ML[SelectGR].unsortl[i] = ML[SelectGR].unsortl[i] + N;
                }
            else
                foreach (int j in MSelectGR)
                {
                    for (int i = 0; i < ML[j].l.Count; i++)
                    {
                        ML[j].l[i] = ML[j].l[i] + N;
                        ML[j].unsortl[i] = ML[j].unsortl[i] + N;
                    }
                    ML[j].l.Sort();
                    ML[j].Refresh();
                }
            WraitData.RefreshList(treeView1, ML);
        }
        private void ExpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть l для X[new]=e^(-l*x[old])");
            double N = Convert.ToDouble(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[SelectGR].l[i] = Math.Round(Math.Pow(2.73, -ML[SelectGR].l[i] * N), 2);
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[j].l[i] = Math.Round(Math.Pow(2.73, -ML[j].l[i] * N), 2); ;
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void LogToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Enter log base:");
            double N = Convert.ToDouble(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[SelectGR].l[i] = Math.Round(Math.Log(ML[SelectGR].l[i] - ML[SelectGR].Min.Q + 1, N), 2);
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[j].l[i] = Math.Round(Math.Log(ML[j].l[i] - ML[j].Min.Q + 1, N), 2);
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void SubmittedToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Enter the degree to which the need to bring:");
            double N = Convert.ToDouble(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[SelectGR].l[i] = Math.Round(Math.Pow(ML[SelectGR].l[i], N), 2);
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                    ML[j].l[i] = Math.Round(Math.Pow(ML[j].l[i], N), 2);
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void SubmittedAPowerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть степінь a для X[new]=1/(X[old]^a):");
            double N = Convert.ToDouble(inputBox.getString());
            for (int i = 0; i < ML.Count; i++)
            {
                if (ML[SelectGR].l[i] == 0)
                {
                    MessageBox.Show("Error \nData=0");
                }
                ML[SelectGR].l[i] = Math.Round(1 / Math.Pow(ML[SelectGR].l[i], N), 2);
            }
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                {
                    if (ML[SelectGR].l[i] == 0)
                        MessageBox.Show("Error \nData=0");
                    ML[SelectGR].l[i] = Math.Round(1 / Math.Pow(ML[SelectGR].l[i], N), 2);
                }
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                {
                    if (ML[j].l[i] == 0)
                        MessageBox.Show("Error \nData=0");
                    ML[j].l[i] = Math.Round(1 / Math.Pow(ML[j].l[i], N), 2);
                }
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void ДіленняНаЧислоToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Enter the degree to which the need to bring:");
            double N = Convert.ToDouble(inputBox.getString());
            if (N == 0)
            {
                MessageBox.Show("Error \nN повинно не дорівнювати 0");
                return;
            }
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                {
                    ML[SelectGR].l[i] /= N;
                    ML[SelectGR].unsortl[i] /= N;
                }
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[j].l.Count; i++)
                {
                    ML[j].l[i] /= N;
                    ML[j].unsortl[i] /= N;
                }
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void StandartToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                for (int i = 0; i < ML[SelectGR].l.Count; i++)
                {
                    ML[SelectGR].l[i] = Math.Round((ML[SelectGR].l[i] - ML[SelectGR].Mx.Q) / ML[SelectGR].Gx.Q, 2);
                    ML[SelectGR].unsortl[i] = Math.Round((ML[SelectGR].unsortl[i] - ML[SelectGR].Mx.Q) / ML[SelectGR].Gx.Q, 2);
                }
            foreach (int j in MSelectGR)
            {
                for (int i = 0; i < ML[j].l.Count; i++)
                {
                    ML[j].l[i] = Math.Round((ML[j].l[i] - ML[j].Mx.Q) / ML[j].Gx.Q, 2);
                    ML[j].unsortl[i] = Math.Round((ML[j].unsortl[i] - ML[j].Mx.Q) / ML[j].Gx.Q, 2);
                }
                ML[j].l.Sort();
                ML[j].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        #endregion

        private void ПовернутисяДоПочатковихДанихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
            {
                ML[SelectGR].l.Clear();
                ML[SelectGR].unsortl = new double[undolist[SelectGR].Length];
                undolist[SelectGR].CopyTo(ML[SelectGR].unsortl, 0);
                for (int i = 0; i < undolist[SelectGR].Length; i++)
                {
                    ML[SelectGR].l.Add(undolist[SelectGR][i]);
                }
                ML[SelectGR].l.Sort();
                ML[SelectGR].Refresh();
            }
            foreach (int i in MSelectGR)
            {
                ML[i].l.Clear();
                ML[i].unsortl = new double[undolist[i].Length];
                undolist[i].CopyTo(ML[i].unsortl, 0);
                for (int j = 0; j < undolist[i].Length; j++)
                {
                    ML[i].l.Add(undolist[i][j]);
                }
                ML[i].l.Sort();
                ML[i].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        #region Change
        private void ЗмінитиКількістьКласівToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть кількість класів:");
            int N = Convert.ToInt32(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
            {
                ML[SelectGR].m.Q = N;
                ML[SelectGR].Refresh();
            }
            foreach (int i in MSelectGR)
            {
                ML[i].m.Q = N;
                ML[i].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        private void ЗмінитиАльфуToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть альфу:");
            double N = Convert.ToDouble(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
            {
                ML[SelectGR].alf.Q = N;
                ML[SelectGR].Refresh();
            }
            foreach (int i in MSelectGR)
            {
                ML[i].alf.Q = N;
                ML[i].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        #region ChangeType
        private void НормальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                ML[SelectGR].AvtoType = Distributions.Normal;
            foreach (int i in MSelectGR)
                ML[i].AvtoType = Distributions.Normal;
            WraitData.RefreshList(treeView1, ML);
        }
        private void ЕкспоненціальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                ML[SelectGR].AvtoType = Distributions.Exp;
            foreach (int i in MSelectGR)
                ML[i].AvtoType = Distributions.Exp;
            WraitData.RefreshList(treeView1, ML);
        }
        private void РівномірнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
                ML[SelectGR].AvtoType = Distributions.Line;
            foreach (int i in MSelectGR)
                ML[i].AvtoType = Distributions.Line;
            WraitData.RefreshList(treeView1, ML);
        }
        #endregion

        private void ЗмінитиГрупуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ML.Count == 0)
                return;
            AdditionalInformationForm inputBox = new AdditionalInformationForm("Введіть групу:");
            int N = Convert.ToInt32(inputBox.getString());
            MSelectGR = WraitData.RefreshChecktTreeView(treeView1);
            if (MSelectGR.Count == 0)
            {
                ML[SelectGR].Group = N;
                ML[SelectGR].Refresh();
            }
            foreach (int i in MSelectGR)
            {
                ML[i].Group = N;
                ML[i].Refresh();
            }
            WraitData.RefreshList(treeView1, ML);
        }
        #endregion

        #endregion

        #endregion

    }
}