using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Analysis.CJK;
using Lucene.Net.Analysis.NGram;
using Version = Lucene.Net.Util.Version;
using System.IO;
using System.Collections;
using Zeospace.DeskPlus.Search.Analysis;

namespace Zeospace.DeskPlus.Search.Web.Prototype
{
    public partial class AnalyzerDemo : System.Web.UI.Page
    {
        //"The quick brown fox jumped over the lazy dogs", "XY&Z Corporation - xyz@example.com", 
        private string[] example = { "1. 실행시 필요한 파일들 : hdic/KLT2000.ini, hdic/*.dic - hdic/KLT2000.ini 다른 디렉토리로 옮기고 싶을 때는 src/coran.c에서 open_HAM()를 호출한 후에 아래와 같이 set_options_from_file()을 호출. open_HAM(...); set_options_from_file(\"./hdic/KLT2000.ini\", &mode); - hdic/*.dic : 이 파일은 실행파일이 있는 디렉토리에 있어야 함. 다른 디렉토리로 옮기고 싶을 때는 KLT2000.ini에서 사전경로를 수정하면 됨. 2. 라이브러리 link할 때 필요한 파일들 - libindex.a - header/*.h - hdic2/*.h 2004년 8월 9일 국민대학교 컴퓨터학부 강승식(sskang@kookmin.ac.kr +82-2-910-4800, 017-226-4136) " };

        // new WhitespaceAnalyzer(), new SimpleAnalyzer(), new StopAnalyzer(), 
        private Lucene.Net.Analysis.Analyzer[] analyzers = new Lucene.Net.Analysis.Analyzer[] {new StandardAnalyzer(), new CJKAnalyzer()}; 
        //private Lucene.Net.Analysis.Analyzer[] analyzers = new Lucene.Net.Analysis.Analyzer[] { new KoreanAnalyzer() }; 
        //private Lucene.Net.Analysis.Analyzer[] analyzers = new Lucene.Net.Analysis.Analyzer[] {new StandardAnalyzer(Version.LUCENE_29) }; 
        
        protected void Page_Load(object sender, EventArgs e)
        {
            String[] strings = example;

            for (int i = 0; i < strings.Length; i++)
            {
                analyze(strings[i]);
            }
        }


        private void analyze(String text)
        {
            Response.Write("Analyzing " + text + "<br/><hr/>");


            for (int i = 0; i < analyzers.Length; i++)
            {
                Lucene.Net.Analysis.Analyzer analyzer = analyzers[i];

                string name = analyzer.GetType().ToString();

                name = name.Substring(name.LastIndexOf(".") + 1);
                Response.Write("  " + name + "  <br/>");
                Response.Write("     ");
                displayTokens(analyzer, text);
                Response.Write("<br/>");
                displayTokensWithFullDetails(analyzer, text);
                Response.Write("<br/><hr/>");
            }
        }

        private Lucene.Net.Analysis.Token[] tokensFromAnalysis(Analyzer analyzer, string Text)
        {
            TokenStream stream = analyzer.TokenStream("contents", new StringReader(Text));

            //ArrayList tokenList = new ArrayList();

            List<Lucene.Net.Analysis.Token> tokenList = new List<Lucene.Net.Analysis.Token>();
           
            while (true)
            {
                Lucene.Net.Analysis.Token token = stream.Next();
                if (token == null) break;

                tokenList.Add(token);
            }

            //while (stream.IncrementToken())
            //{
                
            //    if (token == null) break;

            //    tokenList.Add(token);
            //}

            return (Lucene.Net.Analysis.Token[])tokenList.ToArray();
        }

        /// <summary>
        /// 토큰 분석
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="text"></param>
        private void displayTokens(Analyzer analyzer, String text)
        {
            Lucene.Net.Analysis.Token[] tokens = tokensFromAnalysis(analyzer, text);

            for (int i = 0; i < tokens.Length; i++)
            {
                Lucene.Net.Analysis.Token token = tokens[i];

                Response.Write("[" + token.TermText() + "] ");
            }
        }

        /// <summary>
        /// 상세 토큰 분석
        /// </summary>
        /// <param name="analyzer"></param>
        /// <param name="text"></param>
        private void displayTokensWithFullDetails(Analyzer analyzer, String text)
        {
            Lucene.Net.Analysis.Token[] tokens = tokensFromAnalysis(analyzer, text);

            int position = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                Lucene.Net.Analysis.Token token = tokens[i];

                int increment = token.GetPositionIncrement();

                if (increment > 0)
                {
                    position = position + increment;

                    Response.Write("<br/>");
                    Response.Write(position + " : ");
                }

                Response.Write("[" + token.TermText() + ":" + token.StartOffset() + "->" + token.EndOffset() + ":" + token.Type() + "] ");
            }
        }

    }


}