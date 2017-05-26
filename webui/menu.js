function showMenu() {
    var menu = document.querySelector("#menuPane");

    document.querySelector("#menuTitle").setAttribute("value", "Welcome");
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
        menu.appendChild(category);
    }
    menu.setAttribute("visible", true);
}