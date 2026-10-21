<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.PopUp.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var RecordPerPage = "<%=RecordPerPage %>";
        var Order = "<%=Order %>";

        var TrySaveCnt = 0;

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";

        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_Add_InputTitle = "<%=GetResource("Add_InputTitle") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_Add_InputContent = "<%=GetResource("Add_InputContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";

        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        JsCall("/DeskPlusEIP/ETC/Js/PopUp/Add.js");
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_one">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("AddPopup")%></h1>
                    <!-- 버튼 //-->
                    <ul class="btn_right">                                                
                        <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                        <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>

                <div id="layout_content">
                    <div class="row">
				        <div class="col-12 content-area-body" style="padding-right:30px"><!-- width 100%(col-12) -->

                            <table cellspacing="0" class="table_typeB">
		                    <colgroup>
                                <col width="130px" />
                                <col />
                                <col width="130px" />
                                <col />
                                <col width="130px" />
                                <col />
		                    </colgroup>
		                    <tbody>
                            <tr>
                                <th><%=GetResource("Title")%></th>
                                <td colspan="5"><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                            </tr>
                            <tr id="tdDate">
                                <th><%=GetResource("StartDate")%></th>
                                <td>
                                    <input type="text" id="startDate" size="10" runat="server" class="clsDate" />
                                </td>
                                <th><%=GetResource("EndDate")%></th>
                                <td>
                                    <input type="text" id="endDate" size="10" runat="server" class="clsDate" />
                                </td>
                                <th><%=GetResource("Option")%></th>
                                <td><asp:RadioButtonList ID="rblOption" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></td>
                            </tr>
                            <tr style="display:none">
                                <th>Width</th>
                                <td><cc3:ClientTextBox CssClass="num_only" ID="ctbWidth" runat="server" value="520"></cc3:ClientTextBox>px</td>
                                <th>Height</th>
                                <td><cc3:ClientTextBox CssClass="num_only" ID="ctbHeight" runat="server" value="600"></cc3:ClientTextBox>px</td>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <th><%=GetResource("Size")%></th>
                                <td colspan="5"><input type="radio" name="rdopixel" value="400" style="margin-top: -1px; vertical-align: middle;"/>&nbsp;400*600 &nbsp; &nbsp; <input type="radio" name="rdopixel" value="520" style="margin-top: -1px; vertical-align: middle;" checked="checked"  />&nbsp;520*600 &nbsp; &nbsp; <input type="radio" name="rdopixel" value="800" style="margin-top: -1px; vertical-align: middle;" />&nbsp;800*600&nbsp;</td>
                            </tr>
                            </tbody>
                            </table>

                            <div id="tabs" class="layout_subbox">
                                <ul id="tab" class="tab" style="display:none">
                                    <li><a id="ActvieX"><span>ActiveX</span> </a></li>
                                    <li><a id="NonActiveX"  class="tab_on"><span>NonActiveX</span> </a></li>
                                </ul>
                                <div class="divNonEditor">
                                    <div id="divXFE" style="height: 5px;">    
                                    <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>  
                                        <script type="text/javascript">
                                            xfe = new XFE({
                                                basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                                width: '100%',
                                                height: '550px',
                                                ignoreMinHeight: true
                                            });
                                            xfe.render('divXFE');
                                        </script>
                                    </div>
                                </div>   
                              </div>                                                    
                        </div>
                     </div>        
                </div>
            </div>
        </div>  
        
    <div style="display:none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>        
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbScreenShot" Style="display: none"   runat="server" Width="0px"></asp:TextBox>
        <!-- ==================================================================================== -->
        </div>      
    </form>
</body>
</html>
