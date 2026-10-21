<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUpStats.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.PopUpStats" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layer_popup">
    <div class="inside">
        <div id="popup_header">
            <!-- 페이지타이틀 //-->
            <h1>제안통계보기</h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnClose" runat="server" Text="닫기"  BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div id="scrollDiv" >
            
            <div id="phPopStats" style="clear: both;"></div>
            
            <!-- 테이블 끝 //-->

            <!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1">
                <h3>
                    <span id="pageInfo"></span>
                </h3>
                <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            </div>
            <!-- 페이징 끝 //-->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var StartYears = "<%=StartYears %>";
        var StartMonths = "<%=StartMonths %>";
        var EndYears = "<%=EndYears %>";
        var EndMonths = "<%=EndMonths %>";
        var JudgeGubun = "<%=JudgeGubun %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어   
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var ListCount = "<%=ListCount %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var TotalItemCount = "0"; //총게시물수
        var R_Previous = "이전";
        var R_Next = "다음";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/PopUpStats.js"></script>
    <script type="text/javascript">
        //JsCall("../Js/Content/PopUpStats.js");
    </script>
</body>
</html>
