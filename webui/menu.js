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
        /*
        For now, we won't show categories.
        var category = template.cloneNode(true); // deep:true
        category.setAttribute("id", "category" + categoryId);
        category.setAttribute("position", categoryId + " 1 0");
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
        */
        for (var subId = 0; subId < categories[categoryId].Subcategories.length; subId++) {
            var sub = template.cloneNode(/*deep:*/true);
            sub.setAttribute("id", "category" + categoryId + "sub" + subId);
            placeInSpace(sub, (subId - 1) * 20)
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

function placeInSpace(el, angle) {
  distance = 2
  x = distance * Math.sin(angle)
  y = distance * Math.cos(angle)
  el.setAttribute("position", x + " " + y + " 0")
  el.setAttribute("rotation", "0 -" + angle + " 0")
}

function showMenu() {
    document.querySelector("#menuPane").emit("show");
    document.querySelector("#menuCurtain").emit("show");
    document.querySelector("#cursor").setAttribute("visible", "true");
    document.querySelector("#cursor").setAttribute("raycaster", "objects: .ui-menu");
}

function hideMenu() {
    document.querySelector("#menuPane").emit("hide");
    document.querySelector("#menuCurtain").emit("hide");
    document.querySelector("#cursor").setAttribute("visible", "false");
    document.querySelector("#cursor").setAttribute("raycaster", "objects: .ui-scroll");
}

function onSelect(sender, params) {
    console.log(params)
    if (params.length == 0) {
        sender.emit('grow');
    } else {
        console.log(params, document.getElementById(params[0]))
        document.getElementById(params[0]).emit('grow');
    }
};

function onDeselect(sender, params) {
    if (params.length == 0) {
        sender.emit('shrink');
    } else {
        document.getElementById(params[0]).emit('shrink');
    }
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
            console.log("User selected a category");
            goToCategory(category, subcategory);
            hideMenu(); // TODO: show menu by looking up or down
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

function onToggleTimer(sender, params) {
    toggleTimer();
}