using System;
using System.Diagnostics;
using System.Net.Http;
using System.ServiceProcess;
using System.Timers;
using System.Web.Configuration;

namespace WSSync
{
    public partial class Sync : ServiceBase
    {
        Timer timer = new Timer();

        #region [ DI ]

        public Sync()
        {
            InitializeComponent();
        }

        #endregion

        private void RunSync()
        {
            string url = WebConfigurationManager.AppSettings["UrlBase"];
            string[] urls = WebConfigurationManager.AppSettings["RunUrls"].Split('|');
            System.Threading.Tasks.Task objTask = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                foreach (var item in urls)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string URL = string.Format("{0}/{1}", url, item);

                        var response = httpClient.GetAsync(URL).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            EventLog.WriteEntry(string.Format("Chamada da URL {0} data {1}, executada com sucesso.", item, DateTime.Now.ToShortTimeString()), EventLogEntryType.Information);
                        }
                        else
                        {
                            EventLog.WriteEntry(string.Format("Chamada da URL {0} data {1}, executada com falha. Erro: {2}", item, DateTime.Now.ToShortTimeString(), response.RequestMessage), EventLogEntryType.Information);                            
                        }
                    }
                }
            });
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            EventLog.WriteEntry("Executando chamada das URLs: " +
                DateTime.Now.ToShortTimeString(), EventLogEntryType.Information);

            RunSync();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Serviço Inicializado.", EventLogEntryType.Information);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            EventLog.WriteEntry("Serviço Parado.", EventLogEntryType.Information);
        }
    }
}
