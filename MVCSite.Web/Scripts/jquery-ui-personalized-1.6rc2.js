﻿; (function($) {
    var _remove = $.fn.remove;
    $.fn.remove = function() {
        $("*", this).add(this).triggerHandler("remove");
        return _remove.apply(this, arguments);
    };
    function isVisible(element) {
        function checkStyles(element) {
            var style = element.style;
            return (style.display != 'none' && style.visibility != 'hidden');
        }
        var visible = checkStyles(element); (visible && $.each($.dir(element, 'parentNode'),
        function() {
            return (visible = checkStyles(this));
        }));
        return visible;
    }
    $.extend($.expr[':'], {
        data: function(a, i, m) {
            return $.data(a, m[3]);
        },
        tabbable: function(a, i, m) {
            var nodeName = a.nodeName.toLowerCase();
            return (a.tabIndex >= 0 && (('a' == nodeName && a.href) || (/input|select|textarea|button/.test(nodeName) && 'hidden' != a.type && !a.disabled)) && isVisible(a));
        }
    });
    $.keyCode = {
        BACKSPACE: 8,
        CAPS_LOCK: 20,
        COMMA: 188,
        CONTROL: 17,
        DELETE: 46,
        DOWN: 40,
        END: 35,
        ENTER: 13,
        ESCAPE: 27,
        HOME: 36,
        INSERT: 45,
        LEFT: 37,
        NUMPAD_ADD: 107,
        NUMPAD_DECIMAL: 110,
        NUMPAD_DIVIDE: 111,
        NUMPAD_ENTER: 108,
        NUMPAD_MULTIPLY: 106,
        NUMPAD_SUBTRACT: 109,
        PAGE_DOWN: 34,
        PAGE_UP: 33,
        PERIOD: 190,
        RIGHT: 39,
        SHIFT: 16,
        SPACE: 32,
        TAB: 9,
        UP: 38
    };
    function getter(namespace, plugin, method, args) {
        function getMethods(type) {
            var methods = $[namespace][plugin][type] || [];
            return (typeof methods == 'string' ? methods.split(/,?\s+/) : methods);
        }
        var methods = getMethods('getter');
        if (args.length == 1 && typeof args[0] == 'string') {
            methods = methods.concat(getMethods('getterSetter'));
        }
        return ($.inArray(method, methods) != -1);
    }
    $.widget = function(name, prototype) {
        var namespace = name.split(".")[0];
        name = name.split(".")[1];
        $.fn[name] = function(options) {
            var isMethodCall = (typeof options == 'string'),
            args = Array.prototype.slice.call(arguments, 1);
            if (isMethodCall && options.substring(0, 1) == '_') {
                return this;
            }
            if (isMethodCall && getter(namespace, name, options, args)) {
                var instance = $.data(this[0], name);
                return (instance ? instance[options].apply(instance, args) : undefined);
            }
            return this.each(function() {
                var instance = $.data(this, name); (!instance && !isMethodCall && $.data(this, name, new $[namespace][name](this, options))); (instance && isMethodCall && $.isFunction(instance[options]) && instance[options].apply(instance, args));
            });
        };
        $[namespace][name] = function(element, options) {
            var self = this;
            this.widgetName = name;
            this.widgetEventPrefix = $[namespace][name].eventPrefix || name;
            this.widgetBaseClass = namespace + '-' + name;
            this.options = $.extend({},
            $.widget.defaults, $[namespace][name].defaults, $.metadata && $.metadata.get(element)[name], options);
            this.element = $(element).bind('setData.' + name,
            function(e, key, value) {
                return self._setData(key, value);
            }).bind('getData.' + name,
            function(e, key) {
                return self._getData(key);
            }).bind('remove',
            function() {
                return self.destroy();
            });
            this._init();
        };
        $[namespace][name].prototype = $.extend({},
        $.widget.prototype, prototype);
        $[namespace][name].getterSetter = 'option';
    };
    $.widget.prototype = {
        _init: function() {},
        destroy: function() {
            this.element.removeData(this.widgetName);
        },
        option: function(key, value) {
            var options = key,
            self = this;
            if (typeof key == "string") {
                if (value === undefined) {
                    return this._getData(key);
                }
                options = {};
                options[key] = value;
            }
            $.each(options,
            function(key, value) {
                self._setData(key, value);
            });
        },
        _getData: function(key) {
            return this.options[key];
        },
        _setData: function(key, value) {
            this.options[key] = value;
            if (key == 'disabled') {
                this.element[value ? 'addClass': 'removeClass'](this.widgetBaseClass + '-disabled');
            }
        },
        enable: function() {
            this._setData('disabled', false);
        },
        disable: function() {
            this._setData('disabled', true);
        },
        _trigger: function(type, e, data) {
            var eventName = (type == this.widgetEventPrefix ? type: this.widgetEventPrefix + type);
            e = e || $.event.fix({
                type: eventName,
                target: this.element[0]
            });
            return this.element.triggerHandler(eventName, [e, data], this.options[type]);
        }
    };
    $.widget.defaults = {
        disabled: false
    };
    $.ui = {
        plugin: {
            add: function(module, option, set) {
                var proto = $.ui[module].prototype;
                for (var i in set) {
                    proto.plugins[i] = proto.plugins[i] || [];
                    proto.plugins[i].push([option, set[i]]);
                }
            },
            call: function(instance, name, args) {
                var set = instance.plugins[name];
                if (!set) {
                    return;
                }
                for (var i = 0; i < set.length; i++) {
                    if (instance.options[set[i][0]]) {
                        set[i][1].apply(instance.element, args);
                    }
                }
            }
        },
        cssCache: {},
        css: function(name) {
            if ($.ui.cssCache[name]) {
                return $.ui.cssCache[name];
            }
            var tmp = $('<div class="ui-gen">').addClass(name).css({
                position: 'absolute',
                top: '-5000px',
                left: '-5000px',
                display: 'block'
            }).appendTo('body');
            $.ui.cssCache[name] = !!((!(/auto|default/).test(tmp.css('cursor')) || (/^[1-9]/).test(tmp.css('height')) || (/^[1-9]/).test(tmp.css('width')) || !(/none/).test(tmp.css('backgroundImage')) || !(/transparent|rgba\(0, 0, 0, 0\)/).test(tmp.css('backgroundColor'))));
            try {
                $('body').get(0).removeChild(tmp.get(0));
            } catch(e) {}
            return $.ui.cssCache[name];
        },
        disableSelection: function(el) {
            return $(el).attr('unselectable', 'on').css('MozUserSelect', 'none').bind('selectstart.ui',
            function() {
                return false;
            });
        },
        enableSelection: function(el) {
            return $(el).attr('unselectable', 'off').css('MozUserSelect', '').unbind('selectstart.ui');
        },
        hasScroll: function(e, a) {
            if ($(e).css('overflow') == 'hidden') {
                return false;
            }
            var scroll = (a && a == 'left') ? 'scrollLeft': 'scrollTop',
            has = false;
            if (e[scroll] > 0) {
                return true;
            }
            e[scroll] = 1;
            has = (e[scroll] > 0);
            e[scroll] = 0;
            return has;
        }
    };
    $.ui.mouse = {
        _mouseInit: function() {
            var self = this;
            this.element.bind('mousedown.' + this.widgetName,
            function(e) {
                return self._mouseDown(e);
            });
            if ($.browser.msie) {
                this._mouseUnselectable = this.element.attr('unselectable');
                this.element.attr('unselectable', 'on');
            }
            this.started = false;
        },
        _mouseDestroy: function() {
            this.element.unbind('.' + this.widgetName); ($.browser.msie && this.element.attr('unselectable', this._mouseUnselectable));
        },
        _mouseDown: function(e) { (this._mouseStarted && this._mouseUp(e));
            this._mouseDownEvent = e;
            var self = this,
            btnIsLeft = (e.which == 1),
            elIsCancel = (typeof this.options.cancel == "string" ? $(e.target).parents().add(e.target).filter(this.options.cancel).length: false);
            if (!btnIsLeft || elIsCancel || !this._mouseCapture(e)) {
                return true;
            }
            this.mouseDelayMet = !this.options.delay;
            if (!this.mouseDelayMet) {
                this._mouseDelayTimer = setTimeout(function() {
                    self.mouseDelayMet = true;
                },
                this.options.delay);
            }
            if (this._mouseDistanceMet(e) && this._mouseDelayMet(e)) {
                this._mouseStarted = (this._mouseStart(e) !== false);
                if (!this._mouseStarted) {
                    e.preventDefault();
                    return true;
                }
            }
            this._mouseMoveDelegate = function(e) {
                return self._mouseMove(e);
            };
            this._mouseUpDelegate = function(e) {
                return self._mouseUp(e);
            };
            $(document).bind('mousemove.' + this.widgetName, this._mouseMoveDelegate).bind('mouseup.' + this.widgetName, this._mouseUpDelegate);
            return false;
        },
        _mouseMove: function(e) {
            if ($.browser.msie && !e.button) {
                return this._mouseUp(e);
            }
            if (this._mouseStarted) {
                this._mouseDrag(e);
                return false;
            }
            if (this._mouseDistanceMet(e) && this._mouseDelayMet(e)) {
                this._mouseStarted = (this._mouseStart(this._mouseDownEvent, e) !== false); (this._mouseStarted ? this._mouseDrag(e) : this._mouseUp(e));
            }
            return ! this._mouseStarted;
        },
        _mouseUp: function(e) {
            $(document).unbind('mousemove.' + this.widgetName, this._mouseMoveDelegate).unbind('mouseup.' + this.widgetName, this._mouseUpDelegate);
            if (this._mouseStarted) {
                this._mouseStarted = false;
                this._mouseStop(e);
            }
            return false;
        },
        _mouseDistanceMet: function(e) {
            return (Math.max(Math.abs(this._mouseDownEvent.pageX - e.pageX), Math.abs(this._mouseDownEvent.pageY - e.pageY)) >= this.options.distance);
        },
        _mouseDelayMet: function(e) {
            return this.mouseDelayMet;
        },
        _mouseStart: function(e) {},
        _mouseDrag: function(e) {},
        _mouseStop: function(e) {},
        _mouseCapture: function(e) {
            return true;
        }
    };
    $.ui.mouse.defaults = {
        cancel: null,
        distance: 1,
        delay: 0
    };
})(jQuery); (function($) {
    $.widget("ui.draggable", $.extend({},
    $.ui.mouse, {
        getHandle: function(e) {
            var handle = !this.options.handle || !$(this.options.handle, this.element).length ? true: false;
            $(this.options.handle, this.element).find("*").andSelf().each(function() {
                if (this == e.target) handle = true;
            });
            return handle;
        },
        createHelper: function() {
            var o = this.options;
            var helper = $.isFunction(o.helper) ? $(o.helper.apply(this.element[0], [e])) : (o.helper == 'clone' ? this.element.clone() : this.element);
            if (!helper.parents('body').length) helper.appendTo((o.appendTo == 'parent' ? this.element[0].parentNode: o.appendTo));
            if (helper[0] != this.element[0] && !(/(fixed|absolute)/).test(helper.css("position"))) helper.css("position", "absolute");
            return helper;
        },
        _init: function() {
            if (this.options.helper == 'original' && !(/^(?:r|a|f)/).test(this.element.css("position"))) this.element[0].style.position = 'relative'; (this.options.cssNamespace && this.element.addClass(this.options.cssNamespace + "-draggable")); (this.options.disabled && this.element.addClass('ui-draggable-disabled'));
            this._mouseInit();
        },
        _mouseCapture: function(e) {
            var o = this.options;
            if (this.helper || o.disabled || $(e.target).is('.ui-resizable-handle')) return false;
            this.handle = this.getHandle(e);
            if (!this.handle) return false;
            return true;
        },
        _mouseStart: function(e) {
            var o = this.options;
            this.helper = this.createHelper();
            if ($.ui.ddmanager) $.ui.ddmanager.current = this;
            this.margins = {
                left: (parseInt(this.element.css("marginLeft"), 10) || 0),
                top: (parseInt(this.element.css("marginTop"), 10) || 0)
            };
            this.cssPosition = this.helper.css("position");
            this.offset = this.element.offset();
            this.offset = {
                top: this.offset.top - this.margins.top,
                left: this.offset.left - this.margins.left
            };
            this.offset.click = {
                left: e.pageX - this.offset.left,
                top: e.pageY - this.offset.top
            };
            this.cacheScrollParents();
            this.offsetParent = this.helper.offsetParent();
            var po = this.offsetParent.offset();
            if (this.offsetParent[0] == document.body && $.browser.mozilla) po = {
                top: 0,
                left: 0
            };
            this.offset.parent = {
                top: po.top + (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0),
                left: po.left + (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0)
            };
            if (this.cssPosition == "relative") {
                var p = this.element.position();
                this.offset.relative = {
                    top: p.top - (parseInt(this.helper.css("top"), 10) || 0) + this.scrollTopParent.scrollTop(),
                    left: p.left - (parseInt(this.helper.css("left"), 10) || 0) + this.scrollLeftParent.scrollLeft()
                };
            } else {
                this.offset.relative = {
                    top: 0,
                    left: 0
                };
            }
            this.originalPosition = this._generatePosition(e);
            this.cacheHelperProportions();
            if (o.cursorAt) this.adjustOffsetFromHelper(o.cursorAt);
            $.extend(this, {
                PAGEY_INCLUDES_SCROLL: (this.cssPosition == "absolute" && (!this.scrollTopParent[0].tagName || (/(html|body)/i).test(this.scrollTopParent[0].tagName))),
                PAGEX_INCLUDES_SCROLL: (this.cssPosition == "absolute" && (!this.scrollLeftParent[0].tagName || (/(html|body)/i).test(this.scrollLeftParent[0].tagName))),
                OFFSET_PARENT_NOT_SCROLL_PARENT_Y: this.scrollTopParent[0] != this.offsetParent[0] && !(this.scrollTopParent[0] == document && (/(body|html)/i).test(this.offsetParent[0].tagName)),
                OFFSET_PARENT_NOT_SCROLL_PARENT_X: this.scrollLeftParent[0] != this.offsetParent[0] && !(this.scrollLeftParent[0] == document && (/(body|html)/i).test(this.offsetParent[0].tagName))
            });
            if (o.containment) this.setContainment();
            this._propagate("start", e);
            this.cacheHelperProportions();
            if ($.ui.ddmanager && !o.dropBehaviour) $.ui.ddmanager.prepareOffsets(this, e);
            this.helper.addClass("ui-draggable-dragging");
            this._mouseDrag(e);
            return true;
        },
        cacheScrollParents: function() {
            this.scrollTopParent = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-y'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (this.helper);
            this.scrollLeftParent = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-x'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (this.helper);
        },
        adjustOffsetFromHelper: function(obj) {
            if (obj.left != undefined) this.offset.click.left = obj.left + this.margins.left;
            if (obj.right != undefined) this.offset.click.left = this.helperProportions.width - obj.right + this.margins.left;
            if (obj.top != undefined) this.offset.click.top = obj.top + this.margins.top;
            if (obj.bottom != undefined) this.offset.click.top = this.helperProportions.height - obj.bottom + this.margins.top;
        },
        cacheHelperProportions: function() {
            this.helperProportions = {
                width: this.helper.outerWidth(),
                height: this.helper.outerHeight()
            };
        },
        setContainment: function() {
            var o = this.options;
            if (o.containment == 'parent') o.containment = this.helper[0].parentNode;
            if (o.containment == 'document' || o.containment == 'window') this.containment = [0 - this.offset.relative.left - this.offset.parent.left, 0 - this.offset.relative.top - this.offset.parent.top, $(o.containment == 'document' ? document: window).width() - this.offset.relative.left - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), ($(o.containment == 'document' ? document: window).height() || document.body.parentNode.scrollHeight) - this.offset.relative.top - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)];
            if (! (/^(document|window|parent)$/).test(o.containment)) {
                var ce = $(o.containment)[0];
                var co = $(o.containment).offset();
                var over = ($(ce).css("overflow") != 'hidden');
                this.containment = [co.left + (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.relative.left - this.offset.parent.left, co.top + (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.relative.top - this.offset.parent.top, co.left + (over ? Math.max(ce.scrollWidth, ce.offsetWidth) : ce.offsetWidth) - (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.relative.left - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), co.top + (over ? Math.max(ce.scrollHeight, ce.offsetHeight) : ce.offsetHeight) - (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.relative.top - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)];
            }
        },
        _convertPositionTo: function(d, pos) {
            if (!pos) pos = this.position;
            var mod = d == "absolute" ? 1 : -1;
            return {
                top: (pos.top + this.offset.relative.top * mod + this.offset.parent.top * mod - (this.cssPosition == "fixed" || this.PAGEY_INCLUDES_SCROLL || this.OFFSET_PARENT_NOT_SCROLL_PARENT_Y ? 0 : this.scrollTopParent.scrollTop()) * mod + (this.cssPosition == "fixed" ? $(document).scrollTop() : 0) * mod + this.margins.top * mod),
                left: (pos.left + this.offset.relative.left * mod + this.offset.parent.left * mod - (this.cssPosition == "fixed" || this.PAGEX_INCLUDES_SCROLL || this.OFFSET_PARENT_NOT_SCROLL_PARENT_X ? 0 : this.scrollLeftParent.scrollLeft()) * mod + (this.cssPosition == "fixed" ? $(document).scrollLeft() : 0) * mod + this.margins.left * mod)
            };
        },
        _generatePosition: function(e) {
            var o = this.options;
            var position = {
                top: (e.pageY - this.offset.click.top - this.offset.relative.top - this.offset.parent.top + (this.cssPosition == "fixed" || this.PAGEY_INCLUDES_SCROLL || this.OFFSET_PARENT_NOT_SCROLL_PARENT_Y ? 0 : this.scrollTopParent.scrollTop()) - (this.cssPosition == "fixed" ? $(document).scrollTop() : 0)),
                left: (e.pageX - this.offset.click.left - this.offset.relative.left - this.offset.parent.left + (this.cssPosition == "fixed" || this.PAGEX_INCLUDES_SCROLL || this.OFFSET_PARENT_NOT_SCROLL_PARENT_X ? 0 : this.scrollLeftParent.scrollLeft()) - (this.cssPosition == "fixed" ? $(document).scrollLeft() : 0))
            };
            if (!this.originalPosition) return position;
            if (this.containment) {
                if (position.left < this.containment[0]) position.left = this.containment[0];
                if (position.top < this.containment[1]) position.top = this.containment[1];
                if (position.left > this.containment[2]) position.left = this.containment[2];
                if (position.top > this.containment[3]) position.top = this.containment[3];
            }
            if (o.grid) {
                var top = this.originalPosition.top + Math.round((position.top - this.originalPosition.top) / o.grid[1]) * o.grid[1];
                position.top = this.containment ? (!(top < this.containment[1] || top > this.containment[3]) ? top: (!(top < this.containment[1]) ? top - o.grid[1] : top + o.grid[1])) : top;
                var left = this.originalPosition.left + Math.round((position.left - this.originalPosition.left) / o.grid[0]) * o.grid[0];
                position.left = this.containment ? (!(left < this.containment[0] || left > this.containment[2]) ? left: (!(left < this.containment[0]) ? left - o.grid[0] : left + o.grid[0])) : left;
            }
            return position;
        },
        _mouseDrag: function(e) {
            this.position = this._generatePosition(e);
            this.positionAbs = this._convertPositionTo("absolute");
            this.position = this._propagate("drag", e) || this.position;
            if (!this.options.axis || this.options.axis != "y") this.helper[0].style.left = this.position.left + 'px';
            if (!this.options.axis || this.options.axis != "x") this.helper[0].style.top = this.position.top + 'px';
            if ($.ui.ddmanager) $.ui.ddmanager.drag(this, e);
            return false;
        },
        _mouseStop: function(e) {
            var dropped = false;
            if ($.ui.ddmanager && !this.options.dropBehaviour) var dropped = $.ui.ddmanager.drop(this, e);
            if ((this.options.revert == "invalid" && !dropped) || (this.options.revert == "valid" && dropped) || this.options.revert === true || ($.isFunction(this.options.revert) && this.options.revert.call(this.element, dropped))) {
                var self = this;
                $(this.helper).animate(this.originalPosition, parseInt(this.options.revertDuration, 10) || 500,
                function() {
                    self._propagate("stop", e);
                    self._clear();
                });
            } else {
                this._propagate("stop", e);
                this._clear();
            }
            return false;
        },
        _clear: function() {
            this.helper.removeClass("ui-draggable-dragging");
            if (this.options.helper != 'original' && !this.cancelHelperRemoval) this.helper.remove();
            this.helper = null;
            this.cancelHelperRemoval = false;
        },
        plugins: {},
        uiHash: function(e) {
            return {
                helper: this.helper,
                position: this.position,
                absolutePosition: this.positionAbs,
                options: this.options
            };
        },
        _propagate: function(n, e) {
            $.ui.plugin.call(this, n, [e, this.uiHash()]);
            if (n == "drag") this.positionAbs = this._convertPositionTo("absolute");
            return this.element.triggerHandler(n == "drag" ? n: "drag" + n, [e, this.uiHash()], this.options[n]);
        },
        destroy: function() {
            if (!this.element.data('draggable')) return;
            this.element.removeData("draggable").unbind(".draggable").removeClass('ui-draggable ui-draggable-dragging ui-draggable-disabled');
            this._mouseDestroy();
        }
    }));
    $.extend($.ui.draggable, {
        defaults: {
            appendTo: "parent",
            axis: false,
            cancel: ":input",
            delay: 0,
            distance: 1,
            helper: "original",
            scope: "default",
            cssNamespace: "ui"
        }
    });
    $.ui.plugin.add("draggable", "cursor", {
        start: function(e, ui) {
            var t = $('body');
            if (t.css("cursor")) ui.options._cursor = t.css("cursor");
            t.css("cursor", ui.options.cursor);
        },
        stop: function(e, ui) {
            if (ui.options._cursor) $('body').css("cursor", ui.options._cursor);
        }
    });
    $.ui.plugin.add("draggable", "zIndex", {
        start: function(e, ui) {
            var t = $(ui.helper);
            if (t.css("zIndex")) ui.options._zIndex = t.css("zIndex");
            t.css('zIndex', ui.options.zIndex);
        },
        stop: function(e, ui) {
            if (ui.options._zIndex) $(ui.helper).css('zIndex', ui.options._zIndex);
        }
    });
    $.ui.plugin.add("draggable", "opacity", {
        start: function(e, ui) {
            var t = $(ui.helper);
            if (t.css("opacity")) ui.options._opacity = t.css("opacity");
            t.css('opacity', ui.options.opacity);
        },
        stop: function(e, ui) {
            if (ui.options._opacity) $(ui.helper).css('opacity', ui.options._opacity);
        }
    });
    $.ui.plugin.add("draggable", "iframeFix", {
        start: function(e, ui) {
            $(ui.options.iframeFix === true ? "iframe": ui.options.iframeFix).each(function() {
                $('<div class="ui-draggable-iframeFix" style="background: #fff;"></div>').css({
                    width: this.offsetWidth + "px",
                    height: this.offsetHeight + "px",
                    position: "absolute",
                    opacity: "0.001",
                    zIndex: 1000
                }).css($(this).offset()).appendTo("body");
            });
        },
        stop: function(e, ui) {
            $("div.ui-draggable-iframeFix").each(function() {
                this.parentNode.removeChild(this);
            });
        }
    });
    $.ui.plugin.add("draggable", "scroll", {
        start: function(e, ui) {
            var o = ui.options;
            var i = $(this).data("draggable");
            o.scrollSensitivity = o.scrollSensitivity || 20;
            o.scrollSpeed = o.scrollSpeed || 20;
            i.overflowY = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-y'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (this);
            i.overflowX = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-x'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (this);
            if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') i.overflowYOffset = i.overflowY.offset();
            if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') i.overflowXOffset = i.overflowX.offset();
        },
        drag: function(e, ui) {
            var o = ui.options,
            scrolled = false;
            var i = $(this).data("draggable");
            if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') {
                if ((i.overflowYOffset.top + i.overflowY[0].offsetHeight) - e.pageY < o.scrollSensitivity) i.overflowY[0].scrollTop = scrolled = i.overflowY[0].scrollTop + o.scrollSpeed;
                if (e.pageY - i.overflowYOffset.top < o.scrollSensitivity) i.overflowY[0].scrollTop = scrolled = i.overflowY[0].scrollTop - o.scrollSpeed;
            } else {
                if (e.pageY - $(document).scrollTop() < o.scrollSensitivity) scrolled = $(document).scrollTop($(document).scrollTop() - o.scrollSpeed);
                if ($(window).height() - (e.pageY - $(document).scrollTop()) < o.scrollSensitivity) scrolled = $(document).scrollTop($(document).scrollTop() + o.scrollSpeed);
            }
            if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') {
                if ((i.overflowXOffset.left + i.overflowX[0].offsetWidth) - e.pageX < o.scrollSensitivity) i.overflowX[0].scrollLeft = scrolled = i.overflowX[0].scrollLeft + o.scrollSpeed;
                if (e.pageX - i.overflowXOffset.left < o.scrollSensitivity) i.overflowX[0].scrollLeft = scrolled = i.overflowX[0].scrollLeft - o.scrollSpeed;
            } else {
                if (e.pageX - $(document).scrollLeft() < o.scrollSensitivity) scrolled = $(document).scrollLeft($(document).scrollLeft() - o.scrollSpeed);
                if ($(window).width() - (e.pageX - $(document).scrollLeft()) < o.scrollSensitivity) scrolled = $(document).scrollLeft($(document).scrollLeft() + o.scrollSpeed);
            }
            if (scrolled !== false) $.ui.ddmanager.prepareOffsets(i, e);
        }
    });
    $.ui.plugin.add("draggable", "snap", {
        start: function(e, ui) {
            var inst = $(this).data("draggable");
            inst.snapElements = [];
            $(ui.options.snap.constructor != String ? (ui.options.snap.items || ':data(draggable)') : ui.options.snap).each(function() {
                var $t = $(this);
                var $o = $t.offset();
                if (this != inst.element[0]) inst.snapElements.push({
                    item: this,
                    width: $t.outerWidth(),
                    height: $t.outerHeight(),
                    top: $o.top,
                    left: $o.left
                });
            });
        },
        drag: function(e, ui) {
            var inst = $(this).data("draggable");
            var d = ui.options.snapTolerance || 20;
            var x1 = ui.absolutePosition.left,
            x2 = x1 + inst.helperProportions.width,
            y1 = ui.absolutePosition.top,
            y2 = y1 + inst.helperProportions.height;
            for (var i = inst.snapElements.length - 1; i >= 0; i--) {
                var l = inst.snapElements[i].left,
                r = l + inst.snapElements[i].width,
                t = inst.snapElements[i].top,
                b = t + inst.snapElements[i].height;
                if (! ((l - d < x1 && x1 < r + d && t - d < y1 && y1 < b + d) || (l - d < x1 && x1 < r + d && t - d < y2 && y2 < b + d) || (l - d < x2 && x2 < r + d && t - d < y1 && y1 < b + d) || (l - d < x2 && x2 < r + d && t - d < y2 && y2 < b + d))) {
                    if (inst.snapElements[i].snapping)(inst.options.snap.release && inst.options.snap.release.call(inst.element, null, $.extend(inst.uiHash(), {
                        snapItem: inst.snapElements[i].item
                    })));
                    inst.snapElements[i].snapping = false;
                    continue;
                }
                if (ui.options.snapMode != 'inner') {
                    var ts = Math.abs(t - y2) <= d;
                    var bs = Math.abs(b - y1) <= d;
                    var ls = Math.abs(l - x2) <= d;
                    var rs = Math.abs(r - x1) <= d;
                    if (ts) ui.position.top = inst._convertPositionTo("relative", {
                        top: t - inst.helperProportions.height,
                        left: 0
                    }).top;
                    if (bs) ui.position.top = inst._convertPositionTo("relative", {
                        top: b,
                        left: 0
                    }).top;
                    if (ls) ui.position.left = inst._convertPositionTo("relative", {
                        top: 0,
                        left: l - inst.helperProportions.width
                    }).left;
                    if (rs) ui.position.left = inst._convertPositionTo("relative", {
                        top: 0,
                        left: r
                    }).left;
                }
                var first = (ts || bs || ls || rs);
                if (ui.options.snapMode != 'outer') {
                    var ts = Math.abs(t - y1) <= d;
                    var bs = Math.abs(b - y2) <= d;
                    var ls = Math.abs(l - x1) <= d;
                    var rs = Math.abs(r - x2) <= d;
                    if (ts) ui.position.top = inst._convertPositionTo("relative", {
                        top: t,
                        left: 0
                    }).top;
                    if (bs) ui.position.top = inst._convertPositionTo("relative", {
                        top: b - inst.helperProportions.height,
                        left: 0
                    }).top;
                    if (ls) ui.position.left = inst._convertPositionTo("relative", {
                        top: 0,
                        left: l
                    }).left;
                    if (rs) ui.position.left = inst._convertPositionTo("relative", {
                        top: 0,
                        left: r - inst.helperProportions.width
                    }).left;
                }
                if (!inst.snapElements[i].snapping && (ts || bs || ls || rs || first))(inst.options.snap.snap && inst.options.snap.snap.call(inst.element, null, $.extend(inst.uiHash(), {
                    snapItem: inst.snapElements[i].item
                })));
                inst.snapElements[i].snapping = (ts || bs || ls || rs || first);
            };
        }
    });
    $.ui.plugin.add("draggable", "connectToSortable", {
        start: function(e, ui) {
            var inst = $(this).data("draggable");
            inst.sortables = [];
            $(ui.options.connectToSortable).each(function() {
                if ($.data(this, 'sortable')) {
                    var sortable = $.data(this, 'sortable');
                    inst.sortables.push({
                        instance: sortable,
                        shouldRevert: sortable.options.revert
                    });
                    sortable._refreshItems();
                    sortable._propagate("activate", e, inst);
                }
            });
        },
        stop: function(e, ui) {
            var inst = $(this).data("draggable");
            $.each(inst.sortables,
            function() {
                if (this.instance.isOver) {
                    this.instance.isOver = 0;
                    inst.cancelHelperRemoval = true;
                    this.instance.cancelHelperRemoval = false;
                    if (this.shouldRevert) this.instance.options.revert = true;
                    this.instance._mouseStop(e);
                    this.instance.element.triggerHandler("sortreceive", [e, $.extend(this.instance.ui(), {
                        sender: inst.element
                    })], this.instance.options["receive"]);
                    this.instance.options.helper = this.instance.options._helper;
                } else {
                    this.instance._propagate("deactivate", e, inst);
                }
            });
        },
        drag: function(e, ui) {
            var inst = $(this).data("draggable"),
            self = this;
            var checkPos = function(o) {
                var l = o.left,
                r = l + o.width,
                t = o.top,
                b = t + o.height;
                return (l < (this.positionAbs.left + this.offset.click.left) && (this.positionAbs.left + this.offset.click.left) < r && t < (this.positionAbs.top + this.offset.click.top) && (this.positionAbs.top + this.offset.click.top) < b);
            };
            $.each(inst.sortables,
            function(i) {
                if (checkPos.call(inst, this.instance.containerCache)) {
                    if (!this.instance.isOver) {
                        this.instance.isOver = 1;
                        this.instance.currentItem = $(self).clone().appendTo(this.instance.element).data("sortable-item", true);
                        this.instance.options._helper = this.instance.options.helper;
                        this.instance.options.helper = function() {
                            return ui.helper[0];
                        };
                        e.target = this.instance.currentItem[0];
                        this.instance._mouseCapture(e, true);
                        this.instance._mouseStart(e, true, true);
                        this.instance.offset.click.top = inst.offset.click.top;
                        this.instance.offset.click.left = inst.offset.click.left;
                        this.instance.offset.parent.left -= inst.offset.parent.left - this.instance.offset.parent.left;
                        this.instance.offset.parent.top -= inst.offset.parent.top - this.instance.offset.parent.top;
                        inst._propagate("toSortable", e);
                    }
                    if (this.instance.currentItem) this.instance._mouseDrag(e);
                } else {
                    if (this.instance.isOver) {
                        this.instance.isOver = 0;
                        this.instance.cancelHelperRemoval = true;
                        this.instance.options.revert = false;
                        this.instance._mouseStop(e, true);
                        this.instance.options.helper = this.instance.options._helper;
                        this.instance.currentItem.remove();
                        if (this.instance.placeholder) this.instance.placeholder.remove();
                        inst._propagate("fromSortable", e);
                    }
                };
            });
        }
    });
    $.ui.plugin.add("draggable", "stack", {
        start: function(e, ui) {
            var group = $.makeArray($(ui.options.stack.group)).sort(function(a, b) {
                return (parseInt($(a).css("zIndex"), 10) || ui.options.stack.min) - (parseInt($(b).css("zIndex"), 10) || ui.options.stack.min);
            });
            $(group).each(function(i) {
                this.style.zIndex = ui.options.stack.min + i;
            });
            this[0].style.zIndex = ui.options.stack.min + group.length;
        }
    });
})(jQuery); (function($) {
    function contains(a, b) {
        var safari2 = $.browser.safari && $.browser.version < 522;
        if (a.contains && !safari2) {
            return a.contains(b);
        }
        if (a.compareDocumentPosition) return !! (a.compareDocumentPosition(b) & 16);
        while (b = b.parentNode) if (b == a) return true;
        return false;
    };
    $.widget("ui.sortable", $.extend({},
    $.ui.mouse, {
        _init: function() {
            var o = this.options;
            this.containerCache = {};
            this.element.addClass("ui-sortable");
            this.refresh();
            this.floating = this.items.length ? (/left|right/).test(this.items[0].item.css('float')) : false;
            this.offset = this.element.offset();
            this._mouseInit();
        },
        plugins: {},
        ui: function(inst) {
            return {
                helper: (inst || this)["helper"],
                placeholder: (inst || this)["placeholder"] || $([]),
                position: (inst || this)["position"],
                absolutePosition: (inst || this)["positionAbs"],
                options: this.options,
                element: this.element,
                item: (inst || this)["currentItem"],
                sender: inst ? inst.element: null
            };
        },
        _propagate: function(n, e, inst, noPropagation) {
            $.ui.plugin.call(this, n, [e, this.ui(inst)]);
            if (!noPropagation) this.element.triggerHandler(n == "sort" ? n: "sort" + n, [e, this.ui(inst)], this.options[n]);
        },
        serialize: function(o) {
            var items = this._getItemsAsjQuery(o && o.connected);
            var str = [];
            o = o || {};
            $(items).each(function() {
                var res = ($(this.item || this).attr(o.attribute || 'id') || '').match(o.expression || (/(.+)[-=_](.+)/));
                if (res) str.push((o.key || res[1] + '[]') + '=' + (o.key && o.expression ? res[1] : res[2]));
            });
            return str.join('&');
        },
        toArray: function(o) {
            var items = this._getItemsAsjQuery(o && o.connected);
            var ret = [];
            items.each(function() {
                ret.push($(this).attr(o.attr || 'id'));
            });
            return ret;
        },
        _intersectsWith: function(item) {
            var x1 = this.positionAbs.left,
            x2 = x1 + this.helperProportions.width,
            y1 = this.positionAbs.top,
            y2 = y1 + this.helperProportions.height;
            var l = item.left,
            r = l + item.width,
            t = item.top,
            b = t + item.height;
            var dyClick = this.offset.click.top,
            dxClick = this.offset.click.left;
            var isOverElement = (y1 + dyClick) > t && (y1 + dyClick) < b && (x1 + dxClick) > l && (x1 + dxClick) < r;
            if (this.options.tolerance == "pointer" || this.options.forcePointerForContainers || (this.options.tolerance == "guess" && this.helperProportions[this.floating ? 'width': 'height'] > item[this.floating ? 'width': 'height'])) {
                return isOverElement;
            } else {
                return (l < x1 + (this.helperProportions.width / 2) && x2 - (this.helperProportions.width / 2) < r && t < y1 + (this.helperProportions.height / 2) && y2 - (this.helperProportions.height / 2) < b);
            }
        },
        _intersectsWithEdge: function(item) {
            var x1 = this.positionAbs.left,
            x2 = x1 + this.helperProportions.width,
            y1 = this.positionAbs.top,
            y2 = y1 + this.helperProportions.height;
            var l = item.left,
            r = l + item.width,
            t = item.top,
            b = t + item.height;
            var dyClick = this.offset.click.top,
            dxClick = this.offset.click.left;
            var isOverElement = (y1 + dyClick) > t && (y1 + dyClick) < b && (x1 + dxClick) > l && (x1 + dxClick) < r;
            if (this.options.tolerance == "pointer" || (this.options.tolerance == "guess" && this.helperProportions[this.floating ? 'width': 'height'] > item[this.floating ? 'width': 'height'])) {
                if (!isOverElement) return false;
                if (this.floating) {
                    if ((x1 + dxClick) > l && (x1 + dxClick) < l + item.width / 2) return 2;
                    if ((x1 + dxClick) > l + item.width / 2 && (x1 + dxClick) < r) return 1;
                } else {
                    var height = item.height;
                    var direction = y1 - this.updateOriginalPosition.top < 0 ? 2 : 1;
                    if (direction == 1 && (y1 + dyClick) < t + height / 2) {
                        return 2;
                    } else if (direction == 2 && (y1 + dyClick) > t + height / 2) {
                        return 1;
                    }
                }
            } else {
                if (! (l < x1 + (this.helperProportions.width / 2) && x2 - (this.helperProportions.width / 2) < r && t < y1 + (this.helperProportions.height / 2) && y2 - (this.helperProportions.height / 2) < b)) return false;
                if (this.floating) {
                    if (x2 > l && x1 < l) return 2;
                    if (x1 < r && x2 > r) return 1;
                } else {
                    if (y2 > t && y1 < t) return 1;
                    if (y1 < b && y2 > b) return 2;
                }
            }
            return false;
        },
        refresh: function() {
            this._refreshItems();
            this.refreshPositions();
        },
        _getItemsAsjQuery: function(connected) {
            var self = this;
            var items = [];
            var queries = [];
            if (this.options.connectWith && connected) {
                for (var i = this.options.connectWith.length - 1; i >= 0; i--) {
                    var cur = $(this.options.connectWith[i]);
                    for (var j = cur.length - 1; j >= 0; j--) {
                        var inst = $.data(cur[j], 'sortable');
                        if (inst && inst != this && !inst.options.disabled) {
                            queries.push([$.isFunction(inst.options.items) ? inst.options.items.call(inst.element) : $(inst.options.items, inst.element).not(".ui-sortable-helper"), inst]);
                        }
                    };
                };
            }
            queries.push([$.isFunction(this.options.items) ? this.options.items.call(this.element, null, {
                options: this.options,
                item: this.currentItem
            }) : $(this.options.items, this.element).not(".ui-sortable-helper"), this]);
            for (var i = queries.length - 1; i >= 0; i--) {
                queries[i][0].each(function() {
                    items.push(this);
                });
            };
            return $(items);
        },
        _removeCurrentsFromItems: function() {
            var list = this.currentItem.find(":data(sortable-item)");
            for (var i = 0; i < this.items.length; i++) {
                for (var j = 0; j < list.length; j++) {
                    if (list[j] == this.items[i].item[0]) this.items.splice(i, 1);
                };
            };
        },
        _refreshItems: function() {
            this.items = [];
            this.containers = [this];
            var items = this.items;
            var self = this;
            var queries = [[$.isFunction(this.options.items) ? this.options.items.call(this.element, null, {
                options: this.options,
                item: this.currentItem
            }) : $(this.options.items, this.element), this]];
            if (this.options.connectWith) {
                for (var i = this.options.connectWith.length - 1; i >= 0; i--) {
                    var cur = $(this.options.connectWith[i]);
                    for (var j = cur.length - 1; j >= 0; j--) {
                        var inst = $.data(cur[j], 'sortable');
                        if (inst && inst != this && !inst.options.disabled) {
                            queries.push([$.isFunction(inst.options.items) ? inst.options.items.call(inst.element) : $(inst.options.items, inst.element), inst]);
                            this.containers.push(inst);
                        }
                    };
                };
            }
            for (var i = queries.length - 1; i >= 0; i--) {
                queries[i][0].each(function() {
                    $.data(this, 'sortable-item', queries[i][1]);
                    items.push({
                        item: $(this),
                        instance: queries[i][1],
                        width: 0,
                        height: 0,
                        left: 0,
                        top: 0
                    });
                });
            };
        },
        refreshPositions: function(fast) {
            if (this.offsetParent) {
                var po = this.offsetParent.offset();
                this.offset.parent = {
                    top: po.top + this.offsetParentBorders.top,
                    left: po.left + this.offsetParentBorders.left
                };
            }
            for (var i = this.items.length - 1; i >= 0; i--) {
                if (this.items[i].instance != this.currentContainer && this.currentContainer && this.items[i].item[0] != this.currentItem[0]) continue;
                var t = this.options.toleranceElement ? $(this.options.toleranceElement, this.items[i].item) : this.items[i].item;
                if (!fast) {
                    this.items[i].width = t[0].offsetWidth;
                    this.items[i].height = t[0].offsetHeight;
                }
                var p = t.offset();
                this.items[i].left = p.left;
                this.items[i].top = p.top;
            };
            if (this.options.custom && this.options.custom.refreshContainers) {
                this.options.custom.refreshContainers.call(this);
            } else {
                for (var i = this.containers.length - 1; i >= 0; i--) {
                    var p = this.containers[i].element.offset();
                    this.containers[i].containerCache.left = p.left;
                    this.containers[i].containerCache.top = p.top;
                    this.containers[i].containerCache.width = this.containers[i].element.outerWidth();
                    this.containers[i].containerCache.height = this.containers[i].element.outerHeight();
                };
            }
        },
        destroy: function() {
            this.element.removeClass("ui-sortable ui-sortable-disabled").removeData("sortable").unbind(".sortable");
            this._mouseDestroy();
            for (var i = this.items.length - 1; i >= 0; i--) this.items[i].item.removeData("sortable-item");
        },
        _createPlaceholder: function(that) {
            var self = that || this,
            o = self.options;
            if (!o.placeholder || o.placeholder.constructor == String) {
                var className = o.placeholder;
                o.placeholder = {
                    element: function() {
                        var el = $(document.createElement(self.currentItem[0].nodeName)).addClass(className || "ui-sortable-placeholder")[0];
                        if (!className) {
                            el.style.visibility = "hidden";
                            document.body.appendChild(el);
                            el.innerHTML = self.currentItem[0].innerHTML;
                            document.body.removeChild(el);
                        };
                        return el;
                    },
                    update: function(container, p) {
                        if (className && !o.forcePlaceholderSize) return;
                        if (!p.height()) {
                            p.height(self.currentItem.innerHeight() - parseInt(self.currentItem.css('paddingTop') || 0, 10) - parseInt(self.currentItem.css('paddingBottom') || 0, 10));
                        };
                        if (!p.width()) {
                            p.width(self.currentItem.innerWidth() - parseInt(self.currentItem.css('paddingLeft') || 0, 10) - parseInt(self.currentItem.css('paddingRight') || 0, 10));
                        };
                    }
                };
            }
            self.placeholder = $(o.placeholder.element.call(self.element, self.currentItem)) self.currentItem.parent()[0].appendChild(self.placeholder[0]);
            self.placeholder[0].parentNode.insertBefore(self.placeholder[0], self.currentItem[0]);
            o.placeholder.update(self, self.placeholder);
        },
        _contactContainers: function(e) {
            for (var i = this.containers.length - 1; i >= 0; i--) {
                if (this._intersectsWith(this.containers[i].containerCache)) {
                    if (!this.containers[i].containerCache.over) {
                        if (this.currentContainer != this.containers[i]) {
                            var dist = 10000;
                            var itemWithLeastDistance = null;
                            var base = this.positionAbs[this.containers[i].floating ? 'left': 'top'];
                            for (var j = this.items.length - 1; j >= 0; j--) {
                                if (!contains(this.containers[i].element[0], this.items[j].item[0])) continue;
                                var cur = this.items[j][this.containers[i].floating ? 'left': 'top'];
                                if (Math.abs(cur - base) < dist) {
                                    dist = Math.abs(cur - base);
                                    itemWithLeastDistance = this.items[j];
                                }
                            }
                            if (!itemWithLeastDistance && !this.options.dropOnEmpty) continue;
                            this.currentContainer = this.containers[i];
                            itemWithLeastDistance ? this.options.sortIndicator.call(this, e, itemWithLeastDistance, null, true) : this.options.sortIndicator.call(this, e, null, this.containers[i].element, true);
                            this._propagate("change", e);
                            this.containers[i]._propagate("change", e, this);
                            this.options.placeholder.update(this.currentContainer, this.placeholder);
                        }
                        this.containers[i]._propagate("over", e, this);
                        this.containers[i].containerCache.over = 1;
                    }
                } else {
                    if (this.containers[i].containerCache.over) {
                        this.containers[i]._propagate("out", e, this);
                        this.containers[i].containerCache.over = 0;
                    }
                }
            };
        },
        _mouseCapture: function(e, overrideHandle) {
            if (this.options.disabled || this.options.type == 'static') return false;
            this._refreshItems();
            var currentItem = null,
            self = this,
            nodes = $(e.target).parents().each(function() {
                if ($.data(this, 'sortable-item') == self) {
                    currentItem = $(this);
                    return false;
                }
            });
            if ($.data(e.target, 'sortable-item') == self) currentItem = $(e.target);
            if (!currentItem) return false;
            if (this.options.handle && !overrideHandle) {
                var validHandle = false;
                $(this.options.handle, currentItem).find("*").andSelf().each(function() {
                    if (this == e.target) validHandle = true;
                });
                if (!validHandle) return false;
            }
            this.currentItem = currentItem;
            this._removeCurrentsFromItems();
            return true;
        },
        createHelper: function(e) {
            var o = this.options;
            var helper = typeof o.helper == 'function' ? $(o.helper.apply(this.element[0], [e, this.currentItem])) : (o.helper == "original" ? this.currentItem: this.currentItem.clone());
            if (!helper.parents('body').length) $(o.appendTo != 'parent' ? o.appendTo: this.currentItem[0].parentNode)[0].appendChild(helper[0]);
            return helper;
        },
        _mouseStart: function(e, overrideHandle, noActivation) {
            var o = this.options;
            this.currentContainer = this;
            this.refreshPositions();
            this.helper = this.createHelper(e);
            this.margins = {
                left: (parseInt(this.currentItem.css("marginLeft"), 10) || 0),
                top: (parseInt(this.currentItem.css("marginTop"), 10) || 0)
            };
            this.offset = this.currentItem.offset();
            this.offset = {
                top: this.offset.top - this.margins.top,
                left: this.offset.left - this.margins.left
            };
            this.offset.click = {
                left: e.pageX - this.offset.left,
                top: e.pageY - this.offset.top
            };
            this.offsetParent = this.helper.offsetParent();
            var po = this.offsetParent.offset();
            this.offsetParentBorders = {
                top: (parseInt(this.offsetParent.css("borderTopWidth"), 10) || 0),
                left: (parseInt(this.offsetParent.css("borderLeftWidth"), 10) || 0)
            };
            this.offset.parent = {
                top: po.top + this.offsetParentBorders.top,
                left: po.left + this.offsetParentBorders.left
            };
            this.updateOriginalPosition = this.originalPosition = this._generatePosition(e);
            this.domPosition = {
                prev: this.currentItem.prev()[0],
                parent: this.currentItem.parent()[0]
            };
            this.helperProportions = {
                width: this.helper.outerWidth(),
                height: this.helper.outerHeight()
            };
            if (o.helper == "original") {
                this._storedCSS = {
                    position: this.currentItem.css("position"),
                    top: this.currentItem.css("top"),
                    left: this.currentItem.css("left"),
                    clear: this.currentItem.css("clear")
                };
            } else {
                this.currentItem.hide();
            }
            this.helper.css({
                position: 'absolute',
                clear: 'both'
            }).addClass('ui-sortable-helper');
            this._createPlaceholder();
            this._propagate("start", e);
            if (!this._preserveHelperProportions) this.helperProportions = {
                width: this.helper.outerWidth(),
                height: this.helper.outerHeight()
            };
            if (o.cursorAt) {
                if (o.cursorAt.left != undefined) this.offset.click.left = o.cursorAt.left;
                if (o.cursorAt.right != undefined) this.offset.click.left = this.helperProportions.width - o.cursorAt.right;
                if (o.cursorAt.top != undefined) this.offset.click.top = o.cursorAt.top;
                if (o.cursorAt.bottom != undefined) this.offset.click.top = this.helperProportions.height - o.cursorAt.bottom;
            }
            if (o.containment) {
                if (o.containment == 'parent') o.containment = this.helper[0].parentNode;
                if (o.containment == 'document' || o.containment == 'window') this.containment = [0 - this.offset.parent.left, 0 - this.offset.parent.top, $(o.containment == 'document' ? document: window).width() - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.element.css("marginRight"), 10) || 0), ($(o.containment == 'document' ? document: window).height() || document.body.parentNode.scrollHeight) - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.element.css("marginBottom"), 10) || 0)];
                if (! (/^(document|window|parent)$/).test(o.containment)) {
                    var ce = $(o.containment)[0];
                    var co = $(o.containment).offset();
                    var over = ($(ce).css("overflow") != 'hidden');
                    this.containment = [co.left + (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.parent.left, co.top + (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.parent.top, co.left + (over ? Math.max(ce.scrollWidth, ce.offsetWidth) : ce.offsetWidth) - (parseInt($(ce).css("borderLeftWidth"), 10) || 0) - this.offset.parent.left - this.helperProportions.width - this.margins.left - (parseInt(this.currentItem.css("marginRight"), 10) || 0), co.top + (over ? Math.max(ce.scrollHeight, ce.offsetHeight) : ce.offsetHeight) - (parseInt($(ce).css("borderTopWidth"), 10) || 0) - this.offset.parent.top - this.helperProportions.height - this.margins.top - (parseInt(this.currentItem.css("marginBottom"), 10) || 0)];
                }
            }
            if (!noActivation) {
                for (var i = this.containers.length - 1; i >= 0; i--) {
                    this.containers[i]._propagate("activate", e, this);
                }
            }
            if ($.ui.ddmanager) $.ui.ddmanager.current = this;
            if ($.ui.ddmanager && !o.dropBehaviour) $.ui.ddmanager.prepareOffsets(this, e);
            this.dragging = true;
            this._mouseDrag(e);
            return true;
        },
        _convertPositionTo: function(d, pos) {
            if (!pos) pos = this.position;
            var mod = d == "absolute" ? 1 : -1;
            return {
                top: (pos.top + this.offset.parent.top * mod - (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop) * mod + this.margins.top * mod),
                left: (pos.left + this.offset.parent.left * mod - (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft) * mod + this.margins.left * mod)
            };
        },
        _generatePosition: function(e) {
            var o = this.options;
            var position = {
                top: (e.pageY - this.offset.click.top - this.offset.parent.top + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop)),
                left: (e.pageX - this.offset.click.left - this.offset.parent.left + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft))
            };
            if (!this.originalPosition) return position;
            if (this.containment) {
                if (position.left < this.containment[0]) position.left = this.containment[0];
                if (position.top < this.containment[1]) position.top = this.containment[1];
                if (position.left > this.containment[2]) position.left = this.containment[2];
                if (position.top > this.containment[3]) position.top = this.containment[3];
            }
            if (o.grid) {
                var top = this.originalPosition.top + Math.round((position.top - this.originalPosition.top) / o.grid[1]) * o.grid[1];
                position.top = this.containment ? (!(top < this.containment[1] || top > this.containment[3]) ? top: (!(top < this.containment[1]) ? top - o.grid[1] : top + o.grid[1])) : top;
                var left = this.originalPosition.left + Math.round((position.left - this.originalPosition.left) / o.grid[0]) * o.grid[0];
                position.left = this.containment ? (!(left < this.containment[0] || left > this.containment[2]) ? left: (!(left < this.containment[0]) ? left - o.grid[0] : left + o.grid[0])) : left;
            }
            return position;
        },
        _mouseDrag: function(e) {
            this.position = this._generatePosition(e);
            this.positionAbs = this._convertPositionTo("absolute");
            $.ui.plugin.call(this, "sort", [e, this.ui()]);
            this.positionAbs = this._convertPositionTo("absolute");
            this.helper[0].style.left = this.position.left + 'px';
            this.helper[0].style.top = this.position.top + 'px';
            for (var i = this.items.length - 1; i >= 0; i--) {
                var intersection = this._intersectsWithEdge(this.items[i]);
                if (!intersection) continue;
                if (this.items[i].item[0] != this.currentItem[0] && this.placeholder[intersection == 1 ? "next": "prev"]()[0] != this.items[i].item[0] && !contains(this.placeholder[0], this.items[i].item[0]) && (this.options.type == 'semi-dynamic' ? !contains(this.element[0], this.items[i].item[0]) : true)) {
                    this.updateOriginalPosition = this._generatePosition(e);
                    this.direction = intersection == 1 ? "down": "up";
                    this.options.sortIndicator.call(this, e, this.items[i]);
                    this._propagate("change", e);
                    break;
                }
            }
            this._contactContainers(e);
            if ($.ui.ddmanager) $.ui.ddmanager.drag(this, e);
            this.element.triggerHandler("sort", [e, this.ui()], this.options["sort"]);
            return false;
        },
        _rearrange: function(e, i, a, hardRefresh) {
            a ? a[0].appendChild(this.placeholder[0]) : i.item[0].parentNode.insertBefore(this.placeholder[0], (this.direction == 'down' ? i.item[0] : i.item[0].nextSibling));
            this.counter = this.counter ? ++this.counter: 1;
            var self = this,
            counter = this.counter;
            window.setTimeout(function() {
                if (counter == self.counter) self.refreshPositions(!hardRefresh);
            },
            0);
        },
        _mouseStop: function(e, noPropagation) {
            if ($.ui.ddmanager && !this.options.dropBehaviour) $.ui.ddmanager.drop(this, e);
            if (this.options.revert) {
                var self = this;
                var cur = self.placeholder.offset();
                $(this.helper).animate({
                    left: cur.left - this.offset.parent.left - self.margins.left + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollLeft),
                    top: cur.top - this.offset.parent.top - self.margins.top + (this.offsetParent[0] == document.body ? 0 : this.offsetParent[0].scrollTop)
                },
                parseInt(this.options.revert, 10) || 500,
                function() {
                    self._clear(e);
                });
            } else {
                this._clear(e, noPropagation);
            }
            return false;
        },
        _clear: function(e, noPropagation) {
            if (!this._noFinalSort) this.placeholder.before(this.currentItem);
            this._noFinalSort = null;
            if (this.options.helper == "original") this.currentItem.css(this._storedCSS).removeClass("ui-sortable-helper");
            else this.currentItem.show();
            if (this.domPosition.prev != this.currentItem.prev().not(".ui-sortable-helper")[0] || this.domPosition.parent != this.currentItem.parent()[0]) this._propagate("update", e, null, noPropagation);
            if (!contains(this.element[0], this.currentItem[0])) {
                this._propagate("remove", e, null, noPropagation);
                for (var i = this.containers.length - 1; i >= 0; i--) {
                    if (contains(this.containers[i].element[0], this.currentItem[0])) {
                        this.containers[i]._propagate("update", e, this, noPropagation);
                        this.containers[i]._propagate("receive", e, this, noPropagation);
                    }
                };
            };
            for (var i = this.containers.length - 1; i >= 0; i--) {
                this.containers[i]._propagate("deactivate", e, this, noPropagation);
                if (this.containers[i].containerCache.over) {
                    this.containers[i]._propagate("out", e, this);
                    this.containers[i].containerCache.over = 0;
                }
            }
            this.dragging = false;
            if (this.cancelHelperRemoval) {
                this._propagate("beforeStop", e, null, noPropagation);
                this._propagate("stop", e, null, noPropagation);
                return false;
            }
            this._propagate("beforeStop", e, null, noPropagation);
            this.placeholder.remove();
            if (this.options.helper != "original") this.helper.remove();
            this.helper = null;
            this._propagate("stop", e, null, noPropagation);
            return true;
        }
    }));
    $.extend($.ui.sortable, {
        getter: "serialize toArray",
        defaults: {
            helper: "original",
            tolerance: "guess",
            distance: 1,
            delay: 0,
            scroll: true,
            scrollSensitivity: 20,
            scrollSpeed: 20,
            cancel: ":input",
            items: '> *',
            zIndex: 1000,
            dropOnEmpty: true,
            appendTo: "parent",
            sortIndicator: $.ui.sortable.prototype._rearrange,
            scope: "default",
            forcePlaceholderSize: false
        }
    });
    $.ui.plugin.add("sortable", "cursor", {
        start: function(e, ui) {
            var t = $('body');
            if (t.css("cursor")) ui.options._cursor = t.css("cursor");
            t.css("cursor", ui.options.cursor);
        },
        beforeStop: function(e, ui) {
            if (ui.options._cursor) $('body').css("cursor", ui.options._cursor);
        }
    });
    $.ui.plugin.add("sortable", "zIndex", {
        start: function(e, ui) {
            var t = ui.helper;
            if (t.css("zIndex")) ui.options._zIndex = t.css("zIndex");
            t.css('zIndex', ui.options.zIndex);
        },
        beforeStop: function(e, ui) {
            if (ui.options._zIndex) $(ui.helper).css('zIndex', ui.options._zIndex);
        }
    });
    $.ui.plugin.add("sortable", "opacity", {
        start: function(e, ui) {
            var t = ui.helper;
            if (t.css("opacity")) ui.options._opacity = t.css("opacity");
            t.css('opacity', ui.options.opacity);
        },
        beforeStop: function(e, ui) {
            if (ui.options._opacity) $(ui.helper).css('opacity', ui.options._opacity);
        }
    });
    $.ui.plugin.add("sortable", "scroll", {
        start: function(e, ui) {
            var o = ui.options;
            var i = $(this).data("sortable");
            i.overflowY = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-y'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (i.currentItem);
            i.overflowX = function(el) {
                do {
                    if (/auto|scroll/.test(el.css('overflow')) || (/auto|scroll/).test(el.css('overflow-x'))) return el;
                    el = el.parent();
                } while ( el [ 0 ].parentNode);
                return $(document);
            } (i.currentItem);
            if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') i.overflowYOffset = i.overflowY.offset();
            if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') i.overflowXOffset = i.overflowX.offset();
        },
        sort: function(e, ui) {
            var o = ui.options;
            var i = $(this).data("sortable");
            if (i.overflowY[0] != document && i.overflowY[0].tagName != 'HTML') {
                if ((i.overflowYOffset.top + i.overflowY[0].offsetHeight) - e.pageY < o.scrollSensitivity) i.overflowY[0].scrollTop = i.overflowY[0].scrollTop + o.scrollSpeed;
                if (e.pageY - i.overflowYOffset.top < o.scrollSensitivity) i.overflowY[0].scrollTop = i.overflowY[0].scrollTop - o.scrollSpeed;
            } else {
                if (e.pageY - $(document).scrollTop() < o.scrollSensitivity) $(document).scrollTop($(document).scrollTop() - o.scrollSpeed);
                if ($(window).height() - (e.pageY - $(document).scrollTop()) < o.scrollSensitivity) $(document).scrollTop($(document).scrollTop() + o.scrollSpeed);
            }
            if (i.overflowX[0] != document && i.overflowX[0].tagName != 'HTML') {
                if ((i.overflowXOffset.left + i.overflowX[0].offsetWidth) - e.pageX < o.scrollSensitivity) i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft + o.scrollSpeed;
                if (e.pageX - i.overflowXOffset.left < o.scrollSensitivity) i.overflowX[0].scrollLeft = i.overflowX[0].scrollLeft - o.scrollSpeed;
            } else {
                if (e.pageX - $(document).scrollLeft() < o.scrollSensitivity) $(document).scrollLeft($(document).scrollLeft() - o.scrollSpeed);
                if ($(window).width() - (e.pageX - $(document).scrollLeft()) < o.scrollSensitivity) $(document).scrollLeft($(document).scrollLeft() + o.scrollSpeed);
            }
        }
    });
    $.ui.plugin.add("sortable", "axis", {
        sort: function(e, ui) {
            var i = $(this).data("sortable");
            if (ui.options.axis == "y") i.position.left = i.originalPosition.left;
            if (ui.options.axis == "x") i.position.top = i.originalPosition.top;
        }
    });
})(jQuery);