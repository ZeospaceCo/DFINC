<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Tab.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Approval.Tab"  EnableViewState="false" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var AppVersion = "<%=AppVersion%>";
    var ApprovalStatusMenuUrl = "<%=ApprovalStatusMenuUrl%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnTabPageSize_" + portletDom).val("<%=PageSize %>");
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
            var paramValue = $("#hdnTabSelParamValue_" + portletDom).val();
	    
            if(paramValue != ""){
                //gubun=Approval^dcparm=approval
                var paramValueStr = paramValue.split("^");
                var menuCatID = "";
                var folderStatusID = "", dcparm = "", cabinetID = "";

                for(var i=0;i<paramValueStr.length;i++)
                {
                    switch(paramValueStr[i].split("=")[0])
                    {                        
                        case "folderStatusID" :
                            folderStatusID = paramValueStr[i].split("=")[1]; //process / complete
                            break;
                        case "dcparm" :
                            dcparm = paramValueStr[i].split("=")[1];
                            break;
                        case "cabinetID" :
                            cabinetID = paramValueStr[i].split("=")[1];
                            break;
                    }
                }
                
                ParamArray = {
                    'folderStatusID': folderStatusID,
                    'dcparm': dcparm,
                    'cabinetID' : cabinetID
                };

                CallEncrypt(CallGetParam());   

                parent.frames["frmTop"].Menu.GoParam(dcparm);
		
                if(dcparm == "approvalStatus")
                {                    
                    var statusUrl = (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/DeskplusEIP/Approval/Status/Default.aspx";
                    location = statusUrl + "?p=" + GetEncryptValue;
                }
                else if(dcparm == "approvalComplete")
                {
                    var statusUrl = (typeof ApprovalStatusMenuUrl != "undefined" && ApprovalStatusMenuUrl != "") ? ApprovalStatusMenuUrl : "/DeskplusEIP/Approval/Status/Default.aspx";
                    if (statusUrl.toUpperCase().indexOf("/STATUS/DEFAULTALL.ASPX") > -1) {
                        ParamArray = { 'folderStatusID': GetApprovalCompleteFolderStatusID(cabinetID) };
                        CallEncrypt(CallGetParam());
                        location = statusUrl + "?p=" + GetEncryptValue;
                    } else {
                        location = "/DeskplusEIP/Approval/Folder/Default.aspx?p=" + GetEncryptValue;
                    }
                }
            }
        });

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            tabBPageSize = $("#boardListCount_" + portletDom).val();
            $("#hdnTabPageSize_" + portletDom).val(tabBPageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", tabBPageSize, "");

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

        //setTimeout(function () {
            LoadSelTab_<%=TabPortletID%>_<%=PortletID%>("<%=TabPortletFirst %>");
        //}, 1);
    });

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭선택시
    ***************************************************/
    function LoadSelTab_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        $("#selapprovaltab_" + portletDom + "_" + $("#hdnTabSelID_" + portletDom).val()).attr('class','btn btn-sm');//모든 tab        
        $("#selapprovaltab_" + portletDom + "_" + p_portletID).attr('class','btn btn-sm btn-secondary'); //선택한 tab  
        // 첫 페이지로 세팅
        $("#hdnTabCount_" + portletDom).val("1");  
        
        LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
        //$(".ptl_more").show();

        LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
        LoadTabList_<%=TabPortletID%>_<%=PortletID%>();

        $("#hdnTabSelID_" + portletDom).val(p_portletID);

        LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>();
    }

    function GetApprovalCompleteFolderStatusID(p_cabinetID) {
        switch ((p_cabinetID || "").toUpperCase()) {
            case "PRIVATE":
                return "PrivateCompleteAll";
            case "APPROVAL":
                return "ApproveCompleteAll";
            case "REJECT":
                return "ApproveRejectAll";
            case "ARBITRARY":
                return "ArbitraryAll";
            case "CARBONCOPY":
            case "CARBONCOPY_DOC":
                return "CarboncopyAll";
            case "CARBONCOPY_FORWARD":
                return "CarboncopyForwardAll";
            case "CARBONCOPY_CIRRECEIPT":
                return "CarboncopyCirReceiptAll";
            case "CARBONCOPY_CIRSEND":
                return "CarboncopyCirSendAll";
            case "DEPT":
                return "DeptCompleteAll";
            default:
                return "ApproveCompleteAll";
        }
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 탭별 게시판의 파라미터값 설정하기
    ***************************************************/
    function LoadSelTabParam_<%=TabPortletID%>_<%=PortletID%>()
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";        
        var selPortletID = $("#hdnTabSelID_" + portletDom).val();
        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();

        if(paramValueList != "")
        {
            //menuCatID=1^gubun=process 또는 complete
            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == selPortletID)
                {
                    //menuCatID=1^gubun=process 또는 complete
                    var paramValue = paramValueListStr[i].split(":")[1];                      
                    $("#hdnTabSelParamValue_" + portletDom).val(paramValue);                    
                }
            }            
        }        
    }

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 카테고리값 설정하기
    ***************************************************/
    function LoadTabCategory_<%=TabPortletID%>_<%=PortletID%>(p_portletID)
    {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //포틀릿아이디:menuCatID=47,~
        var paramValueList = $("#hdnTabParamValue_" + portletDom).val();  
        
        //포틀릿아이디:페이지      
        var urlValue = $("#hdnUrlValue_" + portletDom).val();
        
        if(paramValueList != "")
        {
            //menuCatID=1^gubun=process 또는 complete
            var paramValueListStr = paramValueList.split(",");
            for (var i = 0; i < paramValueListStr.length; i++) {
                var portletID = paramValueListStr[i].split(":")[0];
                
                if(portletID == p_portletID)
                {
                    //menuCatID=1^gubun=process 또는 complete
                    var paramValue = paramValueListStr[i].split(":")[1];  
                    
                    //menuCatID=1                  
                    var paramValueStr = paramValue.split("^");                    
                    
                    for (var j = 0; j < paramValueStr.length; j++) {
                    
                        var keyValue = paramValueStr[j].split("=");
                        if (keyValue[0] == "folderStatusID" || keyValue[0] == "cabinetID")
                        {
                           //구분자 process , complete
                            $("#hdnTabCategoryID_" + portletDom).val(keyValue[1]);
                            break;
                        }
                    }
                }
            }            
        }
        //리스트형
        $("#hdnTabUrl_" + portletDom).val("1");
    }


    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var gubun = $("#hdnTabCategoryID_" + portletDom).val();
        var urlPage = "";
        switch (gubun)
        {
            case "Approve":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetApproveListCnt";   //결재대상 문서 카운트
                break;
            case "Process":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetReportListCnt";   //상신문서 문서 카운트
                break;
            case "Carboncopy":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetCarbonListCnt";   //참조문서 문서 카운트
                break;
            case "Carboncopy_Forward":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetForwardListCnt";   //전달문서 문서 카운트
                break;
            case "Carboncopy_CirReceipt":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetCirListCnt";        //받은회람 문서 카운트
                break;
            default :
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetApproveListCnt";   //결재대상 문서 카운트
                break;
        }
        ParamArray = {
            "searchTerm" : '',
            "searchTermValue" : '',
            "searchField" : '',
            "searchValue" : ''
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

        var gubun = $("#hdnTabCategoryID_" + portletDom).val();        

        var urlPage = "";
        switch (gubun)
        {
            case "Approve":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetApproveList";   //결재대상 문서 카운트
                break;
            case "Process":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetReportList";   //상신문서 문서 카운트
                break;
            case "Carboncopy":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetCarbonList";   //참조문서 문서 카운트
                break;
            case "Carboncopy_Forward":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetForwardList";   //전달문서 문서 카운트
                break;
            case "Carboncopy_CirReceipt":
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetCirList";        //받은회람 문서 카운트
                break;
            default :
                urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetApproveList";   //결재대상 문서 카운트
                break;
        }
        ParamArray = {
            'totalItemCount': $("#hdnTabTotCount_" + portletDom).val(),
            'recordPerPage': $("#hdnTabPageSize_" + portletDom).val(),
            'pageSizePageNo': $("#hdnTabCount_" + portletDom).val(),
            "searchTerm" : '',
            "searchTermValue" : '',
            "searchField" : '',
            "searchValue" : ''
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

                var jtUrl = "";
                switch (gubun)
                {
                    case "Approve":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListA.htm?seq=" + Guids();
                        break;
                    case "Process":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListR.htm?seq=" + Guids();
                        break;
                    case "Carboncopy":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListC.htm?seq=" + Guids();
                        break;
                    case "Carboncopy_Forward":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListF.htm?seq=" + Guids();
                        break;
                    case "Carboncopy_CirReceipt":
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListCir.htm?seq=" + Guids();
                        break;
                    default :
                        jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTTabListA.htm?seq=" + Guids();
                        break;
                }
                $('#plList_' + portletDom).setTemplateURL(jtUrl, null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d); 
                
                if($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
                if($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

                

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
            //LoadTabListTotCount_<%=TabPortletID%>_<%=PortletID%>(p_portletID);
            LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    } 
    
    
    /**************************************************
        작성자   : shimpark
        작성일자 : 2011-04-11
        작성목적 : 결재 보기
    ***************************************************/
    function LoadSelTabView(p_docID, p_cabinetID) {
        ParamArray = { 
            'documentID': p_docID 
            ,'cabinetID' : p_cabinetID
        };
        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
        WindowObjectReference = window.open(url, "View" + p_docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    } 
    function LoadSelTabViewCir(_cirDocumentID) {
        ParamArray = { 
            'cirDocumentID': _cirDocumentID 
        };
        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Approval/Common/Popup/ViewCir.aspx?p=" + GetEncryptValue;
        WindowObjectReference = window.open(url, "View" + _cirDocumentID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    }


// 결재하기
TabApproveDoc = function (_cID, _dID, _seq, _apptag, _tuserid, _tdeptid) {

    if (_cID == "" || _cID == "0" || _dID == "" || _dID == "0" || _seq == "" || _seq == "0" || _apptag == "" || _tuserid == "" || _tuserid == "0" || _tdeptid == "" || _tdeptid == "0") return;

    ParamArray = {
        'companyID': _cID
        , 'documentID': _dID
        , 'sequence': _seq
        , 'approvalTag': _apptag
        , 'targetUserID': _tuserid
        , 'targetDeptID': _tdeptid
        , 'func': 'TabApproveComplete()'
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskplusEIP/Approval/Process/Basic/ApprovePopup.aspx?p=" + GetEncryptValue;
    window.open(url, "Approve" + _dID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}
TabApproveComplete = function () {
    LoadTabList_<%=TabPortletID%>_<%=PortletID%>();
}
    
</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/Approval/Approval.js"></script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-reports">    

       <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"  class="text-primary"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
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
                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="TabBList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>

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
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>

</div>
<!-- 포틀릿(리스트:일반형) End //-->
