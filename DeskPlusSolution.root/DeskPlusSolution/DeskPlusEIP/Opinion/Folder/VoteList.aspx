<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoteList.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Folder.VoteList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
</head>
<body>
<form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("BtnVoteUser")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:200px;height:210px;"> 

        <!-- 테이블 시작 -->
        
        <div id="placeholder" style="clear: both;"></div>
        
        <!-- 테이블 끝 -->
        </div>

        <!-- 페이징 시작 //-->
        <div id="Pager1" name="Pager1" class="paging_num_s1">
            <h3><span id="pageInfo"></span></h3>
            <span id="pager"></span>
        </div>
        <!-- 페이징 끝 //-->

    </div>       
</div><!-- //layout_info --> 	
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript">
        var TotCnt = "<%=TotCnt%>"; //총 게시물수
        var PageSize = "<%=PageSize%>"; //보여질 게시물수
        var PageNo = "<%=PageNo %>";    //현재페이지
        var QuesID = "<%=QuesID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_JTF1 = "<%=GetResource("JTF11")%>";
        var R_JTF2 = "<%=GetResource("JTF27")%>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Folder/VoteList.js"></script>     
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Opinion/Js/Folder/VoteList.js");
    </script> 
</body>
</html>
