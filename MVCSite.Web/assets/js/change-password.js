var indicators = document.getElementsByClassName('indicators')[0];
var indicator = new Indicator(indicators, {});
indicator.init();

var passwordForm = document.getElementById("password-form");

if(passwordForm) {
    passwordForm.addEventListener("submit", function(e) {
        e.preventDefault();

        indicator.goTo("step-2");

        return false;
    });
}

