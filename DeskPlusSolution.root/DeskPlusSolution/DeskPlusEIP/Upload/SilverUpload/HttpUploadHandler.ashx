<%@ WebHandler Language="C#" Class="HttpUploadHandler" %>

using System;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Diagnostics;
using System.Web.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

public class HttpUploadHandler : IHttpHandler {

    private HttpContext _httpContext;
    private string _tempExtension = "_temp";//임시파일명 확장자
    private string _fileName;//실질 파일명
    private string _parameters; //인위적 파라미터
    private bool _lastChunk;
    private bool _firstChunk;
    private long _startByte;
    
    private string _guid = "";
    private string _companyID = "";
    private string _userID = "";
    private string _filterType = "";

    StreamWriter _debugFileStreamWriter;
    TextWriterTraceListener _debugListener;
       
    /// <summary>
    /// Start method
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        _httpContext = context;

        //첨부파일 존재여부 확인
        if (context.Request.InputStream.Length == 0)
            throw new ArgumentException("No file input");

        try
        {
            GetQueryStringParameters();
            
            if (string.IsNullOrEmpty(_parameters) || _parameters == "") 
                throw new ArgumentException("parameter is null error");

            _parameters = context.Server.UrlDecode(_parameters);
            _guid = _parameters.Split('|')[0];
            _companyID = _parameters.Split('|')[1];
            _userID = _parameters.Split('|')[2];
            _filterType = _parameters.Split('|')[3];//*.jpg;*.jpeg;*.png;*.gif
            _filterType = _filterType.Replace("*.", "");//jpg;jpeg;png;gif

            if (_guid.Length == 0) throw new ArgumentException("guid error");
            if (_companyID.Length == 0) throw new ArgumentException("CompanyID error");
            if (_userID.Length == 0) throw new ArgumentException("UserID error");

            if (_filterType.Length > 0)
            {
                bool isFilter = false;
                foreach (string filter in _filterType.Split(';'))
                {
                    if (filter != "*") //*.* 는 전체용이므로 pass
                    {
                        if (_fileName.ToUpper().IndexOf("." + filter.ToUpper()) != -1)
                        {
                            isFilter = true; 
                            break;
                        }
                    }
                    else if (filter == "*")
                    {
                        isFilter = true;
                        break;
                    } 
                }
                
                if (!isFilter) throw new ArgumentException("filter error");
            }

            #region 차단 확장자
            string blockExt = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingType.COMMON, "BlockExt", "");

            string[] blockExtStr = null;
            if (blockExt.Length > 0) blockExtStr = blockExt.Split(',');

            bool response = false;
            if (blockExtStr != null)
            {
                foreach (string ext in blockExtStr)
                {
                    int pos = _fileName.LastIndexOf(".");
                    if (pos > -1)
                    {
                        string fileExt = _fileName.Substring(_fileName.LastIndexOf("."), _fileName.Length - pos);

                        if (fileExt.ToLower() == "." + ext.ToLower())
                        {
                            response = true;
                            break;
                        }
                    }
                }
            }

            if (response)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.LogFile(
                    "file ext is problem. (UploadHandler) : " + _fileName, _httpContext.Server.MapPath("/DeskPlusEIP/Data/Logs/FileUpload/"), "");
                return;

            }
            #endregion

            string uploadFilePath = "/DeskPlusEIP/FileData/" + _companyID + "/Users/" + _userID + "/Spool/" + _guid + "/";

            #region 임시폴더 생성
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(context.Server.MapPath(uploadFilePath));
            if (di.Exists == false) di.Create();
            #endregion
            
            //StartDebugListener();           

            
            string tempFileName = _fileName + _tempExtension; //임시파일명 경로 만들기

            string tempPath = GetTempFilePath(tempFileName, _httpContext); //임시파일 저장할 파일 경로이다.
            string targetPath = GetTargetFilePath(_fileName, _httpContext); //실질적 파일 저장할 경로이다

            //Is it the first chunk? Prepare by deleting any existing files with the same name
            if (_firstChunk)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("First chunk arrived at webservice", AppSettingType.COMMON);                

                //Delete temp file  - 동일한 임시 파일이 존재하면 삭제한다.             
                if (File.Exists(tempPath))
                    File.Delete(tempPath);

                //Delete target file - 동일한 실질 파일이 존재하면 삭제한다.    
                if (File.Exists(targetPath))
                    File.Delete(targetPath);

            }
            

            //해당 임시파일을 저장한다.
            using (FileStream fs = File.Open(tempPath, FileMode.Append))
            {
                SaveFile(context.Request.InputStream, fs);
                fs.Close();
            }

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Write data to disk SUCCESS", AppSettingType.COMMON);
            

            //Is it the last chunk? Then finish up...
            if (_lastChunk)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Last chunk arrived", AppSettingType.COMMON);                
                
                //Rename file to original file
                //임시파일 경로를 실질파일경로로 이동한다. 따라서 임시파일명은 사라진다.
                File.Move(tempPath, targetPath);

                //Finish stuff....
                FinishedFileUpload(_fileName, _parameters);
            }
           
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.ToString());

            throw;
        }
        finally
        {
            //StopDebugListener();
        }

    }

    /// <summary>
    /// Get the querystring parameters
    /// </summary>
    private void GetQueryStringParameters()
    {
        _fileName = _httpContext.Request.QueryString["file"];//파일명(경로 없음)
        _parameters = _httpContext.Request.QueryString["param"];//인위적 파라미터
        _lastChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["last"]) ? true : bool.Parse(_httpContext.Request.QueryString["last"]);
        _firstChunk = string.IsNullOrEmpty(_httpContext.Request.QueryString["first"]) ? true : bool.Parse(_httpContext.Request.QueryString["first"]);
        _startByte = string.IsNullOrEmpty(_httpContext.Request.QueryString["offset"]) ? 0 : long.Parse(_httpContext.Request.QueryString["offset"]); ;

        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_fileName : " + _fileName, AppSettingType.COMMON);
        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_parameters : " + _parameters, AppSettingType.COMMON);        
    }

    /// <summary>
    /// Save the contents of the Stream to a file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="fs"></param>
    private void SaveFile(Stream stream, FileStream fs)
    {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            fs.Write(buffer, 0, bytesRead);
        }
    }

    /// <summary>
    /// Do your own stuff here when the file is finished uploading
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="parameters"></param>
    protected virtual void FinishedFileUpload(string fileName, string parameters)
    {
    }    
    

    protected string GetTempFilePath(string fileName, HttpContext context)
    {
        string uploadFilePath = "/DeskPlusEIP/FileData/" + _companyID + "/Users/" + _userID + "/Spool/" + _guid + "/";
        return context.Server.MapPath(uploadFilePath + fileName);
    }

    protected string GetTargetFilePath(string fileName, HttpContext context)
    {
        string uploadFilePath = "/DeskPlusEIP/FileData/" + _companyID + "/Users/" + _userID + "/Spool/" + _guid + "/";
        return context.Server.MapPath(uploadFilePath + fileName);
    }


    /// <summary>
    /// Write debug output to a textfile in debug mode
    /// </summary>
    [Conditional("DEBUG")]
    private void StartDebugListener()
    {
        try
        {
            _debugFileStreamWriter = System.IO.File.AppendText("debug.txt");
            _debugListener = new TextWriterTraceListener(_debugFileStreamWriter);
            Debug.Listeners.Add(_debugListener);
        }
        catch
        {
        }
    }

    /// <summary>
    /// Clean up the debug listener
    /// </summary>
    [Conditional("DEBUG")]
    private void StopDebugListener()
    {
        try
        {
            Debug.Flush();
            _debugFileStreamWriter.Close();
            Debug.Listeners.Remove(_debugListener);
        }
        catch
        {
        }
    }

    

    
    public bool IsReusable {
        get {
            return false;
        }
    }

}