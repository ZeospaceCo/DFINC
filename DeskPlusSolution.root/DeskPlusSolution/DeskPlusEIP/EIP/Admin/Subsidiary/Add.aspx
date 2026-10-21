<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Subsidiary.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">    
    <div id="layout_body" class="write">
        <div class="layout_full" style="padding-right:20px">

            <div id="layout_content">

            <div class="layout_subbox">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("CompanyInfo")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnMod" runat="server" class="btn btn-secondary" BeginEndTag="li" />
                </ul>
            </div>
                <table class="table_typeB">
                    <caption>
                        <%=GetResource("CompanyInfo")%></caption>
                    <colgroup>
                        <col width="120px" />
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
                                <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyName" Width="98%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("MultiNameCompany")%>&nbsp;<a id="MultiNameCompany"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="98%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Incorporation")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEstablishDate" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("InterfaceCode")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInterfaceCode" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CEO")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInstitutorName" Width="98%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("MultiNameCEO")%>&nbsp;<a id="MultiNameInst"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbInstitutorEngName" Width="98%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CompanyHomepage")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbHomePage" Width="98%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Zipcode")%>&nbsp;<a id="cbtnPost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbZipCode" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbAddress1" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbAddress2" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("AddressEng")%>&nbsp;
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEngZipCode" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEnglishAddress" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbEnglishAddress2" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("CompanyPhone")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbPhone" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                            <th>
                                <%=GetResource("CompanyFax")%>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbFax" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
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
                                <cc3:ClientSmartTextBox runat="server" ID="ctbCompanyQuota" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("LicenseCount")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbLicenseCnt" Width="150px" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Slogan")%>
                            </th>
                            <td colspan="3">
                                <cc3:ClientSmartTextBox runat="server" ID="ctbSlogan" TextMode="MultiLine" Wrap="true" Rows="10" Columns="70" Style="height: 98%; width: 100%;" InputMode="None"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <ul>
                                    <li><%=GetResource("LogInImage")%></li>
                                    <li>png(175*35px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="loginAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="loginDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>    
                            </th>
                            <td class="aimageBG">
                                <div id="loginImg" ><img src="<%=LoginImage%>" height="35px" alt="Login Image" /></div>
                            </td>
                            <th>
                                <ul>
                                    <li><%=GetResource("MenuImage")%></li>
                                    <li>png(135*25px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="menuAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="menuDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>   
                            </th>
                            <td class="aimageBG">
                                <div id="menuImg"><img src="<%=MenuImage%>" height="25px" alt="Menu Image" /></div>   
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <ul>
                                    <li><%=GetResource("VLogin")%><br /><%=GetResource("LogInImage")%></li>
                                    <li>png(20*140px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="VloginAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="VloginDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>    
                            </th>
                            <td class="aimageBG">
                                <div id="VloginImg" ><img src="<%=VLoginImage%>" height="140px" alt="Login Image" /></div>
                            </td>
                            <th>
                                <ul>
                                    <li><%=GetResource("SuperAdmin")%><br /><%=GetResource("MenuImage")%></li>
                                    <li>gif(160*28px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="AmenuAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="AmenuDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>   
                            </th>
                            <td class="aimageBG">
                                <div id="AmenuImg"><img src="<%=AMenuImage%>" height="28px" alt="Menu Image" /></div>   
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <ul>
                                    <li><%=GetResource("BGLogin")%></li>
                                    <li>jpg(1370*770px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="BGloginAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="BGloginDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>    
                            </th>
                            <td class="aimageBG">
                                <div id="BGloginImg"><img src="<%=BGloginImg%>" height="140px" alt="Login Image" /></div>
                            </td>
                            <th>
                                <ul>
                                    <li><%=GetResource("BGPotal")%></li>
                                    <li>jpg(1370*770px)</li>
                                    <li>
                                        <ul class="btn_smallL">
                                            <li><a href="#" id="BGPotalAdd" class="btn btn-sm"><%=GetResource("Add")%></a></li>
                                            <li><a href="#" id="BGPotalDel" class="btn btn-sm"><%=GetResource("Del")%></a></li>
                                        </ul>
                                    </li>
                                </ul>   
                            </th>
                            <td class="aimageBG">
                                <div id="BGPortalImg"><img src="<%=BGPortalImg%>" height="140px" alt="Potal Image" /></div>   
                            </td>
                        </tr>
                    </tbody>
                </table>

                <!-- 회사담당자정보 시작 //-->
                <div class="layout_subbox">
                    <h1>
                        <%=GetResource("CompanyPersonInfo")%></h1>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnComUserAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="cbtnComUserDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                    </ul>
                </div>
                <div id="placeholderUserList" style="clear: both;"></div>
                <!-- 회사담당자정보 끝 //-->
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>"; ;
        var CurrentPage = "<%=CurrentPage %>";
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
        var R_ConfirmDeleteCompanyPersonInfo = "<%=GetResource("ConfirmDeleteCompanyPersonInfo") %>";
        var R_CheckDeleteCompanyPersonInfo = "<%=GetResource("CheckDeleteCompanyPersonInfo") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_SUCCESS =  "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError =  "<%=GetResource("ProcessingError") %>";
        var R_LicenseCount=  "<%=GetResource("LicenseCount") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ExcessLicenseCnt = "<%=GetResource("ExcessLicenseCnt") %>";
        var AttachGubun = "";        
        var UploadGUID = "<%=UploadGUID %>"; //flash upload GUID           
        var LoginFileType = "*.png";
        var MenuFileType = "*.gif";
        var BGFileType = "*.jpg";
        var FileTypeDesc = "Images ";
    </script>
    <script type="text/javascript" src="../../Js/Admin/Subsidiary/Add.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Subsidiary/Add.js");
    </script>
</body>
</html>
