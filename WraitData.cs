using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace testgistogr
{
    static class WraitData
    {
        static int round = 4;

        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        static private void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM()
            {
                hItem = node.Handle,
                mask = TVIF_STATE,
                stateMask = TVIS_STATEIMAGEMASK,
                state = 0
            };
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }
        static public void RefreshList(TreeView treeView1,List<InitialStatisticalAnalys> ML)
        {
            string Gr = " Group";
            for (int i = 0; i < ML.Count;i++ )
            {
                if (i < treeView1.Nodes.Count )
                {
                    if (("Кількість = " + ML[i].l.Count.ToString() + " E(x) = " + ML[i].Mx.Q.ToString()
                    + " S = " + ML[i].Gx.Q.ToString() + Gr + "::" + ML[i].Group) != treeView1.Nodes[i].Text)
                    {
                        if (Regex.Split(treeView1.Nodes[i].Text, "::")[1] == ML[i].Group.ToString())
                            treeView1.Nodes[i].Checked = false;

                        treeView1.Nodes[i].Text = ("Кількість = " + ML[i].l.Count.ToString() + " E(x) = " + ML[i].Mx.Q.ToString()
                        + " S = " + ML[i].Gx.Q.ToString() + Gr + "::" + ML[i].Group);
                        treeView1.Nodes[i].BackColor = ColorTreeView(ML[i].Group);
                    }
                    treeView1.Nodes[i].Nodes[0].Text = "Min = " + ML[i].Min.Q.ToString();
                    treeView1.Nodes[i].Nodes[1].Text = "Max = " + ML[i].Max.Q.ToString();
                    treeView1.Nodes[i].Nodes[2].Text = "Тип = " + ML[i].AvtoType;
                    treeView1.Nodes[i].Nodes[3].Text = "К.класів = " + ML[i].m.Q.ToString();
                    treeView1.Nodes[i].Nodes[4].Text = "Альфа = " + ML[i].alf.Q.ToString();
                }
                else{
                    treeView1.Nodes.Add("Кількість = " + ML[i].l.Count + " E(x) = " + ML[i].Mx.Q + " S = " + ML[i].Gx.Q + Gr + "::" + ML[i].Group);
                    treeView1.Nodes[i].BackColor = ColorTreeView(ML[i].Group);
                    treeView1.Nodes[i].Nodes.Add("Min = " + ML[i].Min.Q);
                    treeView1.Nodes[i].Nodes.Add("Max = " + ML[i].Max.Q);
                    treeView1.Nodes[i].Nodes.Add("Тип = " + ML[i].AvtoType);
                    treeView1.Nodes[i].Nodes.Add("К.класів = " + ML[i].m.Q);
                    treeView1.Nodes[i].Nodes.Add("Альфа = " + ML[i].alf.Q);
                    HideCheckBox(treeView1, treeView1.Nodes[i].Nodes[0]);
                    HideCheckBox(treeView1, treeView1.Nodes[i].Nodes[1]);
                    HideCheckBox(treeView1, treeView1.Nodes[i].Nodes[2]);
                    HideCheckBox(treeView1, treeView1.Nodes[i].Nodes[3]);
                    HideCheckBox(treeView1, treeView1.Nodes[i].Nodes[4]);
                }
            }
            for (int i = treeView1.Nodes.Count - 1; i >= ML.Count; i--)
                treeView1.Nodes.RemoveAt(i);
            /*if (treeView1.Nodes.Count>0)
                treeView1.Nodes[0].BackColor = Color.Green;*/
        }
        static public List<int> RefreshChecktTreeView(TreeView treeView1)
        {
            List<int> rez = new List<int>();
            for (int i = 0; i < treeView1.Nodes.Count;i++ )
                if (treeView1.Nodes[i].Checked)
                    rez.Add(i);
            return rez;
        }
        static public List<List<int>> RefreshChecktTreeViewMnog(TreeView treeView1)
        {
            List<List<int>> rez = new List<List<int>>();
            List<int> index = new List<int>();

            for (int i = 0; i < treeView1.Nodes.Count; i++)
                if (treeView1.Nodes[i].Checked)
                {
                    int group = Convert.ToInt32(Regex.Split(treeView1.Nodes[i].Text,"::")[1]);
                    int indexE = index.IndexOf(group);
                    if (indexE == -1)
                    {
                        indexE = rez.Count;
                        index.Add(group);
                        rez.Add(new List<int>());
                    }
                    rez[indexE].Add(i);
                }
            return rez;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaindatGridView"></param>
        /// <param name="gr"></param>
        static public void ReWraitData(DataGridView MaindatGridView, DataGridView dataGridView, InitialStatisticalAnalys gr)
        {
            MaindatGridView.Rows.Clear();
            MaindatGridView.Rows.Add(31);
            MaindatGridView.ColumnCount = 7;
            MaindatGridView.Columns[0].Width = 250;
            int sz = 75;
            MaindatGridView.Columns[1].Width = sz;
            MaindatGridView.Columns[2].Width = sz;
            MaindatGridView.Columns[3].Width = sz;
            MaindatGridView.Columns[4].Width = sz;
            MaindatGridView.Columns[5].Width = sz;
            MaindatGridView.Columns[6].Width = sz;
            MaindatGridView.Columns[0].HeaderText = "Назва";
            MaindatGridView.Columns[1].HeaderText = "Qнижнье";
            MaindatGridView.Columns[2].HeaderText = "Q";
            MaindatGridView.Columns[3].HeaderText = "Qверхнье";
            MaindatGridView.Columns[4].HeaderText = "G(Q)";
            MaindatGridView.Columns[5].HeaderText = "Ttest(Нульове)";
            MaindatGridView.Columns[6].HeaderText = "Ttest(Значення)";
            MaindatGridView[0, 0].Value = "Кількість елементів";
            MaindatGridView[0, 1].Value = "Кількість класів";
            MaindatGridView[0, 2].Value = "Крок";
            MaindatGridView[0, 3].Value = "Мінімальне значення";
            MaindatGridView[0, 4].Value = "Максимальне значення";
            MaindatGridView[0, 5].Value = "Середнье арифметичне";
            MaindatGridView[0, 6].Value = "Середнье арифметичне ранжоване";
            MaindatGridView[0, 7].Value = "Дисперсія(S^2)";
            MaindatGridView[0, 8].Value = "Сігма(S)";
            MaindatGridView[0, 9].Value = "Вибіркова медіана(MEDIANA)";
            MaindatGridView[0, 10].Value = "Медіана середніх Уолша";
            MaindatGridView[0, 11].Value = "MAD";
            MaindatGridView[0, 12].Value = "Мода(MODA)";
            MaindatGridView[0, 13].Value = "Коефіціент асіметріі(А)";
            MaindatGridView[0, 14].Value = "Коефіціент ексцесу(E)";
            MaindatGridView[0, 15].Value = "Коуфіціент контрексцесу(1/(|E|)^0.5";
            MaindatGridView[0, 16].Value = "Коефіціент віріації Пірсона(W)";
            MaindatGridView[0, 17].Value = "Інтервал передбачення";
            MaindatGridView[0, 18].Value = "Тип росподілу\n(Avto)";
            MaindatGridView[0, 19].Value = "Квантиль(0,05)";
            MaindatGridView[0, 20].Value = "Квантиль(0,1)";
            MaindatGridView[0, 21].Value = "Квантиль(0,25)";
            MaindatGridView[0, 22].Value = "Квантиль(0,5)";
            MaindatGridView[0, 23].Value = "Квантиль(0,75)";
            MaindatGridView[0, 24].Value = "Квантиль(0,9)";
            MaindatGridView[0, 25].Value = "Квантиль(0,95)";
            MaindatGridView[0, 26].Value = "Хі квадрат";
            MaindatGridView[0, 27].Value = "Критерій Колмогорова";
            MaindatGridView[0, 30].Value = "Кр.Аббе";
            dataGridView.Rows.Clear();
            dataGridView.ColumnCount = 1;
            dataGridView.Columns[0].Name = "Данні";
            dataGridView.Rows.Add(gr.l.Count);
            for (int i = 0; i < gr.l.Count; i++)
                dataGridView[0, i].Value = Math.Round(gr.l[i],2);
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///DATA_WRITE
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///
            MaindatGridView[2, 0].Value = Convert.ToString(gr.l.Count);
            MaindatGridView[2, 1].Value = Convert.ToString(gr.m.Q);
            MaindatGridView[2, 2].Value = Convert.ToString(gr.Step.Q);
            MaindatGridView[2, 3].Value = Convert.ToString(gr.Min.Q);
            MaindatGridView[2, 4].Value = Convert.ToString(gr.Max.Q);
            MaindatGridView[2, 5].Value = Convert.ToString(gr.Mx.Q);
            MaindatGridView[2, 6].Value = Convert.ToString(gr.Mx_rang.Q);
            MaindatGridView[2, 7].Value = Convert.ToString(gr.Dx.Q);
            MaindatGridView[2, 8].Value = Convert.ToString(gr.Gx.Q);
            MaindatGridView[2, 9].Value = Convert.ToString(gr.Mediana.Q);
            MaindatGridView[2, 10].Value = Convert.ToString(gr.AverYolsha.Q);
            MaindatGridView[2, 11].Value = Convert.ToString(gr.MAD.Q);
            MaindatGridView[2, 12].Value = Convert.ToString(gr.Moda.Q);
            MaindatGridView[2, 13].Value = Convert.ToString(gr.CoefAsim.Q);
            MaindatGridView[2, 14].Value = Convert.ToString(gr.CoefEcscec.Q);
            MaindatGridView[2, 15].Value = Convert.ToString(Math.Round(1 / Math.Sqrt(Math.Abs(gr.CoefEcscec.Q)), 4));
            MaindatGridView[2, 16].Value = Convert.ToString(gr.CoefVarPirson.Q);

            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///Interval
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///
            MaindatGridView[1, 5].Value = Convert.ToString(gr.Mx.QButton);
            MaindatGridView[1, 7].Value = Convert.ToString(gr.Dx.QButton);
            MaindatGridView[1, 8].Value = Convert.ToString(gr.Gx.QButton);
            MaindatGridView[1, 13].Value = Convert.ToString(gr.CoefAsim.QButton);
            MaindatGridView[1, 14].Value = Convert.ToString(gr.CoefEcscec.QButton);
            MaindatGridView[1, 17].Value = Convert.ToString(gr.predictioninterval.QButton);

            MaindatGridView[3, 5].Value = Convert.ToString(gr.Mx.QUpper);
            MaindatGridView[3, 7].Value = Convert.ToString(gr.Dx.QUpper);
            MaindatGridView[3, 8].Value = Convert.ToString(gr.Gx.QUpper);
            MaindatGridView[3, 13].Value = Convert.ToString(gr.CoefAsim.QUpper);
            MaindatGridView[3, 14].Value = Convert.ToString(gr.CoefEcscec.QUpper);
            MaindatGridView[3, 17].Value = Convert.ToString(gr.predictioninterval.QUpper);

            MaindatGridView[4, 5].Value = Convert.ToString(Math.Round(gr.Mx.QSigma, 4));
            MaindatGridView[4, 7].Value = Convert.ToString(Math.Round(gr.Dx.QSigma, 4));
            MaindatGridView[4, 8].Value = Convert.ToString(Math.Round(gr.Gx.QSigma, 4));
            MaindatGridView[4, 13].Value =Convert.ToString(Math.Round(gr.CoefAsim.QSigma, 4));
            MaindatGridView[4, 14].Value =Convert.ToString(Math.Round(gr.CoefEcscec.QSigma, 4));
            MaindatGridView[4, 17].Value =Convert.ToString(Math.Round(gr.predictioninterval.QSigma, 4));
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///Quantile
            ///++++++++++++++++++++++++++++++++++++++++++++++

            MaindatGridView[2, 19].Value = Convert.ToString(gr.Quantile[0]);
            MaindatGridView[2, 20].Value = Convert.ToString(gr.Quantile[1]);
            MaindatGridView[2, 21].Value = Convert.ToString(gr.Quantile[2]);
            MaindatGridView[2, 22].Value = Convert.ToString(gr.Quantile[3]);
            MaindatGridView[2, 23].Value = Convert.ToString(gr.Quantile[4]);
            MaindatGridView[2, 24].Value = Convert.ToString(gr.Quantile[5]);
            MaindatGridView[2, 25].Value = Convert.ToString(gr.Quantile[6]);



            MaindatGridView[2, 30].Value = Convert.ToString(gr.KrAbbe.Q);
            MaindatGridView[5, 30].Value = Convert.ToString(gr.KrAbbe.QKvant);
            MaindatGridView[6, 30].Value = Convert.ToString(Math.Abs(gr.KrAbbe.Q) <= gr.KrAbbe.QKvant);
        }

        static public void ReWraitData(DataGridView MaindatGridView,List<Data> dat)
        {
            int NumRows = MaindatGridView.Rows.Count-1;
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///DATA_WRITE
            ///++++++++++++++++++++++++++++++++++++++++++++++
            //MaindatGridView.Rows.Clear();
            MaindatGridView.ColumnCount = 7;
            MaindatGridView.Columns[0].Width = 250;
            int sz = 70;
            MaindatGridView.Columns[0].HeaderText = "Назва";
            MaindatGridView.Columns[1].Width = sz;
            MaindatGridView.Columns[1].HeaderText = "Qнижнье";
            MaindatGridView.Columns[2].Width = sz;
            MaindatGridView.Columns[2].HeaderText = "Q";
            MaindatGridView.Columns[3].Width = sz;
            MaindatGridView.Columns[3].HeaderText = "Qверхнье";
            MaindatGridView.Columns[4].Width = sz;
            MaindatGridView.Columns[4].HeaderText = "G(Q)";
            MaindatGridView.Columns[5].Width = sz;
            MaindatGridView.Columns[5].HeaderText = "Qквантиль";
            MaindatGridView.Columns[6].Width = sz;
            MaindatGridView.Columns[6].HeaderText = "H0";
            for (int i = 0; i < dat.Count; i++)
            {
                MaindatGridView.Rows.Add(); NumRows++;
                if (dat[i].Name!=null)
                    MaindatGridView[0, i].Value = dat[i].Name;
                if (dat[i].QButton != 0)
                    MaindatGridView[1, i].Value = Math.Round(dat[i].QButton,round);
                //if (dat[i].Q != 0)
                    MaindatGridView[2, i].Value = Math.Round(dat[i].Q, round);
                if (dat[i].QUpper != 0)
                    MaindatGridView[3, i].Value = Math.Round(dat[i].QUpper, round);
                if (dat[i].QSigma != 0)
                    MaindatGridView[4, i].Value = Math.Round(dat[i].QSigma, round);
                if (dat[i].QKvant != 0)
                    MaindatGridView[5, i].Value = Math.Round(dat[i].QKvant, round);
                //if (dat[i].H != null)
                    MaindatGridView[6, i].Value = dat[i].H;
                //MaindatGridView[4, i].Value = dat[i].QKvant;
            }
        }
        static public void ReWraitData(DataGridView MaindatGridView, DataGridView dataGridView,UniformityCriteria UC, List<InitialStatisticalAnalys> ML, List<int> MSelectGR)
        {
            int NumRows = -1;
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///DATA_WRITE
            ///++++++++++++++++++++++++++++++++++++++++++++++
            MaindatGridView.Rows.Clear();
            MaindatGridView.ColumnCount = 7;
            MaindatGridView.Columns[0].Width = 250;
            int sz = 70;
            MaindatGridView.Columns[0].HeaderText = "Назва";
            MaindatGridView.Columns[1].Width = sz;
            MaindatGridView.Columns[1].HeaderText = "Qнижнье";
            MaindatGridView.Columns[2].Width = sz;
            MaindatGridView.Columns[2].HeaderText = "Q";
            MaindatGridView.Columns[3].Width = sz;
            MaindatGridView.Columns[3].HeaderText = "Qверхнье";
            MaindatGridView.Columns[4].Width = sz;
            MaindatGridView.Columns[4].HeaderText = "G(Q)";
            MaindatGridView.Columns[5].Width = sz;
            MaindatGridView.Columns[5].HeaderText = "Qквантиль";
            MaindatGridView.Columns[6].Width = sz;
            MaindatGridView.Columns[6].HeaderText = "H0";

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, 0].Value = "Збіг середніх";
            double T = Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, (int)ML[MSelectGR[0]].m.Q - 1);
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.SravnSred[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(UC.SravnSred[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(UC.SravnSred[0]) <= UC.SravnSred[1]);

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, NumRows].Value = "Збіг дисперсій";
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.SravnDisper[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(UC.SravnDisper[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(UC.SravnDisper[0] <= UC.SravnDisper[1]);

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, NumRows].Value = "Міжгрупова варіація(Sm2)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.Sm2);

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, NumRows].Value = "Варіація всередині кожної вибірки(Sv2)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.Sv2);

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, NumRows].Value = "H0: середні знач. вибірок рівні між собою";
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.VarSv2Sm2[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(UC.VarSv2Sm2[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(UC.VarSv2Sm2[0] <= UC.VarSv2Sm2[1]);

            MaindatGridView.Rows.Add(); NumRows++;
            MaindatGridView[0, NumRows].Value = "Критерій Крускала-Уоліса(H)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrKruskalaUolisa[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrKruskalaUolisa[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrKruskalaUolisa[0] <= UC.KrKruskalaUolisa[1]);

            if (UC.Doubl)
            {
                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Знаків";
                MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrZnakiv[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrZnakiv[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrZnakiv[0] < UC.KrZnakiv[1]);

                if (UC.KrKohrena[0] != -1)
                {
                    MaindatGridView.Rows.Add(); NumRows++;
                    MaindatGridView[0, NumRows].Value = "Кр. Кохрена";
                    MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrKohrena[0]);
                    MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrKohrena[1]);
                    MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrKohrena[0] <= UC.KrKohrena[1]);
                }

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. узгодженості Смирного-Колмагорова";
                MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrSmirnKolmag[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrSmirnKolmag[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrSmirnKolmag[0] > UC.KrSmirnKolmag[1]);


                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Суми рангів Вілкоксона(W)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrsumRangVils[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrsumRangVils[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrsumRangVils[0] < UC.KrsumRangVils[1]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Манна-Уітні(U)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(UC.KrUMannaUit[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(UC.KrUMannaUit[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(UC.KrUMannaUit[0] <= UC.KrUMannaUit[1]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. різниці середніх рангів вибірок";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(UC.RizSerRangVib[0] - T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(UC.RizSerRangVib[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(UC.RizSerRangVib[0] + T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(1);
                MaindatGridView[5, NumRows].Value = Convert.ToString(UC.RizSerRangVib[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(UC.RizSerRangVib[0]) <= UC.RizSerRangVib[1]);



            }
            dataGridView.Rows.Clear();
            dataGridView.ColumnCount = MSelectGR.Count;
            dataGridView.Rows.Add((int)(UC.Nmax + 1));
            for (int i = 0; i < MSelectGR.Count; i++)
            {
                dataGridView.Columns[i].Width = 55;
                dataGridView.Columns[i].Name = "X" + (i + 1).ToString();
                for (int j = 0; j < ML[MSelectGR[i]].l.Count; j++)
                    dataGridView[i, j].Value = Math.Round(ML[MSelectGR[i]].unsortl[j],2);
            }
        }
        static public void ReWraitData(DataGridView MaindatGridView, DataGridView dataGridView, SimpleClass SC, List<InitialStatisticalAnalys> ML,List<int> MSelectGR)
        {
            int NumRows = -1;
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///DATA_WRITE
            ///++++++++++++++++++++++++++++++++++++++++++++++
            MaindatGridView.Rows.Clear();
            MaindatGridView.ColumnCount = 7;
            MaindatGridView.Columns[0].Width = 250;
            int sz = 70;
            MaindatGridView.Columns[0].HeaderText = "Назва";
            MaindatGridView.Columns[1].Width = sz;
            MaindatGridView.Columns[1].HeaderText = "Qнижнье";
            MaindatGridView.Columns[2].Width = sz;
            MaindatGridView.Columns[2].HeaderText = "Q";
            MaindatGridView.Columns[3].Width = sz;
            MaindatGridView.Columns[3].HeaderText = "Qверхнье";
            MaindatGridView.Columns[4].Width = sz;
            MaindatGridView.Columns[4].HeaderText = "G(Q)";
            MaindatGridView.Columns[5].Width = sz;
            MaindatGridView.Columns[5].HeaderText = "Qквантиль";
            MaindatGridView.Columns[6].Width = sz;
            MaindatGridView.Columns[6].HeaderText = "H0";

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, 0].Value = "Збіг середніх";
            double T = Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, (int)ML[MSelectGR[0]].m.Q - 1);
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.SravnSred[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(SC.SravnSred[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.SravnSred[0]) <= SC.SravnSred[1]);

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, NumRows].Value = "Збіг дисперсій";
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.SravnDisper[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(SC.SravnDisper[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(SC.SravnDisper[0] <= SC.SravnDisper[1]);

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, NumRows].Value = "Міжгрупова варіація(Sm2)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.Sm2);

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, NumRows].Value = "Варіація всередині кожної вибірки(Sv2)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.Sv2);

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, NumRows].Value = "H0: середні знач. вибірок рівні між собою";
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.VarSv2Sm2[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(SC.VarSv2Sm2[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(SC.VarSv2Sm2[0] <= SC.VarSv2Sm2[1]);

            MaindatGridView.Rows.Add();NumRows++;
            MaindatGridView[0, NumRows].Value = "Критерій Крускала-Уоліса(H)";
            MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrKruskalaUolisa[0]);
            MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrKruskalaUolisa[1]);
            MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrKruskalaUolisa[0] <= SC.KrKruskalaUolisa[1]);

            if (SC.Doubl)
            {
                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Знаків";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrZnakiv[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrZnakiv[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrZnakiv[0] < SC.KrZnakiv[1]);   

                if (SC.KrKohrena[0] != -1)
                {
                    MaindatGridView.Rows.Add();NumRows++;
                    MaindatGridView[0, NumRows].Value = "Кр. Кохрена";
                    MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrKohrena[0]);
                    MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrKohrena[1]);
                    MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrKohrena[0] <= SC.KrKohrena[1]);
                }

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. узгодженості Смирного-Колмагорова";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrSmirnKolmag[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrSmirnKolmag[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrSmirnKolmag[0] > SC.KrSmirnKolmag[1]);


                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Суми рангів Вілкоксона(W)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrsumRangVils[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrsumRangVils[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrsumRangVils[0] < SC.KrsumRangVils[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. Манна-Уітні(U)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KrUMannaUit[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KrUMannaUit[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KrUMannaUit[0] <= SC.KrUMannaUit[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кр. різниці середніх рангів вибірок";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(SC.RizSerRangVib[0] - T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.RizSerRangVib[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(SC.RizSerRangVib[0] + T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(1);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.RizSerRangVib[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.RizSerRangVib[0]) <= SC.RizSerRangVib[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кф. Кореляції(r)";
                MaindatGridView[1, NumRows].Value = Convert.ToString(SC.Korelation[1]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(SC.Korelation[2]);
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.Korelation[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0:Кф. Кореляції == 0";
                double t = SC.Korelation[0] * Math.Sqrt(ML[MSelectGR[0]].l.Count - 2) / Math.Sqrt(1 - Math.Pow(SC.Korelation[0], 2));
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(t, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(
                    Distributions.StudentQuantile(1 - ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(t) <=
                    Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count), 4));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Кореляційне відношення(p)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KorelationVidnoh[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: p == 0";
                t = SC.KorelationVidnoh[0] * Math.Sqrt(ML[MSelectGR[0]].l.Count - 2) / Math.Sqrt(1 - Math.Pow(SC.KorelationVidnoh[0], 2));
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(t, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(
                    Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(t) <=
                    Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Ранговий коефіціент кореляції Спірмена";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.RangKorelation[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Р. коеф. кор. Спірмена == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.RangKorelation[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));            
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.RangKorelation[1]) <= Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Ранговий коефіціент Кендала";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.RangKoefKend[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Р. коеф. Кендала == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.RangKoefKend[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.RangKoefKend[2]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.RangKoefKend[1]) <= SC.RangKoefKend[2]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Індекс Фехнера";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.IndexFehnera);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Коефіціент сполучень Ф(`Фі`)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefSpolF[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Ф == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefSpolF[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KoefSpolF[2]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KoefSpolF[1] >= SC.KoefSpolF[2]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Коефіціенти зв'язку Юла";
                MaindatGridView[1, NumRows].Value = "Q = " + Convert.ToString(SC.KoefSvazYoola[0]);
                MaindatGridView[3, NumRows].Value = "Y = " + Convert.ToString(SC.KoefSvazYoola[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Q == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefSvazYoola[2]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.KoefSvazYoola[2]) <=
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Y == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefSvazYoola[3]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(SC.KoefSvazYoola[3]) <= Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = Convert.ToString(SC.MeraZvazKendallaStatStyardName);
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(SC.MeraZvazKendallaStatStyard[0] -
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * SC.MeraZvazKendallaStatStyard[1], 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.MeraZvazKendallaStatStyard[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(SC.MeraZvazKendallaStatStyard[0] +
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * SC.MeraZvazKendallaStatStyard[1], 4));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Коеф. ранг. кор. Спірмена для табл. сполучень";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(SC.KoefRKorSpir[0] -
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * SC.KoefRKorSpir[1], 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefRKorSpir[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(SC.KoefRKorSpir[0] +
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * SC.KoefRKorSpir[1], 4));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Критерій Бартлетта для\n H0: D{y/x} ==const";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KriterBarkleta[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.KriterBarkleta[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.KriterBarkleta[0] <= SC.KriterBarkleta[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Коеф. Детермінації";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.KoefDeterm) + "%";

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "a";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(SC.AB[0] - SC.AB[2] * SC.T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.AB[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(SC.AB[0] + SC.AB[2] * SC.T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(SC.AB[2]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "b";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(SC.AB[1] - SC.AB[3] * SC.T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.AB[1]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(SC.AB[1] + SC.AB[3] * SC.T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(SC.AB[3]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Перевірка адекватності відтвореної моделі регресії";
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(SC.Szal / ML[MSelectGR[1]].Dx.Q, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.FisherQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count - 1, ML[MSelectGR[0]].l.Count - 3), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.Szal / ML[MSelectGR[1]].Dx.Q <= Distributions.FisherQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count - 1, ML[MSelectGR[0]].l.Count - 3));

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Перевірка адекватності відтвореної моделі регресії,що враховуэ вигляд регресійної залежності";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.ProvRegrs[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.ProvRegrs[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.ProvRegrs[0] <= SC.ProvRegrs[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "Оцінка адекватності відтворення двовимірної функції Х^2 ";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.X2f[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(SC.X2f[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(SC.X2f[0] <= SC.X2f[1]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "a за Тейлом";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.ABTeil[0]);

                MaindatGridView.Rows.Add();NumRows++;
                MaindatGridView[0, NumRows].Value = "b за Тейлом";
                MaindatGridView[2, NumRows].Value = Convert.ToString(SC.ABTeil[1]);

            }
            dataGridView.Rows.Clear();
            dataGridView.ColumnCount = MSelectGR.Count;
            dataGridView.Rows.Add((int)(SC.Nmax + 1));
            for (int i = 0; i < MSelectGR.Count; i++)
            {
                dataGridView.Columns[i].Width = 55;
                dataGridView.Columns[i].Name = "X" + (i + 1).ToString();
                for (int j= 0; j < ML[MSelectGR[i]].l.Count; j++)
                    dataGridView[i, j].Value = (Math.Round(ML[MSelectGR[i]].unsortl[j],2));
            }
        }
        static public void ReWraitData(DataGridView MaindatGridView, DataGridView dataGridView, Correlation_RegressionAnalysis CRA, List<InitialStatisticalAnalys> ML, List<int> MSelectGR)
        {
            int NumRows = -1;
            ///++++++++++++++++++++++++++++++++++++++++++++++
            ///DATA_WRITE
            ///++++++++++++++++++++++++++++++++++++++++++++++
            MaindatGridView.Rows.Clear();
            MaindatGridView.ColumnCount = 7;
            MaindatGridView.Columns[0].Width = 250;
            int sz = 70;
            MaindatGridView.Columns[0].HeaderText = "Назва";
            MaindatGridView.Columns[1].Width = sz;
            MaindatGridView.Columns[1].HeaderText = "Qнижнье";
            MaindatGridView.Columns[2].Width = sz;
            MaindatGridView.Columns[2].HeaderText = "Q";
            MaindatGridView.Columns[3].Width = sz;
            MaindatGridView.Columns[3].HeaderText = "Qверхнье";
            MaindatGridView.Columns[4].Width = sz;
            MaindatGridView.Columns[4].HeaderText = "G(Q)";
            MaindatGridView.Columns[5].Width = sz;
            MaindatGridView.Columns[5].HeaderText = "Qквантиль";
            MaindatGridView.Columns[6].Width = sz;
            MaindatGridView.Columns[6].HeaderText = "H0";





            if (CRA.Doubl)
            {

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кф. Кореляції(r)";
                MaindatGridView[1, NumRows].Value = Convert.ToString(CRA.Korelation[1]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(CRA.Korelation[2]);
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.Korelation[0]);
                MaindatGridView[4, NumRows].Value = Convert.ToString(CRA.Korelation[3]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0:Кф. Кореляції == 0";
                double t = CRA.Korelation[0] * Math.Sqrt(ML[MSelectGR[0]].l.Count - 2) / Math.Sqrt(1 - Math.Pow(CRA.Korelation[0], 2));
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(t, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(
                    Distributions.StudentQuantile(1 - ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(t) <=
                    Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count), 4));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Кореляційне відношення(p)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KorelationVidnoh[0]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: p == 0";
                t = CRA.KorelationVidnoh[0] * Math.Sqrt(ML[MSelectGR[0]].l.Count - 2) / Math.Sqrt(1 - Math.Pow(CRA.KorelationVidnoh[0], 2));
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(t, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(
                    Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(t) <=
                    Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Ранговий коефіціент кореляції Спірмена";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.RangKorelation[0]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Р. коеф. кор. Спірмена == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.RangKorelation[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(CRA.RangKorelation[1]) <= Distributions.StudentQuantile(ML[MSelectGR[0]].alf.Q / 2, ML[MSelectGR[0]].l.Count - 2));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Ранговий коефіціент Кендала";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.RangKoefKend[0]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Р. коеф. Кендала == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.RangKoefKend[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(CRA.RangKoefKend[2]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(CRA.RangKoefKend[1]) <= CRA.RangKoefKend[2]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Індекс Фехнера";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.IndexFehnera);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Коефіціент сполучень Ф(`Фі`)";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefSpolF[0]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Ф == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefSpolF[1]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(CRA.KoefSpolF[2]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(CRA.KoefSpolF[1] >= CRA.KoefSpolF[2]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Коефіціенти зв'язку Юла";
                MaindatGridView[1, NumRows].Value = "Q = " + Convert.ToString(CRA.KoefSvazYoola[0]);
                MaindatGridView[3, NumRows].Value = "Y = " + Convert.ToString(CRA.KoefSvazYoola[1]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Q == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefSvazYoola[2]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(CRA.KoefSvazYoola[2]) <=
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "H0: Y == 0";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefSvazYoola[3]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(Math.Abs(CRA.KoefSvazYoola[3]) <= Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = Convert.ToString(CRA.MeraZvazKendallaStatStyardName);
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(CRA.MeraZvazKendallaStatStyard[0] -
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * CRA.MeraZvazKendallaStatStyard[1], 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.MeraZvazKendallaStatStyard[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(CRA.MeraZvazKendallaStatStyard[0] +
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * CRA.MeraZvazKendallaStatStyard[1], 4));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Коеф. ранг. кор. Спірмена для табл. сполучень";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(CRA.KoefRKorSpir[0] -
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * CRA.KoefRKorSpir[1], 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefRKorSpir[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(CRA.KoefRKorSpir[0] +
                    Distributions.NormalQuantile(1 - ML[MSelectGR[0]].alf.Q / 2) * CRA.KoefRKorSpir[1], 4));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Критерій Бартлетта для\n H0: D{y/x} ==const";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KriterBarkleta[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(CRA.KriterBarkleta[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(CRA.KriterBarkleta[0] <= CRA.KriterBarkleta[1]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Коеф. Детермінації";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.KoefDeterm) + "%";

                /*MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "a";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(CRA.AB[0] - CRA.AB[2] * CRA.T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.AB[0]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(CRA.AB[0] + CRA.AB[2] * CRA.T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(CRA.AB[2]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "b";
                MaindatGridView[1, NumRows].Value = Convert.ToString(Math.Round(CRA.AB[1] - CRA.AB[3] * CRA.T, 4));
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.AB[1]);
                MaindatGridView[3, NumRows].Value = Convert.ToString(Math.Round(CRA.AB[1] + CRA.AB[3] * CRA.T, 4));
                MaindatGridView[4, NumRows].Value = Convert.ToString(CRA.AB[3]);*/

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Перевірка адекватності відтвореної моделі регресії";
                MaindatGridView[2, NumRows].Value = Convert.ToString(Math.Round(CRA.Szal / ML[MSelectGR[1]].Dx.Q, 4));
                MaindatGridView[5, NumRows].Value = Convert.ToString(Math.Round(Distributions.FisherQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count - 1, ML[MSelectGR[0]].l.Count - 3), 4));
                MaindatGridView[6, NumRows].Value = Convert.ToString(CRA.Szal / ML[MSelectGR[1]].Dx.Q <= Distributions.FisherQuantile(ML[MSelectGR[0]].alf.Q, ML[MSelectGR[0]].l.Count - 1, ML[MSelectGR[0]].l.Count - 3));

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Перевірка адекватності відтвореної моделі регресії,що враховуэ вигляд регресійної залежності";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.ProvRegrs[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(CRA.ProvRegrs[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(CRA.ProvRegrs[0] <= CRA.ProvRegrs[1]);

                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Оцінка адекватності відтворення двовимірної функції Х^2 ";
                MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.X2f[0]);
                MaindatGridView[5, NumRows].Value = Convert.ToString(CRA.X2f[1]);
                MaindatGridView[6, NumRows].Value = Convert.ToString(CRA.X2f[0] <= CRA.X2f[1]);


                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "Тип регресії";
                MaindatGridView[2, NumRows].Value = CRA.RegresTypeVib;
                for (int i = 0; i < CRA.Q.Count;i++)
                {
                    MaindatGridView.Rows.Add(); NumRows++;
                    if (CRA.Q[i].Name != null)
                        MaindatGridView[0, NumRows].Value = CRA.Q[i].Name;
                    if (CRA.Q[i].QButton != 0)
                        MaindatGridView[1, NumRows].Value = Math.Round(CRA.Q[i].QButton,4);
                    if (CRA.Q[i].Q != 0)
                        MaindatGridView[2, NumRows].Value = Math.Round(CRA.Q[i].Q,4);
                    if (CRA.Q[i].QUpper != 0)
                        MaindatGridView[3, NumRows].Value = Math.Round(CRA.Q[i].QUpper,4);
                    if (CRA.Q[i].QSigma != 0)
                        MaindatGridView[4, NumRows].Value = Math.Round(CRA.Q[i].QSigma,4);
                }
                if (CRA.RegresTypeVib == RegresTypeName.LineRegresion)
                {
                    MaindatGridView.Rows.Add(); NumRows++;
                    MaindatGridView[0, NumRows].Value = "a за Тейлом";
                    MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.ABTeil[0]);

                    MaindatGridView.Rows.Add(); NumRows++;
                    MaindatGridView[0, NumRows].Value = "b за Тейлом";
                    MaindatGridView[2, NumRows].Value = Convert.ToString(CRA.ABTeil[1]);
                }
                MaindatGridView.Rows.Add(); NumRows++;
                MaindatGridView[0, NumRows].Value = "S залишкова";
                MaindatGridView[2, NumRows].Value = Math.Round(CRA.Szal,4);
            }
            dataGridView.Rows.Clear();
            dataGridView.ColumnCount = MSelectGR.Count;
            dataGridView.Rows.Add((int)(CRA.Nmax));
            for (int i = 0; i < MSelectGR.Count; i++)
            {
                dataGridView.Columns[i].Width = 55;
                dataGridView.Columns[i].Name = "X" + (i + 1).ToString();
                for (int j = 0; j < ML[MSelectGR[i]].l.Count; j++)
                    dataGridView[i, j].Value =(Math.Round(ML[MSelectGR[i]].unsortl[j],2));
            }
        }


        static public void ReWraitData(DataGridView MaindatGridView, DataGridView dataGridView, 
            DataGridView CorelatDataGridView, InitialAnalysMultidimensionalData IAM, List<InitialStatisticalAnalys> ML, List<int> MSelectGR)
        {
            double Nmax = 0;
            dataGridView.Rows.Clear();
            dataGridView.ColumnCount = MSelectGR.Count;
            CorelatDataGridView.ColumnCount = MSelectGR.Count;
            foreach (DataGridViewColumn column in CorelatDataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            CorelatDataGridView.Rows.Clear();
            CorelatDataGridView.Rows.Add(MSelectGR.Count );
            for (int i = 0; i < MSelectGR.Count;i++ ) 
                Nmax = Math.Max(Nmax, ML[MSelectGR[i]].unsortl.Length);
            dataGridView.Rows.Add((int)(Nmax));
            for (int i = 0; i < MSelectGR.Count; i++)
            {
                dataGridView.Columns[i].Width = 55;
                dataGridView.Columns[i].Name = "X" + (i + 1).ToString();
                for (int j = 0; j < ML[MSelectGR[i]].l.Count; j++)
                    dataGridView[i, j].Value = (Math.Round(ML[MSelectGR[i]].unsortl[j], 2));
            }

            MaindatGridView.ColumnCount = MSelectGR.Count + 1;
            MaindatGridView.Columns[0].Width = 130;
            MaindatGridView.Rows.Clear();
            MaindatGridView.Rows.Add(MSelectGR.Count + 24);
            MaindatGridView[0, 0].Value = "vectorE";
            MaindatGridView.Columns[0].HeaderText = "";
            for (int i = 0; i < IAM.Ex.Length;i++ )
            {
                MaindatGridView.Columns[i + 1].HeaderText = "";
                MaindatGridView[i + 1,0].Value = Convert.ToString(IAM.Ex[i]);
            }
            MaindatGridView[0, 2].Value = "matrixDC";
            for (int i = 0; i < IAM.DC.GetLength(0); i++)
            {
                for (int j = 0; j < IAM.DC.GetLength(1); j++)
                {
                    MaindatGridView[j + 1, 2 + i].Value = Convert.ToString(Math.Round(IAM.DC[i, j],4));
                }
            }
            //MaindatGridView[0,3 + IAM.DC.GetLength(0)].Value = "matrixK";
            for (int i = 0; i < IAM.K.GetLength(0); i++)
            {
                for (int j = 0; j < IAM.K.GetLength(1); j++)
                {
                    CorelatDataGridView[j, i].Style.BackColor = Color.Green;
                    CorelatDataGridView[j, i].Value = Convert.ToString(Math.Round(IAM.K[i, j], round));
                }
            }
            int nRow = MSelectGR.Count + 3;
            MaindatGridView[0, nRow].Value = "k = ";
            MaindatGridView[1, nRow++].Value = IAM.k + 1;
            MaindatGridView[0, nRow].Value = IAM.CMC[0].Name;
            for (int i = 0; i < IAM.n; i++)
            {
                MaindatGridView[i+1, nRow].Value = Math.Round(IAM.CMC[i].Q, round);
                MaindatGridView[0,   nRow + 1].Value = "H0:КБК = 0";
                MaindatGridView[i+1, nRow + 1].Value = Math.Round(IAM.CMC[i].QKvant, round);
                MaindatGridView[i+1, nRow + 2].Value = Math.Round(IAM.CMC[i].Q0, round);
                MaindatGridView[i + 1, nRow + 3].Value = IAM.CMC[i].QKvant < IAM.CMC[i].Q0;
            }
            nRow += 4;
            nRow++;
            MaindatGridView[0, nRow].Value = "A0 = ";
            MaindatGridView[1, nRow++].Value = Math.Round(IAM.A[0].Q, 4);
            MaindatGridView[0, nRow].Value = "Ai = ";
            for (int col = 1; col < IAM.n; col++)
            {
                int colreal = col;
                if (colreal - 1 >= IAM.k)
                    colreal++;
                MaindatGridView[colreal, nRow].Value = Math.Round(IAM.A[col].Q, 4);
            }
            nRow++;
            for (int i = 0; i < IAM.Ex.Length; i++)
                MaindatGridView[i+1,nRow].ReadOnly = false;
            nRow++;
            nRow++;
            nRow++;
            MaindatGridView[0, nRow].Value = "Ck = ";
            for (int i = 0; i < IAM.n - 1; i++)
            {
                int ireal = i;
                if (i >= IAM.k)
                    ireal++;
                MaindatGridView[ireal + 1, nRow].Value = Math.Round(IAM.C[i,i], 4);
            }
            nRow++;
            MaindatGridView[0, nRow].Value = "Szal = ";
            MaindatGridView[1, nRow++].Value = Math.Round(IAM.Szal, 4);
            MaindatGridView[0, nRow].Value = "Sigma Оцінка = ";
            MaindatGridView[1, nRow++].Value = Math.Round(IAM.SigmKv, 4);
            MaindatGridView[0, nRow].Value = "Коефіціент Детермінації = ";
            MaindatGridView[1, nRow++].Value = Math.Round(IAM.CoefDeter.Q, 4);
            MaindatGridView[0, nRow].Value = IAM.ZnachRegres.Name;
            MaindatGridView[1, nRow].Value = Math.Round(IAM.ZnachRegres.QKvant, 4);
            MaindatGridView[2, nRow].Value = Math.Round(IAM.ZnachRegres.Q0, 4);
            MaindatGridView[3, nRow++].Value = IAM.ZnachRegres.QKvant > IAM.ZnachRegres.Q0;
            MaindatGridView[0, nRow].Value = "Sigma Оцінка = Sigma";
            {
                double u = (IAM.N - IAM.n) * Math.Pow(IAM.SigmKv / IAM.Szal, 2);
                double uQv = Hi.HIF(IAM.ISA[0].alf.Q, IAM.N - IAM.n);
                MaindatGridView[1, nRow].Value = Math.Round(u, 4);
                MaindatGridView[2, nRow].Value = Math.Round(uQv, 4);
                MaindatGridView[3, nRow++].Value = u< uQv;
            }

            MaindatGridView[0, nRow].Value = "AiStandatr = ";
            for (int col = 0; col < IAM.Astandart.Length; col++)
            {
                int colreal = col;
                if (colreal >= IAM.k)
                    colreal++;
                MaindatGridView[colreal+1, nRow].Value = Math.Round(IAM.Astandart[col].Q, 4);
            }
        }

        static private Color ColorTreeView(int index)
        {
            if (index == 0)
            {
                return Color.White;
            }
            else if (index == 1)
            {
                return Color.Green;
            }
            else if (index == 2)
            {
                return Color.DarkSalmon;
            }
            else if (index == 3)
            {
                return Color.Red;
            }
            else if (index == 4)
            {
                return Color.Orange;
            }
            else if (index == 5)
            {
                return Color.Blue;
            }
            else if (index == 6)
            {
                return Color.Brown;
            }
            else
                return Color.FromArgb(index * 10000);
        }
    }
}
