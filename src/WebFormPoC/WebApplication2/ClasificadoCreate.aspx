<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClasificadoCreate.aspx.cs" Inherits="WebFormPoC.ClasificadoCreate" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select marca-modelo-version</title>
    <script language="javascript" type="text/javascript" src="Scripts/jquery-1.3.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/jquery.form.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/AjaxExtension.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/nameValueOptions.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/ClasificadoCreateView.js"></script>
</head>

<body>
    <form id="clasificado" action="" method="post">
    <div id="clasificado1">
        <div id="marcasDetails">
            <p>
                Marcas</p>
            <select name="MarcaId" multiple="multiple" id="listamarcas" size="5" onchange="return marca_onchange()">
                <asp:Repeater runat="server" ID="rptMarcas">
                    <HeaderTemplate/>
                    <FooterTemplate/>
                    <ItemTemplate>
                        <option value='<%#Eval("Value")%>'><%#Eval("Name")%></option>
                    </ItemTemplate>
                </asp:Repeater>
            </select>
        </div>
        <div id="modelosDetails">
            <p>
                Modelos</p>
            <select name="ModeloId" multiple="multiple" id="listamodelos" size="5" onchange="return modelo_onchange()">
                <option>Debe selecionar una marca</option>
            </select>
        </div>
        <div id="versionesDetails">
            <p>
                Versiones</p>
            <select name="VersionId" multiple="multiple" id="listaversiones" size="5" onchange="return version_onchange()">
                <option>Debe selecionar un modelo</option>
            </select>
        </div>
        <div id="equipamientos">
            <h4>
                Equipamiento</h4>
            <div id="equipamientoDetails">
            </div>
        </div>
        <input type="button" value="Siguiente" onclick="return siguiente_onclick()" />
    </div>
    <div id="clasificado2">
        e-mail:<input type="text" id="email" name="Usuario.Email" onchange="return usuario_onchange()" /><br />
        Calle :<input type="text" id="calle" name="Usuario.Calle" /><br />
        <input type="button" value="Anterior" onclick="return anterior_onclick()" />
        <input type="submit" value="Publica"/>
   </div>
    </form>
    <div id="errors">
    </div>
</body>
</html>
