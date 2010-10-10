<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Publicado.aspx.cs" Inherits="WebFormPoC.Publicado" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <div class="lista">
        <h3>
            Listado de clasificados</h3>
        <asp:Repeater runat="server" id="rptClasificados">
            <ItemTemplate>
                <%# Container.DataItem %>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <a href="ClasificadoCreate.aspx">Publica Nuevo</a>

</body>
</html>
