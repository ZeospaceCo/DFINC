<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUserDept.aspx.cs" Inherits="Zeospace.DeskPlus.HR.Web.Content.ListUserDept" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_HR.css?v1=1" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_HR.css?v1=1"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css?v=1" />
</head>
<body>
    <div id="fnm" class="btn-secondary text-white"><i class="fa fa-chevron-left fa-chevron-right"></i></div>

    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("ListUserDept")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">   
                    <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li" />                 
                </ul>
            </div>            
             <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->                
                        <table cellspacing="0" id="contentTable" class="table_typeA" style="width: 100% !important;">
                            <colgroup>
                                <col />
                                <col />
                                <col />
                                <col />
                                <col />
                            </colgroup>
                            <thead>
                            <tr>
                                <th class="table_title"><%=GetResource("State")%></th>
                                <th class="table_100"><%=GetResource("BaseDeptName")%></th>
                                <th class="table_100"><%=GetResource("Name")%></th>
                                <th class="table_100"><%=GetResource("Department")%></th>
                                <th class="table_lock"><%=GetResource("Btn_Save")%></th>
                            </tr>
                            </thead>
                            <tbody>
                            <%=UserDeptTable%>
                            </tbody>
                        </table>

                    </div><!-- end col content-area-body-right -->
                        
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%= CurrentUserInfo.CompanyID %>";                      

        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>";

        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";
        var R_ProcessingError = "<%=GetResource("ProcessingError")%>";
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/HR/Js/Content/ListUserDept.js"></script>
</body>
</html>