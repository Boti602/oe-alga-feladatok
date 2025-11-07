using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;

namespace OE.ALGA.Optimalizalas
{
    // 9. heti labor feladat - Tesztek: 09VisszalepesesKeresesTesztek.cs

        public class VisszalepesesOptimalizacio<T>
        {
            protected int n;
            protected int[] M;          // M[szint] = lehetőségek száma az adott szinten
            protected T[,] R;           // R[szint, i] = i. részmegoldás a szinten
            protected Func<int, T, bool> ft;                    // elsődleges feltétel
            protected Func<int, T, T[], bool> fk;               // másodlagos feltétel
            protected Func<T[], float> josag;                   // célfüggvény

            // belső állapot a legjobb megoldásnak
            protected T[] _best;
            protected bool _van;
            protected float _bestValue;

            public VisszalepesesOptimalizacio(int n, int[] m, T[,] r,
                Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
            {
                this.n = n;
                M = m;
                R = r;
                this.ft = ft;
                this.fk = fk;
                this.josag = josag;

                _best = new T[n];
                _van = false;
                _bestValue = float.NegativeInfinity;
            }

            public int LepesSzam { get; protected set; }

            protected virtual void Backtrack(int szint, T[] E)
            {
                for (int i = 0; i < M[szint]; i++)
                {
                    LepesSzam++;

                    T valasztas = R[szint, i];
                    if (!ft(szint, valasztas)) continue;
                    if (!fk(szint, valasztas, E)) continue;

                    E[szint] = valasztas;

                    if (szint == n - 1)
                    {
                        float v = josag(E);
                        if (!_van || v > _bestValue)
                        {
                            // fontos: MÁSOLUNK, nem referencia-átadást használunk
                            Array.Copy(E, _best, n);
                            _bestValue = v;

                        }
                        _van = true;
                    }
                    else
                    {
                        Backtrack(szint + 1, E);
                    }
                }
            }

            // A feladat szerint bool[]-t kell visszaadnunk; a laborban T == bool,
            // így egyszerűen a legjobb E tömböt visszük át bool[] formába.
            public bool[] OptimalisMegoldas()
            {
                LepesSzam = 0;
                _van = false;
                _bestValue = float.NegativeInfinity;

                var E = new T[n];
                Backtrack(0, E);

                if (!_van) return new bool[n];

                var eredmeny = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    // T==bool eset (a laborban így használjuk)
                    eredmeny[i] = Convert.ToBoolean(_best[i]);
                }
                return eredmeny;
            }
        }

        public class VisszalepesesHatizsakPakolas
        {
            protected HatizsakProblema problema;

            public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
            {
                this.problema = problema;
                LepesSzam = 0;
            }

            public int LepesSzam { get; protected set; }

            protected bool[] optimalisMegoldas;
            protected double optimalisErtek;

            public bool[] OptimalisMegoldas()
            {
                int n = problema.n;

                // M és R felépítése: minden szinten két lehetőség: {igaz, hamis}
                int[] M = new int[n];
                bool[,] R = new bool[n, 2];
                for (int i = 0; i < n; i++)
                {
                    M[i] = 2;
                    R[i, 0] = true;
                    R[i, 1] = false;
                }

                // nincs lokális (szint szerinti) tiltás
                Func<int, bool, bool> ft = (szint, valasztas) => true;

                // részfeltétel: súlykorlát ne lépődjön túl
                Func<int, bool, bool[], bool> fk = (szint, valasztas, E) =>
                {
                    double sulySum = 0;
                    for (int i = 0; i < szint; i++)
                        if (E[i]) sulySum += problema.w[i];

                    if (valasztas) sulySum += problema.w[szint];

                    return sulySum <= problema.Wmax;
                };

                // „jóság” = összérték
                Func<bool[], float> josag = (E) =>
                {
                    float ossz = 0;
                    for (int i = 0; i < n; i++)
                        if (E[i]) ossz += problema.p[i];
                    return ossz;
                };

                var opt = new VisszalepesesOptimalizacio<bool>(n, M, R, ft, fk, josag);
                var megoldas = opt.OptimalisMegoldas();

                LepesSzam = opt.LepesSzam;
                optimalisMegoldas = megoldas;

                // az optimális értéket újraszámoljuk a megoldásból
                double ertek = 0;
                for (int i = 0; i < n; i++)
                    if (megoldas[i]) ertek += problema.p[i];
                optimalisErtek = ertek;

                return megoldas;
            }

            public double OptimalisErtek()
            {
                // ha még nem lett lefuttatva az optimalizálás, futtassuk le most
                if (optimalisMegoldas == null)
                    OptimalisMegoldas();
                return optimalisErtek;
            }
        }

        public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
        {
            protected Func<int, T[], float> fb;

            public SzetvalasztasEsKorlatozasOptimalizacio(
                int n,
                int[] m,
                T[,] r,
                Func<int, T, bool> ft,
                Func<int, T, T[], bool> fk,
                Func<T[], float> josag,
                Func<int, T[], float> fb)
                : base(n, m, r, ft, fk, josag)
            {
                this.fb = fb;
            }

            // Jegyzet 10.13 – bounding csak nem-levél szinten
            protected override void Backtrack(int szint, T[] E)
            {
                for (int i = 0; i < M[szint]; i++)
                {
                    LepesSzam++;

                    T v = R[szint, i];
                    if (!ft(szint, v)) continue;
                    if (!fk(szint, v, E)) continue;

                    E[szint] = v;

                    if (szint == n - 1)
                    {
                        float cur = josag(E);
                        if (!_van || cur > _bestValue)
                        {
                            Array.Copy(E, _best, n);
                            _bestValue = cur;
                        }
                        _van = true; // levélnél MINDIG igaz
                    }
                    else
                    {
                        float cur = josag(E);
                        float bound = fb(szint, E); // csak a hátralévőkre adott felső becslés
                        if (!_van || cur + bound > _bestValue)
                        {
                            Backtrack(szint + 1, E);
                        }
                    }
                }
            }
        }

        // 4. Szétválasztás és korlátozás – 0/1 hátizsák
        public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
        {
            public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema)
                : base(problema) { }

            public new bool[] OptimalisMegoldas()
            {
                int n = problema.n;

                // minden szinten két lehetőség: {felveszem, kihagyom}, ebben a sorrendben!
                int[] M = new int[n];
                bool[,] R = new bool[n, 2];
                for (int i = 0; i < n; i++)
                {
                    M[i] = 2;
                    R[i, 0] = true;   // először felvesszük
                    R[i, 1] = false;  // aztán kihagyjuk
                }

                Func<int, bool, bool> ft = (szint, v) => true;

                // részfeltétel: aktuális súly ≤ Wmax (csak a már döntött pozíciókig számolunk)
                Func<int, bool, bool[], bool> fk = (szint, v, E) =>
                {
                    double suly = 0.0;
                    for (int i = 0; i < szint; i++)
                        if (E[i]) suly += problema.w[i];
                    if (v) suly += problema.w[szint];
                    return suly <= problema.Wmax;
                };

                // eddig elért érték
                Func<bool[], float> josag = E =>
                {
                    double ertek = 0.0;
                    for (int i = 0; i < n; i++)
                        if (E[i]) ertek += problema.p[i];
                    return (float)ertek;
                };

                // felső becslés a JEGYZET szerinti lineáris "tört hátizsákkal" (NEM rendezünk!)
                Func<int, bool[], float> fb = (szint, E) =>
                {
                    // eddigi súly csak a már rögzített döntésekből (0..szint)
                    double suly = 0.0;
                    for (int i = 0; i <= szint; i++)
                        if (E[i]) suly += problema.w[i];

                    double marad = problema.Wmax - suly;
                    if (marad <= 0.0) return 0f;

                    double plusz = 0.0;

                    // a hátralévő tárgyakat j = szint+1..n-1 sorrendben próbáljuk kitölteni
                    for (int j = szint + 1; j < n && marad > 0.0; j++)
                    {
                        double wj = problema.w[j];
                        double pj = problema.p[j];

                        if (wj <= marad)
                        {
                            plusz += pj;
                            marad -= wj;
                        }
                        else
                        {
                            // TÖRT rész – ügyeljünk a lebegőpontos osztásra!
                            plusz += (pj / wj) * marad;
                            break;
                        }
                    }

                    return (float)plusz;
                };

                var opt = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(n, M, R, ft, fk, josag, fb);
                var megoldas = opt.OptimalisMegoldas();

                LepesSzam = opt.LepesSzam;

                // optimális érték számítása
                double best = 0.0;
                for (int i = 0; i < n; i++)
                    if (megoldas[i]) best += problema.p[i];

                _lastMegoldas = megoldas;
                _lastErtek = best;

                return megoldas;
            }

            private bool[] _lastMegoldas;
            private double _lastErtek;

            public new double OptimalisErtek()
            {
                if (_lastMegoldas == null)
                    OptimalisMegoldas();
                return _lastErtek;
            }
        }
}
