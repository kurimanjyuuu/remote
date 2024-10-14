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
        selectEmployee();
    });

    // 検索処理関数
    function selectEmployee() {
        var empId = $('#EmpId').val();
        var deptCode = $('#DeptCode').val();
        var seiKanji = $('#Seikanji').val();
        var meiKanji = $('#Meikanji').val();
        var seiKana = $('#Seikana').val();
        var meiKana = $('#Meikana').val();
        var mailAddress = $('#MailAddress').val();
        var joinDate = $('#JoinDate').val();

        // バリデーション
        if (!validateForm(empId, deptCode, seiKanji, meiKanji, seiKana, meiKana, mailAddress, joinDate)) {
            return;
        }

        var formData = {
            empId7: empId,
            deptCode4: deptCode,
            seiKanji: seiKanji,
            meiKanji: meiKanji,
            seiKana: seiKana,
            meiKana: meiKana,
            mailAddress: mailAddress,
            joinDate: joinDate,
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
                    $('.DataGrid tbody').empty();
                    response.data.forEach(item => {
                        $('.DataGrid tbody').append(`
                            <tr>
                                <td>${item.empId7}</td>
                                <td>${item.deptCode4}</td>
                                <td>${item.seiKanji}</td>
                                <td>${item.meiKanji}</td>
                                <td>${item.seiKana}</td>
                                <td>${item.meiKana}</td>
                                <td>${item.mailAddress}</td>
                                <td>${item.joinDate ? new Date(item.joinDate).toLocaleDateString('ja-JP') : ''}</td> 
                            </tr>
                        `);
                    });

                    Swal.fire({
                        title: `${response.data.length}件ヒットしました`,
                        icon: 'success',
                        confirmButtonText: 'OK',
                    });
                } else {
                    showError('指定された社員番号に該当する情報は見つかりませんでした。');
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
