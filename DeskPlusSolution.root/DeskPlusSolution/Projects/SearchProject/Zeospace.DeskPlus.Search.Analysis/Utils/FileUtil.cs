using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Zeospace.DeskPlus.Search.Analysis.Morph;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    /// <summary>
    /// Licensed to the Apache Software Foundation (ASF) under one or more
    /// contributor license agreements.  See the NOTICE file distributed with
    /// this work for additional information regarding copyright ownership.
    /// The ASF licenses this file to You under the Apache License, Version 2.0
    /// (the "License"); you may not use this file except in compliance with
    /// the License.  You may obtain a copy of the License at
    /// 
    ///     http://www.apache.org/licenses/LICENSE-2.0
    /// 
    /// Unless required by applicable law or agreed to in writing, software
    /// distributed under the License is distributed on an "AS IS" BASIS,
    /// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    /// See the License for the specific language governing permissions and
    /// limitations under the License.
    /// </summary>
    
    public class FileUtil
    {
        //public static FileInfo getClassLoaderFile(string filename)
        //{
        //    // note that this method is used when initializing logging, so it must
        //    // not attempt to log anything.
        //    FileInfo file = null;
        //    ClassLoader loader = typeof(FileUtil).ClassLoader;
        //    URL url = loader.GetResource(filename);
        //    if (url == null)
        //    {
        //        url = ClassLoader.getSystemResource(filename);
        //        if (url == null)
        //        {
        //            throw new MorphException("Unable to find " + filename);
        //        }
        //        file = toFile(url);
        //    }
        //    else
        //    {
        //        file = toFile(url);
        //    }
        //    if (file == null || !file.exists())
        //    {
        //        return null;
        //    }
        //    return file;
        //}

        public static List<string> ReadLines(FileInfo file, string encoding)
        {
            Stream @in = null;
            try
            {
                @in = OpenStream(file);
                return ReadLines(@in, encoding);
            }
            finally
            {
                CloseQuietly(@in);
            }
        }

        public static List<string> ReadLines(string fName, string encoding)
        {
            Stream @in = null;
            try
            {

                //File file = getClassLoaderFile(fName);

                FileInfo file = new FileInfo(fName);

                if (file != null && file.Exists)
                {
                    @in = OpenStream(file);
                }
                else
                {
                    //@in = new ByteArrayStream(readByteFromCurrentJar(fName));
                }

                return ReadLines(@in, encoding);
            }
            finally
            {
                CloseQuietly(@in);
            }
        }

        private static bool IsDirectory(System.IO.FileAttributes Attr)
        {
            bool isDirectory = false;
            if ((Attr & FileAttributes.Directory) != 0)
            {
                isDirectory = true;
            }
            return isDirectory;
        } 


        public static FileStream OpenStream(FileInfo file)
        {
            if (file.Exists)
            {
                if (IsDirectory(file.Attributes))
                {
                    throw new IOException("File '" + file.FullName + "' exists but is a directory");
                }

                FileStream fs = new FileStream(file.FullName, FileMode.Open);
                
                if (fs.CanRead == false)
                {
                    throw new IOException("File '" + file.FullName + "' cannot be read");
                }

                return fs;
            }
            else
            {
                throw new FileNotFoundException("File '" + file.FullName + "' does not exist");
            }
        }

        public static List<string> ReadLines(Stream input)
        {
            StreamReader reader = new StreamReader(input);
            return ReadLines(reader);
        }

        public static List<string> ReadLines(Stream input, string encoding)
        {
            if (encoding == null)
            {
                return ReadLines(input);
            }
            else
            {
                StreamReader reader = new StreamReader(input, Encoding.GetEncoding(encoding));
                return ReadLines(reader);
            }
        }

        public static List<string> ReadLines(StreamReader input)
        {
            List<string> list = new List<string>();

            while (input.Peek() > -1)
            {
                list.Add(input.ReadLine());
            }

            return list;
        }

        public static void CloseQuietly(Stream input)
        {
            try
            {
                if (input != null)
                {
                    input.Close();
                }
            }
            catch (IOException ioe)
            {
                // ignore
            }
        }

        //public static FileInfo ToFile(URL url)
        //{
        //    if (url == null || !url.Protocol.Equals("file"))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        string filename = url.File.Replace('/', System.IO.Path.DirectorySeparatorChar);
        //        int pos = 0;
        //        while ((pos = filename.IndexOf('%', pos)) >= 0)
        //        {
        //            if (pos + 2 < filename.Length)
        //            {
        //                string hexStr = filename.Substring(pos + 1, pos + 3 - (pos + 1));
        //                char ch = (char)Convert.ToInt32(hexStr, 16);
        //                filename = filename.Substring(0, pos) + ch + filename.Substring(pos + 3);
        //            }
        //        }
        //        return new FileInfo(filename);
        //    }
        //}

        public static string ReadFileToString(FileInfo file, string encoding)
        {
            Stream @in = null;
            try
            {
                @in = OpenStream(file);
                return StringUtil.ToString(@in, encoding);
            }
            finally
            {
                CloseQuietly(@in);
            }
        }


        public static sbyte[] ReadByteFromCurrentJar(string resource)
        {

            //string jarPath = typeof(FileUtil).ProtectionDomain.CodeSource.Location.Path;

            //JarResources jar = new JarResources(jarPath);
            //try
            //{
            //    return jar.GetResource(resource);
            //}
            //catch (Exception e)
            //{
            //throw new MorphException(e.Message, e);
            //}

            return null;
        }
    }
}
