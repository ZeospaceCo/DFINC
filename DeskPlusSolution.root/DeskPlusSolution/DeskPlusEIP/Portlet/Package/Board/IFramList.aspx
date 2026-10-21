<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IFramList.aspx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.IFramList" EnableViewState="false" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Package.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Package.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    
    <script type="text/javascript">

        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        
        var CatID = "<%=CatID %>";
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "7"; //보여줄게시물수    
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_ProcessingError = "처리시 오류가 발생되었습니다. 관리자에게 문의해주시기 바랍니다.";
        var R_NoContent = "내용이 없습니다.";
        var R_ErrorConnection = "서버와의 통신이 실패했습니다. :  ";
        var R_FirstPage = "첫 페이지 입니다.";
        var R_LastPage = "마지막 페이지 입니다.";
        
    </script>  
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Portlet/Js/Package/Board/IFramList.js");
    </script>  
</head>
<body>
    <form id="form1" runat="server">        
        <div style="padding:15px 0px 0px 0px;"> 
            <ul style="margin:0px 0px 10px 0px;border:0px solid #dedbdb;position:relative;padding:6px 10px 6px 10px;">
                <div id="plList" style="min-height:140px;border:0px solid green;line-height:20px"></div>
            </ul>
            <div class="ptl_more" style="border:0px solid pink;text-align:left;padding:0px 0px 0px 15px;">
                <a onclick="List.PageChange('before')"><img src="<%=BaseImgPath%>/Main/ico_b.gif" alt="<%=GetResource("Prev")%>" title="<%=GetResource("Prev")%>" style="vertical-align:text-bottom" /></a>
                <a onclick="List.PageChange('next')"><img src="<%=BaseImgPath%>/Main/ico_a.gif" alt="<%=GetResource("Next")%>" title="<%=GetResource("Next")%>" style="vertical-align:text-bottom" /></a>
            </div>
        </div>
    </form>
</body>
</html>
