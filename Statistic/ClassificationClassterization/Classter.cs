using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace testgistogr
{
    delegate double D(double[] X1, double[] X2);
    delegate double DSS(Classter S1, Classter S2);
    delegate AbyStruct Aby(Classter Sl, Classter Sh,Classter Sm);

    struct AbyStruct
    {
        public double al;
        public double ah;
        public double beta;
        public double gama;
    }

    struct Dstruct
    {
        public string Name;
        public D func;
    }

    struct DSSstruct
    {
        public string Name;
        public DSS func;
        public Aby aby;
    }


    class Classter
    {
        List<double[]> data = new List<double[]>();
        public InitialAnalysMultidimensionalData IAM = null;
        public double[] Center = null;
        static private double[,] V = null;
        static public Data[] Q = null;
        static public D d = DEvklid;
        static public DSS dSS = DSSMinNeighborhood;
        static public Aby aby = AbyMinNeighborhood;
        public static Dstruct[] dstruct = new Dstruct[]
        {
            new Dstruct(){ Name = "Евклідова", func = DEvklid},
            new Dstruct(){ Name = "Манхетенська", func = DManhetan},
            new Dstruct(){ Name = "Чебишева", func = DChebisheva},
            new Dstruct(){ Name = "Махаланобіса", func = DMahalanobisa},
        };
        public static DSSstruct[] dSSstruct = new DSSstruct[]
        {
            new DSSstruct(){ Name = "Відстань найближчого сусіда", func = DSSMinNeighborhood, aby = AbyMinNeighborhood},
            new DSSstruct(){ Name = "Відстань найвіддаленішого сусіда", func = DSSMaxNeighborhood, aby = AbyMaxNeighborhood},
            new DSSstruct(){ Name = "Середню зважену відстань", func = DSSAverageWeighted, aby = AbyAverageWeighted},
            new DSSstruct(){ Name = "Середню незважену відстань", func = DSSAverageNotWeighted, aby = AbyAverageNotWeighted},
            new DSSstruct(){ Name = "Медіанну відстань", func = DSSMediana, aby = AbyMediana},
            new DSSstruct(){ Name = "Відстань між центрами", func = DSSCenter, aby = AbyCenter},
            new DSSstruct(){ Name = "Відстань Уорда", func = DSSYorda, aby = AbyYorda},
        };
        int n;


        public Classter(int n)
        {
            this.Center = new double[n];
            this.n = Center.Length;
        }

        public Classter(double[] Center)
        {
            n = Center.Length;
            this.Center = new double[n];
            this.Add(Center);
        }
        public void Add(double[] Data)
        {
            data.Add(Data);
            double[] Center = new double[n];

            for (int j = 0; j < n; j++)
                Center[j] = (this.Center[j] * (data.Count - 1) + Data[j]) / data.Count;
            this.Center = Center;
        }

        public double[] Len(List<double[]> Data)
        {

            double[] LenRes = new double[Data.Count];
            for (int i = 0; i < Data.Count; i++)
                LenRes[i] = d(Center, Data[i]);
            return LenRes;
        }

        static public void DrawClassters(Chart chart, Classter[] classters)
        {
            for (int i = 0; i < classters.Length; i++)
                chart.Series.Add(PaintData.CorelPaint(classters[i].IAM.ISA[0].unsortl, classters[i].IAM.ISA[1].unsortl,
                    PaintData.HsvToRgb(360.0 * i / classters.Length, 1, 1), "Класстер: " + (i + 1).ToString()));
        }

        static public void IAMFound(Classter[] classters)
        {
            for (int i = 0; i < classters.Length; i++)
                classters[i].IAMFound();
        }

        public void IAMFound()
        {
            double[,] X = new double[data.Count, n];
            for (int c = 0; c < n; c++)
                for (int r = 0; r < data.Count; r++)
                    X[r, c] = data[r][c];
            IAM = new InitialAnalysMultidimensionalData(X, -1);
        }

        static public double[,] VFound(InitialAnalysMultidimensionalData IAM)
        {
            double[,] result = new double[IAM.n, IAM.n];
            for (int k = 0; k < IAM.n; k++)
                for (int p = 0; p < IAM.n; p++)
                    for (int i = 0; i < IAM.N; i++)
                        result[k, p] += (IAM.ISA[k].unsortl[i] - IAM.ISA[k].Mx.Q) * 
                            (IAM.ISA[p].unsortl[i] - IAM.ISA[p].Mx.Q);
            V = result;
            return result;
        }

        static public Classter Merge(Classter C1, Classter C2)
        {
            Classter result = new Classter(C1.data[0]);
            for (int i = 1; i < C1.data.Count; i++)
                result.Add(C1.data[i]);
            for (int i = 0; i < C2.data.Count; i++)
                result.Add(C2.data[i]);
            return result;
        }

        public double[] Mediana()
        {
            double[] Me = new double[n];
            List<double>[] Data = new List<double>[n];
            for (int i = 0; i < n; i++)
                Data[i] = new List<double>();
            for (int c = 0; c < n; c++)
            {
                for (int r = 0; r < data.Count; r++)
                    Data[c].Add(data[r][c]);
                Me[c] = InitialStatisticalAnalys.MEDFound(Data[c]);
            }
            return Me;

        }

        public InitialStatisticalAnalys[] ToISA(int MaxGroup)
        {
            InitialStatisticalAnalys[] result = new InitialStatisticalAnalys[n];
            for (int i = 0; i < n; i++)
            {
                List<double> ISAData = new List<double>();
                for (int j = 0; j < data.Count; j++)
                    ISAData.Add(data[j][i]);
                result[i] = new InitialStatisticalAnalys(ISAData, MaxGroup);
            }
            return result;
        }
        #region Dsss
        static public double DSSS(Classter Sl, Classter Sh, Classter Sm)
        {
            AbyStruct abyData =  aby(Sl, Sh, Sm);
            double[] dSSarray = new double[] { dSS(Sl, Sm), dSS(Sh, Sm), dSS(Sl, Sh), Math.Abs(dSS(Sh, Sm) - dSS(Sl, Sm)) };
            return abyData.al * dSSarray[0] +
                abyData.ah * dSSarray[1] +
                abyData.beta * dSSarray[2] +
                abyData.gama * dSSarray[3];
        }
        #endregion
        #region Dss
        static public void ChangeDSS(string typeD)
        {
            for (int i = 0; i < dSSstruct.Length; i++)
                if (dSSstruct[i].Name == typeD)
                {
                    dSS = dSSstruct[i].func;
                    aby = dSSstruct[i].aby;
                }
        }

        private static double DSSMinNeighborhood(Classter S1, Classter S2)
        {
            double min = double.MaxValue;
            for (int l1 = 0; l1 < S1.data.Count; l1++)
                for (int l2 = 0; l2 < S2.data.Count; l2++)
                    min = Math.Min(min, d(S1.data[l1], S2.data[l2]));
            return min;
        }

        private static AbyStruct AbyMinNeighborhood(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct { ah = 0.5, al = 0.5, beta = 0, gama = -0.5 };
        }

        private static double DSSMaxNeighborhood(Classter S1, Classter S2)
        {
            double max = double.MinValue;
            for (int l1 = 0; l1 < S1.data.Count; l1++)
                for (int l2 = 0; l2 < S2.data.Count; l2++)
                    max = Math.Max(max, d(S1.data[l1], S2.data[l2]));
            return max;
        }
        private static AbyStruct AbyMaxNeighborhood(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct { ah = 0.5, al = 0.5, beta = 0, gama = 0.5 };
        }

        private static double DSSAverageWeighted(Classter S1, Classter S2)
        {
            double sum = 0;
            for (int l1 = 0; l1 < S1.data.Count; l1++)
                for (int l2 = 0; l2 < S2.data.Count; l2++)
                    sum += d(S1.data[l1], S2.data[l2]);
            return sum / (S1.data.Count * S2.data.Count);
        }
        private static AbyStruct AbyAverageWeighted(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct { al = (double)Sl.data.Count / (Sl.data.Count + Sh.data.Count),
                ah = (double)Sh.data.Count / (Sl.data.Count + Sh.data.Count),
                beta = 0, gama = 0 };
        }

        private static double DSSAverageNotWeighted(Classter S1, Classter S2)
        {
            double sum = 0;
            for (int l1 = 0; l1 < S1.data.Count; l1++)
                for (int l2 = 0; l2 < S2.data.Count; l2++)
                    sum += d(S1.data[l1], S2.data[l2]);
            return sum / (4.0);
        }
        private static AbyStruct AbyAverageNotWeighted(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct
            {
                al = 0.5,
                ah = 0.5,
                beta = 0,
                gama = 0
            };
        }

        private static double DSSMediana(Classter S1, Classter S2)
        {
            double[] Me1 = S1.Mediana();
            double[] Me2 = S2.Mediana();
            return d(Me1, Me2) / 2.0;
        }
        private static AbyStruct AbyMediana(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct
            {
                al = 0.5,
                ah = 0.5,
                beta = -0.25,
                gama = 0
            };
        }
        private static double DSSCenter(Classter S1, Classter S2)
        {
            double[] Ce1 = S1.Center;
            double[] Ce2 = S2.Center;
            return d(Ce1, Ce2);
        }
        private static AbyStruct AbyCenter(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct
            {
                al = (double)Sl.data.Count / (Sl.data.Count + Sh.data.Count),
                ah = (double)Sh.data.Count / (Sl.data.Count + Sh.data.Count),
                beta = -(double)Sh.data.Count * Sl.data.Count / Math.Pow(Sl.data.Count + Sh.data.Count, 2),
                gama = 0
            };
        }

        private static double DSSYorda(Classter S1, Classter S2)
        {
            double[] Ce1 = S1.Center;
            double[] Ce2 = S2.Center;
            return Math.Pow(d(Ce1, Ce2),2) * S1.data.Count * S2.data.Count / (S1.data.Count + S2.data.Count);
        }
        private static AbyStruct AbyYorda(Classter Sl, Classter Sh, Classter Sm)
        {
            return new AbyStruct
            {
                al = (double)(Sm.data.Count + Sl.data.Count )/ (Sm.data.Count + Sl.data.Count + Sh.data.Count),
                ah = (double)(Sm.data.Count + Sh.data.Count) / (Sm.data.Count + Sl.data.Count + Sh.data.Count),
                beta = -(double)Sm.data.Count / (Sm.data.Count + Sl.data.Count + Sh.data.Count),
                gama = 0
            };
        }
        #endregion
        #region D
        static public void ChangeD(string typeD)
        {
            for (int i = 0; i < dstruct.Length; i++)
                if (dstruct[i].Name == typeD)
                    d = dstruct[i].func;
        }

        private static double DEvklid(double[] S1, double[] S2)
        {
            double result = 0;
            if (S1.Length != S2.Length)
                throw new Exception("Data not correct");
            for (int i = 0; i < S1.Length; i++)
                result += Math.Pow(S1[i] - S2[i], 2);
            result = Math.Sqrt(result);

            return result;
        }
        private static double DMahalanobisa(double[] S1, double[] S2)
        {
            double[,] Xlh = new double[1,S1.Length];
            for (int i = 0; i < S1.Length; i++)
                Xlh[0,i] = S1[i] - S2[i];
            double[,] d = Matrix.MultiplicMatrix(Matrix.MultiplicMatrix(Xlh, Matrix.InverseMatrix(V)), Matrix.TranspMatrix(Xlh));
            return d[0, 0];

        }
        private static double DChebisheva(double[] S1, double[] S2)
        {
            double result = 0;
            if (S1.Length != S2.Length)
                throw new Exception("Data not correct");
            for (int i = 0; i < S1.Length; i++)
                result = Math.Max(Math.Abs(S1[i] - S2[i]), result);
            return result;
        }
        private static double DManhetan(double[] S1, double[] S2)
        {
            double result = 0;
            if (S1.Length != S2.Length)
                throw new Exception("Data not correct");
            for (int i = 0; i < S1.Length; i++)
                result += Math.Abs(S1[i] - S2[i]);

            return result;
        }
        #endregion
        static public Data[] QFound(Classter[] classters)
        {
            List<Data> result = new List<Data>()
            {
                Q1Found(classters),
                Q2Found(classters),
                Q3Found(classters),
                //Q3_Found(classters),
                Q4_Found(classters),
                Q4__Found(classters),
            };
            result.Add(Q4Found(classters, result[result.Count - 2], result[result.Count - 1]));

            Q = result.ToArray();
            return result.ToArray();
        }

        static private Data Q1Found(Classter[] classters)
        {

            Data result = new Data()
            {
                Name = "Сума («зважена») внутрішньокластерних дисперсій",
                Q = 0,
            };
            for (int j = 0; j < classters.Length; j++)
                for (int l = 0; l < classters[j].data.Count; l++)
                    result.Q += Math.Pow(d(classters[j].data[l], classters[j].Center), 2);
            return result;
        }

        static private Data Q2Found(Classter[] classters)
        {

            Data result = new Data()
            {
                Name = "Сума попарних внутрішньокластерних відстаней",
                Q = 0,
            };
            for (int j = 0; j < classters.Length; j++)
                for (int l = 0; l < classters[j].data.Count - 1; l++)
                    for (int h = l + 1; h < classters[j].data.Count; h++)
                        result.Q += d(classters[j].data[l], classters[j].data[h]);
            return result;
        }

        static private Data Q3Found(Classter[] classters)
        {
            Data result = new Data()
            {
                Name = "Загальна внутрішньокластерна дисперсія",
                Q = 0,
            };
            double[,] NV = Matrix.MultiplicNumber(Classter.V, classters[0].data.Count);
            for (int j = 1; j < classters.Length; j++)
                NV = Matrix.Addition(Matrix.MultiplicNumber(Classter.V, classters[0].data.Count), NV);
            result.Q = Matrix.Determinant(NV);
            return result;
        }

        static private Data Q3_Found(Classter[] classters)
        {
            Data result = new Data()
            {
                Name = "Загальна внутрішньокластерна дисперсія 2",
                Q = 1,
            };
            for (int j = 0; j < classters.Length; j++)
                result.Q *= Math.Pow(Matrix.Determinant(Classter.V), classters[j].data.Count);
            return result;
        }

        static private Data Q4_Found(Classter[] classters)
        {
            Data result = new Data()
            {
                Name = "Cередня внутрішньокластерна відстань",
                Q = 0,
            };
            double N = 0;
            for (int j = 0; j < classters.Length; j++)
                N += classters[j].data.Count * (classters[j].data.Count - 1) / 2.0;

            for (int j = 0; j < classters.Length; j++)
                for (int l = 0; l < classters[j].data.Count - 1; l++)
                    for (int h = l + 1; h < classters[j].data.Count; h++)
                        result.Q += d(classters[j].data[l], classters[j].data[h]);
            result.Q = result.Q / N;
            return result;
        }

        static private Data Q4__Found(Classter[] classters)
        {
            Data result = new Data()
            {
                Name = "Cередня міжкластерна відстань",
                Q = 0,
            };
            double N = 1;
            for (int j = 0; j < classters.Length; j++)
                N *= classters[j].data.Count;

            for (int j = 0; j < classters.Length - 1; j++)
                for (int l = 0; l < classters[j].data.Count; l++)
                    for (int m = j + 1; m < classters.Length; m++)
                        for (int h = 0; h < classters[m].data.Count; h++)
                            result.Q += d(classters[j].data[l], classters[m].data[h]);
            result.Q = result.Q / N;
            return result;
        }

        static private Data Q4Found(Classter[] classters, Data Q4_, Data Q4__)
        {
            Data result = new Data()
            {
                Name = "Відношення функціоналів",
                Q = Q4_.Q / Q4__.Q,
            };
            return result;
        }
    }
}
