using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel {  get; set; }

        public FeltetelesFeladatTarolo(int size) : base(size)
        {
        }

        public void FeltetelesVegrehajtas(Predicate<T> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            Func<T, bool> feltetel = BejaroFeltetel ?? (_ => true);

            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                    yield return tarolo[i];
            }
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisIndex = -1;
        Func<T, bool> bejaroFeltetel;
        

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> bejaroFeltetel)
        {
            this.tarolo = tarolo;
            this.n = n;
            this.bejaroFeltetel = bejaroFeltetel;
        }

        public T Current => tarolo[aktualisIndex];

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {  
        }

        public bool MoveNext()
        {
            while (aktualisIndex + 1 < n)
            {
                aktualisIndex++;
                if (bejaroFeltetel(tarolo[aktualisIndex]))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}
