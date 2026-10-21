<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSign.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.GetSign" %>
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
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var R_MSG_GETSIGN = "<%=GetResource("MSG_GETSIGN") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/GetSign.js"></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Common/Popup/GetSign.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <h1><%=GetResource("Signature")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnClear" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:220px;">
                <center>
                <div id="drawingSignImage" style="border:solid 1px black;height:210px;width:210px">
                <img id="SignImage" alt="SignImage" src="SignImage.ashx" style="height:200px;width:200px" draggable="false" />
                </div>
                </center>
            </div>                
        </div>
    </div>
    <div id="pnTest" style="height:300px; overflow:auto;display:none;"></div>
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