using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 
    public class Utilities
    {
        public static string ArrayToString(string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strs)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.AnalysisOutput CloneOutPut(org.apache.lucene.Analysis.kr.morph.AnalysisOutput o) throws org.apache.lucene.Analysis.kr.morph.MorphException
        public static AnalysisOutput CloneOutPut(AnalysisOutput o)
        {
            try
            {
                return o.Clone();
            }
            catch (Exception e)  //CloneNotSupportedException e)
            {
                throw new MorphException(e.Message, e);
            }
        }

        public static string BuildOutputString(AnalysisOutput o)
        {


            StringBuilder buff = new StringBuilder();

            buff.Append(MorphUtil.BuildTypeString(o.Stem, o.Pos));
            if (o.Nsfx != null)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Nsfx, PatternConstants.POS_SFX_N));
            }

            if (o.Patn == PatternConstants.PTN_NJ || o.Patn == PatternConstants.PTN_ADVJ)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
            }
            else if (o.Patn == PatternConstants.PTN_NSM)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Vsfx, PatternConstants.POS_SFX_V));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_NSMJ)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Vsfx, PatternConstants.POS_SFX_V));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_NEOMI));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
            }
            else if (o.Patn == PatternConstants.PTN_NSMXM)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Vsfx, PatternConstants.POS_SFX_V));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_COPULA));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Xverb, PatternConstants.POS_XVERB));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_NJCM)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_SFX_V));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_NSMXMJ)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Vsfx, PatternConstants.POS_SFX_V));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[1], PatternConstants.POS_COPULA));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Xverb, PatternConstants.POS_XVERB));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_NEOMI));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
            }
            else if (o.Patn == PatternConstants.PTN_VM)
            {
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_VMJ)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_NEOMI));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
            }
            else if (o.Patn == PatternConstants.PTN_VMCM)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_NEOMI));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[1], PatternConstants.POS_SFX_N));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_VMXM)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_COPULA));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Xverb, PatternConstants.POS_XVERB));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Eomi, PatternConstants.POS_EOMI));
            }
            else if (o.Patn == PatternConstants.PTN_VMXMJ)
            {
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[1], PatternConstants.POS_COPULA));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Xverb, PatternConstants.POS_XVERB));
                if (o.Pomi != null)
                {
                    buff.Append(",").Append(MorphUtil.BuildTypeString(o.Pomi, PatternConstants.POS_PEOMI));
                }
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Elist[0], PatternConstants.POS_NEOMI));
                buff.Append(",").Append(MorphUtil.BuildTypeString(o.Josa, PatternConstants.POS_JOSA));
            }
            return buff.ToString();

        }

        // -----------------------------------------------------------------------
        /// <summary>
        /// <p>
        /// Gets a System property, defaulting to <code>null</code> if the property cannot be read.
        /// </p>
        /// 
        /// <p>
        /// If a <code>SecurityException</code> is caught, the return value is <code>null</code> and a message is written to
        /// <code>System.err</code>.
        /// </p>
        /// </summary>
        /// <param name="property">
        ///            the system property name </param>
        /// <returns> the system property value or <code>null</code> if a security problem occurs </returns>
        public static string GetSystemProperty(string property)
        {
            //try
            //{
            //    return System.getProperty(property);
            //}
            //catch (SecurityException ex)
            //{
            //    // we are not allowed to look at this property
            //    Console.Error.WriteLine("Caught a SecurityException reading the system property '" + property + "'; the SystemUtils property value will default to null.");
            //    return null;
            //}

            return property;
        }
    }
}
