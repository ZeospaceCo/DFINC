<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approve.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.EnforceAdmin.Approve" %>
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
                    <h1><%=GetResource("EnforceConfirm")%></h1>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                        <cc3:ClientSmartButton ID="CbtnCancel" runat="server" BeginEndTag="li" />
                    </ul>
                    <ul class="btn_right" id="ulApprove">
                        <cc3:ClientSmartButton ID="cbtnApprove" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="cbtnReject" runat="server" BeginEndTag="li"  />
                    </ul>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnPreview" runat="server" BeginEndTag="li" />
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
				                    <th><%=GetResource("EnforceSubTit01")%></th>
					                <td colspan="3"><asp:Label ID="lbDept" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("DocNumber")%></th>
                                    <td colspan="3"><asp:Label ID="lbDocNumber" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Subject")%></th>
                                    <td colspan="3"><asp:Label ID="lbSubject" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("TagDic_ENFORCEDEPTNAME")%></th>
                                    <td colspan="3"><asp:Label ID="lbReceive" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("TagDic_ENFORCERECEIVECARBON")%></th>
                                    <td colspan="3"><asp:Label ID="lbReference" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceDate")%></th>
                                    <td colspan="3"><asp:Label ID="lbEnforceDate" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSubTit03")%></th>
                                    <td colspan="3">
                                        <asp:Label ID="lbZipCode" runat="server" Width="60px"></asp:Label>
                                        <asp:Label ID="lbAddressFirstBase" runat="server" Width="40%"></asp:Label>
                                        <asp:Label ID="lbAddressEndBase" runat="server" Width="45%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit04")%></th>
					                <td><asp:Label ID="lbPhoneBase" runat="server"></asp:Label></td>
                                    <th><%=GetResource("EnforceSubTit05")%></th>
					                <td><asp:Label ID="lbFaxBase" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
				                    <th><%=GetResource("EnforceSubTit06")%></th>
					                <td colspan="3"><asp:Label ID="lbHomepage" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceSendType")%></th>
                                    <td><asp:Label ID="lbSendType" runat="server"></asp:Label></td>
                                    <th><%=GetResource("EnforceQuantity")%></th>
                                    <td><asp:Label ID="lbQuantity" runat="server"></asp:Label></td>
                                </tr>
                                <tr class="clsMail">
                                    <th><%=GetResource("EnforceTaregetMail")%></th>
                                    <td colspan="3"><asp:Label ID="lbTargetMail" runat="server"></asp:Label></td>
                                </tr>
                                <tr class="clsMail">
                                    <th><%=GetResource("EnforceTaregetMailSubject")%></th>
                                    <td colspan="3"><asp:Label ID="lbTargetMailSubject" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceDocType")%></th>
                                    <td><asp:Label ID="lbDocType" runat="server"></asp:Label></td>
                                    <th><%=GetResource("EnforceStampType")%></th>
                                    <td><asp:Label ID="lbStampType" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforcePurpose")%></th>
                                    <td colspan="3" class="app_breakB"><asp:Label ID="lbPurpose" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("EnforceFormPath")%></th>
                                    <td colspan="3"><asp:Label ID="lbFormPath" runat="server"></asp:Label></td>
                                </tr>
                        
                            </tbody>
			                </table> 
                        </div><!-- end col-3 content-area-body-right -->
				    </div><!-- end row  -->
                </div><!-- end layout_content  -->

            </div><!-- end layout_full  -->
        </div><!-- end layout_body  -->

<div style="display:none;">
    <asp:DropDownList ID="ddlDept" runat="server" Width="0px"></asp:DropDownList>
    <asp:DropDownList ID="ddlSendType" runat="server" Enabled="false" Width="0px"></asp:DropDownList>
    <asp:DropDownList ID="ddlDocType" runat="server" Width="0px"></asp:DropDownList>
    <asp:DropDownList ID="ddlStampType" runat="server" Width="0px"></asp:DropDownList>
    <asp:DropDownList ID="ddlFormPath" runat="server" Width="0px"></asp:DropDownList>
    <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
    <iframe id="ifrmForm" name="ifrmForm" src="about:blank" frameborder="0" runat="server"></iframe>
    <asp:TextBox ID="tbOpinion" runat="server" Text = ""></asp:TextBox>
</div>

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
        var EnforceID = "<%=EnforceID%>";
        var EnforceStampID = "<%=EnforceStampID%>";
        var StampDeptID = "<%=StampDeptID%>";
        var State  = "<%=State%>";
        var ReqUserID = "<%=ReqUserID %>";

        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL")%>";
        var R_Msg_Del = "<%=GetResource("Msg_Del")%>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/Approve.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/Approve.js");
    </script>

</body>
</html>
