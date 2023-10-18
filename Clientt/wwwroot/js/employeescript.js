$(document).ready(function () {
    let i = 0;
    var t = $("#tb_employee").DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7235/api/Employees",
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
            { "data": "nik" },
            { "data": "firstName" },
            { "data": "phone" },
            { "data": "email" },
            { "data": "address" },
            { "data": "isActive" },
            { "data": "department.name" },
            {
                "data": null, render: function (data, type, row) {
                    return '<button class="btn btn-warning" data-tooltip="tooltip" data-placement="bottom" title="Edit Employee" onclick="return GetById(\'' + row.nik + '\')"><i class="fas fa-edit"></i></button>'
                        + '&nbsp;' +
                        '<button class="btn btn-danger" data-tooltip="tooltip" data-placement="bottom" title="Delete Employee" onclick="return Delete(\'' + row.nik + '\')"><i class="fas fa-trash"></i></button>'
                }
            }], "order": [],
    });
    t.on('order.dt search.dt', function () {
        t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
})

var departmentSelect = document.getElementById('departmentId');
$.ajax({
    type: "GET",
    url: "https://localhost:7235/api/Department",
    dataType: "json",
    success: function (result) {
        $.each(result.data, function (i, d) {
            var select = `<option value="${d.deptId}">${d.name}</option>`;
            $(select).appendTo('#departmentId');
        });
    }
});

function save() {
    if ($('#firstName').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'firstname is required!',
        })
    }
    if ($('#lastName').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'lastname is required!',
        })
    }
    if ($('#phone').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'phone is required!',
        })
    }
    if ($('#address').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'address is required!',
        })
    }
    if ($('#isActive').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'isActive is required!',
        })
    }
    if ($('#departmentId').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Department is required!',
        })
    }
    if ($('#departmentId').val() == "" || $('#departmentId').val() == "" || $('#isActive').val() == "" || $('#address').val() == ""
        || $('#phone').val() == "" || $('#lastName').val() == "" || $('#firstName').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'The field is required!',
        })
    } else {
        var Employee = new Object(); //membuat variabel obj data
        Employee.nik = "";
        Employee.firstName = $('#firstName').val();
        Employee.lastName = $('#lastName').val();
        Employee.email = "";
        Employee.phone = $('#phone').val();;
        Employee.address = $('#address').val();
        Employee.isActive = Boolean($('#isActive').val());
        Employee.Department_id = $('#departmentId').val();

        $.ajax({
            type: 'POST', //API Method
            url: 'https://localhost:7235/api/Employees',//name of url get
            data: JSON.stringify(Employee), //mengubah data department yang diinputkan ke dalam json
            contentType: "application/json; charset=utf-8",
        }).then((result) => {
            if (result.status == 200) {
                Swal.fire('Data berhasil disimpan', result.message, 'success');
                $('#tb_employee').DataTable().ajax.reload();
            }
            else {
                Swal.fire('Data Gagal Ditambahkan', result.message, 'error');
            }
        });
    }
}

function GetById(nik) {
    $.ajax({
        url: 'https://localhost:7235/api/Employees/' + nik,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        data: 'json',
        success: function (result) {
            $('#optionIsActive').remove(); //untuk menghapus option yang terselected sebelumnya
            $('#optionDepartment').remove();

            var Employee = result.data; //adjust with whats your API return
            $('#nik').val(Employee.nik);
            $('#firstName').val(Employee.firstName);
            $('#lastName').val(Employee.lastName);
            $('#email').val(Employee.email);
            $('#phone').val(Employee.phone);
            $('#address').val(Employee.address);
            $('#isActive').val(Employee.isActive);
            $('#departmentId').val(Employee.Department_id);

            //console.log(Employee.isActive, Employee.deptId)

            var selectIsActive = `<option id="optionIsActive" value="${Employee.isActive}" selected>${Employee.isActive ? 'Aktif' : 'Non-Aktif'}</option>`;
            $(selectIsActive).appendTo('#isActive');

            var selectDepartment = `<option id="optionDepartment" value="${Employee.department.deptId}" selected>${Employee.department.name}</option>`;
            $(selectDepartment).appendTo('#departmentId');

            //enable form edit
            $('#exampleModalLabel').text("Edit Data Employee");
            $('#save').hide();
            $('#update').show();
            $('#exampleAdd').modal('show'); //untuk menampilkan modal show form tambah-edit
        }
    });
}

function add() {
    //$('#depart_name').val(department.name);
    $('#firstName').val('');
    $('#firstName').focus();
    $('#lastName').val('');
    $('#email').val('');
    $('#phone').val('');
    $('#address').val('');
    $('#isActive').val('');
    $('#departmentId').val('');
    //$('#ExampleModalLabel').val("");
    $('#save').show();
    $('#update').hide();
}

function Update() {
    var Employee = new Object(); //membuat variabel obj data
    Employee.nik = $('#nik').val();;
    Employee.firstName = $('#firstName').val();
    Employee.lastName = $('#lastName').val();
    Employee.email = $('#email').val();;
    Employee.phone = $('#phone').val();;
    Employee.address = $('#address').val();
    Employee.isActive = Boolean($('#isActive').val());
    Employee.Department_id = $('#departmentId').val();
    if (Employee.firstName == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'firstname kosong!',
        })
    }
    else if (Employee.lastName == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'lastname tidak boleh kosong!',
        })
    }
    else if (Employee.phone == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'phone tidak boleh kosong!',
        })
    } 
    else if (Employee.address == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'address tidak boleh kosong!',
        })
    } 
    else {
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
                    url: 'https://localhost:7235/api/Employees/' + Employee.nik,//name of url
                    type: 'PUT', //API Method
                    data: JSON.stringify(Employee), //mengubah data department yang diinputkan ke dalam json
                    contentType: "application/json; charset=utf-8"
                }).then((result) => {
                    if (result.status == 200) {
                        Swal.fire('Data berhasil diupdate', result.message, 'success');
                        $("#tb_employee").DataTable().ajax.reload();
                        $("#exampleAdd").modal("hide");
                    }
                    else {
                        Swal.fire('Data gagal di update', result.message, 'error');
                    }
                }).catch((error) => {
                    Swal.fire('Data gagal dihapus', error.responseJSON.message, 'error');

                })
            }
        })
    }
}

function Delete(nik) {
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
                url: 'https://localhost:7235/api/Employees/' + nik,
                type: 'DELETE',
                data: 'json',
            }).then((result) => {
                if (result.status == 200) {
                    Swal.fire('berhasil dihapus', result.message, 'success');
                    $("#tb_employee").DataTable().ajax.reload();
                } else {
                    Swal.fire('Data gagal dihapus', result.message, 'error');
                }
            });
        }
    })
}

//function userValid() {
//    var firstname, lastname, phone, address, isaktif, department;
//    firstname = document.getElementById("txtUserId").value;
//    lastname = document.getElementById("ddlType").value;
//    phone = document.getElementById("txt2").value;
//    address = document.getElementById("txtmail").value;
//    isactive = document.getElementById("txtmail").value;
//    department = document.getElementById("txtmail").value;
//} 