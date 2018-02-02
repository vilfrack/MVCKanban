$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $("#File").prop('disabled', true);
})

$('.DivDatapiker input').datepicker({
    todayBtn: "linked",
    autoclose: true,
    todayHighlight: true
});
$("#formKanban").submit(function (e) {
    e.preventDefault();
    var parametros = new FormData($(this)[0]);
    //CREO EL AJAX
    $.ajax({
        type: "POST",
        url: "/Revision/Save",
        cache: false,
        contentType: false, //importante enviar este parametro en false
        processData: false, //importante enviar este parametro en false
        //CONVIERTO EL OBJETO EN FORMATO JSON
        data: parametros,
        dataType: "json",
        success: function (data) {
            if (data.success) {

                $("#Error_Comentario").html('');
                $("#div_Comentario").removeClass(" has-error has-feedback");

                $('#alert_success').show("fast");
                $('#alert_danger').hide();
                $("#btnEnviar").prop('disabled', true);
                LoadComment();
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
                if (data.status == 0) {
                    $('#alert_danger').html("Debe seleccionar un Status");
                    $('#alert_danger').show("fast");

                    $('#alert_success').hide();
                }
            }

        },
        error: function (data) {
            $('#alert_danger').html('error');
            $('#alert_danger').show("fast");
        },

    });
});

function LoadComment() {
    //CREO EL AJAX
    var params = {
        id: $('#TaskID').val()
    };
    $("#DivComentarios").empty();
    $.ajax({
        type: "POST",
        url: "/Revision/getComentario",
        data: params,
        dataType: "json",
        success: function (data) {
            $.each(data, function (key, value) {
                console.log(value.rutaImg);
                var date = new Date(parseInt(value.Fecha.substr(6)));
                var month = date.getMonth() + 1;
                var fecha = date.getDate() + "/" + month + "/" + date.getFullYear();
                var html = '<div class="bordes">' +
                            ' <img src="../' + value.rutaImg + '" class="img-circle FotoPerfil" />' +
                            ' <label>' + value.Nombre + '</label>' +
                            ' <label>' + value.Apellido + '</label>' +
                            ' <label>' + fecha + '</label><br />' +
                            ' <label>' + value.Comentario + '</label>' +
                            '</div>';
                $("#DivComentarios").append(html);
            });
        },
        error: function (data) {

        },

    });
}
function esconderMensaje() {
    $('#alert_danger').hide();
    $('#alert_success').hide();
}
