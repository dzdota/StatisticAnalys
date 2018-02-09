using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testgistogr
{
    class FactorAnalysisMethod
    {
        delegate double[] hFound(double[,] Alf, int w);

        public TabPage DataTabPage;
        private DataGridView DataGridView;
        public InitialAnalysMultidimensionalData IAM;
        public InitialAnalysMultidimensionalData IAMIndepended = null;

        private List<InitialStatisticalAnalys> ML = new List<InitialStatisticalAnalys>();
        private List<double[]> undolist = new List<double[]>();
        private TreeView treeView1;

        ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BottomValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox MinimalDispers;


        public FactorAnalysisMethod(InitialAnalysMultidimensionalData IAM,
            ref List<InitialStatisticalAnalys> ML, ref List<double[]> undolist, ref TreeView treeView1)
        {
            this.IAM = IAM;
            this.ML = ML;
            this.undolist = undolist;
            this.treeView1 = treeView1;


            DataTabPage = TapPageCreate();
            SetData();
        }


        private TabPage TapPageCreate()
        {

            DataGridView = new DataGridView()
            {
                Location = new Point(0, 0),
                AllowUserToAddRows = false,
                RowHeadersWidth = 60,
                Width = 500,
                Dock = DockStyle.Fill
            };



            // 
            // BottomValueToolStripMenuItem
            // 
            this.BottomValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "BottomValueToolStripMenuItem",
                Size = new System.Drawing.Size(223, 22),
                Text = "Провести факторный аналіз"
            };
            this.BottomValueToolStripMenuItem.Click += new System.EventHandler(this.BottomValueToolStripMenuItem_Click);

            this.MinimalDispers = new ToolStripTextBox()
            {

                Name = "DataVariable",
                Text = "0"
            };


            // 
            // CorelContextMenuStrip
            // 
            ContextMenuStrip = new ContextMenuStrip();
            DataGridView.ContextMenuStrip = ContextMenuStrip;



            this.BottomValueToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.MinimalDispers,
            });
            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.BottomValueToolStripMenuItem,
            });

            System.Windows.Forms.TabPage tabPagenew = new TabPage();
            tabPagenew.Controls.Add(DataGridView);
            tabPagenew.Location = new System.Drawing.Point(4, 22);
            tabPagenew.Name = "tabPage3";
            tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            tabPagenew.Size = new System.Drawing.Size(1005, 273);
            tabPagenew.TabIndex = 0;
            tabPagenew.Text = "Факторній аналіз";
            tabPagenew.UseVisualStyleBackColor = true;
            tabPagenew.ResumeLayout(false);

            return tabPagenew;
        }

        private void BottomValueToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AdditionalInformationForm AIF = new AdditionalInformationForm("Кількість факторів");
            int DataVariable;
            try
            {
                DataVariable = Convert.ToInt32(AIF.getString());
            }
            catch
            { return; }
            double ee;
            try { ee = Convert.ToDouble(MinimalDispers.Text); }
            catch { ee = 1; }

            int MinW = 0;
            foreach (double val in IAM.eightvalue)
                if (val > 1)
                    MinW++;

            double[,] Xallstandart = new double[IAM.Xall.GetLength(0), IAM.Xall.GetLength(1)];
            for (int i = 0; i < IAM.Xall.GetLength(0); i++)
                for (int j = 0; j < IAM.Xall.GetLength(1); j++)
                    Xallstandart[i, j] = (IAM.Xall[i, j] - IAM.Ex[j]) / IAM.ISA[j].Gx.Q;




            hFound hF = null;

            double[] h;

            double[] eightvalue;
            double[,] eightvector;

            Matrix.OwnVectors(IAM.K, out eightvalue, out eightvector);
            Matrix.SortEighten(ref eightvalue, ref eightvector);
            int w = MinW;

            if (indexMinfFound(eightvector, w) == 0)
                hF += HSquareFound;
            else
                hF += HSquareMaxFound;
            h = hF(eightvector, w);
            double[,] Rh = SetRh(IAM.K, h);
            double[,] Rzal = RzalFound(Rh, eightvector);
            double f = fFound(Rzal, w);

            double[,] eightvectorPrev = eightvector;
            double fPrev = f;

            int count = 0;
            for (; count == 0 ||
                AlfaCheck(eightvector, eightvectorPrev, ee)
                && f < fPrev && hCheck(h);
                count++)
            {
                fPrev = f;
                eightvectorPrev = eightvector;

                Matrix.OwnVectors(Rh, out eightvalue, out eightvector);
                Matrix.SortEighten(ref eightvalue, ref eightvector);
                w = Math.Max(WFound(eightvalue), MinW);
                h = hF(eightvector, w);
                Rh = SetRh(IAM.K, h);
                Rzal = RzalFound(Rh, eightvector);
                f = fFound(Rzal, w);
            }

            int MaxGroup = -1;
            for (int j = 0; j < ML.Count; j++)
                MaxGroup = Math.Max(MaxGroup, ML[j].Group);

            DataGridView.Rows.Clear();
            SetData();


            double[] Procent = new double[IAM.n];
            for (int c = 0; c < IAM.n; c++)
            {
                for (int r = 0; r < w; r++)
                    Procent[c] += Math.Pow(eightvector[c, r], 2);
                Procent[c] *= 100;
            }
            w = DataVariable;


            SetFactro(eightvector, w);
        }
        private int indexMinfFound(double[,] eightvector, int w)
        {

            double[] farray = new double[2];

            double[] h = HSquareFound(eightvector, w);
            double[,] Rh = SetRh(IAM.K, h);
            double[,] Rzal = RzalFound(Rh, eightvector);
            double f = fFound(Rzal, w);
            farray[0] = f;

            h = HSquareMaxFound(eightvector, w);
            Rh = SetRh(IAM.K, h);
            Rzal = RzalFound(Rh, eightvector);
            farray[1] = fFound(Rzal, w);
            return Array.IndexOf(farray, farray.Min());
        }

        private bool hCheck(double[] h)
        {
            foreach (double val in h)
                if (val > 1)
                    return false;
            return true;
        }

        private int WFound(double[] eightvalue)
        {
            double aver = eightvalue.Sum() / eightvalue.Length;
            int w = 0;
            for (int i = 0; i < eightvalue.Length; i++)
                if (eightvalue[i] >= aver)
                    w++;
            return w;
        }

        private double[,] SetRh(double[,] R, double[] h)
        {
            double[,] Rh = R.Clone() as double[,];
            for (int i = 0; i < R.GetLength(0); i++)
                Rh[i, i] = h[i];
            return Rh;
        }                

        private double[] HSquareMaxFound(double[,] Alf, int w)
        {
            double[] h = new double[Alf.GetLength(0)];
            for (int k = 0; k < Alf.GetLength(0); k++)
                for (int v = 0; v < w; v++)
                    h[k] += Math.Max(h[k],Math.Abs(Alf[v, k]));
            return h;
        }
        private double[] HSquareFound(double[,] Alf, int w)
        {
            double[] h = new double[Alf.GetLength(0)];
            for (int k = 0; k < Alf.GetLength(0); k++)
                for (int v = 0; v < w; v++)
                    h[k] += Math.Pow(Alf[v, k], 2);
            return h;
        }

        private double[,] RzalFound(double[,] R, double[,] A)
        {
            return Matrix.Abs(Matrix.Addition(R, Matrix.MultiplicNumber(Matrix.MultiplicMatrix(A, Matrix.TranspMatrix(A)), -1)));
        }

        private double fFound(double[,] Rzal, int w)
        {
            double result = 0;
            for (int r = 0; r < w; r++)
                for (int c = 0; c < w; c++)
                    if (r != c)
                        result += Math.Pow(Rzal[r, c], 2);
            return result;
        }

        private bool AlfaCheck(double[,] AlfNext, double[,] AlfPrev, double e)
        {
            double temp = 0;
            for (int k = 0; k < AlfNext.GetLength(0); k++)
                for (int g = 0; g < AlfNext.GetLength(1); g++)
                    temp += Math.Pow(AlfNext[k, g] - AlfPrev[k, g], 2);
            return temp > e;
        }

        private void SetData()
        {
            Matrix.OwnVectors(IAM.K, out IAM.eightvalue, out IAM.eightvector);
            SortEighten(ref IAM.eightvalue, ref IAM.eightvector);
            DataGridView.ColumnCount = IAM.n;/*
            if (IAM.n > 2)
            {*/
            DataGridView.Rows.Add("Власні вектори");
            DataGridView.Rows.Add(IAM.n);
            for (int c = 0; c < IAM.n; c++)
            {
                DataGridView.Columns[c].HeaderText = "e" + (c + 1).ToString();
                for (int r = 0; r < IAM.n; r++)
                    DataGridView[c, r + 1].Value = Math.Round(IAM.eightvector[r, c], 5).ToString();
            }
            DataGridView.Columns[0].Width = 130;
            DataGridView.Rows.Add("Власні значення");
            DataGridView.Rows.Add();
            for (int c = 0; c < IAM.n; c++)
                DataGridView[c, DataGridView.Rows.Count - 1].Value = Math.Round(IAM.eightvalue[c], 5).ToString();
            DataGridView.Rows.Add("Відсоток дисперсії");
            DataGridView.Rows.Add();
            for (int c = 0; c < IAM.n; c++)
                DataGridView[c, DataGridView.Rows.Count - 1].Value = Math.Round(IAM.eightvalue[c] / IAM.eightvalue.Sum(), 5).ToString();

        }

        private void SetFactro(double[,] eightvector, int w)
        {

            DataGridView.Rows.Clear();
            SetData();
            DataGridView.Rows.Add("Матриця факторного відображення");
            for (int r = 0; r < IAM.n; r++)
            {
                DataGridView.Rows.Add();
                for (int c = 0; c < w; c++)
                    DataGridView[c, DataGridView.Rows.Count - 1].Value = Math.Round(eightvector[r, c], 5).ToString();
            }
        }


        private void SortEighten(ref double[] eightvalues, ref double[,] eightvectors)
        {
            int n = eightvalues.GetLength(0);
            double[][] eightvectors1 = new double[n][];
            for (int i = 0; i < n; i++)
            {
                eightvectors1[i] = new double[n];
                for (int j = 0; j < n; j++)
                    eightvectors1[i][j] = eightvectors[j, i];
            }
            Array.Sort(eightvalues, eightvectors1);
            Array.Reverse(eightvalues);
            Array.Reverse(eightvectors1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    eightvectors[i, j] = eightvectors1[j][i];
            }

        }

    }
}
