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
            List<string> _args = new List<string>();
            _args.Add("-i");
            _args.Add(Application.ExecutablePath);
            Service.CallInstallUtil(_args.ToArray());
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

}
