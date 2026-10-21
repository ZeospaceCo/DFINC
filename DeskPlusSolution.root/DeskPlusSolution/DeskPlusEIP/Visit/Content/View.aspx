<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.View" %>
<!DOCTYPE html>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1><%=GetResource("VisitReservation")%></h1>   
            <ul class="btn_right">
                 <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />

                 <cc3:ClientSmartButton ID="cbtnMod" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                 <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" Class="btn btn-danger" />
                 <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li"  />
            </ul>
            <ul class="btn_right" id="accept" runat="server" style="display:none">
                <cc3:ClientSmartButton ID="cbtnAccept" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
            </ul>
            <ul class="btn_right" id="unaccept" runat="server" style="display:none">
                <cc3:ClientSmartButton ID="cbtnUnaccept" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->
                    <div id="toPrint">
                    <!--기본정보-->
                    <table class="table_typeB">
                    <caption>View</caption>
		            <colgroup>
			            <col width="80px" />
                        <col width="300px" />
                        <col width="80px" />
                        <col />
		            </colgroup>
		            <tbody>
                        <tr>
                            <th><%=GetResource("Subject") %></th>
                            <td colspan="3"><asp:Label title="" ID="lbSubject" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Date") %></th>
                            <td><asp:Label title="" ID="lbDate" runat="server"></asp:Label></td>
                            <th><%=GetResource("Subscriber")%></th>
                            <td><asp:Label title="" ID="lbWriteName" runat="server"></asp:Label></td>
                        </tr>
                    </tbody>
                    </table>
                    <!--기본정보-->
            
                    <!--본문정보-->
                    <div class="content_box"  style="overflow:auto;">
                    <table cellspacing="0" style="min-height:300px;width:100%;">
                        <tr>
                            <td valign="top">
                                <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    </div>
                    <!--본문정보-->
                    </div>
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                    <ul>
                        <!-- 기본공유자 -->
                        <li class="title">
                             <%=GetResource("Share")%>
                        </li>
                        <li><div id="phShare" style="clear: both;"></div></li>    
                        <!-- 기본공유자 -->

                        <!-- 공유자 -->
                        <li class="title">
                             <%=GetResource("ShareUser")%>
                        </li>
                        <li><div id="phOpenList" style="clear: both;"></div></li>    
                        <!-- 공유자 -->

                        <!-- 예약 자원 -->
                        <li class="title">
                             <%=GetResource("ReserveResource")%>
                        </li>
                        <li><div id="phRm" style="clear: both;"></div></li>    
                        <!-- 예약 자원 -->
                         
                    
                       <!-- 첨부 파일 -->
                        <li class="title">
                            <%=GetResource("Attach")%>
                        </li>
                        <li><div id="phFile" style="clear: both;"></div></li>   
                        <!-- 첨부 파일 -->
                    </ul> 
                </div>
            </div>
        </div>
    </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
        <asp:TextBox ID="hdRepeatSel" Style="display: none" runat="server" Width="0px"></asp:TextBox>    
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" >
        var ReserveID = "<%=ReserveID%>";
        var ReserveGroupID = "<%=ReserveGroupID%>";
        var TagGubun = "<%=TagGubun %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var TmpDate = "<%=TmpDate%>";
        var PreLang = "<%=PreLang%>";
        var VisitType = "<%=VisitType%>";
        var ShareCheck = "<%=ShareCheck%>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Download = "<%=GetResource("Download") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";        
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_ConfirmDel = "<%=GetResource("ConfirmDel") %>";
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";
        var R_Qty = "<%=GetResource("Qty") %>";
        var R_ResourcesUser = "<%=GetResource("ResourcesUser") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_Gubun  = "<%=GetResource("Gubun") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";            
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/View.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Visit/Js/Content/View.js");
    </script>
</body>
</html>
