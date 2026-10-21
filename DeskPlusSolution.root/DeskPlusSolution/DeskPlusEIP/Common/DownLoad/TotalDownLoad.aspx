<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TotalDownLoad.aspx.cs" Inherits="Zeospace.DeskPlus.Common.Web.DownLoad.TotalDownLoad" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc1" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Common.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Common.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" type="text/javascript"></script>
    <script type="text/javascript">
        var UploadHost = "http://<%=Request.ServerVariables["SERVER_NAME"].ToString()%>";        //업로드 대상 호스트
        var UploadPort = "<%=Request.ServerVariables["SERVER_PORT"].ToString()%>";         // 업로드 포트        
        var MsgUploadAttachFileDesktop = "<%=GetResource("MsgUploadAttachFileDesktop")%>";                                               // 바탕 화면
        var MsgUploadAttachFileMyDocuments = "<%=GetResource("MsgUploadAttachFileMyDocuments")%>";                                          // 내 문서
        var MsgUploadAttachFileMyComputer = "<%=GetResource("MsgUploadAttachFileMyComputer")%>";                                         // 내 컴퓨터
        var MsgUploadAttachFileMyNetworkPlaces = "<%=GetResource("MsgUploadAttachFileMyNetworkPlaces")%>";                       // Spool 폴더
        var FileList = "<%=FileList%>";                                                                                 //폴더 ID
        var MsgDelFile = "삭제";                              //파일 삭제 
        var MsgNotFoundFile = "<%=GetResource("MsgNotFoundFile")%>";                                       //업로드 메시지
        var MsgNotFoundPath = "<%=GetResource("MsgNotFoundPath")%>";                               //업로드 메시지
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Common/Js/DownLoad/TotalDownLoad.js"></script>   
</head>
<body style="padding:2px;">
    <form id="Form1" runat="server">    
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("AllDownLoad")%></h1>
                <ul class="btn_right">
                    <li><cc1:ClientSmartButton ID="CbtnSubmit" runat="server" Class="btn btn-secondary" /></li>
                    <li><cc1:ClientSmartButton ID="CbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:320px;height:320px;overflow:hidden !important;">
            

            <table cellspacing="0"> 
            <tr style="display:none;"> 
                <td>    
                <cc1:ClientSmartTextBox ID="CTBFolder" runat="server" InputMode="None" ReadOnly Width="99%"></cc1:ClientSmartTextBox> 
                </td> 
            </tr> 
            <tr>
                <td>
                <div id="activexDPUpload" style="height:245px;"><script src="/DeskPlusEIP/Html/Js/IEActiveX/activexDPUploadTotalDownLoad.js"></script></div>
                </td>
            </tr>
            <tr>
                <td  class="outline" style="width:547px !important" valign="top">
                <div id="divFileList"></div>
                </td>
            </tr>
		    </table> 

            </div>
        </div>
    </div>
    </form>
</body>
</html>
