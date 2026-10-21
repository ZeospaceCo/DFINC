<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.View" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=3.0, minimum-scale=1.0, user-scalable=yes, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.mintable.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/style.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/DeskPlusApprovalView.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loadingH">
    </div>
    <div data-role="page" id="ApprovalView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("DocView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup">
                          <div class="float-left">
                            <button type="button" id="btnApproveS" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("Approve")%></span>
                            </button>
                            <button type="button" id="btnApproveA" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("Arbitrary")%></span>
                            </button>
                            <button type="button" id="btnApproveH" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("Confirm")%></span>
                            </button>
                            <button type="button" id="btnApproveHR" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("NoConfirm")%></span>
                            </button>
                            <button type="button" id="btnReturn" class="btn btn-lg btn-danger" runat="server">
                                <span class="btnb"><%=GetResource("Return")%></span>
                            </button>
                            <button type="button" id="btnApproveSC" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("ApproveSC")%></span>
                            </button>
                            <button type="button" id="btnOpinionView" class="btn btn-lg btn-primary" runat="server" data-toggle="modal" data-target="#modalopinion" style="display: none">
                                <span class="btnb"><%=GetResource("Opinion")%></span>
                            </button>
                            <button type="button" id="btnOpinion" class="btn btn-lg btn-primary" runat="server">
                                <span class="btnb"><%=GetResource("AddOpinion")%></span>
                            </button>
                            <button type="button" id="btnApproveCancel" class="btn btn-lg btn-danger" runat="server">
                                <span class="btnb"><%=GetResource("ApproveSC")%></span>
                            </button>
                            <button type="button" id="btnDelayApp" class="btn btn-lg btn-danger" runat="server" style="display:none">
                                <span class="btnb"><%=GetResource("State3")%></span>
                            </button>
                            <button type="button" id="btnCancelDelayApp" class="btn btn-lg btn-danger" runat="server" style="display:none">
                                <span class="btnb"><%=GetResource("CancelDelayApp")%></span>
                            </button>
                            <button type="button" id="btnCancelApp" class="btn btn-lg btn-danger" runat="server" style="display:none">
                                <span class="btnb"><%=GetResource("CancelApp")%></span>
                            </button>                            
                            <button type="button" id="btnLaterOn" class="btn btn-lg btn-primary" runat="server" style="display:none">
                                <span class="btnb"><%=GetResource("ReadLaterOn")%></span>
                            </button>
                            <button type="button" id="btnEdit" class="btn btn-lg btn-primary" runat="server" style="display:none">
                                <span class="btnb"><%=GetResource("EditDoc")%></span>
                            </button>                       
                          </div>
                      </div>
                    </li>
                    <li class="contentblock withmycard">
        	            <div class="item">
                            <div class="imgbox appimgPhoto">
                                <%=PhotoImage%>
                            </div>
                        </div>
                        <div class="title">
                            <div id="subject" style="word-wrap:break-word;"></div>
            	            <div><span class="bg-secondary text-white" id="num3"></span><span id="sIsmobile"></span></div>
                            <div id="write"></div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title" id="seq">
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title" id="attach">

                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title" id="base">

                        </div>
                    </li>
                    <!-- //docu info -->
                    <!-- docu view -->
                    <li>
                        <div style="width:100%; overflow:hidden">
                            <div id="contentHTML" style="padding-left:10px">
                            </div>
                        </div>
                    </li>
                    <!-- //docu view -->
                    <li class="contentblock">
                      <nav aria-label="Page navigation example row">
                        <ul class="pagination left ">
                          <li class="page-item">
                            <a class="page-link" href="#" id="minus"><%=GetResource("BeforeDoc")%></a>
                          </li>
                          <li class="page-item">
                            <a class="page-link" href="#" id="plus"><%=GetResource("NextDoc")%></a>
                          </li>
                        </ul>
                        <ul class="pagination right ">
                          <li class="page-item">
                            <a class="page-link" href="#" id="before"><%=GetResource("List")%></a>
                          </li>
                        </ul>
                      </nav>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <!-- Modal -->
    <div id="modalopinion"  class="modal fade" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
		  <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
          </button>
          <h4 class="text-primary" style="margin-bottom: 0"><%=GetResource("Opinion")%></h4>
          </div>
          <div class="modal-body">
          </div>
          <!--div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary">Save changes</button>
          </div-->
        </div>
      </div>
    </div>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var BPage = "<%=bPage%>", DID = "<%=dID%>", Stat = "<%=stat%>", ATag = "<%=aTag%>";
        var PageNo = "<%=pageNo%>";
        var AccTag = "<%=accTag%>", Num2 = "<%=num2%>", Seq = "<%=seq%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var CDID = "<%=cDID%>";
        var TargetUID = "<%=targetUID%>";
        var TargetDID = "<%=targetDID%>";
        var GroupID = 0;
        var SealPath = "<%=SealPath%>";
        var SearchValue = "<%=searchValue%>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var BtnClickCheck = ""; 
        var DecisionTag = "";
        var IsLegacy = "";
        var CustomFileName = "";
        var LastTag = "";
        var NextTag = "";
        var NextSeq = "";

        var R_ApprovalInfo = "<%=GetResource("ApprovalInfo")%>";
        var R_Attach = "<%=GetResource("Attach")%>";
        var R_BaseDoc = "<%=GetResource("BaseDoc")%>"

        var R_Proxy1 = "<%=GetResource("Proxy1") %>";
        var R_NoApprove = "<%=GetResource("NoApprove") %>";
        var R_Arbitrary = "<%=GetResource("Arbitrary") %>";
        var R_Approve = "<%=GetResource("Approve") %>";
        var R_Return = "<%=GetResource("Return") %>";
        var R_ReadLaterOn = "<%=GetResource("ReadLaterOn") %>";
        var R_Confirm = "<%=GetResource("Confirm") %>";
        var R_NoConfirm = "<%=GetResource("NoConfirm") %>";
        var R_ApprovalTitle = "<%=GetResource("ApprovalTitle") %>";
        var R_ZA = "<%=GetResource("ZA") %>";
        var R_ZD = "<%=GetResource("ZD") %>";
        var R_ZH = "<%=GetResource("ZH") %>";
        var R_ZD0 = "<%=GetResource("ZD0") %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_ReLoadDoc = "<%=GetResource("ReLoadDoc") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_SignNo = "<%=GetResource("SignNo")%>"; 
        var R_ZR = "<%=GetResource("ZR") %>";
        var R_ConfirmZR= "<%=GetResource("ConfirmZR") %>";
        var R_NoList = "<%=GetResource("NoList") %>";
        var R_ProcessingWait = "<%= GetResource("ProcessingWait") %>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_ApprovalWait = "<%=GetResource("ApprovalWait") %>";
        var R_LogError = "<%=GetResource("LogError") %>";
        var R_Complete = "<%=GetResource("Complete") %>";
        var R_ConfirmCancle = "<%=GetResource("ConfirmCancle") %>";
        var R_CancelApprovalCheck = "<%=GetResource("CancelApprovalCheck") %>";
        var R_MSG_Processing = "<%=GetResource("MSG_Processing") %>";
        var R_MsgCancelApp = "<%=GetResource("MsgCancelApp") %>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/View.js");
    </script>
</body>
</html>
