<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDept.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Company.AddDept" %>
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
                <h1>부서정보</h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("Dept")%></caption>
                    <colgroup>
                        <col width="70px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                부서명
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbDeptName" Required="true" Width="95%"
                                    InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                약어
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbShortName" Width="95%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                연동코드
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbDeptCode" Width="95%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CompanyID %>";
        var DeptID = "<%= DeptID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputDeptName = "<%=GetResource("InputDeptName") %>";
        var R_SUCCESS = "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Company/AddDept.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Company/AddDept.js");
    </script>
</body>
</html>
