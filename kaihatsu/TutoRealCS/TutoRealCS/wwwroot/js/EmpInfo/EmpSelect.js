function searchEmployee() {
    var empId = $('#EmpId').val();  // 社員番号を取得
    var deptCode = $('#DeptCode').val();  // 部署コードを取得
    var seikanji = $('#Seikanji').val();  // 姓を取得
    var meikanji = $('#Meikanji').val();  // 名を取得
    var seikana = $('#Seikana').val();  // せいを取得
    var meikana = $('#Meikana').val();  // めいを取得
    var mailAddress = $('#MailAddress').val();  // メールアドレスを取得

    // 社員番号が必須項目であることを確認
    if (!empId) {
        showError('社員番号を入力してください。');
        return;
    }

    $.ajax({
        url: '/EmpInfo/Regist',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),  // formDataをJSON形式で送信
        success: function (response) {
            // 成功時の処理
            if (response.success) {
                alert("search");
                if (response.data && response.data.length > 0) {
                    // データをフォームに表示
                    $('#DeptCode').val(response.data[0].DeptCode4);
                    $('#Seikanji').val(response.data[0].Seikanji);
                    $('#Meikanji').val(response.data[0].Meikanji);
                    $('#Seikana').val(response.data[0].Seikana);
                    $('#Meikana').val(response.data[0].Meikana);
                    $('#MailAddress').val(response.data[0].MailAddress);
                    alert("search2");
                    Swal.fire({
                        title: '1件ヒットしました',
                        icon: 'success',
                        confirmButtonText: 'OK',
                    });
                } else {
                    alert("search3");
                    showError('指定された社員番号に該当する情報は見つかりませんでした。');
                }
            } else {
                showError(response.message || '検索処理に失敗しました。');
            }
        },
        error: function (xhr) {
            showError('検索処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
        }
    });
}
