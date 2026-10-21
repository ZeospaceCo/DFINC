<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Mail.Tab" EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        //ContextMenu 생성하기
        var conMenu = '<ul id="basicMenu_<%=TabPortletID%>_<%=PortletID%>" class="contextMenu">';
        conMenu += '<li class="del"><a href="#del"><%=GetResource("DelPortlet")%></a></li>';
        conMenu += '<li class="min"><a href="#min"><%=GetResource("SmallPortlet")%></a></li>';
        conMenu += '<li class="max"><a href="#max"><%=GetResource("LargePortlet")%></a></li>';
        conMenu += '<li class="info"><a href="#info"><%=GetResource("PortletSetting")%></a></li>';
        conMenu += '</ul>';
        $("#MenuList").append(conMenu);

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#boardListCount_" + portletDom).val("<%=PageSize %>");

        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();
        

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu_<%=TabPortletID%>_<%=PortletID%> .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        // 포틀릿 설정
        $("#ptConfig_" + portletDom).contextMenu({ menu: 'basicMenu_<%=TabPortletID%>_<%=PortletID%>', leftButton: true }, function (action, el, pos) {
            var pID = el[0].id.replace('ptConfig_', '');
            Main.ContextMenuWork(pID, action, el, pos);

            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
        });

        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnTabPageNo_" + portletDom).val("1");
                //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });

        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
        });

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);
            
            var selMailbox = $("#mailBox_" + portletDom).val();//선택한 메일박스리스트
            var selTabFirstlID = $("#hdnTabFirst_" + portletDom).val();//선택한 탭 설정값 ##
            var etcparms = selMailbox + "," + selTabFirstlID;
            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, etcparms);

            // 첫 페이지로 세팅
            $("#hdnTabPageNo_" + portletDom).val("1");            
            LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();            
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });


        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 서브메뉴 - 초기탭 설정
        $("#cbtnFirst_" + portletDom).click(function (e) {
            e.preventDefault();

            var tabPageSize = $("#hdnTabPageSize_" + portletDom).val();

            var selMailbox = $("#mailBox_" + portletDom).val();//선택한 메일박스리스트
            var selTabFirstlID = $("#hdnTabSelID_" + portletDom).val();//선택한 탭 값 ##
            var etcparms = selMailbox + "," + selTabFirstlID;

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabPageSize, etcparms);

            $("#hdnTabFirst_" + portletDom).val(selTabFirstlID);//초기탭 값 변경하기
                        
            $("#ptListConfigBG_" + portletDom).show();
            $("#ptListConfig_" + portletDom).hide();
        });

        $("#hdnTabPageNo_" + portletDom).val("1");   
        
        //setTimeout(function () {
		        LoadMyMailBox_<%=TabPortletID%>_<%=PortletID%>();
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		//    }, 1);  
	
        /*
	    if ($.browser.msie) {
		    LoadMyMailBox_<%=TabPortletID%>_<%=PortletID%>();
		    var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		    LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
	    }
	    else
	    {
		    setTimeout(function () {
		        LoadMyMailBox_<%=TabPortletID%>_<%=PortletID%>();
		        var tabFirstID = $("#hdnTabFirst_" + portletDom).val();
		        LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(tabFirstID);
		    }, 1);
	    }
        */
    });

    
    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 제타 메일박스 리스트 가져오기
    ***************************************************/
    function LoadMyMailBox_<%=TabPortletID%>_<%=PortletID%>()
    {
        //** 제타 메일 (mailbox) 설정 시작
        $("select[id='mailBox_<%=TabPortletID%>_<%=PortletID%>']").find('option').each(function () {
            $(this).remove();
        });
        

        var pageUrl = "/DeskPlusEIP/Portlet/Ws/Package/Mail/List.asmx/GetMailBoxList";
        ParamArray = {            
        };
        
        CallEncrypt(CallGetParam());
        var prarmData = "{p:'" + GetEncryptValue + "'}";

        
        $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").get(0).options[0] = new Option("------", "");

        jQuery.ajax({
            type: "POST",  //GET 
            url: pageUrl,
            async: false,
            data: prarmData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {                
                if (data != null) {
                    for (var i = 0; i < data.d.length; i++) {
                        $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").get(0).options[i + 1] = new Option(data.d[i].Mailbox_name, data.d[i].Mailbox_code);
                    }
                }
            },
            error: function () {
                alert("error");
            }
        });

        var tabMailBox = $("#hdnTabMailBox_<%=TabPortletID%>_<%=PortletID%>").val();
        //etcparam 은 사용자가 선택한 메일박스값을 넘겨받아 설정한다.
        $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").val(tabMailBox);
        //** 제타 메일 (mailbox) 설정 종료
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#selboardtab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','');//모든 tab        
        $("#selboardtab_" + portletDom + "_" + p_portletID).attr('class','poboxon'); //선택한 tab     
        // 첫 페이지로 세팅
        $("#hdnTabPageNo_" + portletDom).val("1"); 
        $("#hdnTabSelID_" + portletDom).val(p_portletID); 

        // 앨범형태이면 하단의 페이징이 보이지 않아야함
        if($("#hdnTabUrl_" + portletDom).val() == "3")
        {
            $(".ptl_more").hide();
        }
        else
        {
            $(".ptl_more").show();
        }

        //선택 탭 파라미터 정의
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 총 카운트
        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>();
        //선택 탭 리스트
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 선택한 탭별 파리미터 설정
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>()
    {    
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var selPortletID = $("#hdnTabSelID_" + portletDom).val();
        //203:categoryID=1^pageSize=1,204:listcount=5
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //203:웹서비스경로,204:웹서비스경로
        var urlValue = $("#hdnUrlValue_" + portletDom).val();

        //카테고리값 초기화
        $("#hdnTabCategoryID_" + portletDom).val("");
        
        if(paramValueList != "")
        {
            //초기화
            $("#hdnTabCategoryID_" + portletDom).val("");
            $("#hdnTabUrl_" + portletDom).val("");
            $("#hdnTabSelModule_" + portletDom).val("");
            //$("#hdnTabPageSize_" + portletDom).val("");
            $("#hdnTabSelJTList_" + portletDom).val("");

            var paramValueListStr = paramValueList.split(",");
            //203:categoryID=1
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == selPortletID)
                {
                    //catID=2^listType=1
                    var paramValue = paramValueListStr[i].split(":")[1];  

                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                        var keyValue = paramValueStr[j].split("=");
                        if(keyValue[0] == "catID")
                        {
                           //게시판 카테고리값
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "listType")
                        {
                           //list 형태 :리스트(1);블로그(2);갤러리(3)
                            $("#hdnTabUrl_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "module")
                        {
                           //module (board, mail.....)
                            $("#hdnTabSelModule_" + portletDom).val(keyValue[1]);
                        } else if(keyValue[0] == "jtListUrl")
                        {
                           //jt html 경로
                            $("#hdnTabSelJTList_" + portletDom).val(keyValue[1]);
                        }
                    }
                }
            }            
        }
        
        //203:웹서비스경로,204:웹서비스경로
        if(urlValue != "")
        {
            var urlValueStr = urlValue.split(",");
            for(var i = 0; i < urlValueStr.length; i++)
            {
                var portletID = urlValueStr[i].split(":")[0];

                if(portletID == selPortletID)
                {
                    //웹서비스경로값 설정
                    $("#hdnTabSelWS_" + portletDom).val( urlValueStr[i].split(":")[1]);
                }
            }
        }        

        //mailbox 환경값 설정하기
        $("#showMailBox_" + portletDom).hide();
        if($("#hdnTabSelModule_" + portletDom).val() == "mailbox") $("#showMailBox_" + portletDom).show();         
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        
        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/TabMain.asmx/GetPageTotCount";

        ParamArray = {            
            'catID': $("#hdnTabCategoryID_" + portletDom).val(),
            'module': $("#hdnTabSelModule_" + portletDom).val()
        };

        //alert($("#hdnTabSelModule_" + portletDom).val());

        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#hdnTabTotCount_" + portletDom).val(data.d)
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadTabList_<%=TabPortletID%>_<%=PortletID%>() {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var module = $("#hdnTabSelModule_" + portletDom).val();
        var urlPage = $("#hdnTabSelWS_" + portletDom).val();        
        
        ParamArray = {
            'pageSize': $("#hdnTabPageSize_" + portletDom).val(),     // 리스트 행 개수
            'pageSizeNo': $("#hdnTabPageNo_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnTabTotCount_" + portletDom).val(), // 전체 페이지 카운트
            'categoryID': $("#hdnTabCategoryID_" + portletDom).val(),  // 카테고리 아이디
            'mailboxCode' : $("#mailBox_" + portletDom).val() //개인함 mailbox 값
        };
        
        CallEncrypt(CallGetParam());
        var paramData = "{p:'" + GetEncryptValue + "'}";
        
        $.ajax({
            type: "POST",
            url: urlPage,
            async: false,
            data: paramData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                var jtListUrl =  $("#hdnTabSelJTList_" + portletDom).val(); //jtList 경로 반환                    
                $('#plList_' + portletDom).setTemplateURL(jtListUrl + "?seq=" + Guids(), null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);
                
                if($(".NoListData") != undefined) {                    
                    $(".NoListData").text(R_NoContent);
                }

                if ($(".TitleIcon") != undefined) { $(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif'); }
                if ($(".PtlType3Left") != undefined) { $(".PtlType3Left").attr('src', ImagePath + '/Main/ptl_laytype3_left.gif'); }
                if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg2.gif'); }
                if ($(".PtlType3Right") != undefined) { $(".PtlType3Right").attr('src', ImagePath + '/Main/ptl_laytype3_right.gif'); }   
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });

        
    }

    /**************************************************
    작성자: freeman
    작성일:2012-02-07
    작성목적 : 포틀릿 Popup
    ***************************************************/
    function Mail_Popup(obj, mail_sn) {        

        var width = "1024";
        var height = "768";
        var left = Math.ceil((window.screen.availWidth - width) / 2);
        var top = Math.ceil((window.screen.availHeight - height) / 2);
        window.open('', 'mailWin', "history=no,resizable=1,status=no,scrollbars=yes,menuebar=no,width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);
        //document.location.reload();
        $(obj).find('font').attr('style', '');
    }
    
</script>
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">
    <ul class="pobox" onmouseover="this.className='pobox poboxon ';poset_<%=TabPortletID%>_<%=PortletID%>.className='posetover'" onmouseout="this.className='pobox ';poset_<%=TabPortletID%>_<%=PortletID%>.className='poset'">
        
        <div class="portlet-header">
            <!-- 상단 타이틀 //-->
            <li class="title headtitle"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%></a></li>
            <!-- 탭 //-->
            <li class="poboxtab">
                <%=TabPortletTitle %>
            </li>

            <li id="poset_<%=TabPortletID%>_<%=PortletID%>" class="poset"><a class="callContextMenu" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>"></a></li>
        </div>
        
        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-content">
            <!-- 환경설정 //-->
            <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" style="display: none;">
                <li class="poboxlistset">
                    <span class="listsetbox">
                        <div id="showMailBox_<%=TabPortletID%>_<%=PortletID%>">
                            <select id="mailBox_<%=TabPortletID%>_<%=PortletID%>"></select>
                        </div>
                        <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </span>
                    <span class="listsetbtn" style="padding-left:10px;">
                        <a id="cbtnFirst_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("FirstTabSetting")%></span></a>
                    </span>
                    <span class="listsetbtn">
                        <a id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                    <span class="listsetbtn">
                        <a id="cbtnClose_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Close")%></span></a>
                    </span>
                </li>
            </div>
            

            <!-- 내용 //-->
            <div id="plList_<%=TabPortletID%>_<%=PortletID%>">
                <div style="padding-top: 50px; text-align: center;">
                    <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                </div>
            </div>
            <div style="display: none">
                <input type="hidden" id="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageNo_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabTotCount_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabPageSize_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValueList %>" />
                <input type="hidden" id="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnUrlValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=UrlValueList %>" />
                <input type="hidden" id="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabUrl_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelID_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelParamValue_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelWS_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelModule_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSelJTList_<%=TabPortletID%>_<%=PortletID%>" />
                <input type="hidden" id="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabFirst_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletFirst %>" />
                <input type="hidden" id="hdnTabMailBox_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabMailBox_<%=TabPortletID%>_<%=PortletID%>" value="<%=TabPortletMailBox %>" />
            </div>
        </div>
    </ul>
</div>
<!-- 포틀릿(리스트:일반형) End //-->
