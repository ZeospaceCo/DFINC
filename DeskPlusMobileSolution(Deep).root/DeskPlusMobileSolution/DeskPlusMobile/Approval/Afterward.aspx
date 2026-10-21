<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Afterward.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.Afterward" %>
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
    <form id="form1" runat="server" onsubmit="return  Afterward.submitProcess();">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="ApprovalReturn">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("ReadLaterOn")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                            <div class="float-left">
                            <button type="button" class="btn btn-lg btn-danger" id="btCancel">
                                <span class="btnb">
                                    <%=GetResource("Cancel")%></span></button>
                            <button type="button" id="btnLaterOn" class="btn btn-lg btn-primary">
                                <span class="btnb"><%=GetResource("LaterOn")%></span>
                            </button>
                            <span>
                                (<span class="charsRemaining"></span>)
                            </span>
                            </div>
                        </div>
                    </li>
                    <li class="contentblock" style="display:none" id="Atarget">
                        <div class="title">
                            <%=GetResource("LaterOnTarget")%>
                        </div>
                        <div class="title">
                            <div id="aft" style="width: 100%">
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                         <div class="container-fluid">
                            <div class="float-left">
                                <asp:DropDownList ID="ddlReason" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                            </div>
                         </div>
                    </li>
                    <li class="contentblock" id="divpwd">
                        <div class="container-fluid">
                            <div class="float-left">
                                <div>
                                    <%=GetResource("Password")%>
                                </div>
                                <div>
                                    <input type="password" id="pass" maxlength="100" class="form-control"/>
                                </div>
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
        </div>
    </div>
    <uc2:Footer ID="Footer1" runat="server" />
    <iframe id="ifrmForm" name="ifrmForm" width="100" height="100" style="display: none" runat="server" src="about:blank"></iframe>
    <iframe id="ifrmProcessing" name="ifrmProcessing" width="100" height="100" style="display: none" src="about:blank"></iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">  
        var CustomFilePath = "<%=CustomFilePath%>";  
        var BPage = "<%=bPage%>";
        var PageNo = "<%=pageNo%>";
        var Stat = "<%=stat%>";
        var ATag = "<%=aTag%>";
        var AccTag = "<%=accTag%>";
        var UPwd = "<%=uPwd%>";
        var BtnClickCheck = "";    
        var Num2 = "<%=num2%>";                    
        var TargetUID = "<%=targetUID%>";
        var TargetDID = "<%=targetDID%>";
        var NextSeq = "<%=NextSeq %>";
        var NextTag = "<%=NextTag %>";          
        var SearchValue = "<%=searchValue%>";                   
        var DocumentID = "<%=dID%>", Sequence = "<%=seq%>";  
        var UserID = "<%=UserID%>";
        var IsLast = "N";                //최종결재여부 (Y/N)
        var IsZQ = "N";                  //반려가능수신(ZQ) 존재여부 (Y/N)   
        var ApprovalType = "L";
        var ApprovalTag = "<%=aTag%>";
        var AccountTag = "<%=accTag%>";      
        var CallComplete = false;    
        var GroupID = "<%=groupID%>";   
        var OrginUserID = "<%=OrginUserID%>";
        var OrginDeptID = "<%=OrginDeptID%>";

        var IsApprovePwd = "<%=IsApprovePwd %>";    //결재전용 비밀번호 사용 (N,Y)
        
        var R_Menu = "<%=GetResource("Menu")%>";        
        var R_ProcessingWait = "<%= GetResource("ProcessingWait") %>";
        var R_InputPwd = "<%= GetResource("InputPwd") %>";
        var R_NoPwd = "<%= GetResource("NoPwd") %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_ApprovalWait = "<%=GetResource("ApprovalWait") %>";
        var R_LogError = "<%=GetResource("LogError") %>";
        var R_Complete = "<%=GetResource("Complete") %>";
        var R_ReLoadDoc = "<%=GetResource("ReLoadDoc") %>";
        var R_ZA = "<%=GetResource("ZA") %>";
        var R_LimitShortReply = "<%=GetResource("LimitShortReply")%>"; 
        var R_InputOpinion = "<%=GetResource("InputOpinion")%>"; 
        var R_Msg_LaterOnSelItem = "<%=GetResource("Msg_LaterOnSelItem") %>";
        var R_ReadLaterOn = "<%=GetResource("ReadLaterOn") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/Afterward.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/Afterward.js");
    </script>
</body>
</html>
