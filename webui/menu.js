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

        for (var subId = 0; subId < categories[categoryId].Subcategories.length; subId++) {
            var sub = template.cloneNode(/*deep:*/true);
            sub.setAttribute("id", "category" + categoryId + "sub" + subId);
            sub.setAttribute("position", categoryId + " " + (-1 - subId) + " 0");
            var subTitle = sub.querySelector(".title");
            subTitle.setAttribute("value", categories[categoryId].Subcategories[subId].DisplayName);
            sub.querySelector(".thumbL").
                setAttribute("src", "images/" + categories[categoryId].Subcategories[subId].Thumbnail + ".T.L.jpg");
            sub.querySelector(".thumbR").
                setAttribute("src", "images/" + categories[categoryId].Subcategories[subId].Thumbnail + ".T.R.jpg");
            menu.appendChild(sub);
        }
    }
}

async function initializeMenu() {
    var data = httpGet('images/categories.json', 'json');
    var categories = (await data).categories;
    return categories;
}

async function initializeCategory(feed) {
    var data = httpGet('images/' + feed, 'json');
    var scenes = (await data).scenes;
    return scenes;
}