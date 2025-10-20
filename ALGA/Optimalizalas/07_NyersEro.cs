using System;

namespace OE.ALGA.Optimalizalas
{
    // 7. heti labor feladat - Tesztek: 07_NyersEroTesztek.cs

    public class HatizsakProblema
    {
        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            this.n = n;
            Wmax = wmax;
            this.w = w;
            this.p = p;
        }

        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public int OsszSuly(bool[] pakolas)
        {
            int s = 0;
            int hossz = Math.Min(pakolas.Length, w.Length);
            for (int i = 0; i < hossz; i++)
            {
                if(pakolas[i] == true)
                {
                    s = s + w[i];
                }
            }
            return s;
        }

        public float OsszErtek(bool[] pakolas)
        {
            float s = 0;
            int hossz = Math.Min(pakolas.Length, p.Length);
            for (int i = 0;i < hossz;i++)
            {
                if (pakolas[i] == true)
                {
                    s = s + p[i];
                }
            }
            return s;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            if(this.OsszSuly(pakolas) <= this.Wmax)
            {
                return true;
            }
           
            return false;   
        }  
    }

    public class NyersEro<T>
    {
        int m;
        Func<int, T> generator;
        Func<T, float> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }

        public T OptimalisMegoldas()
        {
            T o = generator(1);
            for(int i = 1; i < m; i++)
            {
                T x = generator(i);
                LepesSzam++;
                if(josag(x) > josag(o)) 
                {
                    o = x;
                }
            }
            return o;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; }
        HatizsakProblema problema;

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public static bool[] Generator(int i, int n)
        {
            bool[] K = new bool[n];
            int szam = i - 1;

            for (int j = 1; j <= n; j++)
            {
                K[j - 1] = ((szam / (int)Math.Pow(2, j - 1)) % 2) == 1;
            }

            return K;
        }



    }
}
