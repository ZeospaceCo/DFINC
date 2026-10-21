<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.ShareUser.GetUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="dummy" style="display: none;" />

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("PimConfirmPerson")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnClose"  runat="server"></cc3:ClientSmartButton></li>
            </ul>            
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server"/>
            </div>          
        </div>                
    </div>
</div><!-- //layout_info --> 
 <!-- ############# HIDDEN VALUE ############## -->
 <div style="display:none">
    <input type="hidden" id="tvSelectedValue" />
    <input type="hidden" id="tvSelectedID" />
 </div>
  <!-- ############# start of modal ############## -->
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";

        var RowMax = 0;        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/ShareUser/GetUser.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/ShareUser/GetUser.js");
    </script>
</body>
</html>
