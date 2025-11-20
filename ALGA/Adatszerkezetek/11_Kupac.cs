using System;
using System.ComponentModel.DataAnnotations;

namespace OE.ALGA.Adatszerkezetek
{
    // 11. heti labor feladat - Tesztek: 11_KupacTesztek.cs

    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Szulo(int i)
        {
            return (i -1)/ 2;
        }
        public static int Bal(int i)
        {
            return 2 * i + 1;
        }
        public static int Jobb(int i)
        {
            return 2*i + 2;
        }

        protected void Kupacol(int i)
        {
            int b = Bal(i);
            int j = Jobb(i);
            int max;
            if(b < n && nagyobbPrioritas(E[b], E[i]))
            {
                max = b;
            }
            else
            {
                max = i;
            }
            if(j< n && nagyobbPrioritas(E[j], E[max]))
            {
                max = j;
            }
            if(max != i)
            {
                (E[i], E[max]) = (E[max], E[i]);
                Kupacol(max);
            }
        }

        protected void KupacotEpit()
        {
            for (int i = n/2 -1; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable
    {
        public KupacRendezes(T[] E) : base(E, E.Length, (x,y) => x.CompareTo(y) > 0)
        {
                
        }

        public void Rendezes()
        {
            KupacotEpit();
            for (int i = n-1; i > 0; i--)
            {
                (E[0], E[i]) = (E[i], E[0]);
                n--;
                Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas) : base(new T[meret], 0, nagyobbPrioritas)
        {
            
        }

        public bool Ures => n == 0;

        public T Elso()
        {
            if (!Ures)
            {
                return E[0];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Frissit(T elem)
        {
            int i = 0;
            while (i <= n&& !E[i].Equals(elem))
            {
                i++;
            }
            if(i<=n)
            {
                KulcsotFelvisz(i);
                Kupacol(i);
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Sorba(T ertek)
        {
            if(n < E.Length)
            {
                n++;
                E[n-1] = ertek;
                KulcsotFelvisz(n-1);
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            T max;
            if (!Ures)
            {
                max = E[0];
                E[0] = E[n-1];
                n--;
                Kupacol(0);
                return max;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        private void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);
            if(sz>=0 && nagyobbPrioritas(E[i], E[sz]))
            {
                (E[sz], E[i]) = (E[i], E[sz]);
                KulcsotFelvisz(sz);
            }
        }
    }
}