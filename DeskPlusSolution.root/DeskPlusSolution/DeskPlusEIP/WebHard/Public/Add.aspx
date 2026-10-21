<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.WebHard.Web.Public.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Webhard.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Webhard.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var FolderID = "<%=FolderID %>";
        var Gubun = "<%=Gubun %>";
        var RowMax = "<%=RowMax %>";
        var PublicArrayList = "<%=PublicArrayList %>";        
        
        var PreLang = "<%=PreLang %>";
        var MultiLang = "<%=MultiLang %>";
        var ImagePath = "<%=BaseImgPath%>";        

        var R_Del = "<%=GetResource("Del") %>";
        var R_InputFolderName = "<%=GetResource("InputFolderName") %>";
        var R_InputFolderLength = "<%=GetResource("InputFolderLength") %>";
        var R_InputFileLength = "<%=GetResource("InputFileLength") %>";
        var R_InputFolderLengthInt = "<%=GetResource("InputFolderLengthInt") %>";
        var R_InputFileLengthInt = "<%=GetResource("InputFileLengthInt") %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        
        var R_Security = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")";
        var R_SelectAdd = "<%=GetResource("SelectAdd") %>";
        var R_SelectMe = "<%=GetResource("SelectMe") %>";
        var R_SelectCheckBox = "<%=GetResource("SelectCheckBox") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/WebHard/Js/Public/Add.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/WebHard/Js/Folder/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">  
    <div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <h1><asp:Literal ID="ltHeader" runat="server"></asp:Literal></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="CbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
            </ul>
        </div>          
     
        <div id="layout_srh" class="layout_srh">
        	<div class="float-left">
                <ul id="ulTabMenu" class="tab">
                    <li><a href="javascript:Add.SetTab('1');" class="tab_on" id="tabDivInfo"><span><%=GetResource("PublicRegInfo")%></span></a></li>
                    <li><a href="javascript:Add.SetTab('2');" id="tabDivAuth"><span><%=GetResource("PublicAuthSetting")%></span></a></li>      
                </ul>
            </div>
        </div>

        <div id="layout_content">
            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->         

                    <div id="divInfo">
                        <table class="table_typeB">
                        <caption></caption>
                        <colgroup>
                            <col width="120px" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("ParentFolderName")%></th>
                                        <td><cc3:ClientSmartTextBox ID="CTBParentName" ReadOnly="true" MaxLength="250" runat="server" CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>          
                            </tr>
                            <tr>
                                <th><%=GetResource("FolderName")%></th>
                                        <td><cc3:ClientSmartTextBox ID="CTBName" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                            </tr> 
                            <tr>
                                <th><%=GetResource("MultiName")%>&nbsp;<a id="MultiName"><img src="<%=BaseImgPath%>/Icon/ico_add.gif" alt="" /></a></th>
                                <td><cc3:ClientSmartTextBox ID="CTBMultiName" InputMode="None" MaxLength="250" runat="server" Required="true"  CssStyle="width:95% !important;" TitleAtt=""></cc3:ClientSmartTextBox></td>
                            </tr>                  
                            <tr>
                                <th><%=GetResource("PublicFolderLength")%></th>
                                <td><cc3:ClientSmartTextBox ID="CTBFolderLength" Width="200px" InputMode="None" MaxLength="250" runat="server" Required="true" TitleAtt=""></cc3:ClientSmartTextBox>
                                (KB) (ex : 1 GB => 1048576 KB)
                                </td>
                            </tr> 
                            <tr>
                                <th class="last-child"><%=GetResource("PublicFileLength")%></th>
                                <td class="last-child"><cc3:ClientSmartTextBox ID="CTBFileLength" Width="200px" InputMode="None" MaxLength="250" runat="server" Required="true" TitleAtt=""></cc3:ClientSmartTextBox>
                                (KB) (ex : 1 GB => 1048576 KB)
                                </td>
                            </tr> 
                        </tbody>
                        </table>
                    </div>

                    <div id="divAuth" style="display:none;">
                        <!-- 권한설정 start-->    
                        <div class="acc_auth_left">
                            <div class="layout_scroll"  style="width:270px;min-height:400px;height:400px;overflow:hidden">
                                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="true" UseCheckBoxMember="true" UseIcon="true" />
                            </div>
                        </div>
                        <div class="acc_auth_middle">
                            <ul>
                                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </div>
                        <div class="acc_auth_right">               
                            <table id="contentTable" class="table_typeT">  
                            <caption></caption>
                            <colgroup>
                                <col/>
                                <col width="60px" />
                                <col width="60px" />
                                <col width="70px"/>
                            </colgroup>
                            <thead>
                            <tr>
                                <th class="center"><%=GetResource("Title")%></th>
                                <th class="center"><%=GetResource("Download")%></th>
                                <th class="center"><%=GetResource("Upload")%></th>
                                <th class="center"><%=GetResource("Del")%></th>
                            </tr>      
                            </thead>
                            <tbody>
                                <%=PublicTable%>  
                            </tbody>  
                            </table> 
                        </div>
                        <!-- 권한설정 end-->                            
                    </div>
                </div>
            </div>
        </div>    
    </div>
    </div>

    <div style="display:none;">
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton>        
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>               
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />    
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
</body>
</html>