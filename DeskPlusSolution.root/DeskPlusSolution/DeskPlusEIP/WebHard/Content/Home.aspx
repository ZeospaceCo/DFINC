<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Content.Home" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" /> 
    <link rel='stylesheet' href='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.css' type='text/css' />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" /> 
</head>
<body>
   <form id="form1" runat="server">

   <div id="layout_body" class="">
    <div class="layout_full ">
        <div id="layout_header">
            <h1><%=GetResource("WebHardTitle")%></h1>
            <span class="app_caption" ><%=GetResource("DownloadAuth")%></span>    
			<div class="btn_right"> 
                <!-- 검색 box -->
                <fieldset class="app_srch">
                    <input id="searchValue" title="<%=GetResource("PlaceholderText")%>" class="keyword" accesskey="s" type="text" placeholder="<%=GetResource("PlaceholderMenu")%>" />            
                    <span id="cbtnSearch" class="s_btn" style="cursor:pointer" title="<%=GetResource("Search")%>"><%=GetResource("Search")%></span>
                </fieldset>
            </div>
		</div>

        <div id="layout_content"><!-- 컨텐츠 하단영역 -->
            <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                  <!-- list -->
                  <div class="c_t_Fa">
                      <div class="l_list">
                        <span class="s_tit"><%=GetResource("PublicFolder")%></span>
                        <span class="float-right">
                        <a id="PublicFileL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                        <a id="PublicFileR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                        </span>
                      </div>
                      <div id="phPublicList" style="clear: both;"></div>
                  </div>
                  <!-- //List -->
      
                  <!-- list -->
                  <div class="c_t_Fa">
                      <div class="l_list">
                        <span class="s_tit"><%=GetResource("SendFile")%></span>
                        <span class="float-right">
                        <a id="SendFileL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                        <a id="SendFileR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                        </span>
                      </div>
                      <div id="phSendFile" style="clear: both;"></div>
                  </div>
                  <!-- //List -->

                  <!-- list -->
                  <div class="c_t_Fa">
                      <div class="l_list">
                        <span class="s_tit"><%=GetResource("ShareFolder")%></span>
                        <span class="float-right">
                        <a id="ShareFolderL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                        <a id="ShareFolderR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                        </span>
                      </div>
                      <div id="phShareFolder" style="clear: both;"></div>
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
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src='/DeskPlusEIP/Html/Js/ChartNwagon/Nwagon.js'></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PublicPageNo = "<%=PublicPageNo%>";
        var SendPageNo = "<%=SendPageNo%>";
        var SharePageNo = "<%=SharePageNo%>";
                        
        var R_Folders = "<%=GetResource("Folders") %>";        
        var R_SendUser = "<%=GetResource("SendUser") %>";
        var R_ShareUser = "<%=GetResource("ShareUser") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_Download = "<%=GetResource("File") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_FileType = "<%=GetResource("FileType") %>";        
        var R_RegUser = "<%=GetResource("RegUser") %>";  
        var R_RegDate = "<%=GetResource("RegDate") %>";                
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_FirstList   = "<%=GetResource("FirstList")%>";
        var R_LastList   = "<%=GetResource("LastList")%>";
        var R_LengthOver =  "<%=GetResource("LengthOver")%>";
        var R_PlaceholderMenu =  "<%=GetResource("FileName")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Content/Home.js"></script>
</body>
</html>

