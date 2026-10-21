using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Search.Analysis.Morph;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    /// <summary>
    /// 결합이 가능한 조건을 처리하는 클래스
    /// </summary>
    public class ConstraintUtil
    {
		private static Hashtable hahes = new Hashtable();  // "글로벌화해 ", "민족화해" 처럼 화해와 결합이 가능한 명사
        private static Hashtable eomiPnouns = new Hashtable();
        private static Hashtable PTN_MLIST = new Hashtable();
        private static Hashtable PTN_JLIST = new Hashtable();
        private static Hashtable WORD_GUKS = new Hashtable();
        private static Hashtable JOSA_TWO = new Hashtable();
        private static Hashtable JOSA_THREE = new Hashtable();

		static ConstraintUtil()
		{
			hahes["민족"] = "Y";
			hahes["동서"] = "Y";
			hahes["남북"] = "Y";

			eomiPnouns["ㄴ"] = "Y";
            eomiPnouns["ㄹ"] = "Y";
            eomiPnouns["ㅁ"] = "Y";
        
            PTN_MLIST.Add(PatternConstants.PTN_NSM, PatternConstants.PTN_NSM);
		    PTN_MLIST.Add(PatternConstants.PTN_NSMXM, PatternConstants.PTN_NSMXM);
		    PTN_MLIST.Add(PatternConstants.PTN_NJCM, PatternConstants.PTN_NJCM);
		    PTN_MLIST.Add(PatternConstants.PTN_VM, PatternConstants.PTN_VM);
		    PTN_MLIST.Add(PatternConstants.PTN_VMCM, PatternConstants.PTN_VMCM);
		    PTN_MLIST.Add(PatternConstants.PTN_VMXM, PatternConstants.PTN_VMXM);
		    PTN_MLIST.Add(PatternConstants.PTN_NVM, PatternConstants.PTN_NVM);

		    PTN_JLIST.Add(PatternConstants.PTN_NJ, PatternConstants.PTN_NJ);
		    PTN_JLIST.Add(PatternConstants.PTN_NSMJ, PatternConstants.PTN_NSMJ);
		    PTN_JLIST.Add(PatternConstants.PTN_VMJ, PatternConstants.PTN_VMJ);
		    PTN_JLIST.Add(PatternConstants.PTN_VMXMJ, PatternConstants.PTN_VMXMJ);
            
		    WORD_GUKS.Add("날것", "Y");
		    WORD_GUKS.Add("들것", "Y");
		    WORD_GUKS.Add("별것", "Y");
		    WORD_GUKS.Add("찰것", "Y");
		    WORD_GUKS.Add("탈것", "Y");
		    WORD_GUKS.Add("하잘것", "Y");

            JOSA_TWO.Add("가", "Y");
            JOSA_TWO.Add("는", "Y");
            JOSA_TWO.Add("다", "Y");
            JOSA_TWO.Add("나", "Y");
            JOSA_TWO.Add("니", "Y");
            JOSA_TWO.Add("고", "Y");
            JOSA_TWO.Add("라", "Y");
            JOSA_TWO.Add("와", "Y");
            JOSA_TWO.Add("랑", "Y");
            JOSA_TWO.Add("를", "Y");
            JOSA_TWO.Add("며", "Y");
            JOSA_TWO.Add("든", "Y");
            JOSA_TWO.Add("야", "Y");
            JOSA_TWO.Add("여", "Y");

            JOSA_THREE.Add("과", "Y");
            JOSA_THREE.Add("은", "Y");
            JOSA_THREE.Add("아", "Y");
            JOSA_THREE.Add("으", "Y");
            JOSA_THREE.Add("은", "Y");
            JOSA_THREE.Add("을", "Y");

		}

		public static bool CanHaheCompound(string key)
		{
			if (hahes[key] != null)
			{
				return true;
			}
			return false;
		}

		/// <summary>
        ///  어미가 ㄴ,ㄹ,ㅁ 으로 끝나는지 조사한다. </summary>
		/// <param name="eomi">
		/// @return </param>
		public static bool IsNLM(string eomi)
		{

			if (eomi == null || "".Equals(eomi))
			{
				return false;
			}

			if (eomiPnouns[eomi] != null)
			{
				return true;
			}

			char[] chrs = MorphUtil.Decompose(eomi[eomi.Length - 1]);
			if (chrs.Length == 3 && eomiPnouns[char.ToString(chrs[2])] != null)
			{
				return true;
			}

			return true;

		}

		public static bool IsEomiPhrase(int ptn)
		{

			if (PTN_MLIST[ptn] != null)
			{
				return true;
			}

			return false;

		}

		public static bool IsJosaNounPhrase(int ptn)
		{

			if (PTN_JLIST[ptn] != null)
			{
				return true;
			}

			return false;

		}

		public static bool IsJosaAdvPhrase(int ptn)
		{

			if (PatternConstants.PTN_ADVJ == ptn)
			{
				return true;
			}

			return false;

		}

		public static bool IsAdvPhrase(int ptn)
		{

			if (PatternConstants.PTN_ADVJ == ptn || PatternConstants.PTN_AID == ptn)
			{
				return true;
			}

			return false;

		}

		public static bool IsTwoJosa(string josa)
		{

			return (JOSA_TWO[josa] != null);

		}
		public static bool IsThreeJosa(string josa)
		{

			return (JOSA_THREE[josa] != null);

		}
    }
}
