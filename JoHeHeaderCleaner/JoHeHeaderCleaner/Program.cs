﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace JoHeHeaderCleaner
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new JoHeHeaderCleaner()
            };
            ServiceBase.Run(ServicesToRun);
        }



    }


}
