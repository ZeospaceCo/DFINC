<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Preview" %>
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
<body  scroll="auto">
    <form id="form1" runat="server">

    <div id="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1><span><%=GetResource("Preview")%></span></h1>
                <span class="app_caption">* <%=GetResource("EnforcePreviewMsg1")%></span>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>

            <div class="dotLine"></div>
            <div class="popup_layout_style3">

                    <!-- 양식로드 //-->
                    <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>

            </div>

        </div>
    </div>	
    <div style="display:none">
        <textarea id="htaTemplate" style="DISPLAY: none" name="htaTemplate" rows="2" cols="40" runat="server"></textarea>  
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
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var EnforceID = "<%=EnforceID%>";   // 공문신청ID
        var FormPath = "<%=FormPath%>";     // 시행문템플릿경로
        var StampPath = "";  // 직인경로
        var ifrmObj = null;

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/Common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/Preview.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/Common.js");    //시행문 내용변환 (커스터마이징 용도)
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/Preview.js");
    </script>

</body>
</html>
