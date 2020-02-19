
Element.prototype.remove = function() {
    this.parentElement.removeChild(this);
}

var notifications = document.getElementsByClassName("notification");

for(var i = 0; i <notifications.length; i++) {

    notifications[i].addEventListener("click", function (e) {

        var target = e.target;

        if (target.classList.contains("close-icon")) {

            target.parentNode.remove();
        }
    });
}

