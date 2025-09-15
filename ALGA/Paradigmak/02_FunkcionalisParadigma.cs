using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    // 2. heti labor feladat - Tesztek: 02_FunkcionálisParadigmaTesztek.cs

    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
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
    }

    public class FeltetelesFeladatTaroloBejaro<T>
    {
        FeltetelesFeladatTaroloBejaro<T> tarolo;
        int n;
        int aktualisIndex = -1;

        FeltetelesFeladatTaroloBejaro<T> bejaroFeltetel;

        public FeltetelesFeladatTaroloBejaro(FeltetelesFeladatTaroloBejaro<T> tarolo, int n, Func<T, bool> bejaroFeltetel)
        {
            this.tarolo = tarolo;
            this.n = n;

        }

    }


}
