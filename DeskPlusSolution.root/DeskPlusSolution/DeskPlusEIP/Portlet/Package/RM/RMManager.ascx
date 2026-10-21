<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RMManager.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.RM.RMManager" %>
<script type="text/javascript">
    var RmMa_Count = 1;
    var RmMa_TotalCount = "<%=RmMaTotalCount %>";
    var RmMa_PageSize = "<%=PageSize %>";
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        // 리스트행 개수
        $("#hdnRmMaPageSize_" + portletDom).val("<%=PageSize %>");
        var rmMaListPageSize = $("#hdnRmMaPageSize_" + portletDom).val();

        // 리스트 전체 카운트
        $("#hdnRmMaTotalCount_" + portletDom).val("<%=RmMaTotalCount %>");

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
            $("#rmMaCount_" + portletDom).val($("#hdnRmMaPageSize_" + portletDom).val());
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
                $("#hdnRmMaCount_" + portletDom).val("1");
                LoadRmMaList_<%=TabPortletID%>_<%=PortletID%>();

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);            
        });

        //상세바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();              

            ParamArray = {
                'rmBasePage': 'AdminReserve'
            };

            CallEncrypt(CallGetParam());

            parent.frmTop.Menu.GoParam("Resource"); 
            document.location.href = "/DeskPlusEIP/RM/Default.aspx?p=" + GetEncryptValue;
            //parent.location.href = "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=Resource&t=rmBasePage|AdminReserve";
        });

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            // 리스트 행 개수 설정
            rmMaListPageSize = $("#rmMaCount_" + portletDom).val();
            $("#hdnRmMaPageSize_" + portletDom).val(rmMaListPageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", rmMaListPageSize, "");
            // 첫 페이지로 세팅
            $("#hdnRmMaCount_" + portletDom).val("1");
            LoadRmMaList_<%=TabPortletID%>_<%=PortletID%>();
          
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });      
        
        $("#hdnRmMaCount_" + portletDom).val("1");

        //setTimeout(function () {
            LoadRmMaList_<%=TabPortletID%>_<%=PortletID%>();
        //}, 1);
    });

    /**************************************************
    작성목적 : 페이지 가져오기
    ***************************************************/
    function LoadRmMaList_<%=TabPortletID%>_<%=PortletID%>() {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";
        var R_Top = "<%=GetResource("Top") %>";
        var R_Middle = "<%=GetResource("Middle")%>";
        var R_Bottom = "<%=GetResource("Bottom")%>";
        
        //게시판 리스트
        var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/RM/RMManager.asmx/GetRmMaList";

        ParamArray = {
            'pageSize': $("#hdnRmMaPageSize_" + portletDom).val(),      // 리스트 행 개수
            'pageSizeNo': $("#hdnRmMaCount_" + portletDom).val(),      // 페이지Nuumber
            'totalCount': $("#hdnRmMaTotalCount_" + portletDom).val()  // 전체 페이지 카운트
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
                
                $('#phRmMaList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/RM/JTRmMaList.htm?seq=" + Guids(), null, { filter_data: false });
                $('#phRmMaList_' + portletDom).processTemplate(data.d);

                if($(".RmMa_NoListData") != undefined) {$(".RmMa_NoListData").text(R_NoContent);}
            }
            ,
            error: function (data, status, err) {
                alert(R_ErrorConnection + data);
            }
        });
    }

    /**************************************************
    작성목적 : 포틀릿 페이징(arg; next(이후), before(이전))
    ***************************************************/
    function RmMaList_Change_<%=TabPortletID%>_<%=PortletID%>(arg) {
        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        var rmMaList_PageSize = $("#hdnRmMaPageSize_" + portletDom).val();        // 리스트 행 개수
        var rmMaList_Count = $("#hdnRmMaCount_" + portletDom).val();              // 페이지Nuumber
        var rmMaList_TotalCount = $("#hdnRmMaTotalCount_" + portletDom).val();    // 전체 페이지 카운트   
        
        // MAX페이지 가져오기
        var rmMaList_MaxCount = 1;
        if (rmMaList_TotalCount > 0) {

            // 몫
            var mok = parseInt((rmMaList_TotalCount / rmMaList_PageSize));
            // 나머지
            var balance = rmMaList_TotalCount % rmMaList_PageSize;

            if (balance != 0) {
                rmMaList_MaxCount = mok + 1;                
            }
            else {
                rmMaList_MaxCount = mok;
            }
        }        

        if (arg == "next") {
            rmMaList_Count = parseInt(rmMaList_Count) + 1;
        }
        else if (arg == "before") {
            rmMaList_Count = parseInt(rmMaList_Count) - 1;
        }
        
        // 1페이지에서 이전을 누를 때
        if (rmMaList_Count < 1) {
            rmMaList_Count = 1;
            $("#hdnRmMaCount_" + portletDom).val(rmMaList_Count);
            alert(R_FirstPage);
            return;
        }

        // 최종페이지 에서 이후버튼 누를 때 
        if (rmMaList_Count > rmMaList_MaxCount) {            
            rmMaList_Count = rmMaList_MaxCount;
            $("#hdnRmMaCount_" + portletDom).val(rmMaList_Count);

            alert(R_LastPage);
            return;
        }

        // 현재 페이지Nuumber 세팅
        $("#hdnRmMaCount_" + portletDom).val(rmMaList_Count);

        LoadRmMaList_<%=TabPortletID%>_<%=PortletID%>();
    }

    /**************************************************
    작성목적 : 자원관리 상세보기
    ***************************************************/
    function TabRmMaMain_PopupMyViewList(p_reservationID) { 

        ParamArray = { 'reservationID': p_reservationID };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + GetEncryptValue;
        window.open(url, "RM" + p_reservationID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");  
    }  
</script>

<!-- 포틀릿(리스트:일반형) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-reservation">    

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
                        <select id="rmMaCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
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
                <div class="content-tabs" id="phRmMaList_<%=TabPortletID%>_<%=PortletID%>"></div>      

                <!-- 이전/이후 //-->
                <nav aria-label="Page navigation example">
                    <ul class="pagination pagination-sm justify-content ptl_more">
                    <li class="page-item"><a class="page-link" tabindex="-1" onclick="RmMaList_Change_<%=TabPortletID%>_<%=PortletID%>('before')"><i class="fas fa-chevron-left"></i></a></li>
                    <li class="page-item"><a class="page-link" onclick="RmMaList_Change_<%=TabPortletID%>_<%=PortletID%>('next')"><i class="fas fa-chevron-right"></i></a></li>
                    </ul>
                </nav>
            </div>
        </div>     
        
        <div style="display: none">
            <input type="hidden" id="hdnRmMaTotalCount_<%=TabPortletID%>_<%=PortletID%>"  name="hdnRmMaTotalCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnRmMaPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnRmMaPageSize_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnRmMaCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnRmMaCount_<%=TabPortletID%>_<%=PortletID%>" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
</div>
<!-- 포틀릿(리스트:일반형) End //-->