using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Impianto
{
    // Definiamo il delegato per gli eventi di allarme
    public delegate void AllarmeEventHandler(string messaggio);

    public class MonitoraggioImpianto
    {
        // Soglie per la temperatura e le esalazioni
        private const double SOGLIA_TEMPERATURA = 75.0; // Gradi Celsius
        private const double SOGLIA_ESALAZIONI = 100.0; // Parti per milione (ppm)

        // Valori correnti dei sensori
        private double temperatura=0;
        private double esalazioni=0;

        // Conteggio allarmi
        private int conteggioAllarmiTemperatura = 0;
        private int conteggioAllarmiEsalazioni = 0;

        // Timer per generare dati in modo continuo
        private Timer timer;

        // Eventi di allarme
        public event AllarmeEventHandler AllarmeTemperatura;
        public event AllarmeEventHandler AllarmeEsalazioni;
        
        Random rnd = new Random();

        // Costruttore
        public MonitoraggioImpianto()
        {
            // Inizializza il timer per simulare i dati dei sensori ogni 2 secondi
            timer = new Timer(2000);
            timer.Elapsed += MonitoraSensori;
            timer.AutoReset = true;
        }

        // Metodo per avviare il monitoraggio
        public void AvviaMonitoraggio()
        {
            timer.Start();
            Console.WriteLine("Sistema di monitoraggio avviato.");
        }

        // Metodo per monitorare i valori dei sensori
        private void MonitoraSensori(object sender, ElapsedEventArgs e)
        {
            // Genera valori casuali per temperatura e esalazioni
            temperatura += Math.Round((rnd.NextDouble()*100),2); // Intervallo 50-100 °C
            esalazioni += Math.Round((rnd.NextDouble() * 150),2);  // Intervallo 50-150 ppm

            Console.WriteLine($"Temperatura attuale: {temperatura} °C");
            Console.WriteLine($"Esalazioni attuali: {esalazioni} ppm");

            // Controllo della soglia per la temperatura
            if (temperatura > SOGLIA_TEMPERATURA)
            {
                conteggioAllarmiTemperatura++;
                string messaggio = $"[ALLARME] Temperatura sopra la soglia! Valore: {Math.Round(temperatura,2)} °C (Soglia: {SOGLIA_TEMPERATURA} °C)\n" +
                                   $"Totale allarmi temperatura: {conteggioAllarmiTemperatura}\n";
                AllarmeTemperatura?.Invoke(messaggio);
                temperatura = 0;
            }

            // Controllo della soglia per le esalazioni
            if (esalazioni > SOGLIA_ESALAZIONI)
            {
                conteggioAllarmiEsalazioni++;
                string messaggio = $"[ALLARME] Esalazioni sopra la soglia! Valore: {Math.Round(esalazioni,2)} ppm (Soglia: {SOGLIA_ESALAZIONI} ppm)\n" +
                                   $"Totale allarmi esalazioni: {conteggioAllarmiEsalazioni}\n";
                AllarmeEsalazioni?.Invoke(messaggio);
                esalazioni = 0;
            }
        }

        // Metodo per generare un valore casuale tra un minimo e un massimo
      
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Crea un'istanza della classe di monitoraggio
            MonitoraggioImpianto monitor = new MonitoraggioImpianto();

            // Sottoscrive agli eventi di allarme con gestori
            monitor.AllarmeTemperatura += MessaggioAllarme;
            monitor.AllarmeEsalazioni += MessaggioAllarme;

            // Avvia il monitoraggio
            monitor.AvviaMonitoraggio();

            Console.WriteLine("Premere Invio per terminare il monitoraggio.");
            Console.ReadLine();
        }

        // Gestore degli eventi di allarme
        static void MessaggioAllarme(string messaggio)
        {
            Console.WriteLine(messaggio);
        }
    }
}
