<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuesView.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesView" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Opinion.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Opinion.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MenuID = "<%=MenuID%>";
        var PageSize = "<%=PageSize%>";  //보여질 게시물수
        var PageNo = "<%=PageNo %>";   //현재페이지
        var SKey = "<%=SKey %>";     //검색키워드
        var SVal = "<%=SVal %>";     //검색어
        var SPeriod = "<%=SPeriod%>";
        var SPeriodVal = "<%=SPeriodVal%>";
        var Order = "<%=Order %>";    //정렬
        var QuesID = "<%=QuesID%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        //리소스
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Delete = "<%=GetResource("Msg_Del") %>";
        var R_TitReq = "<%=GetResource("TitReq") %>";
        var R_TitBig = "<%=GetResource("TitBig") %>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/QuesView.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/QuesView.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=PageTitle%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server"></cc3:ClientSmartButton></li>
                <li style="display:none"><cc3:ClientSmartButton ID="cbtnCopy" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnEdit" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
           <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!-- width 100%(col-12) -->
                                        
            <div id="prtArea">
            <!-- 설문정보 시작 //-->
            <table class="table_typeB">
			<caption><%=GetResource("TitInfo") %></caption>
			<colgroup>
                <col width="80px" />
                <col width="40%" />
                <col width="80px" />
                <col />
			</colgroup>
			<tbody>
				<tr>
					<th><%=GetResource("TitF1") %></th>
					<td colspan="3">
                        <asp:LinkButton ID="ltGroupName" runat="server"></asp:LinkButton>
                        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                    </td>
				</tr> 
				<tr>
					<th><%=GetResource("TitF2") %></th>
					<td>
                        <asp:Literal ID="ltSDate" runat="server"></asp:Literal> ~ <asp:Literal ID="ltEDate" runat="server"></asp:Literal>
                        <asp:Label ID="lbSub" runat="server" CssClass="txt_point"></asp:Label>
                    </td>
                    <th><%=GetResource("TitF3") %></th>
                    <td>
                        <span class="aLeft">
                            <asp:Literal ID="ltDeptNm" runat="server"></asp:Literal>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TitF4") %></th>
                    <td>
                        <ul class="aLeft">
                            <li><asp:checkbox runat="server" id="cbIsMain" Checked="true" Enabled="false"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsReply" Checked="true" Enabled="false"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsOpen" Checked="true" Enabled="false"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsVoteUser" Checked="true" Enabled="false"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsSecret" Checked="false" Enabled="false"></asp:checkbox></li>
                        </ul>
                    </td>
                    <th><%=GetResource("TitUnitAddF4") %></th>
                    <td><asp:Literal ID="ltIsActive" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <th><%=GetResource("TitF9") %></th>
                    <td><div class="opn_target"><asp:Literal ID="ltTarget" runat="server"></asp:Literal></div></td>
                    <th><%=GetResource("TitF10") %></th>
                    <td><div class="opn_target"><asp:Literal ID="ltAdmin" runat="server"></asp:Literal></div></td>
                </tr>
                 <tr>
                    <th><%=GetResource("TitF11") %></th>
                    <td colspan="3">
                        <div style="min-height:300px;width:100%; overflow:auto;">
                            <asp:Literal ID="ltCont" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
			</tbody>
		    </table> 
            <!-- 설문정보 끝 //-->
            <!-- 질문정보 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("TitF12") %></h2>
            </div>
            <div class="opn_unitlist"><div id="divUnitCont"></div></div>

            </div>
               </div><!-- width 100%(col-12) -->

            <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-12) -->

              <!-- 파일리스트 시작 //-->
                  <ul> 
                     <li class="title">
							    <%=GetResource("TitF13")%>
							    </li>
                                <li>
                                    <div id="phFileList" style="clear: both;"></div>
                                </li>

                  </ul>

            <!-- 파일리스트 끝 //-->

            </div><!-- end col-3 content-area-body-right -->
        </div>
       </div><!-- row -->
    </div>
</div>


<div style="display:none;">
    
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </form>
</body>
</html>
