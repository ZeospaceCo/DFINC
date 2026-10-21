<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Admin.Menu.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
   <meta charset="UTF-8">
   <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
   <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("GroupManage")%></h1>
        </div>
        <div id="layout_content">
            <div class="row">
               <div class="col-6 p-0 pr-4 pt-4"><!-- width 100%(col-12) -->
                    <h1><%=GetResource("Category")%></h1>
                    <ul class="btn_right" style="padding-top:10px;">
                        <cc3:ClientSmartButton ID="CbtnMove" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                    </ul>

                    <div style="clear:both;" class="gap"></div>
                    <table class="table_typeB">
			        <caption><%=GetResource("Category")%></caption>
			        <colgroup>
                        <col width="100px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("Category")%></th>
                            <td>
                                <span class="aLeft"><cc3:ClientSmartTextBox ID="ctbParentName" Required="true" MaxLength="100" runat="server"></cc3:ClientSmartTextBox>&nbsp;</span>
                                <ul class="btn_left">                                
                                    <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-sm btn-danger" /></li>
                                    <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                    <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th><label ><%=GetResource("MutiName")%></label>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a></th>
                            <td><cc3:ClientSmartTextBox ID="ctbMenuName" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Active")%></th>
                            <td><asp:CheckBox ID="chkActive" runat="server" Checked="true" /></td>
                        </tr>
                        <tr>                
                            <th><%=GetResource("Specifier")%></th>          
                            <td>
                              <ul class="btn_left">
                                <cc3:ClientSmartButton ID="cbtnAddUser" BeginEndTag="li" runat="server" />
                                <cc3:ClientSmartButton ID="cbtnToOrder" BeginEndTag="li" runat="server" />
                                <cc3:ClientSmartButton ID="cbtnCcOrder" BeginEndTag="li" runat="server" />
                               </ul>                    
                            </td>                    
                        </tr>
			        </tbody>                    
		            </table>

                    <div style="clear:both;padding:10px 0px 0px 0px;"></div>
                    <div class="mid">                    
                        <div id="phItemList" style="clear: both;"></div>                    
                    </div>
                </div>

               <div class="col-6 p-0 pt-4"><!-- width 100%(col-12) -->
                    <h1><%=GetResource("SubCategory")%></h1>
                    <ul class="btn_right" style="padding-top:10px;">
                        <cc3:ClientSmartButton ID="CbtnSubMove" runat="server" BeginEndTag="li" class="btn btn-secondary"/>
                    </ul>

                    <div style="clear:both;" class="gap"></div>
                    <table class="table_typeE">
			        <caption><%=GetResource("SubCategory")%></caption>
			        <colgroup>
                        <col width="100px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("SubCategory")%></th>
                            <td>
                                <span class="aLeft"><cc3:ClientSmartTextBox ID="ctbSubName" Required="true" MaxLength="100" runat="server"></cc3:ClientSmartTextBox>&nbsp;</span>
                                <ul class="btn_left">                                
                                    <li><cc3:ClientSmartButton ID="cbtnDelSub" runat="server" Class="btn btn-sm btn-danger" /></li>
                                    <li><cc3:ClientSmartButton ID="cbtnModSub" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                    <li><cc3:ClientSmartButton ID="cbtnAddSub" runat="server" Class="btn btn-sm btn-secondary" /></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th><label><%=GetResource("MutiName")%></label>&nbsp;<a id="MultiNameSub"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a></th>
                            <td><cc3:ClientSmartTextBox ID="ctbSubMenuName" Required="true" MaxLength="100" runat="server" CssStyle="float:left;width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Active")%></th>
                            <td align="left"><asp:CheckBox ID="chkSubAcitve" runat="server" Checked="true" /></td>
                        </tr>
                        <tr>                
                         <th><%=GetResource("Specifier")%></th>          
                        <td>
                          <ul class="btn_left">
                            <cc3:ClientSmartButton ID="cbtnSubUserAdd" BeginEndTag="li" runat="server" />
                            <cc3:ClientSmartButton ID="cbtnItemSubToUserUpdown" BeginEndTag="li" runat="server" />
                            <cc3:ClientSmartButton ID="cbtnItemSubCcUserUpdown" BeginEndTag="li" runat="server" />
                           </ul>                    
                        </td>                    
                        </tr>
                    </tbody>
                    </table>

                    <div style="clear:both;padding:10px 0px 0px 0px;"></div>
                    <div class="mid">                    
                        <div id="phItemSubList" style="clear: both;"></div>                    
                    </div>
                </div><!-- end col content-area-body-right -->
            </div>
        </div>
     </div>
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
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ItemID = "";
        var ItemSub = "";
        var ItemSubID = "";
        var MultiLang = "<%=MultiLang %>";
        var R_NoListData = "<%=GetResource("NoListData") %>";
        var R_BaseName = "<%=GetResource("BaseName") %>";
        var R_SubBaseName = "<%=GetResource("SubBaseName") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_CheckModList = "<%=GetResource("SelectFirst") %>";
        var R_SelectFirst = "<%=GetResource("SelectFirst") %>";
        var R_CheckCategory = "<%=GetResource("CheckCategory") %>";
        var R_DelGubun = "<%=GetResource("DelGubun") %>";
        var R_DelSubCategory = "<%=GetResource("DelSubCategory") %>";
        var R_Active = "<%=GetResource("Active") %>";
        var R_NonActive = "<%=GetResource("NonActive") %>";
        var R_GroupName = "<%=GetResource("GroupName") %>";
        var R_UsedYN = "<%=GetResource("UsedYN") %>";
        var R_Template = "<%=GetResource("Template") %>";
        var R_Add = "<%=GetResource("Add") %>";
        var R_Mod = "<%=GetResource("Mod") %>";
        var R_SpecifierList = "<%=GetResource("SpecifierList") %>"
        var R_SpecifierAdd = "<%=GetResource("SpecifierAdd") %>"
        var R_SpecifierOrder = "<%=GetResource("SpecifierOrder") %>"
        var R_SpecifierUserList = "<%=GetResource("SpecifierUserList") %>"
        var R_SpecifierUserAdd = "<%=GetResource("SpecifierUserAdd") %>"
        var R_SpecifierUserOrder = "<%=GetResource("SpecifierUserOrder") %>"
        var R_Receiver = "<%=GetResource("Receiver") %>"
        var R_Referencer = "<%=GetResource("Referencer") %>"
        var R_SelectFirst ="<%=GetResource("SelectFirst") %>"
        var R_SelectSecond = "<%=GetResource("SelectSecond") %>"
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Admin/Menu/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Admin/Menu/List.js");
    </script>
</body>
</html>
