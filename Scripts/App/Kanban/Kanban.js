$(function () {
    // $( ".column" ).sortable({
    //   connectWith: ".column",
    //   handle: ".portlet-content",
    //   cancel: ".portlet-toggle",
    //   placeholder: "portlet-placeholder ui-corner-all"
    // });

    // $( ".portlet" )
    //   .addClass( "ui-widget ui-widget-content ui-helper-clearfix ui-corner-all" )
    //   .find( ".portlet-content" )
    //     .addClass( "ui-widget-header ui-corner-all" )
    //     .prepend( "<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

    // //se cambia el cursor
    //  $('.portlet').hover( function() {
    //   // the selector should match your link
    //      $(this).css('cursor', 'pointer');
    //},
    //function() {
    $(this).css('cursor', 'default');
    //});

    //CONTAR LA CANTIDAD DE DIV
    //para asignar un tamanio dinamico
    var contar = $(".column").length;

    contar = contar * 100;
    $("#contenido").css("width", contar + "%");


    //CAMBIAR EL COLOR DE LOS TASK POR DIV
    //CAMBIAMOS LOS DIV AL COLOR CORRESPONDIENTE DEL PADRE
    //del div padre buscamos todos los div con la clase panel
    $("#divAsignados div.panel").removeClass().addClass('panel panel-primary');
    $("#divDesarrollo div.panel").removeClass().addClass('panel panel-info');
    $("#divRealizados div.panel").removeClass().addClass('panel panel-success');
    $("#divRechazados div.panel").removeClass().addClass('panel panel-danger');

    //CAMBIAR EL COLOR DE LOS TASK DEL DIV AL CAMBIAR DE ESTATUS
    $("#divAsignados").mouseover(function () {
        $("#divAsignados div.panel").removeClass().addClass('panel panel-primary');
    });
    $("#divDesarrollo").mouseover(function () {
        $("#divDesarrollo div.panel").removeClass().addClass('panel panel-info');
    });
    $("#divRealizados").mouseover(function () {
        $("#divRealizados div.panel").removeClass().addClass('panel panel-success');
    });
    $("#divRechazados").mouseover(function () {
        $("#divRechazados div.panel").removeClass().addClass('panel panel-danger');
    });
    /*CODIGO PARA SALVAR EN LA BASE DE DATOS*/
    $('#divRealizados .column').sortable({
        receive: function (event, ui) {
            var id = ui.item.attr("id");
            var status = 'Realizados';
            operacion(id, status);

        }

    });
    $('#divDesarrollo .column').sortable({
        receive: function (event, ui) {
            var id = ui.item.attr("id");
            var status = 'Desarrollo';
            operacion(id, status);
        }
    });
    $('#divRechazados .column').sortable({
        receive: function (event, ui) {
            var id = ui.item.attr("id");
            var status = 'Rechazados';
            operacion(id, status);
        }
    });
    $('#divAsignados .column').sortable({
        receive: function (event, ui) {
            var id = ui.item.attr("id");
            var status = 'Asignado';
            operacion(id, status);
        }
    });
    //funcion que actualiza el estado
    function operacion(id, status) {
        $.ajax({
            type: "POST",
            url: "/Kanban/editStatus",
            cache: false,
            data: {
                id: id,
                status: status
            },
            dataType: "json",
            success: function (data) {
                if (data.success) {
                    // alert('success');
                }
                else {
                    alert('e');
                }
            },
            error: function (data) {
                $('#alert_danger').html('error');
                $('#alert_danger').show("fast");
            },

        });
    }

});
function getID(TaskID) {
    var url = "/Kanban/detail?sid=" + TaskID + ""; // Establecer URL de la acción
    $("#contenedor-modal").load(url);
    $('#modal').modal('show');
}