<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListCompleteSlip.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.ListCompleteSlip" %>
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
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnDefault" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnCirculate" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false"/>
                    <cc3:ClientSmartButton ID="cbtnForward" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false"/>
                    <cc3:ClientSmartButton ID="cbtnTransfer" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnPost" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false"/>
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger"/>
                    <cc3:ClientSmartButton ID="cbtnDelBatch" runat="server" BeginEndTag="li" class="btn btn-danger"/>
                </ul>
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

            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <!-- 테이블 시작 //-->
            
                        <div id="placeholder" style="clear: both;"></div>
            
                        <!-- 테이블 끝 //-->

                        <!-- 페이징 시작 //-->
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
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
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var FolderStatusID = "<%=FolderStatusID %>";
        var TargetPage = "<%=TargetPage %>"; //리스트 경로 
        var PageUrl = "<%= Request.Url.ToString() %>";

        var R_Previous = "<%=GetResource("Previous")%>";
        var R_Next = "<%=GetResource("Next")%>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate")%>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate")%>";
        var R_NoFileData = "<%=GetResource("NoFileData")%>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";
        var R_MSG_SelPassDoc = "<%=GetResource("MSG_SelPassDoc")%>";
        var R_MSG_SelTransferDoc = "<%=GetResource("MSG_SelTransferDoc")%>";
        var R_MSG_TransferComplete = "<%=GetResource("MSG_TransferComplete")%>";
        var R_MSG_SelDelDoc = "<%=GetResource("MSG_SelDelDoc")%>";
        var R_MSG_DelDoc = "<%=GetResource("MSG_DelDoc")%>";
        var R_MSG_DelAll = "<%=GetResource("MSG_DelAll")%>";
               
        /*****  JT 다국어 리소스 시작******/
        var R_FormName = "<%=GetResource("FormName1")%>";
        var R_Status = "<%=GetResource("Status")%>";
        var R_Subject = "<%=GetResource("Subject")%>";
        var R_AttachName = "<%=GetResource("AttachName")%>";
        var R_Name = "<%=GetResource("ReportUser")%>";
        var R_WriteTime = "<%=GetResource("WriteDate")%>";
        var R_CompleteDate = "<%=GetResource("CompleteDate")%>";

        var R_W = "<%=GetResource("StatusW")%>";
        var R_I = "<%=GetResource("StatusI")%>";
        var R_R = "<%=GetResource("StatusR")%>";
        var R_D = "<%=GetResource("StatusD")%>";
        var R_C = "<%=GetResource("StatusC")%>";
        var R_H = "<%=GetResource("StatusH")%>";
        var R_P = "<%=GetResource("StatusP")%>";
        /*****  JT 다국어 리소스 끝******/

        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";          
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Status/ListCompleteSlip.js?v=3"></script> 
</body>
</html>