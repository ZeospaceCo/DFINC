<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.Dept.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>                
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnChangeParentDept" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnModUser" runat="server" BeginEndTag="li" Visible="false" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnDel" CssClass="btn_red" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->   
                        <table class="table_typeB">
                            <caption>
                                <%=GetResource("Admin_DeptManager_DeptManager")%></caption>
                            <colgroup>
                                <col width="80px" />
                                <col width="40%" />
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("UpperDept")%></th>
                                    <td><ul class="aLeft"><li><cc3:ClientSmartTextBox runat="server" ID="ctbParentName" Width="100px" InputMode="None"></cc3:ClientSmartTextBox></li></ul></td>
                                    <th><%=GetResource("ParentOption")%></th>
                                    <td>
                                        <ul class="chk_item">
                                            <li><asp:CheckBox ID="cbxRootDept" runat="server" /><%=GetResource("TopDept")%></li>
                                            <li style="display:none;"><asp:CheckBox ID="cbxRealDept" runat="server" /><%=GetResource("VirtualDept")%></li>
                                        </ul>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Admin_DeptManager_ltName")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbDeptName"  Required="true" Width="95%" InputMode="None"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("MultilingualName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbMuliLang" Width="95%" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Admin_DeptManager_ltShortName")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbShortName" Width="95%" InputMode="None"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("Admin_DeptManager_ltDeptCode")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbDeptCode" Width="150px" InputMode="None"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Admin_DeptManager_ltDescription")%></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox runat="server" ID="ctbDescription" Width="95%" InputMode="None"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Admin_DeptManager_ltAddress")%>&nbsp;<a id="cbtnPost"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" /></a></th>
                                    <td colspan="3">
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbZipCode" Width="50px" InputMode="None"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbAddress1" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox runat="server" ID="ctbAddress2" Width="40%" InputMode="None"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Private_AccountInfo_ltOfficePhone")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbPhone" Width="95%" InputMode="None"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("Admin_DeptManager_ltFax")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbFax" Width="200px" InputMode="None"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Admin_DeptManager_ltHomePage")%></th>
                                    <td><cc3:ClientSmartTextBox runat="server" ID="ctbHomepage" Width="200px" InputMode="None"></cc3:ClientSmartTextBox></td>
                                    <th><%=GetResource("ActivityDept")%></th>
                                    <td><asp:CheckBox ID="cbxActive" runat="server" Checked="true" /></td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="layout_subbox">
                            <h2><%=GetResource("CheckMember")%></h2>
                        </div>
                        <div id="layout_srh">
                            <ul class="btn_searchbox">
                                <li><div id="loading"></div></li>
                                <li><asp:DropDownList ID="ddlSearchGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                                <li>
                                    <input type="text" id="dummy" style="display: none;" />
                                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </div>
                        <div>&nbsp;</div>
                        
                        <!-- 테이블 시작 //-->                
                        <div id="placeholder" style="clear: both;"></div>
                        <!-- 테이블 끝 //-->
                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1">
                            <h3>
                                <span id="pageInfo"></span>
                            </h3>
                            <span id="pager"></span>
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                                <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
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
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";
        var DeptID = "<%=DeptID %>";
        var ParentDeptID = "<%=ParentDeptID %>";

        var SearchGubun = "<%=SearchGubun %>"; //검색구분자  
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var MultiLang = "<%=MultiLang %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PageUrl = "<%= Request.Url.ToString() %>";

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_LoginID = "<%=GetResource("LoginID") %>";
        var R_Admin_UserList_MSG_HTName = "<%=GetResource("Admin_UserList_MSG_HTName") %>";
        var R_MailAddress = "<%=GetResource("MailAddress") %>";
        var R_Private_AccountInfo_ltMobile = "<%=GetResource("Private_AccountInfo_ltMobile") %>";
        var R_Private_AccountInfo_ltUserDept = "<%=GetResource("Private_AccountInfo_ltUserDept") %>";
        var R_Private_AccountInfo_ltTitle = "<%=GetResource("Private_AccountInfo_ltTitle") %>";
        var R_Private_AccountInfo_ltDuty = "<%=GetResource("Private_AccountInfo_ltDuty") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_SpecialChar = "<%=GetResource("Common_revSpecialChar") %>";

        var R_RegisterDeptName = "<%=GetResource("RegisterDeptName")%>";
        var R_DeleteDept = "<%=GetResource("DeleteDept")%>";
        var R_CompleteOrder = "<%=GetResource("CompleteOrder")%>";
        var R_Admin_DeptManager_cbActive = "<%=GetResource("Admin_DeptManager_cbActive")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";        
    </script>
     <script type="text/javascript" src="../../Js/Admin/Dept/List.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Dept/List.js");
    </script>
</body>
</html>
