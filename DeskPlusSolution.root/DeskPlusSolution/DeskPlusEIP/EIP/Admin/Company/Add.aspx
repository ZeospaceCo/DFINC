<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Company.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div id="loading"></div>
        <div class="layout_one">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>
                    <%=GetResource("CompanyInfo")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnMod" runat="server" class="btn btn-secondary" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("CompanyInfo")%></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                        <col width="120px" />
                        <col />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th>
                                <%=GetResource("CompanyName")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyName" Required="true" Width="98%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("MultiNameCompany")%>&nbsp;<a id="MultiNameCompany"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"  alt="" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="98%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Incorporation")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEstablishDate" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("InterfaceCode")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInterfaceCode" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CEO")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInstitutorName" Width="98%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("MultiNameCEO")%>&nbsp;<a id="MultiNameInst"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInstitutorEngName" Width="98%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CompanyHomepage")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbHomePage" Width="98%" InputMode="None"
                                    class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Zipcode")%>&nbsp;<a id="cbtnPost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbZipCode" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbAddress1" Width="40%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbAddress2" Width="40%" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("AddressEng")%>&nbsp;
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEngZipCode" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEnglishAddress" Width="40%" InputMode="None"  class="searchtext form-control"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEnglishAddress2" Width="40%" InputMode="None"   class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CompanyPhone")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbPhone" Width="150px" InputMode="None"   class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("CompanyFax")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbFax" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Enabled")%>
                            </th>
                            <td>
                                <asp:CheckBox ID="cbxActive" runat="server" Checked="true" />
                            </td>
                            <th>
                                <%=GetResource("Usage")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyQuota" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>(GB)
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("LicenseCount")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbLicenseCnt" Width="150px" InputMode="None" class="searchtext form-control"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr id="trDefaultValue">
                            <th>
                                <%=GetResource("DefaultValue")%>
                            </th>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlCompany" runat="server" Width="200px" class="searchselect  form-control"></asp:DropDownList>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div id="companyUser_Info">
                    <!-- 회사담당자정보 시작 //-->
                    <div class="layout_subbox">
                        <h1>
                            <%=GetResource("CompanyPersonInfo")%></h1>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnComUserAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                            <cc3:ClientSmartButton ID="cbtnComUserDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                        </ul>
                    </div>
                    <div id="placeholderUserList" style="clear: both;"></div>
                    <!-- 회사담당자정보 끝 //-->
                </div>
            </div>
            </div>
            </div>
        </div>
    </div>
    <iframe id="ifrmSLO" name="ifrmSLO" src="about:blank" width="0" height="0"></iframe>
    <div style="display: none;">
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CompanyID %>";
        var CurrentPage = "<%=CurrentPage %>";
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>";
        var RoleAdmin = "<%=RoleAdmin %>";
        var MultiLang = "<%=MultiLang %>";
        var UseLicense = false;
        var R_CompanyName = "<%=GetResource("CompanyName") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_UserName = "<%=GetResource("UserName") %>";
        var R_Login = "<%=GetResource("Login") %>";
        var R_CompanyPhone = "<%=GetResource("CompanyPhone") %>";
        var R_Handphone = "<%=GetResource("Handphone") %>";
        var R_Email = "<%=GetResource("Email") %>";
        var R_InputCompanyName = "<%=GetResource("InputCompanyName") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_ConfirmDeleteCompanyPersonInfo = "<%=GetResource("ConfirmDeleteCompanyPersonInfo") %>";
        var R_CheckDeleteCompanyPersonInfo = "<%=GetResource("CheckDeleteCompanyPersonInfo") %>";
        var R_SearchRequestSelect = "<%=GetResource("SearchRequestSelect") %>";
        var R_SUCCESS =  "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError =  "<%=GetResource("ProcessingError") %>";
        var R_LicenseCount=  "<%=GetResource("LicenseCount") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ExcessLicenseCnt = "<%=GetResource("ExcessLicenseCnt") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="../../Js/Admin/Company/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("../../Js/Admin/Company/Add.js");
    </script>
</body>
</html>
