using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    public class WordSpaceAnalyzer
    {
        private MorphAnalyzer morphAnal;

		public WordSpaceAnalyzer()
		{
			morphAnal = new MorphAnalyzer();
			morphAnal.ExactCompound = false;
		}

        public virtual List<AnalysisOutput> Analyze(string input)
		{
			IList stack = new ArrayList();

			WSOutput output = new WSOutput();

			int wStart = 0;

			int sgCount = -9;

			Dictionary<int, int> fCounter = new Dictionary<int,int>();

			for (int i = 0;i < input.Length;i++)
			{

				char[] f = SyllableUtil.GetFeature(input[i]);

				string prefix = i == input.Length - 1 ? "X" : input.Substring(wStart, i + 2 - wStart);
				IEnumerator iter = DictionaryUtil.FindWithPrefix(prefix);

				List<AnalysisOutput> candidates = new List<AnalysisOutput>();

				WordEntry entry = null;

				if (input[i] == '있' || input[i] == '없' || input[i] == '앞') {
                    AddSingleWord(input.Substring(wStart, i - wStart), candidates);

			        // 다음 음절이 2음절 이상 단어에 포함되어 있고 마지막 음절이 아니라면   띄워쓰기 위치가 아닐 가능성이 크다.
			        // 부사, 관형사, 감탄사 등 단일어일 가능성인 경우 띄워쓰기가 가능하나, 
			        // 이 경우는 다음 음절을 조사하여 
                } else if (i != input.Length - 1 && iter.MoveNext()) {
                    // 아무짓도 하지 않음.
                    sgCount = i;
                } else if (!iter.MoveNext() && (entry = DictionaryUtil.GetBusa(input.Substring(wStart, i + 1 - wStart))) != null) {
                    candidates.Add(BuildSingleOutPut(entry));
			        // 현 음절이 조사나 어미가 시작되는 음절일 가능성이 있다면...	
                } else if (f[SyllableUtil.IDX_EOGAN] == '1' || f[SyllableUtil.IDX_JOSA1] == '1'){
                    if (f[SyllableUtil.IDX_JOSA1] == '1') candidates.AddRange(AnlysisWithJosa(input.Substring(wStart), i - wStart)); 
                    if (f[SyllableUtil.IDX_EOGAN] == '1') candidates.AddRange(AnlysisWithEomi(input.Substring(wStart), i - wStart));
                } 
			    // 호보가 될 가능성이 높은 순으로 정렬한다.
                //Collections.sort(candidates, new WSOuputComparator());
                candidates.Sort();
                // 길이가 가장 긴 단어를 단일어로 추가한다.
                AppendSingleWord(candidates); 
			    // 분석에 실패한 단어를 
                AnalysisCompouns(candidates); 
                // 호보가 될 가능성이 높은 순으로 정렬한다.
                //Collections.sort(candidates, new WSOuputComparator()); 
                candidates.Sort();
                
                int reseult = ValidationAndAppend(output, candidates, input); 
                if (reseult == 1) {
                    i = output.LastEnd - 1; 
                    wStart = output.LastEnd;
                } else if (reseult == -1) {
                    int index = fCounter[output.LastEnd]; 
                    if (index == null) 
                        index = output.LastEnd; 
                    else 
                        index = index + 1; 
                    i = index; 
                    wStart = output.LastEnd; 
                    fCounter.Add(output.LastEnd, index);
                } 
            } 
            
            // 분석에 실패하였다면 원래 문자열을 되돌려 준다.
            if (output.LastEnd < input.Length) {
                string source = input.Substring(output.LastEnd); 
                int score = DictionaryUtil.GetWord(source) == null ? AnalysisOutput.SCORE_ANALYSIS : AnalysisOutput.SCORE_CORRECT; 
                AnalysisOutput o = new AnalysisOutput(source,null,null,PatternConstants.POS_NOUN, PatternConstants.PTN_N,score); 
                o.Source = source; 
                output.Phrases.Add(o); 
                morphAnal.ConfirmCNoun(o);
            } 
            
            return output.Phrases; 
        } 
        
        /// <summary>
        /// 조사로 끝나는 어구를 분석한다.
        /// </summary>
        /// <param name="snipt"></param>
        /// <param name="js"></param>
        /// <returns></returns>
        private List<AnalysisOutput> AnlysisWithJosa(string snipt, int js)
        {
            List<AnalysisOutput> candidates = new List<AnalysisOutput>(); 
            if (js < 1) return candidates; 
            
            int jend = FindJosaEnd(snipt, js); 
            if (jend == -1) return candidates; // 타당한 조사가 아니라면...
            
            string input = snipt.Substring(0,jend); 
            bool josaFlag = true; 
            
            for (int i = input.Length - 1;i>0;i--) {
                string stem = input.Substring(0,i); 
                string josa = input.Substring(i); 
                char[] feature = SyllableUtil.GetFeature(josa[0]); 
                if (josaFlag && feature[SyllableUtil.IDX_JOSA1] == '1') {
                    morphAnal.AnalysisWithJosa(stem,josa,candidates);
                } 
                
                if (josaFlag && feature[SyllableUtil.IDX_JOSA2] == '0') 
                    josaFlag = false; if (!josaFlag) break;
            } 
            
            if (input.Length == 1) {
                AnalysisOutput o = new AnalysisOutput(input,null,null,PatternConstants.POS_NOUN, PatternConstants.PTN_N,AnalysisOutput.SCORE_ANALYSIS); 
                
                candidates.Add(o);
            } 
            
            FillSourceString(input, candidates); return candidates;
        } 
        
        /// <summary>
        /// 조사의 첫음절부터 조사의 2음절이상에 사용될 수 있는 음절을 조사하여
        /// 가장 큰 조사를 찾는다.
        /// </summary>
        /// <param name="snipt"></param>
        /// <param name="jstart"></param>
        /// <returns></returns>
        private int FindJosaEnd(string snipt, int jstart) { 
            int jend = jstart; 
            
            // [것을]이 명사를 이루는 경우는 없다.
            if (snipt[jstart - 1] == '것' && (snipt[jstart] == '을')) return jstart + 1; 
            if (snipt.Length>jstart + 2 && snipt[jstart + 1] == '스') { // 사랑스러운, 자랑스러운 같은 경우르 처리함.
                char[] chrs = MorphUtil.Decompose(snipt[jstart + 2]); 
                if (chrs.Length >= 2 && chrs[0] == 'ㄹ' && chrs[1] == 'ㅓ') return -1; 
            } 
            
            // 조사의 2음절로 사용될 수 마지막 음절을 찾는다.
            for (int i = jstart + 1;i < snipt.Length;i++) { 
                char[] f = SyllableUtil.GetFeature(snipt[i]); 
                if (f[SyllableUtil.IDX_JOSA2] == '0') break; jend = i; 
            } 
            int start = jend; 
            bool hasJosa = false; 
            for (int i = start;i >= jstart;i--) { 
                string str = snipt.Substring(jstart, i + 1 - jstart); 
                if (DictionaryUtil.ExistJosa(str) && !FindNounWithinStr(snipt,i,i + 2) && !IsNounPart(snipt,jstart)) { 
                    jend = i; hasJosa = true; break; 
                } 
            } 
            
            if (!hasJosa) return -1; return jend + 1; 
        } 

        /// <summary>
        /// 향후 계산이나 원 문자열을 보여주기 위해 source string 을 저장한다.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="candidates"></param>
        private void FillSourceString(string source, List<AnalysisOutput> candidates) { 
            foreach (AnalysisOutput o in candidates) { 
                o.Source = source; 
            } 
        } 
        
        /// <summary>
        /// 목록의 1번지가 가장 큰 길이를 가진다.
        /// </summary>
        /// <param name="candidates"></param>
        private void AppendSingleWord(List<AnalysisOutput> candidates) { 
            if (candidates.Count == 0) return; 
            string source = candidates[0].Source; 
            WordEntry entry = DictionaryUtil.GetWordExceptVerb(source); 
            
            if (entry != null) { 
                candidates.Add(BuildSingleOutPut(entry)); 
            } else { 
                if (candidates[0].Patn > PatternConstants.PTN_VM && candidates[0].Patn <= PatternConstants.PTN_VMXMJ) return; 
                if (source.Length < 5) return; 
                AnalysisOutput o = new AnalysisOutput(source,null,null,PatternConstants.POS_NOUN, PatternConstants.PTN_N,AnalysisOutput.SCORE_ANALYSIS); 
                o.Source = source; 
                morphAnal.ConfirmCNoun(o); 
                if (o.Score == AnalysisOutput.SCORE_CORRECT) candidates.Add(o); 
            } 
        } 
        
        private void AddSingleWord(string source, List<AnalysisOutput> candidates) { 
            WordEntry entry = DictionaryUtil.GetWordExceptVerb(source); 
            
            if (entry != null) { 
                candidates.Add(BuildSingleOutPut(entry)); 
            } else { 
                AnalysisOutput o = new AnalysisOutput(source,null,null,PatternConstants.POS_NOUN, PatternConstants.PTN_N,AnalysisOutput.SCORE_ANALYSIS); 
                o.Source = source; 
                morphAnal.ConfirmCNoun(o); 
                candidates.Add(o); 
            } 
        }

        private List<AnalysisOutput> AnlysisWithEomi(string snipt, int estart)
        {
            List<AnalysisOutput> candidates = new List<AnalysisOutput>(); 
            int eend = FindEomiEnd(snipt,estart); 
            
            // 동사앞에 명사분리
            int vstart = 0; 
            
            for (int i = estart - 1;i >= 0;i--) { 
                IEnumerator iter = DictionaryUtil.FindWithPrefix(snipt.Substring(i, estart - i)); 
                
                if (iter.MoveNext()) vstart = i; else break; 
            } 
            
            if (snipt.Length>eend && DictionaryUtil.FindWithPrefix(snipt.Substring(vstart, eend + 1 - vstart)).MoveNext()) 
                return candidates; // 다음음절까지 단어의 일부라면.. 분해를 안한다.
            string pvword = null; 
            if (vstart != 0) pvword = snipt.Substring(0,vstart); 
            
            while (true) { // ㄹ,ㅁ,ㄴ 이기때문에 어미위치를 뒤로 잡았는데, 용언+어미의 형태가 아니라면.. 어구 끝을 하나 줄인다.
                string input = snipt.Substring(vstart, eend - vstart); 
                AnlysisWithEomiDetail(input, candidates); 
                if (candidates.Count == 0) break; 
                if (("ㄹ".Equals(candidates[0].Eomi) || "ㅁ".Equals(candidates[0].Eomi) || "ㄴ".Equals(candidates[0].Eomi)) && eend>estart+1 && candidates[0].Patn!=PatternConstants.PTN_VM && candidates[0].Patn!=PatternConstants.PTN_NSM ) { 
                    eend--; 
                }else if(pvword!=null&&candidates[0].Patn>=PatternConstants.PTN_VM&& candidates[0].Patn<=PatternConstants.PTN_VMXMJ && DictionaryUtil.GetWord(input)!=null){ 
                    candidates.Clear(); 
                    break; 
                }else if(pvword!=null&&VerbUtil.VerbSuffix(candidates[0].Stem) &&DictionaryUtil.GetNoun(pvword)!=null){ 
                    // 명사 + 용언화 접미사 + 어미 처리
                    candidates.Clear(); 
                    AnlysisWithEomiDetail(snipt.Substring(0,eend), candidates); 
                    pvword=null; break; 
                } else { 
                    break; 
                } 
            } if(candidates.Count>0&&pvword!=null) { 
                AnalysisOutput o =new AnalysisOutput(pvword,null,null,PatternConstants.POS_NOUN, PatternConstants.PTN_N,AnalysisOutput.SCORE_ANALYSIS); 
                morphAnal.ConfirmCNoun(o); 
                List<CompoundEntry> cnouns = o.CNounList; 
                if(cnouns.Count==0) { 
                    bool isVerb = DictionaryUtil.GetWordExceptVerb(pvword)!=null;
                    cnouns.Add(new CompoundEntry(pvword, 0, isVerb)); 
                } 
                
                foreach (AnalysisOutput candidate in candidates) { 
                    candidate.CNounList.AddRange(cnouns); 
                    candidate.CNounList.Add(new CompoundEntry(candidate.Stem,0,true)); 
                    candidate.Stem = pvword+candidate.Stem; // 이렇게 해야 WSOutput 에 복합명사 처리할 때 정상처리됨
                } 
            } 
            
            FillSourceString(snipt.Substring(0,eend), candidates); 
            
            return candidates; 
        } 
        
        private void AnlysisWithEomiDetail(String input, List<AnalysisOutput> candidates ) { 
            Boolean eomiFlag = true; 
            int strlen = input.Length; 
            char ch = input[strlen-1]; 
            char[] feature =  SyllableUtil.GetFeature(ch); 
            if(feature[SyllableUtil.IDX_YNPNA]=='1'||feature[SyllableUtil.IDX_YNPLA]=='1'|| feature[SyllableUtil.IDX_YNPMA]=='1') morphAnal.AnalysisWithEomi(input,"",candidates); 
            for(int i=strlen-1;i>0;i--) { 
                String stem = input.Substring(0,i); 
                String eomi = input.Substring(i); 
                feature =  SyllableUtil.GetFeature(eomi[0]); 
                if(eomiFlag) { 
                    morphAnal.AnalysisWithEomi(stem,eomi,candidates); 
                } 
                
                if(eomiFlag&&feature[SyllableUtil.IDX_EOMI2]=='0') 
                    eomiFlag = false; 
                if(!eomiFlag) break; 
            } 
        } 
        
        /// <summary>
        /// 어미의 첫음절부터 어미의 1음절이상에 사용될 수 있는 음절을 조사하여
        /// 가장 큰 조사를 찾는다.
        /// </summary>
        /// <param name="snipt"></param>
        /// <param name="estart"></param>
        /// <returns></returns>
        private int FindEomiEnd(String snipt, int estart) { 
            int jend = 0; 
            String tail = null; 
            char[] chr = MorphUtil.Decompose(snipt[estart]); 
            if(chr.Length==3 && (chr[2]=='ㄴ')) { 
                tail = '은'+snipt.Substring(estart+1); 
            }else if(chr.Length==3 && (chr[2]=='ㄹ')) { 
                tail = '을'+snipt.Substring(estart+1); 
            }else if(chr.Length==3 && (chr[2]=='ㅂ')) { 
                tail = '습'+snipt.Substring(estart+1); 
            }else { 
                tail = snipt.Substring(estart); 
            } 

            // 조사의 2음절로 사용될 수 마지막 음절을 찾는다.
            int start = 0; 
            
            for(int i=1;i<tail.Length;i++) { 
                char[] f = SyllableUtil.GetFeature(tail[i]); 
                if(f[SyllableUtil.IDX_EOGAN]=='0') break; 
                start = i; 
            } 
            
            for(int i=start;i>0;i--) { // 찾을 수 없더라도 1음절은 반드시 반환해야 한다.
                String str = tail.Substring(0,i+1); 
                char[] chrs = MorphUtil.Decompose(tail[i]); 
                if(DictionaryUtil.ExistEomi(str) || (i<2&&chrs.Length==3&&(chrs[2]=='ㄹ'||chrs[2]=='ㅁ'||chrs[2]=='ㄴ'))) { // ㅁ,ㄹ,ㄴ이 연속된 용언은 없다, 사전을 보고 확인을 해보자
                    jend = i; 
                    break; 
                } 
            } 
            
            return estart+jend+1; 
        } 
        
        /// <summary>
        /// validation 후 후보가 될 가능성이 높은 최상위 것을 결과에 추가한다.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="candidates"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private int ValidationAndAppend(WSOutput output, List<AnalysisOutput> candidates, String input) { 
            if(candidates.Count==0) return 0; 
            
            //AnalysisOutput o = candidates.Remove(0); 
            AnalysisOutput o = candidates[0];
            candidates.RemoveAt(0);


            AnalysisOutput po = output.Phrases.Count>0 ?  output.Phrases[output.Phrases.Count-1] : null; 
            
            String ejend = o.Source.Substring(o.Stem.Length); 
            
            char[] chrs = po!=null&&po.Stem.Length>0 ? MorphUtil.Decompose(po.Stem[po.Stem.Length-1]) : null; 
            
            String pjend = po!=null&&po.Stem.Length>0 ? po.Source.Substring(po.Stem.Length) : null; 
            
            char ja = 'x'; // 임의의 문자
            
            if(po!=null&&(po.Patn==PatternConstants.PTN_VM||po.Patn==PatternConstants.PTN_VMCM||po.Patn==PatternConstants.PTN_VMXM)) { 
                char[] chs = MorphUtil.Decompose(po.Eomi[po.Eomi.Length-1]); 
                
                if(chs.Length==3) ja=chs[2]; 
                else if(chs.Length==1) ja=chs[0]; 
            } 
            
            int nEnd = output.LastEnd+o.Source.Length; 
            char[] f = nEnd<input.Length ? SyllableUtil.GetFeature(input[nEnd]) : null; 
            
            // 밥먹고 같은 경우가 가능하나.. 먹고는 명사가 아니다.
            if(po!=null&&po.Patn==PatternConstants.PTN_N&&candidates.Count>0&& o.Patn==PatternConstants.PTN_VM&&candidates[0].Patn==PatternConstants.PTN_N) { 

                //o = candidates.Remove(0); 

                o = candidates[0];
                candidates.RemoveAt(0);

            }else if(
                po!=null&&po.Patn>=PatternConstants.PTN_VM&&candidates.Count>0&& candidates[0].Patn==PatternConstants.PTN_N&& (ja=='ㄴ'||ja=='ㄹ')) { // 다녀가ㄴ, 사,람(e) 로 분해 방지
                //o = candidates.Remove(0); 

                o = candidates[0];
                candidates.RemoveAt(0);

            } if(o.Pos==PatternConstants.POS_NOUN && MorphUtil.HasVerbOnly(o.Stem)) { 
                output.RemoveLast(); 
                return -1; 
            }else if(nEnd<input.Length && f[SyllableUtil.IDX_JOSA1]=='1' && DictionaryUtil.GetNoun(o.Source)!=null) { 
                return -1; 
            }else if(nEnd<input.Length && o.Score==AnalysisOutput.SCORE_ANALYSIS && DictionaryUtil.FindWithPrefix(ejend+input[nEnd]).MoveNext()) { // 루씬하ㄴ 글형태소분석기 방지
                return -1; 
            }else if(po!=null&&po.Patn==PatternConstants.PTN_VM&&"ㅁ".Equals(po.Eomi) && o.Stem.Equals("하")) { // 다짐 합니다 로 분리되는 것 방지 
                output.RemoveLast(); 
                return -1; 
            }else if(po!=null&&po.Patn==PatternConstants.PTN_N&&VerbUtil.VerbSuffix(o.Stem)&& !"있".Equals(o.Stem)) { // 사랑받다, 사랑스러운을 처리, 그러나 있은 앞 단어와 결합하지 않는다.
                output.RemoveLast(); return -1; } else { output.AddPhrase(o); 
            } 
            
            return 1; 
        } 
        
        private AnalysisOutput BuildSingleOutPut(WordEntry entry) { 
            char pos = PatternConstants.POS_NOUN; 
            int ptn = PatternConstants.PTN_N; 
            
            if(entry.GetFeature(WordEntry.IDX_NOUN)=='0') { 
                pos = PatternConstants.POS_AID; ptn = PatternConstants.PTN_AID; 
            } 
            
            AnalysisOutput o = new AnalysisOutput(entry.Word,null,null,pos, ptn,AnalysisOutput.SCORE_CORRECT); 
            o.Source = entry.Word; 
            return o; 
        } 
        
        private void AnalysisCompouns(List<AnalysisOutput> candidates) { 
            // 복합명사 분해여부 결정하여 분해
            Boolean changed = false; 
            Boolean correct = false; 
            foreach(AnalysisOutput o in candidates) { 
                if(o.Score==AnalysisOutput.SCORE_CORRECT) { 
                    if(o.Patn!=PatternConstants.PTN_NJ) correct=true; 
				    // "활성화해"가 [활성화(N),하(t),어야(e)] 분석성공하였는데 [활성/화해]분해되는 것을 방지
                    if("하".Equals(o.Vsfx)) break; 
                        continue; 
                } if(o.Patn<=PatternConstants.PTN_VM&&o.Stem.Length>2) { 
                    if(!(correct&&o.Patn==PatternConstants.PTN_N)) morphAnal.ConfirmCNoun(o); 
                    if(o.Score==AnalysisOutput.SCORE_CORRECT) changed=true; 
                } 
            } 
        } 
        
        /// <summary>
        /// 문자열에 
        /// </summary>
        /// <param name="str">분석하고자 하는 전체 문자열</param>
        /// <param name="ws">문자열에서 명사를 찾는 시작위치</param>
        /// <param name="es">문자열에서 명사를 찾는 끝 위치</param>
        /// <returns></returns>
        private Boolean FindNounWithinStr(String str, int ws, int es) { 
            if(str.Length<es) return false; 
            
            for(int i=es;i<str.Length;i++) { 
                char[] f = SyllableUtil.GetFeature(str[i]); 
                if(i==str.Length || (f[SyllableUtil.IDX_JOSA1]=='1')) { 
                    return (DictionaryUtil.GetWord(str.Substring(ws,i))!=null); 
                } 
            }
            return false; 
        } 
        
        private Boolean IsNounPart(String str, int jstart) { 
            if(true) return false; 
            
            for(int i=jstart-1;i>=0;i--) { 
                if(DictionaryUtil.GetWordExceptVerb(str.Substring(i,jstart+1))!=null) 
                    return true; 
            } 
            return false; 
        }
        
        private void PrintCandidate(WSOutput output) { 
            //List<AnalysisOutput> os = output.Phrases; 
            //foreach (AnalysisOutput o in os) { 
                //System.out.print(o.ToString()+"("+o.Score+")| "); 
            //} 
            //System.out.println("<=="); 
        } 
    }
}
