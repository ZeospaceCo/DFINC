<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrivateMod.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.Popup.PrivateMod" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">                    
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
                <!-- 출퇴근시간 설정 시작 //-->
                <table class="table_typeB">
                <colgroup>
                    <col width="60px" />
                    <col width="130px" />
                    <col width="50px" />
                    <col width="60px" />
                    <col width="80px" />
                    <col />
			    </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("WSTime")%></th>
                        <td class="center">
                            <asp:DropDownList ID="ddlWSTimeHour" runat="server" CssClass="searchselect form-control"></asp:DropDownList>:
                            <asp:DropDownList ID="ddlWSTimeMinute" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </td>
                        <th><%=GetResource("WSConn")%></th>
                        <td class="center">
                            <asp:DropDownList ID="ddlWSConn" runat="server" CssClass="searchselect form-control"></asp:DropDownList>                            
                        </td>       
                        <th><%=GetResource("LateRemark")%></th>
                         <td class="center">
                            <textarea id="txtLateRemark" runat="server" style="width:95%; height: 95%; text-align:left" rows="1" cols="1"></textarea>
                        </td>                 
                    </tr>
                    <tr>
                        <th><%=GetResource("WCTime")%></th>
                        <td class="center">
                            <asp:DropDownList ID="ddlWCTimeHour" runat="server" CssClass="searchselect form-control"></asp:DropDownList>:
                            <asp:DropDownList ID="ddlWCTimeMinute" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </td>
                        <th><%=GetResource("WSConn")%></th>
                        <td class="center">
                            <asp:DropDownList ID="ddlWCConn" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </td>
                        <th><%=GetResource("EarlyRemark")%></th>
                        <td class="center">
                            <textarea id="txtEarlyRemark" runat="server" style="width:95%; height: 95%; text-align:left" rows="1" cols="1"></textarea>
                        </td>
                    </tr>
                </tbody>
                </table>
                <p>&nbsp;</p>  
                <!-- 수정사유 시작 //-->
                <div class="layout_subbox" style="padding-top: 5px;">
                    <h2><%=GetResource("ModReason")%></h2>
                </div>
                <textarea id="txtModReason" style="width:98%; height: 50px; text-align:left" rows="1" cols="1"></textarea>
            </div>
        </div>
    </div>
    
    </form>
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
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var AttID = "<%=AttID%>";
        var UserID = "<%=UserID%>";
        var TargetUserID = "<%=TargetUserID%>";
        var TargetDeptID = "<%=TargetDeptID%>";
        var PathID = "<%=PathID%>";
        var WorkDate = "<%=WorkDate%>";
        var IsHoli = "<%=IsHoli%>";
        
        var bWSTimeHour = "<%=bWSTimeHour%>";
        var bWSTimeMinute = "<%=bWSTimeMinute%>";
        var bWCTimeHour = "<%=bWCTimeHour%>";
        var bWCTimeMinute = "<%=bWCTimeMinute%>";


        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_LateReason = "<%=GetResource("LateReason") %>"; // 지각사유를 입력하세요.
        var R_EarlyReason = "<%=GetResource("EarlyReason") %>"; // 조퇴사유를 입력하세요.
        var R_ModReasonInput = "<%=GetResource("ModReasonInput") %>"; // 수정사유를 입력하세요.
        var R_SelTime = "<%=GetResource("SelTime") %>"; //시간을 선택하세요.
        var R_SelWSTime = "<%=GetResource("SelWSTime") %>"; //출근시간을 선택하세요.
        var R_SelWCTime = "<%=GetResource("SelWCTime") %>"; //퇴근시간을 선택하세요.
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";     
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/Popup/PrivateMod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Content/Popup/PrivateMod.js");
    </script>
</body>
</html>
