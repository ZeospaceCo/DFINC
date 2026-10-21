<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnitAdd.aspx.cs" Inherits="Zeospace.DeskPlus.Opinion.Web.Admin.Content.UnitAdd" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MenuID  = "<%=MenuID%>";
        var Section = "<%=Section%>";
        var TotCnt  = "<%=TotCnt%>"; //총 게시물수
        var PageSize    = "<%=PageSize%>"; //보여질 게시물수
        var PageNo      = "<%=PageNo %>";    //현재페이지
        var SKey        = "<%=SKey %>";    //검색키워드
        var SVal        = "<%=SVal %>";    //검색어
        var SPeriod     = "<%=SPeriod%>";
        var SPeriodVal  = "<%=SPeriodVal%>";
        var Order       = "<%=Order %>";  //정렬
        var UnitID      = "<%=UnitID%>"; //질문ID
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var SwfUploadGUID = "<%=SwfUploadGUID %>";  //flash upload GUID
        var FileType      = "*.jpg;*.jpeg;*.png;*.gif";
        var FileTypeDesc  = "Images (*.jpg *.jpeg *.png *.gif)";
        var SwfUploadPath = "<%=SwfUploadPath %>";
        var Msg_ErrTitle  = "<%=GetResource("Msg_ErrTitle") %>";
        var Msg_ErrNum    = "<%=GetResource("Msg_ErrNum") %>";
        var Msg_ErrMaxNum = "<%=GetResource("Msg_ErrMaxNum") %>";
        var Msg_ErrItem   = "<%=GetResource("Msg_ErrItem") %>";
        var Msg_Del       = "<%=GetResource("Msg_Del") %>";
        var R_JTNoData    = "<%=GetResource("JTNoData") %>";
        var Txt_TitUnitAdd  = "<%=GetResource("TitUnitAddSub3") %>";
        var Msg_ErrSubTit   = "<%=GetResource("TitUnitAddSub12") %>";
        var R_Delete        = "<%=GetResource("Msg_Del") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Opinion/Js/Admin/Content/UnitAdd.js"></script>
    <script type="text/javascript">
        //        JsCall("/DeskPlusEIP/Opinion/Js/Admin/Content/UnitAdd.js");
    </script>
</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=PageTitle%></h1>
            <!-- 버튼 //-->
            <ul class="btn_left">
                
                
            </ul>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnSeq" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnUnitAdd" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>


        <div id="layout_content">
        <div class="row">
                <div class="col-12 content-area-body"><!-- width 100%(col-12) -->   
            <table class="table_typeB">
			<caption>항목정보</caption>
			<colgroup>
                <col width="80px" />
                <col />
			</colgroup>
			<tbody>
				<tr>
					<th><%=GetResource("TitUnitAddF1") %></th>
					<td><cc3:ClientSmartTextBox ID="ctbTitle" InputMode="None" MaxLength="100" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
				</tr> 
				<tr>
					<th>형식</th>
					<td>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <span id="spPoint" title="<%=GetResource("TitUnitAddSub1") %>">
                            <%=GetResource("TitUnitAddSub2") %> : <input id="tbPoint" type="text" value="0" style="width:30px !important;" runat="server" />
                        </span>
                    </td>
				</tr> 
				<tr>
					<th rowspan="2"><%=GetResource("TitUnitAddF2") %></th>
					<td>
                        <ul class="aLeft opn_gap" id="divItem">
                            <li id="liSelItem"><asp:DropDownList ID="ddlSelItem" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                            <li><input type="text" id="txtItem" style="min-width:300px !important;" value="<%=GetResource("TitUnitAddSub3") %>" /></li>
                            <li class="iBtn"><ul><li class="iUpload"><a href="javascript:UnitAdd.UploadAdd();"><span class="btn btn-secondary"><%=GetResource("TitUnitAddSub11") %></span></a></li><li class="iImg" style="display:none"><a href="javascript:UnitAdd.ImgItem();"><span class="btn btn-secondary"><%=GetResource("TitUnitAddSub5") %></span></a></li><li class="iDel" style="display:none"><a href="#"><span class="btn btn-secondary"><%=GetResource("TitUnitAddSub6") %></span></a></li><li class="iAdd"><a href="javascript:UnitAdd.AddItem();"><span class="btn btn-secondary"><%=GetResource("TitUnitAddSub7") %></span></a></li></ul></li>
                            <li style="display:none;"><input type="text" id="tbUrl" /></li>
                        </ul>
                        <div id="divItemFn" style="display:none;">
                            <span class="opn_subtxt">* 항목파일명 : </span>
                            <span id="spfn" class="opn_subtxt rst_isreq"></span>
                            <a id="delfn" href="#"><img src="<%=BaseImgPath%>/Icon/ico_delete.gif" class="opn_valign" /></a>
                        </div>
                    </td>
				</tr> 
                <tr>
                    <td>
                        <div class="divTarget"></div>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TitUnitAddF3") %></th>
                    <td>
                        <asp:DropDownList ID="ddlReq" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                        <span class="opn_subtxt"><%=GetResource("TitUnitAddSub8") %></span>
                    </td>
                </tr>
                <tr>
                    <th><%=GetResource("TitUnitAddF5")%></th>
                    <td>
                        <asp:DropDownList ID="ddlIsHorizon" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </td>
                </tr>
				<tr>
                    <th class="last-child"><%=GetResource("TitUnitAddF4") %></th>
                    <td class="last-child">
                        <asp:DropDownList ID="ddlActive" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                    </td>
                </tr>
			</tbody>
		    </table> 

            <div class="layout_subbox"></div>

            <span class="opn_subtxt"> <%=GetResource("TitUnitAddSub4") %></span><br />
            <span id="spExamTxt" class="opn_subtxt"><%=GetResource("TitUnitAddSub9") %></span>
            <span id="spUnitTxt" class="opn_subtxt"><%=GetResource("TitUnitAddSub10") %></span>
            
        </div>

    </div>
        </div>
    </div>
</div>

<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <asp:TextBox ID="tbHiddenUnits" Style="display:" Width="100" runat="server"></asp:TextBox>    
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
