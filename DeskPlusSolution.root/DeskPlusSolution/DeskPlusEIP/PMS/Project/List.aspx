<%@ Page Language="C#" AutoEventWireup="true" Codebehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Project.List" %>
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
            <h1><%=GetResource("Project_State")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnFolder" runat="server" BeginEndTag="li" />

                <cc3:ClientSmartButton ID="cbtnWrite" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg">
                <li >Page : <b><%=CurrentPageIndex%></b>/<%=pageCount %></li>
            </ul>

            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm" style="display:none">
                        <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="week"></asp:ListItem>
                            <asp:ListItem Value="month"></asp:ListItem>
                            <asp:ListItem Value="period"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li><asp:DropDownList ID="ddlFieldSearch" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><input type="text" id="dummy" style="display: none;" />
                <cc3:ClientSmartTextBox runat="server" ID="tbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnFindit" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>         
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <!-- 테이블 시작 //-->            
                    <div id="placeholder" style="clear: both;">            
                            <cc2:MultiSelectGrid ID="MSGProjectList" runat="server" PageSize="12" DataKeyField="ProjectID" AllowMultiSelect="false"
                                AutoGenerateColumns="False" AllowCustomPaging="True" Width="100%" BaseImgPath="<%#BaseImgPath%>" 
                                ShowFooter="false" OnPageIndexChanged="MSGProjectList_PageIndexChanged">
                                <Columns>
                                    <asp:BoundColumn DataField="ProjectID" visible="False" />
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Left" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>                                
						                &nbsp;&nbsp; <%# RecUrl(DataBinder.Eval(Container.DataItem, "ProjectID").ToString(), 
                                           GetLangName(
                                                Eval("SubjectBase").ToString(),
                                                Eval("SubjectKo").ToString(),
                                                Eval("SubjectEn").ToString(),
                                                Eval("SubjectJa").ToString(),
                                                Eval("SubjectFr").ToString(),
                                                Eval("SubjectZhcn").ToString(),
                                                Eval("SubjectZhtw").ToString(),
                                                PreLang),
                                                                                                 DataBinder.Eval(Container.DataItem, "Status").ToString(),
                                        DataBinder.Eval(Container.DataItem, "WriteUserID").ToString())%>
				            	        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>

						                <%# RecDeptName(
                                        GetLangName(
                                                Eval("DeptBase").ToString(),
                                                Eval("DeptKo").ToString(),
                                                Eval("DeptEn").ToString(),
                                                Eval("DeptJa").ToString(),
                                                Eval("DeptFr").ToString(),
                                                Eval("DeptZhcn").ToString(),
                                                Eval("DeptZhtw").ToString(),
                                                PreLang)
                                        ) %>
				        	        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>
						                <a onclick="AccountInfo('<%# Eval("WriteUserID").ToString() %>');return false;" style="cursor:pointer"><%# RecName(GetLangName(
                                                Eval("NameBase").ToString(),
                                                Eval("NameKo").ToString(),
                                                Eval("NameEn").ToString(),
                                                Eval("NameJa").ToString(),
                                                Eval("NameFr").ToString(),
                                                Eval("NameZhcn").ToString(),
                                                Eval("NameZhtw").ToString(),
                                                PreLang))%></a>
				        	        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>						
						                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"PlanStartDate")).ToShortDateString() %>
			        		        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>
						                <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"PlanEndDate")).ToShortDateString() %>
				        	        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>
						                <%# RecStatus(DataBinder.Eval(Container.DataItem,"Status").ToString()) %>
			        		        </itemtemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <itemstyle horizontalalign="Center" verticalalign="Middle"></itemstyle>
                                        <itemtemplate>
						                <%# DataBinder.Eval(Container.DataItem, "Process").ToString()%> %
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
        <asp:LinkButton ID="lbtnSearch" runat="server" OnClick="lbtnSearch_Click"></asp:LinkButton>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var FolderID = "<%=FolderID %>";
        var Status = "<%=Status %>";
        var Admin_Board_Content_SpecialChar = "<%=GetResource("List_DBInjection") %>";
        var Admin_Board_Content_RequestInput = "<%=GetResource("List_Msg_NotUseSpecial") %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값  
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja    

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Project/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Project/List.js");
    </script>
</body>
</html>
