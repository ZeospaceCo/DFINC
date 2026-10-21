using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;


namespace Zeospace.DeskPlus.Component.WebComponent
{
    /// <summary>
    /// ZipFile에 대한 요약 설명입니다.
    /// </summary>
    public class ZipFile
    {
        public ZipFile()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
            //string startFolder = @"C:\inetpub\wwwroot\conversion";
            //string outputZipPath = @"C:\inetpub\wwwroot";
            //string password = "1111";   
            //Zeospace.DeskPlus.Component.WebComponent.ZipFile.ZipIt(startFolder,outputZipPath + "\\" + DateTime.Now.ToString("yyyyddmmhhmm")	+ ".exe", password);
        }

        static Hashtable excludeDirs = new Hashtable();

        //압축하기
        public static void ZipCompress(string Path, string outPathAndZipFile, string password)
        {
            string OutPath = outPathAndZipFile;
            ArrayList ar = GenerateFileList(Path); // generate file list
            // find number of chars to remove from orginal file path
            int TrimLength = (Directory.GetParent(Path)).ToString().Length;
            TrimLength += 1; //remove '\'
            FileStream ostream;
            byte[] obuffer;
            ZipOutputStream oZipStream = new ZipOutputStream(System.IO.File.Create(OutPath)); // create zip stream
            if (password != String.Empty) oZipStream.Password = password;
            oZipStream.SetLevel(9); // 9 = maximum compression level
            ZipEntry oZipEntry;
            foreach (string Fil in ar) // for each file, generate a zipentry
            {
                oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                oZipStream.PutNextEntry(oZipEntry);

                if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                {
                    ostream = File.OpenRead(Fil);
                    obuffer = new byte[ostream.Length]; // byte buffer
                    ostream.Read(obuffer, 0, obuffer.Length);
                    oZipStream.Write(obuffer, 0, obuffer.Length);
                    ostream.Close();
                    Console.Write(".");
                }
            }

            oZipStream.Finish();
            oZipStream.Close();
        }

        private static ArrayList GenerateFileList(string Dir)
        {
            ArrayList mid = new ArrayList();
            bool Empty = true;
            foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
            {
                mid.Add(file);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0) // if directory is completely empty, add it
                {
                    mid.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // do this recursively
            {
                // set up the excludeDir test
                string testDir = dirs.Substring(dirs.LastIndexOf(@"\") + 1).ToUpper();
                if (ZipFile.excludeDirs.Contains(testDir))
                    continue;
                foreach (object obj in GenerateFileList(dirs))
                {
                    mid.Add(obj);
                }
            }
            return mid; // return file list
        }

        /// <summary>
        /// 압축풀기
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool ZipUnpack(string file, string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(dir + "\\" + directoryName);

                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(dir + "\\" + theEntry.Name);
                        int size = 2048;
                        byte[] data = new byte[2048];

                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }

                s.Close();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
