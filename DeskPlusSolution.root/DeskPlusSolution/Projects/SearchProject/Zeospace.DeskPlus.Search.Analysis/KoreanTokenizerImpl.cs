using System;
using System.IO;
using System.Text;
using Lucene.Net.Analysis.Tokenattributes;
using Token = Lucene.Net.Analysis.Token;

namespace Zeospace.DeskPlus.Search.Analysis
{
    class KoreanTokenizerImpl
    {
	  /// <summary>
	  /// 퍄일의 끝 </summary>
	  public const int YYEOF = -1;

	  /// <summary>
	  /// 버퍼의 초기 크기 </summary>
	  private const int ZZ_BUFFERSIZE = 16384;

	  /// <summary>
	  /// 어휘 주 </summary>
	  public const int YYINITIAL = 0;

	  /// <summary>
	  /// 문자 클래스에 문자를 번역
	  /// </summary>
      private const String ZZ_CMAP_PACKED =
            "\x000B\x0000\x0001\x0000\x0001\x0014\x0001\x0000\x0001\x0000\x0001\x0011\x0016\x0000\x0001\x0000\x0005\x0000\x0001\x0003"
          + "\x0001\x0001\x0004\x0000\x0001\x0007\x0001\x0005\x0001\x0002\x0001\x0007\x000C\x000B\x0006\x0000\x0001\x0004\x0020\x000A"
          + "\x0004\x0000\x0001\x0006\x0001\x0000\x0020\x000A\x0069\x0000\x001B\x000A\x0001\x0000\x0025\x000A\x0001\x0000\u0568\x000A"
          + "\x000C\x000C\x00CE\x000A\x000C\x000C\u026c\x000A\x000C\x000C\x00A6\x000A\x000C\x000C\x00A6\x000A\x000C\x000C\x00A6\x000A"
          + "\x000C\x000C\x00A6\x000A\x000C\x000C\x00A7\x000A\x000B\x000C\x00A6\x000A\x000C\x000C\x00A6\x000A\x000C\x000C\x00A6\x000A"
          + "\x000C\x000C\x0154\x000A\x000C\x000C\x00A6\x000A\x000C\x000C\u0166\x000A\x000C\x000C\x010A\x000A\u0100\x000E\u0e00\x000A"
          + "\u1040\x0000\u0150\x0010\x008C\x0000\x0014\x0010\u0100\x0000\x00C8\x0010\x00C8\x0000\u19c0\x000F\x0064\x0000\u5200\x000F"
          + "\u0c00\x0000\u2bb0\x000D\u2150\x0000\u0200\x000F\u0465\x0000\x0049\x0010\x004B\x000A\x002B\x0000";




	  /// <summary>
	  /// 문자 클래스에 문자를 번역
	  /// </summary>
	  private static readonly char[] ZZ_CMAP = ZzUnpackCMap(ZZ_CMAP_PACKED);

	  /// <summary>
	  /// Translates DFA states to action switch labels.
	  /// </summary>
	  private static readonly int[] ZZ_ACTION = ZzUnpackAction();

      private const String ZZ_ACTION_PACKED_0 = 
            "\x0001\x0000\x0002\x0001\x0003\x0002\x0002\x0003\x0001\x0004\x0001\x0005\x0001\x0001\x0001\x0006"
          + "\x0006\x0000\x0003\x0002\x0001\x0003\x0001\x0006\x0003\x0000\x0001\x0002\x0001\x0003\x0001\x0006"
          + "\x0001\x0002\x0004\x0003\x0001\x0004\x0003\x0000\x0001\x0003\x0001\x0007\x0003\x000A\x0002\x000B"
          + "\x0002\x0000\x0002\x0006\x0002\x0000\x0002\x000A\x0001\x0003\x0004\x0006\x0001\x0000\x0002\x000A"
          + "\x0001\x0003\x0004\x0000\x0002\x0003\x0001\x000C\x0001\x0000\x0001\x000A\x0001\x000D\x0002\x0000"
          + "\x0002\x000A\x0001\x0003\x0001\x000C\x0001\x0000\x0002\x0006\x0001\x0003\x0002\x0006\x0001\x0003"
          + "\x0002\x000D\x0001\x000A\x0002\x0003\x0001\x0006\x0002\x0003\x0001\x0006\x0002\x0003\x0002\x000C"
          + "\x0001\x0000\x0002\x000A\x0001\x0000\x0001\x000D\x0001\x000A\x0002\x0006\x0001\x000D\x0002\x0000"
          + "\x0001\x000D\x0001\x0003\x0001\x000A\x0001\x000E\x0001\x0000\x0001\x0003\x0001\x0000\x0001\x0003"
          + "\x0001\x000D\x0001\x0000\x0001\x0003\x0001\x0000\x0001\x0003\x0001\x0000\x0001\x000A\x0001\x0003"
          + "\x0001\x0006\x0001\x0003\x0001\x0006\x0001\x0003\x0001\x000A\x0002\x0006\x0001\x000D\x0002\x0000"
          + "\x0002\x0003\x0001\x0000\x0002\x0003\x0001\x0000\x0002\x0003\x0001\x000D\x0001\x000A\x0001\x0003"
          + "\x0002\x0000\x0002\x0003\x0001\x000D\x0001\x0000\x0002\x000A\x0002\x0000\x0001\x000D\x0001\x0000"
          + "\x0002\x000A\x0001\x0003\x0002\x0000\x0001\x0003\x0001\x000A\x0001\x0000\x0001\x000D\x0001\x0000"
          + "\x0001\x000D\x0001\x0003\x0001\x0000\x0003\x0003";

	  private static int[] ZzUnpackAction()
	  {
		int[] result = new int[174];
		int offset = 0;
		offset = ZzUnpackAction(ZZ_ACTION_PACKED_0, offset, result);
		return result;
	  }

	  private static int ZzUnpackAction(string packed, int offset, int[] result)
	  {
		int i = 0; // index in packed string
		int j = offset; // index in unpacked array
		int l = packed.Length;
		while (i < l)
		{
		  int count = packed[i++];
          int value_Renamed = packed[i++];
		  do
		  {
              result[j++] = value_Renamed;
		  } while (--count > 0);
		}
		return j;
	  }


	  /// <summary>
	  /// 전환 테이블 행 인덱스에 상태를 번역
	  /// </summary>
	  private static readonly int[] ZZ_ROWMAP = ZzUnpackRowMap();

      private const String ZZ_ROWMAP_PACKED_0 = 
            "\x0000\x0000\x0000\x0015\x0000\x002A\x0000\x003F\x0000\x0068\x0000\x007D\x0000\x0092\x0000\x00A7"
          + "\x0000\x00D2\x0000\x00E7\x0000\x00FC\x0000\x0111\x0000\x013A\x0000\x014F\x0000\x0164\x0000\x0179"
          + "\x0000\u0110\x0000\u0121\x0000\u0132\x0000\u0143\x0000\u0154\x0000\u0165\x0000\u0176\x0000\u0187"
          + "\x0000\u0198\x0000\u01a9\x0000\u01ba\x0000\u01cb\x0000\u01dc\x0000\u01ed\x0000\u01fe\x0000\u020f"
          + "\x0000\u0220\x0000\u0231\x0000\u0242\x0000\u0253\x0000\u0264\x0000\u0275\x0000\u0286\x0000\u0297"
          + "\x0000\u02a8\x0000\u02b9\x0000\u02ca\x0000\x0164\x0000\u02db\x0000\u02ec\x0000\u02fd\x0000\u030e"
          + "\x0000\u031f\x0000\u0330\x0000\u0176\x0000\u0341\x0000\u0352\x0000\u0363\x0000\u0374\x0000\u0385"
          + "\x0000\u0396\x0000\u03a7\x0000\u03b8\x0000\u03c9\x0000\u03da\x0000\u03eb\x0000\x0111\x0000\u03fc"
          + "\x0000\u040d\x0000\u041e\x0000\u042f\x0000\u0440\x0000\u0451\x0000\u0462\x0000\u0473\x0000\u0484"
          + "\x0000\u0495\x0000\u04a6\x0000\u04b7\x0000\u04c8\x0000\u04d9\x0000\u04ea\x0000\u04fb\x0000\u050c"
          + "\x0000\u051d\x0000\u052e\x0000\u053f\x0000\u0550\x0000\u0561\x0000\u0330\x0000\u0176\x0000\u0572"
          + "\x0000\u0583\x0000\u0594\x0000\u05a5\x0000\u05b6\x0000\u05c7\x0000\u05d8\x0000\u05e9\x0000\u05fa"
          + "\x0000\x014F\x0000\u01dc\x0000\u060b\x0000\u061c\x0000\u062d\x0000\u063e\x0000\u064f\x0000\u0660"
          + "\x0000\u0671\x0000\u0682\x0000\u0693\x0000\u06a4\x0000\u06b5\x0000\u06c6\x0000\u06d7\x0000\u06e8"
          + "\x0000\u06f9\x0000\u070a\x0000\u071b\x0000\u072c\x0000\u073d\x0000\u03b8\x0000\u074e\x0000\u075f"
          + "\x0000\u0770\x0000\u0781\x0000\u0792\x0000\u07a3\x0000\u07b4\x0000\u07c5\x0000\u07d6\x0000\u07e7"
          + "\x0000\u07f8\x0000\u0809\x0000\u081a\x0000\u082b\x0000\u083c\x0000\u084d\x0000\u085e\x0000\u086f"
          + "\x0000\u0880\x0000\u0891\x0000\u08a2\x0000\u08b3\x0000\u08c4\x0000\u08d5\x0000\u08e6\x0000\u08f7"
          + "\x0000\u0908\x0000\u0919\x0000\u092a\x0000\u093b\x0000\u094c\x0000\u095d\x0000\u096e\x0000\u097f"
          + "\x0000\u0990\x0000\u09a1\x0000\u09b2\x0000\u09c3\x0000\u09d4\x0000\u09e5\x0000\u09f6\x0000\u0a07"
          + "\x0000\u0a18\x0000\u0a29\x0000\u0a3a\x0000\u0a4b\x0000\u0a5c\x0000\u0a6d\x0000\u0a7e\x0000\u0a8f"
          + "\x0000\u0aa0\x0000\u0ab1\x0000\u0ac2\x0000\u0ad3\x0000\u0ae4\x0000\u0af5";

	  private static int [] ZzUnpackRowMap()
	  {
		int[] result = new int[174];
		int offset = 0;
		offset = ZzUnpackRowMap(ZZ_ROWMAP_PACKED_0, offset, result);
		return result;
	  }

	  private static int ZzUnpackRowMap(string packed, int offset, int[] result)
	  {
		int i = 0; // index in packed string
		int j = offset; // index in unpacked array
		int l = packed.Length;
		while (i < l)
		{
		  int high = packed[i++] << 16;
		  result[j++] = high | packed[i++];
		}
		return j;
	  }

	  /// <summary>
	  /// The transition table of the DFA
	  /// </summary>
	  private static readonly int[] ZZ_TRANS = ZzUnpackTrans();

      private const String ZZ_TRANS_PACKED_0 = 
            "\x0002\x0002\x0001\x0003\x0005\x0002\x0001\x0004\x0001\x0005\x0001\x0006\x0001\x0007\x0001\x000A"
          + "\x0001\x000B\x0001\x000C\x0001\x000D\x0001\x0002\x0020\x0000\x0002\x000E\x0007\x0000\x0001\x000F"
          + "\x0001\x0010\x0001\x0011\x0001\x0014\x0002\x0015\x0001\x0016\x0001\x0017\x0001\x0018\x0001\x0019"
          + "\x0001\x0007\x0001\x001A\x0006\x0000\x0001\x001B\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020"
          + "\x0001\x0021\x0002\x0018\x0001\x0007\x0001\x0022\x0005\x0000\x0001\x000F\x0001\x0023\x0001\x0011"
          + "\x0001\x0014\x0002\x001F\x0001\x0020\x0001\x0024\x0001\x0018\x0001\x0019\x0001\x0007\x0001\x0025"
          + "\x000E\x0000\x0003\x0028\x0002\x0007\x0005\x0000\x0001\x000F\x0001\x0010\x0001\x0011\x0001\x0014"
          + "\x0002\x0015\x0001\x0016\x0001\x001A\x0001\x0029\x0001\x002A\x0001\x0007\x0001\x001A\x000E\x0000"
          + "\x0003\x002B\x0001\x0000\x0001\x002B\x0001\x000B\x0001\x000C\x0011\x0000\x0002\x000C\x0016\x0000"
          + "\x0001\x0002\x0002\x0000\x0001\x002C\x0005\x0000\x0001\x002D\x0002\x002E\x0001\x0007\x0001\x002F"
          + "\x000E\x0000\x0001\x0032\x0001\x0000\x0001\x0032\x0001\x0000\x0001\x0032\x000E\x0000\x0001\x0033"
          + "\x0001\x0034\x0001\x0035\x0001\x0000\x0001\x0033\x000E\x0000\x0001\x0036\x0001\x0000\x0001\x0036"
          + "\x0001\x0000\x0001\x0036\x000E\x0000\x0001\x0037\x0001\x0038\x0001\x0037\x0001\x0000\x0001\x0037" 
          + "\x000E\x0000\x0001\x0039\x0002\x003C\x0001\x0000\x0001\x0039\x000E\x0000\x0001\x0016\x0002\x003D" 
          + "\x0001\x0000\x0001\x0016\x0005\x0000\x0001\x000F\x0001\x003E\x0001\x0011\x0001\x0014\x0002\x0015" 
          + "\x0001\x0016\x0001\x0017\x0001\x0018\x0001\x0019\x0001\x0007\x0001\x001A\x0006\x0000\x0001\x003F" 
          + "\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0021\x0002\x0018\x0001\x0007\x0001\x0022" 
          + "\x0005\x0000\x0001\x000F\x0001\x003F\x0001\x0011\x0001\x0014\x0002\x001F\x0001\x0020\x0001\x0024"
          + "\x0001\x0018\x0001\x0019\x0001\x0007\x0001\x0025\x0005\x0000\x0001\x000F\x0001\x003E\x0001\x0011"
          + "\x0001\x0014\x0002\x0015\x0001\x0016\x0001\x001A\x0001\x0029\x0001\x002A\x0001\x0007\x0001\x001A"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x0040\x0002\x0041\x0001\x0007\x0001\x0042\x000E\x0000"
          + "\x0003\x0038\x0001\x0000\x0001\x0038\x000E\x0000\x0001\x0043\x0002\x0046\x0001\x0000\x0001\x0043"
          + "\x000E\x0000\x0001\x0047\x0002\x0048\x0001\x0000\x0001\x0047\x0006\x0000\x0001\x0049\x0001\x0000"
          + "\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0021\x0002\x0018\x0001\x0007\x0001\x0022\x0006\x0000"
          + "\x0001\x0049\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0022\x0002\x0029\x0001\x0007"
          + "\x0001\x0022\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x004A\x0001\x0041\x0001\x004B\x0001\x0007"
          + "\x0001\x004C\x0005\x0000\x0001\x000F\x0001\x0049\x0001\x0011\x0001\x0014\x0002\x001F\x0001\x0020"
          + "\x0001\x0024\x0001\x0018\x0001\x0019\x0001\x0007\x0001\x0025\x0005\x0000\x0001\x000F\x0001\x0049"
          + "\x0001\x0011\x0001\x0014\x0002\x001F\x0001\x0020\x0001\x0025\x0001\x0029\x0001\x002A\x0001\x0007"
          + "\x0001\x0025\x000E\x0000\x0003\x0028\x0001\x0000\x0001\x0028\x0006\x0000\x0001\x003F\x0001\x0000"
          + "\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0022\x0002\x0029\x0001\x0007\x0001\x0022\x0005\x0000"
          + "\x0001\x000F\x0001\x003F\x0001\x0011\x0001\x0014\x0002\x001F\x0001\x0020\x0001\x0025\x0001\x0029"
          + "\x0001\x002A\x0001\x0007\x0001\x0025\x000E\x0000\x0003\x002B\x0001\x0000\x0001\x002B\x000F\x0000"
          + "\x0002\x004D\x000A\x0000\x0001\x0064\x0002\x0000\x0003\x0064\x0001\x002D\x0002\x002E\x0001\x0007"
          + "\x0001\x002F\x0006\x0000\x0001\x004D\x0002\x0000\x0003\x0065\x0001\x0066\x0002\x002E\x0001\x0007"
          + "\x0001\x0067\x0006\x0000\x0001\x0064\x0002\x0000\x0003\x0064\x0001\x002F\x0002\x0068\x0001\x0007"
          + "\x0001\x002F\x0005\x0000\x0001\x000F\x0006\x0000\x0001\x0032\x0001\x0000\x0001\x0032\x0001\x0000"
          + "\x0001\x0032\x0006\x0000\x0001\x0069\x0001\x0000\x0001\x001E\x0002\x006A\x0001\x0000\x0001\x006B"
          + "\x0002\x0034\x0001\x0000\x0001\x006B\x0006\x0000\x0001\x006E\x0001\x0000\x0001\x001E\x0002\x006F"
          + "\x0001\x0070\x0001\x0071\x0002\x0072\x0001\x0007\x0001\x0073\x0006\x0000\x0001\x0074\x0001\x0000"
          + "\x0001\x001E\x0002\x006F\x0001\x0070\x0001\x0071\x0002\x0072\x0001\x0007\x0001\x0073\x0006\x0000"
          + "\x0001\x0075\x0002\x0000\x0001\x0075\x0002\x0000\x0001\x0037\x0001\x0038\x0001\x0037\x0001\x0000"
          + "\x0001\x0037\x0006\x0000\x0001\x0075\x0002\x0000\x0001\x0075\x0002\x0000\x0003\x0038\x0001\x0000"
          + "\x0001\x0038\x0006\x0000\x0001\x006A\x0001\x0000\x0001\x001E\x0002\x006A\x0001\x0000\x0001\x0039"
          + "\x0002\x003C\x0001\x0000\x0001\x0039\x0006\x0000\x0001\x006F\x0001\x0000\x0001\x001E\x0002\x006F"
          + "\x0001\x0070\x0001\x0078\x0002\x0079\x0001\x0007\x0001\x007A\x0006\x0000\x0001\x0070\x0002\x0000"
          + "\x0003\x0070\x0001\x007B\x0002\x007C\x0001\x0007\x0001\x007D\x000E\x0000\x0001\x006B\x0002\x0034"
          + "\x0001\x0000\x0001\x006B\x0006\x0000\x0001\x007E\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016"
          + "\x0001\x0040\x0002\x0041\x0001\x0007\x0001\x0042\x0006\x0000\x0001\x007F\x0001\x0000\x0001\x001E"
          + "\x0002\x001F\x0001\x0020\x0001\x0082\x0002\x0041\x0001\x0007\x0001\x0083\x0006\x0000\x0001\x007E"
          + "\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0042\x0002\x0084\x0001\x0007\x0001\x0042"
          + "\x0006\x0000\x0001\x0015\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0043\x0002\x0085"
          + "\x0001\x0007\x0001\x0086\x0006\x0000\x0001\x001F\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020"
          + "\x0001\x0046\x0002\x0085\x0001\x0007\x0001\x0087\x0006\x0000\x0001\x0016\x0002\x0000\x0003\x0016"
          + "\x0001\x0047\x0002\x0088\x0001\x0007\x0001\x0089\x0006\x0000\x0001\x0020\x0002\x0000\x0003\x0020"
          + "\x0001\x0048\x0002\x0088\x0001\x0007\x0001\x008C\x000E\x0000\x0001\x0040\x0002\x0082\x0001\x0000"
          + "\x0001\x0040\x0006\x0000\x0001\x008D\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0040"
          + "\x0002\x0041\x0001\x0007\x0001\x0042\x0006\x0000\x0001\x008E\x0001\x0000\x0001\x001E\x0002\x001F"
          + "\x0001\x0020\x0001\x0082\x0002\x0041\x0001\x0007\x0001\x0083\x0006\x0000\x0001\x008D\x0001\x0000"
          + "\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0042\x0002\x0084\x0001\x0007\x0001\x0042\x000E\x0000"
          + "\x0001\x0064\x0002\x008F\x0001\x0000\x0001\x0064\x000E\x0000\x0001\x002D\x0002\x0066\x0001\x0000"
          + "\x0001\x002D\x0006\x0000\x0001\x0065\x0002\x0000\x0003\x0065\x0001\x0066\x0002\x002E\x0001\x0007"
          + "\x0001\x0067\x0006\x0000\x0001\x0065\x0002\x0000\x0003\x0065\x0001\x0067\x0002\x0068\x0001\x0007"
          + "\x0001\x0067\x0006\x0000\x0001\x004D\x0002\x0000\x0003\x0065\x0001\x0067\x0002\x0068\x0001\x0007"
          + "\x0001\x0067\x000E\x0000\x0001\x0090\x0001\x0091\x0001\x0090\x0001\x0000\x0001\x0090\x000E\x0000"
          + "\x0003\x0092\x0001\x0000\x0001\x0092\x0006\x0000\x0001\x0093\x0001\x0000\x0001\x001E\x0002\x006A"
          + "\x0001\x0000\x0001\x006B\x0002\x0034\x0001\x0000\x0001\x006B\x000E\x0000\x0003\x0096\x0001\x0000"
          + "\x0001\x0096\x000E\x0000\x0003\x0097\x0001\x0000\x0001\x0097\x000E\x0000\x0003\x0098\x0001\x0000"
          + "\x0001\x0098\x0006\x0000\x0001\x0099\x0001\x0000\x0001\x001E\x0002\x009A\x0001\x009B\x0001\x0071"
          + "\x0002\x0072\x0001\x0007\x0001\x0073\x0006\x0000\x0001\x009C\x0001\x0000\x0001\x001E\x0002\x009A"
          + "\x0001\x009B\x0001\x0071\x0002\x0072\x0001\x0007\x0001\x0073\x0006\x0000\x0001\x0099\x0001\x0000"
          + "\x0001\x001E\x0002\x009A\x0001\x009B\x0001\x0073\x0002\x009D\x0001\x0007\x0001\x0073\x000E\x0000"
          + "\x0001\x00A0\x0001\x0096\x0001\x00A0\x0001\x0000\x0001\x00A0\x000E\x0000\x0003\x00A1\x0001\x0000"
          + "\x0001\x00A1\x0006\x0000\x0001\x009A\x0001\x0000\x0001\x001E\x0002\x009A\x0001\x009B\x0001\x0078"
          + "\x0002\x0079\x0001\x0007\x0001\x007A\x0006\x0000\x0001\x00A2\x0001\x0000\x0001\x001E\x0002\x009A"
          + "\x0001\x009B\x0001\x0078\x0002\x0079\x0001\x0007\x0001\x007A\x0006\x0000\x0001\x009A\x0001\x0000"
          + "\x0001\x001E\x0002\x009A\x0001\x009B\x0001\x007A\x0002\x00A3\x0001\x0007\x0001\x007A\x0006\x0000"
          + "\x0001\x009B\x0002\x0000\x0003\x009B\x0001\x007B\x0002\x007C\x0001\x0007\x0001\x007D\x0006\x0000"
          + "\x0001\x00A4\x0002\x0000\x0003\x009B\x0001\x007B\x0002\x007C\x0001\x0007\x0001\x007D\x0006\x0000"
          + "\x0001\x009B\x0002\x0000\x0003\x009B\x0001\x007D\x0002\x00A5\x0001\x0007\x0001\x007D\x0006\x0000"
          + "\x0001\x00A6\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0082\x0002\x0041\x0001\x0007"
          + "\x0001\x0083\x0006\x0000\x0001\x00A6\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0083"
          + "\x0002\x0084\x0001\x0007\x0001\x0083\x0006\x0000\x0001\x007F\x0001\x0000\x0001\x001E\x0002\x001F"
          + "\x0001\x0020\x0001\x0083\x0002\x0084\x0001\x0007\x0001\x0083\x0006\x0000\x0001\x00A7\x0001\x0000"
          + "\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0046\x0002\x0085\x0001\x0007\x0001\x0087\x0006\x0000"
          + "\x0001\x0015\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0086\x0002\x00AA\x0001\x0007"
          + "\x0001\x0086\x0006\x0000\x0001\x001F\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0087"
          + "\x0002\x00AA\x0001\x0007\x0001\x0087\x0006\x0000\x0001\x00AB\x0002\x0000\x0003\x0020\x0001\x0048"
          + "\x0002\x0088\x0001\x0007\x0001\x008C\x0006\x0000\x0001\x0016\x0002\x0000\x0003\x0016\x0001\x0089"
          + "\x0002\x00AC\x0001\x0007\x0001\x0089\x0006\x0000\x0001\x0020\x0002\x0000\x0003\x0020\x0001\x008C"
          + "\x0002\x00AC\x0001\x0007\x0001\x008C\x0006\x0000\x0001\x00AD\x0002\x0000\x0003\x00AD\x0001\x0066"
          + "\x0002\x002E\x0001\x0007\x0001\x0067\x0006\x0000\x0001\x0069\x0001\x0000\x0001\x001E\x0002\x006A"
          + "\x0001\x0000\x0003\x0091\x0001\x0000\x0001\x0091\x0006\x0000\x0001\x0093\x0001\x0000\x0001\x001E"
          + "\x0002\x006A\x0001\x0000\x0003\x0091\x0001\x0000\x0001\x0091\x0006\x0000\x0001\x006A\x0001\x0000"
          + "\x0001\x001E\x0002\x006A\x0001\x0000\x0003\x0092\x0001\x0000\x0001\x0092\x000E\x0000\x0003\x0091"
          + "\x0001\x0000\x0001\x0091\x0006\x0000\x0001\x007E\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016"
          + "\x0001\x0096\x0002\x00AE\x0001\x0007\x0001\x00AF\x0006\x0000\x0001\x0015\x0001\x0000\x0001\x001E"
          + "\x0002\x0015\x0001\x0016\x0001\x0097\x0002\x00B0\x0001\x0007\x0001\x00B1\x0006\x0000\x0001\x0016"
          + "\x0002\x0000\x0003\x0016\x0001\x0098\x0002\x00C8\x0001\x0007\x0001\x00C9\x000E\x0000\x0001\x0096"
          + "\x0002\x00CA\x0001\x0000\x0001\x0096\x000E\x0000\x0001\x0097\x0002\x00CB\x0001\x0000\x0001\x0097"
          + "\x000E\x0000\x0001\x0098\x0002\x00CC\x0001\x0000\x0001\x0098\x0006\x0000\x0001\x002C\x0005\x0000"
          + "\x0001\x0096\x0002\x00AE\x0001\x0007\x0001\x00AF\x0006\x0000\x0001\x009C\x0001\x0000\x0001\x001E"
          + "\x0002\x009A\x0001\x009B\x0001\x0073\x0002\x009D\x0001\x0007\x0001\x0073\x0006\x0000\x0001\x008D"
          + "\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x0096\x0002\x00AE\x0001\x0007\x0001\x00AF"
          + "\x0006\x0000\x0001\x0075\x0002\x0000\x0001\x0075\x0002\x0000\x0003\x00A1\x0001\x0000\x0001\x00A1"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x0097\x0002\x00B0\x0001\x0007\x0001\x00B1\x0006\x0000"
          + "\x0001\x00A2\x0001\x0000\x0001\x001E\x0002\x009A\x0001\x009B\x0001\x007A\x0002\x00A3\x0001\x0007"
          + "\x0001\x007A\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x0098\x0002\x00C8\x0001\x0007\x0001\x00C9"
          + "\x0006\x0000\x0001\x00A4\x0002\x0000\x0003\x009B\x0001\x007D\x0002\x00A5\x0001\x0007\x0001\x007D"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x0043\x0002\x0085\x0001\x0007\x0001\x0086\x0006\x0000"
          + "\x0001\x00A7\x0001\x0000\x0001\x001E\x0002\x001F\x0001\x0020\x0001\x0087\x0002\x00AA\x0001\x0007"
          + "\x0001\x0087\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x0047\x0002\x0088\x0001\x0007\x0001\x0089"
          + "\x0006\x0000\x0001\x00AB\x0002\x0000\x0003\x0020\x0001\x008C\x0002\x00AC\x0001\x0007\x0001\x008C"
          + "\x000E\x0000\x0003\x002D\x0001\x0000\x0001\x002D\x0006\x0000\x0001\x00CD\x0001\x0000\x0001\x001E"
          + "\x0002\x00CE\x0001\x00CF\x0001\x00CA\x0002\x00AE\x0001\x0007\x0001\x00D2\x0006\x0000\x0001\x007E"
          + "\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016\x0001\x00AF\x0002\x00D3\x0001\x0007\x0001\x00AF"
          + "\x0006\x0000\x0001\x00D4\x0001\x0000\x0001\x001E\x0002\x00CE\x0001\x00CF\x0001\x00CB\x0002\x00B0"
          + "\x0001\x0007\x0001\x00D5\x0006\x0000\x0001\x0015\x0001\x0000\x0001\x001E\x0002\x0015\x0001\x0016"
          + "\x0001\x00B1\x0002\x00D6\x0001\x0007\x0001\x00B1\x0006\x0000\x0001\x00D7\x0002\x0000\x0003\x00CF"
          + "\x0001\x00CC\x0002\x00C8\x0001\x0007\x0001\x00D8\x0006\x0000\x0001\x0016\x0002\x0000\x0003\x0016"
          + "\x0001\x00C9\x0002\x00D9\x0001\x0007\x0001\x00C9\x0006\x0000\x0001\x00DC\x0001\x0000\x0001\x001E"
          + "\x0002\x00CE\x0001\x00CF\x0001\x00CA\x0002\x00AE\x0001\x0007\x0001\x00D2\x0006\x0000\x0001\x00CE"
          + "\x0001\x0000\x0001\x001E\x0002\x00CE\x0001\x00CF\x0001\x00CB\x0002\x00B0\x0001\x0007\x0001\x00D5"
          + "\x0006\x0000\x0001\x00CF\x0002\x0000\x0003\x00CF\x0001\x00CC\x0002\x00C8\x0001\x0007\x0001\x00D8"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x00DD\x0002\x0072\x0001\x0007\x0001\x00DE\x000E\x0000"
          + "\x0001\x00DF\x0002\x0078\x0001\x0000\x0001\x00DF\x000E\x0000\x0001\x00E0\x0002\x007B\x0001\x0000"
          + "\x0001\x00E0\x0006\x0000\x0001\x00DC\x0001\x0000\x0001\x001E\x0002\x00CE\x0001\x00CF\x0001\x00D2"
          + "\x0002\x00D3\x0001\x0007\x0001\x00D2\x0006\x0000\x0001\x00CD\x0001\x0000\x0001\x001E\x0002\x00CE"
          + "\x0001\x00CF\x0001\x00D2\x0002\x00D3\x0001\x0007\x0001\x00D2\x0006\x0000\x0001\x002C\x0005\x0000"
          + "\x0001\x00DF\x0002\x0079\x0001\x0007\x0001\x00E1\x0006\x0000\x0001\x00CE\x0001\x0000\x0001\x001E"
          + "\x0002\x00CE\x0001\x00CF\x0001\x00D5\x0002\x00D6\x0001\x0007\x0001\x00D5\x0006\x0000\x0001\x00D4"
          + "\x0001\x0000\x0001\x001E\x0002\x00CE\x0001\x00CF\x0001\x00D5\x0002\x00D6\x0001\x0007\x0001\x00D5"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x00E0\x0002\x007C\x0001\x0007\x0001\x00E2\x0006\x0000"
          + "\x0001\x00CF\x0002\x0000\x0003\x00CF\x0001\x00D8\x0002\x00D9\x0001\x0007\x0001\x00D8\x0006\x0000"
          + "\x0001\x00D7\x0002\x0000\x0003\x00CF\x0001\x00D8\x0002\x00D9\x0001\x0007\x0001\x00D8\x000E\x0000"
          + "\x0001\x00DD\x0002\x0071\x0001\x0000\x0001\x00DD\x0006\x0000\x0001\x00E3\x0001\x0000\x0001\x001E"
          + "\x0002\x00E6\x0001\x0064\x0001\x00DD\x0002\x0072\x0001\x0007\x0001\x00DE\x0006\x0000\x0001\x00E3"
          + "\x0001\x0000\x0001\x001E\x0002\x00E6\x0001\x0064\x0001\x00DE\x0002\x009D\x0001\x0007\x0001\x00DE"
          + "\x0006\x0000\x0001\x00E6\x0001\x0000\x0001\x001E\x0002\x00E6\x0001\x0064\x0001\x00DF\x0002\x0079"
          + "\x0001\x0007\x0001\x00E1\x0006\x0000\x0001\x0064\x0002\x0000\x0003\x0064\x0001\x00E0\x0002\x007C"
          + "\x0001\x0007\x0001\x00E2\x0006\x0000\x0001\x00E6\x0001\x0000\x0001\x001E\x0002\x00E6\x0001\x0064"
          + "\x0001\x00E1\x0002\x00A3\x0001\x0007\x0001\x00E1\x0006\x0000\x0001\x0064\x0002\x0000\x0003\x0064"
          + "\x0001\x00E2\x0002\x00A5\x0001\x0007\x0001\x00E2\x000E\x0000\x0001\x00E7\x0002\x00E8\x0001\x0000"
          + "\x0001\x00E7\x000E\x0000\x0001\x00E9\x0002\x00EA\x0001\x0000\x0001\x00E9\x0006\x0000\x0001\x0093"
          + "\x0001\x0000\x0001\x001E\x0002\x006A\x0001\x0000\x0001\x00E7\x0002\x00E8\x0001\x0000\x0001\x00E7"
          + "\x0006\x0000\x0001\x00EB\x0001\x0000\x0001\x001E\x0002\x00EC\x0001\x00AD\x0001\x00ED\x0002\x00F0"
          + "\x0001\x0007\x0001\x00F1\x0006\x0000\x0001\x006A\x0001\x0000\x0001\x001E\x0002\x006A\x0001\x0000"
          + "\x0001\x00E9\x0002\x00EA\x0001\x0000\x0001\x00E9\x0006\x0000\x0001\x00EC\x0001\x0000\x0001\x001E"
          + "\x0002\x00EC\x0001\x00AD\x0001\x00F2\x0002\x00F3\x0001\x0007\x0001\x00F4\x000E\x0000\x0003\x00F5"
          + "\x0001\x0000\x0001\x00F5\x000E\x0000\x0003\x00F6\x0001\x0000\x0001\x00F6\x0006\x0000\x0001\x00F7"
          + "\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00ED\x0002\x00F0\x0001\x0007\x0001\x00F1"
          + "\x0006\x0000\x0001\x00FB\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00ED\x0002\x00F0"
          + "\x0001\x0007\x0001\x00F1\x0006\x0000\x0001\x00F7\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065"
          + "\x0001\x00F1\x0002\x00FC\x0001\x0007\x0001\x00F1\x0006\x0000\x0001\x00FA\x0001\x0000\x0001\x001E"
          + "\x0002\x00FA\x0001\x0065\x0001\x00F2\x0002\x00F3\x0001\x0007\x0001\x00F4\x0006\x0000\x0001\x00FD"
          + "\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00F2\x0002\x00F3\x0001\x0007\x0001\x00F4"
          + "\x0006\x0000\x0001\x00FA\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00F4\x0002\x00FE"
          + "\x0001\x0007\x0001\x00F4\x0006\x0000\x0001\x00E3\x0001\x0000\x0001\x001E\x0002\x00E6\x0001\x0064"
          + "\x0001\x00F5\x0002\x00F0\x0001\x0007\x0001\x00FF\x0006\x0000\x0001\x00E6\x0001\x0000\x0001\x001E"
          + "\x0002\x00E6\x0001\x0064\x0001\x00F6\x0002\x00F3\x0001\x0007\x0001\x0100\x000E\x0000\x0001\x00F5"
          + "\x0002\x00ED\x0001\x0000\x0001\x00F5\x000E\x0000\x0001\x00F6\x0002\x00F2\x0001\x0000\x0001\x00F6"
          + "\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x00F5\x0002\x00F0\x0001\x0007\x0001\x00FF\x0006\x0000"
          + "\x0001\x00FB\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00F1\x0002\x00FC\x0001\x0007"
          + "\x0001\x00F1\x0006\x0000\x0001\x002C\x0005\x0000\x0001\x00F6\x0002\x00F3\x0001\x0007\x0001\x0100"
          + "\x0006\x0000\x0001\x00FD\x0001\x0000\x0001\x001E\x0002\x00FA\x0001\x0065\x0001\x00F4\x0002\x00FE"
          + "\x0001\x0007\x0001\x00F4\x0006\x0000\x0001\x00E3\x0001\x0000\x0001\x001E\x0002\x00E6\x0001\x0064"
          + "\x0001\x00FF\x0002\x00FC\x0001\x0007\x0001\x00FF\x0006\x0000\x0001\x00E6\x0001\x0000\x0001\x001E"
          + "\x0002\x00E6\x0001\x0064\x0001\x0100\x0002\x00FE\x0001\x0007\x0001\x0100\x0004\x0000";

	  private static int [] ZzUnpackTrans()
	  {
		int[] result = new int[5644];
		int offset = 0;
		offset = ZzUnpackTrans(ZZ_TRANS_PACKED_0, offset, result);
		return result;
	  }

	  private static int ZzUnpackTrans(string packed, int offset, int[] result)
	  {
		int i = 0; // index in packed string
		int j = offset; // index in unpacked array
		int l = packed.Length;

        try{
		    while (i < l)
		    {
		      int count = packed[i++];
		      int value = packed[i++];
		      value--;
		      do
		      {
			      result[j++] = value;
		      } while (--count > 0);
		    }
        }
        catch (Exception ex)
        {
            string aa = ex.Message;
        }

		return j;
	  }


	  /* error codes */
	  private const int ZZ_UNKNOWN_ERROR = 0;
	  private const int ZZ_NO_MATCH = 1;
	  private const int ZZ_PUSHBACK_2BIG = 2;

	  /* error messages for the codes above */
	  private static readonly string[] ZZ_ERROR_MSG = {"Unkown internal scanner error", "Error: could not match input", "Error: pushback value was too large"};

	  /// <summary>
	  /// ZZ_ATTRIBUTE[aState] contains the attributes of state <code>aState</code>
	  /// </summary>
	  private static readonly int[] ZZ_ATTRIBUTE = ZzUnpackAttribute();

      private const String ZZ_ATTRIBUTE_PACKED_0 = 
            "\x0001\x0000\x0001\x000B\x000C\x0001\x0006\x0000\x0005\x0001\x0003\x0000\x000B\x0001\x0003\x0000"
          + "\x0007\x0001\x0002\x0000\x0002\x0001\x0002\x0000\x0007\x0001\x0001\x0000\x0003\x0001\x0004\x0000"
          + "\x0003\x0001\x0001\x0000\x0002\x0001\x0002\x0000\x0004\x0001\x0001\x0000\x0017\x0001\x0001\x0000"
          + "\x0002\x0001\x0001\x0000\x0005\x0001\x0002\x0000\x0004\x0001\x0001\x0000\x0001\x0001\x0001\x0000"
          + "\x0002\x0001\x0001\x0000\x0001\x0001\x0001\x0000\x0001\x0001\x0001\x0000\x000C\x0001\x0002\x0000"
          + "\x0002\x0001\x0001\x0000\x0002\x0001\x0001\x0000\x0005\x0001\x0002\x0000\x0003\x0001\x0001\x0000"
          + "\x0002\x0001\x0002\x0000\x0001\x0001\x0001\x0000\x0003\x0001\x0002\x0000\x0002\x0001\x0001\x0000"
          + "\x0001\x0001\x0001\x0000\x0002\x0001\x0001\x0000\x0003\x0001";

	  private static int[] ZzUnpackAttribute()
	  {
		int[] result = new int[174];
		int offset = 0;
		offset = ZzUnpackAttribute(ZZ_ATTRIBUTE_PACKED_0, offset, result);
		return result;
	  }

	  private static int ZzUnpackAttribute(string packed, int offset, int[] result)
	  {
		int i = 0; // index in packed string
		int j = offset; // index in unpacked array
		int l = packed.Length;

        try{
		    while (i < l)
		    {
		      int count = packed[i++];
		      int value = packed[i++];
		      do
		      {
			      result[j++] = value;
		      } while (--count > 0);
		    }
        }
        catch (Exception ex)
        {
            string aa = ex.Message;
        }

		return j;
	  }

	  /// <summary>
	  /// the input device </summary>
	  private System.IO.TextReader zzReader;

	  /// <summary>
	  /// the current state of the DFA </summary>
	  private int zzState;

	  /// <summary>
	  /// the current lexical state </summary>
	  private int zzLexicalState = YYINITIAL;

	  /// <summary>
	  /// this buffer contains the current text to be matched and is
	  ///    the source of the yytext() string 
	  /// </summary>
	  private char[] zzBuffer = new char[ZZ_BUFFERSIZE];

	  /// <summary>
	  /// the textposition at the last accepting state </summary>
	  private int zzMarkedPos;

	  /// <summary>
	  /// the textposition at the last state to be included in yytext </summary>
	  private int zzPushbackPos;

	  /// <summary>
	  /// the current text position in the buffer </summary>
	  private int zzCurrentPos;

	  /// <summary>
	  /// startRead marks the beginning of the yytext() string in the buffer </summary>
	  private int zzStartRead;

	  /// <summary>
	  /// endRead marks the last character in the buffer, that has been read
	  ///    from input 
	  /// </summary>
	  private int zzEndRead;

	  /// <summary>
	  /// number of newlines encountered up to the start of the matched text </summary>
	  private int yyline;

	  /// <summary>
	  /// the number of characters up to the start of the matched text </summary>
	  private int yychar;

	  /// <summary>
	  /// the number of characters from the last newline up to the start of the 
	  /// matched text
	  /// </summary>
	  private int yycolumn;

	  /// <summary>
	  /// zzAtBOL == true <=> the scanner is currently at the beginning of a line
	  /// </summary>
	  private bool zzAtBOL = true;

	  /// <summary>
	  /// zzAtEOF == true <=> the scanner is at the EOF </summary>
	  private bool zzAtEOF;

	  /* user code: */

    public static readonly int ALPHANUM = 0;
    public static readonly int APOSTROPHE = 1;
    public static readonly int ACRONYM = 2;
    public static readonly int COMPANY = 3;
    public static readonly int EMAIL = 4;
    public static readonly int HOST = 5;
    public static readonly int NUM = 6;
    public static readonly int CJ = 7;
    public static readonly int ACRONYM_DEP = 8;
    public static readonly int KOREAN = 9;
    public static readonly int CHINESE = 10;

	public static readonly string[] TOKEN_TYPES = new string [] {"<ALPHANUM>", "<APOSTROPHE>", "<ACRONYM>", "<COMPANY>", "<EMAIL>", "<HOST>", "<NUM>", "<CJ>", "<ACRONYM_DEP>", "<KOREAN>", "<CHINESE>"};

    public int Yychar()
	{
		return yychar;
	}

	/// <summary>
	/// Fills Lucene token with the current token text.
	/// </summary>
    internal void GetText(Token t)
	{
	  t.SetTermBuffer(zzBuffer, zzStartRead, zzMarkedPos - zzStartRead);
	}

	  /// <summary>
	  /// Creates a new scanner
	  /// There is also a java.io.Stream version of this constructor.
	  /// </summary>
	  /// <param name="in">  the java.io.Reader to read input from. </param>
      internal KoreanTokenizerImpl(TextReader in_Renamed)
	  {
          this.zzReader = in_Renamed;
	  }

      /// <summary> Creates a new scanner.
      /// There is also java.io.Reader version of this constructor.
      /// 
      /// </summary>
      /// <param name="in_Renamed"> the java.io.Inputstream to read input from.
      /// </param>
      internal KoreanTokenizerImpl(System.IO.Stream in_Renamed) : this(new System.IO.StreamReader(in_Renamed, System.Text.Encoding.Default))
      {
      }

      /// <summary> Unpacks the compressed character translation table.
      /// 
      /// </summary>
      /// <param name="packed">  the packed character translation table
      /// </param>
      /// <returns>         the unpacked character translation table
      /// </returns>
      private static char[] ZzUnpackCMap(System.String packed)
	  {
		char[] map = new char[0x20000];
		int i = 0; // index in packed string
		int j = 0; // index in unpacked array

        try
        {
            while (i < 156)
            {
                int count = packed[i++];
                char value_Renamed = packed[i++];
                do
                {
                    map[j++] = value_Renamed;
                } while (--count > 0);
            }
        }
        catch (Exception ex)
        {
            string aa = ex.Message;
        }

		return map;
	  }


      /// <summary> Refills the input buffer.
      /// </summary>
      /// <returns><c>false</c>, iff there was new input.
      /// 
      /// </returns>
      /// <exception cref="System.IO.IOException"> if any I/O-Error occurs
      /// </exception>
      private bool ZzRefill()
	  {

		/* first: make room (if you can) */
		if (zzStartRead > 0)
		{
		  Array.Copy(zzBuffer, zzStartRead, zzBuffer, 0, zzEndRead - zzStartRead);

		  /* translate stored positions */
		  zzEndRead -= zzStartRead;
		  zzCurrentPos -= zzStartRead;
		  zzMarkedPos -= zzStartRead;
		  zzPushbackPos -= zzStartRead;
		  zzStartRead = 0;
		}

		/* is the buffer big enough? */
		if (zzCurrentPos >= zzBuffer.Length)
		{
		  /* if not: blow it up */
		  char[] newBuffer = new char[zzCurrentPos * 2];
		  Array.Copy(zzBuffer, 0, newBuffer, 0, zzBuffer.Length);
		  zzBuffer = newBuffer;
		}

		/* finally: fill the buffer with new input */
		int numRead = zzReader.Read(zzBuffer, zzEndRead, zzBuffer.Length - zzEndRead);

		if (numRead < 0)
		{
		  return true;
		}
		else
		{
		  zzEndRead += numRead;
		  return false;
		}
	  }


      /// <summary> Closes the input stream.</summary>
      public void Yyclose()
	  {
            zzAtEOF = true; // indicate end of file
            zzEndRead = zzStartRead; // invalidate buffer

            if (zzReader != null)
            {
	            zzReader.Close();
            }
	  }


	  /// <summary>
	  /// Resets the scanner to read from a new input stream.
	  /// Does not close the old reader.
	  /// 
	  /// All internal variables are reset, the old input stream 
	  /// <b>cannot</b> be reused (internal buffer is discarded and lost).
	  /// Lexical state is set to <tt>ZZ_INITIAL</tt>.
	  /// </summary>
	  /// <param name="reader">   the new input stream  </param>
      public void Yyreset(TextReader reader)
	  {
		zzReader = reader;
		zzAtBOL = true;
		zzAtEOF = false;
		zzEndRead = zzStartRead = 0;
		zzCurrentPos = zzMarkedPos = zzPushbackPos = 0;
		yyline = yychar = yycolumn = 0;
		zzLexicalState = YYINITIAL;
	  }


	  /// <summary>
	  /// Returns the current lexical state.
	  /// </summary>
      public int Yystate()
	  {
		return zzLexicalState;
	  }


	  /// <summary>
	  /// Enters a new lexical state
	  /// </summary>
	  /// <param name="newState"> the new lexical state </param>
      public void Yybegin(int newState)
	  {
		zzLexicalState = newState;
	  }


	  /// <summary>
	  /// Returns the text matched by the current regular expression.
	  /// </summary>
      public String Yytext()
	  {
          return new String(zzBuffer, zzStartRead, zzMarkedPos - zzStartRead);
	  }


      /// <summary> Returns the character at position <tt>pos</tt> from the 
      /// matched text. 
      /// 
      /// It is equivalent to yytext().CharAt(pos), but faster
      /// 
      /// </summary>
      /// <param name="pos">the position of the character to fetch. 
      /// A value from 0 to yyLength()-1.
      /// 
      /// </param>
      /// <returns> the character at position pos
      /// </returns>
      public char Yycharat(int pos)
	  {
		return zzBuffer[zzStartRead + pos];
	  }


	  /// <summary>
	  /// Returns the length of the matched text region.
	  /// </summary>
      public int Yylength()
	  {
		return zzMarkedPos - zzStartRead;
	  }


      /// <summary> Reports an error that occured while scanning.
      /// 
      /// In a wellformed scanner (no or only correct usage of 
      /// Yypushback(int) and a match-all fallback rule) this method 
      /// will only be called with things that "Can't Possibly Happen".
      /// If this method is called, something is seriously wrong
      /// (e.g. a JFlex bug producing a faulty scanner etc.).
      /// 
      /// Usual syntax/scanner level error handling should be done
      /// in error fallback rules.
      /// 
      /// </summary>
      /// <param name="errorCode"> the code of the errormessage to display
      /// </param>
	  private void ZzScanError(int errorCode)
	  {
          String message;
		try
		{
		  message = ZZ_ERROR_MSG[errorCode];
		}
		catch (System.IndexOutOfRangeException e)
		{
		  message = ZZ_ERROR_MSG[ZZ_UNKNOWN_ERROR];
		}

		throw new Exception(message);
	  }


      /// <summary> Pushes the specified amount of characters back into the input stream.
      /// 
      /// They will be read again by then next call of the scanning method
      /// 
      /// </summary>
      /// <param name="number"> the number of characters to be read again.
      /// This number must not be greater than yyLength()!
	  public virtual void Yypushback(int number)
	  {
		if (number > Yylength())
		{
		  ZzScanError(ZZ_PUSHBACK_2BIG);
		}

		zzMarkedPos -= number;
	  }


      /// <summary> Resumes scanning until the next regular expression is matched,
      /// the end of input is encountered or an I/O-Error occurs.
      /// 
      /// </summary>
      /// <returns>      the next token
      /// </returns>
      /// <exception cref="System.IO.IOException"> if any I/O-Error occurs
      public virtual int GetNextToken()
	  {
			int ZzInput;
			int ZzAction;
    
			// cached fields:
			int zzCurrentPosL;
			int zzMarkedPosL;
			int zzEndReadL = zzEndRead;
			char[] zzBufferL = zzBuffer;
			char[] ZzCMapL = ZZ_CMAP;
    
			int[] ZzTransL = ZZ_TRANS;
			int[] ZzRowMapL = ZZ_ROWMAP;
			int[] ZzAttrL = ZZ_ATTRIBUTE;
    
			while (true)
			{
			  zzMarkedPosL = zzMarkedPos;
    
			  yychar += zzMarkedPosL - zzStartRead;
    
			  ZzAction = -1;
    
			  zzCurrentPosL = zzCurrentPos = zzStartRead = zzMarkedPosL;
    
			  zzState = zzLexicalState;
    
    
			  ZzForAction:
			  {
				while (true)
				{
    
				  if (zzCurrentPosL < zzEndReadL)
				  {
					ZzInput = zzBufferL[zzCurrentPosL++];
				  }
				  else if (zzAtEOF)
				  {
					ZzInput = YYEOF;
					goto ZzForAction;
				  }
				  else
				  {
					// store back cached positions
					zzCurrentPos = zzCurrentPosL;
					zzMarkedPos = zzMarkedPosL;
					bool eof = ZzRefill();
					// get translated positions and possibly new buffer
					zzCurrentPosL = zzCurrentPos;
					zzMarkedPosL = zzMarkedPos;
					zzBufferL = zzBuffer;
					zzEndReadL = zzEndRead;
					if (eof)
					{
					  ZzInput = YYEOF;
					  goto ZzForAction;
					}
					else
					{
					  ZzInput = zzBufferL[zzCurrentPosL++];
					}
				  }
				  int ZzNext = ZzTransL[ZzRowMapL[zzState] + ZzCMapL[ZzInput]];
				  if (ZzNext == -1)
				  {
					  goto ZzForAction;
				  }
				  zzState = ZzNext;
    
				  int ZzAttributes = ZzAttrL[zzState];
				  if ((ZzAttributes & 1) == 1)
				  {
					ZzAction = zzState;
					zzMarkedPosL = zzCurrentPosL;
					if ((ZzAttributes & 8) == 8)
					{
						goto ZzForAction;
					}
				  }
    
				}
			  }
    
			  // store back cached position
			  zzMarkedPos = zzMarkedPosL;
    
			  switch (ZzAction < 0 ? ZzAction : ZZ_ACTION[ZzAction])
			  {
				case 8:
				{
					  return HOST;
				}
				case 13:
					break;
				case 11:
				{
					  return ACRONYM_DEP;
				}
				case 14:
					break;
				case 10:
				{
					  return ACRONYM;
				}
				case 15:
					break;
				case 1:
				{ // ignore
				}
			  goto case 16;
				case 16:
					break;
				case 6:
				{
					  return NUM;
				}
				case 17:
					break;
				case 5:
				{
					  return CJ;
				}
				case 18:
					break;
				case 2:
				{
					  return ALPHANUM;
				}
				case 19:
					break;
				case 9:
				{
					  return COMPANY;
				}
				case 20:
					break;
				case 7:
				{
					  return APOSTROPHE;
				}
				case 21:
					break;
				case 4:
				{
					  return CHINESE;
				}
				case 22:
					break;
				case 3:
				{
					  return KOREAN;
				}
				case 23:
					break;
				case 12:
				{
					  return EMAIL;
				}
				case 24:
					break;
				default:
				  if (ZzInput == YYEOF && zzStartRead == zzCurrentPos)
				  {
					zzAtEOF = true;
					return YYEOF;
				  }
				  else
				  {
					ZzScanError(ZZ_NO_MATCH);
				  }
			  break;
			  }
			}
	    }

    }
}
