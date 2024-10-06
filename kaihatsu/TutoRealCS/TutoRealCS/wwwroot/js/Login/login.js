$(function () {
    // フォームの送信イベント
    $('.button_active').on('click', function (event) {
        // デフォルトのフォーム送信をキャンセル
        event.preventDefault();
        var id = $('#LoginUser_EmpId7').val();
        var pass = $('#LoginUser_Password').val();

        if (!id) {  // 入力が空、または検証に失敗した場合
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, '社員番号' + REQUIRE_MSG);
            return;
        }
        if (!pass) {  // 入力が空、または検証に失敗した場合
            // SweetAlertを表示
            showErrorModal(REQUIRE_TITLE, 'パスワード' + REQUIRE_MSG);
            return;
        }
        try {

            showLoading();

            $('#trForm').submit();
        } catch (e) {
            hideLoading();
        }
    });
    $('.button_guest').on('click', function () {
        try {
            showLoading();
            // コントローラ名とアクション名を変数に格納
            var url = '/Main/Index';

            $('#LoginUser_EmpId7').val('');
            $('#LoginUser_Password').val('');
            $('#LoginUser_GuestKbn').prop('checked', true);
            window.location.href = url;
        } catch (e) {
            hideLoading();
        }
    });
    $('.button_skill').on('click', function () {
        $('#LoginUser_EmpId7').val('');
        $('#LoginUser_Password').val('');
        $('#LoginUser_GuestKbn').prop('checked', true);
        try {
            showLoading();
            $.ajax({
                url: '/SkillCheck/Basic', // コントローラーのアクションへのパス
                type: 'GET',
                contentType: false, // デフォルトのContent-Typeを使用
                processData: false, // データを処理せず、そのまま送信
                success: function (response) {
                },
                error: function (xhr, status, error) {
                }
            });
        } catch (e) {
            hideLoading();
        }
    });
});