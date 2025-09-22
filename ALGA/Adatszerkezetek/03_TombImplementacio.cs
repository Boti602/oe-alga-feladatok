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
                T ertek = E[n - 1];
                n--;
                return ertek;
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
                u = (u+1) % E.Length;
                E[u] = ertek;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if( n > 0)
            {
                n--;
                e = (e+1) % E.Length;
                return E[e];
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }

    public class TombLista<T> : Lista<T>
    {
        T[] E;
        int n = 0;

        public TombLista(int meret)
        {
            E = new T[meret];
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
            throw new NotImplementedException();
        }

        public void Beszur(int index, T ertek)
        {
            throw new NotImplementedException();
        }

        public void Hozzafuz(T ertek)
        {
            throw new NotImplementedException();
        }

        public T Kiolvas(int index)
        {
            throw new NotImplementedException();
        }

        public void Modosit(int index, T ertek)
        {
            throw new NotImplementedException();
        }

        public void Torol(T ertek)
        {
            throw new NotImplementedException();
        }
    }
}
