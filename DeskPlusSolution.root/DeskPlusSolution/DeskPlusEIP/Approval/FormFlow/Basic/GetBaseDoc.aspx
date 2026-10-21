<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetBaseDoc.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.GetBaseDoc" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside" style="min-height:577px">
        <div class="popup_header">
            <h1><%=GetResource("AddBaseDoc")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                <li><a href="javascript:self.close();" title="" class="btn"><%=GetResource("Close") %></a></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:170px;">
            <div class="layout_srh" style="padding-left:10px;">
                <ul class="tab" >
                    <li><a class="tab_on" id="dept"> <span><%=GetResource("DeptFolder")%></span> </a></li>
                    <li><a id="private"> <span><%=GetResource("PrivateFolder")%></span> </a></li>
				    <li><a id="approve"> <span><%=GetResource("ApprovalFolder")%></span> </a></li>
                    <li><a id="arbitrary"> <span><%=GetResource("ArbitraryFolder")%></span> </a></li>
				    <li><a id="carboncopy"> <span><%=GetResource("CarbonFolder")%></span> </a></li>
                    <li><a id="forward"> <span><%=GetResource("TransFolderForward")%></span> </a></li>
                    <li><a id="privateprocess"> <span><%=GetResource("DocProcess")%></span> </a></li>
                </ul>
            </div>
            <div id="tabs-1">
                <!-- 검색 //-->                
                <div class="app_popSrh search-container">
                    <ul class="btn_searchbox" style="margin: 0px">
                        <li><div id="loading"></div></li>
                        <li>
                        <div class="searchterm">
                        <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="week"></asp:ListItem>
                        <asp:ListItem Value="month"></asp:ListItem>
                        <asp:ListItem Value="period"></asp:ListItem>
                        </asp:DropDownList></div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                        </div>
                        </li>
                        <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                        <li>
                            <input type="text" id="dummy" style="display: none;" />
                            <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                                class="searchtext form-control" placeholder=""></cc3:ClientSmartTextBox>
                        </li>
                        <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                    </ul>
                </div>
                <!-- 문서함 리스트 //--> 
                
                <div id="placeholder_baseDocList" style="clear: both;"></div>
                
                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
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
            <div class="app_gap10">
            
            <table class="table_typeA" id="contentTable">
                <caption><%=GetResource("SelBaseDocList")%></caption>
                <colgroup>
                    <col width="120px" />
                    <col width="200px" />
                    <col width="120px" />
                    <col />
                    <col width="120px" />
                    <col width="60px" />
                    <col width="50px" />                    
                    <col width="70px" />
                </colgroup>
                <thead>
                    <tr>
                        <th><%=GetResource("Sort")%></th>
						<th><%=GetResource("TagDic_NO")%></th>
						<th><%=GetResource("FormName2")%></th>
						<th><%=GetResource("Subject")%></th>
						<th class="center"><%=GetResource("ReportUser")%></th>
					    <th class="center"><%=GetResource("DocView")%></th>
                        <th class="center"><%=GetResource("Btn_Del")%></th>
                        <th><%=GetResource("ChangeOrder3")%></th>            
                    </tr>
                </thead>
                <tbody>
                    <tr id="noContent">
                        <td colspan="8" class="center"><%=GetResource("NoContent")%></td>
                    </tr>                    
                </tbody>
                </table>
                
                <ul style="display: none">
                    <li >
                        <ul class="itemlist" id="contentTable2">
                        </ul>
                    </li>
                </ul>
            
            </div>            
        </div>
    </div>
</div>

<div style="display:none">
    <input name="tbAddDocID" type="text" id="tbAddDocID" value="" size="150"> 
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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/

        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var FolderName = "<%=FolderName %>";
        var RowMax = 0;
        var BaseDocIDs = "<%=BaseDocIDs %>"; //부모창에 추가된 documentid들
        var BaseDocPopUpIDs = ""; //팝업창에서 추가한 documentid들

        var R_DocNumber = "<%=GetResource("DocNumber") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_FormName = "<%=GetResource("FormName2") %>";
        var R_Name = "<%=GetResource("ReportUser") %>";
        var R_Writetime = "<%=GetResource("WriteDate") %>";
        var R_Completetime = "<%=GetResource("CompleteDate") %>";
        var R_Add = "<%=GetResource("Add") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_ApplyRegDate =  "<%=GetResource("ApplyRegDate")%>";          //"상신일자를 입력해 주세요.";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Msg_ErrBaseDoc = "<%=GetResource("Msg_ErrBaseDoc") %>";
        var R_Msg_ErrBaseDocList = "<%=GetResource("Msg_ErrBaseDocList") %>";
        var R_Msg_ErrBaseSameDoc = "<%=GetResource("Msg_ErrBaseSameDoc") %>";
        var R_DeptFolder = "<%=GetResource("DeptFolder") %>";
        var R_PrivateFolder = "<%=GetResource("PrivateFolder") %>";
        var R_ApprovalFolder= "<%=GetResource("ApprovalFolder") %>";
        var R_ArbitraryFolder= "<%=GetResource("ArbitraryFolder") %>";
        var R_CarbonFolder = "<%=GetResource("CarbonFolder") %>";
        var R_TransFolderForward = "<%=GetResource("TransFolderForward") %>";
        var R_DocProcess = "<%=GetResource("DocProcess") %>";
        var R_MSG_NoSelect = "<%=GetResource("MSG_NoSelect") %>";   //"선택된 문서가 없습니다."
        var R_FileDel   = "<%=GetResource("Del") %>";
        var R_Sort = "<%=GetResource("Sort")%>";
        var R_ReportUser= "<%=GetResource("ReportUser") %>";

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
    
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Basic/GetBaseDoc.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Basic/GetBaseDoc.js");
    </script>
</body>
</html>
