using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    /**
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */
    public class AnalysisOutput// : ICloneable
    {

        public const int SCORE_CORRECT = 100;
        public const int SCORE_COMPOUNDS = 70;
        public const int SCORE_ANALYSIS = 30;
        public const int SCORE_CANDIDATE = 10;
        public const int SCORE_FAIL = 0;

        private string source;  //분석하기 전 문자열(띄워쓰기 모듈에서 사용된다.)
        private int score; // score of this result
        private int patn; // word pattern
        private char type; // type of input word
        private List<CompoundEntry> compound = new List<CompoundEntry>(); // compound noun of input word
        private string stem;
        private char pos; // 3 simplified stem type
        private char pos2; // pos attr. for 'pos'
        private char dinf; // pos info. in Han-dic
        private string nsfx; // index of noun suffix
        private string josa; // josa string
        private List<string> jlist = new List<string>(); // unit-josa sequnce
        private string eomi; // Eomi string
        private List<string> elist = new List<string>(); // unit-Eomi sequence
        private string pomi; // prefinal Eomi
        private string xverb; // Xverb string
        private string vsfx; // verb suffix
        private char vtype; // irregular type

        public AnalysisOutput()
        {
            this.score = SCORE_FAIL;
        }

        public AnalysisOutput(string stem, string josa, string eomi, int patn)
        {
            this.score = SCORE_ANALYSIS;
            this.stem = stem;
            this.josa = josa;
            this.eomi = eomi;
            this.patn = patn;
        }

        public AnalysisOutput(string stem, string josa, string eomi, int patn, int score) : this(stem, josa, eomi, patn)
        {
            this.score = score;
        }

        public AnalysisOutput(string stem, string josa, string eomi, char pos, int patn, int score) : this(stem, josa, eomi, patn, score)
        {
            this.pos = pos;
        }

        public virtual int Score
        {
            set { this.score = value; }
            get { return this.score; }
        }
        public virtual int Patn
        {
            set { this.patn = value; }
            get { return this.patn; }
        }
        public virtual char Type
        {
            set { this.type = value; }
            get { return this.type; }
        }

        public virtual string Stem
        {
            set { this.stem = value; }
            get { return stem; }
        }


        public virtual char Pos
        {
            set { this.pos = value; }
            get { return this.pos; }
        }

        public virtual char Pos2
        {
            set { this.pos2 = value; }
            get { return this.pos2; }
        }

        public virtual char Dinf
        {
            set { this.dinf = value; }
            get { return this.dinf; }
        }

        public virtual string Nsfx
        {
            set { this.nsfx = value; }
            get { return this.nsfx; }
        }

        public virtual string Josa
        {
            set { this.josa = value; }
            get { return this.josa; }
        }

        public virtual void AddJlist(string l)
        {
            this.jlist.Add(l);
        }

        public virtual string Eomi
        {
            set { this.eomi = value; }
            get { return this.eomi; }
        }

        public virtual void AddElist(string l)
        {
            this.elist.Add(l);
        }

        public virtual void SetElist(string l, int index)
        {
            this.elist[index] = l;
        }

        public virtual string Pomi
        {
            set { this.pomi = value; }
            get { return this.pomi; }
        }
        public virtual string Xverb
        {
            set { this.xverb = value; }
            get { return this.xverb; }
        }
        public virtual string Vsfx
        {
            set { this.vsfx = value; }
            get { return this.vsfx; }
        }
        public virtual char Vtype
        {
            set { this.vtype = value; }
            get { return this.vtype; }
        }


        public virtual List<string> Jlist
        {
            get { return this.jlist; }
        }
        public virtual List<string> Elist
        {
            get { return this.elist; }
        }

        public virtual void AddCNoun(CompoundEntry w)
        {
            compound.Add(w);
        }

        public virtual List<CompoundEntry> CNounList
        {
            get { return compound; }
        }

        public List<CompoundEntry> CNoun
        {
            set { compound = value; }
        }

        public virtual void AddCNoun(List<CompoundEntry> cnoun)
        {
            compound.AddRange(cnoun);
        }

        /// <returns> the source </returns>
        public virtual string Source
        {
            get { return source; }
            set { this.source = value; }
        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public AnalysisOutput Clone() throws CloneNotSupportedException
        public AnalysisOutput Clone()
        {
            //AnalysisOutput output = (AnalysisOutput)base.Clone();
            AnalysisOutput output = this.Clone(); 

            output.Dinf = this.dinf;
            output.Eomi = this.eomi;
            output.Josa = this.josa;
            output.Nsfx = this.nsfx;
            output.Patn = this.patn;
            output.Pomi = this.pomi;
            output.Pos = this.pos;
            output.Pos2 = this.pos2;
            output.Score = this.score;
            output.Stem = this.stem;
            output.Type = this.type;
            output.Vsfx = this.vsfx;
            output.Vtype = this.vtype;
            output.Xverb = this.xverb;

            return output;
        }

        public override string ToString()
        {
            return Utilities.BuildOutputString(this);
        }
    }
}
