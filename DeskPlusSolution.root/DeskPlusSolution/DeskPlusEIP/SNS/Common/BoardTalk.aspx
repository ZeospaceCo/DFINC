<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BoardTalk.aspx.cs" Inherits="Zeospace.DeskPlus.SNS.Web.Common.BoardTalk" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_SNS.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_SNS.css"/>        
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">

    <div class="inside">
        <div class="popup_header">
            <h1><span><%=GetResource("WorkTalk")%></span></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnShortReply" runat="server" class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="CbtnReload" runat="server"/></li>
                <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="padding-top:5px;height:310px;min-height:310px; border:0px solid #f00;">

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

        <div class="layout_subbox" style="padding:0 20px 0 20px">
            <h1><%=GetResource("ShortReply")%></h1>
            (<span class="charsRemaining"></span>) 
            <ul class="btn_right">
                
            </ul>
            <textarea name="textarea" id="textarea"  class="textarea" style="width:97%;height:55px;" maxlength="200"></textarea>
        </div>
    </div>

    <div class="bottom-left"></div>
    <div class="bottom-right"></div>
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
        var R_ReallyApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Processing = "<%=GetResource("MSG_SUCCESS") %>";
        var R_Add_ReplyInputContent = "<%=GetResource("NeedContents") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/SNS/Js/Common/BoardTalk.js" ></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/SNS/Js/Common/BoardTalk.js");
    </script>
</body>
</html>
