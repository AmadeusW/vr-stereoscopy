var allLinks = [];

function initialize() {
    document.getElementById('toggle_all').onchange = toggleAll;
    //loadImage("http://orig00.deviantart.net/8afe/f/2012/092/1/4/parish_church_st__georg_3d_____cross_eye_hdr_by_zour-d4upy31.jpg");
}

// Display all visible links.
function showLinks() {
  var linksTable = document.getElementById('links');
  while (linksTable.children.length > 1) {
    linksTable.removeChild(linksTable.children[linksTable.children.length - 1])
  }
  for (var i = 0; i < allLinks.length; ++i) {
    var row = document.createElement('div');
    row.className = 'link';
    
    var checkbox = document.createElement('input');
    checkbox.checked = true;
    checkbox.type = 'checkbox';
    checkbox.id = 'check' + i;
    
    var label = document.createElement('span');
    label.innerText = allLinks[i];
    
    row.appendChild(checkbox);
    row.appendChild(label)
    row.onclick = loadImage(allLinks[i]);
    //label.onclick = loadImage(allLinks[i]);
    linksTable.appendChild(row);
  }
}


// Toggle the checked state of all visible links.
function toggleAll() {
  var checked = document.getElementById('toggle_all').checked;
  for (var i = 0; i < allLinks.length; ++i) {
    document.getElementById('check' + i).checked = checked;
  }
}

function loadImage(url) {
  return function() {
    document.getElementById('image').src = url;
  }
}