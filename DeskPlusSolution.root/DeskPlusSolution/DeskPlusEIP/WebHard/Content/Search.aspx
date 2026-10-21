<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.Search" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" />   
    <link rel='stylesheet' href='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.css' type='text/css' />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
	<style>
	    .orangeSquare badge badge-info{ line-height:20px;padding:2px 3px;margin:0 0 2px 0; text-align:center; color:#FFFFFF; background-color:#ED6405; }
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
     <div class="layout_full">
        <div id="layout_header">            
          <h1><%=GetResource("WebHardTitle")%></h1>
          <span class="app_caption" ><%=GetResource("DownloadAuth")%></span>    
        </div>

        <div id="layout_srh">
            <ul class="btn_searchImg">
                <li>
                  <h1><span><%=GetResource("SearchTarget") %></span></h1>
                </li>
            </ul>

            <ul class="btn_searchbox">                           
                <li>
                  <input name="searchValue" type="text" id="searchValue" class="searchtext form-control" placeholder="<%=GetResource("SearchTarget")%>" required="False" title="" style="width:200px;" />
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary" ></cc3:ClientSmartButton></li>
            </ul>
        </div>
       
		<div id="layout_content"><!-- 컨텐츠 하단영역 -->
			<div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
                    <div id="phSearchList" style="clear: both;"></div>
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
                    <!-- 페이징 끝 //-->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var TxtSearchValue = "<%= TxtSearchValue %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수

        
        var R_FoldersType = "<%=GetResource("Folders") %>" + "<%=GetResource("FileType") %>";
        var R_Folders = "<%=GetResource("Folders") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_Download = "<%=GetResource("File") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_FileType = "<%=GetResource("FileType") %>";        
        var R_RegUser = "<%=GetResource("RegUser") %>";  
        var R_RegDate = "<%=GetResource("RegDate") %>";        
        var R_NoContent = "<%=GetResource("NoContent")%>";

        var R_PublicFolder = "<%=GetResource("PublicFolder")%>";
        var R_SendFile = "<%=GetResource("SendFile")%>";
        var R_ShareFolder = "<%=GetResource("ShareFolder")%>";

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";

        var R_LengthOver =  "<%=GetResource("LengthOver")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/Search.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Content/Search.js");
    </script>
</body>
</html>

