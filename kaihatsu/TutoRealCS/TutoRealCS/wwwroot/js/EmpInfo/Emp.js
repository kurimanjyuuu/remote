$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // 登録ボタンのクリックイベント
    $('.btnActive').on('click', function (e) {
        e.preventDefault();
        insertEmployee();
    });

    // 新規登録関数
    function insertEmployee() {
        var empId = $('#EmpId').val();
        var DeptCode = $('#DeptCode').val();
        var Seikanji = $('#Seikanji').val();
        var Meikanji = $('#Meikanji').val();
        var Seikana = $('#Seikana').val();
        var Meikana = $('#Meikana').val();
        var MailAddress = $('#MailAddress').val();

        if (!validateForm(empId, DeptCode, Seikanji, Meikanji, Seikana, Meikana, MailAddress)) {
            return;
        }

        var formData = {
            EmpId7: empId,
            DeptCode4: DeptCode,
            Seikanji: Seikanji,
            Meikanji: Meikanji,
            Seikana: Seikana,
            Meikana: Meikana,
            MailAddress: MailAddress,
            ActionType: 'Register'
        };

        $.ajax({
            url: '/EmpInfo/Regist',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: '登録完了',
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
                showError('処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // バリデーション関数
    function validateForm(empId, DeptCode, Seikanji, Meikanji, Seikana, Meikana, MailAddress) {
        if (!empId) {
            showError('社員番号は必須です。');
            return false;
        }
        if (!DeptCode) {
            showError('部署コードは必須です。');
            return false;
        }
        if (!Seikanji) {
            showError('姓は必須です。');
            return false;
        }
        if (!Meikanji) {
            showError('名は必須です。');
            return false;
        }
        if (!Seikana) {
            showError('せいは必須です。');
            return false;
        }
        if (!Meikana) {
            showError('めいは必須です。');
            return false;
        }
        if (!MailAddress) {
            showError('メールアドレスは必須です。');
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
