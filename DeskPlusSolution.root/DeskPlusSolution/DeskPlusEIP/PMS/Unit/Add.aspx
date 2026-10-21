<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.PMS.Web.Unit.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_PMS.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_PMS.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <h1><asp:Literal ID="ltlMenuName" runat="server"></asp:Literal></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server" /></li>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >                
                        <table class="table_typeB">
                            <caption>
                            </caption>
                            <colgroup>
                                <col width="100px" />
                                <col />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="tbxTitle" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlPlanStartDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxPlanStartDate" runat="Server" ReadOnly="true" Width="80%"></asp:TextBox>
                                        <input type="checkbox" id="cbxPlanStartDate" /> <%=GetResource("Ditto") %>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlPlanEndDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxPlanEndDate" runat="server" ReadOnly="true" Width="80%"></asp:TextBox>
                                        <input type="checkbox" id="cbxPlanEndDate" /> <%=GetResource("Ditto") %>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlStartDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxStartDate" runat="Server" ReadOnly="true" Width="97%"></asp:TextBox>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlEndDate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxEndDate" runat="Server" ReadOnly="true" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlPlanMoney" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxPlanMoney" Style="ime-mode: disabled;" onkeypress="checkForNumber();" onBlur="checkForNumber();" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlCode" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxCode" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlExecuteMoney" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxExecuteMoney" Style="ime-mode: disabled;" onkeypress="checkForNumber();" onBlur="checkForNumber();" runat="Server" Width="97%"></asp:TextBox>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlCharger" runat="server"></asp:Literal>
                                        <ul class="btn_smallR">
                                            <li>
                                                <cc3:ClientSmartButton ID="cbtnDeptUserTree" runat="server" Class="btn btn-sm btn-secondary"></cc3:ClientSmartButton>
                                            </li>
                                        </ul>
                                    </th>
                                    <td>
                                        <asp:HiddenField ID="hdChargerID" runat="Server"></asp:HiddenField>
                                        <asp:HiddenField ID="hdChargetName" runat="Server"></asp:HiddenField>
                                        <div id="InputDiv1" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px; text-overflow: ellipsis; overflow-y: auto;" contenteditable="false">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlRelateDeptName" runat="server"></asp:Literal>
                                        <ul class="btn_smallR">
                                            <li>
                                                <cc3:ClientSmartButton ID="cbtnDeptUserTree2" runat="server" Class="btn btn-sm btn-secondary"></cc3:ClientSmartButton>
                                            </li>
                                        </ul>
                                    </th>
                                    <td>
                                        <asp:HiddenField ID="hdRelateDeptName" runat="Server"></asp:HiddenField>
                                        <div id="InputDiv3" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px; text-overflow: ellipsis; overflow-y: auto;" contenteditable="false">
                                        </div>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlChargeDeptName" runat="server"></asp:Literal>
                                        <ul class="btn_smallR">
                                            <li>
                                                <cc3:ClientSmartButton ID="cbtnDeptUserTree1" runat="server" Class="btn btn-sm btn-secondary"></cc3:ClientSmartButton>
                                            </li>
                                        </ul>
                                    </th>
                                    <td>
                                        <asp:HiddenField ID="hdChargeDeptName" runat="Server"></asp:HiddenField>
                                        <div id="InputDiv2" style="height: 32px; width: 100%; padding-left: 2px; padding-top: 2px; overflow-y: auto;" contenteditable="false">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlProcess" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlProcess" runat="server" class="searchselect form-control">
                                            <asp:ListItem Text="10%" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20%" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="30%" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="40%" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="50%" Value="50"></asp:ListItem>
                                            <asp:ListItem Text="60%" Value="60"></asp:ListItem>
                                            <asp:ListItem Text="70%" Value="70"></asp:ListItem>
                                            <asp:ListItem Text="80%" Value="80"></asp:ListItem>
                                            <asp:ListItem Text="90%" Value="90"></asp:ListItem>
                                            <asp:ListItem Text="100%" Value="100"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlWeightPercent" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="tbxWeightPercent" Style="ime-mode: disabled;" onkeypress="checkForNumber();" onBlur="checkForNumber();" Width="97%" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" class="searchselect form-control"></asp:DropDownList>
                                    </td>
                                    <th>
                                        <asp:Literal ID="ltlTemplate" runat="server"></asp:Literal>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlTemplate" runat="server" class="searchselect form-control"></asp:DropDownList>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="tabs" class="layout_subbox">
                            <ul id="tab" class="tab">
                                <li><a id="ActvieX" class="tab_on"><span>ActiveX</span> </a></li>
                                <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                            </ul>
                            <div class="divEditor">
                                <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                            </div>
                        </div>
                    </div><!-- end col content-area-body-right -->
                    <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">  
                      <ul> 
                        <!-- 첨부파일 -->
                        <li class="title">
                            <%=GetResource("AttachName")%>
                            <div class="float-right">                            
                            <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        <!-- 첨부파일 리스트 -->
                      </ul>
                    </div>
                </div>               
            </div>
        </div>
    </div>
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>

        <asp:TextBox ID="tbxHiddenCharger" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbxHiddenChargerDept" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbxHiddenChargerLink" Width="0" runat="server"></asp:TextBox>
        
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
        <!-- END 에디터에 필요한 필드 //-->        
    </div>
    <!-- ==================================================================================== -->
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script> 
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
		var ProjectID = "<%=ProjectID %>";
        var MenuID = "<%=MenuID %>";  
        var MenuType = "<%=MenuType %>";   
        var RqProjectID = "<%=RqProjectID %>";
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var Status = "<%=Status %>";
        var ProjParam           = "<%=ProjParam %>"; 
        var preLangReal         = "<%=PreLangReal %>";        
        var chargerName         = "<%=ChargerName %>";
        var linkedDeptName      = "<%=LinkedDeptName %>";
        var chargerDeptName     = "<%=ChargerDeptName %>";
        var EleDom = "";
		var Private_BoardAdd_DPEditorTEXT = "<%=GetResource("Private_BoardAdd_DPEditorTEXT") %>";
		var MSG_DUF_USERDEPT = "<%=GetResource("MSG_DUF_USERDEPT") %>";
		var Unit_Register_LoadTemplate = "<%=GetResource("Unit_Register_LoadTemplate") %>";		
		var Unit_Register_Request1 = "<%=GetResource("Unit_Register_Request1") %>";
		var Unit_Register_Request2 = "<%=GetResource("Unit_Register_Request2") %>";
		var Unit_Register_Request3 = "<%=GetResource("Unit_Register_Request3") %>";
		var Unit_Register_Request4 = "<%=GetResource("Unit_Register_Request4") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Del = "<%=GetResource("Unit_Register_Delete") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";   
        var R_FileName =  "<%=GetResource("FileName") %>"; 
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("FileDel") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";      
        var R_InputDate = "<%=GetResource("InputDate") %>";
        var R_CheckDate = "<%=GetResource("CheckDate") %>";
        var R_InputStartDate = "<%=GetResource("InputStartDate") %>";
        var R_CheckEndDate = "<%=GetResource("CheckEndDate") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/PMS/Js/Unit/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/PMS/Js/Unit/Add.js");
    </script>
</body>
</html>