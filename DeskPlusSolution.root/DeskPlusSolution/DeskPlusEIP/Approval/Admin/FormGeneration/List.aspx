<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">

            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("ManageForm")%></h1>
                <!-- 버튼 //-->
                <!--span class="app_subtxt"> * <%=GetResource("Msg_IE") %></span-->
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnActive" runat="server"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnFormSeq" runat="server"></cc3:ClientSmartButton></li>
                    <li id="check_cbtnAdd"><cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li>
                        <div class="searchterm">
                            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        </div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                        </div>
                    </li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                    <li>
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder=""></cc3:ClientSmartTextBox>
                        <input type="text" id="dummy" style="display: none;" />
                    </li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                </ul>
            </div>

            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                        <!-- 테이블 시작 //-->
            
                        <div id="placeholder" style="clear: both;"></div>
            
                        <!-- 테이블 끝 //-->

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

                    </div><!-- end col content-area-body-right -->
                </div><!-- end row  -->
            </div><!-- end layout_content  -->
    
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
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
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
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
        var SearchTerm      = "<%=SearchTerm %>";       //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>";  //날자값        
        var SearchField     = "<%=SearchField %>";      //검색키워드
        var SearchValue     = "<%=SearchValue %>";      //검색어
        var CurrentPage     = "<%=CurrentPage %>";      //현재페이지 (PageNo)
        var TotalItemCount  = "<%=TotalItemCount %>";   //게시물수 (전체)
        var RecordPerPage   = "<%=RecordPerPage %>";    //보여줄게시물수 (PageSize)
        var Order           = "<%=Order %>";            //정렬
            
        var ImagePath       = "<%=BaseImgPath%>";  
        var PreLang         = "<%=PreLang%>";
        var PreLangReal     = "<%=PreLangReal %>"; //ja     

        var R_FormName = "<%=GetResource("FormName2") %>";  //"양식명";
        var R_FolderName = "<%=GetResource("FolderName") %>";    //"폴더명";
        var R_IsLegacy = "<%=GetResource("Custom") %>";       //"커스텀";
        var R_IsEDM = "<%=GetResource("EDM") %>";     //"EDM";
        var R_WriteDate = "<%=GetResource("AddDate") %>";     //"등록일";
        var R_Msg_ErrActive = "<%=GetResource("MSG_Active") %>";     //"활성/비활성화할 항목을 선택하세요.";
        var R_Msg_ErrDelItem = "<%=GetResource("MSG_SelDel") %>";    //"삭제할 항목을 선택하세요.";
        var R_Msg_Active = "<%=GetResource("MSG_DoActive") %>";     //"활성/비활성화 하시겠습니까?";
        var R_Msg_Del = "<%=GetResource("ConfirmDeleteFavoritePerson") %>";        //"삭제하시겠습니까?";
        var R_ApplyRegDate = "<%=GetResource("MSG_NeedAddDate") %>";      //"등록일자를 입력해 주세요.";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";       //"시작일자가 종료일자보다 작아야 합니다.";
        var R_Draft = "<%=GetResource("PrivateDraftCnt") %>";   
        var R_Admin = "<%=GetResource("Setting") %>";   
        var R_EA = "<%=GetResource("EA") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_IsActiveTit = "<%=GetResource("IsActiveTit") %>";

        var R_Previous  = "<%=GetResource("Previous") %>";
        var R_Next      = "<%=GetResource("Next") %>";
        var R_Msg_IE    = "<%=GetResource("Msg_IE") %>";
        var R_Msg_ErrFormSequence = "<%=GetResource("Msg_ErrFormSequence") %>"; // 변경할 양식을 선택하세요.
        var R_Msg_ErrSelFolder = "<%=GetResource("Msg_ErrSelFolder") %>";   // 순서변경시, 대상폴더는 하나만 선택하세요. 
        
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/FormGeneration/List.js"></script>       
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/FormGeneration/List.js");
    </script>

</body>
</html>
