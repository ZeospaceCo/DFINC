<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.HolidayD.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">    
              <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("HolidayManageDay")%></h1>
                <!-- 버튼 //-->
                  <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                  </ul>
            </div>
            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                    <li>                    
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
            	    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <!-- 테이블 시작 //-->                
                        <div id="placeholder" style="clear: both;"></div>                
                        <!-- 테이블 끝 //-->   
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script> 
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CompanyID%>";
        var SearchField = "<%=SearchField %>";  //년  
        var SearchValue = "<%=SearchValue %>";  //일정조건    
                
        //목록                
        var Holiday_IsHolidayKind2 = "<%=GetResource("Holiday_IsHolidayKind2") %>";     //양음력구분
        var Holiday_SolarLunar = "<%=GetResource("Holiday_SolarLunar") %>";             //양음력구분
        var Holiday_HoliDay2 = "<%=GetResource("Holiday_HoliDay2") %>";                 //휴무일자
        var Holiday_Title = "<%=GetResource("Holiday_Title" )%>";                       //휴일명
        var Holiday_HolidayBetween = "<%=GetResource("Holiday_HolidayBetween") %>";     //휴무구분        
        var Holiday_IsReplace = "<%=GetResource("Holiday_IsReplace") %>";               //대체휴일사용여부      
        var WRITER = "<%=GetResource("WRITER") %>";                                     //작성자  
        
        var Holiday_Solar = "<%=GetResource("Holiday_Solar") %>";                       //양력
        var Holiday_Lunar = "<%=GetResource("Holiday_Lunar") %>";                       //음력          
        
        var TextEmpty = "<%=GetResource("TextEmpty") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/HolidayD/List.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/HolidayD/List.js");
    </script>
</body>
</html>
