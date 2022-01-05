//document.querySelector(".googleSearch").addEventListener("click", googleSearch);
document.querySelector("#googleSearch").addEventListener("click", googleSearch);
function googleSearch() {
  var keyWord = document.getElementById("searchWord").value;
  const titlelink1 = document.querySelector("#titlelink1");
  const titlelink2 = document.querySelector("#titlelink2");
  const titlelink3 = document.querySelector("#titlelink3");
  //var keyWord = document.querySelector(".searchWord").value;
  //var keyWord = "asus";
  console.log(keyWord);
  //Google search API
  const googleurl = `https://google-search3.p.rapidapi.com/api/v1/search/q=${keyWord}`;
  fetch(googleurl, {
    method: "GET",
    headers: {
      "x-user-agent": "desktop",
      "x-proxy-location": "US",
      "x-rapidapi-host": "google-search3.p.rapidapi.com",
      "x-rapidapi-key": "3b18eaeafamsh446d7d16459bedcp1216f5jsn8f88c4df3a35",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      console.log(data);
      //console.log(data.results[0].description);
      //var description = document.getElementById("description").textContent;
      //document.getElementById("description").innerHTML = data.results[0].description;
      //document.getElementById("title").innerHTML = data.results[0].title;
      //document.getElementById("link").innerHTML = data.results[0].link;
      titlelink1.href = data.results[0].link;
      titlelink1.textContent = data.results[0].title;
      titlelink2.href = data.results[1].link;
      titlelink2.textContent = data.results[1].title;
      titlelink3.href = data.results[2].link;
      titlelink3.textContent = data.results[2].title;
    })
    .catch((err) => {
      console.error(err);
    });
}

//Coordinates finder API
var city = "";
document.querySelector("#find-me").addEventListener("click", geoFindMe);
function geoFindMe() {
  const status = document.querySelector("#status");
  const mapLink = document.querySelector("#map-link");
  const yourlocation = document.querySelector("#gps");
  mapLink.href = "";
  mapLink.textContent = "";
  yourlocation.textContent = "";

  function success(position) {
    const latitude = position.coords.latitude;
    const longitude = position.coords.longitude;
    //latitude = 54.77897039100206;
    //longitude = 24.63542203285122;

    status.textContent = "";
    mapLink.href = `https://www.openstreetmap.org/#map=18/${latitude}/${longitude}`;
    mapLink.textContent = `Latitude: ${latitude} °, Longitude: ${longitude} °`;
    var request = `https://wft-geo-db.p.rapidapi.com/v1/geo/locations/${latitude}+${longitude}/nearbyCities?radius=20&limit=2&minPopulation=2000&distanceUnit=KM`;
    //console.log(request);
    cityFinder(request);
  }

  function error() {
    status.textContent = "Unable to retrieve your location";
  }

  if (!navigator.geolocation) {
    status.textContent = "Geolocation is not supported by your browser";
  } else {
    status.textContent = "Locating…";
    navigator.geolocation.getCurrentPosition(success, error);
  }

  //City finder API
  function cityFinder(request) {
    fetch(request, {
      method: "GET",
      headers: {
        "x-rapidapi-host": "wft-geo-db.p.rapidapi.com",
        "x-rapidapi-key": "3b18eaeafamsh446d7d16459bedcp1216f5jsn8f88c4df3a35",
      },
    })
      .then((response) => response.json())
      .then((data) => {
        console.log(data);
        console.log(data.data[0].city);
        console.log(data.data[0].latitude);
        console.log(data.data[0].longitude);
        city = data.data[0].city;
        yourlocation.textContent = `Your current city is: ${city}`;
        //console.log(data.data[0].population);
      })
      .catch((error) => {
        error();
      });
  }
}

const form = document.querySelector(".top-banner form");
const input = document.querySelector(".top-banner input");
const msg = document.querySelector(".top-banner .msg");
const list = document.querySelector(".ajax-section .cities");

const apiKey = "593f569c313b704098e1f53ce0d4b627";
let i = 0;
form.addEventListener("submit", (e) => {
  e.preventDefault();

  let inputVal;
  if (i == 0) {
    inputVal = city;
    i++;
  } else inputVal = input.value;
  //check if there's already a city
  const listItems = list.querySelectorAll(".ajax-section .city");
  const listItemsArray = Array.from(listItems);

  if (listItemsArray.length > 0) {
    const filteredArray = listItemsArray.filter((el) => {
      let content = "";
      content = el.querySelector(".city-name span").textContent.toLowerCase();
      return content == inputVal.toLowerCase();
    });

    if (filteredArray.length > 0) {
      msg.textContent = `You already know the weather for ${
        filteredArray[0].querySelector(".city-name span").textContent
      }`;
      form.reset();
      input.focus();
      return;
    }
  }

  //Wheather API
  const url = `https://api.openweathermap.org/data/2.5/weather?q=${inputVal}&appid=${apiKey}&units=metric`;

  fetch(url)
    .then((response) => response.json())
    .then((data) => {
      const { main, name, sys, weather, wind } = data;
      console.log(data);
      const icon = `https://openweathermap.org/img/wn/${weather[0]["icon"]}@2x.png`;

      const li = document.createElement("li");
      li.classList.add("city");
      const markup = `
        <h2 class="city-name" data-name="${name},${sys.country}">
          <span>${name}</span>
          <sup>${sys.country}</sup>
        </h2>
        <div class="city-temp">Temperature ${Math.round(
          main.temp
        )}<sup>°C</sup></div>
        <div class="city-temp">Feels like ${Math.round(
          main.feels_like
        )}<sup>°C</sup></div>
        <div class="city-temp">Wind speed ${Math.round(
          wind.speed
        )}<sup> m/s</sup></div>
        <figure>
          <img class="city-icon" src="${icon}" alt="${
        weather[0]["description"]
      }">
          <figcaption>${weather[0]["description"]}</figcaption>
        </figure>
      `;
      li.innerHTML = markup;
      list.appendChild(li);
    })
    .catch(() => {
      msg.textContent = "Please search for a valid city";
    });

  msg.textContent = "";
  form.reset();
  input.focus();
});
