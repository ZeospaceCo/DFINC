<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Main.Content" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body" class="write">
    <div class="layout_full">
        <div class="cm_main_cont">
            <div class="main">
                <img src="<%=BaseImgPath%>/Community/cm_main_img.jpg" alt="" title="" />
            </div>
        </div>

        <div id="layout_srh" class="layout_srh clearfix">
            <div class="left">
                <ul class="tab" style="margin-left:30px !important">
                     <li><a href="javascript:Content.SetTab('1');" class="tab_on"  id="tabDivInfo"><%=GetResource("ComList")%></a></li>
                     <li class="last"><a href="javascript:Content.SetTab('2');" id="tabDivJoin"><%=GetResource("JoinComList")%></a></li>
                </ul>
            </div>

            <div class="right">
                <ul class="btn_searchbox">
                     <li><cc3:ClientSmartButton ID="cbtnAdmin" runat="server"  class="btn btn-secondary"  /></li>
                </ul>
            </div>
        </div>

        <div id="layout_content"><!-- 컨텐츠 하단영역 -->
            <div class="cm_main_contsub">
                <div id="divInfo">   
                     <div id="phList" style="clear: both;"></div>
                </div>
                <div id="divJoin" style="display:none">    
                     <div id="phJoin" style="clear: both;"></div>
                </div>   

                <!-- 페이징 시작 //-->
                <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                    <h3><span id="pageInfo"></span></h3>
	                <span id="pager"></span>
                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                        <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                        <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                        <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                        <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <!-- 페이징 끝 //-->

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
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬      
        var ImagePath = "<%=BaseImgPath%>";
        var TabGubun = "1";
        var CommunityID = "";
        var Grade = "";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_CommunityName = "<%=GetResource("CommunityName") %>";
        var R_ComWriteName = "<%=GetResource("CommunitySys") %>";
        var R_CommunityDes = "<%=GetResource("CommunityDes") %>";
        var R_CommunityType = "<%=GetResource("CommunityType") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ApplyDate = "<%=GetResource("ApplyDate") %>";
        var R_JoinDes = "<%=GetResource("JoinDes") %>"; 
        var R_JoinName = "<%=GetResource("JoinName") %>"; 
        var R_Cancel = "<%=GetResource("Cancel") %>";      
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";   
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_DelJoin =  "<%=GetResource("DelJoin") %>";
        var R_Membership = "<%=GetResource("Membership") %>";
        var R_TodayCount = "<%=GetResource("TodayCount") %>";
        var R_Del = "<%=GetResource("Del") %>";  
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Main/Content.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Main/Content.js");
    </script>
</body>
</html>
