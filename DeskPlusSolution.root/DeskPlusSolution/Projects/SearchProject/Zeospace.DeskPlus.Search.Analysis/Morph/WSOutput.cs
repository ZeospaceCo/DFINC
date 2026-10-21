using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class WSOutput //: ICloneable
    {
        private int lastStart = 0;

        private int lastEnd = 0;

        private List<AnalysisOutput> phrases = new List<AnalysisOutput>();

        public WSOutput()
        {

        }

        public WSOutput(AnalysisOutput o)
        {
            AddPhrase(o);
        }

        public virtual int LastStart
        {
            get
            {
                return lastStart;
            }
            set
            {
                this.lastStart = value;
            }
        }


        public virtual int LastEnd
        {
            get
            {
                return lastEnd;
            }
            set
            {
                this.lastStart = value;
            }
        }



        public virtual List<AnalysisOutput> Phrases
        {
            get
            {
                return phrases;
            }
            set
            {
                this.phrases = value;
            }
        }

        public virtual void RemoveLast()
        {

            if (this.phrases.Count == 0)
            {
                return;
            }

            //AnalysisOutput o = this.phrases.Remove(this.phrases.Count - 1);

            AnalysisOutput o = this.phrases[this.phrases.Count - 1];
            this.phrases.RemoveAt(this.phrases.Count - 1);

            if (this.phrases.Count == 0)
            {

                this.lastStart = 0;
                this.lastEnd = 0;

            }
            else
            {

                this.lastEnd -= o.Source.Length;

                if (this.phrases.Count > 1)
                {
                    AnalysisOutput o1 = this.phrases[this.phrases.Count - 1];
                    this.lastStart = lastEnd - o1.Source.Length;
                }
                else
                {
                    this.lastStart = 0;
                }

            }

        }

        public virtual void AddPhrase(AnalysisOutput o)
        {

            this.lastStart = this.lastEnd;
            this.lastEnd += o.Source.Length;

            if (o.CNounList.Count == 0)
            {
                this.phrases.Add(o);
            }
            else
            {
                AddCompounds(o);
            }

        }

        private void AddCompounds(AnalysisOutput o)
        {

            List<CompoundEntry> cnouns = o.CNounList;

            string source = o.Source;
            int rmstemlen = 0;

            //		for(int i=0;i<cnouns.Count;i++) {
            //			System.out.println(cnouns.Get(i).Word);
            //		}
            for (int i = 0; i < cnouns.Count - 1; i++)
            {

                string noun = cnouns[i].Word;
                bool isOnechar = false;

                // 접두어는 처음 음절에만 온다. 복합명사 분해규칙
                // 처음이 아닌 경우 1글자는 앞 문자와 결합한다.
                if (cnouns[i].Word.Length == 1 || cnouns[i + 1].Word.Length == 1) // 접두어는 처음 음절에만 온다. 복합명사 분해규칙
                {
                    noun += cnouns[i + 1].Word;
                    isOnechar = true;
                }

                if (isOnechar && i >= cnouns.Count - 2)
                {
                    break;
                }

                int score = AnalysisOutput.SCORE_CORRECT;
                if (!cnouns[i].Exist)
                {
                    score = AnalysisOutput.SCORE_CANDIDATE;
                }

                AnalysisOutput o1 = new AnalysisOutput(noun, null, null, PatternConstants.POS_NOUN, PatternConstants.PTN_N, score);

                o1.Source = noun;

                if (isOnechar)
                {
                    o1.AddCNoun(cnouns[i]);
                    o1.AddCNoun(cnouns[i + 1]);
                }

                if (source.Length > noun.Length)
                {
                    source = source.Substring(noun.Length);
                }

                this.phrases.Add(o1);
                cnouns.Remove(cnouns[0]);
                i--;

                if (isOnechar)
                {
                    cnouns.Remove(cnouns[0]);
                }

            }

            o.Stem = o.Stem.Substring(o.Source.Length - source.Length);
            o.Source = source;
            if (cnouns.Count == 1)
            {
                cnouns.RemoveAt(0);
            }

            this.phrases.Add(o);

        }


        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public WSOutput Clone() throws CloneNotSupportedException
        public WSOutput Clone()
        {

            //WSOutput candidate = (WSOutput)base.Clone();
            WSOutput candidate = (WSOutput)this.Clone();

            candidate.LastStart = lastStart;

            candidate.LastEnd = lastEnd;

            List<AnalysisOutput> list = new List<AnalysisOutput>();
            list.AddRange(phrases);
            candidate.Phrases = list;

            return candidate;
        }


    }
}
