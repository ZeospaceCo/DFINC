<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.License.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("AddLicense")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" class="btn btn-secondary" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:45px;">

            <table class="table_typeB">
			<caption></caption>
			<colgroup>
                <col width="100px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th>License Key</th>
                    <td><input type="text" id="tbLicenseKey" style="width:90%" /></td>
                </tr>
            </tbody>
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
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_ProcessingErrorLicenseAdd = "<%=GetResource("ProcessingErrorLicenseAdd") %>";    
    </script>
    <script src="../../Js/Admin/License/Add.js" type="text/javascript"></script>
</body>
</html>
