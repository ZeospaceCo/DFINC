<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDept.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Transfer.ListDept" %>
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

    <div id="layout_header">
        <!-- 페이지타이틀 //-->
        <h1><%=GetResource("TransferDocDept")%></h1>
        <ul class="btn_right">
            <li><cc3:ClientSmartButton ID="cbtnTransfer" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
            <li><cc3:ClientSmartButton ID="cbtnTransferAll" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
            <li><cc3:ClientSmartButton ID="cbtnCopy" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
            <li><cc3:ClientSmartButton ID="cbtnCopyAll" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
        </ul>
        <span class="cap1"style="float:right;padding-right:10px;padding-top:3px;color:blue;"><%=GetResource("TransferCap1")%></span>
    </div>

    <div id="layout_content">
        <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                <div class="halfL">

                    <div class="subheader">
                        <div class="title"><h2 style="display:inline;"><%=GetResource("TransferInfo")%></h2></div>
                        <div><a id="btnsend" class="btn"><%=GetResource("Transferbtn")%></a></div>
                        <div class="info" id="sendinfo" runat="server"></div>
                    </div>

                    <div id="layout_srh">
                        <div class="srhgap" style="margin: 9px 30px 8px 0px">
                            <asp:DropDownList ID="ddlFolder" runat="server" CssClass="sel form-control"></asp:DropDownList>
                        </div>
                        <ul class="btn_searchbox">
                            <li><div id="loading"></div></li>
                            <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                            <li>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="100px" InputMode="None" class="searchtext form-control" placeholder=""></cc3:ClientSmartTextBox>
                                <input type="text" id="dummy" style="display: none;" />
                            </li>
                            <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                        </ul>
                    </div>
            
                    <!-- 테이블 시작 //-->
                    <div id="placeholder" class="sublist"></div>
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

                </div>
    
                <div class="halfR">

                    <div class="subheader">
                        <div class="title"><h2 style="display:inline;"><%=GetResource("TransferTarget")%></h2></div>
                        <div><a id="btntarget" class="btn"><%=GetResource("Transferbtn")%></a></div>
                        <div class="info" id="targetinfo" runat="server"></div>
                    </div>

                    <div id="layout_srh">
                        <div class="srhgap" style="margin: 9px 30px 8px 0px">
                            <asp:DropDownList ID="ddlFolderTarget" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <ul class="btn_searchbox">
                            <li><div id="loading2"></div></li>
                            <li><asp:DropDownList ID="ddlSearchFieldTarget" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>   
                            <li>
                                <cc3:ClientSmartTextBox runat="server" ID="ctbSearchTarget" Width="100px" InputMode="None" class="searchtext form-control" placeholder=""></cc3:ClientSmartTextBox>
                                <input type="text" id="dummyTarget" style="display: none;" />
                            </li>
                            <li><cc3:ClientSmartButton ID="cbtnSearchTarget" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li> 
                        </ul>
                    </div>

                    <!-- 테이블 시작 //-->
                    <div id="tplaceholder" class="sublist"></div>
                    <!-- 테이블 끝 //-->
                    <!-- 페이징 시작 //-->
                    <div id="Pager2" name="Pager2" class="paging_num_s1"> 
                        <h3><span id="pageInfo2"></span></h3>
	                    <span id="pager2"></span>
                        <asp:DropDownList ID="ddlListCountTarget" runat="server" CssClass="aRight form-control">
                            <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <!-- 페이징 끝 //-->

                </div>
            </div><!-- end col-3 content-area-body-right -->
        </div><!-- end row  -->
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

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager2.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        //이관할부서
        var SearchField     = "<%=SearchField %>";      //검색키워드
        var SearchValue     = "<%=SearchValue %>";      //검색어
        var CurrentPage     = "<%=CurrentPage %>";      //현재페이지 (PageNo)
        var RecordPerPage   = "<%=RecordPerPage %>";    //보여줄게시물수 (PageSize)
        var Order           = "<%=Order %>";            //정렬

        //이관받을부서
        var tSearchField    = "<%=tSearchField %>";    //검색키워드
        var tSearchValue    = "<%=tSearchValue %>";    //검색어
        var tCurrentPage    = "<%=tCurrentPage %>";    //현재페이지 (PageNo)
        var tRecordPerPage  = "<%=tRecordPerPage %>";  //보여줄게시물수 (PageSize)
        var tOrder          = "<%=tOrder %>";          //정렬

        var TotalItemCount  = 0;            //이관할부서
        var tTotalItemCount = 0;            //이관받을부서

        var callid = "send";                //호출정보  (send or target)
        var ReqID   = "<%=ReqID %>";        // 변경전정보
        var ReqName = "<%=ReqName %>";
        var TargetID   = "<%=TargetID %>";  // 변경후정보
        var TargetName = "<%=TargetName %>";

        var ImagePath   = "<%=BaseImgPath%>";  
        var PreLang     = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_Number = "<%=GetResource("Number")%>";   //"순번";
        var R_DocNumber = "<%=GetResource("TagDic_NO")%>"; //"문서번호";
        var R_Subject = "<%=GetResource("Subject")%>";
        var R_AttachName = "<%=GetResource("AttachName")%>";
        var R_Name = "<%=GetResource("ReportUser")%>";       
        var R_WriteTime = "<%=GetResource("WriteDate")%>";
        var R_CompleteDate = "<%=GetResource("CompleteDate")%>";
        
        var R_NoContent = "<%=GetResource("NoContent")%>";      
        var R_Msg_ErrSelUser = "<%=GetResource("Msg_ErrSelUser")%>";      
        var R_MSG_SUCCESS  = "<%=GetResource("MSG_SUCCESS")%>";      
        var R_MSG_SelItem  = "<%=GetResource("MSG_SelItem")%>";      

        var R_Previous = "<%=GetResource("Previous")%>";
        var R_Next = "<%=GetResource("Next")%>";
        var R_TransferMsgDept = "<%=GetResource("TransferMsgDept")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";    
            
        var R_TransferMsgSubmit = "<%=GetResource("TransferMsgSubmit")%>";          //이관하시겠습니까?
        var R_TransferMsgSubmitAll = "<%=GetResource("TransferMsgSubmitAll")%>";    //전체 이관하시겠습니까?
        var R_TransferMsgCopy = "<%=GetResource("TransferMsgCopy")%>";              //복사하시겠습니까?
        var R_TransferMsgCopyAll = "<%=GetResource("TransferMsgCopyAll")%>";        //전체 복사하시겠습니까?
        var R_TransferMsgTargetInfo = "<%=GetResource("TransferMsgTargetInfo")%>";  //대상정보를 선택하세요.
        var R_TransferMsgChk = "<%=GetResource("TransferMsgChk")%>";    //이관 정보과 이관 받을 정보를 다르게 선택하세요

    </script>
    <%--<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Transfer/ListDept.js"></script>--%>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("/DeskPlusEIP/Approval/Js/Admin/Transfer/ListDept.js");
    </script>
</body>
</html>
