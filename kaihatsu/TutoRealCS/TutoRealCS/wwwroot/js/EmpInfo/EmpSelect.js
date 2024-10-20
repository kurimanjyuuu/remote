$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // 検索ボタンのクリックイベント
    $('.btnSearch').on('click', function (e) {
        e.preventDefault();
        searchEmployee();
    });

    // 検索処理関数
    function searchEmployee() {
        var empId = $('#EmpId').val();

        if (!validateForm(empId)) {
            return;
        }

        var formData = {
            empId7: empId,
            ActionType: 'Search'
        };

        $.ajax({
            url: '/EmpInfo/Regist', 
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                console.log(response.data);
                if (response.success) {
                    // DBの情報を入力フィールドに自動でセット
                    $('#DeptCode').val(response.data.deptCode4);
                    $('#Seikanji').val(response.data.seiKanji);
                    $('#Meikanji').val(response.data.meiKanji);
                    $('#Seikana').val(response.data.seiKana);
                    $('#Meikana').val(response.data.meiKana);
                    $('#MailAddress').val(response.data.mailAddress);
                    $('#JoinDate').val(response.data.joinDate);

                    Swal.fire({
                        title: '検索成功',
                        text: '情報を取得しました。',
                        icon: 'success',
                        confirmButtonText: 'OK',
                    });
                } else {
                    showError(response.message || '検索処理に失敗しました。');
                }
            },
            error: function (xhr) {
                console.error(xhr);
                showError('検索処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // バリデーション関数
    function validateForm(empId) {
        if (!empId) {
            showError('社員番号は必須です。');
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
