using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalhosterLib
{
    public class IISExpress
    {
        private Process p;
        private string configLocation = "";
        private string siteName = "";

        /// <summary>
        /// The given name for this instance. Used for reference purposes only.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Every line that is written to the console.
        /// </summary>
        public List<string> ProcessOutput { get; private set; } = new List<string>();

        /// <summary>
        /// Sets up the initial information for connecting to the correct site.
        /// </summary>
        /// <param name="config">The config file to load.</param>
        /// <param name="site">The specific website to load.</param>
        public IISExpress(string config, string site)
        {
            configLocation = config;
            siteName = site;
        }

        /// <summary>
        /// Starts the given instance of IIS Express with the specified site.
        /// </summary>
        public void Start()
        {
            p = new Process();
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.FileName = @"C:\Program Files (x86)\IIS Express\iisexpress.exe";
            p.StartInfo.WorkingDirectory = @"C:\Program Files (x86)\IIS Express\";
            p.StartInfo.Arguments = $"/config:{ configLocation } / site:{ siteName }";
            p.OutputDataReceived += OutputDataReceived;
            p.ErrorDataReceived += OutputDataReceived;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
        }

        /// <summary>
        /// Stops the running site (and IIS Express entirely if this is the last running site).
        /// </summary>
        public void Stop()
        {
            if (!p.HasExited)
            {
                p.Kill();
            }

            p.OutputDataReceived -= OutputDataReceived;
            p.ErrorDataReceived += OutputDataReceived;
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ProcessOutput.Add(e.Data);
        }
    }
}
