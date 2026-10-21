<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.OtherDept.Tree" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var TargetDeptID = "<%=TargetDeptID %>";

        var CabinetID = "";
        var FormID = "";        
        var YYYY = "";
        var YYYYMM = "";
        var Gubun = "";
        var Types = ""; //S:기본폴더; U : 수발자 관리자 추가 폴더
        var GroupID = "";

        var R_NoContent = "<%=GetResource("NoContent") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/OtherDept/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/OtherDept/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_left" class="left-menu admin">
        <ul class="left-menu-container">
            <li id="layout_treetop">
                <span class="treetop_title"><%=GetResource("VewOtherDeptDoc")%></span>
            </li>
            <li id="layout_treeboxAcc" style="height: 90%;">
            
                <table cellspacing="3" cellpadding="3" border="0" width="100%">
                <tr> 
          	        <td style="vertical-align:top;"> 
                        <div style="height:260px;width:240px;overflow:auto">
                        
                            <div id="placeholder" style="clear: both;"></div>
                        
                        </div>
                    </td>
                </tr>
                <tr>
                <td><div class="dotLine"></div></td>
                </tr>
                <tr> 
          	        <td style="vertical-align:top;"> 
                        <div id="divTree" style="height:270px;width:240px;overflow:auto">
                        <zeoAppFolderDeptTree:UCAppFolderDeptTreeScript ID="UCAppFolderDeptTreeScript1" SetCallbackComplete="onCallbackComplete" 
                        SetNodeSelect="onNodeSelect" UseCheckBox="false" runat="server" />
                        </div>
                    </td>
                </tr>
                </table>

            </li>
        </ul>
    </div>

    </form>
</body>
</html>
