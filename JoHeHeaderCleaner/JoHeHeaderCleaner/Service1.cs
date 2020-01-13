﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;


namespace JoHeHeaderCleaner
{
    public partial class Service1 : ServiceBase
    {


        
        private  Param param;

        public FileSystemWatcher _filesystemwatcher;

        public string configfile;

        public String monitorFolder;

        private static List<String> searchHeaders;



        public Service1()
        {
            InitializeComponent();

            _filesystemwatcher = new FileSystemWatcher();

            
            searchHeaders = new List<string>();

        }

        protected override void OnStart(string[] args)
        {
            this.param = new Param();
            

            Console.WriteLine("Test");

            writeLog("Programm gestartet...");

            configfile = AppDomain.CurrentDomain.BaseDirectory + "config.cfg";
            writeLog("Lese Konfigurationsdatei: " + configfile);
            readConfig(configfile);




            _filesystemwatcher.Path = monitorFolder;
            
            _filesystemwatcher.Created += FileSystemWatcher;
            _filesystemwatcher.Renamed += FileSystemWatcher;

            _filesystemwatcher.EnableRaisingEvents = true;

        }

        protected override void OnStop()
        {
            writeLog("Programm beendet...");
        }

                

        private static void FileSystemWatcher(object sender, FileSystemEventArgs e)
        {
            writeLog("Neue Datei gefunden: " + e.Name);

            string extension = Path.GetExtension(e.Name);
            if (extension == ".csv")
            {
                writeLog("Ist eine CSV... Analysiere");

                string line1 = File.ReadLines(e.FullPath).First(); // gets the first line from file.

                bool found = false;

                // Vergleiche mit Einträgen in Header-Suchliste
                for (int i = 0; i < searchHeaders.Count; i++)
                {

                    if (line1 == searchHeaders[i])
                    {
                        found = true;
                        writeLog("Header gefunden. Lösche erste Zeile: " + line1);
                        File.WriteAllLines(e.FullPath, File.ReadAllLines(e.FullPath).Skip(1));
                    }
                }

                if (!found){
                    writeLog("Header nicht gefunden. Nicht bearbeiten...");
                }
            }

        }


        private void readConfig(String _cfgFile)
        {
            int counter = 0;
            string line;

            System.IO.StreamReader file =
                    new System.IO.StreamReader(_cfgFile);

            

            try
            {
                // Read the file and display it line by line.  

              

                while ((line = file.ReadLine()) != null)
                {
                    // Zu überwachende Ordner
                    if (line.Contains("MonitorFolder="))
                    {
                        String[] _folder = line.Split('=');
                        monitorFolder = _folder[1];
                        writeLog("MonitorFolder aufgenommen: " + _folder[1]);
                    }

                    // Zu suchende Header
                    if (line.Contains("Header="))
                    {
                        String[] _folder = line.Split('=');
                        searchHeaders.Add(_folder[1]);
                        writeLog("Header aufgenommen: " + _folder[1]);
                    }


                    counter++;
                }
            }catch (Exception e)
            {
                writeLog("ERROR: Konfig Datei konnte nicht gelesen werden: " + e.Message);
                writeLog("DEBUG: " + e.StackTrace);
            }
            

        }

        public static void writeLog(String _message)
        {
            StreamWriter sw = null;

            // try
            // {
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + ": " + _message);
            sw.Close();
            //  }
            // catch { }

        }


    }
}
