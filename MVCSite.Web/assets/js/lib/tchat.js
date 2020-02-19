
function Tchat(container, guide, tourist) {
    this.container = container;
    this.messages = [];
    this.guide = guide;
    this.tourist = tourist;

    this.configuration = {
        formatDate: "MM/DD/YYYY"
    }
}

Tchat.prototype.addMessage = function (message) {

    if (message.content == "") {
        return;
    }

    var dateFragment = this.buildDateFragment(message);
    var messageFragment = this.buildMessageFragment(message);

    this.container.appendChild(dateFragment);
    this.container.appendChild(messageFragment);
    this.messages.unshift(message);

    //Scrolldown
    this.container.scrollTop = this.container.scrollHeight;
}

Tchat.prototype.buildDateFragment = function (message) {

    var fragment = document.createDocumentFragment();
    var messageOnSameDay = (this.messages.length > 0 && moment(this.messages[0].date).format(this.configuration.formatDate) == moment(message.date).format(this.configuration.formatDate)) ? true : false;

    if (!messageOnSameDay) {
        
        var div = document.createElement("div");
        var span = document.createElement("span");
        span.className = "date";
        span.innerText = moment(message.date).format(this.configuration.formatDate);

        div.appendChild(span);
        fragment.appendChild(div);
    }
    
    return fragment;
}

Tchat.prototype.buildMessageFragment = function (message) {

    var fragment = document.createDocumentFragment();

    var div = document.createElement("div");

    var messageWrapper = document.createElement("div");
    messageWrapper.className = "message-wrapper";

    ////Remove previous thumbnail
    //if(this.messages.length == 0 || this.messages[0].from != message.from) {
        
    //    var lastMessageElement = this.container.lastChild.previousSibling;

    //    if (lastMessageElement.tagName == "DIV") {

    //        if (lastMessageElement.getElementsByClassName("message-user").length > 0) {

    //            var previousMessageUser = lastMessageElement.getElementsByClassName("message-user")[0];
    //            lastMessageElement.firstChild.removeChild(previousMessageUser);
    //        }
    //    }
    //}

    //Add thumbnail
    if (message.from == this.guide) {

        var messageUser = document.createElement("div");
        messageUser.className = "message-user";

        var img = document.createElement("img");
        img.alt = "me";
        img.className = "user-extra-small-thumbnail img-circle";
        img.src = this.guide.thumbnail;

        messageUser.appendChild(img);
        messageWrapper.appendChild(messageUser);
    }

    var messageContent = document.createElement("div");
    messageContent.className = (message.from == this.guide) ? "message-content drt-left" : "message-content drt-right";
    var span = document.createElement("span");
    span.innerText = message.content;

    messageContent.appendChild(span);
    messageWrapper.appendChild(messageContent);

    div.appendChild(messageWrapper);

    return div;
}
