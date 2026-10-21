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
    
    public class EomiUtil
    {
        public const string RESULT_FAIL = "0";
        public const string RESULT_SUCCESS = "1";

        public static readonly string[] verbSuffix = { "이", "하", "되", "스럽", "스러우", "시키", "있", "없", "같", "당하", "만하", "드리", "받", "나", "내" };

        /// <summary>
        /// 가장 길이가 긴 어미를 분리한다.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static string[] LongestEomi(string term)
        {
			string[] result = new string[2];
			result[0] = term;

			string stem;
			string eomi;
			char[] efeature;

			for (int i = term.Length;i > 0;i--)
			{

				stem = term.Substring(0,i);

				if (i != term.Length)
				{
					eomi = term.Substring(i);
					efeature = SyllableUtil.GetFeature(eomi[0]);
				}
				else
				{
					efeature = SyllableUtil.GetFeature(stem[i - 1]);
					eomi = "";
				}

				if (SyllableUtil.IsAlpanumeric(stem[i - 1]))
				{
					break;
				}

				char[] jasos = MorphUtil.Decompose(stem[i - 1]);

				if (!"".Equals(eomi) && !DictionaryUtil.ExistEomi(eomi))
				{
					// do not anything.
				}
				else if (jasos.Length > 2 && (jasos[2] == 'ㄴ' || jasos[2] == 'ㄹ' || jasos[2] == 'ㅁ' || jasos[2] == 'ㅂ') && DictionaryUtil.CombineAndEomiCheck(jasos[2], eomi) != null) 
                {
                    result[0] = char.ToString(MorphUtil.MakeChar(stem[i - 1], 0)); 
                    if (i != 0) result[0] = stem.Substring(0,i - 1) + result[0]; 
                    result[1] = char.ToString(jasos[2]);
                }else if (i > 0 && (stem.EndsWith("하")&&"여".Equals(eomi)) || (stem.EndsWith("가") && "거라".Equals(eomi)) || (stem.EndsWith("오") && "너라".Equals(eomi))) { 
                    result[0] = stem; result[1] = eomi; 
                }else if(jasos.Length==2&&(!stem.EndsWith("아") && !stem.EndsWith("어"))&& (jasos[1]=='ㅏ'||jasos[1]=='ㅓ'||jasos[1]=='ㅔ'||jasos[1]=='ㅐ')&& (DictionaryUtil.CombineAndEomiCheck('어', eomi)!=null)) { 
                    char[] chs = MorphUtil.Decompose(stem[stem.Length-1]); 
                    result[0] = stem; 
                    result[1] = "어" + eomi; 
                }else if ((jasos[1] == 'ㅘ' || jasos[1] == 'ㅝ' || jasos[1] == 'ㅕ' || jasos[1] == 'ㅐ' || jasos[1] == 'ㅒ') && (DictionaryUtil.CombineAndEomiCheck('어', eomi) != null)) { 
                    string end = ""; 
                    if (jasos[1] == 'ㅘ') 
                        end = MorphUtil.MakeChar(stem[i - 1], 8, 0) + "아"; 
                    else if(jasos[1]=='ㅝ') 
                        end=MorphUtil.MakeChar(stem[i-1], 13, 0)+"어"; 
                    else if (jasos[1] == 'ㅕ') 
                        end = char.ToString(MorphUtil.MakeChar(stem[i - 1], 6, 0)); 
                    else if (jasos[1] == 'ㅐ') 
                        end = MorphUtil.MakeChar(stem[i - 1], 0, 0) + "어"; 
                    else if(jasos[1]=='ㅒ') 
                        end=MorphUtil.MakeChar(stem[i-1], 20, 0)+"애"; 
                    if (jasos.Length == 3) { 
                        end = end.Substring(0,end.Length - 1) + MorphUtil.ReplaceJongsung(end[end.Length - 1],stem[i - 1]); 
                    } 
                    if (stem.Length < 2) result[0] = end; else result[0] = stem.Substring(0,stem.Length - 1) + end; result[1] = eomi; 
                }else if (efeature != null && efeature[SyllableUtil.IDX_EOMI1] != '0' && DictionaryUtil.ExistEomi(eomi)) { 
                    if (!(((jasos.Length == 2 && jasos[0] == 'ㄹ') || (jasos.Length == 3 && jasos[2] == 'ㄹ')) && eomi.Equals("러"))) { // ㄹ 불규칙은 예외 
                        result[0] = stem; 
                        result[1] = eomi; 
                    } 
                } 
                
                if(efeature!=null&&efeature[SyllableUtil.IDX_EOMI2]=='0') break; 
            } 
            
            return result; 
        } 
        
        /// <summary>
        /// 선어말어미를 분석한다
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static String[] SplitPomi(String stem){ 
            //results[0]:성공(1)/실패(0), results[1]: 어근, results[2]: 선어말어미
            String[] results = new String[2]; 
            results[0] = stem; 
            
            if(stem==null||stem.Length==0||"있".Equals(stem)) return results; 
            
            char[] chrs = stem.ToCharArray(); 
            int len = chrs.Length; 
            string pomi = ""; 
            int index = len - 1; 
            char[] jaso = MorphUtil.Decompose(chrs[index]); 
            
            if (chrs[index] != '시' && chrs[index] != 'ㅆ' && jaso[jaso.Length - 1] != 'ㅆ') return results; // 선어말어미가 발견되지 않았다
            
            if (chrs[index] == '겠') { 
                pomi = "겠"; 
                SetPomiResult(results,stem.Substring(0,index),pomi); 
                
                if(--index<=0|| (chrs[index]!='시'&&chrs[index]!='ㅆ'&&jaso[jaso.Length-1]!='ㅆ')) 
                    return results; // 다음이거나 선어말어미가 없다면...
                
                jaso = MorphUtil.Decompose(chrs[index]); 
            } 
            
            if(chrs[index]=='었') { // 시었, ㅆ었, 었
                pomi = chrs[index]+pomi; 
                SetPomiResult(results,stem.Substring(0,index),pomi); 
                if(--index<=0|| (chrs[index]!='시'&&chrs[index]!='ㅆ'&&jaso[jaso.Length-1]!='ㅆ')) 
                    return results; 
                jaso = MorphUtil.Decompose(chrs[index]); 
            } 
            
            if(chrs[index]=='였'){ 
                pomi = MorphUtil.ReplaceJongsung('어',chrs[index])+pomi; 
                if(index>0&&chrs[index-1]=='하') 
                    stem = stem.Substring(0,index); 
                else 
                    stem = stem.Substring(0,index)+"이"; 
                SetPomiResult(results,stem,pomi); 
            }else if (chrs[index] == '셨'){ 
                pomi = MorphUtil.ReplaceJongsung('어',chrs[index]) + pomi; 
                stem = stem.Substring(0,index); 
                SetPomiResult(results,stem,"시"+pomi); 
            }else if(chrs[index]=='았'||chrs[index]=='었') { 
                pomi = chrs[index]+pomi; 
                SetPomiResult(results,stem.Substring(0,index),pomi); 
                if(--index<=0|| (chrs[index]!='시'&&chrs[index]!='으')) return results; // 다음이거나 선어말어미가 없다면...	
                jaso = MorphUtil.Decompose(chrs[index]); 
            }else if(jaso.Length==3&&jaso[2]=='ㅆ') { 
                if(jaso[0]=='ㅎ'&&jaso[1]=='ㅐ') { 
                    pomi = MorphUtil.ReplaceJongsung('어',chrs[index])+pomi; 
                    stem = stem.Substring(0,index)+"하"; 
                }else if (jaso[0] != 'ㅇ' && (jaso[1] == 'ㅏ' || jaso[1] == 'ㅓ' || jaso[1] == 'ㅔ' || jaso[1] == 'ㅐ')) { 
                    pomi = "었"+pomi; 
                    stem = stem.Substring(0,index)+MorphUtil.MakeChar(chrs[index], 0); 
                }else if(jaso[0]!='ㅇ'&&(jaso[1]=='ㅙ')) { 
                    pomi = "었" + pomi; 
                    stem = stem.Substring(0,index) + MorphUtil.MakeChar(chrs[index],11, 0); 
                }else if (jaso[1] == 'ㅘ') { 
                    pomi = MorphUtil.ReplaceJongsung('아',chrs[index]) + pomi; 
                    stem = stem.Substring(0,index) + MorphUtil.MakeChar(chrs[index],8, 0); 
                } else if (jaso[1] == 'ㅝ') { 
                    pomi = MorphUtil.ReplaceJongsung('어',chrs[index]) + pomi; 
                    stem = stem.Substring(0,index) + MorphUtil.MakeChar(chrs[index],13, 0); 
                } else if (jaso[1] == 'ㅕ') { 
                    pomi = MorphUtil.ReplaceJongsung('어',chrs[index]) + pomi; 
                    stem = stem.Substring(0,index) + MorphUtil.MakeChar(chrs[index],20, 0); 
                } else if (jaso[1] == 'ㅐ') { 
                    pomi = MorphUtil.ReplaceJongsung('어',chrs[index]) + pomi; 
                    stem = stem.Substring(0,index); 
                } else if (jaso[1] == 'ㅒ') { 
                    pomi = MorphUtil.ReplaceJongsung('애',chrs[index]) + pomi; 
                    stem = stem.Substring(0,index); 
                } else { 
                    pomi = "었"+pomi; 
                } 
                
                SetPomiResult(results,stem,pomi); 
                
                if(chrs[index]!='시'&&chrs[index]!='으') return results; // 다음이거나 선어말어미가 없다면...	
                
                jaso = MorphUtil.Decompose(chrs[index]); 
            } 
            
            char[] nChrs = null; 
            if(index>0) 
                nChrs = MorphUtil.Decompose(chrs[index-1]); 
            else 
                nChrs = new char[2]; 
            
            if(nChrs.Length==2&&chrs[index]=='시'&&(chrs.Length<=index+1|| (chrs.Length>index+1&&chrs[index+1]!='셨'))) { 
                if(DictionaryUtil.GetWord(results[0])!=null) return results;  //'시'가 포함된 단어가 있다. 성가시다/도시다/들쑤시다 
                pomi = chrs[index]+pomi; 
                SetPomiResult(results,stem.Substring(0,index),pomi); 
                if(--index==0||chrs[index]!='으') return results;    // 다음이거나 선어말어미가 없다면...
                
                jaso = MorphUtil.Decompose(chrs[index]); 
            } 
            
            if(index>0) 
                nChrs = MorphUtil.Decompose(chrs[index-1]); 
            else 
                nChrs = new char[2]; 
            
            if(chrs.Length>index+1&&nChrs.Length==3&&(chrs[index+1]=='셨'||chrs[index+1]=='시')&&chrs[index]=='으') { 
                pomi = chrs[index]+pomi; 
                SetPomiResult(results,stem.Substring(0,index),pomi); 
            } 
            
            return results; 
        } 
        
        /// <summary>
        /// 불규칙 용언의 원형을 구한다.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static ArrayList Irregular(AnalysisOutput output)
        {
            ArrayList results = new ArrayList(); 
            if(output.Stem==null||output.Stem.Length==0) return results; 
            
            String ending = output.Eomi; 
            if(output.Pomi!=null) ending = output.Pomi;

            ArrayList irrs = new ArrayList(); 
            
            IrregularStem(irrs,output.Stem,ending); 
            IrregularEnding(irrs,output.Stem,ending); 
            IrregularAO(irrs,output.Stem,ending); 
            
            try { 
                foreach (String[] irr in irrs) 
                { 
                    AnalysisOutput result = output.Clone(); 
                    result.Stem = irr[0]; 
                    
                    if(output.Patn==PatternConstants.PTN_VM) { 
                        if(output.Pomi==null) 
                            result.Eomi = irr[1]; 
                        else 
                            result.Pomi = irr[1]; 
                    } 
                    
                    results.Add(result); 
                } 
            } catch (Exception e) { //CloneNotSupportedException e) { 
                throw new MorphException(e.Message,e); 
            } 
            
            return results; 
        } 
        
        /// <summary>
        /// 어간만 변하는 경우
        /// </summary>
        /// <param name="results"></param>
        /// <param name="stem"></param>
        /// <param name="ending"></param>
        private static void IrregularStem(ArrayList results, String stem, String ending)
        { 
            char feCh = ending[0]; 
            char[] fechJaso =  MorphUtil.Decompose(feCh); 
            char ls = stem[stem.Length-1]; 
            char[] lsJaso = MorphUtil.Decompose(ls); 
            
            if(feCh=='아'||feCh=='어'||feCh=='으') { 
                if(lsJaso[lsJaso.Length-1]=='ㄹ') { // ㄷ 불규칙
                    results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],7) ,ending ,PatternConstants.IRR_TYPE_DI.ToString()}); 
                } else if(lsJaso.Length==2) { // ㅅ 불규칙
                    results.Add(new String[] { stem.Substring(0, stem.Length - 1) + MorphUtil.MakeChar(stem[stem.Length - 1], 19), ending, PatternConstants.IRR_TYPE_SI.ToString() }); 
                } 
            } 
            
            if((fechJaso[0]=='ㄴ'||fechJaso[0]=='ㄹ'||fechJaso[0]=='ㅁ'||    feCh=='오'||feCh=='시') &&(ls=='우')) { // ㅂ 불규칙
                results.Add(new String[] { stem.Substring(0, stem.Length - 1) + MorphUtil.MakeChar(stem[stem.Length - 1], 17), ending, PatternConstants.IRR_TYPE_BI.ToString() }); 
            } 
            
            if((fechJaso[0]=='ㄴ'||fechJaso[0]=='ㅂ'||fechJaso[0]=='ㅅ'||    feCh=='오') &&(lsJaso.Length==2)) {  // ㄹ 탈락
                results.Add(new String[] { stem.Substring(0, stem.Length - 1) + MorphUtil.MakeChar(stem[stem.Length - 1], 8), ending, PatternConstants.IRR_TYPE_LI.ToString() }); 
            } 
            
            if(lsJaso.Length==2 &&(fechJaso[0]=='ㄴ'||fechJaso[0]=='ㄹ'||fechJaso[0]=='ㅁ'||fechJaso[0]=='ㅂ'|| lsJaso[1]=='ㅏ'||lsJaso[1]=='ㅓ'||lsJaso[1]=='ㅑ'||lsJaso[1]=='ㅕ') &&!"나".Equals(stem)) {  // ㅎ 불규칙, 그러나 [낳다]는 ㅎ 불규칙이 아니다.
                results.Add(new String[] { stem.Substring(0, stem.Length - 1) + MorphUtil.MakeChar(stem[stem.Length - 1], 27), ending, PatternConstants.IRR_TYPE_HI.ToString() }); 
            } 
        } 
        
        /// <summary>
        /// 어미만 변하는 경우
        /// </summary>
        /// <param name="results"></param>
        /// <param name="stem"></param>
        /// <param name="ending"></param>
        private static void IrregularEnding(ArrayList results, String stem, String ending)
        { 
            if(ending.StartsWith("ㅆ")) return; 
            
            char feCh = ending[0]; 
            char ls = stem[stem.Length-1]; 
            
            if(feCh=='러'&&ls=='르') { // '러' 불규칙
                results.Add(new String[] { stem, "어" + ending.Substring(1), PatternConstants.IRR_TYPE_RO.ToString() }); 
            } else if("라".Equals(ending)&&"가거".Equals(stem)) { // '거라' 불규칙
                results.Add(new String[] { stem.Substring(0, stem.Length - 1), "어라", PatternConstants.IRR_TYPE_GU.ToString() }); 
            } else if("라".Equals(ending)&&"오너".Equals(stem)) { // '너라' 불규칙 
                results.Add(new String[] { stem.Substring(0, stem.Length - 1), "어라", PatternConstants.IRR_TYPE_NU.ToString() }); 
            } 
            
            if("여".Equals(ending)&&ls=='하') { // '여' 불규칙
                results.Add(new String[] { stem, "어", PatternConstants.IRR_TYPE_NU.ToString() }); 
            } 
        } 
        
        /// <summary>
        /// 어간과 어미가 모두 변하는 경우
        /// </summary>
        /// <param name="results"></param>
        /// <param name="stem"></param>
        /// <param name="ending"></param>
        private static void IrregularAO(ArrayList results, String stem, String ending) { 
            char ls = stem[stem.Length-1]; 
            char[] lsJaso = MorphUtil.Decompose(ls); 

            if(lsJaso.Length<2) return; 

            if(lsJaso[1]=='ㅘ') { 
                if(stem.EndsWith("도와")||stem.EndsWith("고와")) { // '곱다', '돕다'의 'ㅂ' 불규칙
                    results.Add( new String[]{stem.Substring(0,stem.Length-2)+ MorphUtil.MakeChar(stem[stem.Length-2],17)  // + 'ㅂ'
                        ,MakeTesnseEomi("아",ending),PatternConstants.IRR_TYPE_BI.ToString()}); 
                }else { // '와' 축약
                    results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],8,0), // 자음 + ㅗ 
                        MakeTesnseEomi("아",ending) ,PatternConstants.IRR_TYPE_WA.ToString()}); 
                } 
            } else if(stem.EndsWith("퍼")) { 
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],18,0),  // 자음 + - 
                    MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_WA.ToString()}); 
            } else if(lsJaso[1]=='ㅝ') { 
                if(stem.Length>=2) // 'ㅂ' 불규칙
                    results.Add( new String[]{stem.Substring(0,stem.Length-2)+ MorphUtil.MakeChar(stem[stem.Length-2],17) // + 'ㅂ'
                        ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_BI.ToString()}); 
                    results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],13,0) // 자음 + ㅗ 
                        ,MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_WA.ToString()}); 
            } else if(stem.Length>=2&&ls=='라') { 
                char[] ns = MorphUtil.Decompose(stem[stem.Length-2]); 
                if(ns.Length==3&&ns[2]=='ㄹ') {  // 르 불규칙
                    results.Add( new String[]{stem.Substring(0,stem.Length-2)+ MorphUtil.MakeChar(stem[stem.Length-2],0) + "르"
                        ,MakeTesnseEomi("어",ending) 
                        ,PatternConstants.IRR_TYPE_RO.ToString()}); } 
            } else if(stem.Length>=2&&ls=='러') { 
                char[] ns = MorphUtil.Decompose(stem[stem.Length-2]); 
                if(stem[stem.Length-2]=='르') { // 러 불규칙
                    results.Add( new String[]{stem.Substring(0,stem.Length-1) ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_LO.ToString()}); 
                } else if(ns.Length==3&&ns[2]=='ㄹ') { // 르 불규칙
                    results.Add(new String[]{stem.Substring(0,stem.Length-2)+ MorphUtil.MakeChar(stem[stem.Length-2],0) + "르" ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_RO.ToString()}); 
                } 
            } else if(stem.EndsWith("펴")||stem.EndsWith("켜")) { 
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],20,0),MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_EI.ToString()}); 
            } else if(stem.EndsWith("해")) { 
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],0,0),MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_EI.ToString()}); 
            } else if(lsJaso.Length==2&&lsJaso[1]=='ㅏ') { 
                results.Add( new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],18,0) ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_UO.ToString()}); 
            } else if(lsJaso.Length==2&&lsJaso[1]=='ㅓ') { 
                // 으 탈락
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],18,0),MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_UO.ToString()}); 
                //	 아 불규칙
                results.Add( new String[]{stem ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_AH.ToString()}); 
            } else if(lsJaso[1]=='ㅕ') { 
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],20,0),MakeTesnseEomi("어",ending) ,PatternConstants.IRR_TYPE_EI.ToString()}); 
            } else if(lsJaso[1]=='ㅙ') { 
                results.Add( new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],11,0) ,MakeTesnseEomi("어",ending),PatternConstants.IRR_TYPE_OE.ToString()}); 
            } else if(lsJaso[1]=='ㅐ') { 
                results.Add(new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],0,27),MakeTesnseEomi("아",ending) ,PatternConstants.IRR_TYPE_HI.ToString()}); 
            } else if(lsJaso[1]=='ㅒ') { 
                results.Add( new String[]{stem.Substring(0,stem.Length-1)+ MorphUtil.MakeChar(stem[stem.Length-1],2,27) ,MakeTesnseEomi("아",ending),PatternConstants.IRR_TYPE_HI.ToString()}); 
            } 
        } 
        
        /// <summary>
        /// 시제 선어미말을 만들어서 반환한다.
        /// </summary>
        /// <param name="preword">'아' 또는 '어'</param>
        /// <param name="endword">어미[선어미말을 포함]</param>
        /// <returns>'았' 또는 '었'을 만들어서 반환한다.</returns>
        public static String MakeTesnseEomi(String preword, String endword) { 
            if(preword==null||preword.Length==0) return endword; 
            if(endword==null||endword.Length==0) return preword; 

            if(endword[0]=='ㅆ') { 
                return preword.Substring(0,preword.Length-1)+ MorphUtil.MakeChar(preword[preword.Length-1],20)+endword.Substring(1, endword.Length - 1); 
            } else if(endword[0]=='ㄴ') { 
                return preword.Substring(0,preword.Length-1)+ MorphUtil.MakeChar(preword[preword.Length-1],4)+endword.Substring(1, endword.Length - 1); 
            } else if(endword[0]=='ㄹ') { 
                return preword.Substring(0,preword.Length-1)+ MorphUtil.MakeChar(preword[preword.Length-1],8)+endword.Substring(1, endword.Length - 1); 
            } else if(endword[0]=='ㅁ') { 
                return preword.Substring(0,preword.Length-1)+ MorphUtil.MakeChar(preword[preword.Length-1],16)+endword.Substring(1, endword.Length - 1); 
            } else if(endword[0]=='ㅂ') { 
                return preword.Substring(0,preword.Length-1)+ MorphUtil.MakeChar(preword[preword.Length-1],17)+endword.Substring(1, endword.Length - 1); 
            } return preword+endword; 
        }
        
        /// <summary>
        /// '음/기' + '이' + 어미, '에서/부터/에서부터' + '이' + 어미 인지 조사한다.
        /// </summary>
        /// <param name="stem"></param>
        /// <returns></returns>
        public static Boolean EndsWithEEomi(String stem) { 
            int len = stem.Length; 
            
            if(len<2||!stem.EndsWith("이")) return false; 
            
            char[] jasos = MorphUtil.Decompose(stem[len-2]); 
            
            if(jasos.Length==3&&jasos[2]=='ㅁ') 
                return true; 
            else { 
                int index = stem.LastIndexOf("기");
                if (index == -1) index = stem.LastIndexOf("에서");
                if (index == -1) index = stem.LastIndexOf("부터"); 
                
                if(index==-1) return false; return true; 
            } 
        } 
        
        private static void SetPomiResult(String[] results,String stem, String pomi ) { 
            results[0] = stem; 
            results[1] = pomi; 
        } 
        
        public static Boolean IsNLMBSyl(char ech, char lch) { 
            char[] features = SyllableUtil.GetFeature(ech); 
            switch(lch) {
                case 'ㄴ' : 
                    return (features[SyllableUtil.IDX_YNPNA]=='1' || features[SyllableUtil.IDX_YNPLN]=='1'); 
                case 'ㄹ' : 
                    return (features[SyllableUtil.IDX_YNPLA]=='1'); 
                case 'ㅁ' : 
                    return (features[SyllableUtil.IDX_YNPMA]=='1'); 
                case 'ㅂ' : 
                    return (features[SyllableUtil.IDX_YNPBA]=='1'); 
            } 
            
            return false; 
        } 
        
        /// <summary>
        /// 어미를 분리한다.
        /// 1. 규칙용언과 어간만 바뀌는 불규칙 용언
        /// 2. 어미가 종성 'ㄴ/ㄹ/ㅁ/ㅂ'으로 시작되는 어절
        /// 3. '여/거라/너라'의 불규칙 어절
        /// 4. 어미 '아/어'가 탈락되는 어절
        /// 5. '아/어'의 변이체 분리
        /// </summary>
        /// <param name="stem"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static String[] SplitEomi(String stem, String end) { 
            String[] strs = new String[2]; 
            int strlen = stem.Length; 
            
            if(strlen==0) return strs; 
            char estem = stem[strlen-1]; 
            char[] chrs = MorphUtil.Decompose(estem); 
            
            if(chrs.Length==1) return strs; // 한글이 아니라면...
            if((chrs.Length==3)&&(chrs[2]=='ㄴ'||chrs[2]=='ㄹ'||chrs[2]=='ㅁ'||chrs[2]=='ㅂ')&& EomiUtil.IsNLMBSyl(estem,chrs[2])&& DictionaryUtil.CombineAndEomiCheck(chrs[2], end)!=null) { 
                strs[1] = chrs[2].ToString(); 
                if(end.Length>0) strs[1] += end; strs[0] = stem.Substring(0,strlen-1) + MorphUtil.MakeChar(estem, 0); 
            } else if(estem=='해'&&DictionaryUtil.ExistEomi("어"+end)) { 
                strs[0] = stem.Substring(0,strlen-1)+"하"; 
                strs[1] = "어"+end; 
            } else if(estem=='히'&&DictionaryUtil.ExistEomi("이"+end)) { 
                strs[0] = stem.Substring(0,strlen-1)+"하"; 
                strs[1] = "어"+end; 
            } else if(chrs[0]!='ㅇ'&& (chrs[1]=='ㅏ'||chrs[1]=='ㅓ'||chrs[1]=='ㅔ'||chrs[1]=='ㅐ')&& 
                (chrs.Length==2 || SyllableUtil.GetFeature(estem)[SyllableUtil.IDX_YNPAH]=='1')&& 
                (DictionaryUtil.CombineAndEomiCheck('어', end)!=null)) 
            { 
                strs[0] = stem;                 
                if(chrs.Length==2) strs[1] = "어"+end; 
                else strs[1] = end; 
            } else if(stem.EndsWith("하")&&"여".Equals(end)) { 
                strs[0] = stem; strs[1] = "어"; 
            }else if((chrs.Length==2)&&(chrs[1]=='ㅘ'||chrs[1]=='ㅙ'||chrs[1]=='ㅝ'||chrs[1]=='ㅕ'||chrs[1]=='ㅐ'||chrs[1]=='ㅒ')&& 
                (DictionaryUtil.CombineAndEomiCheck('어', end)!=null)) { 
                
                StringBuilder sb = new StringBuilder(); 

                if(strlen>1) sb.Append(stem.Substring(0,strlen-1)); 
                
                if(chrs[1]=='ㅘ') 
                    sb.Append(MorphUtil.MakeChar(estem, 8, 0)).Append(MorphUtil.ReplaceJongsung('아',estem)); 
                else if(chrs[1]=='ㅝ') 
                    sb.Append(MorphUtil.MakeChar(estem, 13, 0)).Append(MorphUtil.ReplaceJongsung('어',estem)); 
                else if(chrs[1]=='ㅙ') 
                    sb.Append(MorphUtil.MakeChar(estem, 11, 0)).Append(MorphUtil.ReplaceJongsung('어',estem)); 
                else if(chrs[1]=='ㅕ') 
                    sb.Append(MorphUtil.MakeChar(estem, 20, 0).ToString()).Append(MorphUtil.ReplaceJongsung('어',estem)); 
                else if(chrs[1]=='ㅐ') 
                    sb.Append(MorphUtil.MakeChar(estem, 0, 0)).Append(MorphUtil.ReplaceJongsung('어',estem)); 
                else if(chrs[1]=='ㅒ') 
                    sb.Append(MorphUtil.MakeChar(estem, 20, 0)).Append(MorphUtil.ReplaceJongsung('애',estem)); 

                strs[0] = sb.ToString(); 
                end = strs[0].Substring(strs[0].Length-1)+end; 
                strs[0] = strs[0].Substring(0,strs[0].Length-1); 
                strs[1] = end; 
            }else if(!"".Equals(end)&&DictionaryUtil.ExistEomi(end)) { 
                strs = new String[]{stem, end}; } return strs; 
        } 
    }

}
