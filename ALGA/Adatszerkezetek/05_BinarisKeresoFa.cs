using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 5. heti labor feladat - Tesztek: 05_BinarisKeresoFaTesztek.cs

    public class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;

        public FaElem(T tart, FaElem<T>? bal, FaElem<T>? jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        FaElem<T>? gyoker;

        void ReszfaBejarasPreorder(FaElem<T> p, Action<T> muvelet)
        {
            if(p!=null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreorder(p.bal, muvelet);
                ReszfaBejarasPreorder(p.jobb, muvelet);
            }
        }

        void ReszfaBejarasPostorder(FaElem<T> p, Action<T> muvelet)
        {
            if(p!= null)
            {
                ReszfaBejarasPostorder(p.bal, muvelet);
                ReszfaBejarasPostorder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }

        void ReszfaBejarasInorder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasInorder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasInorder(p.jobb, muvelet);
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreorder(gyoker, muvelet);
        }

        FaElem<T> ReszfabaBeszur(FaElem<T>? p, T ertek)
        {
            if (p == null)
            {
                FaElem<T> uj = new FaElem<T>(ertek, null, null);
                return uj;
            }
            else
            {
                if(p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else
                {
                    if(p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabaBeszur(p.jobb, ertek);
                    }
                }
                return p;
            }
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        bool ReszfaEleme(FaElem<T>? p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        return ReszfaEleme(p.jobb, ertek);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        FaElem<T> ReszfabolTorol(FaElem<T>? p, T ertek)
        {
            if(p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                    {
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    }
                    else
                    {
                        if (p.bal == null)
                        {
                            FaElem<T> q = p;
                            p = p.jobb;
                        }
                        else
                        {
                            if (p.jobb == null)
                            {
                                FaElem<T> q = p;
                                p = p.bal;
                            }
                            else
                            {
                                p.bal = KetGyerek(p, p.bal);
                            }
                        }
                    }
                }
                return p;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        FaElem<T> KetGyerek(FaElem<T>? e, FaElem<T> r)
        {
            if (r.jobb != null)
            {
                r.jobb = KetGyerek(e, r.jobb);
                return r;
            }
            else
            {
                e.tart = r.tart;
                FaElem<T> q = r;
                r = r.bal;
                return r;
            }
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }
    }
}
