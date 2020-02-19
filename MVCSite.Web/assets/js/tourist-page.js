$(".purchase-payment-link").on("click", function (e) {
    e.preventDefault();
    
    var arrowDown = "images/icons/arrow_head_down_192X192.png";
    var arrowUp = "images/icons/arrow_head_right_192X192.png";
    var purchasePaymentID = this.dataset.purchasePaymentId;

    if (purchasePaymentID) {
        $("#purchase_" + purchasePaymentID).toggle();

        var currentImage =  $(this).find("img")[0];

        if ($(currentImage).attr("src") == arrowUp) {

            $(currentImage).attr("src", arrowDown);
        } else {

            $(currentImage).attr("src", arrowUp);
        }
    }
})
