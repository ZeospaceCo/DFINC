<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuesAdd.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesAdd" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
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
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MenuID      = "<%=MenuID%>";
        var PageSize    = "<%=PageSize%>";  //보여질 게시물수
        var PageNo      = "<%=PageNo %>";   //현재페이지
        var SKey        = "<%=SKey %>";     //검색키워드
        var SVal        = "<%=SVal %>";     //검색어
        var SPeriod     = "<%=SPeriod%>";
        var SPeriodVal  = "<%=SPeriodVal%>";
        var Order       = "<%=Order %>";    //정렬
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var SwfUploadGUID = "<%=SwfUploadGUID %>";  //flash upload GUID
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        //리소스
        var R_FileName  = "<%=GetResource("FileName") %>";
        var R_FileType  = "<%=GetResource("FileType") %>";
        var R_FileLen   = "<%=GetResource("FileLen") %>";
        var R_FileDel   = "<%=GetResource("Del") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Delete    = "<%=GetResource("Msg_Del") %>";
        var ConfirmEditorTab    = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX          = "<%=NonActiveX%>";
        var ClientQueryString   = "<%=Page.ClientQueryString %>";
        var R_Msg_ErrCont       = "<%=GetResource("Msg_ErrCont") %>";
        var R_Msg_ErrTitle      = "<%=GetResource("Msg_ErrTitle") %>";
        var R_Msg_ErrSDate      = "<%=GetResource("Msg_ErrSDate") %>";
        var R_Msg_ErrEDate      = "<%=GetResource("Msg_ErrEDate") %>";
        var R_Msg_ErrSupDept    = "<%=GetResource("Msg_ErrSupDept") %>";
        var R_Msg_ErrDate       = "<%=GetResource("Msg_ErrDate") %>";
        var R_JTNoData          = "<%=GetResource("JTNoData") %>";
        var R_Modify = "<%=GetResource("Modify") %>";
        var R_Del = "<%=GetResource("Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/QuesAdd.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/QuesAdd.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body" class="write">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("TitF20") %><%=PageTitle%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
        <div class="row">
           <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!-- width 100%(col-12) -->
                                          
            <!-- 설문정보 시작 //-->
            <table class="table_typeB">
			<caption><%=GetResource("TitInfo")%></caption>
			<colgroup>
                <col width="80px" />
                <col />
			</colgroup>
			<tbody>
				<tr>
					<th><%=GetResource("TitF1") %></th>
					<td><cc3:ClientSmartTextBox ID="ctbTitle" InputMode="None" MaxLength="200" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
				</tr> 
				<tr>
					<th><%=GetResource("TitF2") %></th>
					<td id="tdDate">
                        <cc3:ClientSmartTextBox ID="ctbSDate" InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                        ~
                        <cc3:ClientSmartTextBox ID="ctbEDate" InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                        <label class="opn_subtxt"><%=GetResource("TitF18") %></label>
                    </td>
				</tr> 
                <tr>
                    <th><%=GetResource("TitF3") %></th>
                    <td>
                        <span class="aLeft">
                            <cc3:ClientSmartTextBox ID="ctbDeptNm" InputMode="None" runat="server" Required="true" ReadOnly="true" CssStyle="width:170px !important;"></cc3:ClientSmartTextBox>
                            (<cc3:ClientSmartTextBox ID="ctbDeptID" InputMode="None" runat="server" Required="true" ReadOnly="true" CssStyle="width:30px !important;"></cc3:ClientSmartTextBox>)
                        </span>
                        <ul class="btn_smallL">
                            <li><cc3:ClientSmartButton ID="cbtnUptSuper" runat="server"></cc3:ClientSmartButton></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TitF4") %></th>
                    <td>
                        <ul class="aLeft">
                            <li style="display:none;"><asp:DropDownList ID="ddlGroupName" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                            <li><asp:checkbox runat="server" id="cbIsMain" Checked="true"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsReply" Checked="true"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsOpen" Checked="true"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsVoteUser" Checked="true"></asp:checkbox></li>
                            <li><asp:checkbox runat="server" id="cbIsSecret"></asp:checkbox></li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <th class="last-child"><%=GetResource("TitUnitAddF4") %></th>
                    <td class="last-child">
                        <asp:DropDownList ID="ddlIsActive" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </td>
                </tr>
			</tbody>
		    </table> 
            <!-- 설문정보 끝 //-->


            <div id="tabs" class="layout_subbox">
                <ul id="tab" class="tab">
                    <li><a id="ActvieX"  class="tab_on"><span>ActiveX</span> </a></li>
                    <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                </ul>
                <div class="divEditor">
                    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                </div>
            </div>   

            </div><!-- width 100%(col-12) -->


            <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-12) -->
            <%--<ul>
            <!-- 첨부파일 시작 //-->
            <li class="title">
                <h2><%=GetResource("TitF21") %></h2><cc3:ClientSmartButton ID="cbtnFileAdd" runat="server"></cc3:ClientSmartButton>
            </li>
           
            <li><div id="phFile" style="clear:both;"></div></li>
            
            
            </ul>--%>
            <ul> 
                <!-- 첨부파일 -->
                <li class="title">
                    <%=GetResource("TitF21")%>
                    <div class="float-right">                            
                        <a href="#" id="cbtnFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                        </div>
                </li>
                <li><div id="phFile" style="clear: both;"></div></li>
                <!-- 첨부파일 리스트 -->
            </ul>
            
            
            <!-- 첨부파일 끝 //-->
            </div><!-- end col-3 content-area-body-right -->
        </div>
        </div>

    </div>
</div>

<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
    <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" rows="1" cols="20"></textarea>
    <asp:TextBox ID="tbDeptID" runat="server" Width="0" Text="0"></asp:TextBox>
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
