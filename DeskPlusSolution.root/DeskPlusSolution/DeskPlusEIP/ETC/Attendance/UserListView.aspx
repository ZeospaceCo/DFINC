<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserListView.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Attendance.UserListView" %>

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
        var StartDate = "<%=StartDate %>";
        var EndDate = "<%=EndDate %>";
        var CompanyID = "<%=CompanyID %>";
        var UserID = "<%=UserID %>";
        var UserName = "<%=UserName %>";
        var DeptID = "<%=DeptID %>";
        var AccessAuth = "<%=AccessAuth %>";
        var WorkType = "<%=WorkType %>";                //정규직구분                
        var CallPage = "<%=CallPage %>";

        var CurrentPage = "<%=CurrentPage %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var RecordPerPage = "<%=RecordPerPage %>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";
    </script>    
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("/DeskPlusEIP/ETC/Js/Attendance/UserListView.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Attendance_Detail")%> : <asp:Label id="lbName" runat="server"></asp:Label></h1>
                <!-- 버튼 //-->
                <ul class="btn_left">
                    <cc3:ClientSmartButton ID="cbtnExcel" runat="server" BeginEndTag="li" />
                </ul>
                <ul class="btn_right">                    
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" /></li>              
                </ul>
            </div>
            <div id="layout_srh">            
                <ul class="btn_searchImg">                                         
                </ul>
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>  
                    <li><%=GetResource("SearchDate")%> : <input type="text" id="startDate" size="10" class="clsDate" /> </li>                 
                    <li><input type="text" id="endDate" size="10" class="clsDate" /> </li>   
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton></li>                             
                </ul>
            </div>                    
            <div id="layout_content">
                <div style="float:left">    
                <%=ListContentH%>   
                </div>                    
                <!-- 테이블 시작 //-->
                <div style="overflow:auto;">    
                    <%=ListContent%>                
                </div>                            
                <!-- 테이블 끝 //-->
            </div>                    
        </div>
        <div style="display:none">     
            <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>                    
            <input type="text" id="userNamee" runat="server" />          
            <input type="text" id="userIDe" runat="server" />          
            <input type="text" id="startDatee" runat="server" />      
            <input type="text" id="endDatee" runat="server" /> 
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
