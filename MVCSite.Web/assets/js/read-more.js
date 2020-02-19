function ReadMore(textElement, minHeight) {
    this.isCollapsed = null;
    this.textElement = textElement;
    this.minHeight = minHeight;
    this.maxHeigh = null;
    this.buttonElement = null;
}

ReadMore.prototype.init = function() {
    var _this = this;

    this.maxHeigh = $(this.textElement).height()+40;
    this.buttonElement = $(this.textElement).next();
    this.isCollapsed = true;

    if(this.maxHeigh <= this.minHeight) {
        $(this.buttonElement).hide();
        return;
    }

    $(this.textElement).css("height", this.minHeight + "px");
    $(this.textElement).css("overflow", "hidden");

    $(this.buttonElement).on("click",function() {
        _this.toggle();
        return false;
    });

    return this;
};

ReadMore.prototype.toggle = function() {
    var _this = this;

    if(this.isCollapsed) {

        $(this.textElement).animate({height: this.maxHeigh}, 500, function() {
            _this.isCollapsed = false;
            $(_this.buttonElement).html("read less");
        });
    } else {
        $(this.textElement).animate({height: this.minHeight}, 500, function() {
            _this.isCollapsed = true;
            $(_this.buttonElement).html("read more");
        });
    }
    
}