<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTab.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Portlet.AddTab" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title></title>
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
                <h1>
                    <%=GetResource("PortletForTab")%></h1>
                <!-- 버튼 //-->                
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" class="btn btn-secondary" runat="server"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("PortletManagement")%></caption>
                    <colgroup>
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("PortletGroup")%>
                            </th>
                            <td>
                                <ul class="aLeft">
                                    <li><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("PortletName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" title="" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbPortletName" Required="true" MaxLength="100" runat="server" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox ID="ctbMultiLang" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("PortletCode")%></th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbPortletCode" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                               <%=GetResource("UIPath")%></th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbUIURL" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Enabled")%>
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlActive" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("PreviewImg")%>
                            </th>
                            <td>
                                <div class="aLeft" id="photoImg"><img style="width:120px;height:60px;" src="<%=BaseImgPath%>/Icon/rss.gif" alt="no image" /></div>

                                <ul class="btn_smallL">
                                    <li><cc3:ClientSmartButton ID="cbtnSwfFileAdd" runat="server"></cc3:ClientSmartButton></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Comments")%>
                            </th>
                            <td>
                                <div class="layout_scroll" style="min-height:120px;height:120px;">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbDescription" TextMode="MultiLine" Wrap="true" Rows="10" Columns="70" Style="height: 98%; width: 97%;" InputMode="None"></cc3:ClientSmartTextBox>
                                </div>                                
                            </td>
                        </tr>
                    </tbody>
                </table>
                <ul><li><%=GetResource("GoToAddPortlet")%></li></ul>
            </div>
            </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:TextBox ID="tbPhotoNEW" runat="server" Width="0"></asp:TextBox>
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var CategoryID = "<%=CategoryID %>";
        var MultiLang = "<%=MultiLang %>";
        var CurrentPage = "<%=CurrentPage %>"; //검색키워드
        var RecordPerPage = "<%=RecordPerPage %>"; //검색키워드
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort    
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var CountCode = 0;
        var PortletCodeCheck = false;
        var PortletID = 0;
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID
        var R_InputPortletCode = "<%=GetResource("InputPortletCode") %>";
        var R_InputPortletName = "<%=GetResource("InputPortletName") %>";
        var R_DuplicatePortletCode = "<%=GetResource("DuplicatePortletCode") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Admin/Portlet/AddTab.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Admin/Portlet/AddTab.js");
    </script>
</body>
</html>
