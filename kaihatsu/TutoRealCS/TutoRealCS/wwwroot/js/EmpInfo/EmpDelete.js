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
        deleteEmployee();
    });

    // 削除関数
    function deleteEmployee() {
        var empId = $('#EmpId').val();
        var retireDate = $('#RetireDate').val();

        // バリデーションを追加
        if (!validateForm(empId, retireDate)) {
            return;
        }

        var formData = {
            empId7: empId,
            retireDate: retireDate,
            ActionType: 'Delete'
        };

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

    // すべての必須項目が入力されているかチェック
    function validateForm(empId, retireDate) {
        let errorMessage = '';

        if (!empId && !retireDate) {
            errorMessage = '社員番号と退職日を入力してください。';
        } else if (!empId) {
            errorMessage = '社員番号は必須です。';
        } else if (!retireDate) {
            errorMessage = '退職日は必須です。';
        }

        if (errorMessage) {
            showError(errorMessage);
            return false;
        }

        return true;
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
