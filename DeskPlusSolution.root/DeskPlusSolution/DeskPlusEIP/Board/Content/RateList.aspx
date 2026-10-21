<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RateList.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.RateList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>         
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("RatingList")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnRating" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="CbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div class="layout_scroll" style="min-height:370px;height:370px;">
            
            <div id="placeholder" style="clear: both;"></div>

			<!-- 페이징 시작 //-->
            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                    <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                    <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                    <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                    <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            </div>
            <!-- 페이징 끝 //-->
        </div>
    </div>

</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%=ListID %>"; //카테고리
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수        
        var Order = "<%=Order %>"; //정렬
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Rating = "<%=GetResource("Rate_Title") %>";
        var R_Comments = "<%=GetResource("RatingComment") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/RateList.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/RateList.js");
    </script>
</body>
</html>
