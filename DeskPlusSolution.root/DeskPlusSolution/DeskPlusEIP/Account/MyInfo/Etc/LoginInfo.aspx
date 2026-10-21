<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Account.Web.MyInfo.Etc.LoginInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var DeptID = "<%=DeptID %>";
        var IsAdmin = "<%=IsAdmin %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja    
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";         
        var R_Name = "<%=GetResource("Admin_UserList_MSG_HTName") %>"; //이름
        var R_Gubun = "<%=GetResource("Gubun") %>"; //구분
        var R_LoginTime = "<%=GetResource("LoginTime") %>"; //접속일
        var R_IPAddress = "<%=GetResource("IPAddress") %>";        
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_ApplyRegDate =  "<%=GetResource("ApplyRegDate") %>";       
        var ImagePath = "<%=BaseImgPath%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Account/Js/MyInfo/Etc/LoginInfo.js"></script>
    <script type="text/javascript">
           //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
           //JsCall("/DeskPlusEIP/Account/Js/MyInfo/Etc/LoginInfo.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("SummaryTit2")%></h1>
                <ul class="btn_right">                
                    <cc3:ClientSmartButton ID="cbtnUser" runat="server" BeginEndTag="li" />
                    <cc3:clientsmartbutton id="cbtnList" runat="server" beginendtag="li"/>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="overflow:hidden;">  
				<div class="col-12" ><!-- width 100%(col-12) -->         
                    <!-- Tab //-->
                    <ul id="tab" class="tab">
                        <li><a href="#" id="log" class="tab_on"><span><%=GetResource("SummaryTit2")%></span> </a></li>
                        <li><a href="#" id="aut"><span><%=GetResource("SummaryTit3")%></span> </a></li>
                        <li><a href="#" id="sum"><span><%=GetResource("SummaryTit4")%></span> </a></li>
                    </ul>

                    <!-- Contents //-->
			        <div id="layout_srh">
				        <ul class="btn_searchbox">
					        <li><div id="loading"></div></li>
					        <li>
                                <div class="searchterm"><asp:dropdownlist id="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:dropdownlist></div>
					            <div id="startEndDate" class="searchterm">
                                    <input type="text" id="startDate" size="12" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" class="clsDate searchtext form-control" />
                                    </div>
					        </li>
					        <li><input type="text" id="dummy" style="display: none;"/></li>
					        <li><cc3:clientsmartbutton id="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:clientsmartbutton></li>
				        </ul>
			        </div>
                        
			        <div id="layout_content" style="min-height:470px;height:470px;overflow:auto;">   
                        <br />                
				        <!-- 테이블 시작 //-->
				        <div id="placeholder" style="clear:both;"></div>
				        <!-- 테이블 끝 //-->
				        <!-- 페이징 시작 //-->
				        <div id="Pager1" name="Pager1" class="paging_num_s1">
					        <h3><span id="pageInfo"></span></h3>
					        <span id="pager"></span>
					        <asp:dropdownlist id="ddlListCount" runat="server" cssclass="aRight form-control">
					        <asp:listitem text="5 " value="5"></asp:listitem>
					        <asp:listitem text="10" value="10"></asp:listitem>
					        <asp:listitem text="15" value="15"></asp:listitem>
					        </asp:dropdownlist>
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

</body>
</html>