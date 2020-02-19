var defaults = {
    notsaved : "Save to the Wish List",
    saved: "Remove to the Wish List"
}

function Wishlist() {
    this.links - null;
}

Wishlist.prototype.setLink = function (links) {

    this.links = links;

    for (var i=0; i < links.length; i++) {

        var link = links[i];

        (function (Wishlist, link) {
            link.addEventListener("click", function (e) {
                if(this.dataset.saved == "false") {
                    Wishlist.add(this);
                } else {
                    Wishlist.remove(this);
                }

                e.preventDefault();
            });
        } (this, link));
    }
}

Wishlist.prototype.add = function (link) {
    link.innerHTML = defaults.saved;
    link.dataset.saved = "true";
}

Wishlist.prototype.remove = function (link) {
    link.innerHTML = defaults.notsaved;
    link.dataset.saved = "false";
}
