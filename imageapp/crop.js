window.onload = function() {
    document.getElementById("url").onchange = function() {
        console.log( document.getElementById("url").value);
        document.getElementById("mainImage").setAttribute("src", document.getElementById("url").value);
    };
};