using System;
using System.Collections;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail
{
	/// <summary>
	/// MailDataInfo에 대한 요약 설명입니다.
	/// </summary>
	[Serializable]
	public class MailDataInfo
	{
		#region 생성자
		
		/// <summary>
		/// Creates a new <see cref="MailDataInfo"/> instance.
		/// </summary>
		public MailDataInfo()
		{
			to = new Hashtable();
			cc = new Hashtable();
			bcc = new Hashtable();
			attach = new Hashtable();
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//
		}

		#endregion

		#region 필드
		
		/// <summary>
		/// characterSet
		/// </summary>
		private string characterSet = "utf-8";

		/// <summary>
		/// to
		/// </summary>
		private Hashtable to = null;

		/// <summary>
		/// cc
		/// </summary>
		private Hashtable cc = null;

		/// <summary>
		/// bcc
		/// </summary>
		private Hashtable bcc = null;

		/// <summary>
		/// attach
		/// </summary>
		private Hashtable attach = null;

		/// <summary>
		/// from
		/// </summary>
		private string from;

		/// <summary>
		/// important
		/// </summary>
		private string important = "3";

		/// <summary>
		/// subject
		/// </summary>
		private string subject;

		/// <summary>
		/// contents
		/// </summary>
		private string contents;

		/// <summary>
		/// mailMessageID
		/// </summary>
		private string mailMessageID;
		#endregion

		#region 프로퍼티
		/// <summary>
		/// CharacterSet.
		/// </summary>
		public string CharacterSet
		{
			get{return characterSet;}
			set{characterSet = value;}
		}

		/// <summary>
		/// To.
		/// </summary>
		public Hashtable To
		{
			get{return to;}
			set{to = value;}
		}

		/// <summary>
		/// Cc.
		/// </summary>
		public Hashtable Cc
		{
			get{return cc;}
			set{cc = value;}
		}

		/// <summary>
		/// Bcc.
		/// </summary>
		public Hashtable Bcc
		{
			get{return bcc;}
			set{bcc = value;}
		}

		/// <summary>
		/// attach
		/// </summary>
		public Hashtable Attach
		{
			get{return attach;}
			set{attach = value;}
		}

		/// <summary>
		/// from
		/// </summary>
		public string From
		{
			get{return from;}
			set{from = value;}
		}

		/// <summary>
		/// important
		/// </summary>
		public string Important
		{
			get{return important;}
			set{important = value;}
		}

		/// <summary>
		/// subject
		/// </summary>
		public string Subject
		{
			get{return subject;}
			set{subject = value;}
		}

		/// <summary>
		/// contents
		/// </summary>
		public string Contents
		{
			get{return contents;}
			set{contents = value;}
		}
   
		/// <summary>
		/// mailMessageID
		/// </summary>
		public string MailMessageID
		{
			get{return mailMessageID;}
			set{mailMessageID = value;}
		}
		#endregion
	}
}
