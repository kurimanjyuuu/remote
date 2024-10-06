$(function () {
    // -------------------------------------------------------------
    // 下記javascriptを使用すると、指定したＫＥＹが抑止されます
    // -------------------------------------------------------------
    $(document).on('keydown', function (evt) {
        if ((evt.which == 112)  // F1
            || (evt.which == 71 && evt.ctrlKey)  // Ctrl+G
            || (evt.which == 70 && evt.ctrlKey)  // Ctrl+F
            || (evt.which == 114)  // F3
            || (evt.which == 115)  // F4
            || (evt.which == 82 && evt.ctrlKey)  // Ctrl+R
            || (evt.which == 116)  // F5
            || (evt.which == 122)  // F11
            || (evt.which == 73 && evt.ctrlKey && evt.shiftKey)  // Ctrl+Shift+I
            || (evt.which == 123)  // F12
            || (evt.which == 37 && evt.altKey)  // Alt+Left Arrow
            || (evt.which == 39 && evt.altKey)  // Alt+Right Arrow
            || (evt.which == 8)  // Backspace
            || (evt.which == 121 && evt.shiftKey)  // Shift+F10
            || (evt.which == 13)  // Enter
            || (evt.which == 27)  // Esc
            || (evt.which == 32)  // Space
            || (evt.which == 46)  // Delete
        ) {
            // 許可する要素をチェック
            var allowBackspace = (evt.target.tagName === 'INPUT' && (evt.target.type === 'text' || evt.target.type === 'password')) ||
                evt.target.tagName === 'TEXTAREA';

            // イベントが許可された要素から発生していない、かつBackspaceキーが押された場合
            if (!(allowBackspace && evt.which === 8)) {
                evt.preventDefault();
                return false;
            }
        }
    });
    // 禁止右クリックメニュー
    $(document).on("contextmenu", function () {
        return false;
    });
    $('.usericon').on('click', function (event) {
        // メニューの表示/非表示を切り替える
        $('.userlist').toggle();

        // イベントの伝播を止める（他の場所をクリックした時のイベントハンドラーが呼ばれないようにする）
        event.stopPropagation();
    });

    // メニュー以外の場所をクリックした時にメニューを閉じる
    $(window).on('click', function () {
        $('.userlist').hide();
    });

    // メニュー内のクリックイベントがウィンドウに伝播するのを防ぐ
    $('.userlist').on('click', function (event) {
        event.stopPropagation();
    });

    $('.side_button').on('click', function () {
        // コントローラ名とアクション名を変数に格納
        var controllerName = $(this).data('controller');
        var actionName = $(this).data('action');

        var url = '/' + controllerName + '/' + actionName;
        window.location.href = url;
    });

    // DIVクリックでinput[type=file]をトリガーする
    $('.thumb').on('click', function () {
        $('#fileInput').click();
    });

    // ファイルが選択されたときにサムネイルを表示する
    $('#fileInput').on('change', function () {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('.thumb').attr('src', e.target.result);
        };
        // ファイルの読み込みを開始
        reader.readAsDataURL(this.files[0]);
    });

    $('.btnBack').on('click', function () {
        // 前のページに戻る
        window.history.back();
    });

    // チェックボックスの状態が変わったら合計値を計算
    $('.CategoryCheck').on('change', function () {
        // 合計値を初期化
        var totalValue = 0;

        // チェックされたチェックボックスのビット値を合算
        $('.CategoryCheck:checked').each(function () {
            // 隣接するhiddenフィールドからBitValueを取得
            var bitValue = parseInt($(this).prev('input[type="hidden"]').val());
            // 合計値に加算
            totalValue += bitValue;
        });

        // 合計値を非表示フィールドに設定
        $('#SelectCategory').val(totalValue);
    });
});

function showErrorModal(subject, msg) {
    Swal.fire({
        title: subject,
        text: msg,
        icon: 'error',
        confirmButtonText: 'OK'
    });
};

function showLoading() {
    Swal.fire({
        title: '処理中',
        html: '処理終了までお待ちください。',
        didOpen: () => {
            Swal.showLoading()
        },
        allowOutsideClick: false,
        allowEscapeKey: false,
        allowEnterKey: false,
        showConfirmButton: false  // 「OK」ボタンを非表示にする
    });
}
function hideLoading() {
    Swal.close();
}