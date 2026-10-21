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
    
    public class MorphAnalyzer
    {
		/// <summary>
		/// starting word of sentence.
		/// </summary>
		public const int POS_START = 1;

		/// <summary>
		/// middle word of sentence
		/// </summary>
		public const int POS_MID = 2;

		/// <summary>
		/// ending word of sentence.
		/// </summary>
		public const int POS_END = 3;

		private CompoundNounAnalyzer cnAnalyzer = new CompoundNounAnalyzer();

		public MorphAnalyzer()
		{
			cnAnalyzer.ExactMach = false;
		}

		public virtual bool ExactCompound
		{
			set
			{
				cnAnalyzer.ExactMach = value;
			}
		}

        public virtual List<AnalysisOutput> Analyze(string input)
		{

			if (input.EndsWith("."))
			{
				return Analyze(input.Substring(0,input.Length - 1), POS_END);
			}

			return Analyze(input, POS_MID);
		}

		/// 
		/// <param name="input"> </param>
		/// <param name="pos">
		/// @return </param>
		/// <exception cref="MorphException"> </exception>
        public virtual List<AnalysisOutput> Analyze(string input, int pos)
		{

            List<AnalysisOutput> candidates = new List<AnalysisOutput>();
			bool isVerbOnly = MorphUtil.HasVerbOnly(input);

			AnalysisByRule(input, candidates);

			if (!isVerbOnly || candidates.Count == 0)
			{
				AddSingleWord(input,candidates);
			}

			candidates.Sort();

			// 복합명사 분해여부 결정하여 분해
			bool changed = false;
			bool correct = false;
			foreach (AnalysisOutput o in candidates)
			{
	//		System.out.println(o);
				if (o.Score == AnalysisOutput.SCORE_CORRECT)
				{
					if (o.Patn != PatternConstants.PTN_NJ)
					{
						correct = true;
					}
					// "활성화해"가 [활성화(N),하(t),어야(e)] 분석성공하였는데 [활성/화해]분해되는 것을 방지
					if (o.Patn == PatternConstants.PTN_NSM)
					{
						break;
					}
					continue;
				}

				if (o.Patn < PatternConstants.PTN_VM && o.Stem.Length>2)
				{
					 if (!(correct && o.Patn == PatternConstants.PTN_N))
					 {
						 ConfirmCNoun(o);
					 }
					 if (o.Score == AnalysisOutput.SCORE_CORRECT)
					 {
						 changed = true;
					 }
				}

			}

			if (changed)
			{
				candidates.Sort();
			}

			List<AnalysisOutput> results = new List<AnalysisOutput>();

			bool hasCorrect = false;
			bool hasCorrectNoun = false;
			bool correctCnoun = false;

            Dictionary<string, AnalysisOutput> stems = new Dictionary<string, AnalysisOutput>();
			AnalysisOutput noun = null;

			double ratio = 0;
			AnalysisOutput compound = null;

			foreach (AnalysisOutput o in candidates)
			{
	//System.out.println("analyze-->"+o.ToString());
				if (o.Score == AnalysisOutput.SCORE_FAIL) // 분석에는 성공했으나, 제약조건에 실패
				{
					continue;
				}
				if (o.Score == AnalysisOutput.SCORE_CORRECT && o.Pos != PatternConstants.POS_NOUN)
				{
					AddResults(o,results,stems);
					hasCorrect = true;
				}
				else if (o.Pos == PatternConstants.POS_NOUN && o.Score == AnalysisOutput.SCORE_CORRECT)
				{

					if ((hasCorrect || correctCnoun) && o.CNounList.Count > 0)
					{
						continue;
					}

					if (o.Pos == PatternConstants.POS_NOUN)
					{
						AddResults(o,results,stems);
					}
					else if (noun == null)
					{
						AddResults(o,results,stems);
						noun = o;
					}
					else if (o.Patn == PatternConstants.PTN_N || (o.Patn > noun.Patn) || (o.Patn == noun.Patn && o.Josa != null && noun.Josa != null && o.Josa.Length > noun.Josa.Length))
					{
						results.Remove(noun);
						AddResults(o,results,stems);
						noun = o;
					}
					hasCorrectNoun = true;
	//				if(o.getCNounList().Count>0) correctCnoun = true;
				}
                else if (o.Pos == PatternConstants.POS_NOUN && o.CNounList.Count > 0 && !hasCorrect && !hasCorrectNoun)
				{
					double curatio = NounUtil.CountFoundNouns(o);
					if (ratio < curatio && (compound == null || (compound != null && compound.Josa == null)))
					{
						ratio = curatio;
						compound = o;
					}
				}
				else if (o.Pos == PatternConstants.POS_NOUN && !hasCorrect && !hasCorrectNoun && compound == null)
				{
					AddResults(o,results,stems);
				}
				else if (o.Patn == PatternConstants.PTN_NSM)
				{
					AddResults(o,results,stems);
				}
			}

			if (compound != null)
			{
				AddResults(compound,results,stems);
			}

			if (results.Count == 0)
			{
				AnalysisOutput output = new AnalysisOutput(input, null, null, PatternConstants.PTN_N, AnalysisOutput.SCORE_ANALYSIS);
				output.Pos = PatternConstants.POS_NOUN;
				results.Add(output);
			}

			return results;
		}


        private void AnalysisByRule(string input, List<AnalysisOutput> candidates)
		{

			bool josaFlag = true;
			bool eomiFlag = true;

			int strlen = input.Length;

	//		Boolean isVerbOnly = MorphUtil.HasVerbOnly(input);
			bool isVerbOnly = false;
			AnalysisWithEomi(input,"",candidates);

			for (int i = strlen - 1;i > 0;i--)
			{

				string stem = input.Substring(0,i);
				string eomi = input.Substring(i);

				char[] feature = SyllableUtil.GetFeature(eomi[0]);
				if (!isVerbOnly && josaFlag && feature[SyllableUtil.IDX_JOSA1] == '1')
				{
					AnalysisWithJosa(stem,eomi,candidates);
				}

				if (eomiFlag)
				{
					AnalysisWithEomi(stem,eomi,candidates);
				}

				if (josaFlag && feature[SyllableUtil.IDX_JOSA2] == '0')
				{
					josaFlag = false;
				}
				if (eomiFlag && feature[SyllableUtil.IDX_EOMI2] == '0')
				{
					eomiFlag = false;
				}

				if (!josaFlag && !eomiFlag)
				{
					break;
				}
			}
		}

        private void AddResults(AnalysisOutput o, List<AnalysisOutput> results, Dictionary<string, AnalysisOutput> stems)
		{
			AnalysisOutput old = stems[o.Stem];
			if (old == null || old.Pos != o.Pos)
			{
				results.Add(o);
				stems[o.Stem] = o;
			}
			else if (old.Patn < o.Patn)
			{
				results.Remove(old);
				results.Add(o);
				stems[o.Stem] = o;
			}
		}

		private void AddSingleWord(string word, List<AnalysisOutput> candidates)
		{

	//		if(candidates.Count!=0&&candidates.Get(0).Score==AnalysisOutput.SCORE_CORRECT) return;

			AnalysisOutput output = new AnalysisOutput(word, null, null, PatternConstants.PTN_N);
			output.Pos = PatternConstants.POS_NOUN;

			WordEntry entry;
			if ((entry = DictionaryUtil.GetWord(word)) != null)
			{

				if (entry.GetFeature(WordEntry.IDX_NOUN) != '1' && entry.GetFeature(WordEntry.IDX_BUSA) == '1')
				{
					AnalysisOutput busa = new AnalysisOutput(word, null, null, PatternConstants.PTN_AID);
					busa.Pos = PatternConstants.POS_ETC;

					busa.Score = AnalysisOutput.SCORE_CORRECT;
					candidates.Insert(0,busa);
				}
				else if (entry.GetFeature(WordEntry.IDX_NOUN) == '1')
				{
					output.Score = AnalysisOutput.SCORE_CORRECT;
					candidates.Insert(0,output);
				}
				else if (entry.GetFeature(WordEntry.IDX_NOUN) == '2')
				{
					candidates.Insert(0,output);
				}

				if (entry.GetFeature(WordEntry.IDX_VERB) != '1')
				{
					return;
				}
			}
			else if (candidates.Count == 0 || !NounUtil.EndsWith2Josa(word))
			{
				output.Score = AnalysisOutput.SCORE_ANALYSIS;
				candidates.Add(output);
			}
		}

		/// <summary>
		/// 체언 + 조사 (PTN_NJ)
        /// 체언 + 용언화접미사 + '음/기' + 조사 (PTN_NSMJ
        /// 용언 + '음/기' + 조사 (PTN_VMJ)
        /// 용언 + '아/어' + 보조용언 + '음/기' + 조사(PTN_VMXMJ)
		/// </summary>
		/// <param name="stem"> </param>
		/// <param name="end"> </param>
		/// <param name="candidates"> </param>
		/// <exception cref="MorphException"> </exception>
        public virtual void AnalysisWithJosa(string stem, string end, List<AnalysisOutput> candidates)
		{

			if (stem == null || stem.Length == 0)
			{
				return;
			}

			char[] chrs = MorphUtil.Decompose(stem[stem.Length - 1]);
			if (!DictionaryUtil.ExistJosa(end) || (chrs.Length == 3 && ConstraintUtil.IsTwoJosa(end)) || (chrs.Length == 2 && (ConstraintUtil.IsThreeJosa(end)) || "".Equals(end))) // ?곌껐??媛?ν븳 議곗궗媛 ?꾨땲硫?..
			{
				return;
			}

			AnalysisOutput output = new AnalysisOutput(stem, end, null, PatternConstants.PTN_NJ);
			output.Pos = PatternConstants.POS_NOUN;

			bool success = false;
			try
			{
				success = NounUtil.AnalysisMJ(output.Clone(), candidates);
			}
			catch (Exception e) //CloneNotSupportedException e)
			{
				throw new MorphException(e.Message,e);
			}

			WordEntry entry = DictionaryUtil.GetWordExceptVerb(stem);
			if (entry != null)
			{
				output.Score = AnalysisOutput.SCORE_CORRECT;
				if (entry.GetFeature(WordEntry.IDX_NOUN) == '0' && entry.GetFeature(WordEntry.IDX_BUSA) == '1')
				{
					output.Pos = PatternConstants.POS_ETC;
					output.Patn = PatternConstants.PTN_ADVJ;
				}
			}
			else
			{
				if (MorphUtil.HasVerbOnly(stem))
				{
					return;
				}
			}

			candidates.Add(output);

		}

		/// 
		/// <summary>
		///  1. 사랑받다 : 체언 + 용언화접미사 + 어미 (PTN_NSM) 
        ///  2. 사랑받아보다 : 체언 + 용언화접미사 + '아/어' + 보조용언 + 어미 (PTN_NSMXM) 
        ///  3. 학교에서이다 : 체언 + '에서/부터/에서부터' + '이' + 어미 (PTN_NJCM) 
        ///  4. 돕다 : 용언 + 어미 (PTN_VM) 
        ///  5. 도움이다 : 용언 + '음/기' + '이' + 어미 (PTN_VMCM) 
        ///  6. 도와주다 : 용언 + '아/어' + 보조용언 + 어미 (PTN_VMXM) <br>
		/// </summary>
		/// <param name="stem"> </param>
		/// <param name="end"> </param>
		/// <param name="candidates"> </param>
		/// <exception cref="CloneNotSupportedException">  </exception>
        public virtual void AnalysisWithEomi(string stem, string end, List<AnalysisOutput> candidates)
		{

			string[] morphs = EomiUtil.SplitEomi(stem, end);
			if (morphs[0] == null) // 어미가 사전에 등록되어 있지 않다면....
			{
				return;
			}

			string[] pomis = EomiUtil.SplitPomi(morphs[0]);

			AnalysisOutput o = new AnalysisOutput(pomis[0],null,morphs[1],PatternConstants.PTN_VM);
			o.Pomi = pomis[1];

			try
			{

				WordEntry entry = DictionaryUtil.GetVerb(o.Stem);
				if (entry != null && !("을".Equals(end)&&entry.GetFeature(WordEntry.IDX_REGURA)==IrregularUtil.IRR_TYPE_LIUL)) {	
                    AnalysisOutput output = o.Clone(); 
                    output.Score = AnalysisOutput.SCORE_CORRECT; 
                    MorphUtil.BuildPtnVM(output, candidates); 
                    char[] features = SyllableUtil.GetFeature(stem[stem.Length-1]); // ㄹ불규칙일 경우
                    
                    if(features[SyllableUtil.IDX_YNPLN]=='0'||morphs[1][0]!='ㄴ')     
                        return; 
                } 
                
                String[] irrs = IrregularUtil.RestoreIrregularVerb(o.Stem, o.Pomi==null?o.Eomi:o.Pomi); 
                if(irrs!=null) { 
                    AnalysisOutput output = o.Clone(); 
                    output.Stem = irrs[0]; 
                    
                    if(output.Pomi==null) 
                        output.Eomi = irrs[1]; 
                    else 
                        output.Pomi = irrs[1]; 
                    output.Score = AnalysisOutput.SCORE_CORRECT; 
                    MorphUtil.BuildPtnVM(output, candidates); 
                } 
                
                if(VerbUtil.AnanlysisNSM(o.Clone(), candidates)) return; 
                if(VerbUtil.AnanlysisNSMXM(o.Clone(), candidates)) return; 
                if(VerbUtil.AnanlysisNJCM(o.Clone(),candidates)) return; 
                if(VerbUtil.AnalysisVMCM(o.Clone(),candidates)) return; 
                VerbUtil.AnalysisVMXM(o.Clone(), candidates); 
            } catch (Exception e) {//CloneNotSupportedException e) { 
                throw new MorphException(e.Message,e); 
            } 
        } 
        
        public void AnalysisCNoun(List<AnalysisOutput> candidates) { 
            Boolean success = false; 
            
            foreach(AnalysisOutput o in candidates) { 
                if(o.Pos!=PatternConstants.POS_NOUN) continue; 
                if(o.Score==AnalysisOutput.SCORE_CORRECT) 
                    success=true; 
                else if(!success) ConfirmCNoun(o); 
            } 
        } 
        
        /// <summary>
        /// 복합명사인지 조사하고, 복합명사이면 단위명사들을 찾는다.\
        /// 복합명사인지 여부는 단위명사가 모두 사전에 있는지 여부로 판단한다.
        /// 단위명사는 2글자 이상 단어에서만 찾는다.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public Boolean ConfirmCNoun(AnalysisOutput o) { 
            if(o.Stem.Length<3) return false; 
            
            WordEntry cnoun = DictionaryUtil.GetCNoun(o.Stem); 
            
            if(cnoun!=null && cnoun.GetFeature(WordEntry.IDX_NOUN)=='2') { 
                o.AddCNoun(cnoun.Compounds);
                o.Score = AnalysisOutput.SCORE_CORRECT; return true; 
            } 
            
            List<CompoundEntry> results = cnAnalyzer.Analyze(o.Stem); 
            Boolean success = false; 
            
            if(results.Count>1) { 
                o.CNoun = results; 
                success = true; 
                
                foreach(CompoundEntry entry in results) { 
                    if(!entry.Exist) success = false; 
                } 
                
                o.Score = AnalysisOutput.SCORE_COMPOUNDS; 
            } 
            
            if(success) { 
                if(Constraint(o)) { 
                    o.Score = AnalysisOutput.SCORE_CORRECT; 
                } else { 
                    o.Score = AnalysisOutput.SCORE_FAIL; 
                    return false; 
                } 
            } else { 
                if(NounUtil.ConfirmDNoun(o)&&o.Score!=AnalysisOutput.SCORE_CORRECT) { 
                    ConfirmCNoun(o); 
                } 
                
                if(o.Score==AnalysisOutput.SCORE_CORRECT) success = true; 
                if(o.CNounList.Count>0&&!Constraint(o)) o.Score = AnalysisOutput.SCORE_FAIL; 
            } 
            
            return success; 
        } 
        
        private Boolean Constraint(AnalysisOutput o) { 
            List<CompoundEntry> cnouns = o.CNounList; 
            
            if("화해".Equals(cnouns[cnouns.Count-1].Word)) { 
                if(!ConstraintUtil.CanHaheCompound(cnouns[cnouns.Count-2].Word)) 
                    return false; 
            }else if(o.Patn==PatternConstants.PTN_NSM) { 
                if("내".Equals(o.Vsfx) && cnouns[cnouns.Count - 1].Word.Length != 1) {
                    WordEntry entry = DictionaryUtil.GetWord(cnouns[cnouns.Count - 1].Word); 
                    if (entry != null && entry.GetFeature(WordEntry.IDX_NE) == '0') 
                        return false;
                }else if ("하".Equals(o.Vsfx)&&cnouns[cnouns.Count-1].Word.Length==1) { 
                    // 짝사랑하다 와 같은 경우에 뒷글자가 1글자이면 제외
                    return false; 
                } 
            } return true; 
        } 
    }
	
}
