using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Zeospace.DeskPlus.Search.Analysis.Morph;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    public class HanjaUtils
    {
        private static IDictionary<string, char[]> mapHanja;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void LoadDictionary()
        {
            try
            {
                List<string> strList = FileUtil.ReadLines("org/apache/lucene/Analysis/kr/dic/mapHanja.dic", "UTF-8");
                mapHanja = new Dictionary<string, char[]>();

                for (int i = 0; i < strList.Count; i++)
                {

                    if (strList[i].Length < 1 || strList[i].IndexOf(",") == -1)
                    {
                        continue;
                    }

                    string[] hanInfos = StringUtil.Split(strList[i], ",");

                    if (hanInfos.Length != 2)
                    {
                        continue;
                    }

                    string hanja = StringEscapeUtil.UnescapeJava(hanInfos[0]);

                    mapHanja[hanja] = hanInfos[1].ToCharArray();
                }
            }
            catch (IOException e)
            {
                throw new MorphException(e);
            }
        }

        public static char[] ConvertToHangul(char hanja)
        {

            if (mapHanja == null)
            {
                LoadDictionary();
            }

            if (hanja > 0x9FFF || hanja < 0x3400)
            {
                return new char[] { hanja };
            }

            return mapHanja[new string(new char[] { hanja })];
        }

    }
}
