<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Add" %>
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
                    <h1><%=GetResource("EnforceAdd")%></h1>
                    <span class="app_caption">* <%=GetResource("EnforceMsg02")%></span>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="CbtnCancel" runat="server" BeginEndTag="li" />
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 content-area-body"><!-- width 100%(col-12) -->    
                            <!-- 이름.기타정보 //-->
                            <table class="table_typeB">
                            <caption><%=GetResource("EnforceMsg03")%></caption>
                            <colgroup>
                                <col width="120px" />
				                <col />
                                <col width="120px" />
                                <col />
                            </colgroup>
			                <tbody>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit01")%><span class="txt_point">*</span></th>
					                <td colspan="3">
                                        <asp:DropDownList ID="ddlDept" runat="server"></asp:DropDownList>
                                        <span id="spMsg" class="app_subtxt" style="display:">* <%=GetResource("EnforceMsg00")%></span>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("DocNumber")%></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbDocNumber" runat="server" InputMode="None" CssStyle="width:98% !important;" ReadOnly="true"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Subject")%><span class="txt_point">*</span></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbSubject" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("TagDic_ENFORCEDEPTNAME")%><span class="txt_point">*</span></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbReceive" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("TagDic_ENFORCERECEIVECARBON")%><span class="txt_point">*</span></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbReference" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceDate")%></th>
                                    <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbEnforceDate" runat="server" InputMode="None" CssStyle="width:120px !important;"></cc3:ClientSmartTextBox>&nbsp;
                                        <span class="app_subtxt">(ex) 2012-01-01</span>
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSubTit03")%> <a id="zipcode"><img src="/DeskPlusEIP/Design/C/Img/Icon/ico_add.gif" alt="" /></a></th>
                                    <td colspan="3">
                                        <cc3:ClientSmartTextBox ID="ctbZipCode" runat="server" InputMode="None" CssStyle="width:60px !important;"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="ctbAddressFirstBase" runat="server" InputMode="None" CssStyle="width:40% !important;"></cc3:ClientSmartTextBox>
                                        <cc3:ClientSmartTextBox ID="ctbAddressEndBase" runat="server" InputMode="None" CssStyle="width:45% !important;"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit04")%></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbPhoneBase" runat="server" InputMode="None" CssStyle="width:120px !important;"></cc3:ClientSmartTextBox>&nbsp;
                                        <span class="app_subtxt">(ex) 02-1234-5678</span>
                                    </td>
                                    <th><%=GetResource("EnforceSubTit05")%></th>
					                <td>
                                        <cc3:ClientSmartTextBox ID="ctbFaxBase" runat="server" InputMode="None" CssStyle="width:120px !important;"></cc3:ClientSmartTextBox>&nbsp;
                                        <span class="app_subtxt">(ex) 02-1234-5678</span>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit06")%></th>
					                <td colspan="3"><cc3:ClientSmartTextBox ID="ctbHomepage" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSendType")%></th>
                                    <td><asp:DropDownList ID="ddlSendType" runat="server"></asp:DropDownList></td>
                                    <th><%=GetResource("EnforceQuantity")%></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbQuantity" runat="server" InputMode="None" CssStyle="width:60px !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr class="clsMail">
                                    <th><%=GetResource("EnforceTaregetMail")%></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbTargetMail" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr class="clsMail">
                                    <th><%=GetResource("EnforceTaregetMailSubject")%></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbTargetMailSubject" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceDocType")%></th>
                                    <td><asp:DropDownList ID="ddlDocType" runat="server"></asp:DropDownList></td>
                                    <th><%=GetResource("EnforceStampType")%></th>
                                    <td><asp:DropDownList ID="ddlStampType" runat="server"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforcePurpose")%><span class="txt_point">*</span></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbPurpose" runat="server" InputMode="None" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceFormPath")%></th>
                                    <td colspan="3"><asp:DropDownList ID="ddlFormPath" runat="server"></asp:DropDownList></td>
                                </tr>
                        
                            </tbody>
			                </table> 
                        </div><!-- end col-3 content-area-body-right -->
				    </div><!-- end row  -->
                </div>

            </div><!-- end layout_full  -->
        </div><!-- end layout_body  -->

<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>

    <asp:TextBox ID="tbEnforceStampID" runat="server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbEnforceDeptID" runat="server" Width="0"></asp:TextBox>
    <asp:TextBox ID="tbExpFirst" runat="server" Width="0"></asp:TextBox>ExpFirst
    <asp:TextBox ID="tbExpMid" runat="server" Width="0"></asp:TextBox>ExpFirst
    <asp:TextBox ID="tbExpEnd" runat="server" Width="0"></asp:TextBox>ExpFirst
    <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
    <iframe id="ifrmForm" name="ifrmForm" src="about:blank" frameborder="0" runat="server"></iframe>
</div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script src="http://dmaps.daum.net/map_js_init/postcode.v2.js"></script>
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

        var R_MSG_EnforceMsg00 = "<%=GetResource("EnforceMsg00")%>";              
        var R_MSG_EnforceMsg04 = "<%=GetResource("EnforceMsg04")%>";              //계열사정보를 입력하세요.
        var R_MSG_EnforceMsg05 = "<%=GetResource("EnforceMsg05")%>";              //표현명을 입력하세요
        var R_MSG_EnforceMsg06 = "<%=GetResource("EnforceMsg06")%>";              //주소를 입력하세요
        var R_MSG_EnforceMsg07 = "<%=GetResource("EnforceMsg07")%>";              //전화번호를 입력하세요
        var R_MSG_EnforceMsg08 = "<%=GetResource("EnforceMsg08")%>";              //팩스번호를 입력하세요
        var R_MSG_SPECIALCHAR = "<%=GetResource("MSG_Secure") %>";              //"해당 특수문자는 보안취약성 공격에 사용되므로 제한됩니다.";
        
        var R_MSG_EnforceSubject =	"<%=GetResource("MSG_EnforceSubject")%>";              //제목을 입력하세요
        var R_MSG_EnforceReceive	=	"<%=GetResource("MSG_EnforceReceive")%>";              //수신처를 입력하세요
        var R_MSG_EnforceReference	=	"<%=GetResource("MSG_EnforceReference")%>";              //참조를 입력하세요
        var R_MSG_EnforceAddress	=	"<%=GetResource("MSG_EnforceAddress")%>";              //메일주소를 입력하세요
        var R_MSG_EnforceTargetMail	=	"<%=GetResource("MSG_EnforceTargetMail")%>";              //메일제목을 입력하세요
        var R_MSG_EnforceTargetMailSubject = "<%=GetResource("MSG_EnforceTargetMailSubject")%>";              //사용목적을 입력하세요

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/Add.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/Add.js");
    </script>

    </form>

    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->

</body>
</html>
