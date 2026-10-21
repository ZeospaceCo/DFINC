<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Folder.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
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
        var TypeInfo = "";
        var FolderPath = "";

        var R_Msg_ErrSelFolder = "<%=GetResource("MSG_SelFolder")%>";                 //"폴더를 선택하세요.";
        var R_Msg_Del = "<%=GetResource("ConfirmDeleteFavoritePerson")%>";            //"삭제하시겠습니까?";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>";                        //"처리되었습니다.";
        var R_MSG_ErrDelSubFolder = "<%=GetResource("MSG_DelSubFolder")%>";           //"하위폴더를 먼저 삭제하세요";
        var R_MSG_ErrDelForm = "<%=GetResource("MSG_DelForm")%>";                     //"해당 폴더에 속한 양식을 먼저 삭제하세요";
        var R_AuthName = "<%=GetResource("TargetUser")%>";                            //"대상자";
        var R_JtNoData = "<%=GetResource("NoContent")%>";                             //"내용이 없습니다.";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Folder/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Folder/List.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("ManageFolder")%></h1>
            <!-- 버튼 //-->
            <span class="app_caption"><%=GetResource("AuthText")%></span>
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
                                        <zeoAppFormFolderTree:UCAppFormFolderTreeScript ID="UCAppFormFolderTreeScript1" 
                                        SetCallbackComplete="onCallbackComplete" SetNodeSelect="onNodeSelect" UseCheckBox="false" runat="server" />                
                                    </div>

                                    <div class="bott">
                                        <ul class="btn_left">
                                            <li><a href="#" id="folderAdd" class="btn btn-sm"><%= GetResource("Btn_Add")%></a></li>
                                            <li><a href="#" id="folderMod" class="btn btn-sm"><%= GetResource("Btn_Mod")%></a></li>
                                            <li><a href="#" id="folderDel" class="btn btn-sm btn-danger"><%= GetResource("Btn_Del")%></a></li>
                                            <li><a href="#" id="folderSeq" class="btn btn-sm"><%= GetResource("Btn_Seq")%></a></li>
                                        </ul>
                                        <ul class="btn_right">
                                            <li><a href="#" id="folderSet" class="btn btn-sm"><%= GetResource("Btn_Set")%></a></li>
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

<div style="display:none;">
    
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>

</body>
</html>
