async function initializeMenu() {
    var data = httpGet('images/categories.json', 'json');
    var categories = (await data).categories;
    return categories;
}

async function initializeCategory(feed) {
    var data = httpGet('images/' + feed, 'json');
    var scenes = (await data).scenes;
    console.log("initializeCategory", scenes);
    return scenes;
}

function buildMenu(categories) {
    var menu = document.querySelector("#menuPane");
    var template = document.querySelector("#categoryTemplate");

    for (var categoryId = 0; categoryId < categories.length; categoryId++) {
        var category = template.cloneNode(/*deep:*/true);
        category.setAttribute("id", "category" + categoryId);
        category.setAttribute("position", categoryId + " 0 0");
        category.setAttribute("visible", true);
        category.querySelector(".title")
            .setAttribute("value", categories[categoryId].DisplayName);
        category.querySelector(".thumbL")
            .setAttribute("src", "images/" + categories[categoryId].Thumbnail + ".T.L.jpg");
        category.querySelector(".thumbR")
            .setAttribute("src", "images/" + categories[categoryId].Thumbnail + ".T.R.jpg");
        category.querySelector(".categoryThumb")
            .setAttribute("listener__click", "event: click; callback: onConfirm; params: "+categoryId+", -1");

        menu.appendChild(category);

        for (var subId = 0; subId < categories[categoryId].Subcategories.length; subId++) {
            var sub = template.cloneNode(/*deep:*/true);
            sub.setAttribute("id", "category" + categoryId + "sub" + subId);
            sub.setAttribute("position", categoryId + " " + (-1 - subId) + " 0");
            sub.setAttribute("visible", true);
            sub.querySelector(".title")
                .setAttribute("value", categories[categoryId].Subcategories[subId].DisplayName);
            sub.querySelector(".thumbL").
                setAttribute("src", "images/" + categories[categoryId].Subcategories[subId].Thumbnail + ".T.L.jpg");
            sub.querySelector(".thumbR").
                setAttribute("src", "images/" + categories[categoryId].Subcategories[subId].Thumbnail + ".T.R.jpg");
            sub.querySelector(".categoryThumb")
                .setAttribute("listener__click", "event: click; callback: onConfirm; params: "+categoryId+", "+subId);

            menu.appendChild(sub);
        }
    }
}

function showMenu() {
    document.querySelector("#menuPane").setAttribute("visible", true);
}

function hideMenu() {
    document.querySelector("#menuPane").setAttribute("visible", false);
}

function onSelect(sender, params) {
    sender.emit('grow');
};

function onDeselect(sender, params) {
    sender.emit('shrink');
};

function onNavigate(sender, params) {
    if (params[0] == 'r') {
        nextImage();
    } else if (params[0] == 'l') {
        previousImage();
    }
}

function onConfirm(sender, params) {
    var category = parseInt(params[0]);
    var subcategory = parseInt(params[1]);
    if (category > -1) {
        if (subcategory > -1) {
            // We are in subcategory. Display images
            goToCategory(category, subcategory);
            //hideMenu();
        }
        // We are in category. Display a subcategory?
    }
}

function onShowMenu(sender, params) {
    showMenu();
}

function onHideMenu(sender, params) {
    hideMenu();
}
