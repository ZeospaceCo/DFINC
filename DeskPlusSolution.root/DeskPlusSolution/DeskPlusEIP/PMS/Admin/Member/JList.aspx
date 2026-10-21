<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JList.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Admin.Member.JList" %>
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
            <h1><%=GetResource("Menu_admin2") %></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li><asp:DropDownList ID="ddlStatus" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><input type="text" id="dummy" style="display: none;" />
                <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control"  placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">

            <div class="row">
			   <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>  
    <script type="text/javascript">
    /**********************************************************************************************
    전역 변수들
    **********************************************************************************************/   
    var R_STATUS= "<%=GetResource("Unit_Register_ltlStatus") %>";
    var R_NAME= "<%=GetResource("Board_List_Name") %>";
    var R_DEPT= "<%=GetResource("ProjectList_Header7") %>";
    var R_WRITEDATE= "<%=GetResource("Member_RegisterDate") %>";
    var R_VISITEDDATE= "<%=GetResource("Member_VisitDate") %>";    

    var R_Member_List_ChargerDel          = "<%=GetResource("Member_List_ChargerDel") %>";
    var R_Member_List_ChargerSelectDel    = "<%=GetResource("Member_List_ChargerSelectDel") %>";
    var R_Previous = "<%=GetResource("Previous") %>";
    var R_Next = "<%=GetResource("Next") %>";
    var R_NoContent = "<%=GetResource("NoContent") %>";
    var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
    
    var ProjectID               = "<%=ProjectID %>";
    var MenuID                  = "<%=MenuID %>";  
    var MenuType                = "<%=MenuType %>"; 
    var SearchTerm = "<%=SearchTerm %>"; //날자 분류
    var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
    var SearchField = "<%=SearchField %>"; //검색키워드
    var SearchValue = "<%=SearchValue %>"; //검색어
    var CurrentPage = "<%=CurrentPage %>"; //현재페이지
    var TotalItemCount = 0; //게시물수
    var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
    var Order = "<%=Order %>"; //정렬     
     
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>"; //ja     
 
    var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
    var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";         
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Admin/Member/JList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/PMS/Js/Admin/Member/JList.js");
    </script>
</body>
</html>