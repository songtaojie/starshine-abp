$(function () {
    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        $(this).next().toggleClass('show');

        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }

        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');

        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show");
        });

        return false;
    });
    const SELECTOR_DATA_TOGGLE$3 = '[data-bs-toggle="dropdown"]:not(.disabled):not(:disabled)';
    $('.dropdown').hover(
        function () {
            const $dropdown = $(this);
            const $dropdownToggle = $dropdown.find(SELECTOR_DATA_TOGGLE$3);
            if ($dropdownToggle && $dropdownToggle.length > 0) {
                const dropdownInstance = bootstrap.Dropdown.getOrCreateInstance($dropdownToggle[0]);
                if (dropdownInstance) {
                    dropdownInstance.show();
                }
            }
            
        },
        function () {
            const $dropdown = $(this);
            const $dropdownToggle = $dropdown.find(SELECTOR_DATA_TOGGLE$3);
            if ($dropdownToggle && $dropdownToggle.length > 0) {
                const dropdownInstance = bootstrap.Dropdown.getOrCreateInstance($dropdownToggle[0]);
                if (dropdownInstance) {
                    dropdownInstance.hide();
                }
            }
        }
    );
    const $alerts = $('#pageAlerts [role="alert"]');

   

    if ($alerts.length) {
        $alerts.each(function () {
            const $alert = $(this);
            let timerId = null;
            let startTime = Date.now();
            let remaining = 3000;
            let closed = false;

            const alertInstance = bootstrap.Alert.getOrCreateInstance(this);

            // 定义关闭逻辑（只执行一次）
            const tryClose = () => {
                if (!closed) {
                    alertInstance.close();
                    closed = true;
                }
            };

            // 启动初始定时器
            timerId = setTimeout(tryClose, remaining);

            // 鼠标进入，暂停定时器
            $alert.on('mouseenter', function () {
                if (closed) return;
                clearTimeout(timerId);
                const now = Date.now();
                remaining -= (now - startTime);
            });

            // 鼠标离开，恢复定时器
            $alert.on('mouseleave', function () {
                if (closed) return;

                if (remaining <= 0) {
                    tryClose();
                } else {
                    startTime = Date.now();
                    timerId = setTimeout(tryClose, remaining);
                }
            });
        });
    }

});