using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Net.Sockets;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail
{

	#region public enum ReadReplyCode
	/// <summary>
	/// Reply reading return codes.
	/// </summary>
	internal enum ReadReplyCode
	{
		/// <summary>
		/// Read completed successfully.
		/// </summary>
		Ok             = 0,

		/// <summary>
		/// Read timed out.
		/// </summary>
		TimeOut        = 1,

		/// <summary>
		/// Maximum allowed lengh exceeded.
		/// </summary>
		LenghExceeded = 2,

		/// <summary>
		/// UnKnown error, eception raised.
		/// </summary>
		UnKnownError   = 3,

		/// <summary>
		/// Closed error, eception raised.
		/// </summary>
		SocketClosed   = 4,
	}
	#endregion

	/// <summary>
	/// Common에 대한 요약 설명입니다.
	/// </summary>
	internal class Common
	{
		public Common()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//
		}


		#region function ReadLineFromSocket

		/// <summary>
		/// Reads line from socket. Line terminator is {CRLF} and maxLenght = 1024.
		/// </summary>
		/// <param name="socket">Referance to socket.</param>
		/// <param name="reply">Data that has been readen from socket.</param>
		/// <param name="cmdTimeOut">Command idle time out in milliseconds.</param>
		/// <returns></returns>
		public static ReadReplyCode ReadLineFromSocket(Socket socket,out string reply,int cmdTimeOut)
		{
			ReadReplyCode replyCode = ReadReplyCode.Ok;
			reply = "";

			try
			{			
				MemoryStream strm = null;
				replyCode = ReadReplyFromSocket(socket,out strm,null,1024,cmdTimeOut,"\r\n","\r\n",true);
				if(replyCode == ReadReplyCode.Ok)
				{
					byte[] byte_reply = strm.ToArray();
					reply = System.Text.Encoding.Default.GetString(byte_reply);
					strm.Close();
				}

			}
			catch(Exception ex)
			{
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.Common[ReadLineFromSocket] : ReadReplyCode.UnKnownError : " + ReadReplyCode.UnKnownError + " :", ex);
				replyCode = ReadReplyCode.UnKnownError;
			}

			return replyCode;
		}

		#endregion

		#region function ReadReplyFromSocket

		/// <summary>
		/// Reads reply from socket.
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="replyData">Data that has been readen from socket.</param>
		/// <param name="addData">Data that has will be written at the beginning of read data. This param may be null.</param>
		/// <param name="maxLenght">Maximum lenght of data which may read.</param>
		/// <param name="cmdIdleTimeOut">Command idle time out in milliseconds.</param>
		/// <param name="terminator">Terminator string which terminates reading. eg '\r\n'.</param>
		/// <param name="removeFromEnd">Removes following string from reply.NOTE: removes only if ReadReplyCode is Ok.</param>
		/// <param name="retIfLenExceeded">If true, returns function if maximum length is exceeded.If false, chuck data.</param>
		/// <returns>Return reply code.</returns>
		public static ReadReplyCode ReadReplyFromSocket(Socket socket,out MemoryStream replyData,byte[] addData,int maxLenght,int cmdIdleTimeOut,string terminator,string removeFromEnd,bool retIfLenExceeded)
		{
			ReadReplyCode replyCode = ReadReplyCode.Ok;	
			replyData = null;

			try
			{
				// Create memory stream, where to store data.
				MemoryStream strm = new MemoryStream();
				// Write additional data to the beginning of stream, if any specified.
				if(addData != null)
				{
					strm.Write(addData,0,addData.Length);
				}

				int lastDataTime = Environment.TickCount;
				while(true)
				{

					if(socket.Available > 0)
					{

						// If maximum lenght is exceeded and there is enough data for terminator, clear stream.
						// NOTE: we may not clear stream if there isn't enough data for terminator, beacuse
						//       some part of terminator may be in stream.
						if(replyCode == ReadReplyCode.LenghExceeded && socket.Available >= terminator.Length)
						{
							strm.SetLength(0);		
						}

						//--- Read data from socket and write to stream --//
						byte[] buf_Return = new byte[socket.Available];
						int received = socket.Receive(buf_Return);
						strm.Seek(0,SeekOrigin.End);
						strm.Write(buf_Return,0,received);
						//------------------------------------------------//

						//---- Check if maximum length is exceeded ------------------------------//
						if(replyCode != ReadReplyCode.LenghExceeded && strm.Length > maxLenght)
						{
							replyCode = ReadReplyCode.LenghExceeded;

							if(retIfLenExceeded)
							{							
								break;
							}
						}
						//-----------------------------------------------------------------------//
						
						//----- Check for terminator ---------------------------------------//
						if(strm.Length >= terminator.Length)
						{
							byte[] bufComp = new byte[terminator.Length];
							strm.Position = strm.Length - terminator.Length;
							strm.Read(bufComp,0,bufComp.Length);

							string strCompare = System.Text.Encoding.Default.GetString(bufComp);
														               				
							// check for terminator.
							if(socket.Available == 0 && strCompare.IndexOf(terminator) > -1)
							{								
								break;
							}
						}
						//--------------------------------------------------------------------//
						
						// reset last data time
						lastDataTime = Environment.TickCount;
					}

					
					//---- Idle and time out stuff ----------------------------------------//
					if(socket.Available == 0)
					{
						if(Environment.TickCount > lastDataTime + cmdIdleTimeOut)
						{
							replyCode = ReadReplyCode.TimeOut;
							break;
						}					
						System.Threading.Thread.Sleep(30);
					}				
					//-----------------------------------------------------------//
				}

				// If reply is ok then remove chars if any specified by 'removeFromEnd'.
				// Remove specified char's from end.
				if(replyCode == ReadReplyCode.Ok && removeFromEnd.Length > 0)
				{
					strm.SetLength(strm.Length - removeFromEnd.Length);
				}
	
				replyData = strm;
			}
			catch(Exception ex)
			{
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.Common[ReadReplyFromSocket] : ReadReplyCode.UnKnownError : " + ReadReplyCode.UnKnownError + " :", ex);
				replyCode = ReadReplyCode.UnKnownError;				
			}

			return replyCode;
		}

		#endregion
	}
}
