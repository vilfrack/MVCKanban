//CUANDO CERREMOS EL MODAL
$("button[data-dismiss='modal']").click(function () {
    $('#alert_danger_asignar').hide();
    $('#alert_success_asignar').hide();
    $("#btnEnviarEditar").prop('disabled', false);
    esconderMensajes();
});
//SE SACAN DEL REEADY PORQUE LUEGO SE EJECUTAN DOS VECES
$("#formAsignar").submit(function (e) {
    e.preventDefault();
    var parametros = new FormData($(this)[0]);
    $.ajax({
        type: "POST",
        url: "/Asignar/asignar",
        cache: false,
        contentType: false, //importante enviar este parametro en false
        processData: false, //importante enviar este parametro en false
        data: parametros,
        dataType: "json",
        success: function (data) {
            if (data.success) {
                $('#alert_success_asignar').show("fast");
                $('#alert_danger_asignar').hide();
                LoadGrid();
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
            $('#alert_danger').html(data);
            $('#alert_danger').show("fast");
        },

    });
});
//CUANDO SE DE CLICK A EDITAR DESDE LA TABLA
$("#tableRequerimiento").on('click', 'tr #asignar', function () {
    var id = $(this).parents("tr").find("td").eq(0).html();
    var url = "/Asignar/asignar?id=" + id + ""; // Establecer URL de la acción
    $("#btnEnviar").prop('disabled', false);
    $("#contenedor-asignar").load(url);
});
/*
  <th>ID</th>
            <th>Titulo</th>
            <th>Descripcion</th>
            <th>Solicitante</th>
            <th>Fecha de solicitud</th>
 */
function LoadGrid() {
    $('#tableRequerimiento').dataTable({
        destroy: true,//PERMITE DESTRUIR LA TABLA PARA VOLVERLA A CREAR
        bProcessing: true,
        sAjaxSource: '/Asignar/get',
        "columns": [
          { "data": "RequerimientoID" },
          { "data": "Titulo" },
          { "data": "Descripcion" },
          { "data": "Solicitante" },
          { "data": "Asignado" },
          {
              "data": "Fecha",
              "render": function (jsonDate) {
                  var date = new Date(parseInt(jsonDate.substr(6)));
                  var month = date.getMonth() + 1;
                  return date.getDate() + "/" + month + "/" + date.getFullYear();
                  //https://www.youtube.com/watch?v=TgD24a9gxXw   explicacion de como cambiar el formato fecha
              }
          },
          {
              "data": null,
              defaultContent: "<button id='asignar' class='btn btn-primary btn-sm'" +
                              "data-toggle='modal' data-target='#modalAsignar' ><span class='glyphicon glyphicon-plus-sign'></span> Asignar </button>&nbsp;&nbsp;"
          }
        ]
    })
}
function esconderMensajes() {
    $('#alert_danger_asignar').hide();
    $('#alert_success_asignar').hide();
}
