<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.Holiday.List" %>
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
            <h1><%=GetResource("AnnivControl")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="CbtnMySchedule" runat="server" ></cc3:ClientSmartButton></li>

            <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><asp:DropDownList ID="ddlSearchYear" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><asp:DropDownList ID="ddlCategory" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>  
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
        var ImagePath ="<%=BaseImgPath%>";
        var AttachFileInfoList = '';
        var type = 'G,M,K,N,A,J';
        var opentype = '100';
        var SearchField = "<%=SearchField %>";  //년  
        var Searchkey = "<%=Searchkey %>";  //일정조건    
        var col1 = "<%=GetResource("KIND") %>"; //구분
        var col2 = "<%=GetResource("Title") %>"; //제목
        var col3 = "<%=GetResource("Period") %>"; //기간
        var col4 = "<%=GetResource("WRITER") %>"; //작성자
        var TextEmpty = "<%=GetResource("TextEmpty") %>"; //내용이없습니다.
        //목록        
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';
        var PreLang = "<%=PreLang%>"; 
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/Holiday/List.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/Holiday/List.js");
    </script>
</body>
</html>

