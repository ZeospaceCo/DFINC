using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Security.Cryptography;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail
{
	/// <summary>
	/// SMTP에 대한 요약 설명입니다.
	/// </summary>
	public class SMTP : IDisposable
	{
		private DNS.DnsEx dns	= null;
		private Socket smptSocket = null;
		private string preCommand = null;
		private bool result = false;
		private bool receiveOK = false;
		private int	rcptToCount = 0;
		private string[]		toEmail		= null;

		private Hashtable		to			= null;
		private string			from		= null;
		private string			subject		= null;


		private static char[] sep={' ','\t','\r','\n'};
		private Hashtable		mxRecord	= null;
		private string[]		mxHost		= new string[5];
		
		private string			toDomain	= null;
		private MemoryStream	emlStream   = null;
		private bool doing					= false;
		private bool ESMTP					= false;		
		private StringBuilder errorMsg		= null;


		private int	smtpPort				= 25;
		private int	commandIdleTimeOut		= 15000;	// Holds command ilde timeout.
		private int dataBlockSize			= 2048;
		private int badCmdCount				= 0;		// Holds number of bad commands.
		private int maxBadCommands			= 8;
		private int sessionIdleTimeOut		= 85000;	// Holds session idle timeout.

		private System.ComponentModel.Container components = null;

		private string emlFilePath = null;

		private int maxReCall				= 4;		//오류가 낳을때 다시 시도하는 횟수
	

		/// <summary>
		/// SMTP
		/// </summary>
		public SMTP(string _emlFilePath)
		{
			//Eml 경로 가져오기
			emlFilePath = _emlFilePath;

			//Error Messeage 초기화
			errorMsg = new StringBuilder();

			//Dns 정보 가져오기
			if (DNS.DnsEx.DnsServers == null)
			{
				try
				{
                    string dnsIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "SmtpDnsIP","");
					string[] dnsServers = dnsIP.Split(',');
					DNS.DnsEx.DnsServers = dnsServers;
				}
				catch
				{
					string[] dnsServers = {"168.126.63.1", "164.124.101.2"};
					DNS.DnsEx.DnsServers = dnsServers;
				}
			}

			//다시 호출
			try
			{
                maxReCall = Convert.ToInt32(AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MaxReCall","0"));
			}
			catch
			{
				maxReCall = 4;
			} 

			dns = new DNS.DnsEx();

			to = new Hashtable();
			emlStream = GetFileStream(emlFilePath);

			// To, From, Domain, MX 레코드 등의 정보를 가져옴.
			GetFileInfo(_emlFilePath);

			mxRecord = DNSQuery(toDomain);

			SetMxHost();


		}

		/// <summary>
		/// 메일 발송
		/// </summary>
		public void Send()
		{
			Send(0);
		}

		#region function Set_Default
		private void SetDefault()
		{
			smptSocket = null;
			//errorMsg.Length = 0;
			preCommand = null;
			result = false;
			receiveOK = false;
			rcptToCount = 0;
			toEmail = null;
		}

		#endregion

		private void Send(int count)
		{
			try
			{
				if (mxRecord == null || mxRecord.Count == 0)
				{
					doing = false;
					return;
				}

				smptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				smptSocket.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.NoDelay,1);

				try
				{
                    IPHostEntry IPHost = Dns.GetHostEntry(mxHost[count]); 
					IPAddress[] addr = IPHost.AddressList; 

					IPAddress hostIP = addr[0];

					if (addr.Length > 1)
					{
						if (count > 0)
						{
							hostIP = addr[1];
						}
					}

					IPEndPoint ipepServer = new IPEndPoint(hostIP, smtpPort);

					smptSocket.Connect(ipepServer);
					

					//------ Create command loop --------------------------------//
					// Loop while QUIT cmd or Session TimeOut.
					long lastCmdTime = DateTime.Now.Ticks;
					bool closing = false;

					string lastCmd  = "";

					while(true)
					{	
						// If there is any data available, begin command reading.
						if(smptSocket.Available > 0)
						{
							try
							{
								ReadReplyCode replyCode = Common.ReadLineFromSocket(smptSocket,out lastCmd,commandIdleTimeOut);
								if(replyCode == ReadReplyCode.Ok)
								{	

									if(ReceiveCommand(lastCmd.TrimEnd(sep),ref closing))
									{
										if (closing)
										{
											break;
										}
										else
										{
											if (count < maxReCall)
											{
												SetDefault();

												Send(count+1);
												doing = true;
												break;
											}
											else
											{
												doing = false;
												break;
											}
										}

									}
								}
								else
								{
									//---- Check that maximum bad commands count isn't exceeded ---------------//
									if(badCmdCount > maxBadCommands-1)
									{
										SendData("QUIT Too many bad commands, closing transmission channel\r\n");
										break;
									}
									badCmdCount++;
									//-------------------------------------------------------------------------//

									switch(replyCode)
									{
										case ReadReplyCode.LenghExceeded:
											SendData("QUIT Line too long.\r\n");
											break;

										case ReadReplyCode.TimeOut:
											SendData("QUIT Command timeout.\r\n");
											break;

										case ReadReplyCode.UnKnownError:
											SendData("QUIT UnKnown Error.\r\n");

											break;
									}

								}
							}							
							catch(Exception ex)
							{
                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SMTP[Send1] : QUIT Unkown temp error", ex);
							
								SendData("QUIT Unkown temp error\r\n");

								if (count < maxReCall)
								{
									SetDefault();

									Send(count+1);
									doing = true;
								}
								else
								{
									doing = false;
								}
							
								break;
							}

							// reset last command time
							lastCmdTime = DateTime.Now.Ticks;
						}
						else
						{
							//----- Session timeout stuff ------------------------------------------------//
							if(DateTime.Now.Ticks > lastCmdTime + ((long)(sessionIdleTimeOut)) * 10000)
							{
								dns.Bad_MX_Record(mxHost[count]);
								// Notify for closing
								SendData("QUIT Session timeout, closing transmission channel\r\n");

								errorMsg.Append("QUIT Session timeout, closing transmission channel<br>");
								
								if (count < maxReCall)
								{
									SetDefault();

									Send(count+1);
									doing = true;
									break;
								}
								else
								{
									doing = false;
									break;
								}

							}
						
							// Wait 100ms to save CPU, otherwise while loop may take 100% CPU. 
							Thread.Sleep(100);
							//---------------------------------------------------------------------------//
						}
					}
				}
				catch (Exception ex)
				{
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SMTP[Send2] : ", ex);

					if(smptSocket.Connected)
					{
						smptSocket.Close();
					}

					smptSocket = null;

					errorMsg.Append(mxHost[count] + " " + ex.Message + "<br>");


					if (count < maxReCall)
					{
						SetDefault();

						Send(count+1);
						doing = true;
					}
					else
					{
						doing = false;
					}
				}
			}
			catch(ThreadInterruptedException ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SMTP[Send3] : ", ex);
				//string dummy = ex.Message;     // Needed for to remove compile warning

				if (count < maxReCall)
				{
					SetDefault();

					Send(count+1);
					doing = true;
				}
				else
				{
					doing = false;
				}


			}
			catch(Exception ex)
			{
				if(smptSocket.Connected)
				{
					SendData("QUIT Service not available, closing transmission channel\r\n" + ex.Message);
				}

				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SMTP[Send4] : ", ex);

				if (count < maxReCall)
				{
					SetDefault();

					Send(count+1);
					doing = true;
				}
				else
				{
					doing = false;
				}


			}
			finally
			{
				try
				{

					preCommand = null;


					if (smptSocket!= null)
					{
						if(smptSocket.Connected)
						{
							smptSocket.Close();
						}
					}


					if (!doing)
					{

						if (!result) // 메일 전송이 정상적으로 완료 되지 않았으면서...
						{
							SendFailure();
						}
						else
						{
							if (to.Keys.Count > 1) // 1명일때는 메시지 전달이 성공했으면.. 에러 메시지 필요없음.
							{
								if (errorMsg.Length > 0) //받는 사람이 여러명일 경우.. 그 중에 한사람이라도 잘못되면...
								{
									SendFailure();
								}
							}
						}

						//EML 삭제
						File.Delete(emlFilePath);					
                       
						Thread.Sleep(5000);

						Dispose();

					}

				}
				catch (Exception ex)
				{
					Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[Send] : Finally catch : ",ex);
				}

			}
		}

		/// <summary>
		/// host 생성
		/// </summary>
		public void SetMxHost()
		{
			if (mxRecord != null && mxRecord.Count > 0)
			{
				switch(mxRecord.Count)
				{
					case 1:
					{
						MakeMXArray (5);

						break;
					}
					case 2:
					{
						MakeMXArray (3);
						break;
					}
					case 3:
					{
						MakeMXArray (2);
						break;
					}
					case 4:
					{
						MakeMXArray (2);
						break;
					}
					case 5:
					{
						MakeMXArray (1);
						break;
					}
					default :
					{
						MakeMXArray (1);
						break;
					}
				}
			}
		}



		#region function Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			if(components != null)
			{
				components.Dispose();
			}
			

			try
			{
				Disconnect();
				smptSocket = null;
				emlStream         = null;
				to			  = null;
				dns = null;
				mxRecord = null;
				mxHost = null;
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[Dispose]", ex);
			}
		}

		#endregion

		#region Receive_Command

		/// <summary>
		/// ReceiveCommand
		/// </summary>
		/// <param name="_commandTxt"></param>
		/// <param name="closing"></param>
		/// <returns></returns>
		private bool ReceiveCommand(string _commandTxt, ref bool closing)
		{
			switch (preCommand)
			{
				case null:
				{
					if (_commandTxt.Substring(0, 3).Equals("220"))
					{
						// EHLO 서버 접속

						if (_commandTxt.ToLower().IndexOf("esmtp") > -1)
						{
							ESMTP = true;

							//SendData("EHLO " + System.Net.Dns.GetHostName() + "\r\n");
							SendData("EHLO " + from.Split('@')[1] + "\r\n");
						}
						else
						{
							//SendData("HELO " + System.Net.Dns.GetHostName() + "\r\n");
							SendData("HELO " + from.Split('@')[1] + "\r\n");
						}

						preCommand = "EHLO";

						return false;
					}
					else
					{
						SendData("QUIT\r\n");

						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");

						return true;

					}
				}
				case "EHLO":
				{

					if (_commandTxt.StartsWith("250-"))
					{
						if (_commandTxt.IndexOf("250 ") > -1)
						{
							if (ESMTP)
							{
								SendData("MAIL FROM: <" + from + "> SIZE=" + emlStream.Length + "\r\n");
							}
							else
							{
								SendData("MAIL FROM: <" + from + ">" + "\r\n");
							}


							preCommand = "MAIL FROM";
						}
					}
					else if (_commandTxt.IndexOf("250 ") > -1)
					{
						if (ESMTP)
						{
							SendData("MAIL FROM: <" + from + "> SIZE=" + emlStream.Length + "\r\n");
						}
						else
						{
							SendData("MAIL FROM: <" + from + ">" + "\r\n");
						}

						preCommand = "MAIL FROM";
					}
					else
					{
						SendData("QUIT\r\n");
						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");
						preCommand = "QUIT";
					}

					return false;
				}
				case "MAIL FROM":
				{
					if (_commandTxt.Substring(0,3).Equals("250"))
					{
						toEmail = new string[to.Keys.Count];

						int i = 0;

						foreach (string temp_email in to.Keys)
						{
							toEmail[i] = temp_email;
							i++;
						}

						SendData("RCPT TO: <" + toEmail[rcptToCount] + ">\r\n");

						rcptToCount ++;

						preCommand = "RCPT TO";
					}
					else
					{
						SendData("QUIT\r\n");
						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");
						preCommand = "QUIT";
						rcptToCount = 0;
					}

					return false;

				}
				case "RCPT TO":
				{
					if (_commandTxt.Substring(0,3).Equals("250")) // 정상 리턴 이면...
					{
						receiveOK = true;
					}
					else
					{
						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");
					}

					if (rcptToCount == to.Count)
					{
						if (receiveOK)
						{
							SendData("DATA\r\n");
							preCommand = "DATA";
							rcptToCount = 0;
						}
						else
						{
							SendData("QUIT\r\n");
							errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");
							preCommand = "QUIT";
							rcptToCount = 0;
						}
					}
					else
					{
						SendData("RCPT TO: <" + toEmail[rcptToCount] + ">\r\n");
						rcptToCount ++;
					}

					return false;

				}
				case "DATA":
				{
					if (!_commandTxt.Substring(0,3).Equals("354")) // normal 한 리턴 형식.. 바로 메일 data 전송..
					{
						SendData("QUIT\r\n");

						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");

						preCommand = "QUIT";

						return true;
					}

					// 메일 패킷 전송
					SendData(emlStream);

					// 전송 완료의 . 표시
					SendData("\r\n.\r\n");

					preCommand = "SEND";

					return false;

				}
				case "SEND":
				{
					if (!_commandTxt.Substring(0, 3).Equals("250"))
					{
						SendData("QUIT\r\n");

						errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");

						preCommand = null;

						return true;
					}

					SendData("QUIT\r\n");

					preCommand = null;

					result = true;

					closing = true;

					return true;
				}
				case "QUIT":
				{
					closing = true;
					return true;
				}
				default :
				{
					SendData("QUIT\r\n");
					errorMsg.Append(_commandTxt.TrimEnd(sep).TrimStart(sep).Replace("<","&lt;").Replace(">","&gt;") + "<br>");
					preCommand = "QUIT";

					return false;
				}
			}
		}

		#endregion

		#region function SendFailure

		private void SendFailure()
		{
			try
			{
				string mailTo = "";

				foreach (string toEmail in to.Keys)
				{
					mailTo += toEmail + "<br>";
				}


				string line = null;
				emlStream.Position = 0;

				

				using (StreamReader reader = new StreamReader((Stream) emlStream, Encoding.Default))
				{
					line = reader.ReadLine();
					string Subject = "";
					int startIdx;

					while (line != null)
					{
						#region Subject
						if(line.ToLower().StartsWith("subject:"))
						{
							// Subject 일시에 
							Subject=line.Substring(8).TrimStart(sep).TrimEnd(sep);

							startIdx = Subject.IndexOf("=?");

							if (startIdx > 0)
							{
								string firstStr = Subject.Substring(0,startIdx);
								string endStr = Subject.Substring(startIdx);

								int endIdx = endStr.IndexOf("?=");

								if (endIdx+2 < Subject.Length)
								{
									string endStr1 = endStr.Substring(endIdx+2);
									string firstStr1 = endStr.Substring(0,endIdx+2);

									subject = firstStr + Parse.QuotedCoding.DecodeOne(firstStr1) + endStr1;
								}
								else
								{
									subject = firstStr + Parse.QuotedCoding.DecodeOne(endStr);
								}
							}
							else
							{
								subject = Parse.QuotedCoding.DecodeOne(Subject);
							}
	
							do
							{
								line = reader.ReadLine();

								if (line.StartsWith("\t"))
								{
									subject += Parse.QuotedCoding.DecodeOne(line.TrimStart(sep).TrimEnd(sep));
								}
							}while(line.StartsWith("\t"));


							break;
						}
						#endregion

						line = reader.ReadLine();
					}

					reader.Close();
				}

				if (subject.IndexOf("Delivery Failure") > -1)
				{

				}
				else
					// 전송 실패되었다는 메시지를 또 보낼려고 시도하다 에러가 발생.. 
					// 무한 루프에 빠지게 되기 때문에.. 한번 전송 실패한 곳은 다시 전송 실패 메시지를 보내지 않음....
				{

//					if (!Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail.GetEmail(from).Equals("postmaster@mail"))
//					{
					string errorLog = "Delivery Failure: " + subject + "<br><br> From :<br><br><b>" + from + "<br><br> To :<br><br><b>" + mailTo + "</b><br><br>Error Message <br><br><b>" + errorMsg.ToString() + "</b><br><br><br>";
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SMTP[SendFailure] : " + errorLog);
//					}
				}
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[SendFailure]", ex);
			}
		}
		#endregion

		#region DNSQuery
		private Hashtable DNSQuery(string domain)
		{
			Hashtable mxRecord = new Hashtable();


			//--- Get MX record -------------------------------------------//
			DNS.DnsEx dns = new Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.DNS.DnsEx();
			DNS.DnsServerResponse dnsResponse = dns.Query(domain,DNS.QTYPE.MX);
	
			Hashtable Bad_MX = dns.Get_Bad_MX();

			switch(dnsResponse.ResponseCode)
			{
				case DNS.RCODE.NO_ERROR:
				{
					DNS.MX_Record[] mxRecords = dnsResponse.GetMXRecords();

					// Try all available hosts by MX preference order, if can't connect specified host.
					foreach(DNS.MX_Record mx in mxRecords)
					{
						try
						{
							if (!Bad_MX.ContainsKey(mx.Host))
							{
								if (!mxRecord.ContainsKey(mx.Host))
								{
									mxRecord.Add(mx.Host,null);
								}
							}
						}
						catch
						{ // Just skip and let for to try next host.									
							Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[DNSQuery] : Failed connect to: " + mx.Host);
						}
					}

					/* Rfc 2821 5
								 If no MX records are found, but an A RR is found, the A RR is treated as
								 if it was associated with an implicit MX RR, with a preference of 0,
								 pointing to that host.

								*/
					if(mxRecords.Length == 0)
					{
						// Try to connect with A record
						IPHostEntry ipEntry = null;
						try
						{
                            ipEntry = Dns.GetHostEntry(domain);									
						}
						catch
						{
							Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[DNSQuery] : Invalid domain,no MX or A record: " + domain);

							mxRecord = null;
							break;
						}

						try
						{
							if (!mxRecord.ContainsKey(domain))
							{
								mxRecord.Add(domain,null);
							}

							break;
						}
						catch
						{
							Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[DNSQuery] : Failed connect to:" + domain);
						}
					}
					break;
				}
				case DNS.RCODE.NAME_ERROR:
				{
					Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[DNSQuery] : Invalid domain,no MX or A record: " + domain);

					mxRecord = null;

					break;
				}
				case DNS.RCODE.SERVER_FAILURE:
				{
					Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[DNSQuery] : Dns server unvailable.");

					mxRecord = null;

					break;
				}
			}

			return mxRecord;
		}
		#endregion	

		#region Make_MX_Array
		private void MakeMXArray (int _times)
		{
			int j = 0;

			for (int i=0; i <= _times; i++)
			{
				foreach (string mx in mxRecord.Keys)
				{
					if (j >= 5) //5개 까지만..
					{
						break;
					}

					mxHost[j] = mx;

					j++;
				}
			}

		}
		#endregion	

		#region function Disconnect
		private void Disconnect()
		{
			if ( smptSocket != null )
			{
				if ( ! smptSocket.Connected ) 
					return ;
			}
			else
				return ;
			
			smptSocket.Close();
			smptSocket = null;
		}
		#endregion

		#region SendData
			
		/// <summary>
		/// Sends data to socket.
		/// </summary>
		/// <param name="data">String data wich to send.</param>
		private void SendData(string data)
		{	
			byte[] byteData = Encoding.ASCII.GetBytes(data.ToCharArray());
			
			int nCount = smptSocket.Send(byteData,byteData.Length,0);

			if(nCount != byteData.Length)
			{
				Disconnect();
				throw new Exception("Smtp.SendData sended less data than requested !");
			}
		}

		private void SendData(MemoryStream strm)
		{
			long totalSent = 0;

			byte[] byteData = new byte[dataBlockSize];
			int nCount;

			while(strm.Position < strm.Length)
			{
				nCount = strm.Read(byteData,0,byteData.Length);
				smptSocket.Send(byteData,nCount,SocketFlags.None);

				totalSent += nCount;
			}
		}

		#endregion

		/// <summary>
		/// eml steam 가져오기
		/// </summary>
		/// <param name="_emlFilePath"></param>
		/// <returns></returns>
		private MemoryStream GetFileStream(string _emlFilePath)
		{
			try
			{
				MemoryStream ms = new MemoryStream();

				using (FileStream fs = File.OpenRead(_emlFilePath))
				{
					byte[] buffer = new byte[fs.Length];
					fs.Read(buffer,0, buffer.Length);

					ms.Write(buffer,0,buffer.Length);
					ms.Position = 0;

					return ms;
				}
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[GetFileStream] : ", ex);
				return null;
			}
		}

		private void GetFileInfo(string _emlFilePath)
		{
			try
			{
				MailDataInfo mailDataInfo = null;
				using (StreamReader reader = new StreamReader(_emlFilePath, Encoding.Default))
				{
					mailDataInfo = new MailDataInfo();

					string line = "";

					string toEmail = "";

					line = reader.ReadLine().ToLower().TrimEnd(sep).TrimStart(sep);

					if (line.Trim().ToLower().StartsWith("x-sender:"))
					{
						from = line.Replace("x-sender:","").Trim();
					}

					line = reader.ReadLine().ToLower().TrimEnd(sep).TrimStart(sep);

					while(line.StartsWith("x-receiver:"))
					{
						toEmail = line.Replace("x-receiver:","").Trim().ToLower();
						toDomain = toEmail.Split('@')[1].Trim();

						if (!to.ContainsKey(toEmail))
						{
							to.Add(toEmail,toDomain);
						}
						line = reader.ReadLine().ToLower().TrimEnd(sep).TrimStart(sep);
					}
					reader.Close();	
				}
			}
			catch (Exception ex)
			{
				Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[GetFileInfo] : ", ex);
			}
		}
	}
}
