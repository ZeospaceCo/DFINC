<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.MyTalk.Add" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <iframe name="hid_func" style="display: none"></iframe>
    <form id="form1" runat="server" name="form1" action="/DeskPlusMobile/Common/Upload/UploadCommon.ashx"
    method="POST" enctype="multipart/form-data" target="hid_func">
    <input type="text" name="companyID" value="<%=CompanyID%>" style="display: none" />
    <input type="text" name="userID" value="<%=UserID%>" style="display: none" />
    <input type="text" name="swfUploadGUID" value="<%=SwfUploadGUID%>" style="display: none" />
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="BoardView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-comments dropbtn "></i><%=GetResource("Comment")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left full-width">
                                <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                    <span class="btnb"><%=GetResource("Cancel")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-primary" id="btAdd">
                                    <span class="btnb"><%=GetResource("Save")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-success" id="btAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <button type="button" class="btn btn-lg btn-success" id="btPCAddFile" style="display:none;">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <input type="file" name="upload" id="upload" accept="image/*" onchange="fileChange(this);" style="display:none;" />
                                <span>
                                    (<span class="charsRemaining"></span>)
                                </span>
                                <div class="float-right">
                                    <input type="checkbox" id="cbOpen" class="rd" runat="server" /><%=GetResource("Open")%>
                                </div>                                
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" id="divFile" class="to5" style="display: none">
                        <div id="fileList" class="to6 dva">
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <textarea id="memo" class="form-control ta3" maxlength="200" runat="server"></textarea>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup full-width">
                            <button type="button" class="btn btn-outline-light full-width" id="before"><%=GetResource("List")%></button>
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
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.form.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">    
        var GroupID = "<%=groupID %>";
        var PageNo = "<%=pageNo%>";
        var RUserID = "<%=rUserID %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; 
        var MobileType = "<%=MobileType %>";
        var DraftListID =  "<%=draftListID %>";
        var FileCheck = "<%=FileCheck %>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var UploadCheck = "<%=uploadCheck%>";  
        var UpFileSize = "<%=upFileSize %>"; 

        var R_InputContent = "<%=GetResource("InputContent") %>";    
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_Menu = "<%=GetResource("Menu")%>";    
        var R_LimitShortReply = "<%=GetResource("LimitShortReply")%>"; 
        var R_WallPost = "<%=GetResource("WallPost")%>"; 
        var R_Comment = "<%=GetResource("Comment")%>"; 
        var R_MaxFile = "<%=GetResource("MaxFile") %>";
        var R_CheckFileType = "<%=GetResource("CheckFileType") %>";
        var R_SelCntFile = "<%=GetResource("SelCntFile") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/MyTalk/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/MyTalk/Add.js");
    </script>
</body>
</html>
