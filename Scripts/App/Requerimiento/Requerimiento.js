

$("#formCreate").submit(function (e) {
    e.preventDefault();
    var parametros = new FormData($(this)[0]);
    //CREO EL AJAX
    $.ajax({
        type: "POST",
        url: "/Requerimiento/Create",
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
                    $("#Error_" + key).html('');
                    $("#div_" + key).removeClass(" has-error has-feedback");
                });
                $('#alert_success').show("fast");
                $("#btnEnviar").prop('disabled', true);
                $('#alert_danger').hide();
                LoadGrid();
            }
            else {
                if (data.cantidad == true) {
                    $('#alert_danger').html('No se puede agregar mas de 3 archivos');
                    $('#alert_danger').show("fast");
                } else {
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

            }

        },
        error: function (data) {
            $('#alert_danger').html('error');
            $('#alert_danger').show("fast");
        },

    });
});
$("button[data-dismiss='modal']").click(function () {
    $('#Titulo').val('');
    $('#Descripcion').val('');
    $('#file').val('');
    $(".fileinput-remove-button").click();
    $("#btnEnviar").prop('disabled', false);
    $("#btnEliminarTask").prop('disabled', false);
    esconderMensajes();
});
//CARGA Y LLAMA EL FORMULARIO PARA EDITAR
$("#tableRequerimiento").on('click', 'tr #editar', function () {
    var idTask = $(this).parents("tr").find("td").eq(0).html();
    var url = "/Requerimiento/Edit?id=" + idTask + ""; // Establecer URL de la acción
    $("#btnEnviarEditar").prop('disabled', false);
    $("#contenedor-editar").load(url);

});
//LLAMAR DETALLE
$("#tableRequerimiento").on('click', 'tr #detalle', function () {
    var idTask = $(this).parents("tr").find("td").eq(0).html();
    getID(idTask);

});
//AL HACER CLICK EN EL BOTON ELIMINAR QUE ESTA EN LA TABLA, ESTE OBTIENE EL ID Y SE LO ENVIA AL HIDDEN QUE ESTA EN EL MODAL DE ELIMINAR
$("#tableRequerimiento").on('click', 'tr #eliminar', function () {
    var idTask = $(this).parents("tr").find("td").eq(0).html();
    $('.mensaje').html('Seguro desea eliminar el registro seleccionados?');
    document.getElementById("hdId").value = idTask;
});
$("#btnEliminarTask").click(function () {
    var params = {
        id: $('#hdId').val()
    };
    $.ajax({
        type: "POST",
        url: "/Requerimiento/Delete",
        data: params,
        dataType: "json",
        success: function (data) {
            $('.mensaje').html('');
            $('#alert_success_eliminar').html('Registro eliminado');
            $('#alert_success_eliminar').show("fast");
            $("#btnEliminarTask").prop('disabled', true);
            LoadGrid();
        },
        error: function (data) {

        },

    });
});


$("#formEdit").submit(function (e) {
    e.preventDefault();
    var parametros = new FormData($(this)[0]);
    $.ajax({
        type: "POST",
        url: "/Requerimiento/Edit",
        cache: false,
        contentType: false, //importante enviar este parametro en false
        processData: false, //importante enviar este parametro en false
        //CONVIERTO EL OBJETO EN FORMATO JSON
        // data: JSON.stringify(list),
        data: parametros,

        dataType: "json",
        success: function (data) {
            if (data.success) {
                $('#alert_success_edit').show("fast");
                $("#btnEnviarEditar").prop('disabled', true);
                $('#alert_danger_edit').hide();

                LoadGrid();

            }
            else {
                if (data.cantidad == true) {
                    $('#alert_danger_edit').html('No se puede agregar mas de 3 archivos');
                    $('#alert_danger_edit').show("fast");
                } else {
                    //VA A CAPTURAR TODOS LOS ERRORES ENVIADOS DEL CONTROLADOR
                    $.each(data.Errors, function (key, value) {
                        //VALUE VA A TRAER SOLO AQUELLOS VALORES QUE NO CUMPLAN CON LOS REQUISITOS ESTABLECIDOS EN EL MODELSTATE
                        if (value != null) {
                            $("#Error_" + key).html(value[value.length - 1].ErrorMessage);
                            $("#div_" + key).addClass(" has-error has-feedback");
                        }
                    });
                }
            }
        },
        error: function (data) {
            $('#alert_danger_editar').html('error');
            $('#alert_danger_editar').show("fast");
        },

    });

});

//AL HACER CLICK EN AGREGAR NOS MOSTRARA EL MODAL CON EL FORMULARIO
$("#agregar").click(function () {
    var url = "/Requerimiento/Create"; // Establecer URL de la acción
    $("#contenedor-agregar").load(url);
});

//FUNCIONES
function LoadGrid() {
    $('#tableRequerimiento').dataTable({
        destroy: true,//PERMITE DESTRUIR LA TABLA PARA VOLVERLA A CREAR
        bProcessing: true,
        sAjaxSource: '/Requerimiento/get',
        "columns": [
          { "data": "RequerimientoID" },
          { "data": "Titulo" },
          { "data": "Status" },
          { "data": "Departamento" },
          {
              "data": "FechaCreacion",
              "render": function (jsonDate) {
                  var date = new Date(parseInt(jsonDate.substr(6)));
                  var month = date.getMonth() + 1;
                  return date.getDate() + "/" + month + "/" + date.getFullYear();
                  //https://www.youtube.com/watch?v=TgD24a9gxXw   explicacion de como cambiar el formato fecha
              }
          },

          {
              "data": null,
              defaultContent: "<button id='editar' class='btn btn-success btn-sm'" +
                              "data-toggle='modal' data-target='#editModal' ><span class='glyphicon glyphicon-retweet'></span> Editar </button>&nbsp;&nbsp;" +
                                //boton de eliminar
                              "<button id='eliminar' class='btn btn-danger btn-sm'" +
                              "data-toggle='modal' data-target='#borrarModal'><span class='glyphicon glyphicon-trash'></span> Eliminar </button>&nbsp;&nbsp;" //+
                                //boton para ver los casos SE COMENTA HASTA TENER EL KANBAN LISTO
                              //"<button id='detalle' class='btn btn-default btn-sm'" +
                              //"data-toggle='modal' data-target='#detalleModal' ><span class='glyphicon glyphicon-retweet'></span> Detalle </button>&nbsp;&nbsp;"
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
function getID(RequerimientoID) {
    var url = "/Kanban/detail?sid=" + RequerimientoID + ""; // Establecer URL de la acción
    $("#contenedor-modal").load(url);
    $('#detalleModal').modal('show');
}