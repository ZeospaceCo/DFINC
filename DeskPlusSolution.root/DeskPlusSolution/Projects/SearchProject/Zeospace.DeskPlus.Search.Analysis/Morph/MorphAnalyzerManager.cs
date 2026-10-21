using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
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

    public class MorphAnalyzerManager
    {
        public virtual void Analyze(string strs)
        {
            MorphAnalyzer analyzer = new MorphAnalyzer();
            //string[] tokens = StringHelperClass.StringSplit(strs, " ", true);

            string[] tokens = strs.Split(' ');
            foreach (string token in tokens)
            {
                try
                {
                    List<AnalysisOutput> results = analyzer.Analyze(token);
                    foreach (AnalysisOutput o in results)
                    {
                        Console.Write(o.ToString() + "->");
                        for (int i = 0; i < o.CNounList.Count; i++)
                        {
                            Console.Write(o.CNounList[i] + "/");
                        }
                        Console.WriteLine("<" + o.Score + ">");
                    }
                }
                catch (MorphException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.Write(e.StackTrace);
                }
            }
        }
    }
}
