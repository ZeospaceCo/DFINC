<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCompany.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.ListCompany" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/Select/bootstrap.css" />
</head>
<body>
    <div id="fnm" class="btn-secondary text-white"><i class="fa fa-chevron-left fa-chevron-right"></i></div>

    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExcel" runat="server" BeginEndTag="li" />
                </ul>
                <ul class="titleDateBox">
                    <li>
                        <i class="fas fa-angle-left" id="imgLeft"></i>
                        <span id="titleDate" class="titleDate"></span>
                        <i class="fas fa-angle-right" id="imgRight"></i>
                    </li>
                </ul>
                <div class="isin">
                    <span style="cursor: help"><abbr title="변경부서 또는 삭제부서의 이력정보조회"><input type="checkbox" id="chkIsIN" runat="server" />변경내역</abbr></span>                
                </div>
            </div>

            <div id="layout_srh">       
                <ul class="btn_searchbox float-left" style="text-align: left">
                    <li>&nbsp;<asp:DropDownList ID="ddlDeptList" runat="server" class="chosen form-control" style="width:300px;"></asp:DropDownList></li>         
                </ul>           
                <ul class="btn_searchbox">                    
                    <li><div id="loading"></div></li>   
                    <li><asp:DropDownList ID="ddlGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li id="liYear" style="display: none">
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                            <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li id="liQuarter" style="display: none">
                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li id="liMonth" style="display: none">
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li><input type="text" id="startDate" size="10" class="clsDate searchtext" readonly />-<input type="text" id="endDate" size="10" class="clsDate searchtext" readonly /></li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>    
                </ul>
            </div> 
            <div class="conmain" style="display: none">
                <div class="conmove"><cc3:ClientSmartButton ID="cbtnDown" runat="server" BeginEndTag="li" /></div>   
            </div>                
            <div id="layout_content" style="padding: 0 30px 20px 30px">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                        <!-- 테이블 시작 //-->
                        <div id="placeholder" style="clear: both; overflow-x:auto; overflow-y:hidden;"></div>
                        <!-- 테이블 끝 //-->
        
                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1" style="display: none"> 
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
                        <div class="conmain" style="display: none">
                            <div class="conmove"><a href="#">▲<%=GetResource("TopView")%></a></div>   
                        </div>
				    </div><!-- end col content-area-body-right -->
                    
			    </div><!-- end row  -->
		    </div><!-- end layout_content  -->      
        </div>
        <div style="display:none">        
            <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
            <input type="text" id="hdexcel" runat="server" />
            <input type="text" id="hdStartDate" runat="server" />
            <input type="text" id="hdEndDate" runat="server" />
            <input type="text" id="hdSearchType" runat="server" />
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/Select/chosen.jquery.js"></script>
    <script type="text/javascript">

        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/ 
        var UserID = "<%=UserID %>";
        var DeptID = "<%=DeptID %>";     
        var StartDate = "<%=StartDate %>";              //시작일자
        var EndDate = "<%=EndDate %>";                  //종료일자

        var WeekStartDate = "<%=WeekStartDate %>";              //시작일자
        var WeekEndDate = "<%=WeekEndDate %>";                  //종료일자
        var SearchType = "<%=SearchType %>";                  //버튼검색 or 선택날짜검색 구분
        var Year = "<%=Year %>";
        var NowYear = "<%=NowYear %>";  //초기화용
        var NowMonth = "<%=NowMonth %>";  //초기화용
        var NowQuarter = "<%=NowQuarter %>";  //초기화용

        var SearchField = "<%=SearchField %>";  //검색키워드
        var SearchValue = "<%=SearchValue %>";  //검색어

        var FolderStatusID = "<%=FolderStatusID %>";
        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수       
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        
        var R_DeptName = "<%=GetResource("DeptName") %>";     //부서명
        var R_PersonCount = "<%=GetResource("PersonCount") %>";     //인원
        var R_WorkTime = "<%=GetResource("WorkTime") %>";  //근무시간
        var R_52Time = "<%=GetResource("52Time") %>";  //주 52시간 기준
        var R_Holi_HR = "<%=GetResource("Holi_HR") %>";     //근태정보
        var R_HoliUserInfo = "<%=GetResource("HoliUserInfo") %>";     //연차 사용자 정보 (휴가/반차/기타)

        var R_PersonCount2 = "<%=GetResource("PersonCount2") %>";     //(명)
        var R_BaseWork = "<%=GetResource("BaseWork") %>";     //기본근무
        var R_Over_Work = "<%=GetResource("Over_Work") %>";   //연장근무
        var R_StandardTime = "<%=GetResource("StandardTime") %>";   //기준시간
        var R_WorkSum = "<%=GetResource("WorkSum") %>";   //근무합산
        var R_StandardTimePer = "<%=GetResource("StandardTimePer") %>";   //요율
        var R_Attendance_Tardy = "<%=GetResource("Attendance_Tardy") %>";     //지각
        var R_Attendance_Early = "<%=GetResource("Attendance_Early") %>";     //조퇴
        var R_HoliType = "<%=GetResource("HoliType") %>";     //연차
        var R_Etc = "<%=GetResource("Etc") %>";     //기타

        var R_SearchLimit = "<%=GetResource("SearchLimit") %>"; //5년 이하만 검색 가능합니다.
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/ListCompany.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/HR/Js/Content/ListCompany.js");
    </script>
</body>
</html>
