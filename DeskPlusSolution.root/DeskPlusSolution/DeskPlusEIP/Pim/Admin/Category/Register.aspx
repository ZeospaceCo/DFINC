<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.Category.Register" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1><%=GetResource("Category")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <table class="table_typeB">
                            <caption>
                            </caption>
                            <colgroup>
                                <col width="80px" />
                                <col width="250px" />
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                         <%=GetResource("CategoryName")%>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox ID="ctbName" runat="server" InputMode="None" Width="200"></cc3:ClientSmartTextBox>
                                    </td>
                                    <th>
                                        <%=GetResource("MultiName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="98%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("LayoutColor")%>
                                    </th>
                                    <td>
                                        <select name="ddlColor" id="ddlColor" runat="server" class="searchselect form-control"></select>
                                    </td>
                                    <th>
                                        <%=GetResource("InterfaceCode")%>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox ID="ctbStatusCode" runat="server" InputMode="None" Width="100"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%=GetResource("Active")%>
                                    </th>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlActive" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>    
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var StatusID = "<%=StatusID %>";
        var MultiLang = "<%=MultiLang %>";

        var R_MsgCategoryName = "<%=GetResource("MsgCategoryName")%>";
        var R_MsgError = "<%=GetResource("MsgError")%>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/Category/Register.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/Category/Register.js");
    </script>
</body>
</html>
