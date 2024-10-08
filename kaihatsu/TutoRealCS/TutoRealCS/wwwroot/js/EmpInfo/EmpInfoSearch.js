
$(function () {

    // フォームの送信イベント
    $('.btnActive').on('click', function (e) {
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
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '社員番号' + REQUIRE_MSG);
            return;
        }
        // 部署コード
        if (!DeptCode) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '部署コード' + REQUIRE_MSG);
            return;
        }
        // 姓
        if (!Seikanji) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '姓' + REQUIRE_MSG);
            return;
        }
        //　名
        if (!Meikanji) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '名' + REQUIRE_MSG);
            return;
        }
        // せい
        if (!Seikana) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, 'せい' + REQUIRE_MSG);
            return;
        }
        // めい
        if (!Meikana) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, 'めい' + REQUIRE_MSG);
            return;
        }
        // メールアドレス
        if (!MailAddress) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, 'メールアドレス' + REQUIRE_MSG);
            return;
        }

        try {
            showLoading();
            const formData = new FormData();
            const serializedData = $('#trForm').serialize();
            $.each(serializedData.split('&'), function () {
                const pair = this.split('=');
                formData.append(decodeURIComponent(pair[0]), decodeURIComponent(pair[1]));
            });

            //入力情報の登録
            // AJAXリクエストを開始
            $.ajax({
                url: '/EmpInfo/Regist', // コントローラーのアクションへのパス
                type: 'POST',
                data: formData, // フォームのデータをシリアライズ
                contentType: false, // デフォルトのContent-Typeを使用
                processData: false, // データを処理せず、そのまま送信
                success: function (response) {
                    // 処理が成功した場合、SweetAlertモーダルを表示
                    Swal.fire({
                        title: '登録処理が完了しました',
                        icon: 'success',
                        showCancelButton: true,
                        confirmButtonText: '続けて登録する',
                        cancelButtonText: 'トップページへ戻る',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        allowEnterKey: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            // ボタンA（自画面リフレッシュ）がクリックされたときの処理
                            window.location.reload();
                        } else {
                            // ボタンB（画面遷移）がクリックされたときの処理
                            window.location.href = '/EmpInfo/Index';
                        }
                    });
                },
                error: function (xhr, status, error) {
                    // エラー処理をここに書く
                    Swal.fire({
                        title: 'エラー',
                        text: '登録処理に失敗しました。',
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

});