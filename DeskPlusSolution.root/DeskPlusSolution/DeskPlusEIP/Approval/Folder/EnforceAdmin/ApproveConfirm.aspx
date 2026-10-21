<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveConfirm.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.EnforceAdmin.ApproveConfirm" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("EnforceConfirm")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="height:270px">
            <span id="appmsg" class="app_captit" style="float:left;" ></span>
            <table class="table_typeB">
                <caption><%=GetResource("EnforceConfirm")%></caption>
				<colgroup>
					<col width="80px" />
					<col />
				</colgroup>
				<tbody>
					<tr>
						<th><%=GetResource("Opinion")%> (<span class="charsRemaining"></span><%=GetResource("InChar")%>)</th>
						<td><asp:TextBox ID="tbOpinion" runat="server" TextMode="MultiLine" Width="93%" Height="210px" MaxLength="200"></asp:TextBox></td>
					</tr>
				</tbody>
			</table> 

            
            
        </div> 
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script> 
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var FullDomainPath = "<%=FullDomainPath %>";

        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
        var R_EnforceSendMsgReject = "<%=GetResource("EnforceSendMsgReject") %>";
        var R_EnforceSendMsgApprove = "<%=GetResource("EnforceSendMsgApprove") %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/ApproveConfirm.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/EnforceAdmin/ApproveConfirm.js");
    </script>
</body>
</html>
