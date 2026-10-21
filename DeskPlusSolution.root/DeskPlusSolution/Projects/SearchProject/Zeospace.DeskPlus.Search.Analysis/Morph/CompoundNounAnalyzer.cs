using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; 
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    /// <summary>
    /// 복합명사를 분해한다.
    /// </summary>
    public class CompoundNounAnalyzer
    {

        private static int score = 1;

        private bool exactMach = true;

        private static Regex NUM_PATTERN;
        private static Regex ALPHANUM_PATTERN;
        static CompoundNounAnalyzer()
        {
            NUM_PATTERN = new Regex("^[0-9\\.,]+$");
            ALPHANUM_PATTERN = new Regex("^[0-9A-Za-z\\.,]+$");
        }

        

        public virtual bool ExactMach
        {
            get
            {
                return exactMach;
            }
            set
            {
                this.exactMach = value;
            }
        }

        public virtual List<CompoundEntry> Analyze(string input)
        {

            return Analyze(input, true);

        }

        public virtual List<CompoundEntry> Analyze(string input, bool isFirst)
        {

            int len = input.Length;
            if (len < 3)
            {
                return new List<CompoundEntry>();
            }

            List<CompoundEntry> outputs = new List<CompoundEntry>();

            switch (len)
            {
                case 3:
                    Analyze3Word(input, outputs, isFirst);
                    break;
                case 4:
                    Analyze4Word(input, outputs, isFirst);
                    break;
                case 5:
                    Analyze5Word(input, outputs, isFirst);
                    break;
                case 6:
                    Analyze6Word(input, outputs, isFirst);
                    break;
                default:
                    AnalyzeLongText(input, outputs, isFirst);
                    break;
            }

            return outputs;

        }

        private void Analyze3Word(string input, List<CompoundEntry> outputs, bool isFirst)
        {

            int[] units1 = { 2, 1 };
            CompoundEntry[] entries1 = AnalysisBySplited(units1, input, isFirst);
            if (entries1 != null && entries1[0].Exist && entries1[1].Exist)
            {
                entries1.GetEnumerator();
                foreach (CompoundEntry ce in entries1)
                {
                    outputs.Add(ce);
                }
                outputs.AddRange(entries1);
                return;
            }
            

            int[] units2 = { 1, 2 };
            CompoundEntry[] entries2 = AnalysisBySplited(units2, input, isFirst);
            if (entries2 != null && entries2[0].Exist && entries2[1].Exist)
            {
                outputs.AddRange(entries2);
            }

        }

        private void Analyze4Word(string input, List<CompoundEntry> outputs, bool isFirst)
        {

            if (!isFirst)
            {
                int[] units0 = { 1, 3 };
                CompoundEntry[] entries0 = AnalysisBySplited(units0, input, isFirst);
                if (entries0 != null && entries0[0].Exist && entries0[1].Exist)
                {
                    outputs.AddRange(entries0);
                    return;
                }
            }

            int[] units2 = { 1, 2, 1 };
            CompoundEntry[] entries2 = AnalysisBySplited(units2, input, isFirst);
            if (entries2 != null && entries2[0].Exist && entries2[1].Exist && entries2[2].Exist)
            {
                outputs.AddRange(entries2);
                return;
            }

            int[] units1 = { 2, 2 };
            CompoundEntry[] entries1 = AnalysisBySplited(units1, input, isFirst);
            if (entries1 != null && entries1[0].Exist && entries1[1].Exist)
            {
                outputs.AddRange(entries1);
                return;
            }

            if (!exactMach && entries1 != null && (entries1[0].Exist || entries1[1].Exist))
            {
                outputs.AddRange(entries1);
            }
        }

        private void Analyze5Word(string input, List<CompoundEntry> outputs, bool isFirst)
        {

            int[] units1 = { 2, 3 };
            CompoundEntry[] entries1 = AnalysisBySplited(units1, input, isFirst);
            if (entries1 != null && entries1[0].Exist && entries1[1].Exist)
            {
                outputs.AddRange(entries1);
                return;
            }

            int[] units2 = { 3, 2 };
            CompoundEntry[] entries2 = AnalysisBySplited(units2, input, isFirst);
            if (entries2 != null && entries2[0].Exist && entries2[1].Exist)
            {
                outputs.AddRange(entries2);
                return;
            }

            int[] units_1 = { 4, 1 };
            CompoundEntry[] entries_1 = AnalysisBySplited(units_1, input, isFirst);
            if (entries_1 != null && entries_1[0].Exist && entries_1[1].Exist)
            {
                outputs.AddRange(entries_1);
                return;
            }

            int[] units3 = { 2, 2, 1 };
            CompoundEntry[] entries3 = AnalysisBySplited(units3, input, isFirst);
            if (entries3 != null && entries3[0].Exist && entries3[1].Exist && entries3[2].Exist)
            {
                outputs.AddRange(entries3);
                return;
            }

            int[] units4 = { 2, 1, 2 };
            CompoundEntry[] entries4 = AnalysisBySplited(units4, input, isFirst);
            if (entries4 != null && entries4[0].Exist && entries4[1].Exist && entries4[2].Exist)
            {
                outputs.AddRange(entries4);
                return;
            }

            if (!exactMach && entries1 != null && (entries1[0].Exist || entries1[1].Exist))
            {
                outputs.AddRange(entries1);
                return;
            }

            if (!exactMach && entries2 != null && (entries2[0].Exist || entries2[1].Exist))
            {
                outputs.AddRange(entries2);
                return;
            }

            if (!exactMach && entries3 != null && (entries3[0].Exist || entries3[1].Exist))
            {
                outputs.AddRange(entries3);
            }

            if (!exactMach && entries4 != null && (entries4[0].Exist || entries4[2].Exist))
            {
                outputs.AddRange(entries4);
            }
        }

        private void Analyze6Word(string input, List<CompoundEntry> outputs, bool isFirst)
        {

            int[] units3 = { 2, 4 };
            CompoundEntry[] entries3 = AnalysisBySplited(units3, input, isFirst);
            if (entries3 != null && entries3[0].Exist && entries3[1].Exist)
            {
                outputs.AddRange(entries3);
                return;
            }

            int[] units4 = { 4, 2 };
            CompoundEntry[] entries4 = AnalysisBySplited(units4, input, isFirst);
            if (entries4 != null && entries4[0].Exist && entries4[1].Exist)
            {
                outputs.AddRange(entries4);
                return;
            }

            int[] units2 = { 3, 3 };
            CompoundEntry[] entries2 = AnalysisBySplited(units2, input, isFirst);
            if (entries2 != null && entries2[0].Exist && entries2[1].Exist)
            {
                outputs.AddRange(entries2);
                return;
            }

            int[] units6 = { 3, 2, 1 };
            CompoundEntry[] entries6 = AnalysisBySplited(units6, input, isFirst);
            if (entries6 != null && entries6[0].Exist && entries6[1].Exist)
            {
                outputs.AddRange(entries6);
                return;
            }

            int[] units7 = { 2, 3, 1 };
            CompoundEntry[] entries7 = AnalysisBySplited(units7, input, isFirst);
            if (entries7 != null && entries7[0].Exist && entries7[1].Exist)
            {
                outputs.AddRange(entries7);
                return;
            }

            int[] units1 = { 2, 2, 2 };
            CompoundEntry[] entries1 = AnalysisBySplited(units1, input, isFirst);
            if (entries1 != null && entries1[0].Exist && entries1[1].Exist && entries1[2].Exist)
            {
                outputs.AddRange(entries1);
                return;
            }

            int[] units5 = { 2, 1, 2, 1 };
            CompoundEntry[] entries5 = AnalysisBySplited(units5, input, isFirst);
            if (entries5 != null && entries5[0].Exist && entries5[1].Exist && entries5[2].Exist && entries5[3].Exist)
            {
                outputs.AddRange(entries5);
                return;
            }

            if (!exactMach && entries1 != null && CanCompound(entries1, 2 * score))
            {
                outputs.AddRange(entries1);
                return;
            }

            if (!exactMach && entries2 != null && (entries2[0].Exist || entries2[1].Exist))
            {
                outputs.AddRange(entries2);
                return;
            }

            if (!exactMach && entries3 != null && (entries3[0].Exist || entries3[1].Exist))
            {
                outputs.AddRange(entries3);
                return;
            }

            if (!exactMach && entries4 != null && (entries4[0].Exist || entries4[1].Exist))
            {
                outputs.AddRange(entries4);
            }

        }

        private void AnalyzeLongText(string input, List<CompoundEntry> outputs, bool isFirst)
        {

            int pos = input.Length / 2;
            if (input.Length % 2 == 1)
            {
                pos++;
            }

            if (input.Length > 20) // 20글자 이상의 복합명사는 무시함.
            {
                return;
            }

            int score = 0;
            List<CompoundEntry> results = new List<CompoundEntry>();
            bool hasContain = false;

            for (int i = pos; i >= 2; i--)
            {

                string prev = input.Substring(0, i);
                string rear = input.Substring(i);

                List<CompoundEntry> candidates = new List<CompoundEntry>();

                CompoundEntry prevEntry = AnalyzeSingle(prev);
                if (prevEntry.Exist)
                {
                    candidates.Add(prevEntry);
                }
                else
                {
                    List<CompoundEntry> list = Analyze(prev, true);
                    if (list.Count == 0)
                    {
                        candidates.Add(prevEntry);
                    }
                    else
                    {
                        candidates.AddRange(list);
                    }
                }

                CompoundEntry e = candidates[candidates.Count - 1];
                if (!hasContain && ContainWord(e.Word, input, i))
                {
                    i -= e.Word.Length - 1;
                    hasContain = true;
                    continue;
                }

                CompoundEntry rearEntry = AnalyzeSingle(rear);
                if (rearEntry.Exist || rear.Length == 3)
                {
                    candidates.Add(rearEntry);
                }
                else
                {
                    List<CompoundEntry> list = Analyze(rear, false);

                    if (list.Count == 0)
                    {
                        if (!e.Exist)
                        {
                            candidates[candidates.Count - 1] = AnalyzeSingle(e.Word + rearEntry.Word);
                        }
                        else
                        {
                            candidates.Add(rearEntry);
                        }
                    }
                    else
                    {
                        if (!e.Exist)
                        {
                            list.RemoveAt(0);
                            string tmp = list[0].Word;

                            candidates[candidates.Count - 1] = AnalyzeSingle(e.Word + tmp);
                        }
                        candidates.AddRange(list);
                    }
                }

                int eval = Evaluation(candidates);

                if (results == null || score < eval || (score == eval && results.Count > candidates.Count))
                {
                    results = candidates;
                    score = eval;
                }

                if (eval == 110)
                {
                    break;
                }
            }

            outputs.AddRange(results);

        }

        private int Evaluation(List<CompoundEntry> candidates)
        {

            int eval = 10;

            int one = 0;
            int exist = 0;

            foreach (CompoundEntry entry in candidates)
            {
                if (entry.Word.Length == 1)
                {
                    one++;
                }
                if (entry.Exist)
                {
                    exist++;
                }
            }

            if (one > 3)
            {
                return eval;
            }

            eval = eval + (exist * 100) / candidates.Count - (one * 100) / candidates.Count;

            return eval;
        }

        private bool ContainWord(string before, string input, int pos)
        {

            string prev = null;
            for (int i = pos; i < input.Length; i++)
            {

                string text = before + input.Substring(pos, i + 1 - pos);
                if (DictionaryUtil.FindWithPrefix(text).MoveNext())
                {
                    prev = text;
                    continue;
                }

                if (prev != null && DictionaryUtil.GetNoun(prev) != null)
                {
                    return true;
                }

                break;
            }

            return false;

        }

        //	private void AnalyzeLongText(String input,List outputs, Boolean isFirst) throws MorphException {
        //		
        //		if(exactMach) return;
        //
        //		String[] words = splitByLongestWord(input);
        //		
        //		if(words==null) return;
        //
        //		if(isFirst&&words[0]!=null&&words[0].Length==1&&!DictionaryUtil.ExistPrefix(words[0])) return;
        //		if(words[2]!=null&&words[2].Length==1&&!DictionaryUtil.ExistSuffix(words[2])) return;
        //	
        //		CompoundEntry e1 = null;
        //		List list1 = null;
        //		Boolean success1 = false;
        //		if(words[0]!=null) {
        //			e1 = AnalyzeSingle(words[0]);
        //			if(!e1.isExist()) list1 = Analyze(words[0],false);
        //		}
        //		
        //		CompoundEntry e2 = null;
        //		List list2 = null;
        //		Boolean success2 = false;
        //		if(words[2]!=null) {	
        //			e2 = AnalyzeSingle(words[2]);
        //			if(!e2.isExist()) list2 = Analyze(words[2],false);
        //		}
        //		
        //		if(list1!=null&&list1.Count>0) {
        //			outputs.addAll(list1);
        //		}else if(e1!=null){
        //			outputs.Add(e1);
        //		}
        //		
        //		outputs.Add(AnalyzeSingle(words[1]));
        //		
        //		if(list2!=null&&list2.Count>0) {
        //			outputs.addAll(list2);				
        //		}else if(e2!=null){
        //			outputs.Add(e2);
        //		}
        //	}

        //	private String[] splitByLongestWord(String input) throws MorphException  {
        //		
        //		int pos = 0;
        //		String lngWord = "";
        //		int last = input.Length-1;
        //		Boolean ftry = true;	
        //		
        //		for(int i=0;i<last;i++) {
        //			String ss = input.Substring(i);
        //			String word =lookupWord(ss,ftry);			
        //			if(word!=null&&ValidCompound(word, ss.Substring(word.Length), ftry,i+1)&&lngWord.Length<word.Length) {			
        //				lngWord = word;			
        //				pos = i;
        //				if(i==0) {
        //					ftry = false;
        //				}else {
        //					Matcher m = ALPHANUM_PATTERN.matcher(input.Substring(0,i));
        //					if(!m.find()) {
        //						ftry = false;
        //					}					
        //				}
        //			}			
        //		}
        //		
        //		if("".Equals(lngWord)) return null;
        //		
        //		String[] results = new String[3];
        //		int end = pos+lngWord.Length;		
        //		
        //		if(pos!=0) results[0] = input.Substring(0,pos);
        //		results[1] = lngWord;
        //
        //		if(end<input.Length) results[2] = input.Substring(end);
        //		
        //		return results;
        //	}

        //	private String lookupWord(String text, Boolean ftry) throws MorphException  {
        //		
        //		String word = null;
        //		String prev = null;
        //				
        //		for(int i=2;i<=text.Length;i++) {
        //			
        //			word = text.Substring(0,i);
        //			Iterator prefix = DictionaryUtil.FindWithPrefix(word);
        //			if(prefix.MoveNext()) {
        //				prev = word;
        //				continue;
        //			}
        //			
        //			if(prev==null) return null;
        //			
        //			WordEntry entry = DictionaryUtil.GetWordExceptVerb(prev);
        //			if(entry!=null) {
        //				String str = entry.Word;	
        //				String suffix = text.Substring(i-1,i);				
        //				if(ftry&&str.Length==2&&DictionaryUtil.ExistSuffix(suffix)) return null; // 2湲???묐???寃고빀??留롮쑝誘濡?
        //				return str;
        //			}
        //			
        //			return null;
        //		}
        //		
        //		WordEntry entry = DictionaryUtil.GetWordExceptVerb(text);
        //		if(entry!=null) return entry.Word;
        //		
        //		return null;
        //		
        //	}

        private CompoundEntry[] AnalysisBySplited(int[] units, string input, bool isFirst)
        {

            CompoundEntry[] entries = new CompoundEntry[units.Length];

            int pos = 0;
            string prev = null;

            for (int i = 0; i < units.Length; i++)
            {

                string str = input.Substring(pos, units[i]);

                if (i != 0 && !ValidCompound(prev, str, isFirst && (i == 1), i))
                {
                    return null;
                }

                entries[i] = AnalyzeSingle(str); // CompoundEntry 濡?蹂??

                pos += units[i];
                prev = str;
            }

            return entries;

        }

        private bool CanCompound(CompoundEntry[] entries, int thredhold)
        {

            int achived = 0;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Exist)
                {
                    achived += score;
                }
            }

            if (achived < thredhold)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ?낅젰??String ??CompoundEntry 濡?蹂?? </summary>
        /// <param name="input">
        /// @return </param>
        /// <exception cref="MorphException"> </exception>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private CompoundEntry AnalyzeSingle(String input) throws MorphException
        private CompoundEntry AnalyzeSingle(string input)
        {

            bool success = false;
            int score = AnalysisOutput.SCORE_ANALYSIS;
            int ptn = PatternConstants.PTN_N;
            char pos = PatternConstants.POS_NOUN;
            if (input.Length == 1)
            {
                return new CompoundEntry(input, 0, true, pos);
            }

            WordEntry entry = DictionaryUtil.GetWordExceptVerb(input);
            if (entry != null)
            {
                score = AnalysisOutput.SCORE_CORRECT;
                if (entry.GetFeature(WordEntry.IDX_NOUN) != '1')
                {
                    ptn = PatternConstants.PTN_AID;
                    pos = PatternConstants.POS_AID;
                }
            }

            return new CompoundEntry(input, 0, score == AnalysisOutput.SCORE_CORRECT, pos);

        }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: private Boolean ValidCompound(String before, String after, Boolean isFirst, int pos) throws MorphException
        private bool ValidCompound(string before, string after, bool isFirst, int pos)
        {

            if (pos == 1 && before.Length == 1 && (!isFirst || !DictionaryUtil.ExistPrefix(before)))
            {
                return false;
            }

            if (after.Length == 1 && !DictionaryUtil.ExistSuffix(after))
            {
                return false;
            }

            if (pos != 1 && before.Length == 1)
            {

                WordEntry entry1 = DictionaryUtil.GetUnCompound(before + after);
                if (entry1 != null)
                {
                    List<CompoundEntry> compounds = entry1.Compounds;
                    if (before.Equals(compounds[0].Word) && after.Equals(compounds[1].Word))
                    {
                        return false;
                    }
                }

            }

            WordEntry entry2 = after.Length == 1 ? null : DictionaryUtil.GetUnCompound(after);
            if (entry2 != null)
            {
                List<CompoundEntry> compounds = entry2.Compounds;
                if ("*".Equals(compounds[0].Word) && after.Equals(compounds[1].Word))
                {
                    return false;
                }
            }

            return true;

        }
    }
}
