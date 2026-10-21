<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPortletUser.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Portal.AddPortletUser" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = "<%=TotalCount%>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬
        var CatID = "0";
        var PhotoName = "<%=PhotoName %>";
        var PreLang = "<%=PreLangReal %>";
        var AccordionActive = 0;
        var Gubun = "add";
        var CategoryName = "";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DirectAdd = "<%=GetResource("DirectAdd") %>";
        var R_CheckModGroup = "<%=GetResource("CheckModGroup") %>";
        var R_CheckAddRSSGroup = "<%=GetResource("CheckGroup") %>";
        var R_CheckDeleteGroup = "<%=GetResource("CheckDeleteGroup") %>";
        var R_InputGroupName = "<%=GetResource("InputGroupName") %>";
        var R_ConfirmDeleteTabAddPortlet = "<%=GetResource("ConfirmDeleteTabAddPortlet") %>";
        var R_CheckGroup = "<%=GetResource("CheckGroup") %>";
        var R_InputPortletName = "<%=GetResource("InputPortletName") %>";
        var R_InputRSSURL = "<%=GetResource("InputRSSURL") %>";
        var R_CheckGroupAfterDeletePortlet = "<%=GetResource("CheckGroupAfterDeletePortlet") %>";
        var R_DeletePortlet = "<%=GetResource("DeletePortlet") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/EIP/Js/Portal/AddPortletUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/EIP/Js/Portal/AddPortletUser.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
      
    <div class="layer_popup">
      <ul class="left-menu-container left-menu-2">
      <li>
       
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ConfigPortletUser")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnAddPortlet" runat="server" class="btn btn-secondary"/></li>
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3" style="min-height:410px;">
                <div class="eip_portlet_left" style="width:180px;">

                <div id="eip_portlet_treetop" style="display:none">
                    <span id="folderName" class="treetop_title"><%=GetResource("Portlet")%></span>
                </div>

                <div id="layout_treeboxAcc">
                    <div class="accordion" style="height:430px;">
                        <div class="menu"><a href="#" onclick="" ><%=GetResource("PersonalPortlet")%></a></div>
                        <div class="menusub">
                            <zeoPortletTree:UCPortletTreeScript ID="PrivatePortletTree" runat="server" UseIcon="true" UseActive="true"
                                SetCallbackComplete="onCallbackComplete" SetNodeSelect="PrivatePortletTree_onNodeSelect" />
                        </div>
                    </div>            
                </div>

                <div id="layout_treebuttom"></div>
                <div>
                    <div id="layout_treebutton">
                        <a href="#" id="folderAdd" style="cursor: pointer" alt="add" class="btn btn-sm"><%=GetResource("Add")%></a>
                        <a href="#" id="folderModify" style="cursor: pointer" alt="modify" class="btn btn-sm"><%=GetResource("Mod")%></a> 
                        <a href="#" id="folderDelete" style="cursor: pointer" alt="delete" class="btn btn-sm btn-danger"><%=GetResource("Del")%></a>            
                    </div>
                </div>    

                </div>
                <div class="eip_portlet_mid">

                    <div class="actionDiv" id="divList">
                        <div class="table_typeLtop"><p></p></div>
                        <div id="phList"></div> 
                        <div class="table_typeLbottom"><p></p></div>

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1">
                            <h3>
                                <span id="pageInfo"></span>
                            </h3>
                            <span id="pager"></span>
                            <div style="display:none;">
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10 " Value="10"></asp:ListItem>
                                <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                            </asp:DropDownList>
                            </div>
                        </div>
                        <!-- 페이징 끝 //-->
                    </div>


                    <div class="actionDiv" id="divfolderAdd">
                        <table class="table_typeB">
                        <caption><%=GetResource("AddPrivateGroup")%></caption>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("ParentsGroupName")%></th>
                            <td><span id="ltlHeader_folderAdd"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("GroupName")%>&nbsp;</th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbAddName" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                                <a id="cbtnAddName" class="btn btn-sm"><%=GetResource("AddGroup")%></a>
                            </td>
                        </tr>
                        </tbody>
                        </table>
                    </div>


                    <div class="actionDiv" id="divfolderMod">
                        <table class="table_typeB">
                        <caption><%=GetResource("ModPrivateGroup")%></caption>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("ParentsGroupName")%></th>
                            <td><span id="ltlHeader_folderMod"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("GroupName")%>&nbsp;</th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbModName" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                                <a id="cbtnModName" class="btn btn-sm"><%=GetResource("ModGroup")%></a>
                            </td>
                        </tr>
                        </tbody>
                        </table>
                    </div>


                    <div class="actionDiv" id="divAddRSS">
                        <table class="table_typeB">
                        <caption><%=GetResource("AddPortlet")%></caption>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tbody>
                        <tr>
                            <th><%=GetResource("GroupName")%></th>
                            <td><span id="ltlHeader_AddRSS"></span></td>
                        </tr>
                        <tr>
                            <th><%=GetResource("PortletName")%>&nbsp;</th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbAddRSSName" Required="true" MaxLength="100" runat="server" CssStyle="width:200px !important;"></cc3:ClientSmartTextBox>
                                <a id="cbtnAddRSSName" class="btn btn-sm"><%=GetResource("AddPortlet")%></a>
                            </td>
                        </tr>
                        <tr>
                            <th>RSS URL&nbsp;</th>
                            <td>
                                 <cc3:ClientSmartTextBox ID="ctbAddRSSUrl" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                        </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
      </li>
      </ul>

    </div>
    </form>
</body>
</html>
