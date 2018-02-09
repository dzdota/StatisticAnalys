using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace testgistogr
{
    partial class Classification
    {
        int n;


        public InitialAnalysMultidimensionalData IAM;
        public List<Classter> Classters = new List<Classter>();

        private List<double[]> Data = new List<double[]>();


        public Classification(InitialAnalysMultidimensionalData IAM, int index)
        {
            this.IAM = IAM;
            n = IAM.n - 1;
            InitializeComponent();
            InitializeData(index);
            NewDataDataGridView.ColumnCount = n;
            NewDataDataGridView.Rows.Add();
        }

        private void InitializeData(int index)
        {
            Classter.VFound(IAM);
            if (index < 0 || index >= IAM.ISA.Count)
                return;
            Classters.Clear();
            int parse;
            for (int i = 0; i < IAM.ISA[index].unsortl.Length; i++)
                if (int.TryParse(IAM.ISA[index].unsortl[i].ToString(), out parse) != true)
                    return;
            for (int i = 0; i <= IAM.ISA[index].Max.Q; i++)
                Classters.Add(new Classter(n));
            Data = new List<double[]>();
            for (int i = 0; i < IAM.N; i++)
            {
                double[] value = new double[n];
                for (int j = 0; j < n; j++)
                    value[j] = IAM.Xall[i, j >= index ? j + 1 : j];
                Data.Add(value);
                Classters[(int)IAM.Xall[i, index]].Add(value);
            }
            try
            {
                Classter.IAMFound(Classters.ToArray());
            }
            catch
            {
                Classters.Clear();
                return;
            }
            SetData();

            if (n == 2)
            {
                chart.Series.Clear();
                Classter.DrawClassters(chart, Classters.ToArray());
            }

            pDataGridView.ColumnCount = Classters.Count;
            pDataGridView.Rows.Add();
            for (int i = 0; i < Classters.Count; i++)
            {
                pDataGridView.Columns[i].Width = 45;
                pDataGridView[i, 0].Value = "1";
            }

            Classter.QFound(Classters.ToArray());
            ShowQ(Classter.Q);
        }


        private void ChangeIndex_Click(object sender, EventArgs e)
        {
            AdditionalInformationForm AIF = new AdditionalInformationForm("Введіть індекс");
            int index;
            try
            { index = Convert.ToInt32(AIF.getString()); }
            catch
            { return; }
            InitializeData(index);
        }

        private void SetData()
        {
            DataGridView.ColumnCount = n;
            DataGridView.Rows.Add(IAM.N);
            for (int c = 0; c < n; c++)
            {
                DataGridView.Columns[c].HeaderText = "X" + (c + 1).ToString();
                for (int r = 0; r < IAM.N; r++)
                    DataGridView[c, r].Value = Math.Round(Data[r][c], 5).ToString();
            }
        }

        private void AddNewData_Click(object sender, EventArgs e)
        {
            try
            {
                double[] X = new double[n];
                double[] p = new double[Classters.Count];
                for (int c = 0; c < n; c++)
                    X[c] = Convert.ToDouble(NewDataDataGridView[c, 0].Value);
                for (int c = 0; c < Classters.Count; c++)
                    p[c] = Convert.ToDouble(pDataGridView[c, 0].Value);

                double[] f = new double[Classters.Count];
                double[] pf = new double[Classters.Count];
                for (int i = 0; i < Classters.Count; i++)
                {
                    f[i] = Distributions.Normalf(Classters[i].IAM, X);
                    pf[i] = f[i] * p[i];
                }
                ShowP(pf, f);
            }
            catch { }
        }
        private void ShowP(double[] pf, double[] f)
        {
            int round = 12;
            ResultAddDataGridView.Rows.Clear();
            ResultAddDataGridView.ColumnCount = Classters.Count;
            ResultAddDataGridView.Rows.Add("Функції щільності розподілу ймовірностей");
            ResultAddDataGridView.Rows.Add();
            for (int i = 0; i < Classters.Count; i++)
                ResultAddDataGridView[i, ResultAddDataGridView.Rows.Count - 1].Value = Math.Round(f[i],round).ToString();
            ResultAddDataGridView.Rows.Add("P[j] * f[j]");
            ResultAddDataGridView.Rows.Add();
            for (int i = 0; i < Classters.Count; i++)
                ResultAddDataGridView[i, ResultAddDataGridView.Rows.Count - 1].Value = Math.Round(pf[i],round).ToString();
            ResultAddDataGridView.Rows.Add("Відповідь");
            ResultAddDataGridView.Rows.Add();
            ResultAddDataGridView[Array.IndexOf(pf, pf.Max()), ResultAddDataGridView.Rows.Count - 1].Value = "Віднесено";
            ResultAddDataGridView[Array.IndexOf(pf, pf.Max()), ResultAddDataGridView.Rows.Count - 1].Style.BackColor = Color.Green;
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
