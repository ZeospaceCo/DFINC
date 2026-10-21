<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Content.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body>
<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <h1>교육보기</h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnComplete" runat="server" BeginEndTag="li" Text="수료증 등록 및 완료" Class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
            </ul>
        </div>

        <div id="layout_content">           
            <div class="row">
				<div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   
                        <div id="toPrint">                 
                        <!--기본정보-->
                        <table class="table_typeB">
                            <caption>List</caption>
		                    <colgroup>
			                    <col width="80px" />
                                <col />
                                <col width="80px" />
                                <col />
		                    </colgroup>
		                    <tbody>
                                <tr>
                                    <th>분류</th>
                                    <td colspan="3"><asp:Literal ID="ltlItem" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>완료여부</th>
                                    <td><asp:Literal ID="ltlComplete" runat="server"></asp:Literal></td>
                                    <th>진행상태</th>
                                    <td><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>기간</th>
                                    <td colspan="3"><asp:Literal ID="ltlDate" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>완료일</th>
                                    <td colspan="3"><asp:Literal ID="ltlCompleteDate" runat="server" Mode="Encode"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>과정명</th>
                                    <td colspan="3"><asp:Label title="" ID="lbTitle" runat="server"></asp:Label></td>
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
                        <!--첨부파일-->
                        <li class="title">첨부파일</li>  
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        <!--첨부파일-->
                     </ul>                     
                    <ul>
                        <li class="title">내 수료증</li>
                        <li><div id="placeholder_certificate" style="clear: both;"><asp:Literal ID="ltlCertificateFile" runat="server"></asp:Literal></div></li>
                    </ul>
                                       
	            </div><!-- end col content-area-body-right --> 
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->

</form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="auto" src="about:blank" title=""></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.resize.js"></script>
 
    <script type="text/javascript">

        var HasEducation = <%=HasEducation ? "true" : "false" %>;
        var ListID = "<%= System.Web.HttpUtility.JavaScriptStringEncode(ListID.ToString()) %>";
        var CurrentPage = "<%=System.Web.HttpUtility.JavaScriptStringEncode(CurrentPage.ToString()) %>";
        var SearchItem = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchItem) %>";
        var SearchStatus = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchStatus) %>";
        var SearchComplete = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchComplete) %>";
        var SearchDateField = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchDateField) %>";
        var SearchDateValue = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchDateValue) %>";
        var SearchField = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchField) %>";
        var SearchValue = "<%=System.Web.HttpUtility.JavaScriptStringEncode(SearchValue) %>";
     
    </script>
    <!--script type="text/javascript" src="/DeskPlusEIP/Education/Js/Content/View.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Content/View.js");
    </script>
</body>
</html>
