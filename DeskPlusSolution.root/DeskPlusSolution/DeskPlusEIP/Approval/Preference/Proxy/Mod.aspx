<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Proxy.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>
                <span><%=GetResource("ModProxy")%></span>                 
            </h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="layout_subbox">
                <h2><%=GetResource("SetProxy")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SetProxy")%></caption>
            <colgroup>
                <col width="110px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FavTitIsDept0")%></th>
                    <td><asp:Label ID="lbUserInfo" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><%=GetResource("AbsencePeriod")%></th>
                    <td>
                        <input type="text" id="startDate" size="12"  runat="server" class="text required" readonly="readonly"/>
                        <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        ~
                        <input type="text" id="endDate" size="12"  runat="server" class="text required" readonly="readonly" /> 
                        <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <input id="cbxAllDay" type="checkbox" name="cbxAllDay" runat="server" /><label for="cbxAllDay"><%=GetResource("AllDay")%></label>                    
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TypeAbsence")%></th>
                    <td><asp:DropDownList ID="ddlAbsenceType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><%=GetResource("ContentsAbsence")%></th>
                    <td><textarea id="AbsenceContent" name="AbsenceContent" runat="server"></textarea></td>
                </tr>
                <tr>
                    <th><%=GetResource("HomeProxy")%></th>
                    <td>
                        <asp:TextBox ID="tbxProxyDeptName" runat="Server" class="text required" style='width: 100px !important;' disabled="disabled"></asp:TextBox>
                        <asp:TextBox ID="tbxProxyUserName" runat="Server" class="text required" style='width: 100px !important;' disabled="disabled"></asp:TextBox>
                         <a href="#" onclick="Mod.GetUser()"><img src="<%=BaseImgPath%>/Icon/icon_search.gif" /></a>
                    </td>
                </tr>
            </tbody>
            </table>            
        </div>

    </div>
</div>


<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <asp:TextBox ID="tbxUserID" runat="Server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbxProxyDeptID" runat="Server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbxProxyUserID" runat="Server" Width="0"></asp:TextBox>
</div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript">
    
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";   

        var CurrentPage = "<%=CurrentPage%>";
        var RecordPerPage = "<%=RecordPerPage%>";
        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var TargetProxyID = "<%=TargetProxyID%>";

        var R_SelectProxyPerson = "<%=GetResource("SelectProxyPerson")%>"; //"대결자를 선택해 주세요."
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate")%>";      //"시작일자가 종료일자보다 작아야 합니다.";
        var R_ApplyAllRegDate = "<%=GetResource("ApplyAllRegDate") %>"; //"시작일자, 종료일자 둘다 입력해 주시기 바랍니다"
        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>"; //"종료시간은 시작시간과 같거나 작을 수 없습니다"
        var R_SelectProxyDiffer = "<%=GetResource("SelectProxyDiffer")%>"; //"동일인은 지정 불가합니다."
        var R_SelectTypeAbsence = "<%=GetResource("SelectTypeAbsence")%>"; //"부재유형을 선택하세요."
        var R_ApplyStartDate = "<%=GetResource("ApplyStartDate")%>"; //"시작일자를 입력해 주세요"

        var R_SelectProxyOverlap = "<%=GetResource("SelectProxyOverlap")%>"; //"선택한 사용자는 이미 대결자로 등록되어 있습니다."        
        var R_ApplyDateOverlap = "<%=GetResource("ApplyDateOverlap")%>"; //"선택한 날짜에 이미 대결자가 지정되어 있습니다."
        var R_ProxyConfirm1 = "<%=GetResource("ProxyConfirm1")%>"; //"등록된 대결자는 부재기간에 따라 대결진행, 대기, 종료됩니다.";
        var R_ProxyConfirm2 = "<%=GetResource("ProxyConfirm2")%>"; //"등록하시겠습니까?";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Proxy/Mod.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Proxy/Mod.js");
    </script>

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

</body>
</html>