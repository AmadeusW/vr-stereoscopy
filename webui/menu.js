function showMenu() {
    var menu = document.querySelector("#menuPane");

    if (menu === null) {
        console.log("null");
    }
    else if (typeof(menu) === undefined) {
        console.log("undefined");
    }
    else {
        console.log("Exists! " + menu);
        menu.setAttribute("visible", true);
    }
    
    document.querySelector("#menuTitle").setAttribute("value", "Welcome");
    var template = document.querySelector("#categoryTemplate");

    for (var categoryId = 0; categoryId < categories.length; categoryId++) {

        var category = template.cloneNode(/*deep:*/true);
        category.setAttribute("id", "category" + categoryId);
        category.setAttribute("position", categoryId + " 0 0");
        var title = category.querySelector(".title");
        title.setAttribute("value", categories[categoryId].DisplayName);
        category.querySelector(".thumbL").
            setAttribute("src", "images/" + categories[categoryId].Thumbnail + ".T.L.jpg");
        category.querySelector(".thumbR").
            setAttribute("src", "images/" + categories[categoryId].Thumbnail + ".T.R.jpg");
        menu.appendChild(category);
    }
    goToCategory(0); //xxx: experiment
}

function goToCategory(categoryId) {
    // todo: actually go to category. for now just testing loading JSON
    console.log("goToCategory");
    getJSON('images/posts.json',
        function(err, data) {
            if (err != null) {
                console.log('Error accessing image data: ' + err);
            } else {
                console.log('Image data: ' + data);
            }
        }
    );
}