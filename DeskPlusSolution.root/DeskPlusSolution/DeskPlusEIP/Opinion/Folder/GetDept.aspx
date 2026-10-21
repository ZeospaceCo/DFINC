<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetDept.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Folder.GetDept" %>
<%@ Register TagPrefix="componentart" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
                <h1><%=GetResource("TitGetDept")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"></cc3:ClientSmartButton></li>
                </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="margin:5px 5px 10px 5px;padding:5px;outline:1px solid rgba(0,0,0,.1);">
             <div class="layout_scroll"  style="min-height:300px;height:300px;"> 
                  <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true" UseCheckBox="false" UseActiveDept="true" />
             </div>          
        </div>    
    </div>
</div><!-- //layout_info --> 

    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
        RqDeptID = "<%=DeptID%>";
        RqDeptNm = "<%=DeptNm%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Msg_Dept = "<%=GetResource("Msg_Dept")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Folder/GetDept.js"></script>     
</body>
</html>
