<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMain.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.ListMain" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />   
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Core_Popup.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Core_Popup.css" />   
</head>
<body>
<!-- ############# start of modal ############## -->
<div id="zeoModalContact">
    <!-- 드래그공간(br Tag 처리) //-->        
    <div id="zeoModalDrag" class="modal-drag"><p></p></div>
    <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
    <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
</div>
<div id="zeoModalBG"></div>
<!-- ############# end of modal ############## -->
    
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <!-- 버튼 //-->
        <div id="layout_header">
            <h1><%=SiteMap%></h1>
            <ul class="btn_right">
            <cc3:ClientSmartButton ID="cbtnAdmin" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
            <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>

            <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" Class="btn btn-secondary" ></cc3:ClientSmartButton>
            </ul>
        </div>

        <!-- 검색 //-->       
        <div id="layout_srh" class="layout_srh clearfix">
            <div>
                <ul class="tab">
                    <li style="width:90px"><a href="#" id="aDay" onclick="ListMain.TabPageMove('day')" ><%=GetResource("Day")%></a></li>
                    <li style="width:90px"><a href="#" id="aWeek" class="tab_on" onclick="ListMain.TabPageMove('week')"><%=GetResource("Week")%></a></li>
                    <li style="width:90px" class="last"><a href="#" id="aMonth" onclick="ListMain.TabPageMove('month')" ><%=GetResource("Month")%></a></li>
                </ul>
                <ul id="TypeGubun"  class="btn_searchImg" style="margin-left:310px; margin-top:5px">
                    <li id="list"><a id="listType1" href="#" class="board" title="<%=GetResource("Type1")%>"><i class="fas fa-list-ul"></i></a></li>
                    <li id="whole"><a id="listType2" href="#" class="blog" title="<%=GetResource("Type2")%>"><i class="fas fa-th-large"></i></a></li>
                </ul>
            </div>
            
            <div class="right">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>               
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                    <li><cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox></li>
                    <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" BeginEndTag="li" Class="searchbtn btn-secondary" /></li>  
                </ul>
            </div>
        </div>
        
        <div id="layout_content">
                <div class="row"> 
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <iframe id="frmContent1" name="frmContent1" class="pane ui-layout-center" width="100%" frameborder="0" scrolling="no"></iframe>    
                    </div><!-- end col content-area-body-right -->
                        
                </div><!-- end row  -->
        </div><!-- end layout_content  -->
        
    </div><!-- end layout_full  -->
</div><!-- end layout_body  -->
        
        
        
</form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/iframeResizer.min.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID%>";
        var TagGubun = "<%=TagGubun %>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var TmpDate = "<%=TmpDate%>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var GroupGubun = "<%=GroupGubun %>";
        var CntYN = "<%=CntYN%>";
        var RmCnt = "<%=RmCnt%>";
        var FullDomainPath = "<%=FullDomainPath %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/ListMain.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/RM/Js/Content/ListMain.js");
    </script>    
    <script type="text/javascript">
        $(function () {
            $("iframe").iFrameResize({
                log: false,                  // Enable console logging
                enablePublicMethods: true,                  // Enable methods within iframe hosted page
                resizedCallback: function (messageData) { // Callback fn when resize is received
                },
                messageCallback: function (messageData) { // Callback fn when message is received
                },
                closedCallback: function (id) { // Callback fn when iFrame is closed
                }
            });
        })
    </script>
</body>
</html>
