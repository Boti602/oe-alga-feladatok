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

        public int CompareTo(EgeszGrafEl other)
        {
            if (other == null) return 1;
            if (Honnan != other.Honnan)
                return Honnan.CompareTo(other.Honnan);
            return Hova.CompareTo(other.Hova);
        }
    }

    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n;
        float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
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
                        if (!float.IsNaN(M[i,j])) 
                            elekSzama++;
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
                            elek.Beszur(new SulyozottEgeszGrafEl(i, j, M[i,j]));
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
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (ez, ennel) => L.Kiolvas(ez) < L.Kiolvas(ennel));

            g.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.MaxValue);
                S.Sorba(x);
            });
            L.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                g.Szomszedai(u).Bejar(x =>
                {
                   if((L.Kiolvas(u) + g.Suly(u, x)) < L.Kiolvas(x))
                   {
                        L.Beir(x, L.Kiolvas(u) + g.Suly(u, x));
                        P.Beir(x, u);

                        S.Frissit(x);
                   }
                });
            }
            return L;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable<V>
        {
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (ez, ennel) => K.Kiolvas(ez).CompareTo(K.Kiolvas(ennel)) < 0);
            Halmaz<V> W = new FaHalmaz<V>();

            g.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.MaxValue);
                S.Sorba(x);
                W.Beszur(x);
            });
            K.Beir(start, 0);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                W.Torol(u);
                g.Szomszedai(u).Bejar(x =>
                {
                    if(W.Eleme(x) && g.Suly(u, x) < K.Kiolvas(x))
                    {
                        K.Beir(x, g.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return P;
        }

        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g) where E : SulyozottGrafEl<V>, IComparable<E>
        {
            Halmaz<E> A = new FaHalmaz<E>();
            Szotar<V, int> vhalmaz = new HasitoSzotarTulcsordulasiTerulettel<V, int>(g.CsucsokSzama);
            KupacPrioritasosSor<E> S = new KupacPrioritasosSor<E>(g.ElekSzama, (e1, e2) => e1.Suly < e2.Suly);
            
            int i = 0;
            g.Csucsok.Bejar(x => { vhalmaz.Beir(x, i++); });

            g.Elek.Bejar(e1 =>
            {
                S.Sorba(e1);
            });

            while(!S.Ures)
            {
                E e1 = S.Sorbol();
                V u = e1.Honnan;
                V v = e1.Hova;

                int hu = vhalmaz.Kiolvas(u);
                int hv = vhalmaz.Kiolvas(v);

                if(hu != hv)
                {
                    A.Beszur(e1);

                    int from = hv;
                    int to = hu;

                    g.Csucsok.Bejar(x =>
                    {
                        if (vhalmaz.Kiolvas(x) == from)
                        {
                            vhalmaz.Beir(x, to);
                        }
                    });
                }
            }

            return A;
        }
    }
}
