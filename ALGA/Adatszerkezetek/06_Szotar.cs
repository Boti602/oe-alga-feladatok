using System;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 6. heti labor feladat - Tesztek: 06_SzotarTesztek.cs

    internal class SzotarElem<K,T>
    {
        public K kulcs;
        public T tart;
        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        SzotarElem<K, T>[]? E;
        Func<K, int> h;
        Lista<SzotarElem<K, T>> U = new LancoltLista<SzotarElem<K, T>>();

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
           E = new SzotarElem<K, T>[meret];
            h = (x => Math.Abs(hasitoFuggveny(x)) % E.Length);
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this (meret, x => x.GetHashCode())
        {
        }

        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            int i = h(kulcs);

            if (E[i] is not null && EqualityComparer<K>.Default.Equals(E[i].kulcs, kulcs))
            {
                return E[i];
            }

            SzotarElem<K, T> eredmeny = null;

            U.Bejar(x =>
            {
                if (EqualityComparer<K>.Default.Equals(x.kulcs, kulcs))
                {
                    eredmeny = x;
                }
            });

            return eredmeny;
        }


        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> meglevo;

            meglevo = KulcsKeres(kulcs);
            if(meglevo != null)
            {
                meglevo.tart = ertek;
            }
            else
            {
                SzotarElem<K,T> uj = new SzotarElem<K, T>(kulcs, ertek);
                if (E[h(kulcs)] == null)
                {
                    E[h(kulcs)] = uj;
                }
                else
                {
                    U.Hozzafuz(uj);
                }
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> meglevo;
            meglevo = KulcsKeres(kulcs);
            if(meglevo != null)
            {
                return meglevo.tart;
            }
            else
            {
                throw new HibasKulcsKivetel();
            }
        }

        public void Torol(K kulcs)
        {
            int i = h(kulcs);

            if (E[i] is not null && EqualityComparer<K>.Default.Equals(E[i].kulcs, kulcs))
            {
                E[i] = null;
            }
            else
            {
                SzotarElem<K, T> e = null;
                U.Bejar(x =>
                {
                    if (EqualityComparer<K>.Default.Equals(x.kulcs, kulcs))
                    {
                        e = x;
                    }
                });

                if (e != null)
                {
                    U.Torol(e);
                }
                else
                {
                    throw new HibasKulcsKivetel();
                }
            }
        }

    }
}
