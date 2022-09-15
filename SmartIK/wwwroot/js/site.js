// Add image and preview
var img = $("#preview > img")[0];
$("#Image").change(function (event) {
    var imgFile = this.files[0];
    if (imgFile && imgFile.type.startsWith("image/")) {
        img.src = URL.createObjectURL(imgFile);
        $("#preview").removeClass("d-none");
    }
    else {
        img.src = "#";
        $("#preview").addClass("d-none");
    }
});