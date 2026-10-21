using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class WSOuputComparator : IComparer<AnalysisOutput>
    {
        public virtual int Compare(AnalysisOutput o1, AnalysisOutput o2)
        {

            // 길이의 역순으로 정렬한다.

            int score = o2.Score - o1.Score;
            if (score != 0)
            {
                return score;
            }

            int len = o2.Source.Length - o1.Source.Length;
            if (len != 0)
            {
                return len;
            }


            int ptn = GetPtnScore(o2.Patn) - GetPtnScore(o1.Patn);
            if (ptn != 0)
            {
                return ptn;
            }

            int stem = o1.Stem.Length - o2.Stem.Length;
            if (stem != 0)
            {
                return stem;
            }


            return 0;
        }

        private int GetPtnScore(int ptn)
        {

            if (ptn == PatternConstants.PTN_N)
            {
                ptn = 7;
            }
            else if (ptn == PatternConstants.PTN_AID)
            {
                return 50;
            }

            return ptn;

        }
    }
}
