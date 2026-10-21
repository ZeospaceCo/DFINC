<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Zeospace.DeskPlus.Search.Web.Admin.Index.Default" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Search.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>색인</h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnDel" Text="Del" runat="server" Class="btn btn-danger" /></li>
                <li><cc3:ClientSmartButton ID="cbtnOptimize" Text="Optimize" runat="server" Class="btn btn-secondary" /></li>
                <li><cc3:ClientSmartButton ID="cbtnAdd" Text="Add" runat="server" Class="btn btn-secondary" /></li>
            </ul>
        </div>  

        <div id="layout_content">
             <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <div id="divLoading" align="center" class="divLoading"></div>           

                    <div id="phCategory" style="clear: both;"></div>         
                        
                    <!-- 테이블 끝 //-->

                    <div class="dotLine"></div>            
                    <div id="phLogDetail" style="clear: both;"></div>        
                 </div>
              </div>
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
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
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
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬   
        var UserID = "<%=UserID %>";
        var R_SelectList = "<%=GetResource("SelectList") %>";   
        var R_ConfirmExec = "<%=GetResource("ConfirmExec") %>";   
        var R_ConfirmDel = "<%=GetResource("ConfirmDel") %>";
        var R_ConfirmOpt = "<%=GetResource("ConfirmOpt") %>";
        var R_ComfirmComplete = "<%=GetResource("ComfirmComplete") %>";
        var R_Category = "<%=GetResource("Category") %>";   
        var R_Status = "<%=GetResource("Status") %>";   
        var R_StartDate = "<%=GetResource("StartDate") %>";   
        var R_EndDate = "<%=GetResource("EndDate") %>";   
        var R_ExecTime = "<%=GetResource("ExecTime") %>";   
        var R_FTRTextCount = "<%=GetResource("FTRTextCount") %>";   
        var R_IndexCount = "<%=GetResource("IndexCount") %>";   
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Message = "<%=GetResource("Message") %>";
        var R_P = "<%=GetResource("StatusP")%>";
        var R_C = "<%=GetResource("StatusC")%>";
        var R_E = "<%=GetResource("StatusE")%>";
        var R_D = "<%=GetResource("StatusD")%>";
        var R_N = "<%=GetResource("StatusN")%>";        
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Search/Js/Admin/Index/Default.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Search/Js/Admin/Index/Default.js");
    </script>
</body>
</html>
