using System.Collections.Generic;

namespace Analizzatore {

    public class Nodo{
        public int idNodo = 0;      // id identificativo del nodo
        public List<Arco> vicini;   // ogni nodo ha una lista di archi che lo connettono ai vicini
        public int grado = 0;       // numero di vicini, degree

        public Nodo(int idNodo) {
            this.idNodo = idNodo;
            vicini      = new List<Arco>();
        }


        public void aggiungiCollegamento(Arco arcoNuovo) {
            vicini.Add(arcoNuovo);
            grado++;
            return;
        }

        public bool arcoGiaPresente(int idDestinazione) { // controllo se già esiste l'arco da inserire
            for (int i = 0 ; i < vicini.Count ; i++) {
                if (vicini[i].getDestinazione() == idDestinazione)
                    return true;
            }
            return false;
        }

        public bool connessionePresente(Nodo nodoDestinazione) { // controllo se già esiste l'arco da inserire
            int idDestinazione = nodoDestinazione.idNodo;
            for(var i=0; i < vicini.Count; i++) {
                if (vicini[i].getDestinazione() == idDestinazione) {
                    if (vicini[i].getStatoArco())
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public void disattivaArcoSpecifico(int id) {
            for(var i = 0; i<grado; i++) {
                if(vicini[i].getDestinazione() == id) {
                    vicini[i].disattivaArco();
                    return;
                }
            }
        }



    }
}
