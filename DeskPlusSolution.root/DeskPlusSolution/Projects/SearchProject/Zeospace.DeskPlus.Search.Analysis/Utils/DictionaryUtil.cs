using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;
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
    /// 
    public class DictionaryUtil
    {
		private static Trie<string, WordEntry> dictionary;

		private static Hashtable josas;

		private static Hashtable eomis;

		private static Hashtable prefixs;

		private static Hashtable suffixs;

		private static Dictionary<string, WordEntry> uncompounds;

		private static Dictionary<string, string> cjwords;

		/// <summary>
		/// 사전을 로드한다
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void LoadDictionary()
		{

			dictionary = new Trie<string, WordEntry>(true);
			List<string> strList = null;
			List<string> compounds = null;
			try
			{
				strList = FileUtil.ReadLines(KoreanEnv.Instance.GetValue(KoreanEnv.FILE_DICTIONARY),"UTF-8");
				strList.AddRange(FileUtil.ReadLines(KoreanEnv.Instance.GetValue(KoreanEnv.FILE_EXTENSION),"UTF-8"));
				compounds = FileUtil.ReadLines(KoreanEnv.Instance.GetValue(KoreanEnv.FILE_COMPOUNDS),"UTF-8");
			}
			catch (Exception e)
			{
				new MorphException(e.Message,e);
			}
            //catch (Exception e)
            //{
            //    new MorphException(e.Message,e);
            //}
			if (strList == null)
			{
				throw new MorphException("dictionary is null");
			}

			foreach (string str in strList)
			{
				string[] infos = StringUtil.Split(str,",");
				if (infos.Length != 2)
				{
					continue;
				}
				infos[1] = infos[1].Trim();
				if (infos[1].Length == 6)
				{
					infos[1] = infos[1].Substring(0,5) + "000" + infos[1].Substring(5);
				}

				WordEntry entry = new WordEntry(infos[0].Trim(),infos[1].Trim().ToCharArray());
				dictionary.Add(entry.Word, entry);
			}

			foreach (string compound in compounds)
			{
				string[] infos = StringUtil.Split(compound,":");
				if (infos.Length != 2)
				{
					continue;
				}
				WordEntry entry = new WordEntry(infos[0].Trim(),"20000000X".ToCharArray());
				entry.Compounds = CompoundArrayToList(infos[1], StringUtil.Split(infos[1],","));
				dictionary.Add(entry.Word, entry);
			}
		}


		public static IEnumerator FindWithPrefix(string prefix)
		{
			if (dictionary == null)
			{
				LoadDictionary();
			}
			return dictionary.GetPrefixedBy(prefix);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetWord(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetWord(string key)
		{
			if (dictionary == null)
			{
				LoadDictionary();
			}
			if (key.Length == 0)
			{
				return null;
			}

			return (WordEntry)dictionary.Get(key);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetWordExceptVerb(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetWordExceptVerb(string key)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_NOUN) == '1' || entry.GetFeature(WordEntry.IDX_BUSA) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetNoun(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetNoun(string key)
		{

			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_NOUN) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetCNoun(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetCNoun(string key)
		{

			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_NOUN) == '1' || entry.GetFeature(WordEntry.IDX_NOUN) == '2')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetVerb(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetVerb(string key)
		{

			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_VERB) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetAdverb(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetAdverb(string key)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_BUSA) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetBusa(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetBusa(string key)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_BUSA) == '1' && entry.GetFeature(WordEntry.IDX_NOUN) == '0')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetIrrVerb(String key, char irrType) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetIrrVerb(string key, char irrType)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_VERB) == '1' && entry.GetFeature(WordEntry.IDX_REGURA) == irrType)
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetBeVerb(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetBeVerb(string key)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_BEV) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetDoVerb(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetDoVerb(string key)
		{
			WordEntry entry = GetWord(key);
			if (entry == null)
			{
				return null;
			}

			if (entry.GetFeature(WordEntry.IDX_DOV) == '1')
			{
				return entry;
			}
			return null;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static org.apache.lucene.Analysis.kr.morph.WordEntry GetUnCompound(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static WordEntry GetUnCompound(string key)
		{

			try
			{
				if (uncompounds == null)
				{
					uncompounds = new Dictionary<string, WordEntry>();
					List<string> lines = FileUtil.ReadLines(KoreanEnv.Instance.GetValue(KoreanEnv.FILE_UNCOMPOUNDS),"UTF-8");
					foreach (string compound in lines)
					{
						string[] infos = StringUtil.Split(compound,":");
						if (infos.Length != 2)
						{
							continue;
						}
						WordEntry entry = new WordEntry(infos[0].Trim(),"90000X".ToCharArray());
						entry.Compounds = CompoundArrayToList(infos[1], StringUtil.Split(infos[1],","));
						uncompounds[entry.Word] = entry;
					}
				}
			}
			catch (Exception e)
			{
				throw new MorphException(e);
			}
			return uncompounds[key];
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static String GetCJWord(String key) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static string GetCJWord(string key)
		{

			try
			{
				if (cjwords == null)
				{
					cjwords = new Dictionary<string, string>();
					List<string> lines = FileUtil.ReadLines(KoreanEnv.Instance.GetValue(KoreanEnv.FILE_CJ),"UTF-8");
					foreach (string cj in lines)
					{
						string[] infos = StringUtil.Split(cj,":");
						if (infos.Length != 2)
						{
							continue;
						}
						cjwords[infos[0]] = infos[1];
					}
				}
			}
			catch (Exception e)
			{
				throw new MorphException(e);
			}
			return cjwords[key];

		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static Boolean ExistJosa(String str) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static bool ExistJosa(string str)
		{
			if (josas == null)
			{
				josas = new Hashtable();
				ReadFile(josas,KoreanEnv.FILE_JOSA);
			}
			if (josas[str] == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static Boolean ExistEomi(String str) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static bool ExistEomi(string str)
		{
			if (eomis == null)
			{
				eomis = new Hashtable();
				ReadFile(eomis,KoreanEnv.FILE_EOMI);
			}

			if (eomis[str] == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static Boolean ExistPrefix(String str) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static bool ExistPrefix(string str)
		{
			if (prefixs == null)
			{
				prefixs = new Hashtable();
				ReadFile(prefixs,KoreanEnv.FILE_PREFIX);
			}

			if (prefixs[str] == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static Boolean ExistSuffix(String str) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static bool ExistSuffix(string str)
		{
			if (suffixs == null)
			{
				suffixs = new Hashtable();
				ReadFile(suffixs,KoreanEnv.FILE_SUFFIX);
			}

			if (suffixs[str] != null)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// ㄴ,ㄹ,ㅁ,ㅂ과 eomi 가 결합하여 어미가 될 수 있는지 점검한다.</summary>
		/// <param name="s"> </param>
		/// <param name="end">
		/// @return </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static String CombineAndEomiCheck(char s, String eomi) throws org.apache.lucene.Analysis.kr.morph.MorphException
		public static string CombineAndEomiCheck(char s, string eomi)
		{

			if (eomi == null)
			{
				eomi = "";
			}

			if (s == 'ㄴ') eomi = "은" + eomi; 
            else if (s == 'ㄹ') eomi = "을" + eomi; 
            else if(s=='ㅁ') eomi = "음" + eomi; 
            else if (s == 'ㅂ') eomi = "습" + eomi; 
            else eomi = s+eomi; 
            
            if(ExistEomi(eomi)) return eomi; 
            
            return null; 
        } 
        
		/// 
		/// <param name="map"> </param>
		/// <param name="type">	1: josa, 2: eomi </param>
		/// <exception cref="MorphException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static synchronized void ReadFile(java.util.HashMap map, String dic) throws org.apache.lucene.Analysis.kr.morph.MorphException
		[MethodImpl(MethodImplOptions.Synchronized)]
		private static void ReadFile(Hashtable map, string dic)
		{

			string path = KoreanEnv.Instance.GetValue(dic);

			try
			{
				List<string> line = FileUtil.ReadLines(path,"UTF-8");
				for (int i = 1;i < line.Count;i++)
				{
					map[line[i].Trim()] = line[i];
				}
			}
			catch (IOException e)
			{
				 throw new MorphException(e.Message,e);
			}
			catch (Exception e)
			{
				 throw new MorphException(e.Message,e);
			}
		}

        private static List<CompoundEntry> CompoundArrayToList(string source, string[] arr)
		{
            List<CompoundEntry> list = new List<CompoundEntry>();
            
			foreach (string str in arr)
			{
				CompoundEntry ce = new CompoundEntry(str);
				ce.Offset = source.IndexOf(str);
				list.Add(ce);
			}
			return list;
		}

    }
}
