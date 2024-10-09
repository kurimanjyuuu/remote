$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // 検索ボタンのクリックイベント
    $('.btnSearch').on('click', function (e) {
        e.preventDefault(); // デフォルトのフォーム送信をキャンセル

        var formData = {
            EmpId7: $('#EmpId').val(), // 社員番号を取得
            ActionType: 'Search' // アクションタイプを設定
        };

        // AJAXリクエスト
        $.ajax({
            url: '/EmpInfo/Regist',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                console.log(response); // レスポンスを確認
                // 検索結果が存在する場合、フォームに値を設定
                if (response.success && response.data) {
                    $('#DeptCode').val(response.data.DeptCode4);
                    $('#Seikanji').val(response.data.Seikanji);
                    $('#Meikanji').val(response.data.Meikanji);
                    $('#Seikana').val(response.data.Seikana);
                    $('#Meikana').val(response.data.Meikana);
                    $('#MailAddress').val(response.data.MailAddress);

                    // ボタンのアクションを更新
                    $('.btnActive').data('action', 'Update').text('更新'); // 更新ボタンに変更
                } else {
                    Swal.fire({
                        title: 'エラー',
                        text: response.message || 'データが見つかりませんでした。',
                        icon: 'error',
                        confirmButtonText: 'OK',
                    });
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                Swal.fire({
                    title: 'エラー',
                    text: '検索処理に失敗しました。' + xhr.status + ': ' + xhr.statusText,
                    icon: 'error',
                    confirmButtonText: 'OK',
                });
            }
        });
    });

    // 削除ボタンのクリックイベント
    $('.btnDelete').on('click', function (e) {
        e.preventDefault(); // デフォルトのフォーム送信をキャンセル
        var empId = $('#EmpId').val();

        // EmpIdが空の場合は削除処理を行わない
        if (!empId) {
            Swal.fire({
                title: 'エラー',
                text: '削除する社員番号を入力してください。',
                icon: 'error',
                confirmButtonText: 'OK',
            });
            return;
        }

        var formData = {
            EmpId7: empId,
            ActionType: 'Delete' // ボタンのアクションタイプ
        };

        // AJAXリクエスト
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
                        window.location.reload(); // 画面をリロード
                    });
                } else {
                    Swal.fire({
                        title: 'エラー',
                        text: response.message,
                        icon: 'error',
                        confirmButtonText: 'OK',
                    });
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText);
                Swal.fire({
                    title: 'エラー',
                    text: '削除処理に失敗しました。' + xhr.status + ': ' + xhr.statusText,
                    icon: 'error',
                    confirmButtonText: 'OK',
                });
            }
        });
    });

    // バリデーション関数
    function validateForm(data) {
        if (!data.EmpId7) {
            showErrorModal(REQUIRE_TITLE, '社員番号' + REQUIRE_MSG);
            return false;
        }
        if (!data.DeptCode4) {
            showErrorModal(REQUIRE_TITLE, '部署コード' + REQUIRE_MSG);
            return false;
        }
        if (!data.Seikanji) {
            showErrorModal(REQUIRE_TITLE, '姓' + REQUIRE_MSG);
            return false;
        }
        if (!data.Meikanji) {
            showErrorModal(REQUIRE_TITLE, '名' + REQUIRE_MSG);
            return false;
        }
        if (!data.Seikana) {
            showErrorModal(REQUIRE_TITLE, 'せい' + REQUIRE_MSG);
            return false;
        }
        if (!data.Meikana) {
            showErrorModal(REQUIRE_TITLE, 'めい' + REQUIRE_MSG);
            return false;
        }
        if (!data.MailAddress) {
            showErrorModal(REQUIRE_TITLE, 'メールアドレス' + REQUIRE_MSG);
            return false;
        }
        return true;
    }
});
