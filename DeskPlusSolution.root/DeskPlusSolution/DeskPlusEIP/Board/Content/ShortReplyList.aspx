<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShortReplyList.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.ShortReplyList" %>
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
            <h1><%=GetResource("ShortReply")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3">
            <div id="scrollDiv">
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


            <div class="layout_subbox">
                <h2><%=GetResource("ShortReply")%></h2>
                (<span class="charsRemaining"></span>) 
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <textarea name="textarea" id="textarea"  class="textarea" style="width:97%;height:55px;" maxlength="200"></textarea>

        </div>
    </div>
</div>
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CategoryID = "<%=CategoryID %>"; //카테고리
        var ListID = "<%=ListID %>"; //카테고리
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수     
        var Order = "0";// 0 : asc , 1: desc           
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ReallyApplyDelete = "<%=GetResource("ReallyApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("Add_ReplyInputContent") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/ShortReplyList.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Content/ShortReplyList.js");
    </script>
</body>
</html>
