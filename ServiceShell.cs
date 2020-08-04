using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace MyApplic
{
    public partial class ServiceShell : System.ServiceProcess.ServiceBase
    {
        private EventHandler _start;
        private EventHandler _stop;

        public ServiceShell()
        {
            try
            {
                //Trace.WriteLine("Get service name");
                System.Reflection.MemberInfo info = GetType();
                var attribs = info.GetCustomAttributes(typeof(ServiceAttribute), true);
                //Trace.WriteLine("attribs: " + attribs.Length);
                for (int i = 0; i < attribs.Length; i++)
                {
                    ServiceName = ((ServiceAttribute)attribs[i]).ServiceName;
                    break;
                }

                Trace.WriteLine(ServiceName);

                CanStop = true;
                CanPauseAndContinue = false;
                CanShutdown = true;
                AutoLog = true;
                Trace.WriteLine("constructor done");
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw new Exception("isntall error!",ex);
            }
        }

        /// <summary>
        /// 获取一个服务实例，以便与ServiceBase.Run()一起使用
        /// </summary>
        /// <param name="start">Startup action</param>
        /// <param name="stop">Shutdown action</param>
        /// <returns></returns>
        public static void StartService<T>(EventHandler start = null, EventHandler stop = null, bool asConsole = false) where T : ServiceShell, new()
        {
            if (asConsole)
            {
                if (start != null)
                    start(null, null);

                Trace.WriteLine("Press [Enter] to close the service.");
                Console.ReadLine();

                if (stop != null)
                    stop(null, null);
            }
            else
                ServiceBase.Run(new T() { _start = start, _stop = stop });
        }

        protected override void OnStart(string[] args)
        {
            Trace.WriteLine("ServiceShell start");
            if (_start != null)
                _start(null, null);
            Trace.WriteLine("ServiceShell done");
        }

        protected override void OnStop()
        {
            Trace.WriteLine("ServiceShell stop");
            if (_stop != null)
                _stop(null, null);
            Trace.WriteLine("ServiceShell stop done");
        }

        /// <summary>
        /// 检查参数安装或卸载
        /// </summary>
        /// <param name="args">参数数组，例如from Main()</param>
        /// <returns>True if parameters included -i/-install or -u/-uninstall options.</returns>
        public static bool ProcessInstallOptions(string[] args)
        {
            var result = false;

            if ((args != null)
                 && (args.Length == 1)
                 && (args[0].Length > 1)
                 && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        Install();
                        result = true;
                        break;

                    case "uninstall":
                    case "u":
                        Uninstall();
                        result = true;
                        break;
                }
            }

            return result;
        }

        private static bool Install()
        {
            try
            {
                Trace.WriteLine("Service Install");
                ManagedInstallerClass.InstallHelper(new string[] { "/LogToConsole=true", Assembly.GetEntryAssembly().Location });
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                return false;
            }
            return true;
        }

        private static bool Uninstall()
        {
            try
            {
                Trace.WriteLine("Service Uninstall");
                ManagedInstallerClass.InstallHelper(new string[] { "/LogToConsole=true", "/u", Assembly.GetEntryAssembly().Location });
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
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
            //File.WriteAllText(@"c:\installutil_config.txt", proc.StartInfo.FileName + "\r\n" + proc.StartInfo.Arguments);
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();
            string outputResult = proc.StandardOutput.ReadToEnd();
            //File.WriteAllText(@"c:\setup_windows_service_info.txt", outputResult);
            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                return false;
            }

            return true;
        }

        public static bool CheckWindowsService(string servicename)
        {
            ServiceController[] Services = ServiceController.GetServices();
            for (int i = 0; i < Services.Length; i++)
            {
                if (Services[i].ServiceName == servicename)
                    return true;
            }
            return false;
        }

        public static bool Start_WindowsService(string servicename, bool start)
        {
            try
            {
                var service = new ServiceController(servicename);
                ServiceController[] Services = ServiceController.GetServices();
                for (int i = 0; i < Services.Length; i++)
                {
                    if (Services[i].ServiceName == servicename)
                        service = Services[i];
                }
                if (service != null && start)
                    service.Start(new[] { "" });
                else if (service != null && !start)
                    service.Stop();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
