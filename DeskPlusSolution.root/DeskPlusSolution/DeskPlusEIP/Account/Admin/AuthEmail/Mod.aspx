<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.AuthEmail.Mod" %>
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
</head>
<body>
    <form id="form1" runat="server">

    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("PasswordManager") %></h1>
                <div class="menusub">
                    <ul class="menu_line">
                        <li><h2><%=GetResource("ManagerGuide") %></h2></li>
                    </ul>
                </div>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnSubmit" Class="btn btn-secondary" runat="server" />
                </ul>
            </div>

            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->  
                        <div class="acc_auth_left">
                            <div class="layout_scroll"  style="width:270px;min-height:400px;height:400px;overflow:hidden">
                                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseIcon="true" UseCheckBox="true" UseActiveDept="false" />
                            </div>
                        </div>
                        <div class="acc_auth_middle">
                            <ul>
                                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </div>
                        <div class="acc_auth_right">                                
                            <table cellspacing="0" id="contentTable" class="table_typeT" style="width: 100% !important;">
                                <caption><%=GetResource("AuthList")%></caption>
                                <colgroup>
                                    <col />
                                    <col width="80px" />
                                </colgroup>
                                <thead>
                                <tr>
                                    <th class="center"><%=GetResource("Admin_UserList_MSG_HTName")%></th>
                                    <th class="center"><%=GetResource("Admin_UserList_cbsUserDel")%></th>
                                </tr>                        
                                </thead>
                                <tbody>
                                <%=AuthTable%>                        
                                </tbody>
                            </table>                    
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div style="display: none">        
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
        <asp:TextBox ID="tbAuthID" runat="server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>        
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var NowManagerID = "<%=NowManagerID %>";
        var RowMax = <%=MaxRowMax %>;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        
        var R_NotCheckUser = "<%=GetResource("NotCheckUser") %>"; //사용자가 선택되지 않았습니다.
        var R_ManagerOnlyOne = "<%=GetResource("ManagerOnlyOne") %>"; //한명만 등록 가능합니다.
        var R_ManagerNotDept = "<%=GetResource("ManagerNotDept") %>"; //관리자는 부서등록 할 수 없음
        var R_Admin_CompanyManager_btnDel = "<%=GetResource("Admin_CompanyManager_btnDel") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/Admin/AuthEmail/Mod.js"></script>
</body>
</html>
