<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Home.Menu" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Homesrc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Homesrc.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_appro.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<form id="form1" runat="server">
<div style="vertical-align:top;">

    <table class="Ta_tb"  cellpadding=0 cellspacing=0>
    <tr>
        <td class="Ta_tb_td1">
            <div id="su_area" class="left-menu">
                <!-- 결재정보요약 -->
                <ul id="su_aprinfo" class="left-menu-container">
                    <li class="in_way writebtn" style="min-height:55px;">                        
                        <div id="divProxyAction"><a id="btnProxyStop" class="btn btn-lg btn-primary btn-block" href="#"><%=GetResource("HomeProxyStop")%></a></div>
                        <div id="divProxyMenu"><a id="btnProxyMgr" class="btn btn-lg btn-primary btn-block" href="#"><%=GetResource("HomeProxyMgr")%></a></div>
                        <span id="spProxyUserInfo"></span>
                    </li>
                    <li class="left-sub-menu">
                        <ul class="in_list">
                            <li><a id="cntSM" href="#">0</a></li>
                            <li><a id="cntCC" href="#">0</a></li>
                            <li><a id="cntSC" href="#">0</a></li>
                            <li><a id="cntCP" href="#">0</a></li>
                            <br /><br />
                            <li><a id="PrivateFolder" href="#"></a></li> <!-- //개인별문서함 -->
                            <li><a id="ApprovalFolder" href="#"></a></li> <!-- //완결결재문서함 -->
                            <li><a id="RejectFolder" href="#"></a></li> <!-- //반려문서함 -->
                        </ul>
                    </li>

                    <!-- 자주사용하는 양식 -->
                    <li class="menu left-main-menu ui-state-active">자주사용하는 양식</li>
                    <li class="left-sub-menu  ">
                        <div id="divFavList" class="menusub">
                            <ul class="doc_list"></ul> 
                        </div>
                    </li>
                </ul>
            </div>
        </td>        
    </tr>    
    </table>
</div>
</form>
    <!-- ############# start of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang %>";
    var PreLangReal = "<%=PreLangReal %>"; //ja         

    var R_HomeProxy = "<%=GetResource("HomeProxy") %>";
    var R_HomeProxyNo = "<%=GetResource("HomeProxyNo") %>";
    var R_HomeProxyAlert = "<%=GetResource("HomeProxyAlert") %>";
    var R_HomeFavTit = "<%=GetResource("HomeFavTit") %>";
    var R_HomeFavTitMgr = "<%=GetResource("HomeFavTitMgr") %>";

	var HomeCntSM = "<%=GetResource("HomeCntSM")%>"
	var HomeCntCC = "<%=GetResource("HomeCntCC")%>"
	var HomeCntSC = "<%=GetResource("HomeCntSC")%>"
	var HomeCntCP = "<%=GetResource("HomeCntCP")%>"

    var HomePrivate = "<%=GetResource("HomePrivate")%>";
    var HomeApproval= "<%=GetResource("HomeApproval")%>";
    var HomeReject= "<%=GetResource("HomeReject")%>";
    var ApprovalStatusMenuUrl = "<%=ApprovalStatusMenuUrl%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Home/Menu.js?v=15"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Home/Menu.js");
    </script>
</body>
</html>
