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

    public class AnalysisOutputComparator
    {
        public virtual int Compare(object o1, object o2)
        {

            AnalysisOutput out1 = (AnalysisOutput)o1;
            AnalysisOutput out2 = (AnalysisOutput)o2;

            int score = out2.Score - out1.Score;
            int pattern = out2.Patn - out1.Patn;
            int len = out1.Stem.Length - out2.Stem.Length;

            if (score != 0)
            {
                return score;
            }

            if (out2.Score == AnalysisOutput.SCORE_CORRECT && out1.Score == AnalysisOutput.SCORE_CORRECT)
            {
                pattern = out1.Patn == PatternConstants.PTN_N || out1.Patn == PatternConstants.PTN_AID ? -1 : pattern;
                pattern = out2.Patn == PatternConstants.PTN_N || out2.Patn == PatternConstants.PTN_AID ? 1 : pattern;
            }

            if (pattern != 0)
            {
                return pattern;
            }

            return len;
        }
    }
}
