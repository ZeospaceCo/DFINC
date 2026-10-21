<%@ Page Language="C#" AutoEventWireup="true" Codebehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Template.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<%@ Register TagPrefix="cc2" Namespace="Zeospace.DeskPlus.Component.Grids" Assembly="Zeospace.DeskPlus.Component.Grids" %>
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
            <h1><%=GetResource("Manage_Template")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary"  />
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg">
                <li >Page : <b><%=CurrentPageIndex%></b>/<%=pageCount %></li>
            </ul>

            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                        <div class="searchterm">                        
                            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="week"></asp:ListItem>
                                <asp:ListItem Value="month"></asp:ListItem>
                                <asp:ListItem Value="period"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="12" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" class="clsDate searchtext form-control" />
                        </div>
                    </li>
                <li><asp:DropDownList ID="ddlFieldSearch" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><cc3:ClientSmartTextBox runat="server" ID="tbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li>
                    <div id="plDate" style="display:none">
                    <a href="#" onclick="List.SelectDateCode()"><img src="<%=BaseImgPath%>/Icon/calendar.gif" alt="calendar" /></a>
                    </div>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnFindit" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>         
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
            
                    <div id="placeholder" style="clear: both;">
            
                    <cc2:MultiSelectGrid ID="MsgList" runat="server" PageSize="12" DataKeyField="TemplateID"
                        AutoGenerateColumns="False" AllowCustomPaging="True" Width="100%" BaseImgPath="<%#BaseImgPath%>"
                        ShowFooter="false" OnPageIndexChanged="MsgList_PageIndexChanged">
                        <Columns>
                            <asp:BoundColumn DataField="TemplateID" visible="False" />
                            <asp:TemplateColumn>
                                <itemstyle horizontalalign="Center" width="45px" verticalalign="Middle"></itemstyle>
                                <itemtemplate>
			                    <%# RecNo(DataBinder.Eval(Container.DataItem, "TemplateNum").ToString())%>
			                    </itemtemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
                                <itemtemplate>
                                <a onclick="AccountInfo('<%# Eval("WriteUserID").ToString() %>');return false;" style="cursor:pointer">
			                    <%# RecWriteUserName(GetLangName(
                                                Eval("NameBase").ToString(),
                                                Eval("NameKo").ToString(),
                                                Eval("NameEn").ToString(),
                                                Eval("NameJa").ToString(),
                                                Eval("NameFr").ToString(),
                                                Eval("NameZhcn").ToString(),
                                                Eval("NameZhtw").ToString(),
                                                PreLang)) %></a>
			                    </itemtemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <itemstyle horizontalalign="Left" width="" verticalalign="Middle"></itemstyle>
                                <itemtemplate>	
                                <a onclick="List.Read('<%# DataBinder.Eval(Container.DataItem, "TemplateID").ToString() %>')"><%# Eval("Subjects").ToString()%></a>						
			                    </itemtemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <itemstyle horizontalalign="Center" width="100px" verticalalign="Middle"></itemstyle>
                                <itemtemplate>
			                    <%# RecWriteDate(DataBinder.Eval(Container.DataItem,"WriteDate").ToString()) %>
			                    </itemtemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </cc2:MultiSelectGrid>            
                    </div>
            
                     <!-- 테이블 끝 //-->
                </div>
            </div>
        </div>
    </div>
    </div>   

    <div style="display: none">
    <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
    <asp:TextBox ID="tbSearchTermValue" runat="server" Width="0"></asp:TextBox>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/
    var List_DBInjection        = "<%=GetResource("List_DBInjection") %>";
    var List_Msg_Input          = "<%=GetResource("List_Msg_Input") %>";
    var List_Msg_NotUseSpecial  = "<%=GetResource("List_Msg_NotUseSpecial") %>";
    var projectID           = "<%=ProjectID %>";
    var menuID              = "<%=MenuID %>";  
    var menuType            = "<%=MenuType %>";      
    var SearchField = "<%=SearchField %>"; //검색키워드
    var SearchValue = "<%=SearchValue %>"; //검색어
    var SearchTerm = "<%=SearchTerm %>"; //날자 분류
    var SearchTermValue = "<%=SearchTermValue %>"; //날자값  
    var rqProjectID = "<%=RqProjectID %>";
    var projParam   = "<%=ProjParam %>"; 
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>"; //ja    

    var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
    var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Template/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Template/List.js");
    </script>
</body>
</html>
