using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


    /// <summary>
    /// JarResources: JarResources maps all resources included in a
    /// Zip or Jar file. Additionaly, it provides a method to extract one
    /// as a blob.
    /// </summary>
    
    public class JarResources
    {
        // external debug flag
        public bool debugOn = false;

        // jar resource mapping tables
        private Hashtable htSizes = new Hashtable();

        // a jar file
        private string jarFileName;

        /// <summary>
        /// creates a JarResources. It extracts all resources from a Jar
        /// into an internal hashtable, keyed by resource names. </summary>
        /// <param name="jarFileName"> a jar or zip file </param>
        public JarResources(string jarFileName)
        {
            this.jarFileName = jarFileName;
        }

        /// <summary>
        /// Extracts a jar resource as a blob. </summary>
        /// <param name="name"> a resource name. </param>
        public sbyte[] GetResource(string name)
        {
            return Read(name);
        }

        /// <summary>
        /// initializes internal hash tables with Jar file resources.
        /// </summary>
        private sbyte[] Read(string name)
        {
            //try
            //{
            //    // extracts just sizes only. 
            //    ZipFile zf = new ZipFile(jarFileName);
            //    System.Collections.IEnumerator e = zf.entries();
            //    while (e.hasMoreElements())
            //    {
            //        ZipEntry ze = (ZipEntry)e.nextElement();
            //        if (debugOn)
            //        {
            //            Console.WriteLine(dumpZipEntry(ze));
            //        }
            //        htSizes[ze.Name] = new int?((int)ze.Size);
            //    }
            //    zf.Close();

            //    // extract resources and put them into the hashtable.
            //    FileStream fis = new FileStream(jarFileName);
            //    BufferedStream bis = new BufferedStream(fis);
            //    ZipStream zis = new ZipStream(bis);
            //    ZipEntry ze = null;
            //    while ((ze = zis.NextEntry) != null)
            //    {
            //        if (ze.Directory)
            //        {
            //            continue;
            //        }
            //        if (debugOn)
            //        {
            //            Console.WriteLine("ze.getName()=" + ze.Name + "," + "getCount=" + ze.Size);
            //        }
            //        int size = (int)ze.Size;
            //        // -1 means unknown size. 
            //        if (size == -1)
            //        {
            //            size = (int)((int?)htSizes[ze.Name]);
            //        }
            //        sbyte[] b = new sbyte[(int)size];
            //        int rb = 0;
            //        int chunk = 0;
            //        while (((int)size - rb) > 0)
            //        {
            //            chunk = zis.read(b, rb, (int)size - rb);
            //            if (chunk == -1)
            //            {
            //                break;
            //            }
            //            rb += chunk;
            //        }

            //        if (debugOn)
            //        {
            //            Console.WriteLine(ze.Name + "  rb=" + rb + ",size=" + size + ",csize=" + ze.CompressedSize);
            //        }

            //        if (ze.Name.Equals(name))
            //        {
            //            return b;
            //        }
            //    }
            //}
            //catch (NullPointerException e)
            //{
            //    Console.WriteLine("done.");
            //}
            //catch (FileNotFoundException e)
            //{
            //    Console.WriteLine(e.ToString());
            //    Console.Write(e.StackTrace);
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine(e.ToString());
            //    Console.Write(e.StackTrace);
            //}

            return null;
        }

        /// <summary>
        /// Dumps a zip entry into a string. </summary>
        /// <param name="ze"> a ZipEntry </param>
        //private string dumpZipEntry(ZipEntry ze)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (ze.Directory)
        //    {
        //        sb.Append("d ");
        //    }
        //    else
        //    {
        //        sb.Append("f ");
        //    }
        //    if (ze.Method == ZipEntry.STORED)
        //    {
        //        sb.Append("stored   ");
        //    }
        //    else
        //    {
        //        sb.Append("defalted ");
        //    }
        //    sb.Append(ze.Name);
        //    sb.Append("\t");
        //    sb.Append("" + ze.Size);
        //    if (ze.Method == ZipEntry.DEFLATED)
        //    {
        //        sb.Append("/" + ze.CompressedSize);
        //    }
        //    return (sb.ToString());
        //}
    }
}
