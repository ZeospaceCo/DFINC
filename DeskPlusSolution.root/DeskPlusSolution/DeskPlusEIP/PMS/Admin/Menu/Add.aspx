<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Menu.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
<form id="form1" runat="server">
   <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("Menu_admin3") %></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>
            </ul>
        </div>


        <div id="layout_content">
            <div class="pms_menu_layout">  
            
                <!-- 메뉴 //-->
                <div class="left" style="padding-top:10px;">   
                    <!-- 메뉴내용 //-->
                    <div style="height:200px !important;" class="mid">
                        <ul class="PMSMenu">
                            <asp:Repeater ID="rpMenuList" runat="server">
                            <ItemTemplate>
                                <li id="MenuItem_<%#DataBinder.Eval(Container.DataItem,"MenuID").ToString()%>">
                                <h2><a href="#" onclick="Add.SelectItem('<%#DataBinder.Eval(Container.DataItem,"MenuID").ToString()%>')">
                                <%# GetLangName(
                                                Eval("MenuBase").ToString(),
                                                Eval("MenuKo").ToString(),
                                                Eval("MenuEn").ToString(),
                                                Eval("MenuJa").ToString(),
                                                Eval("MenuFr").ToString(),
                                                Eval("MenuZhcn").ToString(),
                                                Eval("MenuZhtw").ToString(),
                                                PreLang)%></a></h2></li>
                            </ItemTemplate>
                            </asp:Repeater>
                         </ul>
                    </div> 
                    <!-- 메뉴버튼 //-->
                    <div class="bott">
                        <ul class="btnimg">
                            <li><a onclick="Add.Move('up');" ><img src="<%=BaseImgPath %>/icon/tree_up.gif" /></a></li>
                            <li><a onclick="Add.Move('down');" ><img src="<%=BaseImgPath %>/icon/tree_down.gif" /></a></li>
                        </ul>                        
                    </div>
                </div>

                <!-- 메뉴상세설정 //-->
                <div class="cont" style="padding-top:10px;width:80% !important;">

	
                   <table class="table_typeE">
                    <caption></caption>
                    <colgroup>
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tbody>
                    <tr>
                        <th><%=GetResource("Menu_Header1")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th>
                        <td><asp:TextBox ID="tbMenuName" runat="Server" Width="100px"></asp:TextBox>
                            <cc3:ClientSmartTextBox ID="CTBMultiName" runat="server" CssStyle="width:200px !important;" TitleAtt=""></cc3:ClientSmartTextBox>
                        </td>          
                    </tr>
                    <tr>
                        <th><%=GetResource("Menu_Header3")%></th>
                        <td><asp:CheckBox ID="cbIsUse" runat="server"/><%=GetResource("Menu_Header3")%></td>
                    </tr>
                    <tr>
                        <th><%=GetResource("Menu_Header2")%></th>
                        <td><asp:TextBox ID="tbMenuDesc" runat="server" TextMode="MultiLine" Rows="5" Width="97%" Height="60px"></asp:TextBox></td>
                    </tr> 
                    </tbody>
                    </table>
                  </div>
            </div>
        </div>
    </div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
    <asp:HiddenField ID="hdMenuID" runat="server" />
    <asp:HiddenField ID="hdMenuType" runat="server" />
    <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbtnMod" runat="server" OnClick="lbtnMod_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbtnDel" runat="server" OnClick="lbtnDel_Click"></asp:LinkButton>
        
    </div>
    <!-- ==================================================================================== -->
 
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>     
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CompanyID.ToString() %>";
        var ProjectID = "<%=ProjectID.ToString() %>";
        var PreLang = "<%=PreLang %>";
        var MultiLang = "<%=MultiLang %>";
        var MSG_NOSELECT_MENU = "<%=GetResource("MSG_NOSELECT_MENU") %>";
        var MSG_CONFIRM_DEL = "<%=GetResource("MSG_CONFIRM_DEL") %>";
        var Menu_Register_MSGName = "<%=GetResource("Menu_Register_MSGName") %>";
        var Menu_Register_MSGDesc = "<%=GetResource("Menu_Register_MSGDesc") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Menu/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Menu/Add.js");
    </script>
</body>
</html>
