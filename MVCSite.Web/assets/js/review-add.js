var starOptions = {
    min:0,
    max:5,
    step:1,
    size: 'xs',
    showClear: false,
    showCaption: false,
    theme: "star-kootour",
    emptyStar: "<i class='glyphicon glyphicon-star'></i>"
};

$("#accuracy").rating(starOptions);
$("#communication").rating(starOptions);
$("#services").rating(starOptions);
$("#knowledge").rating(starOptions);
$("#value").rating(starOptions);
