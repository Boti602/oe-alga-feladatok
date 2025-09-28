using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 3. heti labor feladat - Tesztek: 03_TombImplementacioTesztek.cs

    public class TombVerem<T> : Verem<T>
    {
        T[] E;
        int n = 0;
        public bool Ures
        {
            get
            {
                return n == 0;
            }

        }

        public TombVerem(int meret)
        {
            E = new T[meret];
        }

        public T Felso()
        {
            if (!Ures)
            {
                return E[n - 1];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Verembe(T ertek)
        {
            if (n < E.Length)
            {
                E[n] = ertek;
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Verembol()
        {
            if (n > 0)
            {
                n--;
                return E[n];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }

    public class TombSor<T> : Sor<T>
    {
        T[] E;
        int n = 0;
        int e = 0;
        int u = 0;

        public TombSor(int meret)
        {
            E = new T[meret];
        }

        public bool Ures
        {
            get
            {
                return n == 0;
            }
        }

        public T Elso()
        {
            if (n > 0)
            {
                return E[(e % E.Length) + 1];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                n++;
                u = (u + 1) % E.Length;
                E[u] = ertek;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if (n > 0)
            {
                n--;
                e = (e + 1) % E.Length;
                return E[e];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }

    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        T[] E;
        int n;

        public TombLista() : this(4) { }

        public TombLista(int meret)
        {
            E = new T[meret];
            n = 0;
        }

        public int Elemszam
        {
            get
            {
                return n;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (index <= n)
            {
                if (n == E.Length)
                {
                    MeretNovel();
                }

                n++;
                for (int i = n-1; i > index; i--)
                {
                    E[i] = E[i - 1];
                }
                E[index] = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index <= n - 1)
            {
                return E[index];
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Modosit(int index, T ertek)
        {
            if (index <= n - 1)
            {
                E[index] = ertek;
            }
            else
            {
                throw new HibasIndexKivetel();
            }
        }

        public void Torol(T ertek)
        {
            int db = 0;
            for (int i = 0; i < n; i++)
            {
                if (E[i]!.Equals(ertek))
                {
                    db++;
                }
                else
                {
                    E[i - db] = E[i];
                }
            }
            n = n - db;
        }
        public void MeretNovel()
        {
            T[] E2 = E;
            E = new T[E.Length * 2];
            for (int i = 0; i < n; i++)
            {
                E[i] = E2[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            TombListaBejaro<T> bejaro = new TombListaBejaro<T>(E, n);
            return bejaro;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        T[] E;
        int n = 0;
        int aktualisIndex = -1;

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
        }

        public T Current => E[aktualisIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (aktualisIndex < n - 1)
            {
                aktualisIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}
