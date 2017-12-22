using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using Emgu.CV;

namespace testgistogr
{
    static class PaintData
    {

        static public void Repaint(InitialStatisticalAnalys gr, Chart chart1, Chart chart2)
        {

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///GISTOGAMA_PAINT
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///


            chart1.Titles[0].Text = "f";
            chart1.Titles[1].Text = "x";
            chart2.Titles[0].Text = "x";
            chart2.Titles[1].Text = "F";

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart1.Series.Add(GistogPaint(gr, Color.DarkBlue, "f"));/*
            chart1.Series["f"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series["f"]["PointWidth"] = "1";
            chart1.Series["f"].BorderWidth = 1;
            chart1.Series["f"].Color = Color.DarkBlue;
            chart1.Series["f"].BorderColor = Color.Black;*/
            chart1.Series.Add("01");
            chart1.Series["01"].IsVisibleInLegend = false;
            chart1.Series["01"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["01"].BorderWidth = 2;
            chart1.Series["01"].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.Minimum = chart2.ChartAreas[0].AxisX.Minimum = Math.Round(gr.Min.Q - 0.1 * (gr.Len.Q), 3);
            chart1.ChartAreas[0].AxisX.Maximum = chart2.ChartAreas[0].AxisX.Maximum = Math.Round(gr.Max.Q + 0.1 * (gr.Len.Q), 3);
            chart1.ChartAreas[0].AxisX.Interval = chart2.ChartAreas[0].AxisX.Interval = Math.Round(0.1 * (gr.Len.Q), 3);
            chart1.ChartAreas[0].AxisY.Interval = 0.05;
            chart2.ChartAreas[0].AxisY.Interval = 0.1;
            chart1.ChartAreas[0].AxisY.Minimum = chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Maximum = gr.f.Max() + 0.07;


            chart2.Series.Add("-1");
            chart2.Series["-1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["-1"].Color = Color.Green;
            chart2.Series["-1"].BorderWidth = 3;
            chart2.Series["-1"].IsVisibleInLegend = false;
            chart2.Series["-1"].Points.AddXY(gr.Min.Q - gr.Step.Q, 0);
            chart2.Series["-1"].Points.AddXY(gr.Min.Q, 0);
            chart2.Series["-1"].Points.AddXY(gr.Min.Q - gr.Step.Q * 0.1, 0 + 0.03);
            chart2.Series["-1"].Points.AddXY(gr.Min.Q, 0);
            chart2.Series["-1"].Points.AddXY(gr.Min.Q - gr.Step.Q * 0.1, 0 - 0.03);

            chart2.Series.Add("-2");
            chart2.Series["-2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["-2"].Color = Color.Green;
            chart2.Series["-2"].BorderWidth = 3;
            chart2.Series["-2"].IsVisibleInLegend = false;
            chart2.Series["-2"].Points.AddXY(gr.Max.Q, 1);
            chart2.Series["-2"].Points.AddXY(gr.Max.Q + gr.Step.Q, 1);
            chart2.Series["-2"].Points.AddXY(gr.Max.Q + gr.Step.Q * 0.9, 1 + 0.03);
            chart2.Series["-2"].Points.AddXY(gr.Max.Q + gr.Step.Q, 1);
            chart2.Series["-2"].Points.AddXY(gr.Max.Q + gr.Step.Q * 0.9, 1 - 0.03);

            chart2.Series.Add("Інтервал \nпередбачення");
            chart2.Series["Інтервал \nпередбачення"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Інтервал \nпередбачення"].BorderWidth = 2;
            chart2.Series["Інтервал \nпередбачення"]["PointWidth"] = "2";
            chart2.Series["Інтервал \nпередбачення"].Color = Color.Black;

            chart2.Series.Add("Інтервал передбачення2");
            chart2.Series["Інтервал передбачення2"].IsVisibleInLegend = false;
            chart2.Series["Інтервал передбачення2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Інтервал передбачення2"].BorderWidth = 2;
            chart2.Series["Інтервал передбачення2"]["PointWidth"] = "2";
            chart2.Series["Інтервал передбачення2"].Color = Color.Black;
            chart2.Series["Інтервал \nпередбачення"].Points.AddXY(gr.predictioninterval.QButton, 0);
            chart2.Series["Інтервал \nпередбачення"].Points.AddXY(gr.predictioninterval.QButton, 1);
            chart2.Series["Інтервал передбачення2"].Points.AddXY(gr.predictioninterval.QUpper, 0);
            chart2.Series["Інтервал передбачення2"].Points.AddXY(gr.predictioninterval.QUpper, 1);






            for (double i = gr.l[0], v = 0, F = 0; v < gr.Y2.Count; i += gr.Step.Q, v++, F++)
            {
                string str;
                if (F == 0)
                    str = "F";
                else
                    str = Convert.ToString(F);
                chart2.Series.Add(str);
                chart2.Series[str].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series[str].Color = Color.Green;
                chart2.Series[str].BorderWidth = 3;
                if (F != 0)
                    chart2.Series[str].IsVisibleInLegend = false;
                chart2.Series[str].Points.AddXY(gr.Min.Q + gr.Step.Q * v, gr.Y2[(int)v]);
                chart2.Series[str].Points.AddXY(gr.Min.Q + gr.Step.Q * (v + 1), gr.Y2[(int)v]);
                chart2.Series[str].Points.AddXY(gr.Min.Q + gr.Step.Q * (v + 0.9), gr.Y2[(int)v] + 0.03);
                chart2.Series[str].Points.AddXY(gr.Min.Q + gr.Step.Q * (v + 1), gr.Y2[(int)v]);
                chart2.Series[str].Points.AddXY(gr.Min.Q + gr.Step.Q * (v + 0.9), gr.Y2[(int)v] - 0.03);
                //chart1.Series["f"].Points.AddXY(Math.Round(gr.Min.Q + gr.Step.Q * v + gr.Step.Q / 2,3), gr.f[(int)v]);
            }
            string str2 = "F не розбита\n на класи";
            chart2.Series.Add(str2);
            chart2.Series[str2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart2.Series[str2].Color = Color.Red;
            chart2.Series[str2].BorderWidth = 3;
            for (int i = 0; i < gr.l.Count; i++)
            {
                chart2.Series[str2].Points.AddXY(Math.Round(gr.l[i], 3), Math.Round(gr.F[i], 3));
            }
        }
        static public Bitmap Paintf(Correlation_RegressionAnalysis CRA, int width, int height)
        {
            Bitmap mapf = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(mapf))
            {
                for (double j = 0; j < CRA.f.GetLength(0); j++)
                    for (double k = 0; k < CRA.f.GetLength(1); k++)
                    {
                        double z = CRA.f[(int)j, (int)k];
                        Rectangle rec = new Rectangle((int)(j * width / CRA.f.GetLength(0)),
                            height - (int)(k * height / CRA.f.GetLength(1)),
                            width / CRA.f.GetLength(0) + 2, height/ CRA.f.GetLength(1) + 2);
                        if (z > 0.07)
                            g.FillRectangle(new SolidBrush(Color.Black), rec);
                        else
                        {
                            ///g.FillRectangle(new SolidBrush(Color.Beige), ))
                            int r = 255 - (int)(255 * CRA.f[(int)j, (int)k] / 0.07);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(r, r, r)), rec);
                        }
                    }
            }
            return mapf;
        }
        static public void Repaint(InitialStatisticalAnalys gr, InitialStatisticalAnalys gr2, Correlation_RegressionAnalysis CRA, Chart chart1, Chart chart2)
        {
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///GISTOGAMA_PAINT
            ///++++++++++++++++++++++++++++++++++++++++++++++
            chart1.Series.Clear();
            chart2.Series.Clear();

            if (CRA.f == null)
                return;
            chart1.Series.Add(CorelPaint(gr.unsortl, gr2.unsortl, Color.DarkBlue, "f"));
            chart1.ChartAreas[0].AxisX.Minimum = gr.Min.Q - gr.Len.Q * 0.05;
            chart1.ChartAreas[0].AxisX.Maximum = gr.Max.Q + gr.Len.Q * 0.05;
            chart1.ChartAreas[0].AxisX.Interval = Math.Round(gr.Len.Q * 0.1, 3);
            chart1.ChartAreas[0].AxisY.Interval = Math.Round(gr2.Len.Q * 0.1, 3);
            chart1.ChartAreas[0].AxisY.Minimum = gr2.Min.Q - gr2.Len.Q * 0.05;
            chart1.ChartAreas[0].AxisY.Maximum = gr2.Max.Q + gr2.Len.Q * 0.05;
            chart2.ChartAreas[0].AxisX.Minimum = chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Maximum = CRA.f.GetLength(0);
            chart2.ChartAreas[0].AxisY.Maximum = CRA.f.GetLength(1);
            chart2.ChartAreas[0].AxisX.Interval = chart2.ChartAreas[0].AxisY.Interval = 100;

            chart1.Titles[0].Text = "y";
            chart1.Titles[1].Text = "x";
            chart2.Titles[0].Text = "x";
            chart2.Titles[1].Text = "y";

            chart2.ChartAreas[0].AxisY.CustomLabels.Clear();

            /*for (int j = 0; j < CRA.f.GetLength(0); j++)
            {
                string str2 = "";
                for (int k = 0; k < CRA.f.GetLength(1); k++)
                {
                    str2 = "f" + j.ToString() + "_" + k.ToString();
                    chart2.Series.Add(str2);
                    chart2.Series[str2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chart2.Series[str2].MarkerStyle = MarkerStyle.Square;
                    chart2.Series[str2].IsVisibleInLegend = false;
                    chart2.Series[str2].MarkerSize = (chart2.Size.Width - 20 + k * 10) / CRA.f.GetLength(0);
                    //chart2.Series[str2]["PixelPointWidth"] = "200";//PointWidth
                    double z = CRA.f[j, k];
                    if (z > 0.07)
                        chart2.Series[str2].Color = Color.Black;
                    else
                    {
                        int r = 255 - (int)(255 * CRA.f[j, k] / 0.07);
                        chart2.Series[str2].Color = Color.FromArgb(r, r, r);
                    }
                    //chart2.Series[str2].Color = Color.FromArgb(r, r, r);
                    //chart2.Series[str2]["StackedGroupName"] = "Group" + j.ToString();
                    chart2.Series[str2].Points.AddXY(j + 0.5,
                        k + 0.5);
                }
            }*/
            Image d = PaintData.Paintf(CRA, 1000, 1000);
            NamedImage ni = new NamedImage("backimage", d);
            chart2.Images.Add(ni);
            chart2.ChartAreas[0].BackImageAlignment = ChartImageAlignmentStyle.Center;
            chart2.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;
            chart2.ChartAreas[0].BackImage = "backimage";

            if (CRA.Doubl == true && CRA.Nezal == true)
            {
                chart1.Series.Add("Лін Рег" + ":" + CRA.RegresTypeVib);
                chart1.Series["Лін Рег" + ":" + CRA.RegresTypeVib].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Лін Рег" + ":" + CRA.RegresTypeVib].Color = Color.Yellow;
                chart1.Series["Лін Рег" + ":" + CRA.RegresTypeVib].BorderWidth = 3;
                if (CRA.RegresTypeVib == RegresTypeName.LineRegresion)
                {

                    //CRA.ABTeil
                    ///teilor
                    chart1.Series.Add("Лін Рег Тейл");
                    chart1.Series["Лін Рег Тейл"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series["Лін Рег Тейл"].Color = Color.Green;
                    chart1.Series["Лін Рег Тейл"].Points.AddXY(gr.Min.Q, CRA.ABTeil[0] + gr.Min.Q * CRA.ABTeil[1]);
                    chart1.Series["Лін Рег Тейл"].BorderWidth = 3;
                    chart1.Series["Лін Рег Тейл"].Points.AddXY(gr.Max.Q, CRA.ABTeil[0] + gr.Max.Q * CRA.ABTeil[1]);
                }

                chart1.Series.Add("Тол меж");
                chart1.Series["Тол меж"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Тол меж"].Color = Color.DarkRed;
                chart1.Series["Тол меж"].BorderWidth = 2;
                chart1.Series.Add("Тол меж2");
                chart1.Series["Тол меж2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Тол меж2"].Color = chart1.Series["Тол меж"].Color;
                chart1.Series["Тол меж2"].IsVisibleInLegend = false;
                chart1.Series["Тол меж2"].BorderWidth = 2;

                chart1.Series.Add("Дов інтр");
                chart1.Series["Дов інтр"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Дов інтр"].Color = Color.DarkMagenta;
                chart1.Series["Дов інтр"].BorderWidth = 2;
                chart1.Series.Add("Дов інтр2");
                chart1.Series["Дов інтр2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series["Дов інтр2"].Color = chart1.Series["Дов інтр"].Color;
                chart1.Series["Дов інтр2"].IsVisibleInLegend = false;
                chart1.Series["Дов інтр2"].BorderWidth = 2;
                if (CRA.RegresTypeVib != RegresTypeName.ParabRegresion)
                {
                    for (double x0 = gr.Min.Q; x0 <= gr.Max.Q; x0 += gr.Len.Q * 0.005)
                    {
                        double Sx0 = Math.Sqrt(CRA.Szal * (1 + 1.0 / gr.l.Count) + CRA.Q[1].QSigma * Math.Pow(x0 - gr.Mx.Q, 2));
                        if (CRA.RegresTypeVib == RegresTypeName.LineRegresion)
                        {
                            chart1.Series["Дов інтр"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) - CRA.T * Sx0);
                            chart1.Series["Дов інтр2"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) + CRA.T * Sx0);
                        }
                        chart1.Series["Тол меж"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) - CRA.T * CRA.Szal);
                        chart1.Series["Тол меж2"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) + CRA.T * CRA.Szal);
                    }
                    for (double x0 = gr.Min.Q; x0 <= gr.Max.Q; x0 += gr.Len.Q * 0.005)
                    {
                        chart1.Series["Лін Рег" + ":" + CRA.RegresTypeVib].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib));
                    }
                }
                else if (CRA.RegresTypeVib == RegresTypeName.ParabRegresion)
                {
                    double x2 = InitialStatisticalAnalys.StartMoment(gr.l, 2);
                    double x3 = InitialStatisticalAnalys.StartMoment(gr.l, 3);
                    double x4 = InitialStatisticalAnalys.StartMoment(gr.l, 4);
                    double Tt = Distributions.StudentQuantile(1 - gr.alf.Q / 2, gr.unsortl.Length - 3);
                    for (double x0 = gr.Min.Q; x0 <= gr.Max.Q; x0 += gr.Len.Q * 0.005)
                    {
                        double Sx0 = Math.Sqrt(Math.Pow(CRA.Szal2, 2) * (1 + 1.0 / gr.l.Count) +
                            Math.Pow(CRA.Q[4].QSigma * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q), 2) +
                            Math.Pow(CRA.Q[5].QSigma * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3), 2));

                        /*
                        chart1.Series["Дов інтр"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) - CRA.T * Sx0);
                        chart1.Series["Дов інтр2"].Points.AddXY(x0, RegresType.Model(x0, CRA.Q, CRA.RegresTypeVib) + CRA.T * Sx0);*/

                        chart1.Series["Дов інтр"].Points.AddXY(x0, CRA.Q[3].Q + CRA.Q[4].Q * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q)
                            + CRA.Q[5].Q * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3)
                            - Tt * Sx0);
                        chart1.Series["Дов інтр2"].Points.AddXY(x0, CRA.Q[3].Q + CRA.Q[4].Q * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q)
                            + CRA.Q[5].Q * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3)
                            + Tt * Sx0);


                        chart1.Series["Тол меж"].Points.AddXY(x0, CRA.Q[3].Q + CRA.Q[4].Q * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q)
                            + CRA.Q[5].Q * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3)
                            - Tt * CRA.Szal2);
                        chart1.Series["Тол меж2"].Points.AddXY(x0, CRA.Q[3].Q + CRA.Q[4].Q * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q)
                            + CRA.Q[5].Q * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3)
                            + Tt * CRA.Szal2);
                    }
                    for (double x0 = gr.Min.Q; x0 <= gr.Max.Q; x0 += gr.Len.Q * 0.005)
                    {
                        chart1.Series["Лін Рег" + ":" + CRA.RegresTypeVib].Points.AddXY(x0, CRA.Q[3].Q + CRA.Q[4].Q * Correlation_RegressionAnalysis.fi1F(x0, gr.Mx.Q)
                            + CRA.Q[5].Q * Correlation_RegressionAnalysis.fi2F(x0, gr.Dx.Q, gr.Mx.Q, x2, x3));
                    }
                }
            }


        }

        static public Series CorelPaint(double[] UnsortlX, double[] UnsortlY, Color col, string SerName)
        {
            Series SerCor = new Series(SerName)
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                BorderWidth = 1,
                Color = col,
                BorderColor = Color.Black,
                ["PointWidth"] = "1"
            };
            for (int i = 0; i < UnsortlX.Length && i < UnsortlY.Length; i++)
                SerCor.Points.AddXY(UnsortlX[i], UnsortlY[i]);
            return SerCor;
        }
        static public Series GistogPaint(InitialStatisticalAnalys ISA, Color col, string SerName)
        {
            Series SerCor = new Series(SerName)
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column,
                BorderWidth = 1,
                Color = col,
                BorderColor = Color.Black,
                ["PointWidth"] = "1"
            };
            for (double v = 0; v < ISA.Y2.Count; v++)
                SerCor.Points.AddXY(Math.Round(ISA.Min.Q + ISA.Step.Q * v + ISA.Step.Q / 2, 3), ISA.f[(int)v]);
            return SerCor;
        }

        static public Series LineRegresPaint(double minX, double maxX, double A, double B, Color colLine, string SerName)
        {
            Series SerCor = new Series(SerName)
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                ["PointWidth"] = "3",
                BorderWidth = 3,
                Color = colLine,
                BorderColor = Color.Black
            };
            SerCor.Points.AddXY(minX, A + B * minX);
            SerCor.Points.AddXY(maxX, A + B * maxX);
            return SerCor;
        }

        static public Bitmap RotateImage(Bitmap source, double AngleRotatete)
        {
            Bitmap rotatedImage = new Bitmap(source.Width, source.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(source.Width / 2, source.Height / 2);
                // Rotate
                g.RotateTransform((float)(AngleRotatete * 180 / Math.PI), System.Drawing.Drawing2D.MatrixOrder.Append);
                // Restore rotation point in the matrix
                g.TranslateTransform(-source.Width / 2, -source.Height / 2);
                // Draw the image on the bitmap
                g.DrawImage(source, new Point(0, 0));
            }

            return rotatedImage;/*
            Image<Emgu.CV.Structure.Rgb, byte> result = new Image<Emgu.CV.Structure.Rgb, byte>(source as Bitmap);
            return result.ToBitmap();
            return result.Rotate(AngleRotatete * 180 / Math.PI, new  PointF(source.Width / 2, source.Height / 2), Emgu.CV.CvEnum.Inter.Area,
                new Emgu.CV.Structure.Rgb(0, 255, 255),true).Bitmap;*/
        }

        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        static public Color HsvToRgb(double h, double S, double V)
        {
            int r, g, b;
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    // Red is the dominant color
                    case 0:
                        R = V; G = tv; B = pv;
                        break;
                    // Green is the dominant color
                    case 1:
                        R = qv; G = V; B = pv;
                        break;
                    case 2:
                        R = pv; G = V; B = tv;
                        break;
                    // Blue is the dominant color
                    case 3:
                        R = pv; G = qv; B = V;
                        break;
                    case 4:
                        R = tv; G = pv; B = V;
                        break;
                    // Red is the dominant color
                    case 5:
                        R = V; G = pv; B = qv;
                        break;
                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.
                    case 6:
                        R = V; G = tv; B = pv;
                        break;
                    case -1:
                        R = V; G = pv; B = qv;
                        break;
                    // The color is not defined, we should throw an error.
                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
