
using System;
using System.Collections.Generic;

namespace Analizzatore {
    
    public class Grafo {                 // struttura del tipo G=<V,E>
        int conteggioNodi = 0;         // numero di nodi totali
        int conteggioArchi = 0;         // numero di archi totali
        public List<Nodo> nodi;         // contiene tutti i nodi del grafo      -> V
        public List<Arco> archi;        // contiene tutti gli archi del grafo   -> E

        public Grafo() {
            nodi    = new List<Nodo>();
            archi   = new List<Arco>();
        }

        public Nodo aggiungiNodo(int id) {
            bool trovato = false;
            var i = 0;
            for (i = 0; i < nodi.Count && trovato == false; i++) {
                if (nodi[i].idNodo == id) trovato = true;
            }
            if (trovato == false) {
                Nodo nuovoNodo = new Nodo(id);
                nodi.Add(nuovoNodo);
                conteggioNodi++;
                return nuovoNodo;
            } else {
                //System.Diagnostics.Debug.WriteLine("Viene richiesto:"+ id + " Ho ottenuto:"+ nodi[i - 1].idNodo);
                return nodi[i - 1];
            }
        }

        public Nodo aggiungiNodoFast(int id) {
            Nodo nuovoNodo = new Nodo(id);
            nodi.Add(nuovoNodo);
            conteggioNodi++;
            return nuovoNodo;
        }

        public Arco aggiungiArco(Nodo from, Nodo to, int peso) {
            Arco arco = new Arco(from, to, peso);
            archi.Add(arco);
            conteggioArchi++;
            return arco;
        }

        public int numeroNodi() {
            return conteggioNodi;
        }

        public int numeroArchi() {
            return conteggioArchi;
        }

        public double gradoMedio() {
            double avg = 0;
            for (int i = 0; i < conteggioNodi; i++) {
                avg += nodi[i].grado;
            }
            return avg / conteggioNodi;
        }

        public int gradoMinimo() {
            int min = Int32.MaxValue;
            for (int i = 0; i < conteggioNodi; i++) {
                if (min > nodi[i].grado) min = nodi[i].grado;
            }
            return min;
        }


        public int gradoMassimo() {
            int max = Int32.MinValue;
            for (int i = 0; i < conteggioNodi; i++) {
                if (max < nodi[i].grado) max = nodi[i].grado;
            }
            return max;
        }

        public double densita() {
            return 2*conteggioArchi/(conteggioNodi*(conteggioNodi-1));
            //return conteggioArchi/((conteggioNodi * (conteggioNodi - 1))/2);
        }
        /*
        public void mescolaArchi() {
            var count = archi.Count;
            var last = count - 1;
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < last; ++i) {
                var r = rnd.Next(0, last);
                var tmp = archi[i];
                archi[i] = archi[r];
                archi[r] = tmp;
            }
        }*/
    }
    
}
