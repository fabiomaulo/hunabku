$(window).load(function() {
    $("#clasificado2").hide();
    $("#errors").hide();
});
function marca_onchange() {
    var b = getFirstSelectedId("listamarcas");
    JsonFromServerAction("LoadModelos", { marca: b }, function(a) {
        replaceNameValueOptionsList("listamodelos", a)
    });
    resetOptionsWithMessage("listaversiones", "Debe selecionar un modelo", true);
    replaceCategoriasEquipamiento(null)
}
function modelo_onchange() {
    var b = getFirstSelectedId("listamodelos");
    JsonFromServerAction("LoadVersiones", { modelo: b }, function(a) {
        replaceNameValueOptionsList("listaversiones", a)
    });
    replaceCategoriasEquipamiento(null)
}
function version_onchange() {
    var b = getFirstSelectedId("listaversiones");
    JsonFromServerAction("LoadEquipamiento", { version: b }, function(a) {
        replaceCategoriasEquipamiento(a)
    })
}
function usuario_onchange() {
    JsonFromServerAction("LoadUsuario", { email: $("#email").attr("value") }, function(u) {
        $(document).ready(function() {
            $("#calle").val(u.Calle);
        });
    })
}
function replaceCategoriasEquipamiento(b) {
    var a = document.getElementById("equipamientoDetails");
    a.innerHTML = "";
    if (b != null) for (var c = 0; c < b.length; c++) {
        var d = b[c];
        a.appendChild(GetCategoriaEquipamientoTitulo(d));
        FillEquipamientos(a, d.Equipamiento)
    }
}
function GetCategoriaEquipamientoTitulo(b) {
    var a = document.createElement("p");
    a.appendChild(document.createTextNode(b.Categoria));
    return a
}
function FillEquipamientos(b, a) {
    for (var c = 0; c < a.length; c++) {
        var d = a[c], e = document.createElement("input");
        e.setAttribute("name", "Equipamiento");
        e.setAttribute("class", "equipamientoCheck");
        e.setAttribute("type", "checkbox");
        e.setAttribute("value", d.Id);
        d.Tiene && e.setAttribute("checked", d.Tiene);
        b.appendChild(e);
        b.appendChild(document.createTextNode(d.Nombre))
    }
}
function anterior_onclick() {
    $("#clasificado2").hide();
    $("#clasificado1").show()
}
function siguiente_onclick() {
    $("#clasificado1").hide();
    $("#clasificado2").show()
}
$(document).ready(function() {
    $("#clasificado").ajaxForm({ url: this.location.href + "?LookupAction=Publica", dataType: 'json', success: showResponse })
});

function showResponse(invalid, status) {
    if (invalid.length == 0) {
        window.location = "Publicado.aspx";
    }
    else {
        $("#errors").text("");
        jQuery.each(invalid, function() {
            $("#errors").append(document.createTextNode(this.Message))
            $("#errors").append("<br />") 
        });
        $("#errors").show();
    }
}