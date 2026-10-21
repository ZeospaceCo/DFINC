<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetGarbage.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Transfer.GetGarbage" %>
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
        <div class="inside" style="min-height:325px">
            <div class="popup_header">
                <h1><%=GetResource("TransferBtnDeptDel")%></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <!-- 테이블 시작 //-->
                
                <div id="placeholder" style="clear: both;"></div>
                
                <!-- 테이블 끝 //-->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var Callback = "<%=Callback %>";

        var R_DeptID = "<%=GetResource("DeptID")%>";
        var R_DeptName = "<%=GetResource("DeptName")%>";
        var R_DeptCnt = "<%=GetResource("DocCnt")%>";   
        var R_NoContent = "<%=GetResource("NoContent")%>"; //"내용이 없습니다.";
        var R_EA = "<%=GetResource("EA")%>";   

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
    </script>   
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Transfer/GetGarbage.js"></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Transfer/GetGarbage.js");
    </script>

</body>
</html>