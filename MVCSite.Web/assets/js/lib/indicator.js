Indicator = function(list, options) {
    this.list = list;
    this.options = options;
};


Indicator.prototype.init = function () {

    var elements = this.list.getElementsByTagName("li");

    for (var j = 0; j < elements.length; j++) {

        var stepIndicator = elements[j];
        var step = stepIndicator.dataset.step;
        var stepElement = document.getElementById(step);
            
        if (!elements[j].classList.contains('active')) {
            stepElement.style.display = "none";
        } else {
            stepElement.style.display = "block";
        }


        //onclick
        /*(function(indicatorObject, stepIndicator) {

            stepIndicator.addEventListener("click", function() {

                indicator.callback(stepIndicator);
            });
        } (this, stepIndicator));*/
    }
}

Indicator.prototype.goTo = function (stepReq) {

    var elements = this.list.getElementsByTagName("li");

    for (var j = 0; j < elements.length; j++) {

        var stepIndicator = elements[j];
        var step = stepIndicator.dataset.step;
        var stepElement = document.getElementById(step);
            
        if (step == stepReq) {
            stepIndicator.classList.add('active');
            stepElement.style.display = "block";
        } else {
            stepIndicator.classList.remove('active');
            stepElement.style.display = "none";
        }
    }
}
