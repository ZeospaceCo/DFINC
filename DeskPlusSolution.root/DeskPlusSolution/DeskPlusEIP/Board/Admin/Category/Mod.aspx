<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Category.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>

<form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><%= CategoryTitle%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnMove" runat="server" BeginEndTag="li" />            
                <cc3:ClientSmartButton ID="CbtnScreenShot" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnCreateMenu" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnOpenFolder" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="CbtnHeaders" runat="server" BeginEndTag="li" />

                <cc3:ClientSmartButton ID="CbtnMod" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
            </ul>
        </div>  
		<div id="layout_srh" class="layout_srh">
        	<div class="float-left">
                <ul id="ulTabMenu" class="tab">
                    <li><a href="javascript:Mod.SetTab('1');" class="tab_on" id="tabDivInfo"><span><%=GetResource("View_RegInfo")%></span></a></li>
                    <li><a href="javascript:Mod.SetTab('2');" id="tabDivAuth"><span><%=GetResource("AuthSetting")%></span></a></li>      
                </ul>
            </div>
        </div>
        
        <div id="layout_content"><!-- 컨텐츠 하단영역 -->
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->               


                        <div id="divInfo">
                            <!-- 등록정보 start-->    
                            <table class="table_typeB"> 
                            <caption><%=GetResource("View_RegInfo")%></caption>
                            <colgroup>
                            <col width="120px" />
                            <col />
                            <col width="50px" />
                            <col width="50px" />
                            </colgroup>
                            <tbody>
                            <tr> 
                                <th><%=GetResource("FolderTitle")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif"/></a></th> 
                                <td>
                                    <cc3:ClientSmartTextBox ID="ctbTitle" Required="true" MaxLength="100" runat="server" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox>
                                    <cc3:ClientSmartTextBox ID="ctbMultiLang" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                                </td>
                                <th><%=GetResource("Number")%></th> 
                                <td><%=CatID %></td>
                            </tr> 
                            <tr> 
                                <th><%=GetResource("FullPath")%></th> 
                                <td  colspan="3"><asp:Literal ID="ltPathName" runat="server"></asp:Literal></td>                    
                            </tr> 
                            <tr> 
                                <th class="last-child"><%=GetResource("FolderDescription")%></th> 
                                <td colspan="3" class="last-child"><cc3:ClientSmartTextBox ID="cbtnExplan" InputMode="None"  runat="server"  CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td> 
                            </tr>
                            </tbody>
                            </table> 
                            <!-- 등록정보 end-->
                            <!-- 옵션 start-->
                            <div class="layout_subbox">
                                <h2><%=GetResource("Option")%></h2>
                            </div>
                            <ul>
                                <li class="itemgap"><asp:CheckBox ID="cbShowImage" runat="server" /><%=GetResource("ShowAttachedImage")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbUseReply" runat="server" /><%=GetResource("UseReply")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbUseShortReply" runat="server" /><%=GetResource("UseShortRelpy")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbShortReplyGood" runat="server" /><%=GetResource("UseComments")%> (<asp:CheckBox ID="cbShortReplyTop" runat="server" /><%=GetResource("ShowTop")%>)</li>
                                <li class="itemgap"><asp:CheckBox ID="cbShowViewer" runat="server" /><%=GetResource("CheckHits")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbUseDept" runat="server" /><%=GetResource("UseDeptBoard")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbVote" runat="server" /><%=GetResource("UseEvaluation")%> (<asp:CheckBox ID="cbVoteView" runat="server" />)</li>
                                <li class="itemgap"><asp:CheckBox ID="cbAnonymity" runat="server" /><%=GetResource("UseAnonymity")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbUseView" runat="server" /><%=GetResource("UseSubordinationBoardNotice")%></li>
                                <li class="itemgap">
                                    <asp:CheckBox ID="cbListType" runat="server" /><%=GetResource("BoardListFormat")%>
                                    <asp:DropDownList ID="ddlListType" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                </li>
                                <li class="itemgap">
                                    <asp:CheckBox ID="cbTemplateID" runat="server" /><%=GetResource("WriteUseTemplate")%>
                                    <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                </li>
                                <li class="itemgap">
                                    <asp:CheckBox ID="cbAlarm" runat="server" /><%=GetResource("UseAlarm")%>
                                    <asp:DropDownList ID="ddlAlarm" runat="server" CssClass="searchselect form-control"></asp:DropDownList>(<%=GetResource("SelectAlarm")%>)
                                </li>
                                <li class="itemgap"><asp:CheckBox ID="cbMobile" runat="server" /><%=GetResource("ShowMobile")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbSubApply" runat="server" /><%=GetResource("ApplyAllSubordination")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbPopUp" runat="server" /><%=GetResource("PopUpSetting")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbLinkUrl" runat="server" /><%=GetResource("URLOption")%></li>
                                <li class="itemgap"><asp:CheckBox ID="cbDownLoad" runat="server" /><%=GetResource("CheckDownLoad")%></li>
                            </ul>
                            <!-- 옵션 end -->    
                        </div>
            
                        <div id="divAuth" style="display:none;">
                            <!-- 권한설정 start-->    
                            <div class="acc_auth_left">
                                <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px; overflow:hidden">
                                    <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" UseMultiCompany="false" runat="server" />
                                </div>
                            </div>
                            <div class="acc_auth_middle">
                                <ul>
                                    <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary" ></cc3:ClientSmartButton></li>
                                </ul>
                            </div>
                            <div class="acc_auth_right">
                                <ul> 
                                    <li><asp:CheckBox ID="cbAuthSubApply" runat="server" /><%=GetResource("ApplySameAuthSubordinatioFolder")%></li>           
                                    <li>
                                        <asp:CheckBox ID="cbAllAuthManage" runat="server" /><%=GetResource("SettingSameAuth")%>
                                        <asp:DropDownList ID="ddlAllAuthManage" runat="server" CssClass="searchselect form-control"></asp:DropDownList>
                                    </li>
                                    <li><asp:CheckBox ID="cbEnterpriceAuthManage" runat="server" /><%=GetResource("WholeSettingAuth")%></li>
                                </ul>
                                <br />
                                <table class="table_typeT" id="contentTable">
                                <caption><%=GetResource("AuthSetting")%></caption>
                                <colgroup>
                                    <col />
                                    <col width="120px"/>
                                    <col width="80px"/>
                                </colgroup>
                                <tbody>
                                <tr>
                                    <th class="center"><%=GetResource("Call")%></th>
                                    <th class="center"><%=GetResource("Authority")%></th>
                                    <th class="center"><%=GetResource("Del")%></th>
                                </tr>
                                <%=AuthTable%>
                                <%--<a href="javascript:DelAuth('','')"><img src="<%=BaseImgPath%>/Lang/<%=PreLang%>/Btn/ico_folderset_del.gif" /></a>--%>
                                </tbody>
                                </table>
                            </div>
                            <!-- 권한설정 end-->    
                        </div><!-- end divInfo -->


                 </div><!-- end col content-area-body-right -->
                
            </div><!-- end row  -->
        </div><!-- end layout_content  --> 
    
    <div style="display: none">
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>               
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div>    
    
	</div><!-- end layout_full  -->
</div><!-- end layout_body  -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var OptionUseDept = "<%=OptionUseDept %>";
        var RowMax = <%=MaxRowMax %>;
        var MultiLang = "<%=MultiLang %>";        
        
        var R_SettingAuthority = "<%=GetResource("SettingAuthority") %>";
        var R_ApplyFolderAllAuthority = "<%=GetResource("ApplyFolderAllAuthority") %>";
        var R_AlreadySettingAuthority = "<%=GetResource("AlreadySettingAuthority") %>";
        var R_Whole = "<%=GetResource("Whole") %>";
        var R_FolderName = "<%=GetResource("FolderName") %>";
        var R_Write = "<%=GetResource("Write") %>";
        var R_Read = "<%=GetResource("Read") %>";
        var R_ReadReply = "<%=GetResource("ReadReply") %>";
        var R_WriteReply = "<%=GetResource("WriteReply") %>";
        var R_ACCOUNT_SUCCESS = "<%=GetResource("ACCOUNT_SUCCESS") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_NoAddOtherCompany = "<%=GetResource("NoAddOtherCompany") %>";
        var R_AddOnlyReadAuth = "<%=GetResource("AddOnlyReadAuth") %>";
        var R_Del = "<%=GetResource("Del")%>";  
    </script>     
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Category/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Category/Mod.js");
    </script>
</body>
</html>
