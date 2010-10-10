
function JsonFromServerAction(serverAction, params, onsuccess) {
    var parameters = { LookupAction: serverAction };
    $.getJSON(this.location.href, jQuery.extend(parameters, params), onsuccess);
}
