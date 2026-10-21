using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Zeospace.DeskPlus.Search.Analysis.Morph;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis
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

    public class KoreanFilter : TokenFilter
    {
        private LinkedList<Token> koreanQueue;

        private LinkedList<Token> cjQueue;

        private MorphAnalyzer morph;

        private WordSpaceAnalyzer wsAnal;

        private bool bigrammable = true;

        private bool hasOrigin = true;

        private static readonly string APOSTROPHE_TYPE = KoreanTokenizerImpl.TOKEN_TYPES[KoreanTokenizerImpl.APOSTROPHE];
        private static readonly string ACRONYM_TYPE = KoreanTokenizerImpl.TOKEN_TYPES[KoreanTokenizerImpl.ACRONYM];

        private CompoundNounAnalyzer cnAnalyzer = new CompoundNounAnalyzer();

        public KoreanFilter(TokenStream input)
            : base(input)
        {
            koreanQueue = new LinkedList<Token>();
            cjQueue = new LinkedList<Token>();
            morph = new MorphAnalyzer();
            wsAnal = new WordSpaceAnalyzer();
            cnAnalyzer.ExactMach = false;
        }

        /// 
        /// <param name="input">	input token stream </param>
        /// <param name="bigram">	Whether the bigram index term return or not. </param>
        public KoreanFilter(TokenStream input, bool bigram)
            : this(input)
        {
            bigrammable = bigram;
        }

        public KoreanFilter(TokenStream input, bool bigram, bool has)
            : this(input, bigram)
        {
            hasOrigin = has;
        }

        public override Token Next(Token result)
        {

            if (koreanQueue.Count > 0)
            {
                Token tmpToken = koreanQueue.First();
                koreanQueue.RemoveFirst();

                return tmpToken;
            }
            else if (cjQueue.Count > 0)
            {
                Token tmpCjQueue = cjQueue.First();
                cjQueue.RemoveFirst();

                return tmpCjQueue;
            }

            int skippedPositions = 0;

            try
            {
                while ((result = input.Next(result)) != null)
                {

                    if (result.Type().Equals(KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.KOREAN]))
                    {
                        result = AnalysisKorean(result, skippedPositions);
                    }
                    else if (result.Type().Equals(KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.CHINESE]))
                    {
                        result = AnalysisChinese(result, skippedPositions);
                    }
                    else
                    {
                        result = AnalysisETC(result);
                    }

                    if (result == null)
                    {
                        skippedPositions++;
                        continue;
                    }

                    return result;
                }
            }
            catch (MorphException e)
            {
                //throw new  IOException(e.Message);
            }

            return null;
        }

        /// <summary>
        /// 한글을 분석한다. </summary>
        /// <param name="token"> </param>
        /// <param name="skipinc">
        /// @return </param>
        /// <exception cref="MorphException"> </exception>
        private Token AnalysisKorean(Token token, int skipinc)
        {

            string input = token.TermText();
            List<AnalysisOutput> outputs = morph.Analyze(input);
            if (outputs.Count == 0)
            {
                return null;
            }

            Dictionary<string, int> map = new Dictionary<string, int>();
            if (hasOrigin)
            {
                map.Add(input, 1);
            }

            if (outputs[0].Score == AnalysisOutput.SCORE_CORRECT)
            {
                ExtractKeyword(outputs, map);
            }
            else
            {
                try
                {
                    List<AnalysisOutput> list = wsAnal.Analyze(input);

                    List<AnalysisOutput> results = new List<AnalysisOutput>();
                    if (list.Count > 1)
                    {
                        foreach (AnalysisOutput o in list)
                        {
                            if (hasOrigin)
                            {
                                map.Add(o.Source, 1);
                            }
                            results.AddRange(morph.Analyze(o.Source));
                        }
                    }
                    else
                    {
                        results.AddRange(list);
                    }

                    ExtractKeyword(results, map);
                }
                catch (Exception e)
                {
                    ExtractKeyword(outputs, map);
                }
            }

            IEnumerator<string> iter = map.Keys.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {

                string text = iter.Current;

                if (text.Length <= 1)
                {
                    continue;
                }

                int index = input.IndexOf(text);
                Token t = new Token(text, token.StartOffset() + (index != -1 ? index : 0), index != -1 ? token.StartOffset() + index + text.Length : token.EndOffset(), KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.KOREAN]);
                if (i == 0)
                {
                    t.SetPositionIncrement(token.GetPositionIncrement() + skipinc);
                }
                else
                {
                    t.SetPositionIncrement(0);
                }
                koreanQueue.AddLast(t);
                i++;
            }

            if (koreanQueue.Count == 0)
            {
                return null;
            }

            Token tmpTokent = koreanQueue.First();
            koreanQueue.RemoveFirst();
            return tmpTokent;
        }

        private void ExtractKeyword(List<AnalysisOutput> outputs, Dictionary<string, int> map)
        {

            foreach (AnalysisOutput output in outputs)
            {

                if (output.Pos != PatternConstants.POS_VERB)
                {
                    map.Add(output.Stem, 1);
                    //			}else {
                    //				map.Put(output.Stem+"??, new Integer(1));	
                }

                if (output.Score >= AnalysisOutput.SCORE_COMPOUNDS)
                {
                    List<CompoundEntry> cnouns = output.CNounList;
                    for (int jj = 0; jj < cnouns.Count; jj++)
                    {
                        CompoundEntry cnoun = cnouns[jj];
                        if (cnoun.Word.Length > 1)
                        {
                            map.Add(cnoun.Word, 0);
                        }
                        if (jj == 0 && cnoun.Word.Length == 1)
                        {
                            map.Add(cnoun.Word + cnouns[jj + 1].Word, 0);
                        }
                        else if (jj > 1 && cnoun.Word.Length == 1)
                        {
                            map.Add(cnouns[jj].Word + cnoun.Word, 0);
                        }
                    }
                }
                else if (bigrammable)
                {
                    AddBiagramToMap(output.Stem, map);
                }
            }

        }

        private void AddBiagramToMap(string input, Dictionary<string, int> map)
        {
            int offset = 0;
            int strlen = input.Length;
            while (offset < strlen - 1)
            {
                if (isAlphaNumChar(input[offset]))
                {
                    string text = findAlphaNumeric(input.Substring(offset));
                    map.Add(text, 0);
                    offset += text.Length;
                }
                else
                {
                    string text = input.Substring(offset, 2 > strlen ? strlen : offset + 2);
                    map.Add(text, 0);
                    offset++;
                }
            }
        }

        private string findAlphaNumeric(string text)
        {
            int pos = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!isAlphaNumChar(text[i]))
                {
                    break;
                }
                pos++;
            }
            return text.Substring(0, pos);
        }

        /**
         * 한자는 2개이상의 한글 음으로 읽혀질 수 있다.
         * 두음법칙은 적용을 하지 않았음.
         * @param term
         * @throws MorphException
         */
        private Token AnalysisChinese(Token token, int skipinc)
        {

            string input = token.Term();

            Token t = new Token(input, token.StartOffset(), token.EndOffset(), KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.CJ]);
            t.SetPositionIncrement(token.GetPositionIncrement() + skipinc);
            cjQueue.AddLast(t); //한자 자체를 색인어로 저장한다.

            if (input.Length < 2) // 1글자 한자는 색인어로 한글을 추출하지 않는다. 
            {
                Token tmpToken = cjQueue.First();
                cjQueue.RemoveFirst();
                return tmpToken;
            }

            List<StringBuilder> candiList = new List<StringBuilder>();
            candiList.Add(new StringBuilder());

            for (int i = 0; i < input.Length; i++)
            {

                char[] chs = HanjaUtils.ConvertToHangul(input[i]);
                if (chs.Length < 1)
                {
                    continue;
                }

                int caniSize = candiList.Count;
                for (int j = 0; j < caniSize; j++)
                {
                    StringBuilder sb = candiList[j];

                    for (int k = 1; k < chs.Length; k++) // 추가로 생성된 음에 대해서 새로운 텍스트를 생성한다.
                    {
                        if (k == 1) // 2개 이상의 음을 가지고 있는 경우 첫번째 음으로만 처리를 한다.
                        {
                            break;
                        }
                        StringBuilder cpSb = new StringBuilder(sb.ToString());
                        cpSb.Append(chs[k]);

                        candiList.Add(cpSb);
                    }

                    sb.Append(chs[0]);
                }
            }

            // 2개의 이상의 음을 가진 한자로 인해 5개 이상 문자열이 만들어지면 5개까지만.. 처리한다.
            int maxCandidate = 5;
            if (candiList.Count < maxCandidate)
            {
                maxCandidate = candiList.Count;
            }

            for (int i = 0; i < maxCandidate; i++)
            {
                Token t1 = new Token(candiList[i].ToString(), token.StartOffset(), token.EndOffset(), KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.KOREAN]);
                t1.SetPositionIncrement(0);
                cjQueue.AddLast(t1);
            }

            Dictionary<string, string> cnounMap = new Dictionary<string, string>();

            // 추출된 명사가 복합명사인 경우 분리한다.
            for (int i = 0; i < maxCandidate; i++)
            {
                List<CompoundEntry> results = ConfirmCNoun(candiList[i].ToString());

                int pos = 0;
                foreach (CompoundEntry entry in results)
                {
                    pos += entry.Word.Length;
                    if (cnounMap[entry.Word] != null)
                    {
                        continue;
                    }

                    try
                    {
                        // 한글과 매치되는 한자를 짤라서 큐에 저장한다.
                        Token t1 = new Token(input.Substring(pos - entry.Word.Length, pos - (pos - entry.Word.Length)), token.StartOffset() + pos - entry.Word.Length, token.StartOffset() + pos, KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.CJ]);
                        t1.SetPositionIncrement(0);
                        cjQueue.AddLast(t1);

                        cnounMap[entry.Word] = entry.Word;

                        if (entry.Word.Length < 2) //  한글은 2글자 이상만 저장한다.
                        {
                            continue;
                        }

                        // 분리된 한글을 큐에 저장한다.			   
                        t1 = new Token(entry.Word, token.StartOffset() + pos - entry.Word.Length, token.StartOffset() + pos, KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.KOREAN]);
                        t1.SetPositionIncrement(0);
                        cjQueue.AddLast(t1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception occured while AnalysisChinese : \n" + input + ":" + pos + ":" + entry.Word.Length + ":" + entry.Word);
                        throw new MorphException(e);
                    }

                }

            }

            Token tmpCjQuery = cjQueue.First();
            cjQueue.RemoveFirst();

            return tmpCjQuery;
        }

        private List<CompoundEntry> ConfirmCNoun(string input)
        {

            WordEntry cnoun = DictionaryUtil.GetCNoun(input);
            if (cnoun != null && cnoun.GetFeature(WordEntry.IDX_NOUN) == '2')
            {
                return cnoun.Compounds;
            }

            return cnAnalyzer.Analyze(input);

        }


        //private Token AnalysisCJ(Token token, int skipinc) throws MorphException {
        		
        //    String input = token.termText();
        		
        //    Token t = new Token(input,0,token.endOffset(),KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.CJ]);
        //    t.setPositionIncrement(token.getPositionIncrement()+skipinc);
        //    cjQueue.Add(t);
        		
        //    String kor = DictionaryUtil.GetCJWord(input);	
        //    if(kor!=null) {			
        //        Token t1 = new Token(kor,0,token.endOffset(),KoreanTokenizer.TOKEN_TYPES[KoreanTokenizer.CJ]);
        //        t1.setPositionIncrement(0);
        //        cjQueue.Add(t1);			
        //    }
        			
        //    return cjQueue.removeFirst();
        //}

        private Token AnalysisETC(Token t)
        {
            char[] buffer = t.TermBuffer();

            int bufferLength = t.TermLength();
            string type = t.Type();

            if (type == APOSTROPHE_TYPE && bufferLength >= 2 && buffer[bufferLength - 2] == '\'' && (buffer[bufferLength - 1] == 's' || buffer[bufferLength - 1] == 'S')) // remove 's
            {
                // Strip last 2 characters off
                t.SetTermLength(bufferLength - 2);
            } // remove dots
            else if (type == ACRONYM_TYPE)
            {
                int upto = 0;
                for (int i = 0; i < bufferLength; i++)
                {
                    char c = buffer[i];
                    if (c != '.')
                    {
                        buffer[upto++] = c;
                    }
                }
                t.SetTermLength(upto);
            }

            return t;
        }

        private bool isAlphaNumChar(int c)
        {
            if ((c >= 48 && c <= 57) || (c >= 65 && c <= 122))
            {
                return true;
            }
            return false;
        }

        public virtual bool HasOrigin
        {
            set
            {
                hasOrigin = value;
            }
        }
    }
}
