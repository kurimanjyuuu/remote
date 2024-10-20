$(function () {
    // CSRFトークンの設定
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });

    // ボタンのクリックイベント
    $(document).on('click', '.button_active', function (e) {
        e.preventDefault();

        if ($('#UpdateDatetime').val() === "") {
            handleRegister();
        } else {
            handleUpdate();
        }
    });

    // 検索ボタンのクリックイベント
    $(document).on('click', '.btnSearch', function (e) {
        e.preventDefault();
        searchEmployee();
    });

    // 新規登録関数
    function handleRegister() {
        var formData = getFormData('Register');
        if (!validateForm(formData)) return;

        $.ajax({
            url: '/EmpInfo/Regist',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                handleResponse(response);
            },
            error: function (xhr) {
                showError('処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // 更新処理
    function handleUpdate() {
        var formData = getFormData('Update');
        if (!validateForm(formData)) return;

        $.ajax({
            url: '/EmpInfo/Regist',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                handleResponse(response);
            },
            error: function (xhr) {
                showError('処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // 検索処理関数
    function searchEmployee() {
        var empId = $('#EmpId').val();

        // 検索用のバリデーション
        if (!empId) {
            showError('社員番号は必須です。');
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
                if (response.success) {
                    fillFields(response.data);
                    showSuccess('情報を取得しました。');
                    $('#RetireDateRow').show(); // 退職日フィールドを表示
                } else {
                    showError(response.message || '検索処理に失敗しました。');
                }
            },
            error: function (xhr) {
                showError('検索処理に失敗しました。' + xhr.status + ': ' + xhr.statusText);
            }
        });
    }

    // フィールドに値を設定する関数
    function fillFields(data) {
        $('#DeptCode').val(data.deptCode4);
        $('#Seikanji').val(data.seiKanji);
        $('#Meikanji').val(data.meiKanji);
        $('#Seikana').val(data.seiKana);
        $('#Meikana').val(data.meiKana);
        $('#MailAddress').val(data.mailAddress);
        $('#JoinDate').val(data.joinDate);
        $('#UpdateDatetime').val(data.updateDatetime);
    }

    // フォームデータをまとめる関数
    function getFormData(actionType) {
        return {
            empId7: $('#EmpId').val(),
            deptCode4: $('#DeptCode').val(),
            seiKanji: $('#Seikanji').val(),
            meiKanji: $('#Meikanji').val(),
            seiKana: $('#Seikana').val(),
            meiKana: $('#Meikana').val(),
            mailAddress: $('#MailAddress').val(),
            joinDate: formatDate($('#JoinDate').val()),  // 日付形式の変換
            retireDate: formatDate($('#RetireDate').val()), 
            updateDatetime: $('#UpdateDatetime').val(),
            ActionType: actionType
        };
    }

    // 日付をフォーマットする関数
    function formatDate(dateString) {
        if (!dateString) return null; // 空の場合はnullを返す
        var date = new Date(dateString);
        return date.toISOString();
    }

    // エラーハンドリングやバリデーション関数
    function handleResponse(response) {
        if (response.success) {
            Swal.fire({
                title: response.message,
                icon: 'success',
                confirmButtonText: 'OK',
            }).then(() => {
                window.location.reload();
            });
        } else {
            showError(response.message);
        }
    }

    // バリデーション関数
    function validateForm(formData) {
        const empIdPattern = /^[0-9]{7}$/; // 数値7桁の正規表現
        if (!empIdPattern.test(formData.empId7)) {
            showError('社員番号は7桁の数値でなければなりません。');
            return false;
        }

        if (!formData.deptCode4) {
            showError('部署コードを入力してください。');
            return false;
        }
        if (!formData.seiKanji) {
            showError('姓を入力してください。');
            return false;
        }
        if (!formData.meiKanji) {
            showError('名を入力してください。');
            return false;
        }
        if (!formData.seiKana) {
            showError('せいを入力してください。');
            return false;
        }
        if (!formData.meiKana) {
            showError('めいを入力してください。');
            return false;
        }
        if (!formData.mailAddress) {
            showError('メールアドレスを入力してください。');
            return false;
        }
        if (!formData.joinDate) {
            showError('入社日を選択してください。');
            return false;
        }
        return true;
    }

    // 成功メッセージ表示関数
    function showSuccess(message) {
        Swal.fire({
            title: '成功',
            text: message,
            icon: 'success',
            confirmButtonText: 'OK',
        });
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

    // 初期状態で退職日フィールドを非表示にする
    $('#RetireDateRow').hide(); 
});
