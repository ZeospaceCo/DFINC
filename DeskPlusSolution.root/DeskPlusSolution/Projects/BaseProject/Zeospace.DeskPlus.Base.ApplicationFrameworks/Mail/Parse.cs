using System;
using System.Text;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.Parse
{
	/// <summary>
	/// Summary description for Coding.
	/// </summary>
	public abstract class QuotedCoding
	{
		/// <summary>
		/// GetByteArray                             
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte[] GetByteArray(string s)
		{

			//Console.WriteLine("GetByteArray : " + s);

			byte[] buffer=new byte[s.Length];
			int bufferPosition=0;
			
			try
			{
				for(int i=0;i<s.Length;i++)
				{
					if (s[i]=='=')
					{
						try
						{
							if (s[i+1]=='\r' || s[i+2] == '\n')
							{
								bufferPosition --;
							}
							else
							{
								buffer[bufferPosition]=System.Convert.ToByte(s.Substring(i+1,2),16);
								
								i+=2;
							}
						}
						catch
						{

						}
					}
					else if (s[i]=='_')
						buffer[bufferPosition]= 95;//32=공백;
					else if (s[i]==' ')
						buffer[bufferPosition]= 32;//32=공백;

					else
					{
						buffer[bufferPosition]=(byte)s[i];
					}

					bufferPosition++;
				}
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.Parse[GetByteArray] : GetByteArray Error - ", ex);
			}

			byte[] newArray=new byte[bufferPosition];
			Array.Copy(buffer,newArray,bufferPosition);

			return newArray;
		}

		/// <summary>
		/// Decoduje string "=?iso-8859-2?Q?Kupis_Pawe=B3?=" 
		/// lub zakodowany base64
		/// </summary>
		/// <param name="s">=?iso-8859-2?Q?Kupis_Pawe=B3?=</param>
		/// <returns>Kupis Pawe</returns>
		public static string DecodeOne(string s)
		{
			try
			{

				// Subject: [=?euc-kr?Q?=BE=C6=C0=CC=C7=CF=BF=EC=BD=BA?= 
				// Subject: =?ISO-8859-1?b?RG8geW91IG5lZWQgcw==?=ome?
				// 위와 같은 아주 특수한 경우를 위한 처리
				// ome? 부분은 인코딩된 부분이 아닌, 그냥 그대로 사용될 string을 짬뽕으로 끼워 넣은 부분임.
				// 외국 놈들 메일들 중에 일부 이런 메일들이 있음.


				int start_idx = s.IndexOf("=?");

				if (start_idx == -1)
				{
					return s;
				}


				string check_encoding = s.Split('?')[1];

				char[] separator={'?'};
				string[] sArray=s.Split(separator);

				byte[] bArray;
				//rozpoznaj rodzj kodowania
				if (sArray[2].ToLower().Equals("q")) //querystring
				{
					bArray = GetByteArray(sArray[3]);
				}
				else if (sArray[2].ToLower().Equals("b"))//base64
				{
					bArray = Convert.FromBase64String(sArray[3]);
				}
				else
				{
					return s;
				}
				


				if (check_encoding.ToLower().Equals("utf-8"))
				{
					return Encoding.UTF8.GetString(bArray);
				}
				else if (check_encoding.ToLower().Equals("euc-jp"))
				{
					//Console.WriteLine("일본어");
					return Encoding.GetEncoding("euc-jp").GetString(bArray);
				}
				else if (check_encoding.ToLower().Equals("euc-kr"))
				{
					//Console.WriteLine("EUC-KR");
					return Encoding.GetEncoding("euc-kr").GetString(bArray);
				}

				return Encoding.Default.GetString(bArray);

			}
			catch(Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.Parse[DecodeOne] : ", ex);
				return s;
			}
		}

		/// <summary>
		/// Decode
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string Decode(string s)
		{
			char[] separator={'?'};
			string[] sArray=s.Split(separator);

			StringBuilder retString=new StringBuilder();

			int old=0, start=0, stop;

			for (;;)
			{
				start=s.IndexOf("=?",start);
				//Console.WriteLine("start : " + start.ToString());

				if (start==-1)
				{
					retString.Append(s,old,s.Length-old);
					//Console.WriteLine("1 : " + retString.ToString());

					return retString.ToString();
				}
				stop=s.IndexOf("?=",start+2);
				//Console.WriteLine("stop : " + stop.ToString());

				if (sArray[2].ToLower().Equals("q")) //quetedprintable
				{
					if (stop > -1)
					{
						if (s.IndexOf("?=",stop+2) > -1)
						{
							stop=s.IndexOf("?=",stop+2);
						}
					}
					//Console.WriteLine("stop : " + stop.ToString());
				}
			
				if (stop==-1) //blad w stringu
				{
					Console.WriteLine("2 : " + s);
					return s;
				}
				retString.Append(s,old,start-old);
				//Console.WriteLine("3 : " + retString.ToString());

				//Console.WriteLine(s.Substring(start,stop-start+2));

				retString.Append(DecodeOne(s.Substring(start,stop-start+2)));
				//Console.WriteLine("4 : " + retString.ToString());

				start=stop+2;
				old=stop+2;

			}

		}
	}
}
