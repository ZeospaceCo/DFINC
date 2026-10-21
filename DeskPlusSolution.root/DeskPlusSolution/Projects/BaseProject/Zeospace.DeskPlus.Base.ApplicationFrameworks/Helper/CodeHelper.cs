using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CodeHelper
    {
        #region EncryptToByte(), DecryptFromByte(), DecodeEucKr(), ConvertToHex()

        /// <summary>
        /// 문자열을 "엔퍼센트" 로 겹합된 바이트 배열형태로 변환한다.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string EncryptToByte(string src)
        {
            byte[] bytes = Encoding.Default.GetBytes(src);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append("&");
                }
                sb.Append(bytes[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// "엔퍼센트"로 겹합된 바이트 배열형태의 문자열을 문자로 변환한다.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecryptFromByte(string s)
        {
            string[] src = s.Split('&');
            byte[] bytes = new byte[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                bytes[i] = Convert.ToByte(src[i]);
            }

            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// 바이트배열형태의 문자열을 디코딩한다.
        /// </summary>
        /// <param name="src">Euc-kr바이트배열형태의 문자열</param>
        /// <returns>UTF-8 문자열</returns>
        public static string DecodeEucKr(string src)
        {
            StringBuilder decStr = new StringBuilder(src.Length / 2);
            char[] chArray = src.ToCharArray();

            for (int i = 0; i < chArray.Length; i++)
            {
                if (chArray[i] == '+')
                {	// Space
                    decStr.Append(' ');
                }
                else if (chArray[i] == '%')
                {	// 2Byte Character
                    string hexStr1 = src.Substring(i, 3).Replace("%", "");
                    int hex1 = ConvertToHex(hexStr1);

                    if (hex1 < 0 || hex1 >= 128)
                    {
                        string hexStr2 = src.Substring(i + 3, 3).Replace("%", "");

                        byte[] charCode = new byte[2];
                        charCode[0] = Convert.ToByte(ConvertToHex(hexStr1));
                        charCode[1] = Convert.ToByte(ConvertToHex(hexStr2));

                        decStr.Append(Encoding.Default.GetString(charCode));

                        i += 5;
                    }
                    else
                    {
                        byte[] charCode = new byte[1];
                        charCode[0] = Convert.ToByte(hex1);

                        decStr.Append(Encoding.Default.GetString(charCode));

                        i += 2;
                    }
                }
                else
                {	// 1Byte Character
                    decStr.Append(chArray[i]);
                }
            }

            return decStr.ToString();
        }

        /// <summary>
        /// 문자열을 Hex형태의 정수값으로 변환한다.
        /// </summary>
        /// <param name="s">Hex문자열</param>
        /// <returns>Hex정수값</returns>
        public static int ConvertToHex(string s)
        {
            int retVal = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (i == s.Length - 1)
                    retVal += ConvertToHex(s[i]);
                else
                    retVal = ConvertToHex(s[i]) * (16 * (s.Length - 1));
            }

            return retVal;
        }

        /// <summary>
        /// 문자를 Hex형태의 정수값으로 변환한다.
        /// </summary>
        /// <param name="c">Hex문자</param>
        /// <returns>Hex정수값</returns>
        public static int ConvertToHex(char c)
        {
            int retVal = 0;
            switch (c)
            {
                case 'A':
                case 'a':
                    retVal = 10;
                    break;

                case 'B':
                case 'b':
                    retVal = 11;
                    break;

                case 'C':
                case 'c':
                    retVal = 12;
                    break;

                case 'D':
                case 'd':
                    retVal = 13;
                    break;

                case 'E':
                case 'e':
                    retVal = 14;
                    break;

                case 'F':
                case 'f':
                    retVal = 15;
                    break;

                default:
                    retVal = (char.IsDigit(c) ? Convert.ToInt32(c.ToString()) : 0);
                    break;
            }

            return retVal;
        }

        #endregion EncryptToByte(), DecryptFromByte(), DecodeEucKr(), ConvertToHex()

        #region ShowMessageBox()

        /// <summary>
        /// 메시지 박스를 출력한다.
        /// </summary>
        /// <param name="page">대상 페이지</param>
        /// <param name="key">스크립트Key</param>
        /// <param name="message">출력메시지</param>
        public static void ShowMessageBox(Page page, string key, string message)
        {
            ShowMessageBox(page, key, message, MessageType.Information);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void ShowMessageBox(Page page, string key, string message, MessageType type)
        {
            if (!page.ClientScript.IsStartupScriptRegistered(key))
            {
                string SCRIPT_MESSAGEBOX = String.Format("alert('{0}')", message);

                page.ClientScript.RegisterStartupScript(page.GetType(), key, SCRIPT_MESSAGEBOX, true);
            }
        }

        #endregion ShowMessageBox()

        #region ShowPopupWindow() - Not Released

        /// <summary>
        /// Confirm 메시지를 처리하는 창
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        public static void ShowPopupWindow(Page page, string key, string url)
        {
            string SCRIPT_MESSAGEBOX = String.Format("window.open('{0}','ShowPopupWindow','width=480,height=350,resizable=0,status=1,titlebar=0,toolbar=0,scrollbars=0');", url);

            if (!page.ClientScript.IsStartupScriptRegistered(key))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), key, SCRIPT_MESSAGEBOX, true);
            }
        }

        /// <summary>
        ///  Confirm 메시지를 처리하는 창
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="url"></param>
        public static void ShowPopup(Page page, string key, string url)
        {
            string SCRIPT_MESSAGEBOX = String.Format("window.open('{0}','ShowPopup');", url);

            if (!page.ClientScript.IsStartupScriptRegistered(key))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), key, SCRIPT_MESSAGEBOX, true);
            }
        }

        #endregion ShowPopupWindow() - Not Released

        #region ShowGlobalMessage()

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="messageKey"></param>
        /// <param name="type"></param>
        public static void ShowGlobalMessage(Page page, string messageKey, MessageType type)
        {
            string msg = (string)HttpContext.GetGlobalResourceObject("MessageTable", messageKey);

            ShowMessageBox(page, messageKey, msg, type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="messageKey"></param>
        public static void ShowGlobalMessage(Page page, string messageKey)
        {
            ShowGlobalMessage(page, messageKey, MessageType.Information);
        }

        #endregion ShowGlobalMessage()

        #region Redirect()

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        public static void Redirect(string message, string url)
        {
            HttpContext ctx = HttpContext.Current;

            ctx.Response.Clear();

            string SCRIPT_MESSAGE_REDIRECT = @"<script language='javascript'>alert('{0}'); window.location='{1}';</script>";
            ctx.Response.Write(String.Format(SCRIPT_MESSAGE_REDIRECT, message, url));

            ctx.Response.Flush();
            ctx.Response.End();
        }

        #endregion Redirect()

        #region IsNullOrEmpty()

        /// <summary>
        ///
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(DataSet ds)
        {
            return ds == null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(DataSet ds, int tableIndex)
        {
            return !(!IsNullOrEmpty(ds) && (ds.Tables.Count >= tableIndex + 1) && !IsNullOrEmpty(ds.Tables[tableIndex]));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(DataSet ds, string tableName)
        {
            return !(!IsNullOrEmpty(ds) && ds.Tables.Contains(tableName) && !IsNullOrEmpty(ds.Tables[tableName]));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(DataTable dt)
        {
            return !(dt != null && dt.Rows.Count > 0);
        }

        #endregion IsNullOrEmpty()

        #region Select Item in ListItemCollection

        /// <summary>
        /// DropDownList의 Items중에서 전달한 문자열값과 일치하는
        /// Text속성을 가지는 Item을 찾아서 선택한다.
        /// </summary>
        /// <param name="dropDownList">대상 DropDownList 컨트롤</param>
        /// <param name="text">Text 문자열</param>
        public static void SelectItemByText(DropDownList dropDownList, string text)
        {
            dropDownList.ClearSelection();
            SelectItemByText(dropDownList.Items, text);
        }

        /// <summary>
        /// DropDownList의 Items중에서 전달한 문자열값과 일치하는
        /// Value속성을 가지는 Item을 찾아서 선택한다.
        /// </summary>
        /// <param name="dropDownList">대상 DropDownList 컨트롤</param>
        /// <param name="value">Value 문자열</param>
        public static void SelectItemByValue(DropDownList dropDownList, string value)
        {
            dropDownList.ClearSelection();
            SelectItemByValue(dropDownList.Items, value);
        }

        /// <summary>
        /// RadioButtonList의 Items중에서 전달한 문자열값과 일치하는 Text속성을 가지는 Item을 찾아서 선택한다.
        /// </summary>
        /// <param name="radioButtonList"></param>
        /// <param name="text"></param>
        public static void SelectItemByText(RadioButtonList radioButtonList, string text)
        {
            radioButtonList.ClearSelection();
            SelectItemByText(radioButtonList.Items, text);
        }

        /// <summary>
        /// RadioButtonList의 Items중에서 전달한 문자열값과 일치하는
        /// Value속성을 가지는 Item을 찾아서 선택한다.
        /// </summary>
        /// <param name="radioButtonList">대상 RadioButtonList 컨트롤</param>
        /// <param name="value">Value 문자열</param>
        public static void SelectItemByValue(RadioButtonList radioButtonList, string value)
        {
            radioButtonList.ClearSelection();
            SelectItemByValue(radioButtonList.Items, value);
        }

        /// <summary>
        ///  ListItemCollection에서 Text속성이 일치하는 Item을 선택한다.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="text"></param>
        public static void SelectItemByText(ListItemCollection items, string text)
        {
            ListItem item = items.FindByText(text);

            if (item != null)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// ListItemCollection에서 Value속성이 일치하는 Item을 선택한다.
        /// </summary>
        /// <param name="items">ListItemCollection</param>
        /// <param name="value">Value</param>
        public static void SelectItemByValue(ListItemCollection items, string value)
        {
            ListItem item = items.FindByValue(value);

            if (item != null)
            {
                item.Selected = true;
            }
        }

        #endregion Select Item in ListItemCollection

        #region DrawEmptyGridView()

        /// <summary>
        /// EmptyDataRow 츨력시 헤더를 포함한 정보를 출력한다.
        /// * CSS의 정의가 필수적, border=0 기본설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void DrawEmptyGridView(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                GridView gridView = (GridView)sender;

                string emptyString = gridView.EmptyDataText;
                StringBuilder sbEmptyGrid = new StringBuilder();

                sbEmptyGrid.AppendFormat("<table border=\"0\" class=\"{0}\">", gridView.CssClass);
                sbEmptyGrid.Append("<tr>");

                int colSpan = 0;
                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (gridView.Columns[i].Visible)
                    {
                        double width = gridView.Columns[i].HeaderStyle.Width.Value;
                        if (width > 0)
                        {
                            sbEmptyGrid.AppendFormat("<th style=\"width:{0};\">", width);
                        }
                        else
                        {
                            sbEmptyGrid.Append("<th>");
                        }

                        sbEmptyGrid.Append(gridView.Columns[i].HeaderText);
                        sbEmptyGrid.Append("</th>");
                        colSpan++;
                    }
                }

                sbEmptyGrid.Append("</tr>");

                sbEmptyGrid.Append("<tr>");
                sbEmptyGrid.AppendFormat("<td colSpan=\"{0}\">", colSpan);
                sbEmptyGrid.Append(emptyString);
                sbEmptyGrid.Append("</td>");
                sbEmptyGrid.Append("</tr>");

                sbEmptyGrid.Append("</table>");

                e.Row.Cells[0].Text = sbEmptyGrid.ToString();
            }
        }

        #endregion DrawEmptyGridView()

        #region BindPagingGridView(), GetPagingDataTable()

        /// <summary>
        ///
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="dt"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public static void BindPagingGridView(GridView gridView, DataTable dt, int pageSize, int pageIndex)
        {
            DataTable dtPager = GetPagingDataTable(dt, pageSize, pageIndex);

            gridView.DataSource = dtPager;
            gridView.DataBind();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static DataTable GetPagingDataTable(DataTable dt, int pageSize, int pageIndex)
        {
            return GetPagingDataTable(dt, pageSize, pageIndex, "0");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pagingType"></param>
        /// <returns></returns>
        public static DataTable GetPagingDataTable(DataTable dt, int pageSize, int pageIndex, string pagingType)
        {
            int totalCount = dt.Rows.Count;

            DataTable dtPager = dt.Clone();

            int startCount = pageIndex * pageSize;
            int endCount = (pageIndex * pageSize) + pageSize;
            if (endCount > totalCount)
            {
                endCount = totalCount;
            }

            if (pagingType.Equals("1"))
                startCount = 0;

            for (; startCount < endCount; startCount++)
            {
                dtPager.LoadDataRow(dt.Rows[startCount].ItemArray, false);
            }

            return dtPager;
        }

        #endregion BindPagingGridView(), GetPagingDataTable()

        #region SelectByText(), SelectByValue()

        /// <summary>
        ///
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="value"></param>
        public static void SelectByValue(DropDownList dropDownList, string value)
        {
            dropDownList.SelectedIndex = -1;

            ListItem item = dropDownList.Items.FindByValue(value);
            if (item != null)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="radioButtonList"></param>
        /// <param name="value"></param>
        public static void SelectByValue(RadioButtonList radioButtonList, string value)
        {
            radioButtonList.SelectedIndex = -1;

            ListItem item = radioButtonList.Items.FindByValue(value);
            if (item != null)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="checkBoxList"></param>
        /// <param name="value"></param>
        public static void SelectByValue(CheckBoxList checkBoxList, string value)
        {
            checkBoxList.SelectedIndex = -1;

            string[] values = value.Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                ListItem item = checkBoxList.Items.FindByValue(values[i]);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        #endregion SelectByText(), SelectByValue()

        #region xml read

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public static DataSet xmlNodeReader(string xmlString, NodeType nodeType)
        {
            xmlString = xmlString.Trim();
            DataSet dsNode = new DataSet();
            StringReader strReader = new StringReader(xmlString);
            XmlTextReader myReader = new XmlTextReader(strReader);
            myReader.WhitespaceHandling = WhitespaceHandling.Significant;

            while (myReader.Read())
            {
                if (myReader.NodeType != XmlNodeType.XmlDeclaration
                        && myReader.NodeType != XmlNodeType.EndElement &&
                            myReader.Name.Trim() != "") //myReader.Name != nodeType.ToString() &&
                {
                    if (dsNode.Tables.Count == 0)
                    {
                        dsNode.Tables.Add();
                    }

                    if (!dsNode.Tables[0].Columns.Contains(myReader.Name))
                    {
                        dsNode.Tables[0].Columns.Add(myReader.Name);
                    }

                    if (dsNode.Tables[0].Rows.Count == 0)
                    {
                        DataRow drValue = dsNode.Tables[0].NewRow();
                        dsNode.Tables[0].Rows.Add(drValue);
                    }

                    dsNode.Tables[0].Rows[0][myReader.Name] = myReader.ReadString();
                }
            }

            myReader.Close();
            strReader.Close();

            return dsNode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="myReader"></param>
        /// <returns></returns>
        public static DataSet XmlConvertDataSet(XmlTextReader myReader)
        {
            int row = 0;
            DataSet dsNode = new DataSet();
            myReader.WhitespaceHandling = WhitespaceHandling.Significant;

            dsNode.Tables.Add();
            dsNode.Tables[0].Columns.Add("KeyCode");
            dsNode.Tables[0].Columns.Add("CodeDesc");

            while (myReader.Read())
            {
                if (myReader.NodeType == XmlNodeType.Element && myReader.HasAttributes)
                {
                    DataRow drValue = dsNode.Tables[0].NewRow();
                    dsNode.Tables[0].Rows.Add(drValue);

                    dsNode.Tables[0].Rows[row]["KeyCode"] = myReader.Name;
                    dsNode.Tables[0].Rows[row]["CodeDesc"] = myReader.Value;

                    if (myReader.MoveToFirstAttribute())
                    {
                        if (!dsNode.Tables[0].Columns.Contains(myReader.Name))
                        {
                            dsNode.Tables[0].Columns.Add(myReader.Name);
                        }

                        dsNode.Tables[0].Rows[row][myReader.Name] = myReader.Value;

                        while (myReader.MoveToNextAttribute())
                        {
                            if (!dsNode.Tables[0].Columns.Contains(myReader.Name))
                            {
                                dsNode.Tables[0].Columns.Add(myReader.Name);
                            }

                            dsNode.Tables[0].Rows[row][myReader.Name] = myReader.Value;
                        }
                    }

                    row++;
                    myReader.MoveToElement();
                }
            }

            myReader.Close();
            return dsNode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="myReader"></param>
        /// <returns></returns>
        public static DataSet XmlConvertMenuDataSet(XmlTextReader myReader)
        {
            int row = 0;
            string RootMenu = null;
            DataSet dsNode = new DataSet();
            myReader.WhitespaceHandling = WhitespaceHandling.Significant;

            dsNode.Tables.Add();
            dsNode.Tables[0].Columns.Add("RootMenu");
            dsNode.Tables[0].Columns.Add("ParentMenu");
            dsNode.Tables[0].Columns.Add("MenuDesc");

            while (myReader.Read())
            {
                if (myReader.NodeType == XmlNodeType.Element && myReader.HasAttributes)
                {
                    DataRow drValue = dsNode.Tables[0].NewRow();
                    dsNode.Tables[0].Rows.Add(drValue);

                    if (myReader.GetAttribute("MenuAsc") == "0")
                    {
                        RootMenu = myReader.Name;
                        dsNode.Tables[0].Rows[row]["RootMenu"] = myReader.Name;
                        dsNode.Tables[0].Rows[row]["ParentMenu"] = "";
                        dsNode.Tables[0].Rows[row]["MenuDesc"] = "";
                    }
                    else
                    {
                        dsNode.Tables[0].Rows[row]["RootMenu"] = RootMenu;
                        dsNode.Tables[0].Rows[row]["ParentMenu"] = myReader.Name;
                        dsNode.Tables[0].Rows[row]["MenuDesc"] = myReader.Value;
                    }

                    if (myReader.MoveToFirstAttribute())
                    {
                        if (!dsNode.Tables[0].Columns.Contains(myReader.Name))
                        {
                            dsNode.Tables[0].Columns.Add(myReader.Name);
                        }

                        dsNode.Tables[0].Rows[row][myReader.Name] = myReader.Value;

                        while (myReader.MoveToNextAttribute())
                        {
                            if (!dsNode.Tables[0].Columns.Contains(myReader.Name))
                            {
                                dsNode.Tables[0].Columns.Add(myReader.Name);
                            }

                            dsNode.Tables[0].Rows[row][myReader.Name] = myReader.Value;
                        }
                    }

                    row++;
                    myReader.MoveToElement();
                }
            }

            myReader.Close();
            return dsNode;
        }

        #endregion xml read

        #region Password Create

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string PasswordCreate()
        {
            Guid newPass = Guid.NewGuid();

            return newPass.ToString().Replace("-", "").Substring(0, 10);
        }

        /// <summary>
        /// 지정된 암호와 해시 알고리즘을 기반으로 구성 파일에 저장하기에 적합한 해시 암호를 만듭니다.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CastCryptograph(string password)
        {
            //지정된 암호와 해시 알고리즘을 기반으로 구성 파일에 저장하기에 적합한 해시 암호를 만듭니다.(MD5,SHA1)
            string hashMethod = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "HashMethod", "");
            if (hashMethod.Equals(""))
            {
                hashMethod = "SHA1";
            }

            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, hashMethod);
        }

        #endregion Password Create

        #region SetDisplayStyle(), ToggleDisplayStyle()

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>
        /// <param name="display"></param>
        public static void SetDisplayStyle(WebControl control, bool display)
        {
            control.Style["display"] = display ? "" : "none";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>
        public static void ToggleDisplayStyle(WebControl control)
        {
            control.Style["display"] = control.Style["display"].Equals("none", StringComparison.OrdinalIgnoreCase) ? "" : "none";
        }

        #endregion SetDisplayStyle(), ToggleDisplayStyle()

        #region Mail Sending

        /// <summary>
        ///
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="fromMail"></param>
        /// <param name="mailSuject"></param>
        /// <param name="mailAddress"></param>
        /// <param name="MailContent"></param>
        /// <param name="smtp"></param>
        /// <param name="smtpPort"></param>
        /// <param name="smtpAuthent"></param>
        /// <param name="smtpUserName"></param>
        /// <param name="smtpUserPass"></param>
        /// <param name="myBlogurl"></param>
        /// <param name="filePath"></param>
        /// <param name="myUrl"></param>
        /// <returns></returns>
        public static bool SendMailing(string fromName, string fromMail, string mailSuject, string[] mailAddress, string MailContent
            , string smtp, int smtpPort, bool smtpAuthent, string smtpUserName, string smtpUserPass, string myBlogurl, string filePath, string myUrl)
        {
            bool msg = false;
            string pattern = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(pattern);

            if (!rex.IsMatch(fromMail))
            {
                return msg;
            }
            MailMessage msgMail = new MailMessage();

            try
            {
                msgMail.From = new MailAddress(fromName + "<" + fromMail + ">");

                StreamReader sr;
                String line;

                // string[] mailAddress = txtUsersMail.Text.Split(';');

                foreach (string mailRow in mailAddress)
                {
                    if (mailRow.Trim() == "")
                        break;

                    if (!rex.IsMatch(mailRow))
                    {
                        msg = false;
                        return msg;
                    }

                    // IO & Mail Send
                    msgMail.To.Add(new MailAddress(mailRow));
                    msgMail.Subject = mailSuject;

                    sr = new StreamReader(filePath, System.Text.Encoding.Default);
                    line = string.Format(sr.ReadToEnd().ToString(), new object[5] { mailRow, MailContent, myBlogurl, fromMail, myUrl });
                    msgMail.Body = line;

                    sr.Close();

                    msgMail.BodyEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                    msgMail.IsBodyHtml = true;

                    SmtpClient SmtpMail = new SmtpClient(smtp, smtpPort);

                    if (smtpAuthent)
                    {
                        SmtpMail.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpUserPass);

                        SmtpMail.EnableSsl = true;
                        SmtpMail.Timeout = 60;
                    }

                    SmtpMail.Send(msgMail);
                }

                msg = true;
                return msg;
            }
            catch
            {
                msg = false;
            }
            finally
            {
            }

            return msg;
        }

        #endregion Mail Sending

        #region Substring()

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <param name="allowHtml"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static string Substring(string source, int startIdx, int endIdx, bool allowHtml, string footer)
        {
            // HTML을 허용하지 않는 경우 태그를 제거한다.
            if (!allowHtml)
            {
                source = Regex.Replace(source, @"\<[^>]*\>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                source = Regex.Replace(source, @"\s\s", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }

            if (source.Length > endIdx)
            {
                source = source.Substring(startIdx, endIdx);

                if (!String.IsNullOrEmpty(footer))
                {
                    source += footer;
                }
            }

            return source;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="endIdx"></param>
        /// <param name="allowHtml"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static string Substring(string source, int endIdx, bool allowHtml, string footer)
        {
            return Substring(source, 0, endIdx, allowHtml, footer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="endIdx"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        public static string Substring(string source, int endIdx, string footer)
        {
            return Substring(source, 0, endIdx, false, footer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="source"></param>
        /// <param name="endIdx"></param>
        /// <returns></returns>
        public static string Substring(string source, int endIdx)
        {
            return Substring(source, 0, endIdx, false, String.Empty);
        }

        #endregion Substring()
    }

    #region Enum : MessageType, PopUpType, NodeType

    /// <summary>
    ///
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        ///
        /// </summary>
        Information,

        /// <summary>
        ///
        /// </summary>
        Confirm,

        /// <summary>
        ///
        /// </summary>
        Error
    }

    /// <summary>
    ///
    /// </summary>
    public enum PopUpType
    {
        /// <summary>
        ///
        /// </summary>
        Modal,

        /// <summary>
        ///
        /// </summary>
        Modaless
    }

    /// <summary>
    ///
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        ///
        /// </summary>
        isblogversion,

        /// <summary>
        ///
        /// </summary>
        isblogskin,

        /// <summary>
        ///
        /// </summary>
        isblogtree,

        /// <summary>
        ///
        /// </summary>
        gadget
    }

    #endregion Enum : MessageType, PopUpType, NodeType
}