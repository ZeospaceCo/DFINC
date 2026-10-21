using System;
using System.Collections;
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
    
    public class WordEntry
    {
        public const int IDX_NOUN = 0;
        public const int IDX_VERB = 1;
        public const int IDX_BUSA = 2;
        public const int IDX_DOV = 3;
        public const int IDX_BEV = 4;
        public const int IDX_NE = 5;
        public const int IDX_ADJ = 6; // ?뺤슜??
        public const int IDX_NPR = 7; // 紐낆궗??遺꾨쪟 (M:Measure)
        public const int IDX_CNOUNX = 8;
        public const int IDX_REGURA = 9;

        /// <summary>
        /// ?⑥뼱
        /// </summary>
        private string word;

        /// <summary>
        /// ?⑥뼱?뱀꽦
        /// </summary>
        private char[] features;

        private List<CompoundEntry> compounds = new List<CompoundEntry>();

        public WordEntry()
        {

        }

        public WordEntry(string word)
        {
            this.word = word;
        }

        public WordEntry(string word, char[] cs)
        {
            this.word = word;
            this.features = cs;
        }

        public WordEntry(string word, List<CompoundEntry> c)
        {
            this.word = word;
            this.compounds = c;
        }

        public virtual string Word
        {
            set
            {
                this.word = value;
            }
            get
            {
                return this.word;
            }
        }


        public virtual char[] Features
        {
            set
            {
                this.features = value;
            }
            get
            {
                return this.features;
            }
        }

        public virtual char GetFeature(int index)
        {
            if (features == null || features.Length < index)
            {
                return '0';
            }
            return features[index];
        }


        public virtual List<CompoundEntry> Compounds
        {
            set
            {
                this.compounds = value;
            }
            get
            {
                return this.compounds;
            }
        }
    }
}
