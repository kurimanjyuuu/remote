$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // ボタンのクリックイベント
    $('.btnSearch').on('click', function (e) {
        // デフォルトのフォーム送信をキャンセル
        e.preventDefault();

        var EmpId = $('#EmpId').val();
        var DeptCode = $('#DeptCode').val();
        var Seikanji = $('#Seikanji').val();
        var Meikanji = $('#Meikanji').val();
        var Seikana = $('#Seikana').val();
        var Meikana = $('#Meikana').val();
        var MailAddress = $('#MailAddress').val();

        // 社員番号
        if (!EmpId) {
            showErrorModal(REQUIRE_TITLE, '社員番号' + REQUIRE_MSG);
            return;
        }
        // 部署コード
        if (!DeptCode) {
            showErrorModal(REQUIRE_TITLE, '部署コード' + REQUIRE_MSG);
            return;
        }
        // 姓
        if (!Seikanji) {
            showErrorModal(REQUIRE_TITLE, '姓' + REQUIRE_MSG);
            return;
        }
        // 名
        if (!Meikanji) {
            showErrorModal(REQUIRE_TITLE, '名' + REQUIRE_MSG);
            return;
        }
        // せい
        if (!Seikana) {
            showErrorModal(REQUIRE_TITLE, 'せい' + REQUIRE_MSG);
            return;
        }
        // めい
        if (!Meikana) {
            showErrorModal(REQUIRE_TITLE, 'めい' + REQUIRE_MSG);
            return;
        }
        // メールアドレス
        if (!MailAddress) {
            showErrorModal(REQUIRE_TITLE, 'メールアドレス' + REQUIRE_MSG);
            return;
        }

        try {
            showLoading();
            const formData = $('#trForm').serialize();

            // AJAXリクエストを開始
            $.ajax({
                url: '/EmpInfo/Regist', // コントローラーのアクションへのパス
                type: 'POST',
                data: formData, // フォームのデータをシリアライズ
                success: function (response) {
                    console.log(response); // レスポンスを確認
                    // 処理が成功した場合、SweetAlertモーダルを表示
                    Swal.fire({
                        title: '1件ヒットしました',
                        icon: 'success',
                        showCancelButton: true,
                        confirmButtonText: '確認する',
                        cancelButtonText: 'トップページへ戻る',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        allowEnterKey: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            // 自画面リフレッシュ
                            window.location.reload();
                        } else {
                            // 画面遷移
                            window.location.href = '/EmpInfo/Index';
                        }
                    });
                },
                error: function (xhr, status, error) {
                    // エラー処理
                    Swal.fire({
                        title: 'エラー',
                        text: '入力した情報はありません' + xhr.status + ': ' + xhr.statusText,
                        icon: 'error',
                        confirmButtonText: 'OK',
                    });
                }
            });
        } catch (e) {
            hideLoading();
        }
    });
});
