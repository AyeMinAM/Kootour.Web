var indicators = document.getElementsByClassName('indicators')[0];
var indicator = new Indicator(indicators, {});
indicator.init();

var phoneForm = document.getElementById("phone-form");
var phoneNumberLabel = document.getElementById("phoneNumber-label");
var phoneNumber = null;
var digitForm = document.getElementById("digit-form");

phoneForm.addEventListener("submit", function(e) {
    e.preventDefault();

    phoneNumberLabel.innerHTML = document.getElementById("phonenumber").value;

    indicator.goTo("step-2");

    return false;
});

digitForm.addEventListener("submit", function(e) {
    e.preventDefault();

    indicator.goTo("step-3");

    return false;
})
