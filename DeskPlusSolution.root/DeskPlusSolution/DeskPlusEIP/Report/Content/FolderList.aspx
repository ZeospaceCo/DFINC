<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FolderList.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.FolderList" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<%@ Register src="../Common/FolderTreeScript.ascx" tagname="FolderTreeScript" tagprefix="uc1" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
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
        var FolderID = "0";
        var ChildCnt = 0;
        var R_Msg_ErrSelFolder = "<%=GetResource("SelFolder")%>";//"폴더를 선택하세요.";
        var R_Msg_Del = "<%=GetResource("ReallyApplyDelete")%>";//"삭제하시겠습니까?";
        var R_MSG_SUCCESS = "<%=GetResource("Processing")%>";//"처리되었습니다.";
        var R_MSG_ErrDelSubFolder = "<%=GetResource("MSG_ErrDelSubFolder")%>";//"하위폴더를 먼저 삭제하세요";        
        var R_AuthName = "<%=GetResource("ReportUser")%>";//"대상자";
        var R_Gubun = "<%=GetResource("Gubun")%>";//"구분";
        var R_Del= "<%=GetResource("Del")%>";//"삭제";
        var R_NoContent = "<%=GetResource("NoContent")%>";//"내용이 없습니다.";
        var R_Dept = "<%=GetResource("Dept")%>";
        var R_User = "<%=GetResource("User")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/FolderList.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Report/Js/Content/FolderList.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("BriefingSetting")%></h1>
            <!-- 버튼 //-->
            <span class="app_caption"><%=GetResource("BriefingSetMsg")%></span>
        </div>
        <div id="layout_content">
            <div class="row">
                <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                    <div class="app_target">
                        <div class="app_fdr">
                            <table width="100%" cellspacing="0" cellpadding="0">
						    <tr>
                            <td width="260px" valign="top" style="vertical-align:top">
                                <div class="leftL" style="border:0px">
                                    <div class="cont">
                                        <uc1:FolderTreeScript ID="FolderTreeScript1" SetNodeSelect="onNodeSelect" runat="server" />
                                    </div>
                                    <div class="bott">
                                        <ul class="btn_left">
                                            <li><a href="#" id="folderAdd" class="btn btn-sm"><%= GetResource("Add")%></a></li>
                                            <li><a href="#" id="folderMod" class="btn btn-sm"><%= GetResource("Modify")%></a></li>
                                            <li><a href="#" id="folderDel" class="btn btn-sm btn-danger"><%= GetResource("Del")%></a></li>
                                            <li><a href="#" id="folderSeq" class="btn btn-sm"><%= GetResource("ChangeOrder")%></a></li>
                                        </ul>
                                        <ul class="btn_right">
                                            <li><a href="#" id="folderSet" class="btn btn-sm"><%= GetResource("Auth")%></a></li>
                                        </ul>
                                    </div>
                                 </div>
						    </td>
						    <td valign="top">
						        <div id="placeholder" style="clear: both;"></div>
						    </td>
						    </tr>
						    </table>
                        </div>
                    </div>
                </div><!-- end col content-area-body-left -->
            </div><!-- end row  -->
        </div><!-- end layout_content  -->
        
    </div>
</div>
    <div style="display: none;">
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </form>
</body>
</html>