<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("AddDetail")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnConfirm" BeginEndTag="li" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnCancel" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
            </ul>
        </div>

        <div id="layout_content">
        <div class="row">
        <div class="col-12 content-area-body"><!-- width 100%(col-12) --> 
            <table class="table_typeB">        
            <caption></caption>
            <colgroup>
                <col width="80px"/>
                <col />
                <col width="80px"/>
                <col />
                <col width="100px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FamilyName") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFamilyName" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("FirstName") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbLastName" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("MiddleName") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbMiddleName" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Name") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbNickName" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("InternationalName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td ><cc3:ClientSmartTextBox ID="ctbEnglishName" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Email") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbMail" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("LunarGubun")%></th>
                    <td><asp:DropDownList ID="ddlLunar" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    <th><%=GetResource("BirthDay")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbBirthday" InputMode="None" MaxLength="250" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Sex")%></th>
                    <td><asp:DropDownList ID="ddlSex" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><%=GetResource("CellPhone") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbMobilePhone" placeholder="010-0000-0000" InputMode="None" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th rowspan="2"><%=GetResource("Picture")%></th>
                    <td colspan="3" rowspan="2">
                        <div class="aLeft" id="photoImg"><img width="52px" height="52px" src="<%=BaseImgPath%>/Icon/ico_noimg.gif" alt=<%=GetResource("UploadPicture")%> /></div>
                        <ul>
                            <li><cc3:ClientSmartButton ID="cbtnSwfFileAdd" runat="server" Class="btn btn-sm"></cc3:ClientSmartButton></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TelePhone") %></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbPhone" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Fax") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFax" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("WebPage") %></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbHomepage" InputMode="None" MaxLength="250" runat="server" Required="true"  width="98%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("Memo") %></th>
                    <td class="last-child" colspan="5"><cc3:ClientSmartTextBox ID="ctbMemo" TextMode="MultiLine" InputMode="None" MaxLength="250" runat="server" Required="true" width="98%"></cc3:ClientSmartTextBox></td>
                </tr>    
            </tbody>
            </table>

            <div class="layout_subbox">
                <h2><%=GetResource("Basics") %>(<%=GetResource("Address") %>)</h2>
            </div>
            <table class="table_typeB">
            <caption></caption>
            <colgroup>
                <col width="80px"/>
                <col />
                <col width="80px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("ZipCode") %>&nbsp;<a id="HomeZip"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbHomeZipCode" InputMode="None" MaxLength="7" runat="server" Required="true" CssStyle="width:20% !important;" placeholder="000-000"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Country") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbHomeCountry" InputMode="None" MaxLength="50" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("City") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbHomeCity" InputMode="None" MaxLength="50" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("State") %></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbHomeGu" InputMode="None" MaxLength="50" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("Address") %></th>
                    <td class="last-child" colspan="3"><cc3:ClientSmartTextBox ID="ctbHomeAddress" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>

            <div class="layout_subbox">
                <h2><%=GetResource("Company") %></h2>
            </div>
            <table class="table_typeB">
            <caption></caption>
            <colgroup>
                <col width="80px"/>
                <col />
                <col width="80px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("CompanyName")%>&nbsp;<a id="MultiCompany"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbCompany" InputMode="None" MaxLength="250" runat="server" Required="true"></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="ctbCompanyMulti" InputMode="None" MaxLength="250" runat="server" Required="true"></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("Office") %></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbOfficeAddress" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox>                        
                    </td>
                </tr>
                <tr>            
                    <th><%=GetResource("WebPage")%></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbCompanyHomePage" placeholder="http://www.example.com" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Dept") %>&nbsp;<a id="MultiDept"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbDepartment" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="ctbDepartmentMulti" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("Title") %>&nbsp;<a id="MultiTitle"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbTitle" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="ctbTitleMulti" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TelePhone") %></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbCompanyPhone" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("Fax")%></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbCompanyFax" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("IPTelePhone") %></th>
                    <td class="last-child"><cc3:ClientSmartTextBox ID="ctbIpPhone" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox></td>
                    <th class="last-child"><%=GetResource("Beeper") %></th>
                    <td class="last-child"><cc3:ClientSmartTextBox ID="ctbPager" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox></td>
                </tr>
            
            </tbody>
            </table>

            <div class="layout_subbox">
                <h2><%=GetResource("Company")%>(<%=GetResource("Address") %>)</h2>
            </div>
            <table class="table_typeB">
            <caption></caption>
            <colgroup>
                <col width="80px"/>
                <col />
                <col width="80px"/>
                <col />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("ZipCode") %>&nbsp;<a id="CompanyZip"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbCompanyZipCode" InputMode="None" MaxLength="250" runat="server" Required="true"  placeholder="000-000"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Country") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbCompanyCountry" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("City") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbCompanyCity" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("State") %></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbCompanyGu" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("Address")%></th>
                    <td class="last-child" colspan="3"><cc3:ClientSmartTextBox ID="ctbCompanyAddress" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>

            <div class="layout_subbox"></div>
        </div><!-- end col content-area-body-right -->                    
        </div><!-- end row  -->
        </div>
    </div>
</div>

<div style="display:none;">
    <asp:LinkButton ID="lbtnRegister" runat="server" OnClick="lbtnRegister_Click"></asp:LinkButton>
    <asp:TextBox ID="tbPhotoNEW" runat="server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbPhotoOLD" runat="server" Width="0"></asp:TextBox>
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
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>      
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var CatID = "<%=CatID %>";
        var CategoryID = "<%=CategoryID %>";
        var Types = "<%=Types %>";
        var RTypes = "<%=RTypes %>";        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //정렬
        var MultiLang = "<%=MultiLang %>";
        var SearchOption = "<%=SearchOption %>"; //검색옵션(하위검색)
        var SearchKey = "<%=SearchKey %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchFilter = "<%=SearchFilter %>"; //검색ㄱㄴㄷㄹ
        var R_InputFamilyName = "<%=GetResource("InputFamilyName") %>";
        var R_InputFitstName = "<%=GetResource("InputFitstName") %>";
        var R_InputEmail = "<%=GetResource("InputEmail") %>";
        var R_NotEmailFormat = "<%=GetResource("NotEmailFormat") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";
        var PhotoName = "<%=PhotoName %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var GoPage = "<%=GoPage%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Address/Js/Content/Add.js");
    </script>
</body>
</html>
