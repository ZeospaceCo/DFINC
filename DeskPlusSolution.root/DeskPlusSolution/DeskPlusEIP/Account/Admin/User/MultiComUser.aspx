<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultiComUser.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.Admin.User.MultiComUser" %>
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
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />    
</head>
<body>
    <form id="form1" runat="server">


    <div id="layout_body">
        <div class="layout_one">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1>타회사인사관리</h1>
                <!-- 버튼 //-->
                <ul class="btn_right">    
                    <span style="float:left"><asp:DropDownList ID="ddlMultiCompany" runat="server" CssClass="searchselect form-control"></asp:DropDownList>&nbsp;</span>

                    <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />            
                </ul>
            </div>

            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"> <!-- width 100%(col-12) -->  
                        <div class="acc_auth_left">
	                        <div class="layout_scroll"  style="width:270px;min-height:400px;height:400px;overflow:hidden">
                                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
                            </div>
                        </div>
                        <div class="acc_auth_middle">
                            <ul><li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li></ul>
                        </div>
                        <div class="acc_auth_right">                                
                            <table cellspacing="0" id="contentTable" class="table_typeT" style="width: 100% !important;">
                                <caption><%=GetResource("AuthList")%></caption>
                                <colgroup>
                                    <col />
                                    <col width="100px" />
                                    <col width="80px" />
                                </colgroup>
                                <thead>
                                <tr>
                                    <th class="center">부서</th>
                                    <th class="center">이름</th>
                                    <th class="center">삭제</th>
                                </tr>
                                </thead>
                                <tbody>
                                <%=UserDeptTable%>
                                <%--<a href="javascript:DelAuth('','')"><img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/Btn/folderset_del.gif" /></a>--%>
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
        var CompanyID = "<%= CompanyID %>";
        var MultiCompanyID = "<%= MultiCompanyID %>";
        var UserID = "<%=UserID %>";
        var GroupGUID = "<%=GroupGUID %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";        
        var RowMax = 0;

        var R_SettingAuthority = "<%=GetResource("SettingDeptAuthority")%>";
        var R_Both = "<%=GetResource("Both")%>";
        var R_Basic = "<%=GetResource("Basic")%>";
        var R_SettingBothDept = "<%=GetResource("SettingBothDept")%>";
        var R_SettingBaseDeptOnlyone = "<%=GetResource("SettingBaseDeptOnlyone") %>";
        var R_Admin_UserManager_MSG_SUCCESS = "<%=GetResource("Admin_UserManager_MSG_SUCCESS")%>";
        var R_ProcessingError = "<%=GetResource("ProcessingError")%>";
        var R_Admin_CompanyManager_btnDel = "<%=GetResource("Admin_CompanyManager_btnDel") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/Admin/User/MultiComUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Account/Js/Admin/User/MultiComUser.js");
    </script>
</body>
</html>
