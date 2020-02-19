var templates = {
    discount: function (value) {

        var fragment = document.createDocumentFragment();
        
        var div = document.createElement("div");
        div.className = "widget-row col-xs-12";

        var div1 = document.createElement("div");
        div1.className = "col-xs-6 nopadding";
        div1.innerHTML = "<span>Discount:</span>";

        var div2 = document.createElement("div");
        div2.className = "col-xs-6 nopadding text-right";
        div2.innerHTML = "<span>" + value + "%</span>";

        div.appendChild(div1);
        div.appendChild(div2);

        fragment.appendChild(div);

        return fragment;
    }
}

function Cart(options) {
    this.container = options.container;
    this.discountForm = options.discountForm;
}

Cart.prototype.addDiscount = function (value) {
    var discountElement = templates.discount;
    var html = discountElement(15);

    this.container.insertBefore(html, this.discountForm.nextSibling);

    this.discountForm.remove();
}



