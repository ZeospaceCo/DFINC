using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
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
    
    public class KoreanEnv
    {
        public const string FILE_SYLLABLE_FEATURE = "syllable.dic";
        public const string FILE_DICTIONARY = "dictionary.dic";
        public const string FILE_JOSA = "josa.dic";
        public const string FILE_EOMI = "eomi.dic";
        public const string FILE_EXTENSION = "extension.dic";
        public const string FILE_PREFIX = "prefix.dic";
        public const string FILE_SUFFIX = "suffix.dic";
        public const string FILE_COMPOUNDS = "compounds.dic";
        public const string FILE_UNCOMPOUNDS = "uncompounds.dic";
        public const string FILE_CJ = "cj.dic";

        private Hashtable defaults = null;
        private static KoreanEnv instance = null;

        private KoreanEnv()
        {
            try
            {
                InitDefaultProperties();
            }
            catch (MorphException e)
            {
                throw new MorphException("Failure while initializing property values:\n" + e.Message);
            }
        }

        public static KoreanEnv Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KoreanEnv();
                }

                return instance;
            }
        }

        /// <summary>
        /// Initialize the default property values.
        /// </summary>
        private void InitDefaultProperties()
        {
            defaults = new Hashtable();

            defaults.Add(FILE_SYLLABLE_FEATURE, "/DeskPlusEIP/Search/Dic/syllable.dic");
            defaults.Add(FILE_DICTIONARY, "/DeskPlusEIP/Search/Dic/dictionary.dic");
            defaults.Add(FILE_DICTIONARY, "/DeskPlusEIP/Search/Dic/extension.dic");
            defaults.Add(FILE_JOSA, "/DeskPlusEIP/Search/Dic/josa.dic");
            defaults.Add(FILE_EOMI, "/DeskPlusEIP/Search/Dic/eomi.dic");
            defaults.Add(FILE_PREFIX, "/DeskPlusEIP/Search/Dic/prefix.dic");
            defaults.Add(FILE_SUFFIX, "/DeskPlusEIP/Search/Dic/suffix.dic");
            defaults.Add(FILE_COMPOUNDS, "/DeskPlusEIP/Search/Dic/compounds.dic");
            defaults.Add(FILE_UNCOMPOUNDS, "/DeskPlusEIP/Search/Dic/uncompounds.dic");
            defaults.Add(FILE_CJ, "/DeskPlusEIP/Search/Dic/cj.dic");
        }



        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <param name="name"> The name of the property whose value is to be retrieved. </param>
        /// <returns> The value of the property. </returns>
        public virtual string GetValue(string name)
        {
            return defaults[name].ToString();
            //HttpContext context = HttpContext.Current;
            //string targetPath = "/DeskPlusEIP/Search/DIC/"; //사전 경로
            //context.Server.MapPath(targetPath + name);
            //return name;
            //return props.getProperty(name);
        }
    }
}
