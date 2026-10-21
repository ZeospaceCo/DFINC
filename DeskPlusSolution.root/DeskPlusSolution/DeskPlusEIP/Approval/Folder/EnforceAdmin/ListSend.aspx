<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSend.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.EnforceAdmin.ListSend" %>
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
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("Administrator")%><%=GetResource("EnforceTitle2")%></h1>
            <!-- 버튼 //-->
            <span class="app_caption">* <%=GetResource("EnforceTitleSub5")%></span>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    <div class="searchterm">
                        <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect form-control">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="week"></asp:ListItem>
                        <asp:ListItem Value="month"></asp:ListItem>
                        <asp:ListItem Value="period"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="startEndDate" class="searchterm">
                        <input type="text" id="startDate" size="10" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="10" class="clsDate searchtext form-control" />
                    </div>
                </li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder=""></cc3:ClientSmartTextBox>
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
                </div><!-- end col-3 content-area-body-right -->
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
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var TargetPage = "<%=TargetPage %>";
        var CabinetID = "<%=CabinetID %>"; 
        
        var StampDeptIDArr = "<%=StampDeptIDArr %>";
        var EnforceStampIDArr = "<%=EnforceStampIDArr %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  

        var R_Number = "<%=GetResource("Number")%>";              //"번호";
        var R_DocNumber =  "<%=GetResource("DocNumber")%>";       //"문서번호";
        var R_Subject = "<%=GetResource("Subject")%>";            //"제목";
        var R_AttachName = "<%=GetResource("AttachName")%>";      //"첨부파일";
        var R_Name = "<%=GetResource("ReportUser")%>";            //"상신자";
        var R_Writetime = "<%=GetResource("AddDate")%>";        //"상신일";
        var R_SendDeptname = "<%=GetResource("SendDept")%>";       //"발신부서";
        var R_CompleteDate = "<%=GetResource("CompleteDate")%>";       //"완료일";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_MSG_NeedAddDate = "<%=GetResource("MSG_NeedAddDate")%>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate")%>";
        var R_EnforceCompleteNumber  = "<%=GetResource("EnforceCompleteNumber")%>";  //승인번호	
        var R_EnforceReqNumber	 = "<%=GetResource("EnforceReqNumber")%>";  //접수번호	
        var R_ReceiveDept = "<%=GetResource("ReceiveDept")%>";              //수신처
        var R_EnforceIsSendInfo = "<%=GetResource("EnforceIsSendInfo")%>";    //발송여부
        var R_EnforceView = "<%=GetResource("EnforceView")%>";   //신청내용보기
        var R_EnforceSend = "<%=GetResource("EnforceSend")%>";   //시행문발송
        var R_EnforceConfirmInfo= "<%=GetResource("EnforceConfirmInfo")%>";   //확인자정보
        var R_EnforceConfirmDate= "<%=GetResource("EnforceConfirmDate")%>";   //확인일
        var R_EnforceReqInfo = "<%=GetResource("EnforceReqInfo")%>";    //신청자정보


        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_Msg_Del = "<%=GetResource("Msg_Del")%>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/ListSend.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/ListSend.js");
    </script>

</body>
</html>
