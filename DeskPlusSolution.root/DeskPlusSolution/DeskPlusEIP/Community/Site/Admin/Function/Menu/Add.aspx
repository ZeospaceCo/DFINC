<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Menu.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MenuManager")%></h1>
            <!-- 버튼 //-->            
        </div>

        <div id="layout_content">
            <div class="cm_menu_layout">                            
                <div class="top">
                    <h2><%=GetResource("AddMenu")%></h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    </ul>
                    <div style="clear:both;"></div>
                    <table class="table_typeB">
			        <caption></caption>
			        <colgroup>
                        <col width="120px" />
                        <col />
                        <col width="120px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("BasicMenu")%></th>
                            <td>
                                <span class="aLeft">
                                    <asp:DropDownList ID="ddlBasicMenu" runat="server" class="searchselect form-control"></asp:DropDownList>
                                </span>                                                                                       
                            </td>
                            <th><%=GetResource("Weight")%></th>
                            <td>
                                <span class="aLeft" style="padding-left:2px;">
                                    <asp:DropDownList ID="ddlBasicWeight" runat="server" class="searchselect form-control"></asp:DropDownList>
                                </span>                                                               
                            </td>
                        </tr>                        
                        <tr>     
                            <th><%=GetResource("MenuName")%></th>                     
                            <td colspan="3">
                                <cc3:ClientSmartTextBox ID="ctbNameBase" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>               
                            </td>
                        </tr>  
                        <tr>
                            <th><%=GetResource("MultiName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                            <td colspan="3"><cc3:ClientSmartTextBox ID="CTBMultiName" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>    
                        <tr>
                            <th><%=GetResource("SettingAuth")%></th>
                            <td colspan="3">
                                <ul class="cm_lstpad">                                    
                                    <li><%=GetResource("WriteAuth")%> : <asp:DropDownList ID="ddlBasicWriteAuth" runat="server" class="searchselect form-control"></asp:DropDownList><%=GetResource("Up")%></li>
                                    <li><%=GetResource("ReadAuth")%> : <asp:DropDownList ID="ddlBasicReadAuth" runat="server" class="searchselect form-control"></asp:DropDownList><%=GetResource("Up")%></li>
                                </ul>                                                            
                            </td>
                        </tr>                          
			        </tbody>
		            </table>
                </div>
                
                <!-- 메뉴 //-->
                <div class="left" style="padding-top:80px;">

                    <h2><%=GetResource("SettingMenu")%></h2>
                    <div style="clear:both;"></div>

                    <!-- 메뉴내용 //-->
                    <div style="height:200px !important">
                        <asp:ListBox ID="lbxMenu" runat="server" Width="100%" Height="200px" Rows="17" CssClass="line-height:15;"></asp:ListBox>
                    </div>
                    <!-- 메뉴버튼 //-->
                    <div class="bott">
                        <ul class="btnimg">
                            <li><a onclick="Add.MenuUp();" ><img src="<%=BaseImgPath%>/Community/cm_menu_bott02.gif" alt="" title="" /></a></li>
                            <li><a onclick="Add.MenuDown();" ><img src="<%=BaseImgPath%>/Community/cm_menu_bott03.gif" alt="" title="" /></a></li>
                        </ul>                        
                    </div>

                </div>
                <!-- 메뉴상세설정 //-->
                <div class="cont" style="padding-top:80px;width:80% !important;">
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger"></cc3:ClientSmartButton></li>
                        <li><cc3:ClientSmartButton ID="cbtnMod" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>                        
                    </ul>
                    <div style="clear:both;"></div>
                    
                    <table class="table_typeE">
			        <caption></caption>
			        <colgroup>
                        <col width="100px" />
                        <col />
			        </colgroup>
			        <tbody>
                        <tr>
                            <th><%=GetResource("BasicMenu")%></th>
                            <td><cc3:ClientSmartTextBox ID="CTBMenuName" ReadOnly="true" MaxLength="250" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("MenuName")%></th>
                            <td><cc3:ClientSmartTextBox ID="ctbSubBase" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>    </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("MultiName")%>&nbsp;<a id="MultiSub"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                            <td><cc3:ClientSmartTextBox ID="CTBMultiSub" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                        </tr> 
                        <tr>
                            <th><%=GetResource("SettingAuth")%></th>
                            <td>
                                <ul class="cm_lstpad">                                    
                                    <li><%=GetResource("WriteAuth")%> : <asp:DropDownList ID="ddlWriteAuth" runat="server" class="searchselect form-control"></asp:DropDownList><%=GetResource("Up")%></li>
                                    <li><%=GetResource("ReadAuth")%> : <asp:DropDownList ID="ddlReadAuth" runat="server" class="searchselect form-control"></asp:DropDownList><%=GetResource("Up")%></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <th><%=GetResource("Weight")%></th>
                            <td>
                                <asp:DropDownList ID="ddlWeight" runat="server" class="searchselect form-control"></asp:DropDownList>
                            </td>
                        </tr>
                    </tbody>
                    </table>
                    

                </div>
                <div style="clear:both;"></div>
            </div>
        </div>
        
    </div>
</div>
    <!-- ================================== Hidden Input ===================================== -->	
    <div style="display:none;">
    <asp:TextBox ID="tbType" Style="display: none" runat="server" Width="0px"></asp:TextBox>   
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
        var CommunityID = "<%=CommunityID %>";
        var tmpMenuID = 0;
        var tmpSequence = 0;
        var BtnCheck = "N";
        var ScheduleYN = "<%=ScheduleYN %>";
        var MultiLang = "<%=MultiLang %>";        
        var R_InputMenu = "<%=GetResource("InputMenu") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_SelMenu  = "<%=GetResource("SelMenu") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_ScheduleOne = "<%=GetResource("ScheduleOne") %>";
        var R_DelMenu = "<%=GetResource("DelMenu") %>"; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Admin/Function/Menu/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/Function/Menu/Add.js");
    </script>
</body>
</html>
