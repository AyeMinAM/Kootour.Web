var loadFile = function(event) {
    var output = document.getElementById('output-uploader');
    output.style.backgroundImage = "url('" + URL.createObjectURL(event.target.files[0]) + "')";
};
