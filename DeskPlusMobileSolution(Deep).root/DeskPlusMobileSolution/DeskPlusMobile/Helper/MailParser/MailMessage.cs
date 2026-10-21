using System;
using System.Collections;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlusMobile.Web.Helper.MailParser
{
    public class MailMessage
    {

        private string m_Full_File_Name;
        private string m_Attach_Save_Path;
        private bool m_Secure;
        private byte[] m_Full_Eml;
        private string m_MsgID;
        private bool m_recall = false;
        public string[] Boundary;
        public string[] Boundary_str;
        public string ContentType;
        public string Content_CharSet;
        public string C_Encoding;
        private string m_Notification;
        public Hashtable X_Receive;
        private Hashtable m_to;
        public Hashtable Cc;
        public Hashtable Bcc;
        public string From;          // From : 
        public string Subject;       // 제목 ( Subject )            
        public string Contents_Html;
        public string Contents_Text;
        public DateTime Date;          // 날짜 			
        public string Important;
        public Hashtable Attach;
        public int Attach_Count;
        private bool m_x_forward = false;
        public long Size;
        //public MemoryStream Body;
        public StringBuilder Body_Str;
        public StringBuilder Header_Str;
        public Hashtable Content_ID;

        public string Attach_Save_Path
        {
            get { return m_Attach_Save_Path; }
            set { m_Attach_Save_Path = value; }
        }

        public string Notification
        {
            get { return m_Notification; }
            set { m_Notification = value; }
        }

        public string Full_File_Name
        {
            get { return m_Full_File_Name; }
            set { m_Full_File_Name = value; }
        }

        public byte[] Full_Eml
        {
            get { return m_Full_Eml; }
            set { m_Full_Eml = value; }
        }

        public Hashtable To
        {
            get { return m_to; }
            set { m_to = value; }
        }

        public bool Secure
        {
            get { return m_Secure; }
            set { m_Secure = value; }
        }

        public bool X_Forwarding
        {
            get { return m_x_forward; }
            set { m_x_forward = value; }
        }

        public bool recall
        {
            get { return m_recall; }
            set { m_recall = value; }
        }

        public string MsgID
        {
            get { return m_MsgID; }
            set { m_MsgID = value; }
        }
    }
}
