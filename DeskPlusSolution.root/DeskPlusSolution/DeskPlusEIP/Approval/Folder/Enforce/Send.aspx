<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Send" %>
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
        <div id="layout_body" class="view">
            <div class="layout_full">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("EnforceSend")%></h1>
                    <span class="app_caption">* <%=GetResource("EnforceSendMsg1")%></span>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="CbtnPrint" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="CbtnHistory" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="CbtnPreviewStamp" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="CbtnMail" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="CbtnPrintStamp" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="CbtnCancel" runat="server" BeginEndTag="li" />
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                            <!-- 양식로드 //-->
                            <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>
                        </div><!-- end col-3 content-area-body-right -->
		            </div><!-- end row  -->
                </div>

            </div>
        </div>

<div style="display:none;">

    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <textarea id="htaTemplate" style="DISPLAY: none" name="htaTemplate" rows="2" cols="40" runat="server"></textarea>  
    <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>  
    <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>  

    <asp:TextBox ID="tbReceive" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbReference" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbEnforceDate" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbExpFirst" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbExpMid" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbExpEnd" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbZipCode" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbAddressFirst" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbAddressEnd" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbPhone" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbFax" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbHomepage" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbTargetMail" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbTargetMailSubject" runat="server" Text=""></asp:TextBox>

    <asp:TextBox ID="tbSubject" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbWriteTime" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbCompleteDate" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="tbDocNumber" runat="server" Text=""></asp:TextBox>
</div>

    </form>

    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage%>";
        var RecordPerPage = "<%=RecordPerPage%>";
        var SearchField = "<%=SearchField%>";
        var SearchValue = "<%=SearchValue%>";
        var SearchTerm = "<%=SearchTerm%>";
        var SearchTermValue = "<%=SearchTermValue%>";
        var Order = "<%=Order%>";
        var TargetPage = "<%=TargetPage%>";
        var CabinetID = "<%=CabinetID%>";

        var EnforceID = "<%=EnforceID%>";   // 공문신청ID
        var SendType = "<%=SendType%>";     // 발송형태 - 메일발송(0)/등기(1)/팩스(2)
        var StampType = "<%=StampType%>";   // 온라인직인(0)/오프라인직인(1)
        var DocType = "<%=DocType%>";       // 문서형태 - 온라인(0)/오프라인(1)
        var IsSend = "<%=IsSend%>";         // 공문발송여부
        var FormPath = "<%=FormPath%>";     // 시행문템플릿경로
        var StampPath = "<%=StampPath %>";  // 직인경로
        var R_EnforceMsgSendMail =	"<%=GetResource("EnforceMsgSendMail")%>";
        var ifrmObj = null;

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/Common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/Send.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/Common.js");    //시행문 내용변환 (커스터마이징 용도)
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/Send.js");
    </script>

</body>
</html>
