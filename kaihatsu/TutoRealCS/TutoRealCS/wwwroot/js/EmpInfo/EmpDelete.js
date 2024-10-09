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
        var DeptCode = $('#DeptCode').val();
        var Seikanji = $('#Seikanji').val();
        var Meikanji = $('#Meikanji').val();
        var Seikana = $('#Seikana').val();
        var Meikana = $('#Meikana').val();
        var MailAddress = $('#MailAddress').val();

        // すべての必須項目が入力されているかチェック
        if (!empId) {
            showError('社員番号を入力してください。');
            return;
        }
        if (!DeptCode) {
            showError('部署コードを入力してください。');
            return;
        }
        if (!Seikanji) {
            showError('姓を入力してください。');
            return;
        }
        if (!Meikanji) {
            showError('名を入力してください。');
            return;
        }
        if (!Seikana) {
            showError('せいを入力してください。');
            return;
        }
        if (!Meikana) {
            showError('めいを入力してください。');
            return;
        }
        if (!MailAddress) {
            showError('メールアドレスを入力してください。');
            return;
        }

        var formData = {
            EmpId7: empId,
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
