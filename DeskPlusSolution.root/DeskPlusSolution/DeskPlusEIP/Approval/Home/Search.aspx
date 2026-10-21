<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Home.Search" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1.DTD/xhtml1-frameset.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Homesrc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Homesrc.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_appro.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
<form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">

        <div id="layout_header">            
         <h1><%=GetResource("HomeTitle")%></h1>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg">
                <li>
                  <h1><span><%=GetResource("SearchTarget")%></span></h1>
                </li>
            </ul>

            <ul class="btn_searchbox">      
                <li>
                  <input name="searchValue" type="text" id="searchValue" class="searchtext" placeholder="<%=GetResource("HomeSrhTarget") %>" required="False" title="" style="width:200px;">
                </li>
                <li><a href="#" class="searchbtn btn-secondary" id="cbtnSearch" title="<%=GetResource("OrganBtnSearch") %>"><%=GetResource("OrganBtnSearch") %></a></li>
            </ul>
        </div>

        <div id="layout_content"><!-- 컨텐츠 하단영역 -->
			<div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
                    <div id="phSearchDoc" style="clear: both;"></div>
                    <!-- 테이블 끝 //-->
        
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                        <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->


				</div><!-- end col content-area-body-right -->
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
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
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang %>";
    var PreLangReal = "<%=PreLangReal %>"; //ja     


    var SearchValue = "<%=SearchValue %>";
    var IsAuthDoc = "";

    var CurrentPage = "<%=CurrentPage %>"; //현재페이지
    var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
    var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
    var R_Previous = "<%=GetResource("Previous")%>";
    var R_Next = "<%=GetResource("Next")%>";


    var R_HomeNoData = "<%=GetResource("HomeNoData") %>";    

    var R_DocNumber = "<%=GetResource("DocNumber") %>";
    var R_Subject = "<%=GetResource("Subject") %>";
    var R_AttachName = "<%=GetResource("AttachName") %>";
    var R_ReportUser = "<%=GetResource("ReportUser") %>";
    var R_WriteDate = "<%=GetResource("WriteDate") %>";
    var R_CompleteDate = "<%=GetResource("CompleteDate") %>";
    var R_HomeSearchDocTit = "<%=GetResource("HomeSearchDocTit") %>";

    var R_Msg_AttachDownAuth = "<%=GetResource("Msg_AttachDownAuth") %>";
    var R_NoFileData = "<%=GetResource("NoFileData") %>";
    var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
    var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
    var R_InputSearch = "<%=GetResource("InputSearch") %>";
    
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Home/Search.js?v=6"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Home/Search.js");
    </script>
</body>
</html>