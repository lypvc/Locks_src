using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyApplic
{
    public class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            //var keys = GenerateKeys();
            //var csp = new CspParameters();
            //csp.KeyNumber = 1024;
            //csp.ProviderName = "mycspparameters1024";
            //csp.KeyContainerName = "259E5D92-8E46-4128-BD19-6847DA0D3385.";
            //GenerateKeys(csp);
            List<string> _args = new List<string>();
            _args.Add("-i");
            _args.Add(Application.ExecutablePath);
            using (Runner r = new Runner())
                Service.CallInstallUtil(_args.ToArray());
            //Service.StartService<Service>((o, e) => r.Start(), (o, e) => r.Stop(), false);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Service.Start_WindowsService("filelock", true);
        }

        static string[] GenerateKeys(CspParameters cspParameters = null)
        {
            string[] sKeys = new String[2];
            var rsa = new RSACryptoServiceProvider();
            if (cspParameters != null)
                rsa = new RSACryptoServiceProvider(cspParameters);
            sKeys[0] = rsa.ToXmlString(true);
            sKeys[1] = rsa.ToXmlString(false);
            return sKeys;
        }
    }

    [DesignerCategory("")]
    [Service("filelock")]
    public class Service : ServiceShell { }

    [RunInstaller(true)]
    [Service("filelock", DisplayName = "filelock", Description = "1111aaaa")]
    public class Installer : ServiceInstallerShell { }

    public class Runner : IDisposable
    {
        private Thread thread;

        public void MyThread()
        {
            LockHelper lockHelper = new LockHelper();
            lockHelper.Main();
        }

        public void Dispose()
        {
            Stop();
        }
        public void Start()
        {

        }

        public void Stop()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
