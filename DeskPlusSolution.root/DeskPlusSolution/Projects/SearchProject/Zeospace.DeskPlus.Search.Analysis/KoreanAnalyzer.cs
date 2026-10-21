using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text;
using Lucene.Net.Analysis;

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
    public class KoreanAnalyzer : Analyzer
    {
        private HashSet<string> stopSet;
        private Boolean bigrammable = true;
        private Boolean hasOrigin = true;

        /** 검색에 ​​유용하지 않은 일반적인 영어 단어를 포함하는 배열입니다. */
        public static readonly string[] STOP_WORDS = new String[]{
	      "a", "an", "and", "are", "as", "at", "be", "but", "by",
	      "for", "if", "in", "into", "is", "it",
	      "no", "not", "of", "on", "or", "such",
	      "that", "the", "their", "then", "there", "these",
	      "they", "this", "to", "was", "will", "with"	
        };
        public static readonly string[] KOR_STOP_WORDS = new String[] { "이", "그", "저", "것", "수", "등", "들" };
        public const string DIC_ENCODING = "UTF-8";

        /** 최대 토큰 길이 */
        public const int DEFAULT_MAX_TOKEN_LENGTH = 255;

        public KoreanAnalyzer() : this(STOP_WORDS) {}

        
        public KoreanAnalyzer(string[] stopWords)
        {
            stopSet = new HashSet<string>();
            stopSet.UnionWith(StopFilter.MakeStopSet(stopWords));

            //foreach (string str in StopFilter.MakeStopSet(stopWords))
            //{
            //    stopSet.Add(str);
            //}

            stopSet.UnionWith(StopFilter.MakeStopSet(KOR_STOP_WORDS));

            //foreach (string str in StopFilter.MakeStopSet(KOR_STOP_WORDS))
            //{
            //    stopSet.Add(str);
            //}
        }

        //주어진 파일로 단어 분석기를 구축합니다.
        public KoreanAnalyzer(FileInfo stopWords)
        {
            stopSet.UnionWith(WordlistLoader.GetWordSet(stopWords));
            //foreach (string str in WordlistLoader.GetWordSet(stopwords))
            //{
            //    stopSet.Add(str);
            //}
        }

        //주어진 TextReader 객체로 단어 분석기를 구축합니다.
        public KoreanAnalyzer(TextReader stopWords)
        {
            stopSet.UnionWith(WordlistLoader.GetWordSet(stopWords));
            //foreach (string str in WordlistLoader.GetWordSet(stopwords))
            //{
            //    stopSet.Add(str);
            //}
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            KoreanTokenizer tokenStream = new KoreanTokenizer(reader);
            TokenStream result = new KoreanFilter(tokenStream, bigrammable, hasOrigin);

            //Lucene.Net.Analysis.Standard.StandardTokenizer tokenStream = new Lucene.Net.Analysis.Standard.StandardTokenizer(reader);
            //TokenStream result = new Lucene.Net.Analysis.Standard.StandardFilter(tokenStream);

            //Lucene.Net.Analysis.Standard.StandardTokenizer tokenStream = new Lucene.Net.Analysis.Standard.StandardTokenizer(reader);
            //TokenStream result = new KoreanFilter(tokenStream, bigrammable, hasOrigin);




            result = new LowerCaseFilter(result);
            result = new StopFilter(result, stopSet);
            return result;
        }

        /// <summary>
        /// 입력 단어 분석에 실패하는 경우 bigram 색인 용어가 반환 여부를 결정 
        /// </summary>
        public virtual bool Bigrammable
        {
            set
            {
                bigrammable = value;
            }
        }

        /// <summary>
        /// 입력 단어가 형태소 분석하는 경우 원래 용어의 반환 여부를 결정
        /// </summary>
        public virtual bool HasOrigin
        {
            set
            {
                hasOrigin = value;
            }
        }
    }
}
