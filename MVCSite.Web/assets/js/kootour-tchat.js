var guide = {
    thumbnail: "/images/faces/face_2.jpg"
}

var tourist = {
    thumbnail: "/images/faces/face_1.jpg"
}

var formElement = document.getElementById('kootour-form-tchat');

var tchat = new Tchat(document.getElementById('kootour-tchat'), guide, tourist);

formElement.addEventListener("submit", function (e) {
    e.preventDefault();

    var message = {
        date: new Date(),
        from: tourist,
        content: this.elements[0].value
    }

    tchat.addMessage(message);
    formElement.reset();
    
    return false;
});


tchat.addMessage({
    date: new Date(2016, 05, 21),
    from: guide,
    content: "Hello"
});

tchat.addMessage({
    date: new Date(2016, 05, 21),
    from: guide,
    content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a luctus lectus. Mauris mattis sapien vel nunc aliquet volutpat. Nulla eget dui ut erat pretium maximus nec id sapien."
});

tchat.addMessage({
    date: new Date(2016, 05, 23),
    from: tourist,
    content: "Hello"
});

tchat.addMessage({
    date: new Date(2016, 05, 23),
    from: tourist,
    content: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut a luctus lectus. Mauris mattis sapien vel nunc aliquet volutpat. Nulla eget dui ut erat pretium maximus nec id sapien."
});

