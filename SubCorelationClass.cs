using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testgistogr
{
    class SubCorelationClass
    {
        public TabPage CorelTabPage;
        public DataGridView CorelDataGridView;
        public InitialAnalysMultidimensionalData IAM;

        ContextMenuStrip CorelContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BottomValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UppperValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HToolStripMenuItem;

        public SubCorelationClass(InitialAnalysMultidimensionalData IAM)
        {
            this.IAM = IAM;
            CorelTabPage = TapPageCreate();
        }

        private TabPage TapPageCreate()
        {

            CorelDataGridView = new DataGridView();
            CorelDataGridView.Location = new Point(0, 0);
            CorelDataGridView.Dock = DockStyle.Fill;
            CorelDataGridView.AllowUserToAddRows = false;
            CorelDataGridView.ColumnHeaderMouseDoubleClick +=
                new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.CorelatioDataGridView_ColumnHeaderMouseDoubleClick);
            CorelDataGridView.TopLeftHeaderCell.Value
                = Math.Round(Distributions.StudentQuantile(1 - IAM.ISA[0].alf.Q / 2, IAM.N - 2),4).ToString();
            CorelDataGridView.RowHeadersWidth = 60;

            // 
            // BottomValueToolStripMenuItem
            // 
            this.BottomValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomValueToolStripMenuItem.Name = "BottomValueToolStripMenuItem";
            this.BottomValueToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.BottomValueToolStripMenuItem.Text = "Нижній довірчий інтервал";
            this.BottomValueToolStripMenuItem.Click += new System.EventHandler(this.BottomValueToolStripMenuItem_Click);
            // 
            // UppperValueToolStripMenuItem
            // 
            this.UppperValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UppperValueToolStripMenuItem.Name = "UppperValueToolStripMenuItem";
            this.UppperValueToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.UppperValueToolStripMenuItem.Text = "Верхній довірчий інтервал";
            this.UppperValueToolStripMenuItem.Click += new System.EventHandler(this.UppperValueToolStripMenuItem_Click);
            // 
            // ValueToolStripMenuItem
            // 
            this.ValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ValueToolStripMenuItem.Name = "ValueToolStripMenuItem";
            this.ValueToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.ValueToolStripMenuItem.Text = "Оцінка кореляції";
            this.ValueToolStripMenuItem.Click += new System.EventHandler(this.ValueToolStripMenuItem_Click);
            // 
            // TToolStripMenuItem
            // 
            this.TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TToolStripMenuItem.Name = "TToolStripMenuItem";
            this.TToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.TToolStripMenuItem.Text = "t ";
            this.TToolStripMenuItem.Click += new System.EventHandler(this.TToolStripMenuItem_Click);
            // 
            // HToolStripMenuItem
            // 
            this.HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HToolStripMenuItem.Name = "HToolStripMenuItem";
            this.HToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.HToolStripMenuItem.Text = "Значущість (= 0)";
            this.HToolStripMenuItem.Click += new System.EventHandler(this.HToolStripMenuItem_Click);
            // 
            // CorelContextMenuStrip
            // 
            CorelContextMenuStrip = new ContextMenuStrip();
            CorelDataGridView.ContextMenuStrip = CorelContextMenuStrip;
            this.CorelContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UppperValueToolStripMenuItem,
            this.ValueToolStripMenuItem,
            this.BottomValueToolStripMenuItem,
            this.TToolStripMenuItem,
            this.HToolStripMenuItem
            });
            this.CorelContextMenuStrip.Name = "contextMenuStrip1";
            this.CorelContextMenuStrip.Size = new System.Drawing.Size(224, 158);


            System.Windows.Forms.TabPage tabPagenew = new TabPage();
            tabPagenew.Controls.Add(CorelDataGridView);
            tabPagenew.Location = new System.Drawing.Point(4, 22);
            tabPagenew.Name = "tabPage3";
            tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            tabPagenew.Size = new System.Drawing.Size(1005, 273);
            tabPagenew.TabIndex = 0;
            tabPagenew.Text = "Кореляція";
            tabPagenew.UseVisualStyleBackColor = true;
            tabPagenew.ResumeLayout(false);
            return tabPagenew;
        }

        private void HToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bool[] KTurnOn = KTurnOnFound();
            int w = 0;
            foreach (bool b in KTurnOn)
                if (!b)
                    w++;
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, KTurnOn);
            double[,] T = TFound(SubK, KTurnOn);
            double T_alf_v = Distributions.StudentQuantile(1 - IAM.ISA[0].alf.Q / 2, IAM.N - w - 2);
            bool[,] H = new bool[T.GetLength(0), T.GetLength(1)];
            for (int i = 0; i < H.GetLength(0); i++)
                for (int j = 0; j < H.GetLength(1); j++)
                    if (Math.Abs(T[i, j]) <= T_alf_v)
                        H[i, j] = true;
            WraitH(CorelDataGridView, H, KTurnOn);
        }

        private void TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[] KTurnOn = KTurnOnFound();
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, KTurnOn);
            double[,] T = TFound(SubK, KTurnOn);
            WraitK(CorelDataGridView, T, KTurnOn);
        }

        private void ValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[] KTurnOn = KTurnOnFound();
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, KTurnOn);
            WraitK(CorelDataGridView, SubK, KTurnOn);
        }

        private void UppperValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[] KTurnOn = KTurnOnFound();
            int w = 0;
            foreach (bool b in KTurnOn)
                if (!b)
                    w++;
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, KTurnOn);

            double[,] V2 = new double[SubK.GetLength(0), SubK.GetLength(1)];
            double[,] BV = new double[SubK.GetLength(0), SubK.GetLength(1)];
            double U = Distributions.NormalQuantile(1 - IAM.ISA[0].alf.Q / 2);
            U /= (IAM.N - w - 3);
            for (int col = 0; col < SubK.GetLength(1); col++)
                for (int row = 0; row < SubK.GetLength(0); row++)
                {
                    if (KTurnOn[col] && KTurnOn[row])
                    {
                        V2[row, col] = Math.Log((1 + SubK[row, col]) / (1 - SubK[row, col])) / 2 + U;
                        double exp = Math.Exp(2 * V2[row, col]);
                        if (double.IsInfinity(exp))
                            BV[row, col] = 1;
                        else
                            BV[row, col] = (exp - 1.0) / (exp + 1.0);
                    }
                }
            WraitK(CorelDataGridView, BV, KTurnOn);

        }

        private void BottomValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[] KTurnOn = KTurnOnFound();
            int w = 0;
            foreach (bool b in KTurnOn)
                if (b)
                    w++;
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, KTurnOn);
            double[,] V1 = new double[SubK.GetLength(0), SubK.GetLength(1)];
            double[,] BV = new double[SubK.GetLength(0), SubK.GetLength(1)];
            double U = Distributions.NormalQuantile(1 - IAM.ISA[0].alf.Q / 2);
            U /= (IAM.N - w - 3);
            for (int col = 0; col < SubK.GetLength(1); col++)
                for (int row = 0; row < SubK.GetLength(0); row++)
                {
                    if (KTurnOn[col] && KTurnOn[row])
                    {
                        V1[row, col] = Math.Log((1 + SubK[row, col]) / (1 - SubK[row, col])) / 2 - U;
                        double exp = Math.Exp(2 * V1[row, col]);
                        if (double.IsInfinity(exp))
                            BV[row, col] = 1;
                        else
                            BV[row, col] = (exp - 1.0) / (exp + 1.0);
                    }
                }
            WraitK(CorelDataGridView, BV, KTurnOn);

        }

        private void CorelatioDataGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            bool[] b = new bool[CorelDataGridView.ColumnCount];
            int size = 0;
            for (int i = 0; i < CorelDataGridView.ColumnCount; i++)
            {
                if (CorelDataGridView[i, i].Style.BackColor != Color.Red)
                {
                    size++;
                    b[i] = true;
                }
            }
            if (CorelDataGridView[e.ColumnIndex, 0].Style.BackColor != Color.Red)
            {
                b[e.ColumnIndex] = false;
                size--;
                if (size < 2)
                    return;
                for (int i = 0; i < CorelDataGridView.ColumnCount; i++)
                {
                    CorelDataGridView[i, e.ColumnIndex].Style.BackColor = Color.Red;
                    CorelDataGridView[e.ColumnIndex, i].Style.BackColor = Color.Red;
                    CorelDataGridView[i, e.ColumnIndex].Value = "";
                    CorelDataGridView[e.ColumnIndex, i].Value = "";
                }
            }
            else
            {

                b[e.ColumnIndex] = true;
                size++;
                for (int i = 0; i < CorelDataGridView.ColumnCount; i++)
                {
                    if (b[i])
                    {
                        CorelDataGridView[i, e.ColumnIndex].Style.BackColor = Color.Green;
                        CorelDataGridView[e.ColumnIndex, i].Style.BackColor = Color.Green;
                    }
                }
            }
            CorelDataGridView.TopLeftHeaderCell.Value
                = Math.Round(Distributions.StudentQuantile(1 - IAM.ISA[0].alf.Q / 2, IAM.N - (b.Length - size) - 2), 4).ToString();
            double[,] SubK = InitialAnalysMultidimensionalData.SubK(IAM.K, b);
            WraitK(CorelDataGridView, SubK, b);
        }
        private bool[] KTurnOnFound()
        {
            bool[] KTurnOn = new bool[CorelDataGridView.ColumnCount];
            for (int i = 0; i < CorelDataGridView.ColumnCount; i++)
                if (CorelDataGridView[i, i].Style.BackColor != Color.Red)
                    KTurnOn[i] = true;
            return KTurnOn;
        }
        private void WraitK(DataGridView CorelDataGridView,double[,] K,bool[] KTurnOn)
        {

            for (int i = 0; i < IAM.K.GetLength(0); i++)
            {
                for (int j = 0; j < IAM.K.GetLength(1); j++)
                {
                    if (KTurnOn[i] && KTurnOn[j])
                        CorelDataGridView[j, i].Value = Convert.ToString(Math.Round(K[j, i], 4));
                    else
                        CorelDataGridView[j, i].Value = "";
                }
            }
        }
        private void WraitH(DataGridView CorelDataGridView, bool[,] H, bool[] KTurnOn)
        {

            for (int i = 0; i < IAM.K.GetLength(0); i++)
            {
                for (int j = 0; j < IAM.K.GetLength(1); j++)
                {
                    if (KTurnOn[i] && KTurnOn[j])
                        CorelDataGridView[j, i].Value = H[j, i];
                    else
                        CorelDataGridView[j, i].Value = "";
                }
            }
        }
        private double[,] TFound(double[,] K, bool[] KTurnOn)
        {
            double[,] T = new double[K.GetLength(0), K.GetLength(1)];
            int w = 0;
            foreach (bool b in KTurnOn)
                if (!b)
                    w++;

            for (int i = 0; i < K.GetLength(0); i++)
                for (int j = 0; j < K.GetLength(1); j++)
                    if (KTurnOn[i] && KTurnOn[j])
                        T[i, j] = K[j, i] * Math.Sqrt((IAM.N - w - 2) / (1 - Math.Pow(K[j, i], 2)));
            return T;
        }
    }
}
