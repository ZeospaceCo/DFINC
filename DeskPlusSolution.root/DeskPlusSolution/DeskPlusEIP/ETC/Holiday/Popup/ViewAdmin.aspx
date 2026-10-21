<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewAdmin" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnExcel" runat="server" /></li>
                    <li class="sec"></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
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
                <!-- 페이징 끝 //-->
            </div>
        </div>
        <div style="display:none">        
            <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
            <input type="text" id="hdexcel" runat="server" />    
            <asp:HiddenField ID="hdstartdate" runat="server" />
            <asp:HiddenField ID="hdenddate" runat="server" />
            <asp:HiddenField ID="hdname" runat="server" />
        </div>
    </div>
    
    </form>
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
        var UserID = "<%=UserID%>";
        var Name = "<%=Name%>";
        var StartDate = "<%=StartDate%>";
        var EndDate = "<%=EndDate%>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수                
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_StartDate = "<%=GetResource("StartDate") %>";
        var R_StartHalf = "<%=GetResource("StartHalf") %>";
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_EndHalf = "<%=GetResource("EndHalf") %>";
        var R_Duration = "<%=GetResource("Duration") %>";
        var R_Gubun = "<%=GetResource("Gubun") %>";
        var R_DocView = "<%=GetResource("DocView") %>";
        var R_HoliDel = "<%=GetResource("HoliDel") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";      
        var R_MSG_DELSUCCESS = "<%=GetResource("MSG_DELSUCCESS")%>";
        var R_MSG_DELFAIL = "<%=GetResource("MSG_DELFAIL") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewAdmin.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewAdmin.js");
    </script>
</body>
</html>