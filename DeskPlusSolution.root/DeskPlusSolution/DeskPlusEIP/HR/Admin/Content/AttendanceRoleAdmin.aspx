<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttendanceRoleAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Admin.Content.AttendanceRoleAdmin" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Modify")%><%=GetResource("AuthorityManagement")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_left">
                    
                </ul>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary" />            
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                </ul>
            </div>
            <div id="layout_srh">            
                <ul class="btn_searchbox">                    
                    <li><div id="loading"></div></li>                    
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                            class="searchtext form-control"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>         
                </ul>
            </div>                    
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
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

        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/   
        var menuID = "<%=menuID %>";         
        var SearchField = "<%=SearchField %>";          //검색키워드
        var SearchValue = "<%=SearchValue %>";          //검색어
        var CurrentPage = "<%=CurrentPage %>";          //현재페이지
        var TotalItemCount = "<%=TotalCount %>";        //게시물수
        var RecordPerPage = "<%=RecordPerPage %>";      //보여줄게시물수
        var RoleID = "<%=RoleID %>";
                                           
        var R_DeptName = "<%=GetResource("DeptName") %>";                                       //부서명
        var R_UserName = "<%=GetResource("Name") %>";                                           //이름
        var R_TitleName = "<%=GetResource("TitleName") %>";                                     //직급
        var R_DutyName = "<%=GetResource("DutyName") %>";                                       //직책
        var R_View = "<%=GetResource("View") %>";                                               //조회
        
        var R_Previous = "<%=GetResource("Previous") %>";                                       //이전
        var R_Next = "<%=GetResource("Next") %>";                                               //다음
        var R_NoContent = "<%=GetResource("NoContent") %>";                                     //데이터 없음

        var R_CheckDeleteList = "<%=GetResource("CheckDeleteList") %>";
        var R_ConfirmDelete = "<%=GetResource("ConfirmDelete") %>";
        var R_ErrorProcessing = "<%=GetResource("ErrorProcessing") %>";
                              
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Admin/Content/AttendanceRoleAdmin.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/HR/Js/Admin/Content/AttendanceRoleAdmin.js");
    </script>
</body>
</html>