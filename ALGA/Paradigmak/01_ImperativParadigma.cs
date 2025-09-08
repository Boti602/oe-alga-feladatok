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
        T[] tarolo;
        int n;

        public FeladatTarolo(int size)
        {
            tarolo = new T[size];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            throw new TaroloMegteltKivetel();
            //todo
        }

        public void MindentVegrehajt()
        {
            //todo
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerable<T>
    { }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int size):base(size) 
        {
            
        }

        public bool FuggosegTeljesul => throw new NotImplementedException();
    }
}

