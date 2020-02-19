var indicators = document.getElementsByClassName('indicators')[0];
var indicator = new Indicator(indicators, {});
indicator.init();

var emailForm = document.getElementById("email-form");

if(emailForm) {
    emailForm.addEventListener("submit", function(e) {
        e.preventDefault();

        indicator.goTo("step-2");

        return false;
    });
}

