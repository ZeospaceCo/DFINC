<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.General.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("ChangeManager")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">                
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>                
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <table class="table_typeB">
			        <caption><%=GetResource("ChangeManager")%></caption>
			        <colgroup>
                        <col width="200px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
					        <th><%=GetResource("NowManager")%></th>
					        <td colspan="2">
                                <label id="lbManager"></label>
                            </td>
				        </tr>
                        <tr>
                            <th><%=GetResource("ChangeUser")%></th>
                            <td><cc3:ClientSmartTextBox runat="server" ID="ctbUserInfo" Width="98%" InputMode="None" Required="true" class="searchtext"></cc3:ClientSmartTextBox></td>
                            <td>
                                <ul class="btn_smallL" style="padding-left:5px;">
                                    <li><cc3:ClientSmartButton ID="cbtnMember" runat="server" /></li>
                                </ul>
                            </td>
                        </tr>                
			        </tbody>
		            </table> 
                    <div class="cm_gap10"></div>
                    <span class="cm_subtxt">* <%=GetResource("CMNScript1")%></span><br />
                    <span class="cm_subtxt">* <%=GetResource("CMNScript4")%></span><br />
                </div>
             </div>
        </div>        
    </div>
</div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CommunityID = "<%=CommunityID %>";
        var UserID = 0;
        var AdminCheck = "<%=AdminCheck %>";
        var R_ConfirmManager = "<%=GetResource("ConfirmManager") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_CheckManager = "<%=GetResource("CheckManager") %>";
        var R_ConfirmChange = "<%=GetResource("ConfirmChange") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Admin/General/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/General/Mod.js");
    </script>
</body>
</html>
