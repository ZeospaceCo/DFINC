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

            if (context.Request.Files.Count > 0)
            {
                string path = context.Server.MapPath("~/temp");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                var file = context.Request.Files[0];
                string filename = System.IO.Path.Combine(path, file.FileName);
                file.SaveAs(filename);

                context.Request.ContentType = "text/plain";
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var result = new { name = file.FileName };
                context.Response.Write(serializer.Serialize(result));
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