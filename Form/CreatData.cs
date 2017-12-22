using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TestSimpleRNG;
using System.Data.SqlClient;

namespace testgistogr
{
    public partial class CreatData : Form
    {
        double[] FirstData;
        private List<double> RezDat = new List<double>();
        List<InitialStatisticalAnalys> ISA;
        int type = -1;
        public CreatData()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
            Q3textBox.Text = "1";
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == Distributions.Normal) 
            {
                Q1label.Visible = true;
                Q1textBox.Visible = true;
                Q2label.Visible = true;
                Q2textBox.Visible = true;
                Nlabel.Visible = true;
                NtextBox.Visible = true;

                Q1label.Text = "Середнье значення";
                Q1textBox.Text = "1";
                Q2label.Text = "Сігма";
                Q2textBox.Text = "1";
                Nlabel.Text= "Кількість елементів";
                NtextBox.Text= "";
            }
            else if (comboBox1.Text == Distributions.Exp)
            {
                Q1label.Visible = true;
                Q1textBox.Visible = true;
                Q2label.Visible = false;
                Q2textBox.Visible = false;
                Nlabel.Visible = true;
                NtextBox.Visible = true;

                Q1label.Text = "Lambda ";
                Q1textBox.Text = "1";
                Q2label.Text = "";
                Q2textBox.Text = "1";
                Nlabel.Text = "Кількість елементів";
                NtextBox.Text = "";
            }
            else if (comboBox1.Text == Distributions.Line)
            {

                Q1label.Visible = true;
                Q1textBox.Visible = true;
                Q2label.Visible = true;
                Q2textBox.Visible = true;
                Nlabel.Visible = true;
                NtextBox.Visible = true;

                Q1label.Text = "A";
                Q1textBox.Text = "0";
                Q2label.Text = "B";
                Q2textBox.Text = "1";
                Nlabel.Text = "Кількість елементів";
                NtextBox.Text = "";
            }
            else if (comboBox1.Text == RegresTypeName.ParabRegresion)
            {
                Q3label.Visible = true;
                Q3textBox.Visible = true;
            }
            else
            {
                Q3label.Visible = false;
                Q3textBox.Visible = false;
                Q3textBox.Text = "0";
            }
        }
        
        public object[] SaveData()
        {
            type = 0;
            comboBox1.Items.Clear();
            Namelabel.Text = "Створення одномірних даних";
            comboBox1.Items.Add(Distributions.Normal);
            comboBox1.Items.Add(Distributions.Exp);
            comboBox1.Items.Add(Distributions.Line);
            comboBox1.SelectedIndex = 0;

            Q1label.Visible = true;
            Q1textBox.Visible = true;
            Q2label.Visible = true;
            Q2textBox.Visible = true;
            Nlabel.Visible = true;
            NtextBox.Visible = true;

            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return new object[]{RezDat,comboBox2.SelectedIndex};
        }
        public object[] SaveData(double[] FirstData)
        {
            this.FirstData = FirstData;
            type = 1;
            comboBox1.Items.Clear();
            Namelabel.Text = "Створення двомірних даних";
            comboBox1.Items.AddRange(Regex.Split(RegresTypeName.TypeRegresion,"\n"));
            comboBox1.SelectedIndex = 0;


            Q1label.Visible = true;
            Q1textBox.Visible = true;
            Q2label.Visible = true;
            Q2textBox.Visible = true;
            Q3label.Visible = false;
            Q3textBox.Visible = false;
            Nlabel.Visible = true;
            NtextBox.Visible = true;

            Nlabel.Text = "sigma e";
            NtextBox.Text = "10";
            Q1label.Text = "a";
            Q1textBox.Text = "";
            Q2label.Text = "b";
            Q2textBox.Text = "";
            Q3label.Text = "c";
            Q3textBox.Text = "1";

            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return new object[] { RezDat, comboBox2.SelectedIndex };
        }
        internal object[] SaveData(InitialAnalysMultidimensionalData IAMD)
        {
            this.ISA = IAMD.ISA;
            type = 2;
            comboBox1.Items.Clear();
            Namelabel.Text = "Створення "+ IAMD.ISA.Count+"-мірних даних";/*
            comboBox1.Items.AddRange(Regex.Split(RegresTypeName.TypeRegresion, "\n"));
            comboBox1.SelectedIndex = 0;*/
            comboBox1.Visible = false;

            Q1label.Visible = true;
            Q1textBox.Visible = true;
            Q2label.Visible = true;
            Q2textBox.Visible = true;
            Q3label.Visible = false;
            Q3textBox.Visible = false;
            Nlabel.Visible = true;
            NtextBox.Visible = true;

            Nlabel.Text = "sigma e";
            NtextBox.Text = "10";
            Q1label.Text = "A";
            Q1textBox.Text = "";
            Q2label.Text = "A0";
            Q2textBox.Text = "";
            Q3label.Text = "c";
            Q3textBox.Text = "1";

            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return new object[] { RezDat, comboBox2.SelectedIndex };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            if (type < 2)
            {
                double Q1 = 0, Q2 = 0, Q3 = 0, N = 0;
                try
                {
                    Q1 = Convert.ToDouble(Q1textBox.Text);
                    Q2 = Convert.ToDouble(Q2textBox.Text);
                    Q3 = Convert.ToDouble(Q3textBox.Text);
                    N = Convert.ToDouble(NtextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невірні дані\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (type == 0)
                {
                    if (comboBox1.Text == Distributions.Normal)
                        for (int i = 0; i < N; i++)
                            RezDat.Add(SimpleRNG.GetNormal(Q1, Q2));
                    else if (comboBox1.Text == Distributions.Exp)
                        for (int i = 0; i < N; i++)
                            RezDat.Add(Math.Log(1.0 / (1 - rand.NextDouble())) / Q1);
                    else if (comboBox1.Text == Distributions.Line)
                        for (int i = 0; i < N; i++)
                            RezDat.Add((rand.NextDouble() * (Q2 - Q1) + Q1));
                }
                else if (type == 1)
                {
                    for (int i = 0; i < FirstData.Length; i++)
                    {
                        double dat = RegresType.Model(FirstData[i], new double[] { Q1, Q2, Q3 }, comboBox1.Text);
                        double eps = SimpleRNG.GetNormal(0, N);
                        if (double.IsInfinity(dat) || double.IsNaN(dat))
                        {
                            MessageBox.Show("Неможливо створити даний тип регресії\n", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            RezDat.Clear();
                            return;
                        }
                        dat = dat + eps;
                        RezDat.Add(dat);
                    }
                }
            }
            else if (type == 2)
            {
                double A0 = 0, N = 0;
                double[] A = new double[ISA.Count];
                try
                {
                    N = Convert.ToDouble(NtextBox.Text);
                    A0 = Convert.ToDouble(Q2textBox.Text);
                    string[] As = Regex.Split(Q1textBox.Text, ";");
                    if (As.Length != A.Length)
                        throw new System.ArgumentException("Невірний вектор A", "Помилка");
                    for (int i = 0;i<As.Length&&i<A.Length;i++)
                        A[i] = Convert.ToDouble(As[i]);
                }
                catch { goto error; }
                for (int i = 0; i < ISA[0].unsortl.Length; i++)
                {
                    double dat = 0;
                    for (int j = 0;j<A.Length;j++)
                    {
                        dat += A[j] * ISA[j].unsortl[i];
                    }
                    dat += A0;
                    double eps = SimpleRNG.GetNormal(0, N);
                    dat = dat + eps;
                    RezDat.Add(dat);
                }

            }
            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
            error:
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
