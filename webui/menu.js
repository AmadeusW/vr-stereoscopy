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
    var galleryItems = [];

    for (var categoryId = 0; categoryId < categories.length; categoryId++) {
        for (var subId = 0; subId < categories[categoryId].Subcategories.length; subId++) {
            galleryItems.push({
                title: categories[categoryId].Subcategories[subId].DisplayName,
                description: categories[categoryId].Subcategories[subId].DisplayName,
                picture: cdnPrefix + categories[categoryId].Subcategories[subId].Thumbnail + ".T.L.jpg",
                id: categoryId,
                subId: subId
            });
        }
    }

    var galleriesVue = new Vue({
        el: '#galleries-menu',
        data: {
          items: galleryItems
        }
      });
}

function onGalleryItemClick(event) {
    var category = event.target.getAttribute("cid")
    var subcategory = event.target.getAttribute("sid")

    goToCategory(category, subcategory);
    goToVR();
}

function placeInCircle(el, angle) {
  radius = 1.5
  distance = 3; // Z position of #menuPane
  DegPerRad = 57.29577

  rads = angle / DegPerRad; // convert to radians
  x = radius * Math.sin(rads)
  y = radius * Math.cos(rads)
  el.setAttribute("position", x + " " + y + " 0")

  // Now that element is placed, find appropraite rotation so that it faces the user
  angleX = Math.asin(x/distance) * DegPerRad * -1;
  angleY = Math.asin(y/distance) * DegPerRad;
  el.setAttribute("rotation", angleY + " " + angleX + " 0")
}

function goToVR() {
    document.getElementById("menu").classList.add("hidden");
    document.getElementById("scene").enterVR();
    document.getElementById("scene").enterVR();
}

function onVrClosed() {
    document.getElementById("menu").classList.remove("hidden");
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

function onToggleGaze(sender, params) {
    toggleGaze();
}
