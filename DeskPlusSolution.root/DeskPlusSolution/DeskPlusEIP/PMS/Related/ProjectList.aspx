<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectList.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Related.ProjectList" %>
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
    <div class="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Admin_Menu3")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
                </ul>

            </div>
            <div class="dotLine"></div>

            <div id="layout_srh">
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
                        <div id="startEndDate" class="searchterm" style="display:none">
                            <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                        </div>
                    </li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li><input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>         
                </ul>
            </div>

            <div class="blankLine"></div>

            <div class="popup_layout_style3">
                <div id="scrollDiv" style="min-height:370px">
                <!-- 테이블 시작 //-->                
                <div id="placeholder" style="clear: both;"></div>                
                <!-- 테이블 끝 //-->
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
                </div>
            </div>      
        </div>
    </div>
   
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/
        var Status = "<%=Status %>";
        var ProjectID = "<%=ProjectID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var Admin_Board_Content_SpecialChar = "<%=GetResource("List_DBInjection") %>";
        var Admin_Board_Content_RequestInput = "<%=GetResource("List_Msg_NotUseSpecial") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_projectname = "<%=GetResource("List_ProjectName") %>";
        var R_deptname = "<%=GetResource("ProjectList_Header1") %>";
        var R_username = "<%=GetResource("ProjectList_Header2") %>";
        var R_planstartdate = "<%=GetResource("ProjectList_Header3") %>";
        var R_planenddate = "<%=GetResource("ProjectList_Header4") %>";
        var R_status = "<%=GetResource("ProjectList_Header5") %>";
        var R_process = "<%=GetResource("ProjectList_Header6") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Project_Content_MsgProject = "<%=GetResource("Project_Content_MsgProject") %>";
        var R_CompleteAdd = "<%=GetResource("CompleteAdd") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja    

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Related/ProjectList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Related/ProjectList.js");
    </script>
</body>
</html>