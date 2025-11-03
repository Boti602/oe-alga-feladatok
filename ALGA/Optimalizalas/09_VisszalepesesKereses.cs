using System;
using System.Collections.Generic;

namespace OE.ALGA.Optimalizalas
{
    // 9. heti labor feladat - Tesztek: 09VisszalepesesKeresesTesztek.cs

    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], float> josag;

        public VisszalepesesOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            M = m;
            R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public int LepesSzam { get; private set; }

        protected void Backtrack(int szint, T[] E, ref bool van, T[] O)
        {
            int i = -1;
            while (!van && i < M[szint])
            {
                i++;
                LepesSzam++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                O = E;
                            }
                            van = true;
                        }
                        else
                        {
                            Backtrack(szint + 1, E, ref van, O);
                            
                        }
                    }
                }
            }
        }

        public bool[] OptimalisMegoldas()
        {
            bool van = false;
            T[] E = new T[n];    
            T[] O = new T[n];    
            LepesSzam = 0;

            
            Backtrack(0, E, ref van, O);

           
            if (!van)
                return new bool[n];

            
            bool[] eredmeny = new bool[n];
            for (int i = 0; i < n; i++)
            {
                
                eredmeny[i] = !EqualityComparer<T>.Default.Equals(O[i], default(T));
            }

            return eredmeny;
        }

    }

    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema problema;
        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
           this.problema = problema;
            LepesSzam = 0;
        }

        public int LepesSzam { get; private set; }

        bool[] optimalisMegoldas;
        double optimalisErtek;

        public bool[] OptimalisMegoldas()
        {
            int n = problema.n;

            int[] M = new int[n];
            bool[,] R = new bool[n, 2];

            for (int i = 0; i<n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, valasztas) => true;

            Func<int, bool, bool[], bool> fk = (szint, valasztas, E) =>
            {
                double sulySum = 0;
                for (int i = 0; i < szint; i++)
                    if (E[i]) sulySum += problema.w[i];

                if (valasztas)
                    sulySum += problema.w[szint];

                return sulySum <= problema.Wmax;
            };

            Func<bool[], float> josag = (E) =>
            {
                float ossz = 0;
                for (int i = 0; i < n; i++)
                    if (E[i]) ossz += problema.p[i];
                return ossz;
            };

            var opt = new VisszalepesesOptimalizacio<bool>(n, M, R, ft, fk, josag);

            bool[] megoldas = opt.OptimalisMegoldas();

            LepesSzam = opt.LepesSzam;
            optimalisMegoldas = megoldas;
            optimalisErtek = josag(megoldas);

            return megoldas;
        }

        public double OptimalisErtek()
        {
            return optimalisErtek;
        }
    }
}

