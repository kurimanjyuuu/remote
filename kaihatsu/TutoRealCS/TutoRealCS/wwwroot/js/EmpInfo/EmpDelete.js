$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // 削除ボタンのクリックイベント
    $('.btnDelete').on('click', function (e) {
        e.preventDefault();
        confirmDelete();
    });

    // 確認ダイアログ関数
    function confirmDelete() {
        Swal.fire({
            title: '本当に削除しますか？',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'はい',
            cancelButtonText: 'いいえ'
        }).then((result) => {
            if (result.isConfirmed) {
                deleteEmployee();
            }
        });
    }

    // 削除関数
    function deleteEmployee() {
        var empId = $('#EmpId').val().trim();
        console.log({ empId7: empId, ActionType: 'Delete' });

        var formData = { empId7: empId, ActionType: 'Delete' };

        $.ajax({
            url: '/EmpInfo/Regist',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: '削除しました',
                        icon: 'success',
                        confirmButtonText: 'OK',
                    }).then(() => {
                        window.location.reload();
                    });
                } else {
                    showError(response.message);
                }
            },
            error: function (xhr) {
                showError('削除処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // エラーメッセージ表示関数
    function showError(message) {
        Swal.fire({
            title: 'エラー',
            text: message,
            icon: 'error',
            confirmButtonText: 'OK',
        });
    }
});
