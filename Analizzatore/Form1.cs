using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizzatore
{
public partial class Form1 : Form
{
        OpenFileDialog ofd;
        //List<string> listaNodi = new List<string>();
        Grafo grafo = new Grafo();
        Grafo grafoCompleto = new Grafo();
        int debug = -1;
        int percentualeDiTraining=10;
        int archiTolti = 0;
        List<Arco> archiTrainingSet = new List<Arco>();
        List<Arco> archiPredettiRandom = new List<Arco>();
        List<Arco> archiPredettiCN = new List<Arco>();
        List<Arco> archiPredettiAD = new List<Arco>();
        List<int> indiceOkCN = new List<int>();
        List<int> indiceOkAD = new List<int>();
        List<int> indiceOkRandom = new List<int>();
        bool giaDisattivati = false;
        int valoreTaglio=1;
        Object lockMe = new Object();

        public Form1() {
            InitializeComponent();
            ofd = new OpenFileDialog();
            textBox2.Text += "Software caricato correttamente.\r\n";
        }
        
        private void button1_Click(object sender, EventArgs e) {
            if (ofd.ShowDialog() == DialogResult.OK){
                textBox1.Text = ofd.FileName; // il solo nome del file è .SaveFileName
                textBox2.Text += "Hai selezionato il file "+ ofd.FileName+ "\r\n";
            }
        }
        private async void button2_Click(object sender, EventArgs e){
            if (button2.Enabled == false)
                        return;
            button2.Enabled = false;
            button6.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            archiTrainingSet = new List<Arco>();
            percentualeDiTraining = 0;
            archiTolti = 0;
            //listaNodi = new List<string>();
            await Task.Delay(100);
            var path = textBox1.Text.Trim();
            if (path == "") {
                MessageBox.Show("Devi prima selezionare un file", "Nessun file selezionato", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                button2.Enabled = true;
                return;
            }
            grafo = new Grafo();
            grafoCompleto = new Grafo();
            textBox2.Text += "Tento di leggere " + ofd.FileName + "\r\n";
            var t = await Task.Run(() => letturaElementi(path));
            textBox2.Text += t[1];
            if (t[0] == "0") {
                button2.Enabled = true;
                return;
            }
            textBox2.Text += "Il software è di nuovo IDLE\r\n";
            button2.Enabled = true;
            button6.Enabled = true;
            button5.Enabled = true;
            button7.Enabled = true;
            giaDisattivati = false;
        }
        
        private string[] letturaElementi(string path) {
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            var returnValue = new String[2];
            var testoRitorno = "";
            try {                 
                var separatore = ",";
                if (radioButton1.Checked) separatore = ",";
                else separatore = ";";
                using (var fs = File.OpenRead(path)) {
                    int numeroLinee = File.ReadAllLines(path).Count();
                    int lineeLette = 0;
                    int progresso = 0;
                    using (var reader = new StreamReader(fs)) {
                        reader.ReadLine(); // salto la prima linea
                        float percentuale = numeroLinee / 100;
                        if (checkBox2.Checked) {
                            while (!reader.EndOfStream) {
                                //AGGIUNTA 6/07/17
                                var line = reader.ReadLine();
                                var values = line.Split(separatore[0]);
                                //if (Int32.Parse(values[2]) > 0) {
                                if (true) {
                                    // FINE AGGIUNTA
                                    int sorgente = Int32.Parse(values[0]);
                                    int destinazione = Int32.Parse(values[1]);
                                    Nodo nodoAttuale = grafo.aggiungiNodo(sorgente); // aggiungo il nuovo nodo e lo ritorno
                                    if (!nodoAttuale.arcoGiaPresente(destinazione)) {
                                        Nodo nodoDestinazione = grafo.aggiungiNodo(destinazione);
                                        Arco arco1 = grafo.aggiungiArco(nodoAttuale, nodoDestinazione, 1);
                                        Arco arco2 = grafo.aggiungiArco(nodoDestinazione, nodoAttuale, 1);
                                        nodoAttuale.aggiungiCollegamento(arco1);
                                        nodoDestinazione.aggiungiCollegamento(arco2);
                                    } else {
                                        //System.Diagnostics.Debug.WriteLine("Skippo "+sorgente+" -> "+destinazione);
                                    }
                                    /*
                                    if (!listaNodi.Contains(values[0]))
                                        listaNodi.Add(values[0]);
                                    if (!listaNodi.Contains(values[1]))
                                        listaNodi.Add(values[1]);
                                    */                                    
                                }
                                lineeLette++;
                                if (lineeLette % percentuale == 0) {
                                    progresso++;
                                    if(progresso<100 && progresso > 0)
                                        progressBar1.Invoke((Action)(() => progressBar1.Value = progresso));
                                }
                            }
                        } else {
                            while (!reader.EndOfStream) {
                                //AGGIUNTA 6/07/17
                                var line = reader.ReadLine();
                                var values = line.Split(separatore[0]);
                                //if (Int32.Parse(values[2]) > 0) {
                                if (true) {
                                    // FINE AGGIUNTA
                                    int sorgente = Int32.Parse(values[0]);
                                    int destinazione = Int32.Parse(values[1]);
                                    Nodo nodoAttuale = grafo.aggiungiNodo(sorgente); // aggiungo il nuovo nodo e lo ritorno
                                    if (!nodoAttuale.arcoGiaPresente(destinazione)) {
                                        Nodo nodoDestinazione = grafo.aggiungiNodo(destinazione);
                                        Arco arco1 = grafo.aggiungiArco(nodoAttuale, nodoDestinazione, 1);
                                        nodoAttuale.aggiungiCollegamento(arco1);
                                    }
                                    /*
                                    if (!listaNodi.Contains(values[0]))
                                        listaNodi.Add(values[0]);
                                    if (!listaNodi.Contains(values[1]))
                                        listaNodi.Add(values[1]);
                                    */
                                }
                                lineeLette++;
                                if (lineeLette % percentuale == 0) {
                                    progresso++;
                                    if (progresso < 100 && progresso > 0)
                                        progressBar1.Invoke((Action)(() => progressBar1.Value = progresso));
                                }
                            }
                        }
                    }
                }
                testoRitorno += "Lette " + grafo.numeroNodi() + " righe \r\n";
                testoRitorno += "Lettura avvenuta con successo! \r\n";                
                progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
                returnValue[0] = "1";
            } catch(Exception e){
                testoRitorno += "Lettura dei dati fallita, sono separati da virgole ? \r\n";
                testoRitorno += e.Message+"\r\n";
                returnValue[0] = "0";
            }
            returnValue[1] = testoRitorno;
            return returnValue;
        }

        /*
        public string generazioneGrafoCompleto() {
            var testoRitorno = "";
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            //float totale = (listaNodi.Count * (listaNodi.Count - 1))/2;
            int elementiGenerati =0;
            //float progresso = 0;
            int numeroNodi = listaNodi.Count;
            try {
                for (var i = 0; i < listaNodi.Count; i++) {
                    grafoCompleto.aggiungiNodoFast(Int32.Parse(listaNodi[i]));
                }
                for (var i = 0; i < listaNodi.Count; i++) { // questo FOR può essere parallelizzato!!!
                    // questo FOR può essere parallelizzato!!!
                    // questo FOR può essere parallelizzato!!!
                    // questo FOR può essere parallelizzato!!!
                    // questo FOR può essere parallelizzato!!!
                    // questo FOR può essere parallelizzato!!!
                    Nodo nodoSorgente = grafoCompleto.nodi[i];
                    for (var j = i + 1; j < listaNodi.Count - 1; j++) {
                        Nodo nodoDestinazione = grafoCompleto.nodi[j];
                        Arco arco = grafoCompleto.aggiungiArco(nodoSorgente, nodoDestinazione, 1);
                        nodoSorgente.aggiungiCollegamento(arco);
                        Arco arco1 = grafoCompleto.aggiungiArco(nodoDestinazione, nodoSorgente, 1);
                        nodoDestinazione.aggiungiCollegamento(arco1);
                        elementiGenerati++;  
                    }
                    if (i % 100 == 0) {
                        //progressBar1.Invoke((Action)(() => progressBar1.Value = (int)(elementiGenerati*100/ totale)));
                        progressBar1.Invoke((Action)(() => progressBar1.Value = i*100/ numeroNodi));
                    }


                }
                testoRitorno += "Generazione grafo completo avvenuta con successo \r\n";
                testoRitorno += "Il grafo completo ha "+ elementiGenerati + " archi \r\n";
                progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
            } catch(Exception e) {
                testoRitorno += "Generazione grafo completo fallita \r\n";
                testoRitorno += e.Message+"\r\n";
            }
            return testoRitorno;
        }
        */
        
                
        // Stampa i collegamenti della struttura ricevuta come parametro
        private void stampaCollegamenti(Grafo grafo) {
            for (var j = 0; j < grafo.numeroNodi(); j++) {
            //for (var j = 0; j < 2; j++) {
                textBox2.Text += "Il nodo "+ grafo.nodi[j].idNodo +" risulta connesso a ";
                for (var h = 0; h < grafo.nodi[j].grado; h++)
                    textBox2.Text += grafo.nodi[j].vicini[h].getDestinazione() + ",";
                textBox2.Text += "\r\n";
            }
        }
        


        

        



        private async void button7_Click(object sender, EventArgs e) {
            // SE il training set si è generato correttamente allora attivo il bottone 4
            if (button7.Enabled == false)
                return;
            button7.Enabled = false;
            archiTrainingSet = new List<Arco>();
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            await Task.Delay(100);
            textBox2.Text += "Generazione Training Set...\r\n";
            textBox2.Text += "Mescolo l'insieme degli archi...\r\n";
            if (!await Task.Run(() => mescolaArchi())) {
                textBox2.Text += "Errore nel mescolamento degli archi, arresto ! \r\n";
                return;
            }
            textBox2.Text += "L'insieme degli archi è stato mescolato...\r\n";
            percentualeDiTraining = 10;
            try {
                percentualeDiTraining = Int32.Parse(textBox9.Text);
            }catch(Exception ex) {
                textBox2.Text += ex.Message+"\r\n";
                textBox2.Text += "La percentuale inserita non è un numero valido, viene usato il 10% \r\n";
            }
            if (!giaDisattivati) {
                if (!await Task.Run(() => disattivaArchi(percentualeDiTraining))) {
                    textBox2.Text += "Errore nella generazione del training set, errore fatale ! \r\n";
                    return;
                }
                textBox2.Text += "Archi rimossi correttamente per il training...\r\n";
                textBox2.Text += "Sono stati rimossi " + archiTolti + " archi \r\n";
                giaDisattivati = true;
            } else {
                textBox2.Text += "Riattivazione archi originali...\r\n";
                if (await Task.Run(() => attivaArchi())) {
                    textBox2.Text += "Disattivazione archi casuali...\r\n";
                    if (!await Task.Run(() => disattivaArchi(percentualeDiTraining))) {
                        textBox2.Text += "Errore nella generazione del training set, errore fatale ! \r\n";
                        return;
                    }
                    textBox2.Text += "Archi rimossi correttamente per il training...\r\n";
                    textBox2.Text += "Sono stati rimossi " + archiTolti + " archi \r\n";
                    giaDisattivati = true;
                }
            }
            textBox2.Text += "Training Set generato correttamente...\r\n";
            button7.Enabled = true;
            button4.Enabled = true;
            button11.Enabled = true;
        }

        private bool disattivaArchi(int percentuale) {
            try {
                int numeroArchiDaDisattivare = grafo.archi.Count - 1 ;
                numeroArchiDaDisattivare = numeroArchiDaDisattivare * percentuale / 100 ;
                archiTolti = numeroArchiDaDisattivare;
                if (debug == 1) System.Diagnostics.Debug.WriteLine("Disattiva Archi; numeroArchiDaDisattivare:"+ numeroArchiDaDisattivare);
                //Parallel.For(0, numeroArchiDaDisattivare, new ParallelOptions { MaxDegreeOfParallelism = 11 }, i => {
                for (int i = 0; i<numeroArchiDaDisattivare; i++) {
                    grafo.archi[i].disattivaArco();
                    grafo.archi[i].ottieniDestinazione().disattivaArcoSpecifico(grafo.archi[i].getSorgente()) ;
                    archiTrainingSet.Add(grafo.archi[i]);
                    //System.Diagnostics.Debug.WriteLine("Disattivo l'arco: "+ grafo.archi[i].getSorgente()+" -> "+ grafo.archi[i].getDestinazione()+" ; e l'arco "+grafo.archi[i].ottieniDestinazione().idNodo+" -> "+ grafo.archi[i].getSorgente());
                }//);
                if (debug == 1) System.Diagnostics.Debug.WriteLine("Rimossi:" + archiTrainingSet.Count);
            } catch {
                return false;
            }
            return true;
        }

        private bool attivaArchi() {
            try {
                for (int i = 0; i < grafo.archi.Count; i++) {
                    grafo.archi[i].attivaArco();
                }
            } catch {
                return false;
            }
            return true;
        }



        private bool mescolaArchi() {
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            try {
                var count = grafo.archi.Count;
                var last = count - 1;
                float percentuale = (count / 100)-1;
                short progresso = 0;
                var rnd = new Random(DateTime.Now.Millisecond);
                for (var i = 0; i < last; ++i) {
                    var r = rnd.Next(0, last);
                    Arco tmp = grafo.archi[i];
                    grafo.archi[i] = grafo.archi[r];
                    grafo.archi[r] = tmp;
                    if (i % percentuale == 0) {
                        progresso++;
                        if(progresso<100 && progresso > 0)
                            progressBar1.Invoke((Action)(() => progressBar1.Value = progresso));
                    }
                }
            } catch {
                return false;
            }
            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
            return true;
        }

        // Bottone calcola
        private async void button4_Click(object sender, EventArgs e) {
            if (button4.Enabled == false)
                return;
            button4.Enabled = false;
            button7.Enabled = false;
            button2.Enabled = false;
            await Task.Delay(100);
            try {
                valoreTaglio = Int32.Parse(textBox13.Text);
            }catch {
                valoreTaglio = 3;
            }


            if(radioButton3.Checked) { // se Adamic
                textBox2.Text += "Calcolo la formula di Adamic Adar...\r\n";
                textBox3.Text = "Running...";
                textBox16.Text = "Running...";
                button8.Enabled = false;
                indiceOkAD = null;
                indiceOkAD = new List<int>();
                if (await Task.Run(() => adamicAdar())) {
                    textBox2.Text += "Calcolo terminato con successo\r\n";
                    int contatore = 0;
                    if (debug == 1)
                        System.Diagnostics.Debug.WriteLine("Calcolo percentuale; archiPredetti.Count:" + archiPredettiRandom.Count +
                            ", archiTrainingSet.Count: " + archiTrainingSet.Count + ", archiTolti: " + archiTolti);
                    textBox2.Text += "Gli archi papabili sono: " + archiPredettiAD.Count + "\r\n";
                    textBox2.Text += "Conto gli archi predetti con successo..\r\n";
                    int conteggio = await Task.Run(() => {
                        int minimo = 0;
                        if (archiPredettiAD.Count > archiTrainingSet.Count) minimo = archiTrainingSet.Count;
                        else minimo = archiPredettiAD.Count;
                        Parallel.For(0, minimo, new ParallelOptions { MaxDegreeOfParallelism = 11 }, i => {
                            //for (var i = 0; i < archiPredettiRandom.Count && i < archiTrainingSet.Count && i < archiTolti; i++) {
                            for (var j = 0; j < archiTrainingSet.Count; j++) {
                                //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                if ((archiPredettiAD[i].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                    && archiPredettiAD[i].getSorgente() == archiTrainingSet[j].getSorgente()
                                    ) || (archiPredettiAD[i].getDestinazione() == archiTrainingSet[j].getSorgente()
                                    && archiPredettiAD[i].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                    lock (lockMe) {
                                        contatore++;
                                        indiceOkAD.Add(i);
                                    }
                                    if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                    break;
                                }
                            }
                        });
                        if (minimo+1 < archiPredettiAD.Count) {
                            if (archiPredettiAD[minimo].getPeso() == archiPredettiAD[minimo + 1].getPeso()) {
                                for (var h = minimo + 1; h < archiPredettiAD.Count; h++) {
                                    if (archiPredettiAD[h].getPeso() != archiPredettiAD[minimo].getPeso())
                                        break;
                                    Parallel.For(0, archiTrainingSet.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                        if ((archiPredettiAD[h].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                            && archiPredettiAD[h].getSorgente() == archiTrainingSet[j].getSorgente()
                                            ) || (archiPredettiAD[h].getDestinazione() == archiTrainingSet[j].getSorgente()
                                            && archiPredettiAD[h].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                            lock (lockMe) {
                                                contatore++;
                                                indiceOkAD.Add(h);
                                            }
                                            if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                            // break ??
                                        }
                                    });
                                }
                            }
                        }
                        return contatore;
                    });
                    if (archiTolti == 0) archiTolti = 1;
                    //double cont = contatore;
                    textBox3.Text = (Math.Truncate(100*((double)contatore * 100 / archiTolti))/100).ToString() + "%";
                    //textBox3.Text = "Completed!";
                    textBox16.Text = contatore.ToString();
                    button8.Enabled = true;
                } else {
                    textBox3.Text = "Errore";
                    textBox16.Text = "Errore";
                    textBox2.Text += "Errore nel calcolo\r\n";
                }
            }
            if(radioButton4.Checked) { // se commonNeig
                textBox2.Text += "Calcolo la formula dei Common Neighbor...\r\n";
                textBox8.Text = "Running...";
                textBox14.Text = "Running...";
                button9.Enabled = false;
                indiceOkCN = null;
                indiceOkCN = new List<int>();
                if (await Task.Run(() => commonNeighbour())) {
                    textBox2.Text += "Calcolo terminato con successo\r\n";
                    int contatore = 0;
                    if (debug == 1)
                        System.Diagnostics.Debug.WriteLine("Calcolo percentuale; archiPredetti.Count:"+ archiPredettiCN.Count+
                            ", archiTrainingSet.Count: "+archiTrainingSet.Count+ ", archiTolti: "+archiTolti);
                    textBox2.Text += "Gli archi papabili sono: " + archiPredettiCN.Count + "\r\n";
                    textBox2.Text += "Conto gli archi predetti con successo..\r\n";
                    int conteggio = await Task.Run(() => {
                        int minimo = 0;
                        if (archiPredettiCN.Count > archiTrainingSet.Count) minimo = archiTrainingSet.Count;
                        else minimo = archiPredettiCN.Count;
                        Parallel.For(0, minimo, new ParallelOptions { MaxDegreeOfParallelism = 11 }, i => {
                            //for (var i = 0; i < archiPredettiRandom.Count && i < archiTrainingSet.Count && i < archiTolti; i++) {
                            for (var j = 0; j < archiTrainingSet.Count; j++) {
                                //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                if ((archiPredettiCN[i].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                    && archiPredettiCN[i].getSorgente() == archiTrainingSet[j].getSorgente()
                                    ) || (archiPredettiCN[i].getDestinazione() == archiTrainingSet[j].getSorgente()
                                    && archiPredettiCN[i].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                    lock (lockMe) {
                                        contatore++;
                                        indiceOkCN.Add(i);
                                    }
                                    if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                    break;
                                }
                            }
                        });
                        if (minimo + 1 < archiPredettiCN.Count) {
                            if (archiPredettiCN[minimo].getPeso() == archiPredettiCN[minimo + 1].getPeso()) {
                                for (var h = minimo + 1; h < archiPredettiCN.Count; h++) {
                                    if (archiPredettiCN[h].getPeso() != archiPredettiCN[minimo].getPeso())
                                        break;
                                    Parallel.For(0, archiTrainingSet.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                        if ((archiPredettiCN[h].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                            && archiPredettiCN[h].getSorgente() == archiTrainingSet[j].getSorgente()
                                            ) || (archiPredettiCN[h].getDestinazione() == archiTrainingSet[j].getSorgente()
                                            && archiPredettiCN[h].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                            lock (lockMe) {
                                                contatore++;
                                                indiceOkCN.Add(h);
                                            }
                                            if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                            // break ??
                                        }
                                    });
                                }
                            }
                        }
                        return contatore;
                    });
                    if (archiTolti == 0) archiTolti = 1;
                    //textBox8.Text = (conteggio * 100/archiTolti).ToString()+"%";
                    textBox8.Text = (Math.Truncate(100 * ((double)conteggio * 100 / archiTolti)) / 100).ToString() + "%";
                    //textBox8.Text = "Completed!";
                    textBox14.Text = conteggio.ToString();
                    button9.Enabled = true;
                } else {
                    textBox8.Text = "Errore";
                    textBox14.Text = "Errore";
                    textBox2.Text += "Errore nel calcolo\r\n";
                }
            }
            if (radioButton5.Checked) { // se random
                textBox2.Text += "Calcolo la formula dei pesi randomizzati...\r\n";
                textBox7.Text = "Running...";
                textBox15.Text = "Running...";
                button10.Enabled = false;
                indiceOkRandom = null;
                indiceOkRandom = new List<int>();
                if (await Task.Run(() => pesiRandomici())) {
                    textBox2.Text += "Calcolo terminato con successo\r\n";
                    int contatore = 0;
                    if (debug == 1)
                        System.Diagnostics.Debug.WriteLine("Calcolo percentuale; archiPredetti.Count:" + archiPredettiRandom.Count +
                            ", archiTrainingSet.Count: " + archiTrainingSet.Count + ", archiTolti: " + archiTolti);
                    textBox2.Text += "Gli archi papabili sono: "+ archiPredettiRandom.Count + "\r\n";
                    textBox2.Text += "Conto gli archi predetti con successo..\r\n";
                    int conteggio = await Task.Run(() => {
                        int minimo=0;
                        if (archiPredettiRandom.Count > archiTrainingSet.Count) minimo = archiTrainingSet.Count;
                        else minimo = archiPredettiRandom.Count;
                        Parallel.For(0, minimo, new ParallelOptions { MaxDegreeOfParallelism = 11 }, i => {
                            //for (var i = 0; i < archiPredettiRandom.Count && i < archiTrainingSet.Count && i < archiTolti; i++) {
                            for (var j = 0; j < archiTrainingSet.Count; j++) {
                                //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                if ((archiPredettiRandom[i].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                    && archiPredettiRandom[i].getSorgente() == archiTrainingSet[j].getSorgente()
                                    ) || (archiPredettiRandom[i].getDestinazione() == archiTrainingSet[j].getSorgente()
                                    && archiPredettiRandom[i].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                    lock (lockMe) {
                                        contatore++;
                                        indiceOkRandom.Add(i);
                                    }
                                    if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                    break;
                                }
                            }
                        });
                        /*
                        if (minimo + 1 < archiPredettiRandom.Count) {
                            if (archiPredettiRandom[minimo].getPeso() == archiPredettiRandom[minimo + 1].getPeso()) {
                                for (var h = minimo + 1; h < archiPredettiRandom.Count; h++) {
                                    if (archiPredettiRandom[h].getPeso() != archiPredettiRandom[minimo].getPeso())
                                        break;
                                    Parallel.For(0, archiTrainingSet.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Testo un nuovo arco");
                                        if ((archiPredettiRandom[h].getDestinazione() == archiTrainingSet[j].getDestinazione()
                                            && archiPredettiRandom[h].getSorgente() == archiTrainingSet[j].getSorgente()
                                            ) || (archiPredettiRandom[h].getDestinazione() == archiTrainingSet[j].getSorgente()
                                            && archiPredettiRandom[h].getSorgente() == archiTrainingSet[j].getDestinazione())) {
                                            lock (lockMe) {
                                                contatore++;
                                            }
                                            if (debug == 1) System.Diagnostics.Debug.WriteLine("Calcolo percentuale; Trovato un nuovo arco, contatore: " + contatore);
                                            // break ??
                                        }
                                    });
                                }
                            }
                        }
                        */
                        return contatore;
                    });                    
                    if (archiTolti == 0) archiTolti = 1;
                    //textBox7.Text = (conteggio * 100 / archiTolti).ToString() + "%";
                    textBox7.Text = (Math.Truncate(100 * ((double)conteggio * 100 / archiTolti)) / 100).ToString() + "%";
                    //textBox7.Text = "Completed!";
                    textBox15.Text = conteggio.ToString();
                    button10.Enabled = true;
                } else {
                    textBox7.Text = "Errore";
                    textBox15.Text = "Errore";
                    textBox2.Text += "Errore nel calcolo\r\n";
                }
            }
            
            /*for (var i = 0; i < archiPredetti.Count; i++) {
                textBox2.Text += "Arco da "+ archiPredetti[i].getSorgente()+" , "+ archiPredetti[i].getDestinazione()+" , peso: "+ archiPredetti[i].getPeso()+"\r\n";
            }*/
            textBox2.Text += "Il software è di nuovo IDLE\r\n";
            button4.Enabled = true;
            button7.Enabled = true;
            button2.Enabled = true;
        }

        private bool pesiRandomici() {
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            archiPredettiRandom = new List<Arco>();
            int numeroNodi = grafo.nodi.Count;
            int valoreProgressBar = 0;
            if (debug == 1) System.Diagnostics.Debug.WriteLine("Pesi Randomici; numeroNodi:" + numeroNodi);
            //try {
            for (int i = 0; i < grafo.nodi.Count; i++) {
                Parallel.For(i + 1, grafo.nodi.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                    //for (var j = i + 1; j < grafo.nodi.Count; j++) {
                    if (!grafo.nodi[i].connessionePresente(grafo.nodi[j])) { // se NON sono già connessi
                        var rnd = new Random(DateTime.Now.Millisecond);
                        var r = rnd.Next(0, 99);
                        if (r > valoreTaglio) {
                            Arco arco = new Arco(grafo.nodi[i], grafo.nodi[j], r);
                            lock (lockMe) {
                                archiPredettiRandom.Add(arco); // qui l'errore
                            }
                        }
                    }
                });
                if (i % 100 == 0) {
                    valoreProgressBar = i * 100 / numeroNodi;
                    if(valoreProgressBar>0 && valoreProgressBar<100)
                        progressBar1.Invoke((Action)(() => progressBar1.Value = valoreProgressBar));
                }
            }
            ordinaLista(archiPredettiRandom,3);
            /* } catch{
                 return false;
             }*/
            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
            return true;

        }
        
        private bool commonNeighbour() {
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            archiPredettiCN = new List<Arco>();
            int numeroNodi = grafo.nodi.Count;
            int valoreProgressBar = 0;
            if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; numeroNodi:" + numeroNodi);
            //try {
            for (int i = 0; i < grafo.nodi.Count; i++) {
                //for (var j = i + 1; j < grafo.nodi.Count; j++) {
                Parallel.For(i + 1, grafo.nodi.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                    int contatore = 0;
                    // per ogni coppia conto i vicini in comune
                    // in questo punto mi ritrovo la coppia di nodi (i,j)
                    // conto il numero di vicini in comune |vicini(i) intersecato vicini(j)|
                    if (!grafo.nodi[i].connessionePresente(grafo.nodi[j])) { // se NON sono già connessi
                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Non connessi:" + grafo.nodi[i].idNodo+" , "+ grafo.nodi[j].idNodo);
                        for (var a = 0; a < grafo.nodi[i].grado; a++) {
                            if (grafo.nodi[i].vicini[a].getStatoArco()) { // se l'arco verso il vicino di i è attivo
                                for (var b = 0; b < grafo.nodi[j].grado; b++) {
                                    if (grafo.nodi[i].vicini[a].getDestinazione() == grafo.nodi[j].vicini[b].getDestinazione() 
                                        && grafo.nodi[j].vicini[b].getStatoArco()) {
                                        // Se trovo dei vicini in comune e il loro arco non è stato disattivato
                                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Contatore aumenta:" + grafo.nodi[i].idNodo + " , " + grafo.nodi[j].idNodo);
                                        contatore++;
                                    }
                                }
                            }
                        }
                    }
                    if (contatore >= valoreTaglio && contatore > 0) {
                        Arco arco = new Arco(grafo.nodi[i], grafo.nodi[j], contatore);
                        lock (lockMe) {
                            archiPredettiCN.Add(arco);
                        }
                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Non connessi; Aggiungo arco predetto:" + grafo.nodi[i].idNodo + " , " + grafo.nodi[j].idNodo + " -> contatore: "+contatore);
                    }
                });
                if (i % 100 == 0) {
                    valoreProgressBar = i * 100 / numeroNodi;
                    if (valoreProgressBar > 0 && valoreProgressBar < 100)
                        progressBar1.Invoke((Action)(() => progressBar1.Value = valoreProgressBar));
                }
            }
            ordinaLista(archiPredettiCN,2);
            /* } catch{
                 return false;
             }*/
            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
            return true;
        }

        private bool adamicAdar() {
            progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
            archiPredettiAD = new List<Arco>();
            int valoreProgressBar = 0;
            int numeroNodi = grafo.nodi.Count;
            if (debug == 1) System.Diagnostics.Debug.WriteLine("AdamicAdar; numeroNodi:" + numeroNodi);
            //try {
            for (int i = 0; i < grafo.nodi.Count; i++) {
                //for (var j = i + 1; j < grafo.nodi.Count; j++) {
                Parallel.For(i + 1, grafo.nodi.Count, new ParallelOptions { MaxDegreeOfParallelism = 11 }, j => {
                    double contatore = 0;
                    // per ogni coppia conto i vicini in comune
                    // in questo punto mi ritrovo la coppia di nodi (i,j)
                    // conto il numero di vicini in comune |vicini(i) intersecato vicini(j)|
                    if (!grafo.nodi[i].connessionePresente(grafo.nodi[j])) { // se NON sono già connessi
                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Non connessi:" + grafo.nodi[i].idNodo+" , "+ grafo.nodi[j].idNodo);
                        for (var a = 0; a < grafo.nodi[i].grado; a++) { // per ogni vicino di i
                            if (grafo.nodi[i].vicini[a].getStatoArco()) { // il cui arco è attivo
                                for (var b = 0; b < grafo.nodi[j].grado; b++) { // per ogni vicini di j
                                    if (grafo.nodi[i].vicini[a].getDestinazione() == grafo.nodi[j].vicini[b].getDestinazione()
                                        && grafo.nodi[j].vicini[b].getStatoArco()) {
                                        // Se trovo dei vicini in comune e il loro arco non è stato disattivato
                                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Contatore aumenta:" + grafo.nodi[i].idNodo + " , " + grafo.nodi[j].idNodo);
                                        double valoreGradoDestinazione = (double)grafo.nodi[i].vicini[a].gradoDestinazioneAdamic();
                                        if(valoreGradoDestinazione>0)
                                            contatore = contatore + (1 / Math.Log(valoreGradoDestinazione));
                                    }
                                }
                            }
                        }
                    }
                    int valore = (int)(contatore * 1000);
                    //if(valore > 0) System.Diagnostics.Debug.WriteLine("commonNeighbour; Contatore aumenta:" +valore);
                    if (valore >= valoreTaglio && valore > 0) {
                        Arco arco = new Arco(grafo.nodi[i], grafo.nodi[j], valore);
                        lock (lockMe) {
                            archiPredettiAD.Add(arco);
                        }
                        //if (debug == 1) System.Diagnostics.Debug.WriteLine("commonNeighbour; Non connessi; Aggiungo arco predetto:" + grafo.nodi[i].idNodo + " , " + grafo.nodi[j].idNodo + " -> contatore: "+contatore);
                    }
                });
                if (i % 100 == 0) {
                    valoreProgressBar = i * 100 / numeroNodi;
                    if (valoreProgressBar > 0 && valoreProgressBar < 100)
                        progressBar1.Invoke((Action)(() => progressBar1.Value = valoreProgressBar));
                }
            }
            ordinaLista(archiPredettiAD,1);
            /* } catch{
                 return false;
             }*/
            progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
            return true;
        }

        // Ordina gli archi predetti
        private void ordinaLista(List<Arco> lista, int selettore) {
            if (debug == 1)
                System.Diagnostics.Debug.WriteLine("Ordino lista archi predetti: " + lista.Count +"<<<<<<<<<<<<<<<<<<<<<<");
            //archiPredetti.Sort((x, y) => x.getPeso().CompareTo(y.getPeso()));
            try {
                lista = lista.OrderBy(x => x.getPeso()).ToList();
            }catch {
                //if (debug == 1)
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>Crash Ordinamento");
            }
            lista.Reverse();
            if (selettore == 1)
                archiPredettiAD = lista;
            if (selettore == 2)
                archiPredettiCN = lista;
            if (selettore == 3)
                archiPredettiRandom = lista;
            /*
            archiPredetti.Sort(
            delegate (Arco p1, Arco p2) {
                if (p1==null || p2 ==null ) return 0;
                else {
                    if (p1.getPeso() >= p2.getPeso()) return -1;
                    else return 1;
                }
                //return p2.getPeso()-p1.getPeso();
            });
            */
            //stampaArchiPredetti(lista);
        }

        private void stampaArchiPredetti(List<Arco> lista) {
            for (var i = 0; i < lista.Count; i++)
                System.Diagnostics.Debug.WriteLine("Predetto: " + lista[i].getSorgente() + " -> " + lista[i].getDestinazione() + ", peso: " + lista[i].getPeso());
        }

        



        // Bottone Stampa dei collegamenti 
        private void button5_Click(object sender, EventArgs e) {
            stampaCollegamenti(grafo);
        }


        // Bottone calcolo valori secondari
        private async void button6_Click(object sender, EventArgs e) {
            if (button6.Enabled == false)
                return;
            button6.Enabled = false;
            textBox4.Text = "...";
            textBox5.Text = "...";
            textBox6.Text = "...";
            textBox10.Text = "...";
            textBox11.Text = "...";
            await Task.Delay(100);
            try {
                textBox2.Text += "Calcolo il numero di nodi totali..\r\n";
                textBox4.Text = await Task.Run(() => grafo.numeroNodi().ToString());
                textBox2.Text += "Calcolo il numero di archi totali..\r\n";
                textBox5.Text = await Task.Run(() => grafo.numeroArchi().ToString());
                textBox2.Text += "Calcolo il grado medio..\r\n";
                textBox6.Text = await Task.Run(() => grafo.gradoMedio().ToString());
                textBox2.Text += "Calcolo il grado minimo..\r\n";
                textBox10.Text = await Task.Run(() => grafo.gradoMinimo().ToString());
                textBox2.Text += "Calcolo il grado massimo..\r\n";
                textBox11.Text = await Task.Run(() => grafo.gradoMassimo().ToString());
                textBox2.Text += "Calcolo la densità del grafo..\r\n";
                textBox12.Text = await Task.Run(() => grafo.densita().ToString());
            }catch(Exception ex) {
                textBox2.Text += "Errore:"+ex.Message+"\r\n";
            }
            button6.Enabled = true;
        }

        // Funzione che stampa gli elementi nelle label
        private void stampaElementi(List<string> l1, List<string> l2) {
            for (var i = 0; i < l1.Count; i++) {
                textBox2.Text += l1[i] + " , " + l2[i] + " \r\n";
            }
        }




































        private void textBox2_TextChanged_1(object sender, EventArgs e) {
            textBox2.SelectionStart = textBox2.TextLength;
            textBox2.ScrollToCaret();
        }


        private void textBox10_TextChanged(object sender, EventArgs e) {

        }

        private void textBox11_TextChanged(object sender, EventArgs e) {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) {
            if (radioButton4.Checked)
                textBox13.Text = "0";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e) {
            if (radioButton5.Checked)
                textBox13.Text = "50";
        }
        private void radioButton3_CheckedChanged_1(object sender, EventArgs e) {
            if (radioButton3.Checked)
                textBox13.Text = "20";
        }

        private void textBox8_TextChanged(object sender, EventArgs e) {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) {

        }

        private void textBox7_TextChanged(object sender, EventArgs e) {

        }

        private void textBox12_TextChanged(object sender, EventArgs e) {

        }


        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (debug == -1) {
                textBox2.Text += "Attivata la modalità di debug.\r\n";
                debug = 1;
            }else {
                textBox2.Text += "Disattivata la modalità di debug.\r\n";
                debug = -1;
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            textBox2.Text = "";
        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {

        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {

        }

        

        private void progressBar1_Click(object sender, EventArgs e) {

        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) {

        }

        private void textBox5_TextChanged(object sender, EventArgs e) {

        }

        private void textBox6_TextChanged(object sender, EventArgs e) {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {

        }

        private void textBox9_TextChanged(object sender, EventArgs e) {

        }

        private void textBox13_TextChanged(object sender, EventArgs e) {

        }

        private void textBox14_TextChanged(object sender, EventArgs e) {

        }

        private void textBox15_TextChanged(object sender, EventArgs e) {

        }

        private void textBox16_TextChanged(object sender, EventArgs e) {

        }

        private void button8_Click(object sender, EventArgs e) {
            //salvataggio Adamic
            salvataggioDati(1);
        }

        private void button9_Click(object sender, EventArgs e) {
            //salvataggio CN
            salvataggioDati(2);
        }

        private void button10_Click(object sender, EventArgs e) {
            //salvataggio Random
            salvataggioDati(3);
        }

        private async void salvataggioDati(int selettore) {
            List<Arco> riferimentoLista;
            List<int> archiCorretti=null;
            int max = Int32.MaxValue;
            var soloCorretti = false;
            if (selettore == 1) {
                riferimentoLista = archiPredettiAD;
                archiCorretti = indiceOkAD;
                if (MessageBox.Show("Premi si per salvare solo gli archi che hanno contribuito al calcolo della percentuale, premi no per salvare tutti gli archi generati come possibili.", "Salvare solo gli archi corretti?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    try {
                        max = Int32.Parse(textBox16.Text);
                    } catch (Exception e) {
                        textBox2.Text += e.Message + "\r\n";
                        textBox2.Text += "Errore nella lettura dei dati calcolati correttamente, vengono salvati tutti gli archi generati.\r\n";
                    }
                }
            } else if (selettore == 2) {
                riferimentoLista = archiPredettiCN;
                archiCorretti = indiceOkCN;
                if (MessageBox.Show("Premi si per salvare solo gli archi che hanno contribuito al calcolo della percentuale, premi no per salvare tutti gli archi generati come possibili.", "Salvare solo gli archi corretti?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    try {
                        max = Int32.Parse(textBox14.Text);
                    } catch (Exception e) {
                        textBox2.Text += e.Message + "\r\n";
                        textBox2.Text += "Errore nella lettura dei dati calcolati correttamente, vengono salvati tutti gli archi generati.\r\n";
                    }
                }
            } else if (selettore == 3) {
                riferimentoLista = archiPredettiRandom;
                archiCorretti = indiceOkRandom;
                if (MessageBox.Show("Premi si per salvare solo gli archi che hanno contribuito al calcolo della percentuale, premi no per salvare tutti gli archi generati come possibili.", "Salvare solo gli archi corretti?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    try {
                        max = Int32.Parse(textBox15.Text);
                        soloCorretti = true;
                    } catch (Exception e) {
                        textBox2.Text += e.Message + "\r\n";
                        textBox2.Text += "Errore nella lettura dei dati calcolati correttamente, vengono salvati tutti gli archi generati.\r\n";
                    }
                }
            } else { // qui se salvo il training set
                riferimentoLista = archiTrainingSet;
                archiCorretti = indiceOkRandom;
                if (MessageBox.Show("Verranno salvati gli archi rimossi dal grafo originale e che si tenta di predire", "Procedere ?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    try {
                        max = archiTrainingSet.Count;
                        soloCorretti = false; // forzo il programma ad andare nel ramo else poco sotto, in questo modo non considero l'array di interi "soloCorretti"
                    } catch (Exception e) {
                        textBox2.Text += e.Message + "\r\n";
                        textBox2.Text += "Errore nella lettura dei dati calcolati correttamente, vengono salvati tutti gli archi generati.\r\n";
                    }
                }else {
                    return;
                }
            }
            var file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK) {
                var path = file.FileName;
                bool t = await Task.Run(() => {
                    try {
                        int percentuale=1;
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 0));
                        var text = "";
                        System.IO.File.WriteAllText(path, text);
                        int massimoValoreSalvabile = riferimentoLista.Count;
                        if (max < riferimentoLista.Count)
                            massimoValoreSalvabile = max;
                        if (massimoValoreSalvabile > 0)
                            text += "Source;Target;Weight\n";
                        if (soloCorretti == true) {
                            for (var i = 0; i < massimoValoreSalvabile && i < archiCorretti.Count; i++) {
                                text += riferimentoLista[archiCorretti[i]].getSorgente() + ";" + riferimentoLista[archiCorretti[i]].getDestinazione() + ";" + riferimentoLista[archiCorretti[i]].getPeso() + "\n";
                                if (i % 200 == 0) {
                                    System.IO.File.AppendAllText(path, text);
                                    text = "";
                                    percentuale = i * 100 / archiCorretti.Count;
                                    if (percentuale > 0 && percentuale < 100)
                                        progressBar1.Invoke((Action)(() => progressBar1.Value = i * 100 / archiCorretti.Count));
                                }
                            }
                        } else {
                            for (var i = 0; i < massimoValoreSalvabile; i++) {
                                text += riferimentoLista[i].getSorgente() + ";" + riferimentoLista[i].getDestinazione() + ";" + riferimentoLista[i].getPeso() + "\n";
                                if (i % 200 == 0) {
                                    System.IO.File.AppendAllText(path, text);
                                    text = "";
                                    percentuale = i * 100 / massimoValoreSalvabile;
                                    if (percentuale > 0 && percentuale < 100)
                                        progressBar1.Invoke((Action)(() => progressBar1.Value = i * 100 / massimoValoreSalvabile));
                                }
                            }
                        }
                        System.IO.File.AppendAllText(path, text);
                        progressBar1.Invoke((Action)(() => progressBar1.Value = 100));
                        //System.IO.File.WriteAllText(path, text);
                    } catch {
                        return false;
                    }
                    return true;
                });
                if (t)
                    textBox2.Text += "Scrittura avvenuta correttamente!\r\n";
                else
                    textBox2.Text += "Scrittura fallita \r\n";
            }

            textBox2.Text += "Il software è di nuovo IDLE\r\n";
        }

        private void button11_Click(object sender, EventArgs e) {
            salvataggioDati(4);
        }

        private void button12_Click(object sender, EventArgs e) {
            if (checkBox2.Checked) {
                textBox2.Text += "La casella è selezionata\r\n";
            }else {

                textBox2.Text += "La casella NON è selezionata\r\n";
            }
        }
    }
}
