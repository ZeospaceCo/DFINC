using System;
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
    
    public class MorphUtil
    {
        private static char[] CHOSEONG = { 
		'ㄱ','ㄲ','ㄴ','ㄷ','ㄸ','ㄹ','ㅁ','ㅂ','ㅃ','ㅅ',
		'ㅆ','ㅇ','ㅈ','ㅉ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ'}; 
        
        private static char[] JUNGSEONG = {'ㅏ','ㅐ','ㅑ','ㅒ','ㅓ','ㅔ','ㅕ','ㅖ','ㅗ','ㅘ',
		'ㅙ','ㅚ','ㅛ','ㅜ','ㅝ','ㅞ','ㅟ','ㅠ','ㅡ','ㅢ', 'ㅣ' }; 

        private static char[] JONGSEONG = { '\0','ㄱ','ㄲ','ㄳ','ㄴ','ㄵ','ㄶ','ㄷ','ㄹ','ㄺ',
		'ㄻ','ㄼ','ㄽ','ㄾ','ㄿ','ㅀ','ㅁ','ㅂ','ㅄ','ㅅ',
		'ㅆ','ㅇ','ㅈ','ㅊ','ㅋ','ㅌ','ㅍ','ㅎ' }; 
    
        private static int JUNG_JONG = JUNGSEONG.Length * JONGSEONG.Length; 
        
        /// <summary>
        /// 한글 한글자를 초성/중성/종성의 배열로 만들어 반환한다.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static char[] Decompose(char c) { 
            char[] result = null; 
            if (c > 0xD7A3 || c < 0xAC00) return new char[]{c}; 
            c -= (char)0xAC00; 
            char choseong = CHOSEONG[c / JUNG_JONG]; 
            c = (char)(c % JUNG_JONG); 
            char jungseong = JUNGSEONG[c / JONGSEONG.Length]; 
            char jongseong = JONGSEONG[c % JONGSEONG.Length]; 
            
            if (jongseong != 0) { 
                result = new char[] {choseong, jungseong, jongseong}; 
            }else { 
                result = new char[] {choseong, jungseong}; 
            } 
            
            return result; 
        } 
        
        public static char Compound(int first, int middle, int last) {
            return (char)((char)0xAC00 + first * JUNG_JONG + middle * JONGSEONG.Length + last); 
        } 
        
        public static char MakeChar(char ch, int mdl, int last) {
            ch -= (char)0xAC00; int first = ch / JUNG_JONG; 
            return Compound(first,mdl,last); 
        } 
        
        public static char MakeChar(char ch, int last) {
            ch -= (char)0xAC00; 
            int first = ch / JUNG_JONG; 
            ch = (char)(ch % JUNG_JONG); 
            int middle = ch / JONGSEONG.Length; 
            return Compound(first,middle,last); 
        } 
        
        public static char ReplaceJongsung(char dest, char source) {
            source -= (char)0xAC00; 
            int last = source % JONGSEONG.Length; 
            return MakeChar(dest,last); 
        } 
        
        /// <summary>
        /// 형태소 유형 출력을 위한 문자열을 생성한다.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string BuildTypeString(string word, char type) { 
            StringBuilder sb = new StringBuilder(); 
            sb.Append(word); 
            sb.Append("("); 
            sb.Append(type); 
            sb.Append(")"); 
            return sb.ToString(); 
        }

        public static void BuildPtnVM(AnalysisOutput output, List<AnalysisOutput> candidates)
        { 
            string end = output.Eomi; 
            
            if (output.Pomi != null) end = output.Pomi; 
            output.Patn = PatternConstants.PTN_VM; 
            output.Pos = PatternConstants.POS_VERB; 
            
            if (output.Score == AnalysisOutput.SCORE_CORRECT) { 
                candidates.Add(output); 
            }else { 
                String[] irrs = IrregularUtil.RestoreIrregularVerb(output.Stem,end); 
                if (irrs != null) { 
                    output.Score = AnalysisOutput.SCORE_CORRECT; 
                    output.Stem = irrs[0];
                    candidates.Add(output); 
                } 
            } 
        } 
        
        /// <summary>
        /// 용언 + '음/기' + '이' + 어미, 체언 + '에서/부터/에서부터' + '이' + 어미
        /// </summary>
        /// <param name="output"></param>
        /// <param name="candidates"></param>
        public static void BuildPtnCM(AnalysisOutput output, List<AnalysisOutput> candidates)
        { 
            char ch = output.Stem[output.Stem.Length - 2]; 
            char[] jasos = MorphUtil.Decompose(ch); 
            if (jasos.Length == 3 || ch == '기') { 
                BuildPtnVMCM(output,candidates); 
            } else { } 
        }

        private static void BuildPtnVMCM(AnalysisOutput output, List<AnalysisOutput> candidates)
        { 
            string stem = output.Stem; 
            output.Patn = PatternConstants.PTN_VMCM; 
            output.Pos = PatternConstants.POS_VERB; 
            char ch = stem[stem.Length - 2]; 
            char[] jasos = MorphUtil.Decompose(ch); 
            
            if (ch == '기') { 
                output.AddElist("기"); 
                output.AddElist("이"); 
                output.Stem = stem.Substring(0,stem.Length - 2); 
                
                if (DictionaryUtil.GetVerb(output.Stem) != null) candidates.Add(output); 
            }else if (jasos[2] == 'ㅁ') { 
                if (stem.Length>1) stem = stem.Substring(0,stem.Length - 2); 
                stem += MorphUtil.MakeChar(ch, 0); output.AddElist("ㅁ"); 
                output.AddElist("이"); 
                output.Stem = stem; 
                
                if (DictionaryUtil.GetVerb(stem) != null) 
                    candidates.Add(output); 
                else { 
                    String[] morphs = IrregularUtil.RestoreIrregularVerb(stem,"ㅁ"); 
                    
                    if(morphs!=null) { 
                        output.Score = AnalysisOutput.SCORE_CORRECT; 
                        output.Stem = morphs[0]; 
                        candidates.Add(output); 
                    } 
                } 
            } 
        } 
        
        public static Boolean HasVerbOnly(String input) { 
            for(int i=input.Length-1;i>=0;i--) { 
                char[] feature = SyllableUtil.GetFeature(input[i]); 
                
                if(feature[SyllableUtil.IDX_WDSURF]=='1'&&input.Length>i) 
                    return true; 
            } 
            
            return false; 
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
                return preword.Substring(0,preword.Length-1)+ MakeChar(preword[preword.Length-1],20)+endword.Substring(1,endword.Length); 
            } else if(endword[0]=='ㄴ') { 
                return preword.Substring(0,preword.Length-1)+ MakeChar(preword[preword.Length-1],4)+endword.Substring(1,endword.Length); 
            } else if(endword[0]=='ㄹ') { 
                return preword.Substring(0,preword.Length-1)+ MakeChar(preword[preword.Length-1],8)+endword.Substring(1,endword.Length); 
            } else if(endword[0]=='ㅁ') { 
                return preword.Substring(0,preword.Length-1)+ MakeChar(preword[preword.Length-1],16)+endword.Substring(1,endword.Length); 
            } else if(endword[0]=='ㅂ') { 
                return preword.Substring(0,preword.Length-1)+ MakeChar(preword[preword.Length-1],17)+endword.Substring(1,endword.Length); 
            } return preword+endword; 
        } 
        
        /// <summary>
        /// 용언화접미사가 결합될 수 있는지 여부를 점검한다.
        /// 특히 사전에 등록된 되다, 하다형 의 접속이 가능한지를 조사한다.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Boolean IsValidSuffix(WordEntry entry, AnalysisOutput o) { return true; } 
    }
}
