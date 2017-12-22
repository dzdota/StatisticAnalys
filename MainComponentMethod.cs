using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testgistogr
{
    class MainComponentMethod
    {
        public TabPage DataTabPage;
        private DataGridView DataGridView;
        private PictureBox picture;
        public InitialAnalysMultidimensionalData IAM;
        public InitialAnalysMultidimensionalData IAMIndepended = null;

        private List<InitialStatisticalAnalys> ML = new List<InitialStatisticalAnalys>();
        private List<double[]> undolist = new List<double[]>();
        private TreeView treeView1;

        ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BottomValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReversTransformValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox MinimalDispers;

        double MinimalDispersValue;
        double[] Procent;

        public MainComponentMethod(InitialAnalysMultidimensionalData IAM,
            ref List<InitialStatisticalAnalys> ML, ref List<double[]> undolist, ref TreeView treeView1)
        {
            this.IAM = IAM;
            this.ML = ML;
            this.undolist = undolist;
            this.treeView1 = treeView1;


            DataTabPage = TapPageCreate();
            SetData();

            if (IAM.n == 2)
            {
                Correlation_RegressionAnalysis CRA = new Correlation_RegressionAnalysis(new List<InitialStatisticalAnalys>() { IAM.ISA[0], IAM.ISA[1] },
                    new List<int>() { 0, 1 }, RegresTypeName.LineRegresion);
                picture.Image = PaintData.Paintf(CRA, picture.Width, picture.Height);
            }

        }


        private TabPage TapPageCreate()
        {

            DataGridView = new DataGridView()
            {
                Location = new Point(0, 0),
                AllowUserToAddRows = false,
                RowHeadersWidth = 60,
                Width = 500,
            };
            if (IAM.n == 2)
            {
                DataGridView.Dock = DockStyle.Left;
            }
            else
                DataGridView.Dock = DockStyle.Fill;




            // 
            // BottomValueToolStripMenuItem
            // 
            this.BottomValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "BottomValueToolStripMenuItem",
                Size = new System.Drawing.Size(223, 22),
                Text = "Перейти до незалежних координат"
            };
            this.BottomValueToolStripMenuItem.Click += new System.EventHandler(this.BottomValueToolStripMenuItem_Click);

            this.MinimalDispers = new ToolStripTextBox()
            {

                Name = "MinimalDispers",
                Text = "0"
            };

            // 
            // ReversTransformValueToolStripMenuItem
            // 
            this.ReversTransformValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "ReversTransformValueToolStripMenuItem",
                Size = new System.Drawing.Size(223, 22),
                Text = "Зворотньє перетворення",
                Visible = false
            };
            this.ReversTransformValueToolStripMenuItem.Click += new System.EventHandler(this.ReversTransformValueToolStripMenuItem_Click);

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
                this.ReversTransformValueToolStripMenuItem
            });

            System.Windows.Forms.TabPage tabPagenew = new TabPage();
            tabPagenew.Controls.Add(DataGridView);
            tabPagenew.Location = new System.Drawing.Point(4, 22);
            tabPagenew.Name = "tabPage3";
            tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            tabPagenew.Size = new System.Drawing.Size(1005, 273);
            tabPagenew.TabIndex = 0;
            tabPagenew.Text = "Метод головних компонент";
            tabPagenew.UseVisualStyleBackColor = true;
            tabPagenew.ResumeLayout(false);


            if (IAM.n == 2)
            {
                picture = new PictureBox()
                {
                    Dock = DockStyle.Right,
                    Width = 513,
                };
                picture.ContextMenuStrip = ContextMenuStrip;
                tabPagenew.Controls.Add(picture);
            }

            return tabPagenew;
        }

        private void ReversTransformValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialAnalysMultidimensionalData IAMReverse = ReversTransform(IAMIndepended, IAM.eightvector);
            int MaxGroup = -1;
            for (int j = 0; j < ML.Count; j++)
                MaxGroup = Math.Max(MaxGroup, ML[j].Group);
            for (int i = 0; i < IAMReverse.n; i++)
            {
                IAMReverse.ISA[i].Group = MaxGroup + 1;
                ML.Add(IAMReverse.ISA[i]);
                undolist.Add(ML[ML.Count - 1].unsortl);
            }
            WraitData.RefreshList(treeView1, ML);
        }

        private void SetData()
        {
            Matrix.OwnVectors(IAM.DC, out  IAM.eightvalue, out IAM.eightvector);
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

            DataGridView.Rows.Add("Сума відсотоків");
            DataGridView.Rows.Add();
            {
                double SumIElem = 0;
                for (int c = 0; c < IAM.n; c++)
                {
                    SumIElem += IAM.eightvalue[c];
                    DataGridView[c, DataGridView.Rows.Count - 1].Value = Math.Round(SumIElem / IAM.eightvalue.Sum(), 5).ToString();
                }
            }
            /*
            }
            else
            {
                double corel = IAM.K[0, 1];

                double fi = Math.Atan(2 * corel * IAM.ISA[0].Gx.Q * IAM.ISA[1].Gx.Q /
                    (IAM.ISA[0].Dx.Q - IAM.ISA[1].Dx.Q)) / 2;
                double cos = Math.Cos(fi);
                double sin = Math.Sin(fi);
                DataGridView.Rows.Add("fi:", Math.Round(fi, 5));
                DataGridView.Rows.Add("Cos(fi):", Math.Round(cos, 5));
                DataGridView.Rows.Add("Sin(fi):", Math.Round(sin, 5));
            }*/
        }

        private void SetNextData()
        {
            DataGridView.Rows.Add("Який відсоток дисперсії k - ої вихідної ознаки xk пояснено першими w головними компонентами.");
            DataGridView.Rows.Add();
            for (int c = 0; c < Procent.Length; c++)
                DataGridView[c, DataGridView.Rows.Count - 1].Value = Math.Round(Procent[c], 4).ToString();
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
            /*
            List<PointF> eightvalues  = new List<PointF>();
            Array.Sort(eightvalues, eightvectors);*/

        }

        private void BottomValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { MinimalDispersValue = Convert.ToDouble(MinimalDispers.Text); }
            catch { MinimalDispersValue = 0; }

            if (IAM.n == 2)
            {
                double corel = IAM.K[0, 1];
                double fi = Math.Atan(2 * corel * IAM.ISA[0].Gx.Q * IAM.ISA[1].Gx.Q /
                    (Math.Pow(IAM.ISA[0].Gx.Q, 2) - Math.Pow(IAM.ISA[1].Gx.Q, 2))) / 2;
                picture.Image = PaintData.RotateImage(new Bitmap(picture.Image), fi);
            }
            double[,] Xallstandart = new double[IAM.Xall.GetLength(0), IAM.Xall.GetLength(1)];
            for (int i = 0; i < IAM.Xall.GetLength(0); i++)
                for (int j = 0; j < IAM.Xall.GetLength(1); j++)
                    Xallstandart[i, j] = (IAM.Xall[i, j] - IAM.Ex[j])/* / IAM.ISA[j].Gx.Q*/;
            double[,] X = Matrix.MultiplicMatrix(Xallstandart, IAM.eightvector);


            int MaxGroup = -1;
            for (int j = 0; j < ML.Count; j++)
                MaxGroup = Math.Max(MaxGroup, ML[j].Group);
            List<int> index = new List<int>();
            for (int i = 0; i < IAM.n; i++)
            {
                List<double> Data = new List<double>();
                for (int j = 0; j < IAM.N; j++)
                {
                    Data.Add(X[j, i]);
                }
                InitialStatisticalAnalys ISA = new InitialStatisticalAnalys(Data, MaxGroup + 1);
                if (ISA.Dx.Q >= MinimalDispersValue)
                {
                    ML.Add(ISA);
                    index.Add(ML.Count - 1);
                    undolist.Add(ML[ML.Count - 1].unsortl);
                }
            }
            WraitData.RefreshList(treeView1, ML);
            IAMIndepended = new InitialAnalysMultidimensionalData(ML, index, -1);
            ReversTransformValueToolStripMenuItem.Visible = true;

            Procent = new double[IAM.n];
            for (int c = 0; c < IAM.n; c++)
            {
                for (int r = 0; r < IAMIndepended.Xall.GetLength(1); r++)
                    Procent[c] += Math.Pow(IAM.eightvector[r, c], 2);
                Procent[c] *= 100;
            }

            DataGridView.Rows.Clear();
            SetData();
            SetNextData();

        }

        private InitialAnalysMultidimensionalData ReversTransform(InitialAnalysMultidimensionalData IAMIndepended, double[,] eightvector)
        {
            double[,] eightvectorClone = new double[IAMIndepended.Xall.GetLength(1), eightvector.GetLength(1)];
            for (int r = 0; r < eightvectorClone.GetLength(0); r++)
                for (int c = 0; c < eightvectorClone.GetLength(1); c++)
                    eightvectorClone[r, c] = eightvector[r, c];
            double[,] X = Matrix.MultiplicMatrix(IAMIndepended.Xall, Matrix.TranspMatrix(eightvectorClone));
            
            InitialAnalysMultidimensionalData result =  new InitialAnalysMultidimensionalData(X, -1);


            if (IAM.n == 2)
            {
                double corel = IAM.K[0, 1];
                double fi = Math.Atan(2 * corel * IAM.ISA[0].Gx.Q * IAM.ISA[1].Gx.Q /
                    (Math.Pow(IAM.ISA[0].Gx.Q, 2) - Math.Pow(IAM.ISA[1].Gx.Q, 2))) / 2;
                picture.Image = PaintData.RotateImage(new Bitmap(picture.Image), -fi);
            }

            return result;
        }
    }
}
