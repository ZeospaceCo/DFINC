<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Content.Forward.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
   <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">
    
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("CollaboReceived")%></h1>
        </div>

        <div id="layout_srh">
            <ul class="cw_src co_gap">
                <li><asp:CheckBoxList ID="cbxStatus" RepeatDirection="Horizontal" runat="server" CssClass="sel_stat"></asp:CheckBoxList></li>
            </ul>
            <ul class="btn_searchbox">

                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlIsGubun" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlItemSub" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control">
                            <asp:ListItem Text="" Value="Title"></asp:ListItem>
                            <asp:ListItem Text="" Value="ExcessDay"></asp:ListItem>
                            <asp:ListItem Text="" Value="TaskNumber"></asp:ListItem>                                                        
                            <asp:ListItem Text="" Value="RequestDate"></asp:ListItem>
                            <asp:ListItem Text="" Value="StartDate"></asp:ListItem>
                            <asp:ListItem Text="" Value="EndDate"></asp:ListItem>                            
                            <asp:ListItem Text="" Value="ManyName"></asp:ListItem>     
                        </asp:DropDownList>
                    </div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>                
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            
                    <div id="phList" style="clear: both;"></div>           

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
                    <!-- 페이징 끝 //-->
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
    <!-- ############# end of modal ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>  
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
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
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchStatus = "<%=SearchStatus %>"; //상태값     
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어           
        var ListCount = "<%=ListCount %>"; //보여줄게시물수        
        var ItemID = "<%=ItemID %>"; //대분류
        var ItemSubID = "<%=ItemSubID %>"; //소분류
        var TotalItemCount = "0"; //총게시물수
        var TreeTotalCnt = "<%=TreeTotalCnt%>";
        var TreeNoReadCnt = "<%=TreeNoReadCnt%>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_No = "<%=GetResource("No") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_AttachName = "<%=GetResource("AttachFile") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_Category = "<%=GetResource("Category") %>";
        var R_SubCategory = "<%=GetResource("SubCategory") %>";
        var R_RequestDate = "<%=GetResource("RequestDate") %>";        
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_NoListData = "<%=GetResource("NoListData") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Requester = "<%=GetResource("Requester") %>";
        var R_Forwarder = "<%=GetResource("Forwarder") %>";
        var IsGubun = "<%=IsGubun %>"; //구분
        var R_IsGubun = "<%=GetResource("IsGubun") %>";
        var R_IsGubun1 = "<%=GetResource("ShortIsGubun1") %>";
        var R_IsGubun2 = "<%=GetResource("ShortIsGubun2") %>";   
        var R_CollaboReceived = "<%=GetResource("CollaboReceivedStr") %>"; 
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Content/Forward/List.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Task/Js/Content/Forward/List.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
    </script>
</body>
</html>
