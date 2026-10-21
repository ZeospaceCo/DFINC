<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Admin.Category.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
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
            <div class="cm_fdr">
                <div class="left">
                    <h2><%=GetResource("Category")%></h2>
                    <div style="clear:both;"></div>

                    <table class="table_typeB">
			        <caption><%=GetResource("DefaultMenu")%></caption>
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
                            <th>
                                <label title="<%=GetResource("MutiName")%>"><%=GetResource("MutiName")%></label>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbMenuName" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
			        </tbody>
		            </table>

                    <div style="clear:both;padding:20px 0px 0px 0px;"></div>

                    <div class="mid">
                        <ul class="cm_menu_item">
                            <asp:ListBox ID="lbxParentCategory" runat="server" Width="100%" Rows="26" style="height:350px;"></asp:ListBox>
                        </ul>
                    </div>                    
                </div>

                <div class="right">
                    <span class="aLeft"><h2><%=GetResource("SubCategory")%></h2></span>
                    <div style="clear:both;"></div>

                    <table class="table_typeE">
			        <caption><%=GetResource("SetDetailInfo")%></caption>
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
                            <th>
                                <label title="<%=GetResource("MutiName")%>"><%=GetResource("MutiName")%></label>&nbsp;<a id="MultiNameSub"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a>
                            </th>
                            <td>
                                <cc3:ClientSmartTextBox ID="ctbSubMenuName" Required="true" MaxLength="100" runat="server" CssStyle="float:left;width:95% !important;"></cc3:ClientSmartTextBox>
                            </td>
                        </tr>
                    </tbody>
                    </table>

                    <div style="clear:both;padding:20px 0px 0px 0px;"></div>

                    <div class="mid">
                    <ul class="cm_menu_item">
                            <asp:ListBox ID="lbxSubCategory" runat="server" Width="100%" Rows="26" style="height:350px;"></asp:ListBox>
                    </ul>
                    </div>  
                </div>
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
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬      
        var ImagePath = "<%=BaseImgPath%>";
        var CategoryID = 0;
        var SubCategoryID = 0;
        var DelCheck = "0";
        var MultiLang = "<%=MultiLang %>";
        var R_BaseName = "<%=GetResource("BaseName") %>";
        var R_SubBaseName = "<%=GetResource("SubBaseName") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_CheckModList = "<%=GetResource("CheckModList") %>";
        var R_CheckDelList = "<%=GetResource("CheckDelList") %>";
        var R_CheckCategory = "<%=GetResource("CheckCategory") %>";
        var R_DelGubun = "<%=GetResource("DelGubun") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Admin/Category/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Admin/Category/List.js");
    </script>
</body>
</html>
