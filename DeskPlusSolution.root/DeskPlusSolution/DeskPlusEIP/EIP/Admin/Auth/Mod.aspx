<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Auth.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("LevelManage")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <div class="acc_auth_left">
                    <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px; overflow:hidden">
                        <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                    </div>
                </div>
                <div class="acc_auth_middle">
                    <ul>
                        <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton></li>
                    </ul>
                </div>
                <div class="acc_auth_right">
                    <!-- 테이블 시작 //-->
                    <div class="layout_scroll"  style="min-height:417px;height:417px;">   
                    <table cellspacing="0" id="contentTable" class="table_typeB"> 
                        <caption>
                            <%=GetResource("AuthList")%></caption>
                        <colgroup>                            
                            <col />
                            <col />
                            <col />
                            <col width="70px"/>
                        </colgroup>
                        <thead>
                            <tr>
                                <th class="center"><%=GetResource("UserName")%></th>
                                <th class="center"><%=GetResource("Section")%></th>
                                <th>&nbsp;</th>
                                <th class="center"><%=GetResource("Del")%></th>
                            </tr>
                        </thead>
                        <tbody>
                            <%=AuthTable%>
                        </tbody>
                    </table>
                        </div>
                    <!-- 테이블 끝 //-->
                </div>
                </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>               
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">      
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var RoleID = "<%=RoleID %>";
        var Gubun = "<%=Gubun %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var RowMax = <%=MaxRowMax %>;
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지        
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var R_SettingAuthority = "<%=GetResource("SettingAuthority") %>";
        var R_Users = "<%=GetResource("Users") %>";
        var R_Dept = "<%=GetResource("Dept") %>";
        var R_Apply = "<%=GetResource("Apply") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>     
    <script type="text/javascript" src="../../Js/Admin/Auth/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Auth/Mod.js");
    </script>
</body>
</html>
