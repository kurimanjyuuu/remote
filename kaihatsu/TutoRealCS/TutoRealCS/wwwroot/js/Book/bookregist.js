$(document).ready(function () {
    var filepath = $('.LoadFilePath').val();
    // サムネイルを表示する関数
    if (filepath) {
        // 取得したファイルパスをもとに画像を表示
        $('<img>')
            .attr('src', filepath)
            .attr('alt', 'サムネイル')
            .appendTo('.thumb');
    }
});
$(function () {
    $('.RossFlg').on('click', function () {
        // チェックボックスの状態をチェック
        var isChecked = $(this).is(':checked');
        // ドロップダウンリストの無効/有効状態を切り替え
        $('.dropdown').prop('disabled', isChecked);
    });

    // フォームの送信イベント
    $('.button_active').on('click', function (e) {
        // デフォルトのフォーム送信をキャンセル
        e.preventDefault();
        var name = $('#Name').val();
    
        //書籍名称
        if (!name) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '書籍名称' + REQUIRE_MSG);
            return;
        }

        //カテゴリチェックリスト
        var values = getCheckedValues();
        if (values.length == 0) {
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, 'カテゴリ' + REQUIRE_CHK);
            return;
        }
        try {
            showLoading();
            //ファイル情報
            var formData = new FormData();
            formData.append('fileInput', $('#fileInput')[0].files[0]);

            var serializedData = $('#trForm').serialize();
            $.each(serializedData.split('&'), function () {
                var pair = this.split('=');
                formData.append(decodeURIComponent(pair[0]), decodeURIComponent(pair[1]));
            });

            //入力情報の登録
            // AJAXリクエストを開始
            $.ajax({
                url: '/Book/Regist', // コントローラーのアクションへのパス
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
                        confirmButtonText: '次の書籍を登録',
                        cancelButtonText: '書籍一覧へ',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        allowEnterKey: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            // ボタンA（自画面リフレッシュ）がクリックされたときの処理
                            window.location.reload();
                        } else {
                            // ボタンB（画面遷移）がクリックされたときの処理
                            window.location.href = '/Book/Search';
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

function getCheckedValues() {
    var values = $('.CategoryCheck:checked').map(function () {
        return this.value;
    }).get();
    return values;
}