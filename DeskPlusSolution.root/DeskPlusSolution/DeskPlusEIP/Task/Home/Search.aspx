<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Home.Search" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" /> 
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Homesrc.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Homesrc.css" /> 
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
   <form id="form1" runat="server">
      <div id="layout_body">
         <div class="layout_full">
            <div id="layout_header">            
                <h1><%=GetResource("Task")%></h1>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchImg">
                    <li>
                      <h1><span><%=GetResource("SearchStr")%></span></h1>
                    </li>
                </ul>             
                <ul class="btn_searchbox">                           
                    <li><input id="searchValue" type="text" runat="server" class="searchtext form-control" accesskey="s"  style="width:200px;" /></li>
                    <li><input id="cbtnSearch" type="submit"  runat="server" Class="searchbtn btn-secondary" alt="검색" /></li>
                </ul>               
    
           </div>
      
		   <div id="layout_content"><!-- 컨텐츠 하단영역 -->
			   <div class="row">
				    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <div id="phList" style="clear: both;"></div>

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1">
                            <h3>
                                <span id="pageInfo"></span>
                            </h3>
                            <span id="pager"></span>
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchValue = "<%=SearchValue %>"; //검색어           
        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var AdminYN  = "<%=AdminYN %>"; 
        var ReLoad = "<%=ReLoad%>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";        
        var R_Title   = "<%=GetResource("Title")%>";
        var R_HName   = "<%=GetResource("HName")%>";
        var R_HGubun  = "<%=GetResource("HGubun")%>";
        var R_RequestDept = "<%=GetResource("RequestDept")%>";
        var R_Requester   = "<%=GetResource("Requester")%>";
        var R_RequestDate = "<%=GetResource("RequestDate")%>";
        var R_EndDate = "<%=GetResource("HEndDate")%>";
        var R_Receiver = "<%=GetResource("Receiver")%>";
        var R_ToDeptName = "<%=GetResource("ToDeptName")%>";    
        var R_State = "<%=GetResource("State") %>";   
        var R_AttachFile = "<%=GetResource("AttachFile") %>";   
        var R_InputSearch = "<%=GetResource("InputSearch")%>";
        var R_NoListData = "<%=GetResource("NoContent")%>";    
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Home/Search.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Home/Search.js");
    </script>
</body>
</html>
