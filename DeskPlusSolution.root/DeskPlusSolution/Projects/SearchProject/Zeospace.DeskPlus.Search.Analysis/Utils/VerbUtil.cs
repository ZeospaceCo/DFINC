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
    
    public class VerbUtil
    {
		public static readonly IDictionary verbSuffix_Renamed = new Hashtable();

		public static readonly IDictionary XVerb = new Hashtable();

		static VerbUtil()
		{
			string[] suffixs = {"이","하","되","내", "나", "스럽","시키","있","없","같","당하","만하","드리","받","짓"};
			for (int i = 0;i < suffixs.Length;i++)
			{
				verbSuffix_Renamed[suffixs[i]] = suffixs[i];
			}

			string[] xverbs = {"오","내","주","보","지","오르","올리"};
			for (int i = 0;i < xverbs.Length;i++)
			{
				XVerb[xverbs[i]] = xverbs[i];
			}
		}

		/// <summary>어간이 용언화접미사로 끝나면 index 를 반환한다.  아니면 -1을 반환한다.</summary>
		/// <param name="result">
		/// @return </param>
	   public static int EndsWithVerbSuffix(string stem)
	   {
			int len = stem.Length;
			if (len < 2)
			{
				return -1;
			}
			int start = 2;
			if (len == 2)
			{
				start = 1;
			}
			for (int i = start;i > 0;i--) // suffix 의 가장 긴 글자수가 2이다.
			{
				if (verbSuffix_Renamed[stem.Substring(len - i)] != null)
				{
					return (len - i);
				}
			}
			return -1;
	   }

	   /// <summary>어간부에 보조용언 [하,되,오,내,주,지]가 있는지 조사한다.</summary>
	   /// <param name="stem">
	   /// @return </param>
	   public static int EndsWithXVerb(string stem)
	   {
			int len = stem.Length;
			if (len < 2)
			{
				return -1;
			}
			int start = 2;
			if (len == 2)
			{
				start = 1;
			}
			for (int i = start;i > 0;i--) //xverbs 의 가장 긴 글자수는 2이다.
			{
				if (XVerb[stem.Substring(len - i)] != null)
				{
					return (len - i);
				}
			}
		   return -1;
	   }

	   public static bool VerbSuffix(string stem)
	   {

		   return verbSuffix_Renamed[stem] != null;

	   }

	   public static bool ConstraintVerb(string start, string end)
	   {

		   char[] schs = MorphUtil.Decompose(start[start.Length - 1]);
		   char[] echs = MorphUtil.Decompose(end[0]);

		   if (schs.Length == 3 && schs[2] == 'ㄹ' && echs[0] == 'ㄹ') return false; 
           return true; 
       } 
       
        /// <summary>
        /// 3. 학교에서이다 : 체언 + '에서/부터/에서부터' + '이' + 어미 (PTN_NJCM) <br>
        /// </summary>
        /// <param name="o"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static bool AnanlysisNJCM(AnalysisOutput o, List<AnalysisOutput> candidates){
            int strlen = o.Stem.Length; 
            bool success = false; 
            
            if (strlen > 3 && (o.Stem.EndsWith("에서이")||o.Stem.EndsWith("부터이"))) { 
                o.AddElist(o.Stem.Substring(strlen-1)); 
                o.Josa = o.Stem.Substring(strlen-3,strlen-1); 
                o.Stem = o.Stem.Substring(0,strlen-3); 
                success = true; 
            }else if(strlen>5&&(o.Stem.EndsWith("에서부터이"))) { 
                o.AddElist(o.Stem.Substring(strlen-1)); 
                o.Josa = o.Stem.Substring(strlen-5,strlen-1); 
                o.Stem = o.Stem.Substring(0,strlen-5); 
                success = true; 
            } 
            
            if(!success) return false; 
            if(success&&DictionaryUtil.GetNoun(o.Stem)!=null) { 
                o.Score = AnalysisOutput.SCORE_CORRECT; 
            } 
            
            o.Patn = PatternConstants.PTN_NJCM; 
            o.Pos = PatternConstants.POS_NOUN; 
            candidates.Add(o); 
            return true; 
        } 
        
        /// <summary>
        /// 어미부와 어간부가 분리된 상태에서 용언화접미사가 결합될 수 있는지 조사한다.
        /// </summary>
        /// <param name="o">어미부와 어간부가 분리된 결과</param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static Boolean AnanlysisNSM(AnalysisOutput o, List<AnalysisOutput> candidates) { 
            if(o.Stem.EndsWith("스러우")) o.Stem = o.Stem.Substring(0,o.Stem.Length - 3) + "스럽"; 
            int idxVbSfix = VerbUtil.EndsWithVerbSuffix(o.Stem); 
            
            if (idxVbSfix < 1) return false; 
            o.Vsfx = o.Stem.Substring(idxVbSfix); 
            o.Stem = o.Stem.Substring(0,idxVbSfix); 
            o.Patn = PatternConstants.PTN_NSM; 
            o.Pos = PatternConstants.POS_NOUN; 
            
            WordEntry entry = DictionaryUtil.GetWordExceptVerb(o.Stem); 
            if (entry != null) {
                if (entry.GetFeature(WordEntry.IDX_NOUN) == '0') 
                    return false; 
                else if (o.Vsfx.Equals("하")&&entry.GetFeature(WordEntry.IDX_DOV)!='1') return false; 
                else if(o.Vsfx.Equals("되") && entry.GetFeature(WordEntry.IDX_BEV) != '1') return false; 
                else if (o.Vsfx.Equals("내")&&entry.GetFeature(WordEntry.IDX_NE)!='1') return false; 
                o.Score = AnalysisOutput.SCORE_CORRECT;  // '입니다'인 경우 인명 등 미등록어가 많이 발생되므로 분석성공으로 가정한다.	
            }else { 
                o.Score = AnalysisOutput.SCORE_ANALYSIS; // '입니다'인 경우 인명 등 미등록어가 많이 발생되므로 분석성공으로 가정한다.
            } 
          
            candidates.Add(o); 
            return true; 
        } 
        
        public static Boolean AnanlysisNSMXM(AnalysisOutput o, List<AnalysisOutput> candidates) { 
            int idxXVerb = VerbUtil.EndsWithXVerb(o.Stem); 
            if(idxXVerb==-1) return false; 
            String eogan = o.Stem.Substring(0,idxXVerb); 
            String[] stomis = null; 
            if((eogan.EndsWith("아") || eogan.EndsWith("어"))&&eogan.Length>1) 
                stomis = EomiUtil.SplitEomi(eogan.Substring(0,eogan.Length-1),eogan.Substring(eogan.Length-1)); 
            else 
                stomis = EomiUtil.SplitEomi(eogan,""); 
            if(stomis[0]==null) return false; 
            o.AddElist(stomis[1]); 
            int idxVbSfix = VerbUtil.EndsWithVerbSuffix(stomis[0]); 
            
            if(idxVbSfix==-1) return false; 
            o.Xverb = o.Stem.Substring(idxXVerb); 
            o.Vsfx = stomis[0].Substring(idxVbSfix); 
            o.Stem = stomis[0].Substring(0,idxVbSfix); 
            o.Patn = PatternConstants.PTN_NSMXM; 
            o.Pos = PatternConstants.POS_NOUN; 
            
            WordEntry entry = DictionaryUtil.GetNoun(o.Stem); 
            
            if(entry==null) return false; 
            
            if(o.Vsfx.Equals("하") && entry.GetFeature(WordEntry.IDX_DOV) != '1') return false; 
            if(o.Vsfx.Equals("되")&&entry.GetFeature(WordEntry.IDX_BEV)!='1') return false; 
            o.Score = AnalysisOutput.SCORE_CORRECT; 
            
            candidates.Add(o); 
            
            return true; 
        } 
        
        public static Boolean AnalysisVMCM(AnalysisOutput o, List<AnalysisOutput> candidates) { 
            int strlen = o.Stem.Length; 
            
            if(strlen<2) return false; 
            
            if(!o.Stem.EndsWith("이")) return false; 
            
            char[] chrs = MorphUtil.Decompose(o.Stem[strlen - 2]); 
            
            bool success = false; 
            
            if (strlen>2 && o.Stem.EndsWith("기이")) {
                o.Stem = o.Stem.Substring(0,strlen - 2); 
                o.AddElist("기"); 
                
                success = true; 
            } else if(chrs.Length>2&&chrs[2]=='ㅁ'){ 
                String[] eres = EomiUtil.SplitEomi(o.Stem.Substring(0,strlen-1), ""); 
                if(eres[0]==null) return false; 
                
                o.AddElist(eres[1]); 
                String[] irrs = IrregularUtil.RestoreIrregularVerb(eres[0], eres[1]); 
                
                if(irrs!=null) 
                    o.Stem = irrs[0]; 
                else 
                    o.Stem = eres[0]; 
                success = true; 
            } if(success) { 
                o.AddElist("이"); 
                if (DictionaryUtil.GetVerb(o.Stem) != null) {
                    o.Pos = PatternConstants.POS_VERB; 
                    o.Patn = PatternConstants.PTN_VMCM; 
                    o.Score = AnalysisOutput.SCORE_CORRECT; 
                    candidates.Add(o); 
                    return true;
                }
            } 
            
            return false;
        } 
        
        /// <summary>
        /// 6. 도와주다 : 용언 + '아/어' + 보조용언 + 어미 (PTN_VMXM)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static bool AnalysisVMXM(AnalysisOutput o, List<AnalysisOutput> candidates) {
            int idxXVerb = VerbUtil.EndsWithXVerb(o.Stem); 
            
            if (idxXVerb == -1) return false; 
            
            o.Xverb = o.Stem.Substring(idxXVerb); 
            
            string eogan = o.Stem.Substring(0,idxXVerb); 
            String[] stomis = null; 
            
            if (eogan.EndsWith("아")||eogan.EndsWith("어")) {
                stomis = EomiUtil.SplitEomi(eogan.Substring(0,eogan.Length - 1),eogan.Substring(eogan.Length - 1)); 
                if (stomis[0] == null) return false;
            }else {stomis = EomiUtil.SplitEomi(eogan, ""); 
                if (stomis[0] == null || !(stomis[1].StartsWith("아")||stomis[1].StartsWith("어"))) return false;
            } 
            
            String[] irrs = IrregularUtil.RestoreIrregularVerb(stomis[0], stomis[1]); 
            
            if (irrs != null) {o.Stem = irrs[0]; 
                o.AddElist(irrs[1]);
            } else {
                o.Stem = stomis[0]; 
                o.AddElist(stomis[1]);
            } 
            
            if (DictionaryUtil.GetVerb(o.Stem) != null) {
                o.Pos = PatternConstants.POS_VERB; 
                o.Patn = PatternConstants.PTN_VMXM; 
                o.Score = AnalysisOutput.SCORE_CORRECT; 
                candidates.Add(o); 
                return true;
            } 
            
            return false;
        }
    }
}
