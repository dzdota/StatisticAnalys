using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace testgistogr
{
    class Classterization
    {
        public TabPage ClassterizationTabPage;
        private TabControl DataTabControl;
        private TabPage DataTabPage;
        private TabPage ResultTabPage;
        private DataGridView ResultDataGrid;

        private DataGridView DataGridView;
        private Chart chart;
        private ComboBox TypeDComboBox;
        private ComboBox TypeDSSComboBox;
        private ComboBox MethodComboBox;
        private GroupBox groupbox;

        public InitialAnalysMultidimensionalData IAM;
        public List<Classter> Classters = new List<Classter>();
        public Series ClassterCenterSer = null;

        private List<InitialStatisticalAnalys> ML = null;
        private List<double[]> undolist = null;
        private TreeView treeView1 = null;

        private List<double[]> Data = new List<double[]>();

        ContextMenuStrip ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Classterizate;
        private System.Windows.Forms.ToolStripMenuItem RemoveClassters;

        public Classterization(InitialAnalysMultidimensionalData IAM,
            ref List<InitialStatisticalAnalys> ML, ref List<double[]> undolist, ref TreeView treeView1)
        {
            this.IAM = IAM;
            this.ML = ML;
            this.undolist = undolist;
            this.treeView1 = treeView1;

            Data = new List<double[]>();
            for (int i = 0; i < IAM.N; i++)
            {
                double[] value = new double[IAM.n];
                for (int j = 0; j < IAM.n; j++)
                    value[j] = IAM.Xall[i, j];
                Data.Add(value);
            }

            InitializeComponent();
            SetData();

            if (IAM.n == 2)
            {
                chart.Series.Add(PaintData.CorelPaint(IAM.ISA[0].unsortl, IAM.ISA[1].unsortl, Color.Blue, ""));
                chart.Series.Add(ClassterCenterSer);
            }

        }

        private void InitializeComponent()
        {

            DataGridView = new DataGridView()
            {
                Location = new Point(0, 0),
                AllowUserToAddRows = false,
                RowHeadersWidth = 60,
                Width = 500,
            };
            DataGridView.CellDoubleClick += DataGridView_CellDoubleClick;
            if (IAM.n == 2)
            {
                DataGridView.Dock = DockStyle.Left;
            }
            else
                DataGridView.Dock = DockStyle.Fill;

            this.Classterizate = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "Classterizate",
                Size = new System.Drawing.Size(223, 22),
                Text = "Кластерезувати"
            };
            this.Classterizate.Click += new System.EventHandler(this.Classterizate_Click);

            this.RemoveClassters = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "RemoveClassters",
                Size = new System.Drawing.Size(223, 22),
                Text = "Видалити всі кластери"
            };
            this.RemoveClassters.Click += new System.EventHandler(this.RemoveClassters_Click);


            // 
            // CorelContextMenuStrip
            // 
            ContextMenuStrip = new ContextMenuStrip();
            DataGridView.ContextMenuStrip = ContextMenuStrip;



            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                Classterizate,
                RemoveClassters,
            });

            groupbox = new GroupBox()
            {
                Dock = DockStyle.Right,
                Width = 100,
                Text = ""
            };

            MethodComboBox = new ComboBox()
            {
                Dock = DockStyle.Top,
                Size = new Size(100, 50),
            };
            MethodComboBox.SelectedIndexChanged += MethodComboBox_SelectedIndexChanged;
            TypeDSSComboBox = new ComboBox()
            {
                Dock = DockStyle.Top,
                Size = new Size(100, 50),
            };
            TypeDSSComboBox.Visible = true;
            TypeDComboBox = new ComboBox()
            {
                Dock = DockStyle.Top,
                Size = new Size(100, 50),
            };
            groupbox.Controls.Add(TypeDComboBox);
            groupbox.Controls.Add(TypeDSSComboBox);
            groupbox.Controls.Add(MethodComboBox);

            for (int i = 0; i < Classter.dstruct.Length; i++)
                TypeDComboBox.Items.Add(Classter.dstruct[i].Name);
            TypeDComboBox.SelectedIndex = 0;

            for (int i = 0; i < Classter.dSSstruct.Length; i++)
                TypeDSSComboBox.Items.Add(Classter.dSSstruct[i].Name);
            TypeDSSComboBox.SelectedIndex = 0;

            MethodComboBox.Items.Add("К-середніх");
            MethodComboBox.Items.Add("Агломеративний");
            MethodComboBox.SelectedIndex = 0;
            System.Windows.Forms.TabPage tabPagenew = new TabPage();


            if (IAM.n == 2)
            {
                chart = new Chart()
                {
                    Dock = DockStyle.Right,
                    Width = 513,
                    Location = new System.Drawing.Point(513, 0),
                    Name = "Correlationchart",
                    TabIndex = 0,
                    Text = "Correlationchart",
                };
                chart.ContextMenuStrip = ContextMenuStrip;
                tabPagenew.Controls.Add(chart);
                this.chart.ChartAreas.Add(new ChartArea() { });
                chart.MouseUp += Chart_MouseUp;

                ClassterCenterSer = new Series("Classter")
                {
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    BorderWidth = 1,
                    Color = Color.Red,
                    BorderColor = Color.Black,
                    ["PointWidth"] = "1",
                };
            }

            tabPagenew.Controls.Add(DataGridView);
            tabPagenew.Controls.Add(groupbox);
            tabPagenew.Location = new System.Drawing.Point(4, 22);
            tabPagenew.Name = "DataTabPage";
            tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            tabPagenew.Size = new System.Drawing.Size(1005, 273);
            tabPagenew.TabIndex = 0;
            tabPagenew.Text = "Дані";
            tabPagenew.UseVisualStyleBackColor = true;
            tabPagenew.ResumeLayout(false);


            ResultDataGrid = new DataGridView()
            {
                Dock = DockStyle.Fill,
            };
            ResultTabPage = new TabPage()
            {
                Text = "Результат"
            };
            ResultTabPage.Controls.Add(ResultDataGrid);

            DataTabPage = tabPagenew;
            DataTabControl = new TabControl() { Dock = DockStyle.Fill};
            DataTabControl.Controls.Add(DataTabPage);
            DataTabControl.Controls.Add(ResultTabPage);

            ClassterizationTabPage = new TabPage()
            {
                UseVisualStyleBackColor = true,
                TabIndex = 0,
                Text = "Кластеризація"
            };
            ClassterizationTabPage.Controls.Add(DataTabControl);
        }

        private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TypeDSSComboBox.Visible = !TypeDSSComboBox.Visible;
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor == Color.Green)
                return;
            for (int i = 0; i < DataGridView.ColumnCount; i++)
                DataGridView[i, e.RowIndex].Style.BackColor = Color.Green;
            Classters.Add(new Classter(Data[e.RowIndex]));
        }

        private void RemoveClassters_Click(object sender, EventArgs e)
        {
            Classters.Clear();
            if (IAM.n == 2)
            {
                ClassterCenterSer.Points.Clear();
                while (chart.Series.Count > 2)
                    chart.Series.RemoveAt(2);
                chart.Series[0].Enabled = chart.Series[1].Enabled = Classterizate.Visible = true;
            }
        }

        private void Chart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            try
            {
                var pos = e.Location;
                var results = chart.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
                double xVal = 0;
                double yVal = 0;
                xVal = results[0].ChartArea.AxisX.PixelPositionToValue(pos.X);
                yVal = results[0].ChartArea.AxisY.PixelPositionToValue(pos.Y);

                double min = double.MaxValue;
                int index = -1;
                for (int i = 0; i < IAM.N; i++)
                {
                    double val = Math.Sqrt(Math.Pow(IAM.ISA[0].unsortl[i] - xVal, 2) + Math.Pow(IAM.ISA[1].unsortl[i] - yVal, 2));
                    if (val < min)
                    {
                        min = val;
                        index = i;
                    }
                }
                Classters.Add(new Classter(new double[] { IAM.ISA[0].unsortl[index], IAM.ISA[1].unsortl[index] }));
                ClassterCenterSer.Points.AddXY(IAM.ISA[0].unsortl[index], IAM.ISA[1].unsortl[index]);
            }
            catch { }
        }

        private void SetData()
        {
            DataGridView.ColumnCount = IAM.n;
            DataGridView.Rows.Add(IAM.N);
            for (int c = 0; c < IAM.n; c++)
            {
                DataGridView.Columns[c].HeaderText = "X" + (c + 1).ToString();
                for (int r = 0; r < IAM.N; r++)
                    DataGridView[c, r].Value = Math.Round(Data[r][c], 5).ToString();
            }
        }

        private void Classterizate_Click(object sender, EventArgs e)
        {
            Classter.ChangeD(TypeDComboBox.Text);
            Classter.ChangeDSS(TypeDSSComboBox.Text);

            Classter.VFound(IAM);

            if (MethodComboBox.SelectedIndex == 0)
            {
                if (Classters.Count < 2)
                    return;
                KAverage();
            }
            else if (MethodComboBox.SelectedIndex == 1)
            {
                AdditionalInformationForm AIF = new AdditionalInformationForm("Введіть K");
                int index;
                try
                { index = Convert.ToInt32(AIF.getString()); }
                catch
                { return; }
                AgmomerativClassterizate(index > 1 ? index : 2);
            }

            if (IAM.n == 2)
                chart.Series[0].Enabled = chart.Series[1].Enabled = Classterizate.Visible = false;
            int MaxGroup = -1;
            for (int j = 0; j < ML.Count; j++)
                MaxGroup = Math.Max(MaxGroup, ML[j].Group);
            for (int i = 0; i < Classters.Count; i++)
            {
                InitialStatisticalAnalys[] ISA = Classters[i].ToISA(MaxGroup + i + 1);
                if (IAM.n == 2)
                {
                    chart.Series.Add(
                        PaintData.CorelPaint(ISA[0].unsortl, ISA[1].unsortl,GetColor(i), 
                            "Classter" + (i + 1).ToString()));
                    chart.Series[chart.Series.Count - 1].MarkerSize = 10;
                }
                for (int j = 0; j < ISA.Length;j++)
                {
                    ML.Add(ISA[j]);
                    undolist.Add(ML[ML.Count - 1].unsortl);
                }
            }
            WraitData.RefreshList(treeView1, ML);

            Classter.QFound(Classters.ToArray());
            ShowQ(Classter.Q);
        }


        private void AgmomerativClassterizate(int K)
        {
            List<Classter> classters = new List<Classter>();
            for (int i = 0; i < Data.Count; i++)
                classters.Add(new Classter(Data[i]));
            DataGridView datagrid = new DataGridView()
            { AllowUserToAddRows = false};
            datagrid.ColumnCount = classters.Count;
            datagrid.Rows.Add(classters.Count);
            for (int c = 0; c < classters.Count; c++)
                for (int r = c; r < classters.Count; r++)
                {
                    double len = Classter.d(classters[c].Center, classters[r].Center);
                    datagrid[c, r].Value = len == 0 ? double.MaxValue: len;
                    datagrid[r, c].Value = datagrid[c, r].Value;
                }
            while (datagrid.ColumnCount != K)
            {
                Point index = IndexMinValueDataGrid(datagrid);

                datagrid.Columns.Add("NewCol" + datagrid.ColumnCount.ToString(),"");
                datagrid.Rows.Add();
                classters.Add(Classter.Merge(classters[index.X], classters[index.Y]));

                for (int i = 0; i < classters.Count; i++)
                {
                    double len = Classter.DSSS(classters[index.X], classters[index.Y], classters[i]);
                    datagrid[datagrid.ColumnCount - 1, i].Value = len == 0 ? double.MaxValue : len;
                    datagrid[i, datagrid.Rows.Count - 1].Value = datagrid[datagrid.ColumnCount - 1, i].Value;
                }

                classters.RemoveAt(Math.Max(index.X, index.Y));
                datagrid.Columns.RemoveAt(Math.Max(index.X, index.Y));
                datagrid.Rows.RemoveAt(Math.Max(index.X, index.Y));

                classters.RemoveAt(Math.Min(index.X, index.Y));
                datagrid.Columns.RemoveAt(Math.Min(index.X, index.Y));
                datagrid.Rows.RemoveAt(Math.Min(index.X, index.Y));

            }
            Classters = classters;
        }

        private void KAverage()
        {
            List<double[]> DataClone = new List<double[]>(Data);
            while (DataClone.Count != 0)
            {
                double[][] Lens = new double[Classters.Count][];
                double[] Min = new double[Classters.Count];
                for (int i = 0; i < Classters.Count && DataClone.Count != 0; i++)
                {
                    Lens[i] = Classters[i].Len(DataClone);
                    Min[i] = Lens[i].Min();
                }
                int index1 = Array.IndexOf(Min, Min.Min());
                int index2 = Array.IndexOf(Lens[index1], Min[index1]);
                if (Lens[index1][index2] == 0)
                {
                    DataClone.RemoveAt(index2);
                    continue;
                }
                Classters[index1].Add(DataClone[index2]);
                DataClone.RemoveAt(index2);
            }
        }

        private Point IndexMinValueDataGrid(DataGridView datagrid)
        {
            Point index = new Point(0, 1);
            double min = (double)datagrid[index.X, index.Y].Value;
            for (int r = 0; r < datagrid.Rows.Count; r++)
                for (int c = 0; c < datagrid.ColumnCount; c++)
                    if ((double)datagrid[c, r].Value < min && c != r)
                    {
                        index = new Point(r, c);
                        min = (double)datagrid[c, r].Value;
                    }
            return index;
        }

        private void ShowQ(Data[] Q)
        {
            ResultDataGrid.Rows.Clear();
            ResultDataGrid.ColumnCount = 2;
            ResultDataGrid.Columns[0].HeaderText = "QName";
            ResultDataGrid.Columns[0].Width = 250;
            ResultDataGrid.Columns[1].HeaderText = "Q";
            for (int i = 0; i < Q.Length; i++)
                ResultDataGrid.Rows.Add(Q[i].Name, Math.Round(Q[i].Q, 4).ToString());
        }

        static private Color GetColor(int index)
        {
            Random rnd = new Random();
            if (index == 0)
                return Color.Green;
            else if (index == 1)
                return Color.DarkSalmon;
            else if (index == 2)
                return Color.Orange;
            else if (index == 3)
                return Color.Blue;
            else if (index == 4)
                return Color.Brown;
            else
                return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }
    }

}
