<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Home.Content" %>

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
        <div class="layout_full ">
            <div id="layout_header">
				<h1><%=GetResource("HomeTitle") %></h1>
                <div class="btn_right"> 
                    <!-- 검색 -->
                    <fieldset class="app_srch">
                        <input id="searchValue" title="<%=GetResource("HomeSrhTarget") %>" class="keyword" accesskey="s" type="text" placeholder="<%=GetResource("PlaceholderMenu") %>">
                        <span id="cbtnSearch" class="s_btn" style="cursor:pointer" title="<%=GetResource("OrganBtnSearch") %>"><%=GetResource("OrganBtnSearch")%></span>
                    </fieldset>
                </div>

			</div><!-- end layout_header  -->

            <div class="layout_srh clearfix"> 
                <div class="float-left">
                    <!-- Tab Menu //-->
                    <ul id="ulTabMenu" class="tab">
                        <li class="selected" title="<%=GetResource("HomeTabTit02")%>+<%=GetResource("HomeTabTit03")%>+<%=GetResource("HomeTabTit04")%>"><a onclick="javascript:Content.SetTab('ListTotal');"><%=GetResource("HomeTabTit01")%></a></li>
                        <li><a onclick="javascript:Content.SetTab('ListApprove');"><%=GetResource("HomeTabTit02")%><i id="appmore" class="fas fa-arrow-circle-right bullet" title="more"></i></a></li>
                        <li><a onclick="javascript:Content.SetTab('ListComplete');"><%=GetResource("HomeTabTit03")%><i id="compmore" class="fas fa-arrow-circle-right bullet"  title="more"></i></a></li>
                        <li><a onclick="javascript:Content.SetTab('ListCarbon');"><%=GetResource("HomeTabTit04")%><i id="ccmore" class="fas fa-arrow-circle-right bullet"  title="more"></i></a></li>
                    </ul>
                </div>
                <div class="search-container float-right"> 
                </div> 
            </div> 

            <div id="layout_content"> 
            <div class="row"> 
                <div class="src_body col-12 content-area-body row" style="vertical-align:top;"> 

    
                            <div class="c_t_Wa col-12 mb-5 p-0">
                                <div id="phlist" style="clear: both;min-height:260px;"></div>
                            </div> 
 							   
                            <div class="col-12 col-lg-6 p-0 pr-lg-4 " >
                                <div class="c_t_Wa  mb-5">
                                <!-- 최근 첨부파일 사용내역 -->
                                    <div class="l_list">
                                        <span class="s_tit" id="spAttachTit"></span><span class="s_tit_s" id="spAttachTitSub"></span>
                                        <span class="float-right">
                                        <a href="#" id="attachB"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="attachN"><i class="fas fa-chevron-right"></i></a>                                        
                                        </span>
                                    </div>
                                    <div id="phAttach" style="clear: both;"></div>
                                </div>
    
                                <div class="c_t_Wa mb-5">
                                    <div class="l_list">
                                        <span class="s_tit" id="spImpoTit"></span>
                                        <span class="float-right">
                                        <a href="#" id="impoB"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="impoN"><i class="fas fa-chevron-right"></i></a>                                        
                                        </span>
                                    </div>
                                    <div id="phImpo" style="clear: both;"></div>
                                </div>  
    
                                <!-- 자주 지정하는 결재자 -->
                                <div class="c_t_Wa  mb-5">
                                    <div class="l_list">
                                        <span class="s_tit" id="spFavAppTit"></span>
                                        <span class="float-right">
                                        <a href="#" id="favAppB"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="favAppN"><i class="fas fa-chevron-right"></i></a>                                        
                                        </span>
                                    </div>
                                    <div id="phFavApp" style="clear: both;"></div>
                                </div>
                            </div>
                            <div class="col-12 col-lg-6 p-0 ">
                            <!-- 결재의견 리스트 -->
                                <div class="c_t_Wa">
                                    <div class="l_list">
                                        <span class="s_tit" id="spReplyTit"></span>
                                        <span class="float-right">
                                        <a href="#" id="replyB"><i class="fas fa-chevron-left"></i></a>
                                        <a href="#" id="replyN"><i class="fas fa-chevron-right"></i></a>                                        
                                        </span>
                                    </div>
                                    <div id="phReply" style="clear: both;"></div>
                                    </div>
				            </div>

                </div><!-- end col  -->

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

    var AttachCnt = "";  //리소스삭제
    var ReplyCnt = "<%=ReplyCnt %>";
    var FavAppCnt = "<%=FavAppCnt %>";
    var ImpoCnt = "<%=ImpoCnt %>";
    var DefaultTab = "<%=DefaultTab %>";

    var R_HomeAttachTit= "<%=GetResource("HomeAttachTit") %>";
    var R_HomeAttachTitSub = "<%=GetResource("HomeAttachTitSub") %>";
    var R_HomeNoData = "<%=GetResource("HomeNoData") %>";
    var R_HomeImpoLink = "<%=GetResource("HomeImpoLink") %>";
    var R_HomeFavLink = "<%=GetResource("HomeFavLink") %>";
    var R_HomeReplyTit = "<%=GetResource("HomeReplyTit") %>";
    var R_HomeFavAppTit = "<%=GetResource("HomeFavAppTit") %>";
    var R_HomeDelMsg = "<%=GetResource("HomeDelMsg") %>";
    var R_HomeImpoTit = "<%=GetResource("HomeImpoTit") %>";
    var R_HomeImpoTh01 = "<%=GetResource("HomeImpoTh01") %>";
    var R_HomeImpoTh02 = "<%=GetResource("HomeImpoTh02") %>";
    var R_HomeFileTh01 = "<%=GetResource("HomeFileTh01") %>";
    var R_HomeFileTh02 = "<%=GetResource("HomeFileTh02") %>";
    var R_HomeFileTh03 = "<%=GetResource("HomeFileTh03") %>";
    var R_HomeFileTh04 = "<%=GetResource("HomeFileTh04") %>";
    var R_HomeTabTH01 = "<%=GetResource("HomeTabTH01") %>";     // 구분
    var R_HomeTabTH02 = "<%=GetResource("HomeTabTH02") %>";     // 제목
    var R_HomeTabTH03 = "<%=GetResource("HomeTabTH03") %>";     // 진행률
    var R_HomeTabTH04 = "<%=GetResource("HomeTabTH04") %>";     // 결재자정보
    var R_HomeTabTH05 = "<%=GetResource("HomeTabTH05") %>";     // 상신자
    var R_HomeTabTH06 = "<%=GetResource("HomeTabTH06") %>";     // 정보    

    var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
    var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    var R_InputSearch = "<%=GetResource("InputSearch") %>";
    var R_Btn_Del = "<%=GetResource("Btn_Del") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Home/Content.js?v=10"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Home/Content.js");
    </script>
</body>
</html>