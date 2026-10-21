<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuesMod.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesMod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
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
        var R_JTF7  = "<%=GetResource("JTF7") %>";
        var R_JTF8  = "<%=GetResource("JTF8") %>";
        var R_JTF9  = "<%=GetResource("JTF9") %>";
        var R_JTF10 = "<%=GetResource("JTF10") %>";
        var R_JTF11 = "<%=GetResource("JTF11") %>";
        var R_JTF12 = "<%=GetResource("JTF12") %>";
        var R_JTNoData = "<%=GetResource("JTNoData") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/QuesMod.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/QuesMod.js");
    </script>
</head>
<body>
<form id="form1" runat="server">
 
<div id="layout_body" class="write">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=PageTitle%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
         
        <div id="layout_content">
        <div class="row">
           <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!-- width 100%(col-12) -->
            
            <!-- 설문정보 시작 //-->
            <table class="table_typeB">
			<caption><%=GetResource("TitInfo") %></caption>
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
                            <li><asp:checkbox runat="server" id="cbIsSecret" Checked="false"></asp:checkbox></li>
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
                <ul id="tab" class="tab" style="display:none">
                    <li><a id="ActiveX"><span>ActiveX</span> </a></li>
                    <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                </ul>
                <div class="divNonEditor">
                    <div id="divXFE" style="height: 5px;">                        
                        <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>   
                        <script type="text/javascript">
                            xfe = new XFE({
                                basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                width: '100%',
                                height: '550px',
                                ignoreMinHeight: true
                            });
                            xfe.render('divXFE');
                        </script>
                    </div>
                </div>
            </div> 
            </div><!-- width 100%(col-12) -->
            <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-12) -->
                <ul> 
                    <!-- 파일리스트 시작 //-->
                    <li class="title"><%=GetResource("TitF13") %></li>
                    <li><div id="phFileList" style="clear:both;"></div></li>            
                    <!-- 파일리스트 끝 //-->           

                    <!-- 첨부파일 -->
                    <li class="title">
                        <div class="float-left"><%=GetResource("TitF19") %></div>
                        <div class="float-right">                            
                            <a href="#" id="cbtnFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                        </div>
                    </li>
                    <li><div id="phFile" style="clear: both;"></div></li>
                    <!-- 첨부파일 리스트 -->
            
                    <!-- 질문관리 시작 //-->
                    <li class="title" ID="liReply" runat="server">
                        <div class="float-left"><%=GetResource("Title4") %></div>
                        <div class="float-right">
                            <cc3:ClientSmartButton ID="cbtnUnitSeq" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>&nbsp;
                            <cc3:ClientSmartButton ID="cbtnUnitAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>&nbsp;
                        </div>
                    </li>                                   
                    <li><div id="phUnitList" style="clear:both;"></div></li>
                    <!-- 질문관리 끝 //-->            
                        
                    <!-- 대상자 시작 //-->
                    <li class="title" ID="li1" runat="server">
                        <div class="float-left"><%=GetResource("TitF9") %></div>
                        <div class="float-right">
                            <a href="#" id="cbtnTargetAdd" title="대상자추가"><i class="fas fa-plus-circle text-secondary"></i></a>
                        </div>
                    </li>            
                    <li><div id="phTarget" style="clear:both;"></div></li>            
                    <!-- 대상자 끝 //-->               
            
                    <!-- 관리자 시작 //-->
                    <li class="title">
                        <div class="float-left"><%=GetResource("TitF10") %></div>
                        <div class="float-right">
                            <a href="#" id="cbtnAdminAdd" title="관리자추가"><i class="fas fa-plus-circle text-secondary"></i></a>
                        </div>
                    </li>            
                    <li><div id="phAdmin" style="clear:both;"></div></li>            
                    <!-- 관리자 끝 //-->
                </ul>
            </div><!-- end col-3 content-area-body-right -->
            </div>

        </div>

    </div>
</div>


<div style="display:none;">
    <asp:TextBox ID="tbDeptID" runat="server" Width="0" Text="0"></asp:TextBox>
    <input type="hidden" name="contents" value="<%: Contents %>" />
    <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
    <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" rows="1" cols="20"></textarea>
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <!--  파일 첨부시 필요한 필드 //-->
    <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
    <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>
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
