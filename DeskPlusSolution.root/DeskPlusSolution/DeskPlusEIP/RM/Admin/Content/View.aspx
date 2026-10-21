<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Admin.Content.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("ResourcesView") %></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server"></cc3:ClientSmartButton></li>

                    <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnList" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_content">            
                <div class="row">
				    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->
                        <div id="toPrint">
                        <!-- 기본정보 -->
                        <table class="table_typeB">
                            <caption><%=GetResource("MenuAdmin")%></caption>
                            <colgroup>
                                <col width="120px" />
                                <col />
                                <col width="120px" />
                                <col width="100px" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("FolderName")%></th>
                                    <td><asp:Literal ID="ltFolderName" runat="server"></asp:Literal></td>
                                    <th><%=GetResource("QRCodeTitle")%></th>
                                    <td><asp:Literal ID="ltQRCode" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("ResourcesTitle")%>"><%=GetResource("ResourcesTitle")%></label></th>
                                    <td><asp:Literal ID="ltName" runat="server"></asp:Literal></td>
                                    <th><label title="<%=GetResource("Qty")%>"><%=GetResource("Qty")%></label></th>
                                    <td><asp:Literal ID="ltCount" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("Code")%>"><%=GetResource("Code")%></label></th>
                                    <td><asp:Literal ID="ltCode" runat="server"></asp:Literal></td>
                                    <th><label title="<%=GetResource("UsableQty")%>"><%=GetResource("UsableQty")%></label></th>
                                    <td><asp:Literal ID="ltReCount" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("Place")%>"><%=GetResource("Place")%></label></th>
                                    <td><asp:Literal ID="ltPlace" runat="server"></asp:Literal></td>
                                    <th><label title="<%=GetResource("Authority")%>"><%=GetResource("Authority")%></label></th>
                                    <td><asp:Literal ID="ltAdminConfirm" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("IconName")%>"><%=GetResource("IconName")%></label></th>
                                    <td colspan="3" style="padding:10px">
                                    <%=IconName%>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label title="<%=GetResource("Content")%>">
                                            <%=GetResource("Content")%></label>
                                    </th>
                                    <td colspan="3">
                                        <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <!-- 기본정보 -->
                        </div><!-- end toPrint -->
                    </div><!-- end col content-area-body-right -->
                    <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                        <ul>
                            <!-- 이미지 파일 -->
                            <li class="title">
                                <%=GetResource("ImageFile")%>
                            </li>
                            <li><div id="phFile" style="clear: both;"></div></li>   
                            <!-- 이미지 파일 -->

                            <!-- 자원 담당자 -->
                            <li class="title">
                                <%=GetResource("ResourcesUser")%>
                                <em> (<%=GetResource("ResourcesUserStr")%>) </em>            
                            </li>               
                            <li><div id="phUser" style="clear: both;"></div></li>                         
                            <!-- 자원 담당자 -->

                            <!-- QRCode -->
                            <li class="title" id="QRCode" runat="server">
                                QRCode
                            </li>
                            <li id="QRCodeList" runat="server"><ul class="table_typeT ul_typeT"><%=QRCodeContent%></ul></li>   
                            <!-- QRCode -->
                        </ul>
				</div><!-- end col content-area-body-right -->
                    
			</div><!-- end row  -->		
		</div><!-- end layout_content  -->
        
	</div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>     
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        var CategoryID = "<%=CategoryID%>";
        var ListID = "<%=ListID%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var SearchTerm = "<%=SearchTerm %>";   //날짜검색키워드
        var SearchTermValue = "<%=SearchTermValue %>";   //기간검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //정렬 
        var FullDomainPath = "<%=FullDomainPath %>";      
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Download = "<%=GetResource("Download") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DelList = "<%=GetResource("DelList") %>";
        var R_ManagerGubun = "<%=GetResource("ManagerGubun") %>";
        var R_Name = "<%=GetResource("Name") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Admin/Content/View.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Admin/Content/View.js");
    </script>
</body>
</html>
