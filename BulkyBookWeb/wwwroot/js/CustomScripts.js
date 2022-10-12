function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function () {
            $("#formModal .modal-body").html(res);
            $("#formModal .modal-title").html(title);
            $("#formModal").modal('show');

        }
    })
}