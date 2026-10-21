<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCommon.aspx.cs" Inherits="Zeospace.DeskPlus.Address.Web.Content.ViewCommon" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Address.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Address.css"/>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("Basics") %></h1>
            <ul class="btn_right">                
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
                    <td><cc3:ClientSmartTextBox ID="ctbLastName" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("MiddleName") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbMiddleName" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Name") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbNickName" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th>다국이름</th>
                    <td ><cc3:ClientSmartTextBox ID="ctbEnglishName" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Email") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbMail" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("LunarGubun")%></th>
                    <td><asp:DropDownList ID="ddlLunar" runat="server" Class="form-control"></asp:DropDownList></td>
                    <th><%=GetResource("BirthDay")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbBirthday" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Sex")%></th>
                    <td><asp:DropDownList ID="ddlSex" runat="server" Class="form-control"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><%=GetResource("Picture")%></th>
                    <td colspan="5">                                     
                        <div id="photoImg"><img width="52px" height="52px" src="<%=BaseImgPath%>/Icon/ico_noimg.gif" alt=<%=GetResource("UploadPicture")%> /></div>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("Memo") %></th>
                    <td colspan="5"><cc3:ClientSmartTextBox ID="ctbMemo" TextMode="MultiLine" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>        
                <tr>
                    <th><%=GetResource("WebPage") %></th>
                    <td colspan="5"><cc3:ClientSmartTextBox ID="ctbHomepage" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("TelePhone") %></th>
                    <td colspan="2">
                        <cc3:ClientSmartTextBox ID="ctbPhone" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("CellPhone") %></th>
                    <td colspan="2">
                        <cc3:ClientSmartTextBox ID="ctbMobilePhone" placeholder="010-0000-0000" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("Fax") %></th>
                    <td class="last-child" colspan="5"><cc3:ClientSmartTextBox ID="ctbFax" placeholder="02-0000-0000" InputMode="None" runat="server" Required="true" ></cc3:ClientSmartTextBox></td>
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
                    <th><%=GetResource("ZipCode") %></th>
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
                    <th class="last-child"><%=GetResource("DetailAddress") %></th>
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
                    <th><%=GetResource("CompanyName")%></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbCompany" InputMode="None" MaxLength="250" runat="server" Required="true"></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="ctbCompanyMulti" InputMode="None" MaxLength="250" runat="server" Required="true"></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("Office") %></th>
                    <td><cc3:ClientSmartTextBox ID="ctbOfficeAddress" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>            
                    <th><%=GetResource("WebPage")%></th>
                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbCompanyHomePage" placeholder="http://www.example.com" InputMode="None" MaxLength="250" runat="server" Required="true"  width="95%"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Dept") %></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbDepartment" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                        <cc3:ClientSmartTextBox ID="ctbDepartmentMulti" InputMode="None" MaxLength="250" runat="server" Required="true" ></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("Title") %></th>
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
                    <th><%=GetResource("ZipCode") %></th>
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
                    <th class="last-child"><%=GetResource("DetailAddress")%></th>
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
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.placeholder.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var CatID = "<%=CatID %>";
        var CategoryID = "<%=CategoryID %>";
        var Types = "<%=Types %>";
        var RTypes = "<%=RTypes %>";
        var SearchFilter = "<%=SearchFilter %>"; //검색키워드
        var SearchOption = "<%=SearchOption %>"; //검색 옵션
        var SearchKey = "<%=SearchKey %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //정렬
        var R_InputFamilyName = "<%=GetResource("InputFamilyName") %>";
        var R_InputFitstName = "<%=GetResource("InputFitstName") %>";
        var R_InputEmail = "<%=GetResource("InputEmail") %>";
        var R_NotEmailFormat = "<%=GetResource("NotEmailFormat") %>";             
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";
        var PhotoName = "<%=PhotoName %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var GoPage = "<%= GoPage %>"
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Address/Js/Content/ViewCommon.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Address/Js/Content/ViewCommon.js");
    </script>
</body>
</html>

