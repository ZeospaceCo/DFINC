<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Education.Web.Admin.View" %>
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
                                    <th>대상자</th>
                                    <td colspan="3"><asp:Literal ID="ltlReceive" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>기간</th>
                                    <td colspan="3"><asp:Literal ID="ltlDate" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th>팝업설정</th>
                                    <td><asp:Literal ID="ltlPopUpYN" runat="server"></asp:Literal></td>
                                    <th>알람</th>
                                    <td><asp:Literal ID="ltlAlarmDay" runat="server"></asp:Literal></td>
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
                    <asp:Panel ID="pnlCertificates" runat="server" Visible="false">
                        <ul>
                            <li class="title">대상자 수료증</li>
                            <li>
                                <asp:Literal ID="ltlCertificateEmpty" runat="server" Mode="Encode" />
                                <asp:Repeater ID="rpCertificates" runat="server">
                                    <HeaderTemplate>
                                        <table class="table_typeB" style="width:100%;table-layout:fixed;">
                                            <caption>대상자 수료증</caption>
                                            <colgroup><col style="width:25%;" /><col /><col style="width:30%;" /></colgroup>
                                            <thead><tr><th scope="col">등록자</th><th scope="col">파일명</th><th scope="col">등록일</th></tr></thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="overflow-wrap:break-word;"><%# CertificateUserName((Zeospace.DeskPlus.Education.BE.TOAttachInfo)Container.DataItem) %></td>
                                            <td style="overflow-wrap:break-word;"><asp:HyperLink runat="server"
                                                Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("FileName"))) %>'
                                                NavigateUrl='<%# CertificateDownloadUrl(Convert.ToInt32(Eval("UserID")), Convert.ToString(Eval("FileGUID"))) %>' /></td>
                                            <td><%# Convert.ToDateTime(Eval("WriteDate")).ToString("yyyy-MM-dd HH:mm") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate></tbody></table></FooterTemplate>
                                </asp:Repeater>
                            </li>
                        </ul>
                    </asp:Panel>
                                       
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
       
        var HasEducation = <%= HasEducation ? "true" : "false" %>;
        var ListID = "<%= ListID %>";
        var SearchDateValue = "<%=HttpUtility.JavaScriptStringEncode(SearchDateValue) %>";
        var SearchTarget = "<%=HttpUtility.JavaScriptStringEncode(SearchTarget) %>";
        var SearchItem = "<%=HttpUtility.JavaScriptStringEncode(SearchItem) %>";
     
    </script>
    <!--script type="text/javascript" src="/DeskPlusEIP/Education/Js/Admin/View.js"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Education/Js/Admin/View.js");
    </script>
</body>
</html>
