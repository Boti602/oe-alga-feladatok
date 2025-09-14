using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 1. heti labor feladat - Tesztek: 01_ImperativParadigmaTesztek.cs
    public interface IVegrehajthato
    {
        public void Vegrehajtas();
    }

    
    public class TaroloMegteltKivetel : Exception { }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;

        public FeladatTarolo(int size)
        {
            tarolo = new T[size];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n < tarolo.Length)
            {
                tarolo[n] = elem;
                n++;
            }
            else
                throw new TaroloMegteltKivetel(); 
        }

        public IEnumerator<T> GetEnumerator()
        {
            FeladatTaroloBejaro<T> bejaro = new FeladatTaroloBejaro<T>(tarolo, n);
            return bejaro;
        }

        public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size) : base(size)
        {

        }
        public new void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisIndex = -1;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public T Current
        {
            get
            {
                return tarolo[aktualisIndex];
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if(aktualisIndex < n-1)
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

