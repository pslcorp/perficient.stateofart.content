//Ejecutar función en el evento click
document.getElementById("btn_open").addEventListener("click", open_close_menu);

// replace xxxx with the number port
var localServicePath = 'https://localhost:7250/textanalytics/demoFront?id='

//change this variables to show information of the tweet 
var tweet_options = { theme: "dark", width: "auto", conversation: "none", cards: "all" };

//Declaramos variables
var side_menu = document.getElementById("menu_side");
var btn_open = document.getElementById("btn_open");
var body = document.getElementById("body");
var btnExecute = document.getElementById("btn-execute");
var idTwitter = document.getElementById("id-twitter");
var tweetContainer = document.getElementById("tweet-container");
var tweetJsonResults = document.getElementsByClassName("twitter-result");
var preTweetCode = document.getElementsByTagName("pre")[0];
var entitiesTable = document.getElementById("entities-table");
var keyFrasesTable = document.getElementById("keysFrases-table");
var languageTable = document.getElementById("language-table");


//Evento para mostrar y ocultar menú
function open_close_menu() {
    body.classList.toggle("body_move");
    side_menu.classList.toggle("menu__side_move");
}

//Si el ancho de la página es menor a 760px, ocultará el menú al recargar la página

if (window.innerWidth < 760) {

    body.classList.add("body_move");
    side_menu.classList.add("menu__side_move");
}

//Haciendo el menú responsive(adaptable)

window.addEventListener("resize", function() {

    if (window.innerWidth > 760) {

        body.classList.remove("body_move");
        side_menu.classList.remove("menu__side_move");
    }

    if (window.innerWidth < 760) {

        body.classList.add("body_move");
        side_menu.classList.add("menu__side_move");
    }

});

window.onload = () => {
    const data_switchers = document.querySelectorAll('[data-switcher]');

    for (let i = 0; i < data_switchers.length; i++) {
        const data_switcher = data_switchers[i];
        const page_id = data_switcher.dataset.item;

        data_switcher.addEventListener('click', () => {
            document.querySelector('.options__menu .selected').classList.remove('selected');
            data_switcher.classList.add('selected');

            SwitchPage(page_id);
        });
    }
    preTweetCode.parentElement.parentElement.classList.add("hidden");

}

document.addEventListener("DOMContentLoaded", function(event) {

    twttr.widgets.load(
        document.getElementById("tweet-container")
    );
});

function SwitchPage(page_id) {
    console.log(page_id);

    const current_page = document.querySelector('.pages .page.is-active');
    current_page.classList.remove('is-active');

    const next_page = document.querySelector(`.pages .page[data-page="${page_id}"]`);
    next_page.classList.add('is-active');
}


btnExecute.addEventListener("click", () => {
    var id = idTwitter.value;
    if (id == '' || id == undefined || id < 1)
        return;
    tweetContainer.innerText = "";
    preTweetCode.parentElement.parentElement.classList.add("hidden");
    processRequest(id);
});



function processRequest(id) {
    var result = "";
    var request = new XMLHttpRequest();
    request.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            result = JSON.parse(this.responseText);
            if (result != undefined && result != null) {
                bindTweetData(result.tweet);
                bindEntities(result.entities);
                bindLanguage(result.language);
                bindKeyFrases(result.keyFrasess.value);
                bindSentiments(result.sentiment);
            }
        }
    };

    request.open('GET', localServicePath + id);
    request.send();
}


function bindTweetData(tweet) {
    twttr.widgets.createTweet(tweet.id, tweetContainer, tweet_options);
    preTweetCode.parentElement.parentElement.classList.remove("hidden");
    preTweetCode.children[0].innerHTML = JSON.stringify(tweet, undefined, 4);
    Prism.highlightAll(preTweetCode);
}

function bindEntities(entities) {
    deleteRows(entitiesTable);
    entities.map(e => {
        var row = entitiesTable.insertRow(entitiesTable.rows.length);
        row.insertCell(0).innerHTML = e.text;
        row.insertCell(1).innerHTML = e.category;
        row.insertCell(2).innerHTML = e.confidenceScore;
        row.insertCell(3).innerHTML = e.offset;
        row.insertCell(4).innerHTML = e.length;
    });
}

function deleteRows(table) {
    var rowCount = table.rows.length;
    while (rowCount > '1') {
        table.deleteRow(rowCount - 1);
        rowCount--;
    };
}


function bindLanguage(language) {
    deleteRows(languageTable);
    var row = languageTable.insertRow(languageTable.rows.length);
    row.insertCell(0).innerHTML = language.name;
    row.insertCell(1).innerHTML = language.iso6391Name;
    row.insertCell(2).innerHTML = language.confidenceScore;
}

function bindKeyFrases(keyFrasess) {
    deleteRows(keyFrasesTable);
    var i = 1;
    keyFrasess.map(e => {
        var row = keyFrasesTable.insertRow(keyFrasesTable.rows.length);
        row.insertCell(0).innerHTML = i;
        row.insertCell(1).innerHTML = e;
        i++;
    });
}

function bindSentiments(sentiments) {
    var i = 0;
    // get de page context
    var context = document.querySelector(`.pages .page[data-page="4"]`);
    var existedDivs = context.querySelectorAll('div.content');
    if (existedDivs.length > 0) {
        for (let d = 0; d < existedDivs.length; d++) {
            existedDivs[d].remove();
        }
    }

    sentiments.map(e => {
        // create dynamically the div column 
        var divColumn = document.createElement("div");
        divColumn.id = "col-1." + i;
        divColumn.classList.add("column");

        var divColumn2 = document.createElement("div");
        divColumn2.id = "col-2." + i;
        divColumn2.classList.add("column");

        // create dynamically the div row 
        var divRow = document.createElement("div");
        divRow.id = "row-1." + i;
        divRow.classList.add("row");
        divRow.appendChild(divColumn);

        // create dynamically the div row 
        var divRow2 = document.createElement("div");
        divRow2.id = "row-2." + i;
        divRow2.classList.add("row");
        divRow2.appendChild(divColumn2);

        // create dynamically the div container
        var divContainer = document.createElement("div");
        divContainer.id = "container-1." + i;
        divContainer.classList.add("content");
        divContainer.classList.add("pt30");
        divContainer.appendChild(divRow);

        // create dynamically the div container2
        var divContainer2 = document.createElement("div");
        divContainer2.id = "container-2." + i;
        divContainer2.classList.add("content");
        divContainer2.classList.add("pt30");
        divContainer2.appendChild(divRow2);

        // create dynamically the table sentence 
        var tableSentence = document.createElement("table")
        tableSentence.id = "sentence-table-" + i;
        var tsRow = tableSentence.insertRow(tableSentence.rows.length);
        tsRow.insertCell(0).innerHTML = "General sentiment";
        tsRow.insertCell(1).innerHTML = "Positive";
        tsRow.insertCell(2).innerHTML = "Neutral";
        tsRow.insertCell(3).innerHTML = "Negative";
        var tsRow2 = tableSentence.insertRow(tableSentence.rows.length);
        tsRow2.insertCell(0).innerHTML = sentenceSwitch(e.documentSentiment.sentiment);
        tsRow2.insertCell(1).innerHTML = e.documentSentiment.confidenceScores.positive;
        tsRow2.insertCell(2).innerHTML = e.documentSentiment.confidenceScores.neutral;
        tsRow2.insertCell(3).innerHTML = e.documentSentiment.confidenceScores.negative;

        // create dynamically the table details 
        var tableDetails = document.createElement("table");
        tableDetails.id = "details-table-" + i;
        var tdRow = tableDetails.insertRow(tableDetails.rows.length);
        tdRow.insertCell(0).innerHTML = "Sentiment";
        tdRow.insertCell(1).innerHTML = "Text";
        tdRow.insertCell(2).innerHTML = "Positive";
        tdRow.insertCell(3).innerHTML = "Neutral";
        tdRow.insertCell(4).innerHTML = "Negative";
        e.documentSentiment.sentences.map(s => {
            var row = tableDetails.insertRow(tableDetails.rows.length);
            row.insertCell(0).innerHTML = sentenceSwitch(s.sentiment);
            row.insertCell(1).innerHTML = s.text;
            row.insertCell(2).innerHTML = s.confidenceScores.positive;
            row.insertCell(3).innerHTML = s.confidenceScores.neutral;
            row.insertCell(4).innerHTML = s.confidenceScores.negative;
        });

        divContainer.children[0].children[0].appendChild(tableSentence);
        divContainer2.children[0].children[0].appendChild(tableDetails);

        context.appendChild(divContainer);
        context.appendChild(divContainer2);

        i++;

    });





}


function sentenceSwitch(id) {
    switch (id) {
        case 0:
            return "Positive";
        case 1:
            return "Neutral";
        case 2:
            return "Negative";
        case 3:
            return "Mixed";
        default:
            return "Mixed";
    }

}