using System;
using System.IO;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// TweMimeUtil에 대한 요약 설명입니다.
    /// </summary>
    public class MimeUtilClass
    {
        private string strCRLF = "\r\n";
        public string DecodedHTML = "";
        public string FileList;
        private int ActualFileCount = 0;

        public MimeUtilClass()
        {
        }

        public void ProcessDecoding(string m_strMimeValue, string strSavePath, string strSaveUrl, int nRename)
        {
            string m_strBoundary = string.Empty;
            string m_strEncodePartData = string.Empty;

            int iHeaderTemp = 0;
            int iHeaderEnd = 0;

            string[] arrText = null;

            int indexCRLF = 0;
            string strHeader = string.Empty;
            string strContent = string.Empty;
            string strContentID = string.Empty;
            string strContentLocation = string.Empty;
            string strContentTransferEncoding = string.Empty;
            string strContentMainType = string.Empty;
            string strContentSubType = string.Empty;

            string strContentCharset = string.Empty;
            string[] strHeaders = null;

            string strHeaderItem = string.Empty;
            int indexParam = 0;

            string strParam = string.Empty;
            string strValue = string.Empty;

            int iSlash = 0;

            int iColon = 0;

            string strName = string.Empty;

            int _Buffer_size = 1048576;
            MemoryStream inStream = null;
            StreamReader CharReader = null;
            int indexFileName = 0;

            int indexQ = 0;

            string strFilePath = string.Empty;
            System.IO.FileInfo fileInfo = null;
            bool bExist = false;
            int nNumbering = 1;
            string strFileName = string.Empty;
            int indexDot = 0;
            string strRenamedFileName = string.Empty;

            FileInfo path = null;
            FileStream outStream = null;

            string line = string.Empty;
            byte[] ByteArray = null;

            try
            {
                if (m_strMimeValue == null)
                {
                    return;
                }

                if (m_strMimeValue.Equals(string.Empty))
                {
                    return;
                }

                m_strBoundary = FindBoundaryString(m_strMimeValue);

                iHeaderTemp = m_strMimeValue.IndexOf(m_strBoundary);
                iHeaderEnd = iHeaderTemp + m_strBoundary.Length;

                m_strMimeValue = m_strMimeValue.Substring(iHeaderEnd);

                arrText = System.Text.RegularExpressions.Regex.Split(m_strMimeValue, m_strBoundary);

                for (int i = 0; i < arrText.Length; i++)
                {
                    m_strEncodePartData = arrText[i];

                    //2. 파트 구분 처리가 바뀌었음
                    if (m_strEncodePartData.Length <= 9)
                    {
                        continue;
                    }

                    indexCRLF = m_strEncodePartData.IndexOf(strCRLF + strCRLF);
                    strHeader = m_strEncodePartData.Substring(0, indexCRLF).Trim();
                    strContent = m_strEncodePartData.Substring(indexCRLF).Trim();
                    strHeaders = System.Text.RegularExpressions.Regex.Split(strHeader, strCRLF);
                    strContentID = string.Empty;
                    strContentLocation = string.Empty;
                    strContentTransferEncoding = string.Empty;
                    strContentMainType = string.Empty;
                    strContentSubType = string.Empty;

                    for (int k = 0; k < strHeaders.Length; k++)
                    {
                        strHeaderItem = strHeaders[k];

                        indexParam = strHeaderItem.IndexOf(":");

                        if (strHeaderItem.IndexOf("utf-8") != -1)
                        {
                            strContentCharset = "utf-8";
                        }

                        if (indexParam != -1)
                        {
                            strParam = strHeaderItem.Substring(0, indexParam).Trim();
                            strValue = strHeaderItem.Substring(indexParam + 1).Trim();

                            if (string.Compare(strParam, "content-id", true) == 0)
                            {
                                strContentID = strValue;
                            }

                            if (string.Compare(strParam, "content-location", true) == 0)
                            {
                                strContentLocation = strValue;
                            }

                            if (string.Compare(strParam, "content-transfer-encoding", true) == 0)
                            {
                                strContentTransferEncoding = strValue;
                            }

                            if (string.Compare(strParam, "content-type", true) == 0)
                            {
                                iSlash = strValue.IndexOf("/");

                                if (iSlash == -1)
                                {
                                    strContentMainType = strValue;
                                }
                                else
                                {
                                    strContentMainType = strValue.Substring(0, iSlash).Trim();

                                    iColon = strValue.IndexOf(";", iSlash);

                                    if (iColon == -1)
                                    {
                                        strContentSubType = strValue.Substring(iSlash + 1);
                                    }
                                    else
                                    {
                                        strContentSubType = strValue.Substring(0, iColon);
                                        strContentSubType = strContentSubType.Substring(iSlash + 1);
                                    }
                                }
                            }
                        }
                    }

                    strName = "";

                    if (string.Compare(strContentSubType, "html", true) == 0)
                    {
                        if (strContentCharset.IndexOf("utf-8") == -1)
                            DecodedHTML = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(strContent));
                        else
                            DecodedHTML = System.Text.UnicodeEncoding.UTF8.GetString(System.Convert.FromBase64String(strContent));
                    }
                    else
                    {
                        inStream = new MemoryStream(System.Text.Encoding.Default.GetBytes(strContent));
                        CharReader = new StreamReader(inStream, System.Text.Encoding.Default, true, _Buffer_size);
                        indexFileName = strContentLocation.LastIndexOf("\\");

                        if (indexFileName == -1)
                        {
                            indexFileName = strContentLocation.LastIndexOf("/");
                        }

                        strName = strContentLocation.Substring(indexFileName + 1);
                        //must be url decode
                        strName = strName.Replace("%20", " ");
                        strName = strName.Replace("%5B", "[");
                        strName = strName.Replace("%5D", "]");
                        strName = strName.Replace("%60", "`");

                        indexQ = strName.IndexOf("?");

                        if (indexQ != -1)
                        {
                            strName = strName.Substring(0, indexQ);
                        }

                        if (strName.Length != 0)
                        {
                            strFilePath = strSavePath + "\\" + strName;
                            fileInfo = new System.IO.FileInfo(strFilePath);
                            strName = fileInfo.Name;
                            bExist = fileInfo.Exists;
                            nNumbering = 1;
                            strFileName = "";
                            indexDot = strName.IndexOf(".");

                            if (indexDot != -1)
                            {
                                strFileName = strName.Substring(0, indexDot);
                            }

                            if (nRename == 1)
                            {
                                while (bExist)
                                {
                                    strRenamedFileName = strFileName + "_(" + nNumbering + ")";
                                    strFilePath = fileInfo.DirectoryName + "\\" + strRenamedFileName + fileInfo.Extension;
                                    strName = strRenamedFileName + fileInfo.Extension;
                                    bExist = File.Exists(strFilePath);
                                    nNumbering++;
                                }
                            }

                            //1. 폴더 자동 생성 로직
                            if (strFilePath != null || strFilePath != string.Empty)
                            {
                                path = new FileInfo(strFilePath);
                                Directory.CreateDirectory(path.DirectoryName);

                                outStream = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, _Buffer_size);

                                while ((line = CharReader.ReadLine()) != null)
                                {
                                    ByteArray = System.Convert.FromBase64String(line);
                                    outStream.Write(ByteArray, 0, ByteArray.Length);
                                }

                                if (outStream != null)
                                {
                                    outStream.Close();
                                    outStream.Dispose();
                                    outStream = null;
                                }

                                if (ActualFileCount == 0)
                                {
                                    FileList = FileList + strName;
                                }
                                else
                                {
                                    FileList = FileList + ";" + strName;
                                }

                                ActualFileCount++;
                            }
                            //cid 처리
                            if (strContentID.Length != 0)
                            {
                                if (DecodedHTML.Length != 0)
                                {
                                    strContentID = strContentID.Substring(1);
                                    strContentID = strContentID.Substring(0, strContentID.Length - 1);
                                    strContentID = "cid:" + strContentID;
                                    DecodedHTML = DecodedHTML.Replace(strContentID, strSaveUrl + "/" + strName);
                                }
                            }
                        }

                        if (inStream != null)
                        {
                            inStream.Close();
                            inStream.Dispose();
                            inStream = null;
                        }

                        if (CharReader != null)
                        {
                            CharReader.Close();
                            CharReader.Dispose();
                            CharReader = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ProcessDecoding", ex);
                throw ex;
            }
            finally
            {
                if (inStream != null)
                {
                    inStream.Close();
                    inStream.Dispose();
                    inStream = null;
                }

                if (CharReader != null)
                {
                    CharReader.Close();
                    CharReader.Dispose();
                    CharReader = null;
                }

                if (outStream != null)
                {
                    outStream.Close();
                    outStream.Dispose();
                    outStream = null;
                }

                m_strBoundary = null;
                m_strEncodePartData = null;

                arrText = null;

                strHeader = null;
                strContent = null;
                strContentID = null;
                strContentLocation = null;
                strContentTransferEncoding = null;
                strContentMainType = null;
                strContentSubType = null;
                strHeaders = null;

                strHeaderItem = null;

                strParam = null;
                strValue = null;

                strName = null;

                strFilePath = null;
                fileInfo = null;
                strFileName = null;
                strRenamedFileName = null;
                path = null;
                line = null;
                ByteArray = null;
                strContentCharset = null;
            }
        }

        private string FindBoundaryString(string m_strMimeValue)
        {
            string strBoundary = string.Empty;
            int iBoundStart = 0;
            int iBoundEnd = 0;
            string strReturn = string.Empty;

            try
            {
                strBoundary = "boundary=";

                iBoundStart = m_strMimeValue.IndexOf(strBoundary, 0);

                if (iBoundStart != -1)
                {
                    iBoundEnd = m_strMimeValue.IndexOf(strCRLF, iBoundStart);

                    if (iBoundEnd != -1)
                    {
                        strReturn = m_strMimeValue.Substring(iBoundStart + strBoundary.Length, iBoundEnd - (iBoundStart + strBoundary.Length)).Trim();
                        return strReturn = "--" + strReturn;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("FindBoundaryString", ex);
                throw ex;
            }
            finally
            {
                strBoundary = null;
            }

            return strReturn;
        }
    }
}