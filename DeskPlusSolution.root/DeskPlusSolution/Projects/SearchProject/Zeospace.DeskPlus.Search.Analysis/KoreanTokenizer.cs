using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lucene.Net.Analysis;

namespace Zeospace.DeskPlus.Search.Analysis
{
    public class KoreanTokenizer : Tokenizer
    {
        /// <summary>A private instance of the JFlex-constructed scanner </summary>
        private KoreanTokenizerImpl scanner;

        public const int ALPHANUM = 0;
        public const int APOSTROPHE = 1;
        public const int ACRONYM = 2;
        public const int COMPANY = 3;
        public const int EMAIL = 4;
        public const int HOST = 5;
        public const int NUM = 6;
        public const int CJ = 7;
        public const int ACRONYM_DEP = 8;
        public const int KOREAN = 9;
        public const int CHINESE = 10;

        /// <summary>
        /// 정수 상수에 해당하는 문자열 토큰 유형
        /// </summary>
        public static readonly string[] TOKEN_TYPES = new string[] { "<ALPHANUM>", "<APOSTROPHE>", "<ACRONYM>", "<COMPANY>", "<EMAIL>", "<HOST>", "<NUM>", "<CJ>", "<ACRONYM_DEP>", "<KOREAN>", "<CHINESE>" };

        private int maxTokenLength = KoreanAnalyzer.DEFAULT_MAX_TOKEN_LENGTH;

        public KoreanTokenizer(TextReader input)
        {
            this.input = input;
            this.scanner = new KoreanTokenizerImpl(input);
        }

        public virtual Token Current
        {
            get
            {
                return Next(new Token());
            }
        }

        public new Token Next(Token result)
        {

            int posIncr = 1;
            if (result == null)
            {
                result = new Token();
            }

            while (true)
            {
                int tokenType = scanner.GetNextToken();

                if (tokenType == KoreanTokenizerImpl.YYEOF)
                {
                    return null;
                }

                if (scanner.Yylength() <= maxTokenLength)
                {
                    result.Clear();
                    result.SetPositionIncrement(posIncr);
                    scanner.GetText(result);

                    int start = scanner.Yychar();
                    result.SetStartOffset(start);
                    result.SetEndOffset(start + result.TermLength());

                    if (tokenType == KoreanTokenizerImpl.ACRONYM_DEP)
                    {
                        result.SetType(KoreanTokenizerImpl.TOKEN_TYPES[KoreanTokenizerImpl.ACRONYM]);
                    }
                    else
                    {
                        result.SetType(KoreanTokenizerImpl.TOKEN_TYPES[tokenType]);
                    }
                    return result;
                }
                else
                // When we skip a too-long term, we still increment the
                // position increment
                {
                    posIncr++;
                }
            }
        }

        public new void Reset()
        {
            base.Reset();
            scanner.Yyreset(input);
        }

        public new void Reset(TextReader reader)
        {
            input = reader;
            Reset();
        }
    }
}
