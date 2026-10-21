<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddIP.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Admin.Content.AddIP" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RegIP")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnClose" BeginEndTag="li" runat="server"/>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            
            <table class="table_typeB">
            <tr>
                <td>
                    <ul class="center">
                        <li>
                            <input type="text" id="start1" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start2" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start3" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="start4" maxlength="3" style="width:40px !important;" />
                            &nbsp;~ &nbsp;
                            <input type="text" id="end1" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end2" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end3" maxlength="3" style="width:40px !important;" />.
                            <input type="text" id="end4" maxlength="3" style="width:40px !important;" />
                        </li>
                    </ul>
                </td>
            </tr>
            </table>

        </div>
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script> 
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>           
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputIP = "<%=GetResource("InputIP") %>";
        var R_InputIPNumber = "<%=GetResource("InputIPNumber") %>";
        var R_WrongIPRange = "<%=GetResource("WrongIPRange") %>";
        var R_ErrorAccessAuth = "<%=GetResource("ErrorAccessAuth") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Admin/Content/AddIP.js" ></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Admin/Content/AddIP.js");
    </script>
</body>
</html>
