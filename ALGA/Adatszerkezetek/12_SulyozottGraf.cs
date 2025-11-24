using System;
using System.Data;

namespace OE.ALGA.Adatszerkezetek
{
    // 12. heti labor feladat - Tesztek: 12_SulyozottGrafTesztek.cs

    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            this.Suly = suly;
        }

        public float Suly { get; }
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n;
        float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = float.NaN;
                }
            }
        }

        public int CsucsokSzama => n;

        public int ElekSzama
        {
            get
            {
                int elekSzama = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if ((M[i, j] != float.NaN)) elekSzama++;
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

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                Halmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if ((!float.IsNaN(M[i, j])))
                        {
                            elek.Beszur(new SulyozottEgeszGrafEl(i, j, float.NaN));
                        }
                    }
                }
                return elek;
            }
        }

        public float Suly(int honnan, int hova)
        {
            if(!VezetEl(honnan, hova))
            {
                throw new NincsElKivetel();
            }
            else
            {
                return M[honnan, hova];
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            Halmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (!float.IsNaN(M[csucs, i]))
                {
                    szomszedok.Beszur(i);
                }
            }
            return szomszedok;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            if(!float.IsNaN(M[honnan, hova]))
                {
                return true;
            }
            else return false;
        }
    }

    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
        {
            Szotar<V, float> P = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> L = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (ez, ennel) => P.Kiolvas(ez) < P.Kiolvas(ennel));

            g.Csucsok.Bejar(x =>
            {
                P.Beir(x, float.MaxValue);
                S.Sorba(x);
            });
            P.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                g.Szomszedai(u).Bejar(x =>
                {
                   if((L.Kiolvas(u) + g.Suly(u, x)) < L.Kiolvas(x))
                   {
                        L.Beir(x, L.Kiolvas(u) + g.Suly(u, x);
                        P.Beir(x, u);

                        S.Frissit(x);
                   }
                });
            }
            return  ;
        }
    }
}
