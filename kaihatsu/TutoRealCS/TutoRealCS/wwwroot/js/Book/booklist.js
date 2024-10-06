$(function () {
    //登録ボタン押下時の登録画面遷移(新規登録)
    $('.button_active').on('click', function (e) {
        // デフォルトのフォーム送信をキャンセル
        e.preventDefault();
        try {
            showLoading();
            // コントローラ名とアクション名を変数に格納
            var url = '/Book/Index';
            window.location.href = url;
        } catch (e) {
            hideLoading();
        }
    });

    //詳細ボタン押下時処理(データ参照)
    $('.btnDetail').on('click', function (e) {
        e.preventDefault();
        var $row = $(this).closest('tr');
        var bookid = $row.find('td').first().attr('id')
        try {
            showLoading();
            // コントローラ名とアクション名を変数に格納
            var url = '/Book/Index?id=' + bookid;
            window.location.href = url;
        } catch (e) {
            hideLoading();
        }
    });

    //貸出ボタン押下時処理
    $('.btnLend').on('click', function (e) {
        e.preventDefault();
        var $row = $(this).closest('tr');
        var bookid = $row.find('td').first().attr('id');
        $('#Id').val(bookid);
        var formdata = $("#trForm").serialize();

        try {
            showLoading();
            $.ajax({
                url: '/Book/Lend?formData=' + formdata, // コントローラーのアクションへのパス
                type: 'POST',
                contentType: false, // デフォルトのContent-Typeを使用
                processData: false, // データを処理せず、そのまま送信
                success: function (response) {
                    // 処理が成功した場合、SweetAlertモーダルを表示
                    Swal.fire({
                        title: '貸出処理が完了しました',
                        icon: 'success',
                        confirmButtonText: '閉じる',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        allowEnterKey: false
                    });
                    $row.find('.btnLend').toggleClass("hidden");
                    $row.find('.btnReturn').toggleClass("hidden");
                },
                error: function (xhr, status, error) {
                    // エラー処理をここに書く
                    Swal.fire({
                        title: 'エラー',
                        text: '貸出処理に失敗しました。',
                        icon: 'error',
                        confirmButtonText: 'OK',
                    });
                }
            });
        } catch (e) {
            hideLoading();
        }
    });

    //返却ボタン押下時処理
    $('.btnReturn').on('click', function (e) {
        e.preventDefault();
        var $row = $(this).closest('tr');
        var bookid = $row.find('td').first().attr('id');
        $('#Id').val(bookid);
        var formdata = $("#trForm").serialize();

        try {
            showLoading();
            $.ajax({
                url: '/Book/Return?id=' + bookid, // コントローラーのアクションへのパス
                type: 'POST',
                data: formdata, // フォームのデータをシリアライズ
                contentType: false, // デフォルトのContent-Typeを使用
                processData: false, // データを処理せず、そのまま送信
                success: function (response) {
                    // 処理が成功した場合、SweetAlertモーダルを表示
                    Swal.fire({
                        title: '返却処理が完了しました',
                        icon: 'success',
                        confirmButtonText: '閉じる',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        allowEnterKey: false
                    });
                    $row.find('.btnLend').toggleClass("hidden");
                    $row.find('.btnReturn').toggleClass("hidden");
                },
                error: function (xhr, status, error) {
                    // エラー処理をここに書く
                    Swal.fire({
                        title: 'エラー',
                        text: '返却処理に失敗しました。',
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