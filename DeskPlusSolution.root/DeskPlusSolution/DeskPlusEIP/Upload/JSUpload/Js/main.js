/*
* jQuery File Upload Plugin JS Example 8.3.0
* https://github.com/blueimp/jQuery-File-Upload
*
* Copyright 2010, Sebastian Tschan
* https://blueimp.net
*
* Licensed under the MIT license:
* http://www.opensource.org/licenses/MIT
*/

/*jslint nomen: true, regexp: true */
/*global $, window, blueimp */

$(function () {
    var csrf_token = $('meta[name="csrf-token"]').attr('content');

    function csrfSafeMethod(method) {
        // these HTTP methods do not require CSRF protection
        return (/^(GET|HEAD|OPTIONS)$/.test(method));
    }

    $.ajaxSetup({
        beforeSend: function (xhr, settings) {
            if (!csrfSafeMethod(settings.type) && !this.crossDomain) {
                xhr.setRequestHeader("anti-csrf-token", csrf_token);
            }
        }
    });

    'use strict';

    $('#btnfileupload').click(function () {
        $('#over').attr('style', 'display:none');
    })


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        // Uncomment the following to send cross-domain cookies:
        //xhrFields: {withCredentials: true},
        url: 'Default.aspx',
        formData: { p: uploadPath }
    });

    // Enable iframe cross-domain access via redirect option:
    $('#fileupload').fileupload(
        'option',
        'redirect',
        window.location.href.replace(
            /\/[^\/]*$/,
            '/cors/result.html?%s'
        )
    );

    $('#fileupload').fileupload({
        sequentialUploads: true
    });

    //다중 파일 업로드
    $('#cbtnApply').click(function (e) {
        e.preventDefault();

        if (parent.disablePopup) {
            var obj = eval("parent." + callBackEvent);
            obj();
            parent.disablePopup();
        } else {
            var obj = eval("opener." + callBackEvent);
            obj();
            self.close();
        }

    });

    //취소
    $('#cbtnClose').click(function (e) {
        e.preventDefault();
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    });


    //dp upload
    $('#DPUpload').click(function () {

        var iPageUrl = $.url(PageUrl);
        var iQuery = iPageUrl.attr('query');
        location = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?" + iQuery;

    });

    //web hard
    $('#FolderUpload').click(function () {

        var iPageUrl = $.url(PageUrl);
        var iQuery = iPageUrl.attr('query');

        location = "/DeskPlusEIP/WebHard/Upload/WebHardAttach.aspx?" + iQuery;
    });

    $("#ulWidgets").sortable();


    /*
    if (window.location.hostname === 'blueimp.github.io') {
    // Demo settings:
    $('#fileupload').fileupload('option', {
    url: '/Default.aspx',
    // Enable image resizing, except for Android and Opera,
    // which actually support image resizing, but fail to
    // send Blob objects via XHR requests:
    disableImageResize: /Android(?!.*Chrome)|Opera/
    .test(window.navigator.userAgent),
    maxFileSize: 5000000,
    acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
    });
    // Upload server status check for browsers with CORS support:
    if ($.support.cors) {
    $.ajax({
    url: '/Default.aspx',
    type: 'HEAD'
    }).fail(function () {
    $('<span class="alert alert-error"/>')
    .text('Upload server currently unavailable - ' +
    new Date())
    .appendTo('#fileupload');
    });
    }
    } else {
    alert("bbb");
    // Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
    // Uncomment the following to send cross-domain cookies:
    //xhrFields: {withCredentials: true},
    url: $('#fileupload').fileupload('option', 'url'),
    dataType: 'json',
    context: $('#fileupload')[0]
    }).always(function () {
    $(this).removeClass('fileupload-processing');
    }).done(function (result) {
    $(this).fileupload('option', 'done')
    .call(this, null, { result: result });
    });
    }
    */
});
