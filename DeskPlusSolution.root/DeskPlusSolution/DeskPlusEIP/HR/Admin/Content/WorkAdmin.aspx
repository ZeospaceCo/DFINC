<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Admin.Content.WorkAdmin" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css?v1=1" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("WorkAdmin")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_left">
                </ul>
                <ul class="btn_right">   
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>                 
                </ul>
            </div>
            <div id="layout_content">
                <!-- 기본근무시간 설정 시작 //-->
                <div class="layout_subbox" style="padding-top: 5px;">
                    <h2><%=GetResource("BaseWorkManage")%></h2>
                </div>
                <table class="table_typeB">
                <caption><%=GetResource("BaseWorkManage")%></caption>
                <colgroup>
                    <col width="160px" />
                    <col />
                    <col width="160px" />
                    <col />
			    </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("WorkYN")%></th>
                        <td><asp:DropDownList ID="ddlWorkYN" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th><%=GetResource("CecomYN")%></th>
                        <td><asp:DropDownList ID="ddlCecomYN" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("WSTime")%></th>
                        <td><asp:DropDownList ID="ddlStartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th><%=GetResource("WCTime")%></th>
                        <td><asp:DropDownList ID="ddlEndTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("WSTimeLimit")%></th>
                        <td colspan="3"><asp:DropDownList ID="ddlWSTimeLimit" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("WCTimeYN")%></th>
                        <td><asp:DropDownList ID="ddlWCTimeYN" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                        <th><%=GetResource("WCTimeExec")%></th>
                        <td><asp:DropDownList ID="ddlWCTimeExec" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                </tbody>
                </table>

                <!-- 유연근무 대상 설정 시작 //-->
                <div class="layout_subbox">
                    <h2 style="display:inline;"><%=GetResource("WorkTimeEtc")%></h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnUserAdd" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                <div>
                    <div id="phWorkEtc" style="clear:both;"></div>
                </div>
                <div class="app_subtxt"><%=GetResource("WorkAdminSubTxt1")%></div>
                <div class="app_subtxt"><%=GetResource("WorkAdminSubTxt2")%></div>

                <!-- IP 설정 -->
                <div class="layout_subbox">
                    <h2 style="display:inline;">
                        <%=GetResource("IPAccess")%></h2>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnADDIP" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>                        
                    </ul>
                </div>
                <div id="placeholder_IP" style="clear: both;"></div>
                <!-- IP 접근제어 -->
                <p>&nbsp;</p>
            </div>                    
        </div>
        <div style="display:none"> 
            <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/    
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";

        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>";
        var R_BaseWorkTimeError = "<%=GetResource("BaseWorkTimeError") %>";
        var R_Delete = "<%=GetResource("Msg_Del") %>";

        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName2") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_EtcWorkTime = "<%=GetResource("EtcWorkTime") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Gubun = "<%=GetResource("Gubun") %>";
        var R_IPBand = "<%=GetResource("IPBand") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_CecomReady = "<%=GetResource("CecomReady") %>";
        var R_WeekDay = "<%=GetResource("WeekDay") %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Admin/Content/WorkAdmin.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Admin/Content/WorkAdmin.js");
    </script>
</body>
</html>
