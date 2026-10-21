<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocOpinion.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.DocOpinion" %>

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
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server" onsubmit="return  AddOpinion.Add();">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="ApprovalApprove">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <!-- content -->
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("AddOpinion")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" id="btCancel" class="btn btn-lg btn-danger">
                                    <span class="btnb">
                                        <%=GetResource("Cancel")%></span></button>
                                <button type="button" id="btAdd" class="btn btn-lg btn-primary">
                                    <span class="btnb">
                                        <%=GetResource("Save")%></span></button>
                                <span>
                                    (<span class="charsRemaining"></span>)
                                </span>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid">
                            <textarea id="memo" class="form-control ta3" maxlength="200"></textarea>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup full-width">
                            <button type="button" class="btn btn-outline-light full-width" id="before"><%=GetResource("List")%></button>
                        </div>
                    </li>
                </ul>
            </div>
            <!-- //content -->
        </div>
    </div>
    <uc2:Footer ID="Footer1" runat="server" />
    <iframe id="ifrmForm" name="ifrmForm" width="100" height="100" style="display: none"
        runat="server" src="about:blank"></iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">    
        var BPage = "<%=bPage%>";
        var PageNo = "<%=pageNo%>";
        var AccTag = "<%=accTag%>";
        var BtnClickCheck = ""; 
        var GroupID = "<%=groupID%>";    
        var CompanyID = "<%=CompanyID%>";
        var SearchValue = "<%=searchValue%>";         
        var DocumentID = "<%=dID%>";            
        var UserID = "<%=UserID%>";  
        var TargetDeptID = "<%=targetDeptID%>";
        var TargetTitleID = "<%=targetTitleID%>";
        var KeyID = "<%=keyID %>";
        var Key2ID = "<%=key2ID %>";   
        var R_ProcessingWait = "<%= GetResource("ProcessingWait") %>";      
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>"; 
        var R_ApprovalWait = "<%=GetResource("ApprovalWait") %>";       
        var R_Complete = "<%=GetResource("Complete") %>";
        var R_InputOpinion = "<%=GetResource("InputOpinion") %>";
        var R_ReLoadDoc = "<%=GetResource("ReLoadDoc") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_LimitShortReply = "<%=GetResource("LimitShortReply")%>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/DocOpinion.js?v=3"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/DocOpinion.js");
    </script>
</body>
</html>
