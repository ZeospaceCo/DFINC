<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetCategoryInfo.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Help.GetCategoryInfo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
</head>
<body style="background:none transparent;">
    <form id="form1" runat="server">
        <!-- 테이블 상단환경 -->
    <div class="layer_popup">
    
        <div class="inside" style="min-height:350px;height:350px;">
            <div class="popup_header">
                <h1><%=GetResource("CategoryHelp")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"   /></li>
                </ul>
            </div>
            <div class="dotLine"></div>            

            <div style="padding:5px">
                <div id="placeholder" style="clear: both;"></div>
            </div>
            
            <div id="Pager1" name="Pager1" class="paging_num_s1">
                <h3><span id="pageInfo"></span></h3>
                <span id="pager"></span>
                <select class="aRight form-control" id="ddlListCount" style="display:none">
                    <option selected="selected" value="5">5 </option>
                    <option value="10">10</option>
                    <option value="20">20</option>
                    <option value="30">30</option>
                </select>
            </div>
        </div>   
    </div>        
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "0"; //게시물수
        var RecordPerPage = "5"; //보여줄게시물수
        var Order = ""; //정렬
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
        var ImagePath = "<%=BaseImgPath%>"; 
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_TextEmpty = "<%=GetResource("TextEmpty") %>";
        var R_Name = "<%=GetResource("CategoryName") %>";
        var R_Color = "<%=GetResource("LayoutColor") %>";
        var R_Active = "<%=GetResource("Active") %>";
    </script> 
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Help/GetCategoryInfo.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Help/GetCategoryInfo.js");
    </script>
</body>
</html>
