using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace MyApplic
{
    public class ServiceHelper
    {
        public static bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }

        public static void UnInstallService(string filepath, string servicename)
        {
            try
            {
                if (ServiceIsExisted(servicename))
                {
                    //UnInstall Service
                    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                    myAssemblyInstaller.UseNewContext = true;
                    myAssemblyInstaller.Path = filepath;
                    myAssemblyInstaller.Uninstall(null);
                    myAssemblyInstaller.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("unInstallServiceError\n" + ex.Message);
            }
        }

        public static void InstallService(string filepath, IDictionary stateSaver = null)
        {
            try
            {
                System.Configuration.Install.ManagedInstallerClass.InstallHelper(new[] { filepath });
                //System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(servicename);
                //if (!ServiceIsExisted(servicename))
                //{
                //    //Install Service
                //    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                //    myAssemblyInstaller.UseNewContext = true;
                //    myAssemblyInstaller.Path = filepath;

                //    if (stateSaver == null)
                //        stateSaver = new Hashtable();
                //    myAssemblyInstaller.Install(stateSaver);
                //    myAssemblyInstaller.Commit(stateSaver);
                //    myAssemblyInstaller.Dispose();
                //    //--Start Service
                //    service.Start();
                //}
                //else
                //{
                //    if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running &&
                //        service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                //    {
                //        service.Start();
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("installServiceError\n" + ex.Message);
            }
        }

        public static void StartService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status != System.ServiceProcess.ServiceControllerStatus.Running &&
                    service.Status != System.ServiceProcess.ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            throw new Exception(serviceName);
                        }
                    }
                }
            }
        }

        public static void StopService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            throw new Exception(serviceName);
                        }
                    }
                }
            }
        }

        public static bool Install()
        {
            try
            {
                LogHepler.Log("Service Install");
                ManagedInstallerClass.InstallHelper(new string[] { "/LogToConsole=false", Assembly.GetEntryAssembly().Location });
            }
            catch (Exception e)
            {
                LogHepler.Log(e.Message);
                return false;
            }
            return true;
        }

        public static bool Uninstall()
        {
            try
            {
                LogHepler.Log("Service Uninstall");
                ManagedInstallerClass.InstallHelper(new string[] { "/LogToConsole=false", "/u", Assembly.GetEntryAssembly().Location });
            }
            catch (Exception e)
            {
                LogHepler.Log(e.Message);
                return false;
            }
            return true;
        }

        public static bool CallInstallUtil(string[] installUtilArguments)
        {
            string InstallUtilPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Process proc = new Process();
            proc.StartInfo.FileName = Path.Combine(InstallUtilPath, "installutil.exe");
            proc.StartInfo.Arguments = String.Join(" ", installUtilArguments);
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            string outputResult = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                return false;
            }

            return true;
        }
    }
}
