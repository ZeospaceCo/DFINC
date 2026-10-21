using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class WSCandidateComparator : IComparer<WSOutput>
    {
        public virtual int Compare(WSOutput o1, WSOutput o2)
        {

            int end = o2.LastEnd - o1.LastEnd;
            if (end != 0)
            {
                return end;
            }

            int s1 = o1.Phrases.Count == 0 ? 999999999 : o1.Phrases.Count;
            int s2 = o2.Phrases.Count == 0 ? 999999999 : o2.Phrases.Count;

            int size = s1 - s2;
            if (size != 0)
            {
                return size;
            }

            int score = CalculateScore(o2) - CalculateScore(o1);
            if (score != 0)
            {
                return score;
            }

            return 0;
        }

        private int CalculateScore(WSOutput o)
        {

            List<AnalysisOutput> entries = o.Phrases;

            if (entries.Count == 0)
            {
                return 0;
            }

            int sum = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                sum += entries[i].Score;
            }

            return sum / entries.Count;
        }
    }
}
