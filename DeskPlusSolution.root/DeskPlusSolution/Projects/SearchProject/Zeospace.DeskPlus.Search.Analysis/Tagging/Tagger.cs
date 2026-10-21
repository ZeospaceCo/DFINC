using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;
using Zeospace.DeskPlus.Search.Analysis.Morph;

namespace Zeospace.DeskPlus.Search.Analysis.Tagging
{
    /// <summary>
    /// 여러개의 형태소분석 결과 중에 최적의 것을 선택한다.
    /// 이 함수는 문장단위로 호출되어야 한다.
    /// </summary>
    public class Tagger
    {
        private static Trie<string, String[]> occurrences;

        private const string tagDicLoc = "tagger.dic";

        private const string NILL = "NILL";

        private const string NOPATN = "0";

        private AnalysisOutput po;

        public virtual AnalysisOutput Tagging(string psource, List<AnalysisOutput> pmorphs)
        {

            return Tagging(psource, null, pmorphs, null);

        }

        public virtual AnalysisOutput Tagging(string psource, string rsource, List<AnalysisOutput> pmorphs, List<AnalysisOutput> rmorphs)
        {

            if ((pmorphs == null || pmorphs.Count == 0) && (rmorphs == null || rmorphs.Count == 0))
            {
                return null;
            }

            po = LookupBest(psource, rsource, pmorphs, rmorphs);

            po.Source = psource;

            return po;

        }

        /// <summary>
        /// po가 NULL이 아닌 경우만 호출된다.
        /// occurrence.dic 에 등록되어 있는 경우만.. 최적을 찾아서 반환한다.
        /// 1. 첫번째는 어간으로 시작되는 문법 규칙을 찾는다.
        /// 2. 두번째는 표층형으로 시작되는 문법규칙을 찾는다.
        /// </summary>
        /// <param name="psource"></param>
        /// <param name="rsource"></param>
        /// <param name="pmorphs"></param>
        /// <param name="rmorphs"></param>
        /// <returns></returns>
        private AnalysisOutput LookupBest(string psource, string rsource, List<AnalysisOutput> pmorphs, List<AnalysisOutput> rmorphs)
        {

            if (pmorphs.Count == 1)
            {
                return pmorphs[0];
            }

            AnalysisOutput select = null;
            if (rmorphs != null && rmorphs.Count != 0)
            {
                select = LookupBestByRWord(psource, rsource, pmorphs, rmorphs);
            }
            if (select != null)
            {
                return select;
            }

            if (po != null)
            {
                select = LookupBestByPWord(psource, pmorphs);
            }

            if (select != null)
            {
                return select;
            }

            return pmorphs[0];
        }

        /// <summary>
        /// 앞 어절에 의해 현재 어절을 결정한다.
        /// 앞 어절은 NULL이 아니다.
        /// </summary>
        /// <param name="rsource"></param>
        /// <param name="rmorphs"></param>
        /// <returns></returns>
        private AnalysisOutput LookupBestByPWord(string rsource, List<AnalysisOutput> rmorphs)
        {
            List<AnalysisOutput> removes = new List<AnalysisOutput>();

            foreach (AnalysisOutput morph in rmorphs)
            {

                IEnumerator<string[]> iterw = GetGR("F" + rsource + "^W");

                AnalysisOutput best = SelectBest(iterw, po.Source, rsource, po, morph, true, removes);
                if (best != null)
                {
                    return best;
                }

                IEnumerator<string[]> iters = GetGR("F" + morph.Stem + "^S");
                best = SelectBest(iters, po.Source, rsource, po, morph, true, removes);
                if (best != null)
                {
                    return best;
                }

            }

            foreach (AnalysisOutput morph in removes)
            {
                if (rmorphs.Count > 1)
                {
                    rmorphs.Remove(morph);
                }
            }

            return null;

        }

        /// <summary>
        /// 뒷 어절에 의해 현재 어절이 결정된다.
        /// 뒷 어절은 NULL이 아니다.
        /// </summary>
        /// <param name="psource"></param>
        /// <param name="rsource"></param>
        /// <param name="pmorphs"></param>
        /// <param name="rmorphs"></param>
        /// <returns></returns>
        private AnalysisOutput LookupBestByRWord(string psource, string rsource, List<AnalysisOutput> pmorphs, List<AnalysisOutput> rmorphs)
        {
            List<AnalysisOutput> removes = new List<AnalysisOutput>();

            foreach (AnalysisOutput rmorph in rmorphs)
            {
                if (rmorph.Score != AnalysisOutput.SCORE_CORRECT)
                {
                    break;
                }

                string rend = rmorph.Josa;
                if (rend == null)
                {
                    rend = rmorph.Eomi;
                }

                foreach (AnalysisOutput pmorph in pmorphs)
                {

                    IEnumerator<string[]> iterw = GetGR("R" + psource + "^W/");

                    string pend = pmorph.Josa;
                    if (pend == null)
                    {
                        pend = pmorph.Eomi;
                    }

                    AnalysisOutput best = SelectBest(iterw, psource, rsource, pmorph, rmorph, false, removes);
                    if (best != null)
                    {
                        return best;
                    }

                    IEnumerator<string[]> iters = GetGR("R" + NILL + "/" + pend + "/");
                    best = SelectBest(iters, psource, rsource, pmorph, rmorph, false, removes);
                    if (best != null)
                    {
                        return best;
                    }

                    iters = GetGR("R" + pmorph.Stem + "^S/");
                    best = SelectBest(iters, psource, rsource, pmorph, rmorph, false, removes);
                    if (best != null)
                    {
                        return best;
                    }

                }

            }

            foreach (AnalysisOutput morph in removes)
            {
                if (pmorphs.Count > 1)
                {
                    pmorphs.Remove(morph);
                }
            }

            return null;

        }

        private AnalysisOutput SelectBest(IEnumerator<string[]> iter, string psource, string rsource, AnalysisOutput pmorph, AnalysisOutput rmorph, bool rear, IList removes)
        {

            while (iter.MoveNext())
            {

                string[] values = iter.Current;

                if (CheckGrammer(values, psource, rsource, pmorph, rmorph, rear))
                {
                    if (rear)
                    {
                        return rmorph;
                    }
                    else
                    {
                        return pmorph;
                    }
                }
                else if ("1".Equals(values[6]))
                {
                    if (!removes.Contains(pmorph))
                    {
                        removes.Add(pmorph);
                    }
                    break;
                }
            }

            return null;

        }

        private bool CheckGrammer(string[] values, string psource, string rsource, AnalysisOutput pmorph, AnalysisOutput rmorph, bool depFront)
        {

            bool ok = true;

            string pend = pmorph.Josa;
            if (pend == null)
            {
                pend = pmorph.Eomi;
            }

            string rend = rmorph.Josa;
            if (rend == null)
            {
                rend = rmorph.Eomi;
            }

            if (depFront && !NILL.Equals(values[0]) && !CheckWord(psource, values[0], pmorph)) // 앞 어절의 어휘
            {
                return false;
            }

            if (!NILL.Equals(values[1]) && !CheckEomi(values[1], pend)) // 앞 어절의 어미
            {
                return false;
            }

            if (!NOPATN.Equals(values[2]) && !CheckPattern(values[2], pmorph.Patn)) // 앞 어절의 패턴
            {
                return false;
            }

            if (!depFront && !NILL.Equals(values[3]) && !CheckWord(rsource, values[3], rmorph)) // 뒷 어절의 어휘
            {
                return false;
            }

            if (!NILL.Equals(values[4]) && !CheckEomi(values[4], rend)) // 뒷 어절의 어미
            {
                return false;
            }

            if (!NOPATN.Equals(values[5]) && !CheckPattern(values[5], rmorph.Patn)) // 뒷 어절의 패턴
            {
                return false;
            }

            return true;

        }

        private bool CheckWord(string source, string value, AnalysisOutput morph)
        {

            string[] types = StringUtil.Split(value, "^");
            string[] strs = StringUtil.Split(types[0], ",");

            string text = source;
            if ("S".Equals(types[1]))
            {
                text = morph.Stem;
            }

            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].Equals(text))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckEomi(string value, string rend)
        {

            string[] strs = StringUtil.Split(value, ",");

            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].Equals(rend))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckPattern(string value, int ptn)
        {

            string[] strs = StringUtil.Split(value, ",");
            string strPtn = Convert.ToString(ptn);

            for (int i = 0; i < strs.Length; i++)
            {

                if ("E".Equals(strs[i]) && ConstraintUtil.IsEomiPhrase(ptn))
                {
                    return true;
                }
                else if ("J".Equals(strs[i]) && (ConstraintUtil.IsJosaNounPhrase(ptn) || ptn == PatternConstants.PTN_N))
                {
                    return true;
                }
                else if (strs[i].Equals(strPtn))
                {
                    return true;
                }

            }

            return false;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEnumerator<string[]> GetGR(string prefix)
        {

            if (occurrences == null)
            {
                LoadTaggerDic();
            }

            return (IEnumerator<string[]>)occurrences.GetPrefixedBy(prefix);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void LoadTaggerDic()
        {
            occurrences = new Trie<string, string[]>(true);

            try
            {

                List<string> strs = FileUtil.ReadLines(KoreanEnv.Instance.GetValue(tagDicLoc), "UTF-8");

                foreach (string str in strs)
                {
                    if (str == null)
                    {
                        continue;
                    }
                    //str = str.Trim();
                    string[] syls = StringUtil.Split(str, ":");
                    if (syls.Length != 4)
                    {
                        continue;
                    }

                    string key = null;
                    if ("F".Equals(syls[0]))
                    {
                        key = syls[2].Substring(0, syls[2].LastIndexOf("/") + 1) + syls[1].Substring(0, syls[1].LastIndexOf("/"));
                    }
                    else
                    {
                        key = syls[1].Substring(0, syls[1].LastIndexOf("/") + 1) + syls[2].Substring(0, syls[2].LastIndexOf("/"));
                    }

                    string[] patns = StringUtil.Split(syls[1] + "/" + syls[2] + "/" + syls[3], "/");

                    occurrences.Add(syls[0] + key, patns);

                }

            }
            catch (Exception e)
            {
                throw new MorphException("Fail to read the tagger dictionary.(" + tagDicLoc + ")\n" + e.Message);
            }
        }

    }
}
