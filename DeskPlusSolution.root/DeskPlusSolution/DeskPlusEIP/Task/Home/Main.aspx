<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Home.Main" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Home.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Home.css" /> 
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
   <form id="form1" runat="server"> 
 <div id="layout_body">
	<div class="layout_full">   
		    <div id="layout_header">
				<h1><%=GetResource("Task") %></h1>
                <div class="btn_right"> 
                    <!-- 검색 -->
                    <fieldset class="app_srch">
                       <input id="searchValue" title="검색어" class="keyword" accesskey="s" type="text" placeholder="" />
                       <span id="cbtnSearch" class="s_btn" alt="<%=GetResource("OrganBtnSearch")%>" style="cursor:pointer"><%=GetResource("OrganBtnSearch")%></span>
                    </fieldset>
                </div>
			</div><!-- end layout_header  -->
            
            <div class="layout_srh clearfix"> 
                <div class="float-left">
                    <!-- Tab Menu //-->
                    <ul id="Tagmenu" class="tab"  runat="server">
                        <li><a id="tabPage"><%=GetResource("AllTask")%></a></li>
                        <li class="selected"><a><%=GetResource("UserTask")%></a></li>
                    </ul>
                </div>
                <div class="search-container float-right"> 
                </div> 
            </div>             
                       
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body" ><!-- width 100%(col-12) -->    
                       <div class="c_t_Wa col-12 col-lg-6 p-0 pr-lg-4 mb-5">
                            <div class="l_list">
                                <span class="s_tit"><%=GetResource("ProcessList")%><%=GetResource("HRETO")%></span>
                                <span class="float-right">
                                    <a href="#" id="processL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                    <a href="#" id="processR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                </span>
                            </div>
                            <div id="phProcessList" style="clear: both;"></div>
                       </div>

                       <div class="c_t_Wa col-12 col-lg-6 p-0  mb-5">
                            <div class="l_list">
                                <span class="s_tit"><%=GetResource("HCCTitle")%></span>
                                <span class="float-right">
                                    <a href="#" id="ccL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                    <a href="#" id="ccR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                </span>
                            </div>
                            <div id="phCCList" style="clear: both;"></div>
                        </div>       
                              
                       <div class="src_C col-12">
                            <div class="c_t_Fa">
                                <div class="l_list">
                                    <span class="s_tit"><%=GetResource("HDelayTitle")%></span>
                                    <span class="float-right">
                                        <a href="#" id="delayL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="delayR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                </div>
                                <div id="phDelayList" style="clear: both;"></div>
                            </div>
                        </div>
                            
                       <div class="src_C col-12">
                            <div class="c_t_Fa">
                                <div class="l_list">
                                    <span class="s_tit"><%=GetResource("RecentList")%></span>
                                    <span class="float-right">
                                        <a href="#" id="recentL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="recentR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                </div>
                                 <div id="phRecentList" style="clear: both;"></div>
                            </div>
                        </div>                            
                            
                       <div class="src_C col-12">
                            <div class="c_t_Fa">
                                <div class="l_list">
                                    <span class="s_tit"><%=GetResource("WriteCommentList")%></span>
                                    <span class="float-right">
                                        <a href="#" id="writeCommentL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="writeCommentR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                </div>
                                <div id="phWritecommentList" style="clear: both;"></div>
                            </div>
                        </div>                           
                            
                       <div class="src_C" style="display:none">
                            <div class="c_t_Fa">
                                <div class="imp_list">
                                    <span class="s_tit"><%=GetResource("ImpoTitle")%></span>
                                    <span class="float-right">
                                        <a href="#" id="impoL" title="<%=GetResource("BeforePage")%>"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="impoR" title="<%=GetResource("NextPage")%>"><i class="fas fa-chevron-right"></i></a>
                                    </span>
                                </div>
                                <div id="phImpoList" style="clear: both;"></div>
                            </div>
                        </div>   

				    </div><!-- end col content-area-body -->                    
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
        var ProcessPageNo = "<%=ProcessPageNo%>";
        var CCPageNo = "<%=CCPageNo%>";        
        var DelayPageNo = "<%=DelayPageNo%>";                
        var RecentPageNo = "<%=RecentPageNo%>";  
        var WriteCommentPageNo  = "<%=WriteCommentPageNo%>";
        var ImpoPageNo = "<%=ImpoPageNo%>";
        var ReLoad = "<%=ReLoad%>";
        var R_ProgressNoContent = "<%=GetResource("ProgressNoContent")%>";
        var R_CCNoContent  = "<%=GetResource("CCNoContent")%>";
        var R_HDelayNoContent  = "<%=GetResource("HDelayNoContent")%>";
        var R_UserNoContent = "<%=GetResource("UserNoContent")%>";
        var R_PointNoContent = "<%=GetResource("PointNoContent")%>";
        var R_IsGubun  = "<%=GetResource("IsGubun")%>";
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
        var R_FirstList   = "<%=GetResource("FirstList")%>";
        var R_LastList   = "<%=GetResource("LastList")%>";
        var R_InputSearch = "<%=GetResource("InputSearch")%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Home/Main.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Home/Main.js");
    </script>
</body>
</html>

