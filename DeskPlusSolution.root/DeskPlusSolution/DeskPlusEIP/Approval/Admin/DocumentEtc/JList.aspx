<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JList.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.DocumentEtc.JList" %>

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
                <h1><%=GetResource("ManageDoc")%></h1>
                <!--span class="app_caption"> * <%=GetResource("Msg_AdminDocDel")%></span-->
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                </ul>
            </div>

            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 content-area-body pt-0 mt-4" style="" ><!-- width 100%(col-12) -->

                            <div class="app_fdr">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="240px" valign="top" style="vertical-align:top;border-top:1px solid rgba(0,0,0,.1)">
                                        <div class="incontent">
                                             <div class="cont" style="padding-left: 3px; border-left: 1px solid rgba(0,0,0,.1); border-right: 1px solid rgba(0,0,0,.1); border-bottom: 1px solid rgba(0,0,0,.1);">
                                                  <zeoAppFormTree:UCAppFormTreeScript ID="UCFormTreeScript1" runat="server" SetNodeSelect="onFormNodeSelect" UseActiveForm="false" UseSetFolder="1,2,3" UseAdmin="true" />
                                             </div>
                                        </div>
                                    </td>
                                    <td valign="top" style="padding-left:10px;">
                                        
                                    <div id="layout_srh" >
                                        <ul class="btn_searchbox">
                                            <li><div id="loading"></div></li>
                    
                                            <li>
                                                <select name="ddlDocStatus" id="ddlDocStatus" class="searchselect form-control">
                                                    <option value="CompleteDoc"><%=GetResource("DocComplete")%></option>
                                                    <option value="ProcessDoc"><%=GetResource("DocProcess")%></option>
                                                </select>                            
                                            </li>
                                            
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
                                                    class="searchtext" placeholder=""></cc3:ClientSmartTextBox>
                                            </li>
                                            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                                        </ul>
                                    </div>
                    
                                    <div class="app_gap"></div>
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
                    
                                
                                    </td>
                                </tr>
                                </table>
                            </div>

                    </div><!-- end col content-area-body-right -->
                </div><!-- end row  -->
            </div><!-- end layout_content  -->
    
        </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    </form>
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
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
    <form id="Form2" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="Hidden1" />
    <iframe id="Iframe1" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
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
        var TotalItemCount = "0"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var FormID = "0";

        var R_Number = "<%=GetResource("Number")%>";              //순번
        var R_DocID = "DocID";        //문서번호
        var R_DocNumber = "<%=GetResource("DocNumber")%>";        //문서번호
        var R_Subject = "<%=GetResource("Subject")%>";            //제목
        var R_AttachName = "<%=GetResource("AttachName")%>";      //첨부파일
        var R_Name = "<%=GetResource("ReportUser")%>";            //상신자
        var R_Writetime = "<%=GetResource("WriteDate")%>";        //상신일
        var R_Completetime = "<%=GetResource("CompleteDate")%>";  //완료일

        var R_Previous = "<%=GetResource("Previous")%>";                      //"이전";
        var R_Next = "<%=GetResource("Next")%>";                              //"다음";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate")%>";              //"상신일자를 입력해 주세요.";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate")%>";      //"시작일자가 종료일자보다 작아야 합니다.";
        var R_NoFileData = "<%=GetResource("NoFileData")%>";                  //"해당 파일이 존재하지 않습니다.";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";        //"첨부파일 정보를 가져오는데 에러가 발생하였습니다.";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";                //"전체다운로드";
        var R_NoContent = "<%=GetResource("NoContent")%>";                    //"내용이 없습니다.";
        var R_Msg_ErrSelForm = "<%=GetResource("Msg_ErrSelForm")%>";          //"양식을 선택하세요.";

        var R_MSG_Del2 = "<%=GetResource("HomeDelMsg")%>";          //"삭제하시겠습니까?";
        var R_MSG_Del3 = "<%=GetResource("MSG_SelDelDoc")%>";       //"삭제할 문서를 선택해 주세요.";
        var R_MSG_Del4 = "<%=GetResource("MSG_Del4")%>";            //"선택한 문서가 완전 영구삭제되었습니다.";
        var R_MSG_Del5 = "<%=GetResource("MSG_DisuseDel")%>";       //"진행중인 문서는 영구 삭제되어 복구 불가 합니다."

        var R_DisuseDoc = "<%=GetResource("DisuseDoc") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";

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
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/DocumentEtc/JListCom.js?v=1"></script>  
</body>
</html>