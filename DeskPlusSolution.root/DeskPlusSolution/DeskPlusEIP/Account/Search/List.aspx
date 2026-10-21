<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Search.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "0"; //게시물수
        var RecordPerPage = "10"; //보여줄게시물수
        var Order = "CodeTitleID"; //정렬      

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var CompanyID = "<%=CompanyID%>";
        var UserID = "<%=UserID%>";
        var DeptID = "<%=DeptID%>";
        var Types = "<%=Types%>";

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next")%>";
        var R_MyPhoto = "<%=GetResource("MyPhoto") %>";
        var R_Private_AccountInfo_ltUserName = "<%=GetResource("Private_AccountInfo_ltUserName") %>";
        var R_Admin_UserManager_GridHeader2 = "<%=GetResource("Admin_UserManager_GridHeader2") %>";
        var R_TitleDutyName = "<%=GetResource("TitleDutyName") %>";
        var R_MobilePhone = "<%=GetResource("MobilePhone") %>";
        var R_AllPhoneNumber = "<%=GetResource("AllPhoneNumber") %>";
        var R_Private_AccountInfo_ltMail = "<%=GetResource("Private_AccountInfo_ltMail") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Account/Js/Search/List.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%=SiteMap%></h1>
        </div>

        <div style="padding:10px">
            <table class="table_typeB">                
                <colgroup>
                    <col width="80px" />
                    <col />
                    <col width="80px" />
                    <col />
                    <col width="280px" />
                </colgroup>
                <tbody>
                    <tr>
                        <th><%=GetResource("Admin_UserList_MSG_HTName")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbName" MaxLength="250" runat="server" Width="95%"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("MobilePhone")%></th>
                        <td><cc3:ClientSmartTextBox ID="cbtMobile" MaxLength="500" runat="server" Width="95%" ></cc3:ClientSmartTextBox></td>
                        <th rowspan="3">	
                            <table class="table_typeH">
                            <tr>
                            <td>
                                <select id="Condition" class="searchselect form-control">
                                <option value="and"><%=GetResource("AndSearch")%></option>
                                <option value="or"><%=GetResource("OrSearch")%></option>
                                </select>
                            </td>
                            <td>
                                <cc3:ClientSmartButton ID="CbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton>
                            </td>                            
                            </tr>
                            </table>				
						</th>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_GridHeader2")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbDeptName" MaxLength="250" runat="server" Width="95%"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("OfficePhone")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbOfficePhone" MaxLength="100" runat="server" Width="95%"></cc3:ClientSmartTextBox></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Admin_UserManager_GridHeader3")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbTitle" MaxLength="250" runat="server" Width="95%"></cc3:ClientSmartTextBox></td>
                        <th><%=GetResource("Private_AccountInfo_ltMail")%></th>
                        <td><cc3:ClientSmartTextBox ID="ctbMail" MaxLength="250" runat="server" Width="95%"></cc3:ClientSmartTextBox></td>
                    </tr>   
                        <!-- tr><th class="last-child">집주소</th>
                        <td class="last-child" colspan="3"><input name="cbtHomeAddress" type="text" maxlength="500" id="cbtHomeAddress" required="False" class="text" title="" style="width:95%;"></td>
                    </tr -->
                </tbody>
            </table>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 -->            
                    <div id="placeholder" style="clear: both;"></div>            
                    <!-- 테이블 끝 -->
            
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->
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
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
