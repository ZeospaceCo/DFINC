using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
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

    /// 동사의 불규칙 변형을 처리하는 Utility Class
    public class IrregularUtil
    {
        // ㅂ 불규칙
        public const char IRR_TYPE_BIUP = 'B';

        // ㅎ 불규칙
        public const char IRR_TYPE_HIOOT = 'H';

        // ㄹ 불규칙
        public const char IRR_TYPE_LIUL = 'U';

        // 르 불규칙
        public const char IRR_TYPE_LOO = 'L';

        // ㅅ 불규칙
        public const char IRR_TYPE_SIUT = 'S';

        // ㄷ 불규칙
        public const char IRR_TYPE_DI = 'D';

        // 러 불규칙
        public const char IRR_TYPE_RU = 'R';

        // 으 탈락
        public const char IRR_TYPE_UI = 'X';

        // 규칙형
        public const char IRR_TYPE_REGULAR = 'X';

		public static string[] RestoreIrregularVerb(string start, string end)
		{

			if (end == null){ end = ""; }
			char[] jasos = new char[0];

			if (end.Length > 0){ jasos = MorphUtil.Decompose(end[0]); }

			if (end.StartsWith("ㄴ")) {			 
                String[] irrs = RestoreBIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreHIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreELIrregular(start,end); 
                if(irrs!=null) return irrs; 
            }else if(end.StartsWith("ㄹ")) {
				string[] irrs = RestoreBIrregular(start,end);
				if (irrs != null){ return irrs; }
				irrs = RestoreHIrregular(start,end);
				if (irrs != null){ return irrs; }
				irrs = RestoreELIrregular(start,end);
				if (irrs != null){ return irrs; }
			}else if (end.StartsWith("ㅁ")) {
                String[] irrs = RestoreBIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreHIrregular(start,end); 
                if(irrs!=null) return irrs; 
            }else if(end.StartsWith("ㅂ")) {
				string[] irrs = RestoreBIrregular(start,end);
				if (irrs != null){ return irrs; }
				irrs = RestoreHIrregular(start,end);
				if (irrs != null){ return irrs; }
				irrs = RestoreELIrregular(start,end);
				if (irrs != null){ return irrs; }
			}else if (start.EndsWith("우")||start.EndsWith("오"))
			{
				string[] irrs = RestoreBIrregular(start,end);
				if (irrs != null){ return irrs; }
			}
			else if (end.StartsWith("오")) {
                String[] irrs = RestoreBIrregular(start,end); if(irrs!=null) return irrs; 
            }else if(end.StartsWith("시"))
			{
				string[] irrs = RestoreBIrregular(start,end);
				if (irrs != null){ return irrs; }
				irrs = RestoreELIrregular(start,end);
				if (irrs != null){ return irrs; }
			}
			else if (end.StartsWith("으")) {			 
                String[] irrs = RestoreBIrregular(start,end); 
                if(irrs!=null) return irrs; 
            }else if(jasos.Length>1&&jasos[0]=='ㅇ'&&(jasos[1]=='ㅓ'||jasos[1]=='ㅏ')) { 
                String[] irrs = RestoreDIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreSIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreLIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreHIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreUIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreRUIrregular(start,end); 
                if(irrs!=null) return irrs; 
            }else if(jasos.Length>1&&jasos[0]=='ㅇ'&&jasos[1]=='ㅡ') { 
                String[] irrs = RestoreDIrregular(start,end); 
                if(irrs!=null) return irrs; 
                irrs = RestoreSIrregular(start,end); 
                if(irrs!=null) return irrs;
            }
            else if (("가".Equals(start) && "거라".Equals(end)) || ("오".Equals(start) && "너라".Equals(end)))
			{
				return new string[]{start,end};
			}

			return null;

		}

        /// <summary>
        /// ㅂ 불규칙 원형을 복원한다. (돕다, 곱다)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
		private static string[] RestoreBIrregular(string start, string end)
		{
			if (start == null || "".Equals(start) || end == null){ return null; }

			if (start.Length < 2){ return null; }

			if (!(start.EndsWith("오")||start.EndsWith("우"))){ return null; }

			char convEnd = MorphUtil.MakeChar(end[0], 0);
			if ("ㅁ".Equals(end)||"ㄴ".Equals(end) || "ㄹ".Equals(end)|| convEnd=='아'||convEnd=='어') { // 도우(돕), 고오(곱), 스러우(스럽) 등으로 변형되므로 반드시 2자 이상임
                char ch = start[start.Length-2]; 
                ch = MorphUtil.MakeChar(ch, 17); 
                
                if(start.Length>2)
                    start = Utilities.ArrayToString(new String[] { start.Substring(0, start.Length - 2), ch.ToString() }); 
                else 
                    start = ch.ToString(); WordEntry entry = DictionaryUtil.GetVerb(start); 
                
                if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_BIUP) 
                    return new String[]{start,end}; 
            } 
            return null; 
        } 
        
        /// <summary>
        ///  ㄷ 불규칙 원형을 복원한다. (깨닫다, 묻다)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreDIrregular(String start, String end) 
        { 
            if(start==null||"".Equals(start)) return null; 
            char ch = start[start.Length-1]; 
            char[] jasos = MorphUtil.Decompose(ch); 
            
            if(jasos.Length!=3||jasos[2]!='ㄹ') return null; 
            
            ch = MorphUtil.MakeChar(ch, 7); 
            
            if(start.Length>1)
                start = Utilities.ArrayToString(new String[] { start.Substring(0, start.Length - 1), ch.ToString() }); 
            else
                start = ch.ToString(); 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_DI) 
                return new String[]{start,end}; 
            
            return null; 
        } 
        
        /// <summary>
        /// ㅅ 불규칙 원형을 복원한다. (긋다--그어)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreSIrregular(String start, String end) 
        { 
            if(start==null||"".Equals(start)) return null; 
            
            char ch = start[start.Length-1]; 
            char[] jasos = MorphUtil.Decompose(ch); 
            
            if(jasos.Length!=2) return null; 
            ch = MorphUtil.MakeChar(ch, 19); 
            
            if(start.Length>1) 
                start = start.Substring(0,start.Length-1)+ch; 
            else
                start = ch.ToString(); 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_SIUT) 
                return new String[]{start,end}; return null; 
        } 
        
        /// <summary>
        /// 르 불규칙 원형을 복원한다. (흐르다-->흘러)
        /// "따르다"는 ㄹ불규칙이 아니지만.. 인 것처럼 처리한다.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreLIrregular(String start, String end) { 
            if(start.Length<2) return null; 

            char ch1 = start[start.Length-2]; 
            char ch2 = start[start.Length-1]; 
            char[] jasos1 = MorphUtil.Decompose(ch1); 
            
            if(((jasos1.Length==3&&jasos1[2]=='ㄹ')||jasos1.Length==2)&&(ch2=='러'||ch2=='라')) { 
                StringBuilder sb = new StringBuilder(); 
                ch1 = MorphUtil.MakeChar(ch1, 0); 
                
                if(start.Length>2) 
                    sb.Append(start.Substring(0,start.Length-2)).Append(ch1).Append("르");
                else
					sb.Append(char.ToString(ch1)).Append("르"); 
                
                WordEntry entry = DictionaryUtil.GetVerb(sb.ToString()); 
                
                if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_LOO) 
                    return new String[]{sb.ToString(),end}; 
            } 
            
            return null; 
        } 
        
        /// <summary>
        /// ㄹ불규칙 원형을 복원한다. (길다-->긴, 알다-->안)
        /// 어간의 끝소리인 ‘ㄹ’이 ‘ㄴ’, ‘ㄹ’, ‘ㅂ’, ‘오’, ‘시’ 앞에서 탈락하는 활용의 형식
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreELIrregular(String start, String end) { 
            if(start==null || start.Length==0 || end==null||end.Length==0) return null; 
            
            if(!(end[0]=='ㄴ'||end[0]=='ㄹ'||end[0]=='ㅂ'||end[0]=='오'||end[0]=='시')) return null; 
            
            char convEnd = MorphUtil.MakeChar(start[start.Length-1], 8); 
            start = start.Substring(0,start.Length-1)+convEnd; 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null) 
                return new String[]{start,end}; 
            return null; 
        } 
        
        /// <summary>
        /// 러 불규칙 원형을 복원한다. (이르다->이르러, 푸르다->푸르러)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreRUIrregular(String start, String end) { 
            if(start.Length<2) return null; 
            
            char ch1 = start[start.Length-1]; 
            char ch2 = start[start.Length-2]; 
            char[] jasos1 = MorphUtil.Decompose(ch1); 
            char[] jasos2 = MorphUtil.Decompose(ch2); 
            
            if(jasos1[0]!='ㄹ'||jasos2[0]!='ㄹ') return null; 
            
            ch2 = MorphUtil.MakeChar(ch2, 0); 
            
            if(start.Length>2) 
                start = start.Substring(0,start.Length-1); 
            else
                start = ch2.ToString(); 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_RU) return new String[]{start,end}; 
            
            return null; 
        } 
        
        /// <summary>
        /// ㅎ 탈락 원형을 복원한다. (까맣다-->까만,까매서)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreHIrregular(String start, String end) { 
            if(start==null||"".Equals(start)||end==null||"".Equals(end)) return null; 
            char ch1 = end[0]; 
            char ch2 = start[start.Length-1]; 
            char[] jasos1 = MorphUtil.Decompose(ch1); 
            char[] jasos2 = MorphUtil.Decompose(ch2); 
            if(jasos1.Length==1) { 
                ch2 = MorphUtil.MakeChar(ch2, 27); 
            }else { 
                if(jasos2.Length!=2||jasos2[1]!='ㅐ') return null; 
                    ch2 = MorphUtil.MakeChar(ch2, 0, 27); 
            } 
            
            if(start.Length>1) 
                start = start.Substring(0,start.Length-1)+ch2; 
            else
                start = ch2.ToString(); 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null&&entry.GetFeature(WordEntry.IDX_REGURA)==IRR_TYPE_HIOOT) return new String[]{start,end}; 
            
            return null; 
        } 
        
        /// <summary>
        /// 으 탈락 원형을 복원한다. (뜨다->더, 크다-커)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static String[] RestoreUIrregular(String start, String end) { 
            if(start==null||"".Equals(start)) return null; 
            
            char ch = start[start.Length-1]; 
            char[] jasos = MorphUtil.Decompose(ch); 
            
            if(!(jasos.Length==2&&jasos[1]=='ㅓ')) return null; 
                ch = MorphUtil.MakeChar(ch, 18,0); 
            if(start.Length>1) 
                start = start.Substring(0,start.Length-1)+ch;
            else start = ch.ToString(); 
            
            WordEntry entry = DictionaryUtil.GetVerb(start); 
            
            if(entry!=null)    
                return new String[]{start,end}; 
            
            return null; 
        } 
    }
}
