/*
* Pagination 1.0 - jQuery Plugin
* http://www.taeyo.net
* Copyright (c) 2009 Taeyoung Kim
* Dual licensed under the MIT and GPL licenses
*/
(function ($) {
    var iLoop = 1;
    var iLoopEnd = 10;
    var pageCount = 0;

    $.pager2 = function (options) {
        return $.pager2.impl.init(options);
    };


    $.fn.pager2 = function (options) {
        return $.pager2.impl.init(options);
    };

    $.fn.pager2 = function (options) {
        return $.pager2.impl.init(options);
    };


    $.pager2.defaults = {
        pager2: "#pager2",
        pagerMode: "image",
        totalItemCount: 100,
        recordPerPage: 10,
        currentPage: 1,
        pageIndexChanged: "",
        prev10Text: "<<",
        next10Text: ">>",
        prev10ImgSrc: "images/Prev10.gif",
        next10ImgSrc: "images/next10.gif",
        selectedPageFontColor: "lightblue"
    };

    $.fn.pager2.refresh = function (t, r) {
        return $.pager2.impl.refresh(t, r);
    }

    $.fn.pager2.refresh = function (t, r) {
        return $.pager2.impl.refresh(t, r);
    }

    //초기화
    $.pager2.start = function () {
        alert('');
        s.o.currentPage = 1;
        s.o.pageIndexChanged(s.o.currentPage); //페이지 클릭 이벤트 호출
        s.showPager();
    }


    //페이지 개체 배열
    var arr = new Array();
    var s;

    $.pager2.impl = {
        o: null,
        init: function (options) {
            s = this;
            s.o = $.extend($.pager2.defaults, options);

            s.o.pager2 = $(s.o.pager2);
            s.o.prev10img = $(s.o.prev10img);
            s.o.next10img = $(s.o.next10img);

            s.showPager();

            return s;
        },

        refresh: function (totalItemCount, recordPerPage) {

            s.o.totalItemCount = totalItemCount;
            s.o.recordPerPage = recordPerPage;

            s.showPager();

            return Math.ceil(totalItemCount / recordPerPage);
        },

        showPager: function () {

            var pageCount = Math.ceil(s.o.totalItemCount / s.o.recordPerPage);
            var pageLink;

            s.o.pager2.empty();

            var baseVal = s.o.currentPage - 1;
            iLoop = (Math.floor(baseVal / 10) * 10) + 1;
            iLoopEnd = (Math.floor(baseVal / 10) + 1) * 10;

            if (iLoopEnd > pageCount) iLoopEnd = pageCount;

            for (var page = iLoop; page <= iLoopEnd; page++) {

                //현재 페이지가 아닌 경우 클릭 이벤트를 걸어준다
                if (page != s.o.currentPage) {
                    pageLink = $("<a>" + page + "</a>");
                    pageLink
                        .css("cursor", "pointer")
                        .click(function () {  //페이지 번호를 클릭하는 경우의 처리
                            s.o.currentPage = parseInt($(this).text());
                            s.PageClick();
                        });
                } else {
                    //현재 페이지는 클릭 이벤트를 걸지 않고, 스타일만 다르게 한다
                        pageLink = $("<span>" + page + "</span>");
                    pageLink.css("color", s.o.selectedPageFontColor);
                }

                s.o.pager2.append(pageLink);
                s.o.pager2.append("&nbsp;");
            }

            var prev10UniqueId = s.o.pager2.attr("id") + "_prev10";
            var next10UniqueId = s.o.pager2.attr("id") + "_next10";

            //이전 10개, 다음 10개를 위한 영역
            s.o.pager2.prepend("&nbsp;&nbsp;");
            s.o.pager2.prepend("<span id='" + prev10UniqueId + "'></span>");
            s.o.pager2.append("&nbsp;");
            s.o.pager2.append("<span id='" + next10UniqueId + "'></span>");

            var $prev10Span = s.o.pager2.find("#" + prev10UniqueId);
            var $next10Span = s.o.pager2.find("#" + next10UniqueId);

            var $prev10, $next10;

            //이전 10 페이지 관련 처리
            if (s.o.currentPage > 10) {
                if (s.o.pagerMode == "text") {
                    $prev10 = $("<a>" + s.o.prev10Text + "</a>]");
                    $prev10
                        .css("cursor", "pointer")
                        .click(s.GoPrev10);
                }
                else if (s.o.pagerMode == "image") {
                    $prev10 = $("<img align='absmiddle' />");
                    $prev10
                        .attr("src", s.o.prev10ImgSrc)
                        .css("cursor", "pointer")
                        .click(s.GoPrev10);
                }
                $prev10Span.append($prev10);
            }

            //다음 10 페이지 관련 처리
            if (pageCount > iLoopEnd) {
                if (s.o.pagerMode == "text") {
                    $next10 = $("<a>" + s.o.next10Text + "</a>]");
                    $next10
                        .css("cursor", "pointer")
                        .click(s.GoNext10);
                }
                else if (s.o.pagerMode == "image") {
                    $next10 = $("<img align='absmiddle' />");
                    $next10
                            .attr("src", s.o.next10ImgSrc)
                            .css("cursor", "pointer")
                            .click(s.GoNext10);
                }
                $next10Span.append($next10);
            }
        },

        PageClick: function () {
            //이벤트 버블;                                 
            s.o.pageIndexChanged(s.o.currentPage); //페이지 클릭 이벤트 호출
            s.showPager();
        },

        GoPrev10: function () {
            iLoop -= 10;
            iLoopEnd -= 10;
            iLoopEnd = Math.ceil(iLoopEnd / 10) * 10; //최종구역이 아니라면, 페이지 end 수를 10의 배수로 맞춘다
            if (iLoop < 1) iLoopEnd = 1;
            if (iLoopEnd < 10) iLoopEnd = 10;
            s.o.currentPage = iLoop;

            s.showPager();
            //이벤트 버블;
            s.o.pageIndexChanged(s.o.currentPage); //페이지 클릭 이벤트 호출
        },

        GoNext10: function () {
            iLoop += 10;
            iLoopEnd += 10;
            if (iLoopEnd > pageCount) iLoopEnd = pageCount;
            s.o.currentPage = iLoop;

            s.showPager();
            //이벤트 버블;
            s.o.pageIndexChanged(s.o.currentPage); //페이지 클릭 이벤트 호출
        }
    }

})(jQuery);