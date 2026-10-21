<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DraftSend.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Mail.DraftSend" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" href="/DeskPlusMobile/Html/Js/jquery-ui.css" />
</head>
<body class="mobile navihide scrolldown">
    <iframe name="hid_func" style="display: none"></iframe>
    <form id="form1" runat="server" autocomplete="off" name="form1" action="/DeskPlusMobile/Common/Upload/UploadMail.ashx"
    method="POST" enctype="multipart/form-data" target="hid_func">
    <input type="text" name="companyID" value="<%=CompanyID%>" style="display: none" />
    <input type="text" name="userID" value="<%=UserID%>" style="display: none" />
    <input type="text" name="swfUploadGUID" value="<%=SwfUploadGUID%>" style="display: none" />
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="MailFolder">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-envelope dropbtn "></i><%=GetResource("MailSend")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btSend">
                                    <span class="btnb"><%=GetResource("Send")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-success" id="btPCAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <input type="file" name="upload" id="upload" onchange="fileChange(this);" style="display:none;" />
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="divFile" style="display: none">
                        <div class="title">
                            <div id="fileList" class="to6 dva">
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Receiver")%>
                                </div>
                                <div class="area">
                                    <textarea rows="1" class="form-control" id="tbTo"></textarea>
                                    <div class="float-left" id="toMenu" runat="server">
                                        <button type="button" class="btn btn-xs btn-primary" id="btTo">
                                        <span class="btnb"><%=GetResource("AddressStr")%></span>
                                    </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"></div>
                                <div class="area">
                                    <span class="rdwrp fst">
                                        <input type="checkbox" id="cbToMe" name="share1" class="rd" /><label for=""><%=GetResource("ToMe")%></label></span>
                                    <span class="rdwrp">
                                        <input type="checkbox" id="cbViewCC" name="share1" class="rd" /><label for=""><%=GetResource("Refer1")%></label></span>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="cc" name="cc" style="display: none;">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Refer")%>
                                </div>
                                <div class="area">
                                    <textarea rows="1" class="form-control ta2" id="tbCc"></textarea>
                                    <div class="float-left" id="ccMenu" runat="server">
                                        <button type="button" class="btn btn-xs btn-primary" id="btCc">
                                        <span class="btnb"><%=GetResource("AddressStr")%></span>
                                    </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Refer2")%>
                                </div>
                                <div class="area">
                                    <textarea rows="1" class="form-control" id="tbBcc"></textarea>
                                    <div class="float-left" id="bccMenu" runat="server">
                                        <button type="button" class="btn btn-xs btn-primary" id="btBcc">
                                        <span class="btnb"><%=GetResource("AddressStr")%></span>
                                    </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable">
                                <div class="title"><%=GetResource("Subject")%></div>
                                <div class="area">
                                    <input type="text" id="tbSubject" class="form-control text" />
                                </div>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock2 bg-white">
                        <div class="title">
                            <div class="rc" id="contentList"></div>
                        </div>
                    </li>
                    <li class="contentblock" id="att_line" style="display: none; padding:0px 8px;">
                        <div class="title">
                            <span class="istM">
                                <%=GetResource("Attach")%></span>
                            <br />
                            <div id="attachList" style="width: 100%">
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.autogrow-textarea.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery-ui.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.form.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var Mail_sn = "<%=mail_sn%>";
        var Code = "<%=code%>";
        var Action = "<%=action%>";
        var Mail = "<%=mail%>";
        var MailID = "<%=mailID%>";
        var Hearder = "<%=hearder%>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>";    
        var AttachList = "";
        var AttachNameList = "";
        var AttachRealNameList = "";
        var MailUserID = "<%=MailUserID%>";
        var MailAddress = "<%=MailAddress%>";
        var BtnClickCheck = "";  
        var PageNo = "<%=pageNo%>";
        var History = "<%=history%>";
        var Gubun = "<%=gubun%>";
        var OwnerID = "<%=ownerID%>";
        var SearchValue = "<%=searchValue%>";
        var MSearchValue = "<%=mSearchValue%>";
        var UseMailApprove = "<%=UseMailApprove%>";
        var FileCheck = "<%=FileCheck %>";
        var BlockExt = "<%=blockExt %>"; 
        var UpFileSize = "<%=upFileSize %>"; 

        var R_Subject = "<%=GetResource("Subject") %>";
        var R_Sender = "<%=GetResource("Sender") %>";
        var R_Receiver = "<%=GetResource("Receiver") %>";
        var R_Refer = "<%=GetResource("Refer") %>";
        var R_SendDate = "<%=GetResource("SendDate") %>"; 
        var R_ErrorDraftMail = "<%=GetResource("ErrorDraftMail") %>"; 
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>"; 
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>"; 
        var R_InputReceiver = "<%=GetResource("InputReceiver") %>"; 
        var R_InputReceiverMail = "<%=GetResource("InputReceiverMail") %>"; 
        var R_InputReferMail = "<%=GetResource("InputReferMail") %>"; 
        var R_InputRefer2Mail = "<%=GetResource("InputRefer2Mail") %>"; 
        var R_InputSubject = "<%=GetResource("InputSubject") %>"; 
        var R_SaveMail = "<%=GetResource("SaveMail") %>";     
        var R_AskAdmin = "<%=GetResource("AskAdmin") %>";     
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_MailSendUser = "<%=GetResource("MailSendUser")%>";    
        var R_MailError1 = "<%=GetResource("MailError1")%>";  
        var R_MailError2 = "<%=GetResource("MailError2")%>"; 
        var R_MailError3 = "<%=GetResource("MailError3")%>";   
        var R_MaxFile = "<%=GetResource("MaxFile") %>";
        var R_CheckFileType = "<%=GetResource("CheckFileType") %>";
              
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Mail/DraftSend.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Mail/DraftSend.js");
    </script>
</body>
</html>