<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Code.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("CodeManage")%></h1>
            <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("SysCodeManage")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("SysCodeName")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlParentCodeName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("SysCodeComment")%>
                            </th>
                            <td>
                                <asp:Literal ID="ltlParentCodeDescription" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div class="layout_subbox">
                    <h1><%=GetResource("CodeInfo")%></h1>
                </div>
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("CodeManage")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("CodeKey")%>
                            </th>
                            <td  colspan="3">
                                <span class="aLeft"><cc3:ClientSmartTextBox ID="ctbCodeID" runat="server" InputMode="None" Width="200px"  Required="true" class="searchtext form-control"></cc3:ClientSmartTextBox>&nbsp;</span>
                                <ul class="btn_smallL">
                                    <li><a href="#" id="CheckCodeID" class="btn btn-sm btn-secondary"><span><%=GetResource("CheckMultiValue")%></span></a></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CodeName")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbCodeName" runat="server" InputMode="None" CssStyle="width: 96% !important;"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                 <%=GetResource("MultiLang")%>&nbsp;<img src="<%=BaseImgPath%>/Icon/ico_add.gif" id="MultiName" alt="" />
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbMultiLang" runat="server" InputMode="None" CssStyle="width: 96% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Comments")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox ID="ctbDescription" runat="server" InputMode="None" CssStyle="width: 96% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("InterfaceCode")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbInterfaceCode" runat="server" InputMode="None" CssStyle="width: 96% !important;"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("Active")%>
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlActive" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            </div>
            </div>
        </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    </div>
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var SYSCodeID = "<%=SysCodeID %>";
        var CodeID = "<%=CodeID %>";
        var ParentCodeID = "<%=ParentCodeID %>";
        var CountCodeID = 0;
        var MultiLang = "<%=MultiLang %>";
        var CurrentPage = "<%=CurrentPage %>"; //검색키워드
        var RecordPerPage = "<%=RecordPerPage %>"; //검색키워드
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_ApplyUseCode = "<%=GetResource("ApplyUseCode") %>";
        var R_InputCode = "<%=GetResource("InputCode") %>";
        var R_InputCodeName = "<%=GetResource("InputCodeName") %>";
        var R_DuplicateCode = "<%=GetResource("DuplicateCode") %>";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Code/Add.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Code/Add.js");
    </script>
</body>
</html>
