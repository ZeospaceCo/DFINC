(function ($) {
    $.fn.resize = function (options) {

        var settings = $.extend({
            scale: 1,
            maxWidth: null,
            maxHeight: null
        }, options);

        var width = this.naturalWidth;
        var height = this.naturalHeight;
        var userAgent = navigator.userAgent.toString().toLowerCase();
        var IE11 = userAgent.indexOf("trident/7") > -1;
        
        return this.each(function () {

            if (this.tagName.toLowerCase() != "img") {
                // Only images can be resized
                return $(this);
            }

            //modify by shimpark (you must apply onload event if you use safari (chrome, firefox, safari)

            if (userAgent.indexOf('safari') != -1 || userAgent.indexOf('firefox') != -1 || IE11) {

                this.onload = function () {
                    width = this.naturalWidth;
                    height = this.naturalHeight;

                    if (!width || !height) {
                        // Ooops you are an IE user, let's fix it.
                        var img = document.createElement('img');
                        img.src = this.src;

                        width = img.width;
                        height = img.height;
                    }

                    if (settings.scale != 1) {
                        width = width * settings.scale;
                        height = height * settings.scale;
                    }

                    var pWidth = 1;
                    if (settings.maxWidth != null) {
                        pWidth = width / settings.maxWidth;
                    }
                    var pHeight = 1;
                    if (settings.maxHeight != null) {
                        pHeight = height / settings.maxHeight;
                    }

                    var reduce = 1;

                    if (pWidth < pHeight) {
                        reduce = pHeight;
                    } else {
                        reduce = pWidth;
                    }

                    if (reduce < 1) {
                        reduce = 1;
                    }

                    var newWidth = width / reduce;
                    var newHeight = height / reduce;

                    return $(this)
				        .attr("width", newWidth)
				        .attr("height", newHeight);
                }
            }
            else {
                if (!width || !height) {
                    // Ooops you are an IE user, let's fix it.
                    var img = document.createElement('img');
                    img.src = this.src;

                    width = img.width;
                    height = img.height;
                }

                if (settings.scale != 1) {
                    width = width * settings.scale;
                    height = height * settings.scale;
                }

                var pWidth = 1;
                if (settings.maxWidth != null) {
                    pWidth = width / settings.maxWidth;
                }
                var pHeight = 1;
                if (settings.maxHeight != null) {
                    pHeight = height / settings.maxHeight;
                }

                var reduce = 1;

                if (pWidth < pHeight) {
                    reduce = pHeight;
                } else {
                    reduce = pWidth;
                }

                if (reduce < 1) {
                    reduce = 1;
                }

                var newWidth = width / reduce;
                var newHeight = height / reduce;

                return $(this)
				    .attr("width", newWidth)
				    .attr("height", newHeight);
            }

        });
    }
})(jQuery);
