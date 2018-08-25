/************************************************************
* Posts assincronos                                         *
* By: Caio Humberto Francisco                               *
* Date: 18/02/2013 14:34                                    *
*************************************************************/
jQuery.extend({
    ajaxPartialView: function (options) {

        var sDefaults = {
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            traditional: true,
            beforeSend: function () {
                $("#pleaseWaitDialog").show();
            },
            complete: function () {
                $("#pleaseWaitDialog").hide();
            }
        }

        var options = jQuery.extend(sDefaults, options);

        $.ajax(options);
    },

    ajaxPartialViewWithoutLoad: function (options) {

        var sDefaults = {
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            traditional: true
        }

        var options = jQuery.extend(sDefaults, options);

        $.ajax(options);
    },

    ajaxJson: function (options) {
        var sDefaults = {
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            cache: false,
            dataType: "json"
        }

        var options = jQuery.extend(sDefaults, options);

        $.ajax(options);
    },

    ajaxJsonLoad: function (options) {
        var sDefaults = {
            beforeSend: function () {
                $("#pleaseWaitDialog").show();
            },
            complete: function () {
                $("#pleaseWaitDialog").hide();
            }
        }

        var options = jQuery.extend(sDefaults, options);
        $.ajaxJson(options);
    },

    ajaxGetApi: function (options) {
        /// <summary>Chamada assincrona para uma API controller
        ///         type: "GET",
        ///         contentType: "application/json; charset=utf-8"
        ///         cache: false,
        ///         dataType: "json" 
        /// </summary>
        /// <param name="options" type="Object">Parametros como url, success, data e outros</param>

        ///O inicio da url deve ser /api, caso isso não aconteca eu adiciono
        if (options.url.startsWith("/api") == false) {
            if (options.url.startsWith("/")) {
                options.url = "/api" + options.url;
            } else {
                options.url = "/api/" + options.url;
            }
        }

        var sDefaults = {
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            cache: false,
            dataType: "json"
        }

        var options = jQuery.extend(sDefaults, options);

        var xhr = $.ajax(options);

        return xhr;
    }
});

if (typeof String.prototype.startsWith != 'function') {
    // see below for better implementation!
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}


$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();

    $.each(a, function () {
        var name = this.name.split(".")[this.name.split(".").length - 1];
        if (o[name] !== undefined) {
            if (!o[name].push) {
                o[name] = [o[name]];
            }

            o[name].push(this.value || '');
        } else {
            o[name] = this.value || '';
        }
    });

    return o;
};