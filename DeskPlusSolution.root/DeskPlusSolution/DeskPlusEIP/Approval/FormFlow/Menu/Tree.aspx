<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Menu.Tree" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
       
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var FormID = "<%=FormID %>";
        var PopupApp = "<%=PopupApp %>";
        var R_Msg_ErrSelForm = "<%=GetResource("Msg_ErrSelForm") %>";
        var R_Msg_ErrUserInfo = "<%=GetResource("Msg_ErrDefaultInfo") %>";
        var R_Msg_ErrSelUnit = "<%=GetResource("Msg_ErrSelUnit") %>"; 
        var R_Msg_ErrReCancel= "<%=GetResource("Msg_ErrReCancel") %>"; 
        var R_DeptSet = "<%=GetResource("DeptSet") %>"; 
        var R_MSG_SubDept = "<%=GetResource("MSG_SubDept") %>"; 
        var R_TitSub = "<%=GetResource("TitSub") %>"; 
        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Menu/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Menu/Tree.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Menu/Common.js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Menu/Tree.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_left" class="left-menu">
    <div class="left-menu-container">
        <!--div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("MakeDoc")%></span>
        </div-->

        <div id="layout_treeboxAcc">

            <div class="accordion">
                <div class="menu left-main-menu"><a href="#" onclick="" >1.<%=GetResource("Form2")%></a></div>
                <div class="menusub">
                    <zeoAppFormTree:UCAppFormTreeScript ID="UCFormTreeScript1" runat="server" SetNodeSelect="onFormNodeSelect" UseSetFolder="1,2" UseCheckBox="false" />
                </div>

                <div class="menu left-main-menu"><a href="#" onclick="" >2.<%=GetResource("SelApprover")%></a></div>
                <div class="menusub">
                    <input type="text" id="dummy" style="display: none;" />
                    <zeoAppDeptUserTree:UCAppDeptUserTreeScript id="UCDeptUserTreeScript1" runat="server" />
                </div>

                <div class="menu left-main-menu"><a href="#" onclick="" >3.<%=GetResource("ETC2")%></a></div>
                <div class="menusub" style="padding:20px;">
                    <table class="menu-table">
                    <colgroup>
                        <col width="60px;"/>
                        <col />
                    </colgroup>
                    <tbody>
                    <tr id="trAddtional" runat="server">
					    <th><%=GetResource("DeptInfo")%></th>
					    <td><asp:DropDownList ID="ddlAdditionDept" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
				    </tr>
                    <tr>
					    <th><%=GetResource("Form3")%></th>
					    <td><asp:DropDownList ID="ddlGuard" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
				    </tr>
                    <tr id="trCarbonView" runat="server">
                        <th><%=GetResource("CarbonView")%></th>
                        <td><asp:DropDownList ID="ddlCarbonView" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                    </tr>
                    <tr>
					    <th><%=GetResource("ViewAuth")%></th>
					    <td><asp:DropDownList ID="ddlAccessTitle" runat="server" CssClass="searchselect form-control"></asp:DropDownList><%=GetResource("More")%></td>
				    </tr>
                
                    </tbody>
			        </table>

                    <div style="height:10px;"></div>
                    <table style="font-size:11px;" class="table_typeE" id="gradeinfo">
                    <colgroup>
                        <col width="50px" />
                        <col />
                    </colgroup>
                    <tbody>
                    <%=AccessTitleGrade%>
                    </tbody>
                    </table>
                </div>

                <div class="menu left-main-menu"><a href="#" onclick="" >4.<%=GetResource("Opinion2")%></a> (<span class="charsRemaining"></span> <%=GetResource("InChar")%>)</div>
                <div class="menusub">
                    <asp:TextBox ID="tbOpinion" runat="server" TextMode="MultiLine" Width="93%" Height="96%" MaxLength="500"></asp:TextBox>
                </div>

                <!-- div class="menu"><a onclick="Tree.GoPage('Temporary')">5.<%=GetResource("TempFolder")%></a></div //-->

                <div class="menu left-main-menu"><a href="#" onclick="" >5.<%=GetResource("MenuPrivateFolder")%></a></div>
                <div class="menusub">
                    <zeoAppPrivate:UCAppPrivate ID="UCAppPrivate1" runat="server" SetNodeSelect="onPrivateSelect" UseSetFolder="1,2" />
                </div>
            </div>
            <div id="layout_treebuttom"></div>
        </div>
    </div>
</div>

<div style="display: none">
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
</div>
    </form>
</body>
</html>
