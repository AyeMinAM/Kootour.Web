$(function () {
    //Wishlist
    var wishlist = new Wishlist();
    wishlist.setLink(document.getElementsByClassName("wishlink"));
    
    //Question / Answer
    var questionCount = 3;
    var arrowDown = "images/icons/arrow_head_down_192X192.png";
    var arrowUp = "images/icons/arrow_head_right_192X192.png";
    
    function animationCallback(question) {

        if ($(question).find("img").length > 0) {

            var currentImage = $(question).find("img")[0];

            if ($(currentImage).attr("src") == arrowUp) {

                $(currentImage).attr("src", arrowDown);
            } else {

                $(currentImage).attr("src", arrowUp);
            }
        }
    }

    for (var i = 1; i <= questionCount; i++) {

        var question = $("#question_" + i);
        var answer = $("#answer_" + i);

        (function(question, answer) {

            $(question).on("click", function (e) {

                $(answer).slideToggle( "slow", function() {
                    animationCallback(question);
                });

                return false;
            });
        }(question, answer));
    }

    //Handle cart
    var discountButton = document.getElementById("discountButton");

    var cart = new Cart({
        container: document.getElementById("bookingForm"),
        discountForm: document.getElementById("widget-row-discount-form")
    });

    discountButton.addEventListener("click", function (e) {
        var discountValue = document.getElementById("discountValue");
        cart.addDiscount(discountValue.value);
    });
});
