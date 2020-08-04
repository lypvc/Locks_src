using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace MyApplic
{
    [RunInstaller(true)]
    public partial class ServiceInstallerShell : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public ServiceInstallerShell()
        {
            try
            {
                // InitializeComponent();
                this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
                this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();

                this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
                this.serviceProcessInstaller.Password = null;
                this.serviceProcessInstaller.Username = null;

                System.Reflection.MemberInfo info = GetType();
                var cus = info.GetCustomAttributes(typeof(ServiceAttribute), true);
                for (int i = 0; i < cus.Length; i++)
                {
                    var attribs = (ServiceAttribute)cus[i];
                    this.serviceInstaller.Description = attribs.Description;
                    this.serviceInstaller.DisplayName = attribs.DisplayName;
                    this.serviceInstaller.ServiceName = attribs.ServiceName;
                    break;
                }
                this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
                this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                    this.serviceProcessInstaller,
                    this.serviceInstaller});
            }
            catch (Exception ex)
            {
                throw new Exception("my ServiceInstallerShell error!", ex);
            }
        }
    }
}
