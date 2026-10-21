<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabMain.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.TabMain" EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var WebmailURL = "<%= WebmailURL %>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
        $("#boardListCount_" + portletDom).val("<%=PageSize %>");

        var tabBPageSize = $("#hdnTabPageSize_" + portletDom).val();        

        if ("<%=IsAuth%>" != "True") {
            $("#cbtnPDel_" + portletDom).remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
           $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
           $("#ptContent_" + portletDom).fadeOut("slow");   
        }

          
        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            // 리스트 행 개수 바인딩
            $("#boardListCount_" + portletDom).val($("#hdnTabPageSize_" + portletDom).val());
            $("#ptListConfig_" + portletDom).slideToggle(600);      
        });  

        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });

        //Email 바로가기
        $("#portlet_" + portletDom + " .title").on("click", function (e) {
            e.preventDefault();

            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
            location.href = WebmailURL;
        });


        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                // 첫 페이지로 세팅
                $("#hdnTabCount_" + portletDom).val("1");
                //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });

        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();           
            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webmail";
            location.href = WebmailURL;

            /*
            e.preventDefault();
            var paramValue = $("#hdnTabSelParamValue_" + portletDom).val();
            if(paramValue != ""){
                //menuCatID=1^catID=57^dcparm=BOARD1
                var paramValueStr = paramValue.split("^");
                var menuCatID = "";
                var catID = "";
                var dcparm = "";

                for(var i=0;i<paramValueStr.length;i++)
                {
                    switch(paramValueStr[i].split("=")[0])
                    {
                        case "menuCatID" :
                            menuCatID = paramValueStr[i].split("=")[1];
                            break;
                        case "catID" :
                            catID = paramValueStr[i].split("=")[1];
                            break;
                        case "dcparm" :
                            dcparm = paramValueStr[i].split("=")[1];
                            break;
                    }
                }
                
                ParamArray = {
                    'menuCatID': menuCatID,
                    'catID': catID,
                    'dcparm': dcparm
                };

                CallEncrypt(CallGetParam());            
                location = "/DeskPlusEIP/Board/Default.aspx?p=" + GetEncryptValue;
            }
            */
        });

        //환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);
            
            var mailbox = "";
            if($("#hdnTabSelModule_" + portletDom).val() == "mailbox") mailbox = $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").val();

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, mailbox);

            // 첫 페이지로 세팅
            $("#hdnTabCount_" + portletDom).val("1");
            //LoadBoardList_<%=TabPortletID%>_<%=PortletID%>();
            LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });


        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        //$("#ptListConfig_" + portletDom).hide();

        $("#hdnTabCount_" + portletDom).val("1");

        //LoadMyMailBox_<%=TabPortletID%>_<%=PortletID%>();

        //setTimeout(function () {
            LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
        //}, 1);        

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

        //etcparam 은 사용자가 선택한 메일박스값을 넘겨받아 설정한다.
        $("#mailBox_<%=TabPortletID%>_<%=PortletID%>").val("<%=EtcParam %>");
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

        $("#seltemptab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','btn btn-sm');//모든 tab        
        $("#seltemptab_" + portletDom + "_" + p_portletID).attr('class','btn btn-sm btn-secondary'); //선택한 tab     

        // 첫 페이지로 세팅
        $("#hdnTabCount_" + portletDom).val("1"); 
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
        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
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
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

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
                
                if(portletID == p_portletID)
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

                if(portletID == p_portletID)
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
            'pageSizeNo': $("#hdnTabCount_" + portletDom).val(),      // 페이지Nuumber
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

                if(module == "board") //게시판용
                {
                    switch($("#hdnTabUrl_" + portletDom).val())
                    {
                        case "1":       // 리스트형
                            {                            
                                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTList.htm?seq=" + Guids(), null, { filter_data: false });
                                $('#plList_' + portletDom).processTemplate(data.d); 
                                break;
                            }
                        case "2":       // 블로그형
                            {
                                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTBlog.htm?seq=" + Guids(), null, { filter_data: false });
                                $('#plList_' + portletDom).processTemplate(data.d); 
                                break;
                            }
                        case "3":       // 앨범형
                            {
                                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTAlbum.htm?seq=" + Guids(), null, { filter_data: false });
                                $('#plList_' + portletDom).processTemplate(data.d); 
                            
                                 $("#plList_" + portletDom + " .imageSlider .imageList").jCarouselLite({
                                    btnNext: "#plList_" + portletDom + " .imageSlider .right",
                                    btnPrev: "#plList_" + portletDom + " .imageSlider .left",
                                    visible: Math.floor($("#plList_" + portletDom).width() / 160),
                                    speed: 800
                                });
                                break;
                            }
                        default:
                            {
                            
                                $('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTList.htm?seq=" + Guids(), null, { filter_data: false });
                                $('#plList_' + portletDom).processTemplate(data.d); 
                                break;
                            }
                    }
                } 
                else
                {
                    var jtListUrl =  $("#hdnTabSelJTList_" + portletDom).val(); //jtList 경로 반환  
              
                    $('#plList_' + portletDom).setTemplateURL(jtListUrl + "?seq=" + Guids(), null, { filter_data: false });
                    $('#plList_' + portletDom).processTemplate(data.d); 
                }
                
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
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function TabBList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardList_PageSize = $("#hdnTabPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardList_Count = $("#hdnTabCount_" + portletDom).val();              // 페이지Nuumber
        var boardList_TotalCount = $("#hdnTabTotCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var boardList_MaxCount = 1;
        if (boardList_TotalCount > 0) {

            // 몫
            var mok = parseInt((boardList_TotalCount / boardList_PageSize));
            // 나머지
            var balance = boardList_TotalCount % boardList_PageSize;

            if (balance != 0) {
                boardList_MaxCount = mok + 1;                
            }
            else {
                boardList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            boardList_Count = parseInt(boardList_Count) + 1;
        }
        else if (arg == "before") {
            boardList_Count = parseInt(boardList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (boardList_Count < 1) {
            boardList_Count = 1;
            $("#hdnTabCount_" + portletDom).val(boardList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardList_Count > boardList_MaxCount) {            
            boardList_Count = boardList_MaxCount;
            $("#hdnTabCount_" + portletDom).val(boardList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnTabCount_" + portletDom).val(boardList_Count);

        //setTimeout(function () {
            //LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    }  

    
</script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/Mail/Mail.js"></script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">

	   <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a id="ptPreview_<%=TabPortletID%>_<%=PortletID%>" class="text-primary">E-Mail&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
                 <!-- 탭 //-->
                <div class="nav-tabs">                      
                    <%=TabPortletTitle %>
                </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

        <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="boardListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </div>
                </div>
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">


                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
            </div>
        </div>

        <div style="display: none">
            <input type="hidden" id="hdnTabCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabCount_<%=TabPortletID%>_<%=PortletID%>" />
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
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />        
        </div>
</div>

