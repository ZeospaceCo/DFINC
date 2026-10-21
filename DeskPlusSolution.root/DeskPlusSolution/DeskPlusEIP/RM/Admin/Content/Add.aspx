<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Admin.Content.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_RM.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_RM.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("AddResources") %></h1>
                <ul class="btn_right">                    
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" eginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
                 <div class="row">
                    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->               
                    
                        <!-- 기본정보 -->
                        <table class="table_typeB">
                            <caption><%=GetResource("MenuAdmin")%></caption>
                            <colgroup>
                                <col width="120px" />
                                <col />
                                <col width="120px" />
                                <col width="100px" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("FolderName")%></th>
                                    <td><asp:Literal ID="ltFolderName" runat="server"></asp:Literal></td>
                                    <th><%=GetResource("QRCodeTitle")%></th>
                                    <td><asp:DropDownList ID="ddlQRCode" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("ResourcesTitle")%>"><%=GetResource("ResourcesTitle")%></label></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbName" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                                    <th><label title="<%=GetResource("Qty")%>"><%=GetResource("Qty")%></label></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbCount" Required="true" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("Code")%>"><%=GetResource("Code")%></label></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbCode" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                                    <th><label title="<%=GetResource("UsableQty")%>"><%=GetResource("UsableQty")%></label></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbReCount" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("Place")%>"><%=GetResource("Place")%></label></th>
                                    <td><cc3:ClientSmartTextBox ID="ctbPlace" MaxLength="100" runat="server" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox></td>
                                    <th><label title="<%=GetResource("Authority")%>"><%=GetResource("Authority")%></label></th>
                                    <td><asp:DropDownList ID="ddlAdminConfirm" runat="server" CssClass="searchselect form-control"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("IconName")%>"><%=GetResource("IconName")%></label></th>
                                    <td colspan="3" style="padding:12px" >  
                                    <asp:RadioButtonList ID="rblIcon" runat="server" RepeatLayout="UnorderedList" CssClass="rm_status"></asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th><label title="<%=GetResource("Content")%>"><%=GetResource("Content")%></label></th>
                                    <td colspan="3"><cc3:ClientSmartTextBox ID="ctbDesc" MaxLength="200" runat="server" CssStyle="width:95% !important;" TextMode="MultiLine"></cc3:ClientSmartTextBox></td>
                                </tr>
                            </tbody>
                        </table>
                        <!-- 기본정보 -->

                    </div><!-- end col content-area-body-right -->
					<div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                        <ul>
                            <!-- 이미지 파일 -->
                            <li class="title">
                                <%=GetResource("ImageFile")%>
                                <a id="cbtnAddFile" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </li>
                            <li><div id="phFile" style="clear: both;"></div></li>   
                            <!-- 이미지 파일 -->

                            <!-- 자원 담당자 -->
                            <li class="title">
                                <%=GetResource("ResourcesUser")%>
                                <a id="cbtnAddUser" title="<%=GetResource("AddUser")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                                <em> (<%=GetResource("ResourcesUserStr")%>) </em>            
                            </li>               
                            <li><ul id="phUserList" class='table_typeT ul_typeT'></ul></li>                         
                            <!-- 자원 담당자 -->
                         </ul>
                     </div>
                 </div>                 
            </div>
        </div>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>   
        <asp:TextBox ID="tbManager" Style="display: none"   runat="server" Width="0px"></asp:TextBox>   
        <asp:TextBox ID="tbManagerOne" Style="display: none"   runat="server" Width="0px"></asp:TextBox>  
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var RowMax = 0;
        var CategoryID = "<%=CategoryID%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var BtnCheck = "N";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID        
        var FileType = "*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.pjpeg;*.pjpg;*.tif;";
        var FileTypeDesc = "Images (*.jpg *.jpeg *.png *.gif *.bmp *.pjpeg *.pjpg *.tif)";
        var R_InputTitle = "<%=GetResource("InputTitle") %>";
        var R_InputCount = "<%=GetResource("InputCount") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Del = "<%=GetResource("Del")%>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ApplyDelete = "<%=GetResource("ApplyDelete") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_TitleName = "<%=GetResource("TitleName") %>";
        var R_DutyName = "<%=GetResource("DutyName") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_SelectUser = "<%=GetResource("SelectUser") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")";
        var R_InputNumber = "<%=GetResource("InputNumber") %>";
        var R_ResourceCount = "<%=GetResource("ResourceCount") %>";
        var R_ManagerGubun1 = "<%=GetResource("ManagerGubun1") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Admin/Content/Add.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Admin/Content/Add.js");
    </script>
</body>
</html>
