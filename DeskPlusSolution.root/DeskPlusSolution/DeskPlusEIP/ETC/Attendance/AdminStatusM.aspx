<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminStatusM.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.AdminStatusM" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
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
        var UserID = "<%=UserID %>";              //시작일자
        var DeptID = "<%=DeptID %>";              //시작일자
        var StatusYear = "<%=StatusYear %>";                    //년도
        var StatusMonth = "<%=StatusMonth %>";                  //월
        var SearchField = "<%=SearchField %>";  //검색키워드
        var SearchValue = "<%=SearchValue %>";  //검색어
        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수       
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        
        var R_Attendance_Department = "<%=GetResource("Attendance_Department") %>";     //부서                             
        var R_Attendance_personnel = "<%=GetResource("Attendance_personnel") %>";       //인원
        var R_Gubun = "<%=GetResource("Gubun") %>";                                     //구분        
        var R_Attendance_normal = "<%=GetResource("Attendance_normal") %>";             //정상
        var R_Attendance_Start = "<%=GetResource("Attendance_Start") %>";               //출근
        var R_Attendance_Tardy = "<%=GetResource("Attendance_Tardy") %>";               //지각
        var R_Attendance_End = "<%=GetResource("Attendance_End") %>";                   //퇴근  
        var R_Attendance_percent = "<%=GetResource("Attendance_percent") %>";           //율
        var R_Attendance_max = "<%=GetResource("Attendance_max") %>";                   //최대
        var R_Attendance_min = "<%=GetResource("Attendance_min") %>";                   //최소
        var R_Attendance_avg = "<%=GetResource("Attendance_avg") %>";                   //평균                  

        var R_NoContent = "<%=GetResource("NoContent") %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";
    </script>    
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("/DeskPlusEIP/ETC/Js/Attendance/AdminStatusM.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Attendance_Department")%><%=GetResource("Attendance_CurrentStatus")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_left">
                    <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnExcel" runat="server" BeginEndTag="li" />
                </ul>
                <ul class="btn_right">                    
                </ul>
            </div>
            <div id="layout_srh">            
                <ul class="btn_searchImg">
                    <li>
                        <ul class="pim_tab">
                            <li><a href="#" id="aDay"  onclick="AdminStatusM.TabPageMove('day')" ><%=GetResource("Attendance_Date")%></a></li>
                            <li><a href="#" id="aMonth" class="tab_on" onclick="AdminStatusM.TabPageMove('month')" ><%=GetResource("Attendance_month")%></a></li>
                            <li class="last">
                            <a href="#" id="aYear" onclick="AdminStatusM.TabPageMove('year')" ><%=GetResource("Attendance_year")%></a></li>            
                        </ul>
                    </li>
                </ul>
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>   
                    <li><asp:DropDownList ID="ddlYear" runat="server" Width="60" CssClass="searchselect"></asp:DropDownList></li>
                    <li><asp:DropDownList ID="ddlMonth" runat="server" Width="50" CssClass="searchselect"></asp:DropDownList></li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect"></asp:DropDownList></li>   
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                            class="searchtext"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton></li>         
                </ul>
            </div>                    
            <div id="layout_content">
                <!-- 테이블 시작 //-->
                <div class="content_box"  style="overflow:auto;">    
                <div id="placeholder" style="clear: both;"></div>
                </div>
                <!-- 테이블 끝 //-->

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
	                <span id="pager"></span>
                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight">
                        <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <!-- 페이징 끝 //-->
            </div>                    
        </div>
        <div style="display:none">        
            <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
            <input type="text" id="hdexcel" runat="server" />
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
