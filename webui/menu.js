function showMenu(categories) {
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
}

function initializeMenu() {
    // todo: actually go to category. for now just testing loading JSON
    getJSON('images/categories.json',
        function(err, data) {
            console.log(data);
            showMenu(data.categories);
            goToCategory(data.categories[0].Subcategories[0].Feed);
        }
    );
}

function goToCategory(feed) {
    // todo: actually go to category. for now just testing loading JSON
    getJSON('images/' + feed,
        function(err, data) {
            console.info(":: Go to category: " + feed)
            console.log(data);
        }
    );
}