// Copyright (c) 2012 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

// This extension demonstrates using chrome.downloads.download() to
// download URLs.

var allLinks = [];
var visibleLinks = [];

// Display all visible links.
function showLinks() {
  var linksTable = document.getElementById('links');
  while (linksTable.children.length > 1) {
    linksTable.removeChild(linksTable.children[linksTable.children.length - 1])
  }
  for (var i = 0; i < visibleLinks.length; ++i) {
    var row = document.createElement('div');
    row.className = 'link';
    
    var checkbox = document.createElement('input');
    checkbox.checked = true;
    checkbox.type = 'checkbox';
    checkbox.id = 'check' + i;
    
    var label = document.createElement('span');
    label.innerText = visibleLinks[i];
    
    row.appendChild(checkbox);
    row.appendChild(label)
    row.onclick = function() {
      checkbox.checked = !checkbox.checked;
    }
    linksTable.appendChild(row);
  }
}

// Toggle the checked state of all visible links.
function toggleAll() {
  var checked = document.getElementById('toggle_all').checked;
  for (var i = 0; i < visibleLinks.length; ++i) {
    document.getElementById('check' + i).checked = checked;
  }
}

// Download all visible checked links.
function downloadCheckedLinks() {
  for (var i = 0; i < visibleLinks.length; ++i) {
    if (document.getElementById('check' + i).checked) {
      chrome.downloads.download({url: visibleLinks[i]},
                                             function(id) {
      });
    }
  }
  window.close();
}

// Re-filter allLinks into visibleLinks and reshow visibleLinks.
function filterLinks() {
  var filterValue = document.getElementById('filter').value;
  if (document.getElementById('regex').checked) {
    visibleLinks = allLinks.filter(function(link) {
      return link.match(filterValue);
    });
  } else {
    var terms = filterValue.split(' ');
    visibleLinks = allLinks.filter(function(link) {
      for (var termI = 0; termI < terms.length; ++termI) {
        var term = terms[termI];
        if (term.length != 0) {
          var expected = (term[0] != '-');
          if (!expected) {
            term = term.substr(1);
            if (term.length == 0) {
              continue;
            }
          }
          var found = (-1 !== link.indexOf(term));
          if (found != expected) {
            return false;
          }
        }
      }
      return true;
    });
  }
  showLinks();
}

// Add links to allLinks and visibleLinks, sort and show them.  send_links.js is
// injected into all frames of the active tab, so this listener may be called
// multiple times.
chrome.extension.onRequest.addListener(function(links) {
  for (var index in links) {
    allLinks.push(links[index]);
  }
  allLinks.sort();
  visibleLinks = allLinks;
  showLinks();
});

// Set up event handlers and inject send_links.js into all frames in the active
// tab.
window.onload = function() {
  chrome.windows.getCurrent(function (currentWindow) {
    chrome.tabs.query({active: true, windowId: currentWindow.id},
                      function(activeTabs) {
      chrome.tabs.executeScript(
        activeTabs[0].id, {file: 'send_links.js', allFrames: true});
    });
  });
};
