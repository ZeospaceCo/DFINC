<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFolder.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddFolder" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
<form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnAdd" class="btn btn-secondary" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnDel" class="btn btn-secondary" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("PortletGroupManage")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("ParentsGroupName")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlParentCategory" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("GroupName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbName" runat="server" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox ID="ctbMultiLang" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Comments")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbDesc" MaxLength="1000" runat="server" CssStyle="width:98% !important;"
                                    TextMode="MultiLine"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("IsUsed")%>
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlActiveCategory" runat="server" class="searchselect form-control">
                                </asp:DropDownList>
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
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
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
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var CategoryID = "<%=CategoryID %>";
        var Gubun = "<%=Gubun %>";
        var MultiLang = "<%=MultiLang %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //검색키워드
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort 
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_InputGroupName = "<%=GetResource("InputGroupName") %>";
        var R_ConfirmAdd = "<%=GetResource("ConfirmAdd") %>";
        var R_ConfirmMod = "<%=GetResource("ConfirmMod") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/AddFolder.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/AddFolder.js");
    </script>
</body>
</html>
