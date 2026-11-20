DashBoard = { Targets: [], AppliedSearch: null, Loading: false, DataLoaded: false,
    CourseRows: [], FilteredCourseRows: [], CoursePage: 1, CoursePageSize: 5 };

$(function () {
    if (!HasAccess) return;
    DashBoard.RestoreSearch(InitialSearch || {});
    DashBoard.InitializeMonthPicker();
    $('#cbtnAdd').click(function (e) { e.preventDefault(); DashBoard.Add(); });
    $('#cbtnSearch').click(function (e) { e.preventDefault(); DashBoard.Search(); });
    $('#cbtnReset').click(function (e) { e.preventDefault(); DashBoard.ResetSearch(); });
    $('#ctbSearchTitle').keydown(function (e) { if (e.keyCode == 13) { e.preventDefault(); DashBoard.Search(); } });
    $('#cbtnSelectTargets').click(function (e) { e.preventDefault(); DashBoard.OpenTargets(); });
    $('#cbtnCoursePrevious').click(function (e) { e.preventDefault(); DashBoard.MoveCoursePage(-1); });
    $('#cbtnCourseNext').click(function (e) { e.preventDefault(); DashBoard.MoveCoursePage(1); });
    $('#ddlTarget').change(function () { DashBoard.Targets = []; DashBoard.DrawTargets(); });
    $('#form1').submit(function (e) { e.preventDefault(); DashBoard.Search(); });
    $('.edu-summary-count').click(function () { DashBoard.SelectCourseStatus($(this).attr('data-status')); })
        .keydown(function (e) { if (e.keyCode == 13 || e.keyCode == 32) { e.preventDefault(); $(this).click(); } });
    __addEventListener(document, 'click', DashBoard.click);
    $('#zeoModalContact').draggable({ disabled: false });
    DashBoard.Search();
});

DashBoard.PositiveNumber = function (value) {
    if (!/^[0-9]+$/.test(String(value))) return 0;
    var number = Number(value);
    return number > 0 && number <= 2147483647 ? number : 0;
};

DashBoard.Count = function (value) { return DashBoard.PositiveNumber(value); };

DashBoard.Rate = function (value) {
    var number = Number(value);
    return isFinite(number) ? Math.max(0, Math.min(100, Math.round(number * 10) / 10)) : 0;
};

DashBoard.EncodeHtml = function (value) {
    return String(value == null ? '' : value).replace(/&/g, '&amp;').replace(/</g, '&lt;')
        .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
};

DashBoard.RestoreSearch = function (search) {
    DashBoard.Targets = [];
    var hasPeriod = $.trim(search.startYM || '') != '' || $.trim(search.endYM || '') != '';
    $('#tbxStartYM').val(hasPeriod ? search.startYM || '' : DefaultSearchStartMonth);
    $('#tbxEndYM').val(hasPeriod ? search.endYM || '' : DefaultSearchMonth);
    $('#ddlTarget').val(search.searchTarget == 'G' ? 'G' : 'A');
    $('#ddlGubun').val(search.searchItem || '');
    if ($('#ddlGubun').val() == null) $('#ddlGubun').val('');
    $('#ddlCourseStatus').val(/^[1-4]$/.test(String(search.courseStatus || '')) ? search.courseStatus : '');
    $('#ctbSearchTitle').val(search.searchTitle || '');
    var seen = {};
    if ($('#ddlTarget').val() == 'G' && String(search.targetIDs || '').split(',').length == 1) {
        $.each(String(search.targetIDs || '').split(','), function (_, value) {
            var id = DashBoard.PositiveNumber(value);
            if (id && !seen[id] && DashBoard.Targets.length < 1) {
                seen[id] = true; DashBoard.Targets.push({ id: id, name: '선택 ID ' + id });
            }
        });
    }
    DashBoard.DrawTargets();
    if (DashBoard.Targets.length && search.targetNames) $('#targetNames').val(search.targetNames);
};

DashBoard.DrawTargets = function () {
    var all = $('#ddlTarget').val() == 'A', names = [];
    if (all) DashBoard.Targets = [];
    $.each(DashBoard.Targets, function (_, target) { names.push(target.name); });
    $('#targetNames').val(names.join(', ')).toggle(!all);
    $('#cbtnSelectTargets').toggleClass('edu-disabled', all).attr('aria-disabled', all ? 'true' : 'false');
};

DashBoard.OpenTargets = function () {
    if ($('#ddlTarget').val() != 'G') return;
    ParamArray = { 'callBackEvent': 'DashBoard.SetTargets', 'targetType': 'G', 'singleDepartment': '1' };
    CallEncrypt(CallGetParam());
    PopupWin(800, 485, 'GetUserDept.aspx?p=' + encodeURIComponent(GetEncryptValue), '-500', '', true);
};

DashBoard.SetTargets = function (value) {
    if (!value || value == 'undefined' || $('#ddlTarget').val() != 'G') return false;
    var selected = [], seen = {}, invalid = false;
    // 조직도 원문은 unescape하지 않아 부서명에 포함된 % 문자도 그대로 표시합니다.
    $.each(String(value).split(';'), function (_, entry) {
        if (!entry) return;
        var parts = entry.split('$'), id = DashBoard.PositiveNumber(parts[1] || '');
        if (parts.length < 4 || parts[0] != 'G' || !id) { invalid = true; return; }
        if (!seen[id]) { seen[id] = true; selected.push({ id: id, name: parts.slice(3).join('$') || ('선택 ID ' + id) }); }
    });
    if (invalid) { alert('부서만 선택해 주세요. 사용자는 선택 목록에서 삭제해 주세요.'); return false; }
    if (selected.length != 1) { alert('부서는 한 개만 선택해 주세요.'); return false; }
    DashBoard.Targets = selected; DashBoard.DrawTargets(); return true;
};

DashBoard.MonthNumber = function (value) {
    var match = /^(\d{4})-(\d{2})$/.exec(String(value));
    if (!match || +match[1] < 1900 || +match[1] > 9999 || +match[2] < 1 || +match[2] > 12) return 0;
    return +match[1] * 100 + +match[2];
};

DashBoard.InitializeMonthPicker = function () {
    $('.edu-month-input, .edu-month-trigger').click(function () {
        DashBoard.OpenMonthPicker($(this).attr('data-month-input') || this.id, this);
    });
    $('.edu-month-input').keydown(function (e) {
        if (e.keyCode == 13 || e.keyCode == 32 || e.keyCode == 40) {
            e.preventDefault(); DashBoard.OpenMonthPicker(this.id, this);
        }
    });
    $('#eduMonthPreviousYear').click(function () { DashBoard.ChangeMonthYear(-1); });
    $('#eduMonthNextYear').click(function () { DashBoard.ChangeMonthYear(1); });
    $('#eduMonthPicker').on('click', '[data-month]', function () {
        var value = $(this).attr('data-month');
        if (!DashBoard.MonthPicker || !DashBoard.MonthNumber(value)) return;
        $('#' + DashBoard.MonthPicker.inputID).val(value).trigger('change');
        DashBoard.CloseMonthPicker(true);
    }).keydown(function (e) {
        if (e.keyCode == 27) { e.preventDefault(); DashBoard.CloseMonthPicker(true); }
    });
    $(document).on('click.eduMonthPicker focusin.eduMonthPicker', function (e) {
        if (!$(e.target).closest('#eduMonthPicker, .edu-month-input, .edu-month-trigger').length)
            DashBoard.CloseMonthPicker(false);
    });
    $(window).on('resize.eduMonthPicker scroll.eduMonthPicker', function () { DashBoard.CloseMonthPicker(false); });
};

DashBoard.OpenMonthPicker = function (inputID, trigger) {
    if (inputID != 'tbxStartYM' && inputID != 'tbxEndYM') return;
    if (DashBoard.MonthPicker && DashBoard.MonthPicker.inputID == inputID) { DashBoard.CloseMonthPicker(true); return; }
    DashBoard.CloseMonthPicker(false);
    var value = $('#' + inputID).val(), year = +(DashBoard.MonthNumber(value) ? value : DefaultSearchMonth).substring(0, 4);
    DashBoard.MonthPicker = { inputID: inputID, trigger: trigger, year: year };
    DashBoard.RenderMonthPicker();
    $('#' + inputID).attr('aria-expanded', 'true');
    $('.edu-month-trigger[data-month-input="' + inputID + '"]').attr('aria-expanded', 'true');
    var picker = $('#eduMonthPicker').show(), bounds = document.getElementById(inputID).getBoundingClientRect();
    var left = Math.max(8, Math.min(bounds.left, $(window).width() - picker.outerWidth() - 8));
    var top = bounds.bottom + 5;
    if (top + picker.outerHeight() > $(window).height() - 8) top = Math.max(8, bounds.top - picker.outerHeight() - 5);
    picker.css({ left: left, top: top });
    DashBoard.FocusMonthPicker();
};

DashBoard.RenderMonthPicker = function () {
    var state = DashBoard.MonthPicker, selected = $('#' + state.inputID).val(), html = '';
    $('#eduMonthPickerTitle').text(state.year + '년');
    // 현재 월에 따른 기간 제한 없이 서버가 지원하는 모든 연도를 선택합니다.
    $('#eduMonthPreviousYear').prop('disabled', state.year <= 1900);
    $('#eduMonthNextYear').prop('disabled', state.year >= 9999);
    for (var month = 1; month <= 12; month++) {
        var value = state.year + '-' + (month < 10 ? '0' : '') + month;
        var active = value == selected, current = value == DefaultSearchMonth;
        html += '<button type="button" data-month="' + value + '" class="' + (active ? 'is-selected ' : '')
            + (current ? 'is-current' : '') + '" aria-label="' + state.year + '년 ' + month + '월" aria-pressed="'
            + (active ? 'true' : 'false') + '">' + month + '월</button>';
    }
    $('#eduMonthPicker .edu-month-grid').html(html);
};

DashBoard.FocusMonthPicker = function () {
    var selected = $('#eduMonthPicker .is-selected:enabled');
    (selected.length ? selected : $('#eduMonthPicker [data-month]:enabled').first()).focus();
};

DashBoard.ChangeMonthYear = function (direction) {
    if (!DashBoard.MonthPicker || (direction != -1 && direction != 1)) return;
    var year = DashBoard.MonthPicker.year + direction;
    if (year < 1900 || year > 9999) return;
    DashBoard.MonthPicker.year = year;
    DashBoard.RenderMonthPicker();
    DashBoard.FocusMonthPicker();
};

DashBoard.CloseMonthPicker = function (restoreFocus) {
    var state = DashBoard.MonthPicker;
    if (!state) return;
    DashBoard.MonthPicker = null;
    $('#eduMonthPicker').hide();
    $('.edu-month-input, .edu-month-trigger').attr('aria-expanded', 'false');
    if (restoreFocus && state.trigger) $(state.trigger).focus();
};

DashBoard.ReadSearch = function () {
    var start = $('#tbxStartYM').val(), end = $('#tbxEndYM').val();
    var startNumber = DashBoard.MonthNumber(start), endNumber = DashBoard.MonthNumber(end);
    if (!startNumber || !endNumber || startNumber > endNumber) { alert('시작 월과 종료 월을 올바르게 선택해 주세요.'); return null; }
    var target = $('#ddlTarget').val(), ids = [];
    $.each(DashBoard.Targets, function (_, selected) { ids.push(selected.id); });
    if (target == 'G' && !ids.length) { alert('조회할 부서를 선택해 주세요.'); return null; }
    if (target == 'G' && ids.length != 1) { alert('부서는 한 개만 선택해 주세요.'); return null; }
    var courseStatus = $('#ddlCourseStatus').val() || '';
    if (courseStatus != '' && !/^[1-4]$/.test(courseStatus)) { alert('진행상태를 올바르게 선택해 주세요.'); return null; }
    var searchTitle = $.trim($('#ctbSearchTitle').val() || '');
    if (searchTitle.length > 100) { alert('과정명은 100자 이내로 입력해 주세요.'); $('#ctbSearchTitle').focus(); return null; }
    return { startYM: start, endYM: end, searchTarget: target == 'G' ? 'G' : 'A',
        targetIDs: target == 'G' ? ids.join(',') : '', targetNames: target == 'G' ? $('#targetNames').val() : '',
        searchItem: $('#ddlGubun').val() || '', courseStatus: courseStatus, searchTitle: searchTitle };
};

DashBoard.ResetSearch = function () {
    if (DashBoard.Loading) return;
    DashBoard.CloseMonthPicker(false);
    $('#tbxStartYM').val(DefaultSearchStartMonth);
    $('#tbxEndYM').val(DefaultSearchMonth);
    $('#ddlTarget').val('A');
    $('#ddlGubun, #ddlCourseStatus, #ctbSearchTitle').val('');
    DashBoard.Targets = [];
    DashBoard.DrawTargets();
    DashBoard.Search();
};

DashBoard.Search = function () {
    if (DashBoard.Loading) return;
    DashBoard.CloseMonthPicker(false);
    var search = DashBoard.ReadSearch();
    if (!search) return;
    DashBoard.Loading = true; DashBoard.DataLoaded = false;
    $('#loading').html("<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' alt='조회 중' />");
    $('.edu-summary-count').attr('aria-disabled', 'true');
    DashBoard.BindCoursePager();
    setTimeout(function () {
        try { DashBoard.GetList(search); }
        finally {
            DashBoard.Loading = false; $('#loading').empty();
            $('.edu-summary-count').attr('aria-disabled', 'false');
            if (!DashBoard.DataLoaded) {
                DashBoard.AppliedSearch = null;
                DashBoard.CourseRows = []; DashBoard.FilteredCourseRows = []; DashBoard.CoursePage = 1;
                DashBoard.BindSummary({});
                DashBoard.MarkCourseStatus('');
                $('#phList').text('목록을 불러오지 못했습니다. 검색을 다시 실행해 주세요.');
            }
            DashBoard.BindCoursePager();
        }
    }, 100);
};

DashBoard.GetList = function (search) {
    ParamArray = search;
    CallEncrypt(CallGetParam());
    AjaxService('/DeskPlusEIP/Education/Ws/Admin/DashBoard.asmx/GetList', JSON.stringify({ p: GetEncryptValue }), function (response) {
        if (!response || !response.d) return;
        var data = response.d;
        $('#phList').setTemplateURL('/DeskPlusEIP/Education/Jt/Admin/JTDashBoard.htm?seq=' + Guids(), null, { filter_data: false });
        DashBoard.CourseRows = DashBoard.TemplateRows(data.ListData);
        DashBoard.FilterCourses(search.courseStatus || '');
        DashBoard.BindSummary(data);
        // 세부 조회는 변경 중인 입력값이 아니라 화면에 실제 적용된 검색조건을 사용합니다.
        DashBoard.AppliedSearch = $.extend({}, search);
        DashBoard.DataLoaded = true;
    });
};

DashBoard.RenderCoursePage = function () {
    var pageCount = Math.ceil(DashBoard.FilteredCourseRows.length / DashBoard.CoursePageSize);
    DashBoard.CoursePage = Math.min(Math.max(1, DashBoard.PositiveNumber(DashBoard.CoursePage) || 1), Math.max(1, pageCount));
    var start = (DashBoard.CoursePage - 1) * DashBoard.CoursePageSize;
    $('#phList').processTemplate(DashBoard.FilteredCourseRows.slice(start, start + DashBoard.CoursePageSize));
    DashBoard.BindCoursePager();
};

DashBoard.FilterCourses = function (status) {
    DashBoard.FilteredCourseRows = $.grep(DashBoard.CourseRows, function (course) { return status == '' || course.Status == status; });
    DashBoard.CoursePage = 1;
    DashBoard.MarkCourseStatus(status);
    DashBoard.RenderCoursePage();
};

DashBoard.MarkCourseStatus = function (status) {
    $('.edu-summary-count').each(function () {
        var selected = ($(this).attr('data-status') || '') == status;
        $(this).toggleClass('is-selected', selected).attr('aria-pressed', selected ? 'true' : 'false');
    });
};

DashBoard.SelectCourseStatus = function (status) {
    if (DashBoard.Loading || !DashBoard.DataLoaded || !DashBoard.AppliedSearch) return;
    status = String(status == null ? '' : status);
    if (status != '' && !/^[1-4]$/.test(status)) return;
    $('#ddlCourseStatus').val(status);
    DashBoard.AppliedSearch.courseStatus = status;
    DashBoard.FilterCourses(status);
};

DashBoard.BindCoursePager = function () {
    var pageCount = Math.ceil(DashBoard.FilteredCourseRows.length / DashBoard.CoursePageSize);
    var disabled = DashBoard.Loading || !DashBoard.DataLoaded || !DashBoard.AppliedSearch;
    var previousDisabled = disabled || DashBoard.CoursePage <= 1;
    var nextDisabled = disabled || DashBoard.CoursePage >= pageCount;
    $('#cbtnCoursePrevious').prop('disabled', previousDisabled).attr('aria-disabled', previousDisabled ? 'true' : 'false');
    $('#cbtnCourseNext').prop('disabled', nextDisabled).attr('aria-disabled', nextDisabled ? 'true' : 'false');
    $('#coursePageInfo').text(pageCount > 0 ? DashBoard.CoursePage + ' / ' + pageCount + ' 페이지' : '0 / 0 페이지');
};

DashBoard.MoveCoursePage = function (direction) {
    if (DashBoard.Loading || !DashBoard.DataLoaded || !DashBoard.AppliedSearch || (direction !== -1 && direction !== 1)) return;
    var pageCount = Math.ceil(DashBoard.FilteredCourseRows.length / DashBoard.CoursePageSize);
    var page = DashBoard.CoursePage + direction;
    if (page < 1 || page > pageCount) return;
    DashBoard.CoursePage = page;
    DashBoard.RenderCoursePage();
};

DashBoard.BindSummary = function (data) {
    var completeRate = DashBoard.Rate(data.CompleteRate);
    var states = [ { code: '', key: 'TotalCount', id: '#summaryTotalCount', label: '전체' },
        { code: '1', key: 'WaitCount', id: '#summaryWaitCount', label: '대기' },
        { code: '2', key: 'ProgressCount', id: '#summaryProgressCount', label: '진행' },
        { code: '3', key: 'CompleteCount', id: '#summaryCompleteCount', label: '완료' },
        { code: '4', key: 'DelayCount', id: '#summaryDelayCount', label: '지연' } ];
    $.each(states, function (_, state) {
        var count = DashBoard.Count(data[state.key]);
        $(state.id).text(count);
        var label = state.label + ' ' + count + '건';
        if (state.code == '3') label += ', 완료율 ' + completeRate + '%';
        $('.edu-summary-count[data-status="' + state.code + '"]').attr('aria-label', label + ' 조회');
    });
    $('#summaryCompleteRate').text(completeRate);
};

DashBoard.ProgressDistribution = function (row) {
    var total = row.TotalCount, counts = [row.CompleteCount, row.WaitCount, row.ProgressCount, row.DelayCount];
    var keys = ['CompleteWidth', 'WaitWidth', 'ProgressWidth', 'DelayWidth'];
    var distribution = {}, cumulative = 0, previous = 0, scale = 1000000;
    for (var i = 0; i < counts.length; i++) {
        cumulative += counts[i];
        // 누적 경계를 반올림해 구간 폭의 합이 100%를 넘지 않도록 합니다.
        var boundary = total > 0 ? Math.round(Math.min(cumulative, total) / total * scale) : 0;
        distribution[keys[i]] = (boundary - previous) / 10000;
        previous = boundary;
    }
    var label = '대상자 ' + total + '명, 완료 ' + row.CompleteCount + '명, 대기 ' + row.WaitCount
        + '명, 진행 ' + row.ProgressCount + '명, 지연 ' + row.DelayCount + '명';
    if (cumulative < total) label += ', 상태 미확인 ' + (total - cumulative) + '명';
    if (cumulative > total) label += ', 상태 집계가 전체보다 많아 그래프 범위를 제한합니다';
    distribution.DistributionLabel = DashBoard.EncodeHtml(label);
    return distribution;
};

DashBoard.TemplateRows = function (data) {
    var rows = [];
    $.each(data || [], function (_, source) {
        if (!source) return;
        var id = DashBoard.PositiveNumber(source.ListID);
        if (!id) return;
        var row = { ListID: id, ItemName: DashBoard.EncodeHtml(source.ItemName), Title: DashBoard.EncodeHtml(source.Title),
            Status: /^[1-4]$/.test(String(source.Status || '')) ? String(source.Status) : '',
            DateStr: DashBoard.EncodeHtml(source.DateStr), TotalCount: DashBoard.Count(source.TotalCount),
            CompleteCount: DashBoard.Count(source.CompleteCount), WaitCount: DashBoard.Count(source.WaitCount),
            ProgressCount: DashBoard.Count(source.ProgressCount), DelayCount: DashBoard.Count(source.DelayCount),
            CompleteRate: DashBoard.Rate(source.CompleteRate) };
        rows.push($.extend(row, DashBoard.ProgressDistribution(row)));
    });
    return rows;
};

DashBoard.click = function (e) {
    var evt = new __event(e), element = $(evt.target).closest('[action]'), action = element.attr('action');
    if (action != 'view' && action != 'targets') return;
    var nativeEvent = e || window.event;
    if (nativeEvent.preventDefault) nativeEvent.preventDefault(); else nativeEvent.returnValue = false;
    var listID = DashBoard.PositiveNumber(element.closest('[ListID]').attr('ListID'));
    if (!listID) return;
    if (action == 'targets') DashBoard.OpenEducationList(listID, element.attr('data-status'));
    else DashBoard.View(listID);
};

DashBoard.OpenEducationList = function (listID, status) {
    if (DashBoard.Loading || !DashBoard.DataLoaded || !DashBoard.AppliedSearch) return;
    listID = DashBoard.PositiveNumber(listID);
    if (!listID) return;
    status = String(status == null ? '' : status);
    if (status != '' && !/^[1-4]$/.test(status)) return;
    var search = DashBoard.AppliedSearch;
    var start = DashBoard.MonthNumber(search.startYM), end = DashBoard.MonthNumber(search.endYM);
    if (!start || !end || start > end) return;
    var lastDay = new Date(Math.floor(end / 100), end % 100, 0).getDate();
    // 사용자별 상태와 별개로 전체현황의 원래 검색조건을 보관합니다.
    ParamArray = $.extend({}, search);
    CallEncrypt(CallGetParam());
    var dashBoardSearch = GetEncryptValue;
    // 화면에 적용된 대상 조건도 관리자 목록의 검색조건으로 함께 전달합니다.
    ParamArray = { searchListID: listID, searchStatus: status, currentPage: 1, dashBoardSearch: dashBoardSearch,
        searchDateField: 'StartDate', startDate: search.startYM + '-01', endDate: search.endYM + '-' + lastDay,
        searchItem: search.searchItem || '', searchTarget: search.searchTarget == 'G' ? 'G' : 'A',
        targetIDs: search.searchTarget == 'G' ? search.targetIDs || '' : '',
        targetNames: search.searchTarget == 'G' ? search.targetNames || '' : '',
        searchUserField: 'Name', searchUserValue: '', searchTitle: '', searchEmployment: '', searchComplete: '' };
    CallEncrypt(CallGetParam());
    window.location.href = '/DeskPlusEIP/Education/Admin/List.aspx?p=' + encodeURIComponent(GetEncryptValue);
};

DashBoard.View = function (listID) {
    if (DashBoard.Loading || !DashBoard.AppliedSearch) return;
    ParamArray = $.extend({}, DashBoard.AppliedSearch, { listID: DashBoard.PositiveNumber(listID), returnPage: 'DashBoard' });
    CallEncrypt(CallGetParam());
    window.location.href = '/DeskPlusEIP/Education/Admin/View.aspx?p=' + encodeURIComponent(GetEncryptValue);
};

DashBoard.Add = function () {
    if (DashBoard.Loading) return;
    ParamArray = $.extend({}, DashBoard.AppliedSearch || InitialSearch || {});
    ParamArray.returnPage = 'DashBoard';
    ParamArray.searchDateValue = (ParamArray.startYM || '') + ',' + (ParamArray.endYM || '');
    CallEncrypt(CallGetParam());
    location = 'Add.aspx?p=' + encodeURIComponent(GetEncryptValue);
};
