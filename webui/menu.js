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

    var template = document.querySelector("#category1");
    var category = template.cloneNode(/*deep:*/true);
    category.setAttribute("id", "category2");
    menu.appendChild(category);
    var title = category.querySelector(".title");
    title.setAttribute("value", "Second category");
}