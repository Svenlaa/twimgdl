const APP_URL = "http://localhost:5066";
let downloads = [];
let isOnline = false;

function clear() {
  downloads = [];
}

async function getStatus() {
  isOnline = !!(await fetch(`${APP_URL}/status`, {
    method: "GET",
  }));
}
getStatus();

function DOMRegex(regex) {
  let output = [];
  for (let i of document.querySelectorAll("img")) {
    if (regex.test(i.src)) output.push(i);
  }
  return output;
}

async function doThing() {
  if (!isOnline) return;
  const elements = DOMRegex(/https:\/\/pbs.twimg.com\/media\//);

  for (const element of elements) {
    const imageId = element.src.split("/")[4].split("?")[0];

    const parent = element.parentElement;
    if (parent.classList.contains("download")) continue;

    const button = document.createElement("button");
    button.innerText = "💾";
    button.addEventListener("click", (e) => {
      e.preventDefault();
      if (downloads.includes(imageId)) return;
      downloads.push(imageId);
      fetch(`${APP_URL}/image/${imageId}`, { method: "GET" });
    });
    parent.appendChild(button);
    parent.classList.add("download");
  }
}

setInterval(doThing, 500);
setInterval(clear, 5000);
