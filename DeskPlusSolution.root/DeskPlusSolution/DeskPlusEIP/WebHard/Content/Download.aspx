<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.Download" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
</head>
<body style="padding:1px;">
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("AllDownload")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary" ScriptFunctionName="TotalDownLoad.DownLoad()" /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server" ScriptFunctionName="self.close()" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:300px;">
            

            <table cellspacing="0" class="layer_popupA"> 
            <tr style="display:none;"> 
                <td>    
                <cc3:ClientSmartTextBox ID="CTBFolder" runat="server" InputMode="None" ReadOnly="true" Width="99%"></cc3:ClientSmartTextBox> 
                </td> 
            </tr> 
            <tr>
                <td>
                <div id="activexDPUpload" style="height:245px"><script src="/DeskPlusEIP/Html/Js/IEActiveX/activexDPUploadTotalDownLoad.js"></script></div>
                </td>
            </tr>
            <tr>
                <td  class="outline" style="width:530px !important;overflow:auto;word-break:break-all;" valign="top">
                <div id="divFileList" runat="server"></div>
                </td>
            </tr>
		    </table> 

            </div>
        </div>
    </div>

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var FileList = "<%=FileList%>";   
        var Order = "<%=Order %>"; //정렬       
        var AttachFileName = "";
        var AttachFilePath = "";
        var UploadHost = "http://<%=Request.ServerVariables["SERVER_NAME"].ToString()%>";        //업로드 대상 호스트
        var UploadPort = "<%=Request.ServerVariables["SERVER_PORT"].ToString()%>";         // 업로드 포트
        var R_UploadDesktop = "<%=GetResource("UploadDesktop")%>";                                               // 바탕 화면
        var R_UploadDocuments = "<%=GetResource("UploadDocuments")%>";                                          // 내 문서
        var R_UploadComputer = "<%=GetResource("UploadComputer")%>";                                         // 내 컴퓨터
        var R_UploadNetworkPlaces = "<%=GetResource("UploadNetworkPlaces")%>";                       // Spool 폴더
        var R_DelFile = "<%=GetResource("Del")%>";                          //파일 삭제 
        var R_NotFoundFile = "<%=GetResource("NotFoundFile")%>";                                       //업로드 메시지
        var R_NotFoundPath = "<%=GetResource("NotFoundPath")%>";                               //업로드 메시지
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/Download.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Content/Download.js");
    </script>
</body>
</html>
