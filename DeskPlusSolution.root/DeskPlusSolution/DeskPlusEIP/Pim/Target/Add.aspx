<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Target.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>

<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />      
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script> 
    <script type="text/javascript">
        var FileType = "*.*";
        var SwfUploadGUID = '<%=SwfUploadGUID %>'; //flash upload GUID           
        var R_FileName = '<%=GetResource("FileName") %>';
        var R_FileType = '<%=GetResource("FileType") %>';
        var R_FileLen = '<%=GetResource("FileLen") %>';
        var R_FileDel = '<%=GetResource("Del") %>';
        var R_ApplyDelete = '<%= GetResource("ApplyDelete") %>';
        var Add_InputTitle = '<%= GetResource("Add_InputTitle") %>';
        var Add_InputDate = '<%= GetResource("Add_InputDate") %>';
        var RNAME = '<%=GetResource("Name") %>';
        var RDEL = '<%=GetResource("Del") %>';
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var AlertScriptShort = '<%=GetResource("LimitSpecialCharacter") %>' + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = '<%=GetResource("AlertScriptContnet") %>';
        var TimeAlert = '<%=GetResource("TimeAlert") %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
        var BaseImgPath = '<%=BaseImgPath%>';
        var Lang = '<%=this.PreLang%>';
        var TextEmpty = '<%=GetResource("TextEmpty")%>';
        var PreLang = "<%=PreLang%>";
        function AddShare(user, uid) {

            $("#divShare").html('');

            uid = TextReverseToXss2(uid);
            $("#hdShareList").val(uid);

            var arrayid = uid.split(',');

            if (arrayid[0].length > 0) {
                for (var i = 0; i < arrayid.length - 1; i++) {
                    var arrayName = arrayid[i].split('$');

                    if (arrayName[0] == "M") {
                        if (arrayName[1] == UserID) {
                            alert(NoMine);
                        }
                        else {
                            if (arrayName[0].length > 0) {
                                $("#divShare").append(
		                              "<li>"
			                        + "<span>" + arrayName[2] + "</span>"
                                    + "<a onclick='Add.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + RDEL + "'><i class='fas fa-times-circle'></i></a>"
                                    + "</li>"
	                            );

                            }
                        }
                    }
                    else {
                        if (arrayName[0].length > 0) {
                            $("#divShare").append(
		                          "<li>"
			                    + "<span>" + arrayName[2] + "</span>"
                                + "<a onclick='Add.ShareUserClick(this)' value='" + arrayid[i] + "' style='display:inline-block;' title='" + RDEL + "'><i class='fas fa-times-circle'></i></a>"
                                + "</li>"
	                        );
                        }
                    }
                }
            }
            else {
                $("#divShare").html('<li></li>');
            }


            /*
            $("#divtempshare").hide();
            $("#hdShareList").val(uid);

            var DelImg = "<img src='" + BaseImgPath + '/Lang/' + Lang + "/ico_folderset_del.gif' alt='del' />";

            var arrayid = uid.split(',');
            var strHtml = "<table cellspacing='0' class='table_typeT'>";
            strHtml += "<caption></caption>";
            strHtml += "<colgroup><col  /><col width='90px' /></colgroup>";
            strHtml += "<tr><th id='r_name'>" + '<%=GetResource("Name") %>' + "</th><th id='r_title'>" + '<%=GetResource("Del") %>' + "</th></tr>";

            if (arrayid[0].length == 0) {
                strHtml += "<tr><td class='center'>" + TextEmpty + "</td><td>&nbsp;</td></tr>";

            }
            for (var i = 0; i < arrayid.length - 1; i++) {
                var arrayName = arrayid[i].split('$');

                if (arrayName[0].length > 0) {
                    strHtml = strHtml + "<tr><td class='center'>" + arrayName[2] + "</td>";
                    strHtml = strHtml + "<td class='center'><a onclick='javascript:Add.ShareUserClick(this)' value='" + arrayid[i] + "' >" + DelImg + "</a></td>";
                    strHtml = strHtml + "</tr>";
                }
            }
            strHtml += "</table>";
            $("#divShare").html(strHtml);
            */
        }
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Target/Add.js"></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Target/Add.js");
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
     <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("MarkReg")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnCancel" runat="server" ></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                 <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" >

                    <table   class="table_typeB"> 
                     <caption><%=GetResource("MarkManage")%></caption>
                     <colgroup>
                        <col width="80px" />
                        <col />
			        </colgroup>
                    <tr> 
                        <th><%=GetResource("MarkName")%></th> 
                        <td>
                           <input name="txttitle" id="txttitle" runat="server" type="text" class="text required" size="300" value="" style="width:95.5% !important;" />
                        </td> 
                    </tr> 
                    <tr> 
                        <th><%=GetResource("Period")%></th> 
                        <td>
                        <input name="startDate" id="startDate" runat="server" type="text" class="text required" value="" style="width:100px !important;" readonly="readonly"/> ~ 
                        <input name="endDate" id="endDate" runat="server" type="text" class="text required" value="" style="width:100px !important;" readonly="readonly" />
                        </td> 
                    </tr>              
                    </table> 

                 </div><!-- end col-9 content-area-body-left -->
                 <div class="col-12 col-lg-3 col-xl-4 content-area-body-right">
                    <ul> 
                        <!-- 공유대상 리스트 -->    
                        <li class="title">
                            <%=GetResource("ShareSetting")%>
                            <div class="float-right">                            
                            <a href="#" id="CbtnGetUser" title="<%=GetResource("ShareObjSearch")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                            </div>
                        </li>
                        <li><ul id="divShare" class="table_typeT ul_typeT"><li></li></ul></li>
                    </ul>  
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

    <div style="display:none;">
        <asp:TextBox ID="hdShareList" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>           
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
