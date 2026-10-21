<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Dept.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">       
                    <cc3:ClientSmartButton ID="cbtnMove" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>             
                    <cc3:ClientSmartButton ID="cbtnAdd" BeginEndTag="li" class="btn btn-secondary" runat="server"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" class="btn btn-danger" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                        <!-- 기본정보 -->
                        <table class="table_typeB">
                            <caption>
                                <%=GetResource("DeptFolderManage")%></caption>
                            <colgroup>
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tbody>     
                                 <tr>
                                    <th>
                                        <%=GetResource("UpperFolderName")%>
                                    </th>
                                    <td>
                                        <asp:Literal ID="ltlParentName" runat="server"></asp:Literal>
                                    </td>
                                </tr>                   
                                <tr>
                                    <th>
                                        <label title=""><%=GetResource("FolderName")%></label>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox ID="ctbName" Required="true" MaxLength="100" runat="server"
                                            CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label title="">
                                            <%=GetResource("MultiLang")%></label>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="add" /></a>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox ID="ctbMuliLang" MaxLength="100" runat="server"
                                            CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>    
                                <tr style="display:none">
                                    <th>
                                       <%=GetResource("Active")%>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlActive" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>                    
                            </tbody>
                        </table>
                    </div><!-- end col-3 content-area-body-right -->
                </div><!-- end row  -->
            </div>
        </div>
    </div>    
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">

        var CabinetID = "<%=CabinetID %>";
        var FolderID = "<%=FolderID %>";
        var Gubun = "<%=Gubun %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        
        //리소스
        var R_Msg_Folder = "<%=GetResource("Msg_Folder")%>";   //"폴더명을 입력해 주세요";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>"; 		//처리되었습니다.
        var R_MSG_Refresh = "<%=GetResource("MSG_Refresh")%>"; 		//문서함을 갱신합니다.
        var R_MSG_Del5 = "<%=GetResource("MSG_Del5")%>"; 		//"정말로 삭제하시겠습니까?"
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Dept/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Dept/Add.js");
    </script>
</body>
</html>
