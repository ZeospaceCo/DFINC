<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModSetting.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.ModSetting" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">

        var ZQCount = "<%=ZQCount%>";
        var ZPCount = "<%=ZPCount%>";    
        var ZCCount = "<%=ZCCount%>";    

        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";   

        var CurrentPage = "<%=CurrentPage%>";
        var RecordPerPage = "<%=RecordPerPage%>";
        var SearchField = "<%=SearchField%>";
        var SearchValue = "<%=SearchValue%>";
        var SearchTerm = "<%=SearchTerm%>";
        var SearchTermValue = "<%=SearchTermValue%>";
        var Order = "<%=Order%>";
        var FormID = "<%=FormID%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var R_Msg_InputTItle = "<%=GetResource("NeedFolderInfo") %>";       //"폴더정보를 입력하세요.";
        var R_Msg_InputNickName = "<%=GetResource("NeedLegacyName") %>";        //"직접 연동 호출명을 입력하세요.";
        var R_Msg_ErrorOverlap = "<%=GetResource("UsingName") %>";               //"현재 사용중인 연동명입니다.";
        var R_Msg_SuccessOverlap = "<%=GetResource("CanUse") %>";         //"사용 가능합니다.";
        var R_Msg_ErrInputName = "<%=GetResource("NeedFormName") %>";           //"양식명을 입력하세요.";
        var R_MSG_SPECIALCHAR = "<%=GetResource("MSG_Secure") %>";              //"해당 특수문자는 보안취약성 공격에 사용되므로 제한됩니다.";
        var R_NeedFolderInfo2 = "<%=GetResource("NeedFolderInfo2") %>";              

        //리스트사용
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Receive = "<%=GetResource("Receive") %>";
        var R_Carboncopy = "<%=GetResource("Carboncopy") %>";
        var R_IsActiveTitN = "<%=GetResource("IsActiveTitN") %>";
        var R_IsActiveTitY = "<%=GetResource("IsActiveTitY") %>";
        var R_Btn_Del = "<%=GetResource("Btn_Del") %>";

        var R_Msg_ErrorUserBefore = "<%=GetResource("Msg_ErrorUserBefore") %>";

        var R_MSG_ReceiveCarboncopy = "<%=GetResource("Receive") %>" + "/" + "<%=GetResource("Carboncopy") %>" + "<%=GetResource("Msg_ErrorUserNon") %>"; //수신참조정보

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/FormGeneration/ModSetting.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/FormGeneration/ModSetting.js");
    </script>

</head>
<body>
<form id="form1" runat="server">

<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("SetFormInfo")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnSeqOrder" runat="server"></cc3:ClientSmartButton></li>
                <li style="display:none"><cc3:ClientSmartButton ID="cbtnSeqLine" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div id="layout_caption">
            <span class="app_caption" style="font-weight:bold;">
                *<%=GetResource("PrivateDraftCnt")%>(<strong><asp:Literal ID="ltDraftCnt" runat="server"></asp:Literal><%=GetResource("EA")%></strong>)
                / <%=GetResource("ProcessingCnt")%>(<strong><asp:Literal ID="ltDocCnt" runat="server"></asp:Literal><%=GetResource("EA")%></strong>)
            </span>
        </div>
        <div id="layout_content">
            <div class="layout_subbox">
                <h2><%=GetResource("FormName2")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("FormName2")%></caption>
            <colgroup>
                <col width="110px" />
                <col />
                <col width="110px" />
                <col />
			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FormName2")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormName" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("ShortFormName")%></th>
                    <td title="<%=GetResource("UseDocNumber")%>"><cc3:ClientSmartTextBox ID="ctbNameCode" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Korean")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameKO" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("English")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameEN" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Japanese")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameJA" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Franch")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameFR" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
                    <th><%=GetResource("Chinese1")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameZHCN" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                    <th><%=GetResource("Chinese2")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbFormNameZHTW" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox></td>
                </tr>
            </tbody>
            </table>

            <!-- 대상폴더정보 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("TargetFolderInfo")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("TargetFolderInfo")%></caption>
            <colgroup>
                <col width="110px" />
                <col />    
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("FolderGubun")%></th>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                </td>
            </tr>
            <tr class="trFolderInfo">
                <th><%=GetResource("FolderInfo")%></th>
                <td><ul class="btn_smallL"><li><a href="#" id="btnFolder" class="btn btn-sm"><span><%= GetResource("Btn_FolderSel")%></span></a></li></ul>
                    <cc3:ClientSmartTextBox ID="ctbFolderName" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;" ReadOnly="true"></cc3:ClientSmartTextBox>
                    <span class="app_captit"><%=GetResource("NeedFolderInfo2")%></span>
                </td>
            </tr>
            </tbody>
            </table>
            <!-- 대상폴더정보 끝 //-->

            <!-- 연계파일정보 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("LegacyFileInfo")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("LegacyFileInfo")%></caption>
            <colgroup>
                <col width="110px" />
                <col />
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("IsUse")%></th>
                <td><asp:RadioButtonList ID="rblIsLegacy" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>        
            </tr>
            <tr>
                <th><%=GetResource("AfterCall")%></th>
                <td><asp:RadioButtonList ID="rblIsAfterSubmitCall" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <th><%=GetResource("CallName")%></th>
                <td><ul class="btn_smallL"><li><a href="#" id="btnOverlap" class="btn btn-sm"><span><%= GetResource("Btn_Check")%></span></a></li></ul>
                    <cc3:ClientSmartTextBox ID="ctbNickName" InputMode="None" runat="server" Required="true" CssStyle="width:150px !important;"></cc3:ClientSmartTextBox>
                </td>
            </tr>
            <tr>
                <th><%=GetResource("LegacyPath")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbCustomFileName" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            </tbody>
            </table>
            <!-- 연계파일정보 끝 //-->

            <!-- EDM이관설정 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("SetEDM")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("SetEDM")%></caption>
            <colgroup>
                <col width="110px" />
                <col />
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("IsActiveTit")%></th>
                <td><asp:RadioButtonList ID="rblLinked" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <th><%=GetResource("TransferWay")%></th>
                <td><asp:RadioButtonList ID="rblLinkedType" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <th><%=GetResource("ETC")%></th>
                <td><cc3:ClientSmartTextBox ID="ctbIsLinkedEtc" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
            </tr>
            </tbody>
            </table>
            <!-- EDM이관설정 끝 //-->

            <!-- 수신자 시작 //-->   
            <div>         
                <div class="layout_subbox">
                    <h2 style="display:inline;"><%=GetResource("Receive")%>/<%=GetResource("Carboncopy")%> <%=GetResource("Setting")%></h2>
                    <ul class="btn_right">
                        <li><a href="#" id="ModSeq" class="btn"><span ><%=GetResource("SeqTitOrder")%></span></a></li>
                        <cc3:ClientSmartButton ID="cbtnAddReceive" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                        <cc3:ClientSmartButton ID="cbtnAddCarboncopy" BeginEndTag="li" runat="server"></cc3:ClientSmartButton>
                    </ul>        
                </div>
                <table class="table_typeT" id="dataTable">
                    <colgroup>
                        <col width="100px" />
                        <col width="200px" />
                        <col width="*" />                    
                        <col width="90px" />
                    </colgroup>
                    <thead>
                        <tr>                        
                            <th class="center"><%=GetResource("TitSec")%></th>
                            <th class="center"><%=GetResource("IsActiveTit")%></th>
                            <th class="center"><%=GetResource("SeqTitInfo")%></th>
                            <th class="center"><%=GetResource("Del")%></th> 
                        </tr>
                    </thead>
                </table>   
            </div>                   
            <!-- 수신자 시작 //-->   

            <!-- 기타설정정보 시작 //-->
            <div class="layout_subbox">
                <h2><%=GetResource("ETCSetInfo")%></h2>
            </div>
            <table class="table_typeB">
            <caption><%=GetResource("ETCSetInfo")%></caption>
            <colgroup>
                <col width="110px" />
                <col />    
            </colgroup>
            <tbody>
            <tr>
                <th><%=GetResource("UseDoc")%></th>
                <td><asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <th><%=GetResource("MobileWrite")%></th>
                <td><asp:RadioButtonList ID="rblIsMobile" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr style="display:none">
                <th><%=GetResource("ApproveBulk")%></th>
                <td><asp:RadioButtonList ID="rblIsBundle" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
            </tr>
            <tr>
                <th><%=GetResource("EnforceDoc")%></th>
                <td>
                    <asp:RadioButtonList ID="rblIsEnforce" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList>
                    <span class="app_subtxt">* ID: CONTENT : <%=GetResource("Contents")%></span>
                </td>
            </tr>
            </tbody>
            </table>
            <!-- 기타설정정보 끝 //-->
        </div>

    </div>
</div>


<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <asp:TextBox ID="tbFolderID" Style="display: none" Width="0" Text="0" runat="server"></asp:TextBox>

    <asp:TextBox ID="tbAppointerKind" Style="display: none"   runat="server" Width="0px"></asp:TextBox>        
    <asp:TextBox ID="tbDeptID" Style="display: none"   runat="server" Width="0px"></asp:TextBox>    
    <asp:TextBox ID="tbUserID" Style="display: none"   runat="server" Width="0px"></asp:TextBox>    
    <asp:TextBox ID="tbDeptName" Style="display: none"   runat="server" Width="0px"></asp:TextBox>    
    <asp:TextBox ID="tbUserName" Style="display: none"   runat="server" Width="0px"></asp:TextBox>        
    <asp:TextBox ID="tbIsDept" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbIsActive" Style="display: none"   runat="server" Width="0px"></asp:TextBox>    
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
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>

</body>
</html>
