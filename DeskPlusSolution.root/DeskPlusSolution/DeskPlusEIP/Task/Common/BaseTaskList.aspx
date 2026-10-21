<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseTaskList.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Common.BaseTaskList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup">
    <div class="top-left"></div>
    <div class="top-right"></div>

    <div class="inside" style="min-height:577px">
        <div class="popup_header">
            <h5><%=GetResource("Task")%> <%=GetResource("AddDoc")%></h5>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnSave" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />
                <li><a href="javascript:self.close()" title=""><span><%=GetResource("Close")%></span></a></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:170px;">
            <div id="tabs-1">
                <!-- 검색 //-->                
                <div class="layout_srh">
                    <ul class="cw_src co_gap">
                        <li><asp:CheckBoxList ID="cbxStatus" RepeatDirection="Horizontal" runat="server" CssClass="sel_stat"></asp:CheckBoxList></li>
                    </ul>
                    <ul class="btn_searchbox">
                        <li>
                            <div id="loading">
                            </div>
                        </li>
                        <li>
                            <div class="searchterm">
                                <asp:DropDownList ID="ddlItem" runat="server" CssClass="searchselect"></asp:DropDownList>
                                <asp:DropDownList ID="ddlItemSub" runat="server" CssClass="searchselect"></asp:DropDownList>
                                <asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect">
                                    <asp:ListItem Text="" Value="Title"></asp:ListItem>                                    
                                    <asp:ListItem Text="" Value="TaskNumber"></asp:ListItem>                                                        
                                    <asp:ListItem Text="" Value="RequestDate"></asp:ListItem>
                                    <asp:ListItem Text="" Value="StartDate"></asp:ListItem>
                                    <asp:ListItem Text="" Value="EndDate"></asp:ListItem>                            
                                    <asp:ListItem Text="" Value="ManyName"></asp:ListItem>   
                                </asp:DropDownList>
                            </div>
                            <div id="startEndDate" class="searchterm">
                                <input type="text" id="startDate" size="10" class="clsDate" />-<input type="text" id="endDate" size="10" class="clsDate" />
                            </div>
                        </li>              
                        <li>
                            <input type="text" id="dummy" style="display: none;" />
                            <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None"
                                class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                        </li>
                        <li>
                            <cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn"></cc3:ClientSmartButton>
                        </li>
                    </ul>
                </div>
                 <div class="app_gap10" />
                <!-- 문서함 리스트 //--> 
                
                <div id="placeholder_listBase" style="clear: both;"></div>
                
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
           
            
            <table class="table_typeA" id="contentTaskTable">
                <caption><%=GetResource("BaseDocSelList")%></caption>
                <colgroup>
                    <col width="200px" />
                    <col />
                    <col width="60px" />
                    <col width="50px" />                    
                    <col width="70px" />
                </colgroup>
                <thead>
                    <tr>
                        <th class="center"><%=GetResource("DocNumber")%></th>
                        <th><%=GetResource("Title")%></th>
                        <th class="center"><%=GetResource("DocView")%></th>
                        <th class="center"><%=GetResource("Del")%></th>
                        <th><%=GetResource("ChangeOrder")%></th>            
                    </tr>
                </thead>
                <tbody>
                    <tr id="noTaskContent">
                        <td colspan="5" class="center"><span class='NoListData' /></td>
                    </tr>                    
                </tbody>
                </table>
                
            
            </div>            
        </div>
    </div>

    <div class="bottom-left"></div>
    <div class="bottom-right"></div>
</div>

<div style="display:none">
    <input name="tbAddTaskID" type="text" id="tbAddTaskID" value="" size="150"> 
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
        var GroupID = "<%=GroupID %>";        
        var ItemID = "<%=ItemID %>"; //1차분류
        var ItemSubID = "<%=ItemSubID %>"; //2차분류
        var ListCount = "<%=ListCount %>"; //보여줄게시물수        
        var TotalItemCount = "0"; //총게시물수     
        var IsType = "BA";
        var IsGubun = "<%=IsGubun %>";
        var BaseTaskCnt = 0;
        var BaseTasks = "<%=BaseTasks %>"; // (부모창에 추가된 documentid들)        
        var BaseTaskPopUpIDs = "";//(팝업창에서 추가한 documentid들)
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_NoFileData = "<%=GetResource("NoFileData") %>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_No = "<%=GetResource("No") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_RequestDate = "<%=GetResource("RequestDate") %>";
        var R_StartDate = "<%=GetResource("StartDate") %>";
        var R_EndDate = "<%=GetResource("EndDate") %>";
        var R_Add = "<%=GetResource("Add") %>";
        var R_NoListData = "<%=GetResource("NoListData") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";        
        var R_Requester = "<%=GetResource("Requester") %>";
        var R_ExistBase = "<%=GetResource("ExistBase") %>";
        var R_MSG_NoSelect = "<%=GetResource("MSG_NoSelect") %>";
        var R_SubCategory = "<%=GetResource("SubCategory") %>";
        var R_ThisBase  = "<%=GetResource("ThisBase") %>";
        var R_IsGubun  = "<%=GetResource("IsGubun") %>";
        var R_IsGubun1 = "<%=GetResource("ShortIsGubun1") %>";
        var R_IsGubun2 = "<%=GetResource("ShortIsGubun2") %>";  
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; 
    </script>
        <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
        <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
        <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/BaseTaskList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/BaseTaskList.js");
    </script>
</body>
</html>
