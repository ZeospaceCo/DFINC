<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddList.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Draft.AddList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
</head>
<body>
    <form id="form1" runat="server">

    <div id="layer_popup">

            <div id="popup_header">
                <h1><%=GetResource("Draft")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server"  class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>            
            </div>

            <div class="dotLine"></div>

            <ul class="btn_searchbox">
                <li>
                    <asp:literal id="lblSelectedReceiptor" runat="server"></asp:literal> : 
                    <cc3:ClientSmartTextBox runat="server" ID="ctbName" Width="600px" InputMode="None" class="searchtext"></cc3:ClientSmartTextBox>
                </li>
            </ul>
            <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlIsGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlItemSub" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="" Value="Title"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </li>                
                <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
            </div>

            <div  class="popup_layout_style3"   style="min-height:480px;height:450px;"> 
              <div id="scrollDiv">
            
                <div id="phList" style="clear: both;padding-top:20px"></div>
            
                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1">
                    <h3>
                        <span id="pageInfo"></span>
                    </h3>
                    <span id="pager"></span>
                </div>
                <!-- 페이징 끝 //-->
              </div>
            </div>  
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input id="hdnDraftID" type="hidden" name="hdnUserID" runat="server">        
    </div>
    <!-- ==================================================================================== -->
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ListCount = 10; //보여줄게시물수
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어           
        var ListCount = "10"; //보여줄게시물수
        var IsGubun = "<%=IsGubun %>";
        var ItemID = "<%=ItemID %>"; //대분류
        var ItemSubID = "<%=ItemSubID %>"; //소분류
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoListData = "<%=GetResource("NoListData") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_DraftDate  = "<%=GetResource("DraftDate") %>";     
        var R_NoSelectDraft = "<%=GetResource("NoSelectDraft") %>";
        var R_SubCategory = "<%=GetResource("SubCategory") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Draft/AddList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Draft/AddList.js");
    </script>
</body>
</html>
