using System;
using System.Collections;
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

    public class NounUtil
    {
        private static readonly List<string> DNouns = new List<string>();

		static NounUtil()
		{
			string[] strs = new string[]{"등", "들","상","간","뿐","별"};
			foreach (string str in strs)
			{
				DNouns.Add(str);
			}
		}

		/// 
		/// <summary>
		/// 어간부가 음/기 로 끝나는 경우
		/// </summary>
		/// <param name="o"> </param>
		/// <param name="candidates">
		/// @return </param>
		/// <exception cref="MorphException"> </exception>
		  public static bool AnalysisMJ(AnalysisOutput o, List<AnalysisOutput> candidates)
		  {
			   int strlen = o.Stem.Length;

			   if (strlen < 2){ return false; }

			   char[] chrs = MorphUtil.Decompose(o.Stem[strlen - 1]);
			   bool success = false;

			   if (o.Stem[strlen - 1] != '기' && !(chrs.Length == 3 && chrs[2] == 'ㅁ')) return false; 
              
              string start = o.Stem; 
              string end = ""; 
              
              if (o.Stem[strlen - 1] == '기') {
                  start = o.Stem.Substring(0,strlen - 1); 
                  end = "기"; 
              }else if(o.Stem[strlen-1]=='음') { 
                  start = o.Stem.Substring(0,strlen-1); 
                  end = "음";
              } 
              
              String[] eomis = EomiUtil.SplitEomi(start, end); 
              
              if (eomis[0] == null) return false; 
              String[] pomis = EomiUtil.SplitPomi(eomis[0]); 
              o.Stem = pomis[0]; 
              o.AddElist(eomis[1]); 
              o.Pomi = pomis[1]; 
              
              try {
                  if (AnalysisVMJ(o.Clone(),candidates)) return true; 
                  if (AnalysisVMXMJ(o.Clone(),candidates)) return true; 
                  
                  if (AnalysisNSMJ(o.Clone(),candidates)) return true;
              } catch (Exception e) {
                  throw new MorphException(e.Message,e);
              } 
              
              if (DictionaryUtil.GetVerb(o.Stem) != null) {
                  o.Pos = PatternConstants.POS_VERB; 
                  o.Patn = PatternConstants.PTN_VMJ; 
                  o.Score = AnalysisOutput.SCORE_CORRECT; 
                  candidates.Add(o); 
                  return true;
              } 
              
              return false; 
          } 
        
        /// <summary>
        /// 용언 + '음/기' + 조사(PTN_VMXMJ)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static bool AnalysisVMJ(AnalysisOutput o, List<AnalysisOutput> candidates) {
            String[] irrs = IrregularUtil.RestoreIrregularVerb(o.Stem, o.Elist[0]); 
            if (irrs != null) {
                o.Stem = irrs[0]; 
                o.SetElist(irrs[1],0);
            } 
            
            if (DictionaryUtil.GetVerb(o.Stem) != null) {
                o.Patn = PatternConstants.PTN_VMJ; 
                o.Pos = PatternConstants.POS_VERB; 
                o.Score = AnalysisOutput.SCORE_CORRECT; 
                candidates.Add(o); 
                return true;
            } 
            
            return false;
        } 
        
        /// <summary>
        /// 용언 + '아/어' + 보조용언 + '음/기' + 조사(PTN_VMXMJ)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static bool AnalysisVMXMJ(AnalysisOutput o, List<AnalysisOutput> candidates) {
            int idxXVerb = VerbUtil.EndsWithXVerb(o.Stem); 
            if (idxXVerb != -1) { // 2. 사랑받아보다
                string eogan = o.Stem.Substring(0,idxXVerb); 
                o.Xverb = o.Stem.Substring(idxXVerb); 
                String[] stomis = null; 
                
                if (eogan.EndsWith("아")||eogan.EndsWith("어")) 
                    stomis = EomiUtil.SplitEomi(eogan.Substring(0,eogan.Length - 1),eogan.Substring(eogan.Length - 1)); 
                else stomis = EomiUtil.SplitEomi(eogan,""); 
                
                if (stomis[0] == null) return false; 
                
                String[] irrs = IrregularUtil.RestoreIrregularVerb(stomis[0], stomis[1]); 
                
                if (irrs != null) {
                    o.Stem = irrs[0]; 
                    o.AddElist(irrs[1]);
                }else {
                    o.Stem = stomis[0]; 
                    o.AddElist(stomis[1]);
                } 
                
                if (DictionaryUtil.GetVerb(o.Stem) != null) {
                    o.Patn = PatternConstants.PTN_VMXMJ; 
                    o.Pos = PatternConstants.POS_VERB; 
                    o.Score = AnalysisOutput.SCORE_CORRECT; 
                    candidates.Add(o); 
                    return true;
                }else if (AnalysisNSMXMJ(o, candidates)){return true;}
            } 
            
            return false;
        } 
        
        /// <summary>
        /// 체언 + 용언화접미사 + '음/기' + 조사 (PTN_NSMJ)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static bool AnalysisNSMJ(AnalysisOutput o, List<AnalysisOutput> candidates) {
            int idxVbSfix = VerbUtil.EndsWithVerbSuffix(o.Stem); 
            if (idxVbSfix == -1) return false; 
            o.Vsfx = o.Stem.Substring(idxVbSfix); 
            o.Stem = o.Stem.Substring(0,idxVbSfix); 
            o.Patn = PatternConstants.PTN_NSMJ; 
            o.Pos = PatternConstants.POS_NOUN; 
            WordEntry entry = DictionaryUtil.GetWordExceptVerb(o.Stem); 
            
            if (entry != null) {
                if (entry.GetFeature(WordEntry.IDX_NOUN) == '0') 
                    return false; 
                else if (
                    o.Vsfx.Equals("하")&&entry.GetFeature(WordEntry.IDX_DOV)!='1') return false; 
                else if(o.Vsfx.Equals("되") && entry.GetFeature(WordEntry.IDX_BEV) != '1') return false; 
                else if (o.Vsfx.Equals("내")&&entry.GetFeature(WordEntry.IDX_NE)!='1') return false; 
                o.Score = AnalysisOutput.SCORE_CORRECT; 
            }else { 
                o.Score = AnalysisOutput.SCORE_ANALYSIS; 
            } 
            
            candidates.Add(o); 
            
            return true; 
        } 
        
        public static Boolean AnalysisNSMXMJ(AnalysisOutput o, List<AnalysisOutput> candidates) { 
            int idxVbSfix = VerbUtil.EndsWithVerbSuffix(o.Stem); 
            
            if(idxVbSfix==-1) return false; 
            
            o.Vsfx = o.Stem.Substring(idxVbSfix); 
            o.Stem = o.Stem.Substring(0,idxVbSfix); 
            o.Patn = PatternConstants.PTN_NSMXMJ; 
            o.Pos = PatternConstants.POS_NOUN; 
            
            WordEntry entry = DictionaryUtil.GetWordExceptVerb(o.Stem); 
            
            if(entry!=null) { 
                if(entry.GetFeature(WordEntry.IDX_NOUN)=='0') return false; 
                else if(o.Vsfx.Equals("하") && entry.GetFeature(WordEntry.IDX_DOV) != '1') return false; 
                else if (o.Vsfx.Equals("되")&&entry.GetFeature(WordEntry.IDX_BEV)!='1') return false; 
                else if(o.Vsfx.Equals("내") && entry.GetFeature(WordEntry.IDX_NE) != '1') return false; 
                    o.Score = AnalysisOutput.SCORE_CORRECT;
            }else {
                o.Score = AnalysisOutput.SCORE_ANALYSIS;
            } 
            
            candidates.Add(o); 
            return true;
        } 
        
        /// <summary>
        /// 복합명사에서 단위명사를 분리해낸다.
        /// 리스트의 가장 마지막에 위치한 단어가 최장단어이다.
        /// </summary>
        /// <param name="str">복합명사</param>
        /// <param name="pos"></param>
        /// <param name="o">분석결과</param>
        /// <returns>단위명사 리스트</returns>
        private static ArrayList FindNouns(string str, int pos, AnalysisOutput o)
        {
            ArrayList nList = new ArrayList(); 
            
            if (str.Length == 2 && DictionaryUtil.ExistSuffix(str.Substring(0,1)) && DNouns.Contains(str.Substring(1))) {
                o.Stem = o.Stem.Substring(0,o.Stem.Length - 1); 
                o.Nsfx = str.Substring(1); 
                nList.Add(new WordEntry(str.Substring(0,1))); 
                return nList;
            }else if (str.Length == 2 && DictionaryUtil.ExistSuffix(str.Substring(0,1)) && DictionaryUtil.ExistJosa(str.Substring(1))) {
                return null;
            } 
            
            if (pos >= 2 && DictionaryUtil.ExistJosa(str)) return null; 
            if (str.Length == 1 && (DictionaryUtil.ExistSuffix(str) || DNouns.Contains(str))) {
                nList.Add(new WordEntry(str)); return nList;
            } 
            
            for (int i = 1;i < str.Length;i++) {
                string sub = str.Substring(0,i + 1); 
                
                if (!DictionaryUtil.FindWithPrefix(sub).MoveNext()) break; 
                WordEntry entry = DictionaryUtil.GetCNoun(sub); 
                if (entry != null) {
                    nList.Add(entry);
                }
            } 
            
            return nList;
        } 
        
        /// <summary>
        /// 마지막 음절이 명사형 접미사(등,상..)인지 조사한다.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool ConfirmDNoun(AnalysisOutput output) {
            int strlen = output.Stem.Length; 
            string d = output.Stem.Substring(strlen - 1); 
            
            if (!DNouns.Contains(d)) return false; 
            
            string s = output.Stem.Substring(0, strlen - 1); 
            output.Nsfx = d; 
            output.Stem = s; 
            WordEntry cnoun = DictionaryUtil.GetCNoun(s); 
            
            if (cnoun != null) {
                if (cnoun.GetFeature(WordEntry.IDX_NOUN) == '2') 
                    output.CNoun = cnoun.Compounds; 
                else 
                    output.CNoun = new List<CompoundEntry>();
                output.Score = AnalysisOutput.SCORE_CORRECT;
            } 
            
            return true;
        } 
        
        public static bool EndsWith2Josa(string input) {
            bool josaFlag = true; 
            
            for (int i = input.Length - 2;i>0;i--) {
                string josa = input.Substring(i); 
                char[] feature = SyllableUtil.GetFeature(josa[0]); 
                if (josaFlag && DictionaryUtil.ExistJosa(josa)) return true; 
                if (josaFlag && feature[SyllableUtil.IDX_JOSA2] == '0') josaFlag = false; 
                if (!josaFlag) break;} return false;
        } 
        
        public static double CountFoundNouns(AnalysisOutput o) {
            int count = 0; 
            
            for (int i = 0;i < o.CNounList.Count;i++) {
                if (o.CNounList[i].Exist) count++;
            }

            return (count * 100) / o.CNounList.Count;
        } 
     }
}
