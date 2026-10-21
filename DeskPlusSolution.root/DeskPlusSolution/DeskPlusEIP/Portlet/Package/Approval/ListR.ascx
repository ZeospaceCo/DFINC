<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListR.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Approval.ListR" %>
<script type="text/javascript">
    var ApprovalListR_PageSize = "<%=PageSize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var UserID = "<%=CurrentUserInfo.UserID %>";
    var AppVersion = "<%=AppVersion%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnApprovalListRPageSize_" + portletDom).val("<%=PageSize %>");
        var approvalListR_PageSize = $("#hdnApprovalListRPageSize_" + portletDom).val();

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
            $("#approvalRListCount_" + portletDom).val($("#hdnApprovalListRPageSize_" + portletDom).val());
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
                $("#hdnApprovalListRCount_" + portletDom).val("1");

                LoadApprovalTotCountR_<%=TabPortletID%>_<%=PortletID%>();
                LoadApprovalListR_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();            
             
            parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=approvalStatus";
            //parent.frmTop.Menu.GoParam("WebApproval");
        });        

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            approvalListR_PageSize = $("#approvalRListCount_" + portletDom).val();
            $("#hdnApprovalListRPageSize_" + portletDom).val(approvalListR_PageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", approvalListR_PageSize, "");

            // 첫 페이지로 세팅
            $("#hdnApprovalListRCount_" + portletDom).val("1");

            LoadApprovalTotCountR_<%=TabPortletID%>_<%=PortletID%>();
            LoadApprovalListR_<%=TabPortletID%>_<%=PortletID%>();

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });        

        //$("#ptListConfig_" + portletDom).hide();
        //setTimeout(function () {
            // 첫 페이지로 세팅
            $("#hdnApprovalListRCount_" + portletDom).val("1");
            LoadApprovalTotCountR_<%=TabPortletID%>_<%=PortletID%>();
            LoadApprovalListR_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);

    });

    /**************************************************
    작성자: shimpark
    작성일:2012-02-07
    작성목적 : 총카운트 가져오기
    ***************************************************/
    function LoadApprovalTotCountR_<%=TabPortletID%>_<%=PortletID%>() {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetReportListCnt";   //상신문서 문서 카운트

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
                $("#hdnApprovalListRTotCount_" + portletDom).val(data.d)
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
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadApprovalListR_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Approval/Tab.asmx/GetReportList";   //상신문서 문서 리스트

        ParamArray = {
            'totalItemCount': $("#hdnApprovalListRTotCount_" + portletDom).val(),
            'recordPerPage': $("#hdnApprovalListRPageSize_" + portletDom).val(),
            'pageSizePageNo': $("#hdnApprovalListRCount_" + portletDom).val(),
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

                var jtUrl = "/DeskPlusEIP/Portlet/Jt/Package/Approval/JTListR.htm?seq=" + Guids();

                $('#plList_' + portletDom).setTemplateURL(jtUrl, null, { filter_data: false });
                $('#plList_' + portletDom).processTemplate(data.d);

                if($(".NoListData") != undefined) {$(".NoListData").text(R_NoContent);}
                if($(".TitleIcon") != undefined) {$(".TitleIcon").attr('src', ImagePath + '/main/ico_bullet2.gif');}
                if($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }

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
    function ApprovalListR_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var boardList_PageSize      = $("#hdnApprovalListRPageSize_" + portletDom).val();        // 리스트 행 개수
        var boardList_Count         = $("#hdnApprovalListRCount_" + portletDom).val();              // 페이지Number
        var boardList_TotalCount    = $("#hdnApprovalListRTotCount_" + portletDom).val();    // 전체 페이지 카운트   
        
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
            $("#hdnApprovalListRCount_" + portletDom).val(boardList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (boardList_Count > boardList_MaxCount) {            
            boardList_Count = boardList_MaxCount;
            $("#hdnApprovalListRCount_" + portletDom).val(boardList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnApprovalListRCount_" + portletDom).val(boardList_Count);

        //setTimeout(function () {
            LoadApprovalTotCountR_<%=TabPortletID%>_<%=PortletID%>();
            LoadApprovalListR_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    } 

    
    /**************************************************
        작성자   : shimpark
        작성일자 : 2011-04-11
        작성목적 : 결재 보기
    ***************************************************/
    function LoadSelTabView(p_docID) {

        ParamArray = {
            'documentID': p_docID
        };

        CallEncrypt(CallGetParam());
        var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
        WindowObjectReference = window.open(url, "View" + p_docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
        WindowObjectReference.focus();
    } 
      
</script>
<script type="text/javascript" src="/DeskPlusEIP/Portlet/Js/Package/Approval/Approval.js"></script>
<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">
       <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
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
                        <select id="approvalRListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
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
                <div class="nav-tabs">                                   
                </div>

                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="ApprovalListR_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="ApprovalListR_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>

            </div>
        </div>  
           
        <div style="display: none">
            <input type="hidden" id="hdnApprovalListRCount_<%=TabPortletID%>_<%=PortletID%>"  name="hdnApprovalListRCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnApprovalListRPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnApprovalListRPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnApprovalListRTotCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnApprovalListRTotCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>
<!-- 포틀릿(리스트:일반형) End //-->
