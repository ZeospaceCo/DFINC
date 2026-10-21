<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseView.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.BaseView" %>

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
    <div data-role="page" id="ApprovalBaseView">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span>
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("BaseDocView")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock" id="menu" runat="server" style="display: none">
                         <div class="container-fluid buttongroup">
                            <div class="float-left">
                                <button type="button" id="btnOpinionView" class="btn btn-lg btn-primary" runat="server" data-toggle="modal" data-target="#modalopinion">
                                    <span class="btnb"><%=GetResource("Opinion")%></span>
                                </button>
                            </div>
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
                    <li>
                        <div style="width:100%; overflow:hidden">
                            <div id="contentHTML" style="padding-left:10px">
                            </div>
                        </div>
                    </li>
                    <!-- //docu view -->
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
        var BaseDID = "<%=baseDID%>";
        var CDID = "<%=cDID%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var MobileType = "<%=MobileType %>";
        var HttpType = "<%=httpType %>";
        var MobileDOMAIN = "<%=MobileDOMAIN %>";
        var R_NoDownload = "<%=GetResource("NoDownload") %>";
        var R_ReLoadDoc = "<%=GetResource("ReLoadDoc") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_ApprovalInfo = "<%=GetResource("ApprovalInfo")%>";
        var R_Attach = "<%=GetResource("Attach")%>";
        var R_BaseDoc = "<%=GetResource("BaseDoc")%>"
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/BaseView.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/BaseView.js");
    </script>
</body>
</html>
