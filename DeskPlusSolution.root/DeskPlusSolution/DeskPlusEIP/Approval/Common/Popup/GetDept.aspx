<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetDept.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.GetDept" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var DeptID = "<%=DeptID%>";
        var DeptNm = "<%=DeptNm%>";
        var Callback = "<%=Callback %>";
        var IsCheckBox = "<%=IsCheckBox %>";
        var IsActive = "<%=IsActive %>";
        var IsSub= "<%=IsSub %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_Msg_Dept = "<%=GetResource("Msg_Dept")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/GetDept.js"></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Common/Popup/GetDept.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><%=GetResource("SelTarget")%></h1>
                <div id="divsub" style="display:inline-block;" class="cap1">
                    <input id="cbOptionSubFolder" type="checkbox" name="cbOptionSubFolder" runat="server" /><label for="cbOptionSubFolder"><%=GetResource("MSG_ApplySubFolder")%></label>&nbsp;
                </div>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:170px;">
                    <table class="table_typeB" style="margin-bottom: 1px">
			        <caption></caption>
			        <colgroup>
                        <col width="80px;" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th><%=GetResource("Target")%></th>
                            <td><span id="selData" class="app_subtxt"></span></td>
                        </tr>
                    </tbody>
                    </table>
                <div class="layout_scroll"  style="min-height:400px;height:400px;"> 
                    <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" />
                </div>
            </div>                
        </div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <asp:TextBox ID="tvSelectedID" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvSelectedValue" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
        <asp:TextBox ID="tvTypes" runat="server" Width="0" Height="0" Style="display: none"></asp:TextBox>
    </div>
    <!-- ==================================================================================== -->
</form>
</body>
</html>
