function getFirstSelectedId(selectElementId) {
    var lista = document.getElementById(selectElementId);
    return lista.options[lista.selectedIndex].value;
}

function replaceNameValueOptionsList(selectElementId, nameValueList) {
    var output = document.getElementById(selectElementId);
    output.innerHTML = "";

    for (var i = 0; i < nameValueList.length; i++) {
        var nameValue = nameValueList[i];
        var op = document.createElement("option");
        op.setAttribute("value", nameValue.Value);
        op.appendChild(document.createTextNode(nameValue.Name));
        output.appendChild(op);
    }
}

function resetOptionsWithMessage(selectElementId, message, disabled) {
    var output = document.getElementById(selectElementId);
    output.innerHTML = "";

    var op = document.createElement("option");
    op.setAttribute("value", -1);
    if (disabled) {
        op.setAttribute("disabled", disabled);
    }
    op.appendChild(document.createTextNode(message));
    output.appendChild(op);
}

