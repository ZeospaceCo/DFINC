using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Utils;

namespace Zeospace.DeskPlus.Search.Analysis.Morph
{
    /// <summary>
    /// 공백을 분석한 결과를 저장한다.
    /// </summary>
    public class SpaceOutput
    {
        // 분석된 결과  
        private AnalysisOutput output;

        // 분석 결과 앞에 있는 미등록어, 사람 이름은 대부분 이런 경우임.
        private List<AnalysisOutput> nrWords = new List<AnalysisOutput>();

        // 분석하기 이전의 어절
        private string source;

        public virtual void Initialize()
        {
            output = null;
            nrWords = new List<AnalysisOutput>();
            source = null;
        }

        /// <returns> the output </returns>
        public virtual AnalysisOutput Output
        {
            get
            {
                return output;
            }
            set
            {
                this.output = value;
            }
        }


        /// <returns> the nrWord </returns>
        public virtual List<AnalysisOutput> NRWords
        {
            get
            {
                return nrWords;
            }
            set
            {
                this.nrWords = value;
            }
        }


        /// 
        /// <param name="word"> </param>
        public virtual void AddNRWord(string word)
        {
            AddNRWord(word, AnalysisOutput.SCORE_CORRECT);
        }

        /// 
        /// <param name="word"> </param>
        /// <param name="score"> </param>
        public virtual void AddNRWord(string word, int score)
        {
            AnalysisOutput output = new AnalysisOutput(word, null, null, PatternConstants.PTN_N, score);
            output.Source = word;
            output.Pos = PatternConstants.POS_NOUN;
            this.nrWords.Insert(0, output);
        }

        /// <returns> the source </returns>
        public virtual string Source
        {
            get
            {
                return source;
            }
            set
            {
                this.source = value;
            }
        }


        /// <summary>
        /// 분석된 전체 단어의 길이를 반환한다.
        /// @return
        /// </summary>
        public virtual int Length
        {
            get
            {

                if (this.source == null)
                {
                    return 0;
                }

                return this.source.Length;
            }
        }
    }
}
