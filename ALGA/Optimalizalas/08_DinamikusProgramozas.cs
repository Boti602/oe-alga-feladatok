using OE.ALGA.Paradigmak;
using System;

namespace OE.ALGA.Optimalizalas
{
    // 8. heti labor feladat - Tesztek: 08_DinamikusProgramozasTesztek.cs

    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public int LepesSzam { get; private set; }

        public float[,] TablazatFeltoltes()
        {
            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;
            float[] p = problema.p;

            float[,] F = new float[n +1, Wmax + 1];

            for (int t = 0; t <= n; t++)
            {
                F[t, 0] = 0;
            }

            for (int h = 0; h <= Wmax; h++)
            {
                F[0,h] = 0;
            }

            for (int t = 1; t <= n; t++)
            {
                for (int h = 1; h <= Wmax; h++)
                {
                    LepesSzam++;
                    if(h >= w[t-1])
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - w[t - 1]] + p[t - 1]);
                    }
                    else
                    {
                        F[t,h] = F[t-1, h];
                    }
                }
            }

            return F;
        }

        public float OptimalisErtek()
        {
            float[,] F = TablazatFeltoltes();
            return F[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            float[,] F = TablazatFeltoltes();

            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;
            bool[] O = new bool[n];

            int t = n;
            int h = Wmax;

            while(t > 0 && h>0)
            {
                if (F[t,h] != F[t-1,h])
                {
                    O[t-1] = true;
                    h -= w[t-1];
                }
                else
                {
                    O[t-1] = false;
                }
                    t--;
            }
            return O;
        }
    }
}
