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
        galleryItems.push({
            title: categories[categoryId].DisplayName,
            description: categories[categoryId].DisplayName,
            picture: cdnPrefix + categories[categoryId].Thumbnail + ".T.L.jpg",
            id: categoryId
        });
    }

    mainTitleVue = new Vue({
        el: '#main-title',
        data: {
          visible: true
        }
    });

    galleryTitleVue = new Vue({
        el: '#gallery-title',
        data: {
          visible: true,
          item: {
            title: "Image title goes here",
            thumbLeftUrl: "https://vrcv.azureedge.net/vrcv/201705/69ov7p.T.L.jpg",
            originalUrl: "amadeusw.com"
          }
        }
    });

    mainMenuVue = new Vue({
        el: '#main-menu',
        data: {
            visible: true,
            items: galleryItems
        }
    });

    galleryMenuVue = new Vue({
        el: '#gallery-menu',
        data: {
            visible: true,
            items: []
        }
    });

    menuContainerVue = new Vue({
        el: '#menuContainer',
        data: {
            visible: true
        }
    });

    vrContainerVue = new Vue({
        el: '#scene',
        data: {
            visible: true
        }
    });
}

async function onGalleryItemClick(event) {
    var category = event.currentTarget.getAttribute("cid")

    await goToCategory(category);
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
    render();
    menuContainerVue.visible = false;
    vrContainerVue.visible = true;
    document.getElementById("scene").enterVR();
    showGalleryUI();
}

function onVrClosed() {
    menuContainerVue.visible = true;
    vrContainerVue.visible = false;
}

function showMainUI() {
    galleryTitleVue.visible = false;
    mainTitleVue.visible = true;
    galleryMenuVue.visible = false;
    mainMenuVue.visible = true;
    console.log("main");
}

function showGalleryUI() {
    galleryTitleVue.visible = true;
    mainTitleVue.visible = false;
    galleryMenuVue.visible = true;
    mainMenuVue.visible = false;
    console.log("gallery");
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

function onToggleTimer(sender, params) {
    toggleTimer();
}

function onToggleGaze(sender, params) {
    toggleGaze();
}
