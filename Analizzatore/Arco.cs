
namespace Analizzatore {
    public class Arco {
        Nodo from;
        Nodo to;
        int peso=0;
        bool attivo=true;

        public Arco(Nodo nodo1, Nodo nodo2, int pesoArco) {
            from    = nodo1;
            to      = nodo2;
            peso    = pesoArco;
        }

        public int getPeso() {
            return peso;
        }

        public void disattivaArco() {
            attivo = false;
        }

        public void attivaArco() {
            attivo = true;
        }

        public bool getStatoArco() {
            return attivo;
        }

        public int getDestinazione() {
            return to.idNodo;
        }

        public int getSorgente() {
            return from.idNodo;
        }

        public int gradoDestinazione() {
            return to.grado;
        }

        public int gradoDestinazioneAdamic() {
            int contatore = 0;
            for (var i = 0; i < to.vicini.Count ; i++)
                if (to.vicini[i].getStatoArco())
                    contatore++;
            return contatore;
        }

        public Nodo ottieniDestinazione() {
            return to;
        }

        public Nodo ottieniSorgente() {
            return from;
        }
    }
    
}
