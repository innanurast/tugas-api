$(document).ready(function () {
    let i = 0;
    var t = $("#tb_department").DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7235/api/Department",
            type: "GET",
            "datatype": "json",
            "dataSrc": "data",

        },
        "columns": [
            {
                "data": null, orderable: false, ordering: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "deptId" },
            { "data": "name" },
            {
                "data": null, render: function (data, type, row) {
                    return '<button class="btn btn-warning"  data-tooltip="tooltip" data-placement="bottom" title="Edit Department" onclick="return GetById(\'' + row.deptId + '\')"><i class="fas fa-edit"></i></button>'
                        + '&nbsp;' +
                        '<button class="btn btn-danger" data-tooltip="tooltip" data-placement="bottom" title="Delete Department" onclick="return Delete(\'' + row.deptId + '\')"><i class="fas fa-trash"></i></button>'
                }
            }], "order": [],
    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

})

//$(function () {
//    $('[data-tooltip="tooltip"]').tooltip();
//});

//$('body').tooltip({
//    selector: '[data-tooltip="tooltip"]',
//    container: 'body'
//});

function save() {
    var Department = new Object(); //membuat variabel obj data
    Department.deptId = "";
    Department.name = $('#depart_name').val();

    if (Department.name == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Maaf data deparment tidak boleh kosong!',
        })
    } else {

                $.ajax({
                    type: 'POST', //API Method
                    url: 'https://localhost:7235/api/Department',//name of url get
                    data: JSON.stringify(Department), //mengubah data department yang diinputkan ke dalam json
                    contentType: "application/json; charset=utf-8",
                }).then((result) => {
                    if (result.status == 200) {
                        Swal.fire('Data berhasil disimpan', result.message, 'success');
                        $('#tb_department').DataTable().ajax.reload();
                    }
                    else {
                        Swal.fire('Data Gagal Ditambahkan', result.message, 'error');
                    }
                });
    }
            
}


function add() {
    $('#depart_name').val('');
    $('#depart_name').focus();
    $('#save').show();
    $('#update').hide();
}


function GetById(deptId) {
    $.ajax({
        url: 'https://localhost:7235/api/Department/' + deptId,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        data: 'json',
        success: function (result) {
            var department = result.data; //adjust with whats your API return
            $('#depart_Id').val(department.deptId);
            $('#depart_name').val(department.name);
            $('#exampleModalLabel').text("Edit Data Department");
            $('#save').hide();
            $('#update').show();
            $('#exampleAdd').modal('show');  
        }
    });
}

function Delete(deptId) {
    $("exampleAdd").modal("hide");
        Swal.fire({
            title: 'Hapus Data?',
            text: "Yakin data ini dihapus?",
            icon: 'error',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Ya, data dihapus!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'https://localhost:7235/api/Department/' + deptId,
                    type: 'DELETE',
                    data: 'json',
                }).then((result) => {
                    if (result.status == 200) {
                        Swal.fire('berhasil dihapus', result.message, 'success');
                        $("#tb_department").DataTable().ajax.reload();
                    }
                }).catch((error) => {
                    Swal.fire('Data gagal dihapus', error.responseJSON.message, 'error');

                })
            }
        })
    //}
}

$(document).ajaxComplete(function () {
    // Required for Bootstrap tooltips in DataTables
    $('[data-tooltips="tooltip"]').tooltip({
        trigger: 'hover'
    });
})

function Update() {
    var Department = new Object(); //membuat variabel obj data
    Department.deptId = $('#depart_Id').val();
    Department.name = $('#depart_name').val();
    if (Department.name == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Maaf data department tidak boleh kosong!',
        })
    } else {
        Swal.fire({
            title: 'Update Data?',
            text: "Yakin data ini diupdate?",
            icon: 'success',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Ya, data diupdate!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: 'https://localhost:7235/api/Department/',//name of url
                    type: 'PUT', //API Method
                    data: JSON.stringify(Department), //mengubah data department yang diinputkan ke dalam json
                    contentType: "application/json; charset=utf-8"
                }).then((result) => {
                    if (result.status == 200) {
                        Swal.fire('Data berhasil diupdate', result.message, 'success');
                        $("#tb_department").DataTable().ajax.reload();
                        $("#exampleAdd").modal("hide");

                    }
                    else {
                        Swal.fire('Data gagal di update', result.message, 'error');
                    }
                });
            }
        })
    }
}

