$(document).ready(function($) {
    //Evalute the strength of the password
    $('#signupPassword').strength({
        strengthClass: 'strength',
        strengthMeterClass: 'strength_meter',
        strengthButtonClass: 'button_strength',
        strengthButtonText: 'Show Password',
        strengthButtonTextToggle: 'Hide Password'
    });

    //check if the password is the same
    var password = $("#signupPassword");
    var repassword = $("#signupRePassword");
    var button = $("#signupButton");

    $("#signupPassword, #signupRePassword").on("input", function (e) {

        var value1 = $(password).val();
        var value2 = $(repassword).val();

        if (value1 == value2) {
            $(repassword).parent()
            .removeClass("has-warning")
            .addClass("has-success");

            $(button).removeAttr("disabled");
        } else {
            $(repassword).parent()
            .removeClass("has-success")
            .addClass("has-warning");

            $(button).attr("disabled", "disabled");
        }
    })

    $("#signupApprove").on("change", function (e) {

        if (!$(this).prop( "checked")) {
            $("#signupButton").attr("disabled", "disabled");
        } else {
            $("#signupButton").removeAttr("disabled");
        }
    });
});
