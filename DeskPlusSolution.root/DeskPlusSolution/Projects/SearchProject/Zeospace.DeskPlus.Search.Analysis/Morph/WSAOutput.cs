using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class WSAOutput
    {
        private string source;

        private List<AnalysisOutput> results;

        private int wds = 0;

        private int end = 0;

        public WSAOutput()
        {
            results = new List<AnalysisOutput>();
        }

        public WSAOutput(string src)
        {
            source = src;
            results = new List<AnalysisOutput>();
        }

        public WSAOutput(string src, List<AnalysisOutput> list)
        {
            source = src;
            results = list;
        }

        public virtual string Source
        {
            get
            {
                return source;
            }
            set
            {
                this.source = value;
            }
        }


        public virtual List<AnalysisOutput> Results
        {
            get
            {
                return results;
            }
            set
            {
                this.results = value;
            }
        }


        public virtual void AddNounResults(string word)
        {
            AddNounResults(word, null);
        }

        public virtual void AddNounResults(string word, string end)
        {
            AddNounResults(word, end, AnalysisOutput.SCORE_ANALYSIS);
        }

        public virtual void AddNounResults(string word, string end, int score)
        {

            AnalysisOutput output = new AnalysisOutput(word, end, null, PatternConstants.PTN_NJ);
            if (end == null)
            {
                output.Patn = PatternConstants.PTN_N;
            }

            output.Pos = PatternConstants.POS_NOUN;
            output.Score = score;

            this.results.Add(output);
        }
    }
}
