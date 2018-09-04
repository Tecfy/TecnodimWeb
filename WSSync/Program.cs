using System.ServiceProcess;

namespace WSSync
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new Sync()
            };

            ServiceBase.Run(ServicesToRun);
        }
    }
}
