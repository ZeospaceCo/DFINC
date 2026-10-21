<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListModify.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Favorite.ListModify" %>
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
        <div class="inside">

            <div id="popup_header">
                <h1><%=GetResource("Msg_Favorite_Title")%></h1>
                <ul class="btn_right">
                    <li><div id="loading"></div></li>
                    <li id="DelAll" style="display:none"><a href="#" id="Del" class="btn btn-danger"><span><%=GetResource("DelAll")%></span></a></li>
                    <li><a href="#" id="Close" class="btn"><span><%=GetResource("Close")%></span></a></li>
                </ul>            
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:170px;">

                <div class="layout_scroll"  style="min-height:230px;height:230px;"> 
                    <!-- 테이블 시작 //-->
                    
                    <div id="placeholder" style="clear: both;"></div>
                    
                    <!-- 테이블 끝 //-->
                
                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>
                    </div>
                </div>
                <!-- 페이징 끝 //-->
                <span class="app_subtxt"><%=GetResource("Msg_Favorite_Alert")%></span>

            </div>                
        </div>
    </div>

    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
    </div>
    <!-- ==================================================================================== -->

    </form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>           
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = 5; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬      
        
        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>"; 
        var R_ConfirmDeleteFavoritePerson = "<%=GetResource("ConfirmDeleteFavoritePerson") %>"; 
        var R_CheckDeleteFavoritePerson = "<%=GetResource("CheckDeleteFavoritePerson") %>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_DutyName = "<%=GetResource("DutyName") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_AddDate = "<%=GetResource("AddDate") %>"; 
        var R_NoData = "<%=GetResource("NoContent") %>"; 

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
          
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Favorite/ListModify.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Favorite/ListModify.js");
    </script>
    
</body>
</html>
