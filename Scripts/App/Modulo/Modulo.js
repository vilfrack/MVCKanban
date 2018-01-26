
//CUANDO CERREMOS EL MODAL
$("button[data-dismiss='modal']").click(function () {
    $('#Descripcion').val('');
    esconderMensajes();
});
//AL HACER CLICK EN EL BOTON ELIMINAR QUE ESTA EN LA TABLA, ESTE OBTIENE EL ID Y SE LO ENVIA AL HIDDEN QUE ESTA EN EL MODAL DE ELIMINAR
$("#tableModulos").on('click', 'tr #eliminar', function () {
    var id = $(this).parents("tr").find("td").eq(0).html();
    $('.mensaje').html('Seguro desea eliminar el registro seleccionados?');
    document.getElementById("hdId").value = id;
});
//ELIMINAR EL REGISTRO
$("#btnEliminar").click(function () {

    var params = {
        id: $('#hdId').val()
    };
    $.ajax({
        type: "POST",
        url: "/Modulo/Delete",
        data: params,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data.success == true) {

                $('.mensaje').html('');
                $('#alert_success_eliminar').html('Registro eliminado');
                $('#alert_success_eliminar').show("fast");
                $("#btnEliminar").prop('disabled', true);
                LoadGridModulos();
            }
            else {
                $('#alert_danger_eliminar').html(data.mensaje);
                $('#alert_danger_eliminar').show("fast");
            }
        },
        error: function (data) {

        },

    });
});
//SE SACAN DEL REEADY PORQUE LUEGO SE EJECUTAN DOS VECES
$("#formEdit").submit(function (e) {
    e.preventDefault();
    var parametros = new FormData($(this)[0]);
    $.ajax({
        type: "POST",
        url: "/Modulo/Edit",
        cache: false,
        contentType: false, //importante enviar este parametro en false
        processData: false, //importante enviar este parametro en false
        //CONVIERTO EL OBJETO EN FORMATO JSON
        // data: JSON.stringify(list),
        data: parametros,

        dataType: "json",
        success: function (data) {

            if (data.success) {
                //REESTABLECE LOS ESTILOS PREDEFINIDOS DE LOS LABEL ERRORES Y DIV, EN CASO DE QUE SE HAYAN MOSTRADO Y SE HAYAN CORREGIDO
                $.each(parametros, function (key, value) {
                    $("#ErrorEdit_" + key).html('');
                    $("#divEdit_" + key).removeClass(" has-error has-feedback");
                });
                $('#alert_success_edit').show("fast");
                $('#alert_success_edit').show("fast");
                $('#alert_danger_edit').hide();
                LoadGridModulos();

            }
            else {
                //VA A CAPTURAR TODOS LOS ERRORES ENVIADOS DEL CONTROLADOR
                $.each(data.Errors, function (key, value) {
                    //VALUE VA A TRAER SOLO AQUELLOS VALORES QUE NO CUMPLAN CON LOS REQUISITOS ESTABLECIDOS EN EL MODELSTATE
                    //CREAR EN LO POSIBLE UNA CLASE QUE GUARDE ESTE CODIGO
                    if (value != "true") {
                        $("#ErrorEdit_" + key).html(value[value.length - 1].ErrorMessage);
                        $("#divEdit_" + key).addClass(" has-error has-feedback");
                    } else {
                        $("#ErrorEdit_" + key).html('');
                        $("#divEdit_" + key).removeClass(" has-error has-feedback");
                    }

                });
            }
        },
        error: function (data) {
            $('#alert_danger_edit').html(data);
            $('#alert_danger_edit').show("fast");
        },

    });
});
$("#formCreate").submit(function (e) {
    e.preventDefault();
    //ruta la cual recibira nuestro archivo
    var parametros = new FormData($(this)[0]);
    //CREO EL AJAX

    $.ajax({
        type: "POST",
        url: "/Modulo/Create",
        cache: false,
        contentType: false, //importante enviar este parametro en false
        processData: false, //importante enviar este parametro en false
        //CONVIERTO EL OBJETO EN FORMATO JSON
        // data: JSON.stringify(list),
        data: parametros,

        dataType: "json",
        success: function (data) {
            if (data.success) {
                // REESTABLECE LOS ESTILOS PREDEFINIDOS DE LOS LABEL ERRORES Y DIV, EN CASO DE QUE SE HAYAN MOSTRADO Y SE HAYAN CORREGIDO
                $.each(parametros, function (key, value) {
                    $("#Error_" + key).html('');
                    $("#div_" + key).removeClass(" has-error has-feedback");
                });
                $('#alert_success').show("fast");
                $("#btnEnviar").prop('disabled', true);
                $('#alert_danger').hide();

                LoadGridModulos();
            }
            else {

                //VA A CAPTURAR TODOS LOS ERRORES ENVIADOS DEL CONTROLADOR
                $.each(data.Errors, function (key, value) {
                    //VALUE VA A TRAER SOLO AQUELLOS VALORES QUE NO CUMPLAN CON LOS REQUISITOS ESTABLECIDOS EN EL MODELSTATE
                    //CREAR EN LO POSIBLE UNA CLASE QUE GUARDE ESTE CODIGO
                    if (value != "true") {
                        $("#Error_" + key).html(value[value.length - 1].ErrorMessage);
                        $("#div_" + key).addClass(" has-error has-feedback");
                    } else {
                        $("#Error_" + key).html('');
                        $("#div_" + key).removeClass(" has-error has-feedback");
                    }

                });
            }

        },
        error: function (data) {
            $('#alert_danger').html('error');
            $('#alert_danger').show("fast");
        },

    });
});
//CUANDO SE DE CLICK A EDITAR DESDE LA TABLA
$("#tableModulos").on('click', 'tr #editar', function () {
    var id = $(this).parents("tr").find("td").eq(0).html();
    var url = "/Modulo/Edit?id=" + id + ""; // Establecer URL de la acción
    $("#btnEnviarEditar").prop('disabled', false);
    $("#contenedor-editar").load(url);

});
//AL HACER CLICK EN AGREGAR NOS MOSTRARA EL MODAL CON EL FORMULARIO
$("#agregar").click(function () {
    var url = "/Modulo/Create"; // Establecer URL de la acción
    $("#contenedor-agregar").load(url);
});
//FUNCIONES
function LoadGridModulos() {
    $('#tableModulos').dataTable({
        destroy: true,//PERMITE DESTRUIR LA TABLA PARA VOLVERLA A CREAR
        bProcessing: true,
        sAjaxSource: '/Modulo/get',
        "columns": [
          { "data": "ModuloID" },
          { "data": "Descripcion" },
          {
              "data": null,
              defaultContent: "<button id='editar' class='btn btn-success btn-sm'" +
                              "data-toggle='modal' data-target='#editModal' ><span class='glyphicon glyphicon-retweet'></span> Editar </button>&nbsp;&nbsp;" +
                                //boton de eliminar
                              "<button id='eliminar' class='btn btn-danger btn-sm'" +
                              "data-toggle='modal' data-target='#borrarModal'><span class='glyphicon glyphicon-trash'></span> Eliminar </button>"
          }
        ]
    })
}
function esconderMensajes() {

    $('#alert_danger').hide();
    $('#alert_success').hide();
    $('#alert_danger_edit').hide();
    $('#alert_success_edit').hide();
    $('#alert_success_eliminar').hide();
    $('#alert_danger_eliminar').hide();
}
