<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Signature.List" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("ManageStamp")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><a href="#" id="Sign" class="btn btn-secondary"><span><%=GetResource("Sign")%></span></a></li>
                <li><a href="#" id="Add" class="btn btn-secondary"><span><%=GetResource("AddStampFile")%></span></a></li>
                <!--li><a href="#" id="Del" class="btn btn-danger"><span>삭제</span></a></li-->
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                
                <div class="col-12 content-area-body">
                    <div style="min-height:300px;border:1px solid #dddddd;">
                    <center>
                    <div id="drawingSignImage" style="border:solid 1px black;margin-top:100px;height:100px;width:100px">
                    <img id="SignImage" alt="SignImage" src="GetImage.ashx?p=<%=SignImage%>" style="height:95px;width:95px;" />
                    </div>
                    </center>
                    </div>
                    <span class="app_subtxt"><%=GetResource("MSG_Stamp")%></span>
                </div><!-- end col-12 content-area-body -->
                
            </div><!-- end row  -->
        </div><!-- end layout_content  -->
        
    </div><!-- end layout_full  -->
</div><!-- end ifm_layout_body  -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif)";

        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Signature/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Signature/List.js");
    </script>

</body>
</html>
