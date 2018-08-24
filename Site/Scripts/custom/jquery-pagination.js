/************************************************************
* Paginação                                                 *
* By: Caio Humberto Francisco                               *
* Date: 18/09/2013 14:34                                    *
*************************************************************/

; (function ($) {
    $.fn.pagination = function (options) {
        var defaultOptions = {
            //onClick: function (e) { },
            tableTaget: "",
            selectQtdEntries: "",
            search: "",
            url: "",
            urlExport: "",
            beforeSubmit: function (e) {
                return true;
            },
            afterSubmit: function (e, data, textStatus, jqXHR) { },
            searchOnEnter: true,
            searchOnBlur: true,
            paramiterSearch: [
                //{
                //paramiterName: "",
                //objectTarget: "",
                //objectExtra: {separator:" ", targets: ["",""]}      Utilizar para o cado de data(campo 1 = objecttarget) e hora (campo 2 = obj extra)          
                //}
            ]
        };

        var bindEvent = function (name) {
            $("html").on(ReturnEvent(name), $(name), function () {
                $(table).data().currentpage = 1;
                $(tempA).data("page", 1);
                $(tempA).click();
            });
        };

        $.fn.getType = function () { return this[0].tagName == "INPUT" ? $(this[0]).attr("type").toLowerCase() : this[0].tagName.toLowerCase(); }

        function ReturnEvent(obj) {
            if ($(obj).getType() == "text") {
                return "blur";
            }
            else if ($(obj).getType() == "select") {
                return "change";
            }
        }

        var that = $(this);
        var settings = $.extend(defaultOptions, options);
        var table = $(settings.tableTaget);

        $(this).data("settings", settings);
        $(this).find("a[data-page='" + $(table).data().currentpage + "']").closest("li:not(.disabled)").addClass("active");

        $(this).find("li.prev > a[data-page='0']").closest("li").addClass("disabled");
        $(this).find("li.next > a[data-page='0']").closest("li").addClass("disabled");
        $(this).find("li.prev > a[data-page='0']").css("cursor", "default");
        $(this).find("li.next > a[data-page='0']").css("cursor", "default");

        $(settings.selectQtdEntries).val($(table).data().qtdentries);

        var tempA = $("<a>");
        $(tempA).attr({ id: "tempA", href: "javascript:void(0);", "data-page": 0 });
        $(that).find(".pagnationFunction").append($(tempA));

        //Evento dos botões
        $(this).on("click", "li:not(.disabled) > a[data-page]", function (e) {
            e.preventDefault();
            var that = $(this);
            var pagination = $(that).closest("ul");
            var table = $($(pagination).data().settings.tableTaget);
            var tableData = $(table).data();

            if (settings.beforeSubmit($(this))) {
                var param = {
                    page: $(this).data().page,
                    qtdEntries: tableData.qtdentries,
                    filter: $(settings.search).val(),
                    sort: tableData.sort,
                    sortdirection: tableData.sortdirection
                };

                ///Add paramiter for send data
                var paramCompl = new Object();
                $.each(settings.paramiterSearch, function (i, e) {
                    var tempValue;
                    if (e.objectTarget != undefined) {
                        tempValue = $(e.objectTarget).val();
                    }

                    if (e.objectExtra != undefined && e.objectExtra.targets.length > 0) {
                        var separator = (e.objectExtra.separator != undefined ? e.objectExtra.separator : " ");

                        $.each(e.objectExtra.targets, function (index, name) {
                            tempValue += separator + $(name).val();

                        });
                    }
                    paramCompl[e.paramiterName] = tempValue;
                });
                param = $.extend(paramCompl, param);

                if ($(that).data().export != undefined) {

                    param = $.extend({
                        exporttype: $(that).data().export
                    }, param);

                    var paramiters = new Array();

                    for (var name in param) {
                        paramiters.push(name + '=' + encodeURIComponent(param[name]));
                    }

                    window.open(settings.urlExport + '?' + paramiters.join("&"), "_blank");

                } else {

                    $(that).closest("li").addClass("disabled");

                    $.ajaxPartialView({
                        url: settings.url,
                        data: JSON.stringify(param),
                        success: function (data, textStatus, jqXHR) {
                            $("tbody", $(table)).html(data);
                            var ViewBagHeader = JSON.parse(jqXHR.getResponseHeader('ViewBagHeader'));
                            $(that).closest("li").removeClass("disabled");
                            $(pagination).trigger("update", [ViewBagHeader]);

                            settings.afterSubmit($(that), data, textStatus, jqXHR);
                        }
                    });
                }
            }
        });

        //Evento do combo de quantidade por pagina
        $("html").on("change", settings.selectQtdEntries, function () {
            $(table).data().qtdentries = $(this).val();
            $(table).data().currentpage = 1;

            $(tempA).data("page", 1);
            $(tempA).click();
        });
        if (defaultOptions.searchOnEnter) {
            //Evento de enter do campo de filtro
            $("html").on("keyup", settings.search, function (e) {
                var code = e.which;

                if (code == 13) {
                    e.preventDefault();
                    $(table).data().search = $(this).val();
                    $(settings.selectQtdEntries).change();
                }
            });
        }
        if (defaultOptions.searchOnBlur) {
            //Evento do blur do campo de filtro
            $("html").on("blur", settings.search, function (e) {
                $(settings.selectQtdEntries).change();
            });
        }
        $(table).on("click", "th:not(.sorting_disabled)[class*='sort']", function (e) {
            $("th:not(.sorting_disabled)[class*='sort']", $(table)).removeClass("sorting_asc").removeClass("sorting_desc").addClass("sorting")
            $(table).data().sort = $(this).data().sort;


            if ($(table).data().sortdirection == "asc") {
                $(table).data().sortdirection = "desc";
                $(this).removeClass("sorting").addClass("sorting_desc");
            } else {
                $(table).data().sortdirection = "asc";
                $(this).removeClass("sorting").addClass("sorting_asc");
            }

            $(settings.selectQtdEntries).change();

        });

        $(this).bind("find", function () {
            $(tempA).data("page", 1);
            $(tempA).click();
        });

        $(this).bind("refresh", function () {
            /// <summary>trigger para forçar a atualização dos dados</summary>
            $(this).find("a[data-page='" + $(table).data().currentpage + "']").trigger("click");
        });

        $(this).bind("export", function (method, type) {
            /// <summary>trigger para forçar a atualização dos dados</summary>
            $("#tempA").data("page", 0);
            $(this).find("a[data-page='" + $(table).data().currentpage + "']").data("export", type);
            $(this).find("a[data-page='" + $(table).data().currentpage + "']").trigger("click");
            $(this).find("a[data-page='" + $(table).data().currentpage + "']").removeData("export");
        });

        $(this).bind("update", function (event, ViewBagHeader, refresh) {
            /// <summary>Evento para atualizar o UI da paginação</summary>
            /// <param name="event" type="Object"></param>
            /// <param name="ViewBagHeader" type="Object">Coleção de dados a serem atualizados.</param>
            /// <param name="refresh" type="Object">true|false para executar o refresh.</param>

            $($(this).data().settings.tableTaget).data().currentpage = ViewBagHeader.currentPage;
            $($(this).data().settings.tableTaget).data().qtdEntries = ViewBagHeader.qtdEntries;
            $($(this).data().settings.tableTaget).data().amount = ViewBagHeader.amount;
            $($(this).data().settings.tableTaget).data().qtdPage = ViewBagHeader.qtdPage;
            $($(this).data().settings.tableTaget).data().qtdActionNumber = ViewBagHeader.qtdActionNumber;
            $($(this).data().settings.tableTaget).data().search = ViewBagHeader.search;

            var prev = $(this).find("li.prev").clone();
            var next = $(this).find("li.next").clone();

            $(this).find("li:not(.pagnationFunction)").remove();

            var startActionNumber = 1;
            var endActionNumber = ViewBagHeader.qtdPage;

            if (ViewBagHeader.qtdActionNumber < ViewBagHeader.qtdPage) {

                var startActionNumber = (ViewBagHeader.currentPage - (ViewBagHeader.qtdActionNumber / 2) <= 0 ? 1 : ViewBagHeader.currentPage - (ViewBagHeader.qtdActionNumber / 2));
                var endActionNumber = (startActionNumber + ViewBagHeader.qtdActionNumber > ViewBagHeader.qtdPage ? ViewBagHeader.qtdPage : startActionNumber + ViewBagHeader.qtdActionNumber);

                ///Correção do número de start quando estiver chegando nas ultimas paginas
                if (endActionNumber - startActionNumber != ViewBagHeader.qtdActionNumber) {
                    startActionNumber = startActionNumber - (ViewBagHeader.qtdActionNumber - (endActionNumber - startActionNumber));
                }
            }

            $(this).append($(prev));

            //if (startActionNumber > 1)
            //{
            //    var li = $("<li>");
            //    var a = $("<a>").addClass("disabled");
            //    $(li).append($(a).attr({ href: "javascript:void(0)", style: "cursor: pointer" }).html("...")).appendTo($(this));
            //}

            for (var i = startActionNumber; i <= endActionNumber; i++) {
                var li = $("<li>");
                var a = $("<a>");
                $(li).append($(a).attr({ href: "javascript:void(0)", style: "cursor: pointer", "data-page": i }).html(i)).appendTo($(this));
            }


            //if (endActionNumber < ViewBagHeader.qtdPage) {
            //    var li = $("<li>");
            //    var a = $("<a>").addClass("disabled");
            //    $(li).append($(a).attr({ href: "javascript:void(0)", style: "cursor: pointer" }).html("...")).appendTo($(this));
            //}

            $(this).append($(next));
            $(this).find("li").removeClass("active");

            $(prev).addClass("disabled");
            $(prev).find("a").data().page = 0;

            $(prev).find("a").css("cursor", "default");

            if (ViewBagHeader.currentPage > 1) {
                $(prev).removeClass("disabled");
                $(prev).find("a").data().page = ViewBagHeader.currentPage - 1;
                $(prev).find("a").css("cursor", "pointer");
            }

            $(next).addClass("disabled");
            $(next).find("a").data().page = 0;

            $(next).find("a").css("cursor", "default");

            if (ViewBagHeader.currentPage < ViewBagHeader.qtdPage) {
                $(next).removeClass("disabled");
                $(next).find("a").data().page = ViewBagHeader.currentPage + 1;
                $(next).find("a").css("cursor", "pointer");
            }

            $(this).find("a[data-page='" + ViewBagHeader.currentPage + "']").closest("li:not(.prev):not(.next):not(.disabled)").addClass("active");

            $(this).closest(".containerPagination").find(".infoPagination").html(ViewBagHeader.info);

            if (refresh) {
                $(this).trigger("refresh");
            }
        });


        if (defaultOptions.searchOnBlur) {
            ///Bind action change for target filter
            $.each(settings.paramiterSearch, function (i, e) {
                if (e.objectTarget != undefined) {
                    bindEvent(e.objectTarget);
                }

                if (e.objectExtra != undefined && e.objectExtra.targets.lenght > 0) {
                    var separator = (e.objectExtra.separator != undefined ? e.objectExtra.separator : " ");

                    $.each(e.objectExtra.targets, function (index, name) {
                        bindEvent(name);
                    });
                }
            });
        }

        return this.each(function () {
            return $(this);
        });
    }
})(jQuery);