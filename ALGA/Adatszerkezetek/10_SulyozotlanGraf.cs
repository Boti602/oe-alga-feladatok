using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 10. heti labor feladat - Tesztek: 10_SulyozatlanGrafTesztek.cs

    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int Honnan { get; }
        public int Hova { get; }

        public int CompareTo(EgeszGrafEl other)
        {
            if (other == null) return 1;
            if (Honnan != other.Honnan)
                return Honnan.CompareTo(other.Honnan);
            return Hova.CompareTo(other.Hova);
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {

        int n = 0;
        bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }

        public int CsucsokSzama
        {
            get
            {
                return n;
            }
        }

        public int ElekSzama
        {
            get
            {
                int elekSzama = 0;
                for (int i = 0; i < n; i++)
                {   
                    for (int j = 0; j < n; j++)
                    {
                        if(M[i,j]) elekSzama++;
                    }
                }
                return elekSzama;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                Halmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                Halmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0;i < n;i++)
                {
                    for(int j = 0;j < n;j++)
                    {
                        if (M[i,j])
                        {
                            elek.Beszur(new EgeszGrafEl(i,j));
                        }
                    }
                }   
                return elek;
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
           Halmaz<int> szomszedok = new FaHalmaz<int>();
           for(int i = 0; i <n ; i++)
           {
                if (M[csucs, i])
                {
                    szomszedok.Beszur(i);
                }
           }
           return szomszedok;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            if (M[honnan, hova] == true)
            {
                return true;
            }
            else return false;
        }
    }

    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V,E>(Graf<V,E> g, V start, Action<V> muvelet) where V: IComparable<V>
        {
            Sor<V> S = new LancoltSor<V>();
            Halmaz<V> F = new FaHalmaz<V>();

            S.Sorba(start);
            F.Beszur(start);

            while(!S.Ures)
            {
                V k = S.Sorbol();
                muvelet(k);

                Halmaz<V> szomszedok = g.Szomszedai(k);

                szomszedok.Bejar(x =>
                {
                    if(!F.Eleme(x))
                    {
                        S.Sorba(x);
                        F.Beszur(x);
                    }
                });
            }
            return F;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
           Halmaz<V> F = new FaHalmaz<V>();
           MelysegiBejarasRekurzio(g, start, F, muvelet);
           return F;
        }

        public static void MelysegiBejarasRekurzio<V,E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);

            Halmaz<V> szomszedok = g.Szomszedai(k);
            szomszedok.Bejar(x =>
            {
                if(!F.Eleme(x))
                {
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
                }
            });
        }
    }
}