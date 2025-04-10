document.addEventListener('DOMContentLoaded', function () {
    // عناصر DOM
    const periodTitleElement = document.getElementById('period-title');
    const monthDaysElement = document.getElementById('month-days');
    const prevPeriodButton = document.getElementById('prev-period');
    const nextPeriodButton = document.getElementById('next-period');
    const todayFloatingBtn = document.getElementById('today-floating-btn');

    // عناصر خيارات العرض
    const monthViewBtn = document.getElementById('month-view-btn');
    const weekViewBtn = document.getElementById('week-view-btn');
    const dayViewBtn = document.getElementById('day-view-btn');

    // عناصر العروض
    const monthView = document.getElementById('month-view');
    const weekView = document.getElementById('week-view');
    const dayView = document.getElementById('day-view');
    const weekDaysHeader = document.getElementById('week-days-header');
    const weekDays = document.getElementById('week-days');
    const dayHeader = document.getElementById('day-header');
    const timeSlots = document.getElementById('time-slots');

    // متغيرات الحالة
    let currentDate = new Date();
    let currentView = 'month'; // month, week, day

    // أسماء الأشهر والأيام بالعربية
    const monthNames = [
        "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
        "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
    ];

    const dayNames = ["الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت"];

    // أحداث عشوائية للعرض التوضيحي
    const demoEvents = [
        { date: new Date(2023, 5, 15), title: "اجتماع فريق العمل", time: "10:00" },
        { date: new Date(2023, 5, 20), title: "موعد مع العميل", time: "14:30" },
        { date: new Date(2023, 5, 10), title: "عطلة نهاية الأسبوع", time: "09:00" }
    ];

    // تبديل العرض
    function switchView(view) {
        currentView = view;
        monthView.style.display = 'none';
        weekView.style.display = 'none';
        dayView.style.display = 'none';

        monthViewBtn.classList.remove('active');
        weekViewBtn.classList.remove('active');
        dayViewBtn.classList.remove('active');

        switch (view) {
            case 'month':
                monthView.style.display = 'block';
                monthViewBtn.classList.add('active');
                renderMonthView();
                break;
            case 'week':
                weekView.style.display = 'block';
                weekViewBtn.classList.add('active');
                renderWeekView();
                break;
            case 'day':
                dayView.style.display = 'block';
                dayViewBtn.classList.add('active');
                renderDayView();
                break;
        }
    }

    // عرض الشهر
    function renderMonthView() {
        const year = currentDate.getFullYear();
        const month = currentDate.getMonth();

        // تحديث عنوان الشهر والسنة
        periodTitleElement.textContent = `${monthNames[month]} ${year}`;

        // الحصول على أول يوم من الشهر
        const firstDay = new Date(year, month, 1);
        const firstDayIndex = firstDay.getDay();

        // الحصول على عدد الأيام في الشهر
        const lastDay = new Date(year, month + 1, 0);
        const daysInMonth = lastDay.getDate();

        // الحصول على عدد الأيام من الشهر السابق
        const prevLastDay = new Date(year, month, 0);
        const daysInPrevMonth = prevLastDay.getDate();

        // الحصول على عدد الأيام من الشهر التالي
        const lastDayIndex = lastDay.getDay();
        const nextDays = 6 - lastDayIndex;

        // مسح أيام التقويم الحالية
        monthDaysElement.innerHTML = '';

        // إضافة أيام الشهر السابق
        for (let i = firstDayIndex; i > 0; i--) {
            const day = daysInPrevMonth - i + 1;
            const dayElement = document.createElement('div');
            dayElement.classList.add('calendar-day', 'other-month');
            dayElement.textContent = day;
            monthDaysElement.appendChild(dayElement);
        }

        // إضافة أيام الشهر الحالي
        const today = new Date();
        for (let i = 1; i <= daysInMonth; i++) {
            const dayElement = document.createElement('div');
            dayElement.classList.add('calendar-day');
            dayElement.textContent = i;

            // تمييز اليوم الحالي
            if (i === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
                dayElement.classList.add('current-day');
            }

            // التحقق من وجود أحداث في هذا اليوم
            const dayEvents = demoEvents.filter(event =>
                event.date.getDate() === i &&
                event.date.getMonth() === month &&
                event.date.getFullYear() === year
            );

            if (dayEvents.length > 0) {
                dayElement.classList.add('has-event');
                const indicator = document.createElement('div');
                indicator.classList.add('event-indicator');
                dayElement.appendChild(indicator);
            }

            monthDaysElement.appendChild(dayElement);
        }

        // إضافة أيام الشهر التالي
        for (let i = 1; i <= nextDays; i++) {
            const dayElement = document.createElement('div');
            dayElement.classList.add('calendar-day', 'other-month');
            dayElement.textContent = i;
            monthDaysElement.appendChild(dayElement);
        }
    }

    // عرض الأسبوع
    function renderWeekView() {
        const year = currentDate.getFullYear();
        const month = currentDate.getMonth();
        const date = currentDate.getDate();
        const day = currentDate.getDay();

        // حساب بداية الأسبوع (الأحد)
        const startOfWeek = new Date(year, month, date - day);

        // تحديث عنوان الفترة
        const endOfWeek = new Date(year, month, date - day + 6);
        periodTitleElement.textContent = `${startOfWeek.getDate()} ${monthNames[startOfWeek.getMonth()]} - ${endOfWeek.getDate()} ${monthNames[endOfWeek.getMonth()]} ${year}`;

        // مسح المحتوى السابق
        weekDaysHeader.innerHTML = '';
        weekDays.innerHTML = '';

        const today = new Date();

        // إضافة عناوين الأيام ومحتوى الأسبوع
        for (let i = 0; i < 7; i++) {
            const currentDay = new Date(year, month, date - day + i);

            // عناوين الأيام
            const dayHeaderElement = document.createElement('div');
            dayHeaderElement.innerHTML = `
                <div>${dayNames[i]}</div>
                <div>${currentDay.getDate()}/${currentDay.getMonth() + 1}</div>
            `;
            weekDaysHeader.appendChild(dayHeaderElement);

            // محتوى اليوم
            const dayElement = document.createElement('div');
            dayElement.classList.add('week-day');

            // تمييز اليوم الحالي
            if (currentDay.getDate() === today.getDate() &&
                currentDay.getMonth() === today.getMonth() &&
                currentDay.getFullYear() === today.getFullYear()) {
                dayElement.classList.add('current-day');
            }

            // التحقق من وجود أحداث في هذا اليوم
            const dayEvents = demoEvents.filter(event =>
                event.date.getDate() === currentDay.getDate() &&
                event.date.getMonth() === currentDay.getMonth() &&
                event.date.getFullYear() === currentDay.getFullYear()
            );

            if (dayEvents.length > 0) {
                const eventsList = document.createElement('div');
                eventsList.style.marginTop = '10px';
                dayEvents.forEach(event => {
                    const eventElement = document.createElement('div');
                    eventElement.innerHTML = `<strong>${event.time}</strong> - ${event.title}`;
                    eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.2)';
                    eventElement.style.padding = '5px';
                    eventElement.style.margin = '5px 0';
                    eventElement.style.borderRadius = '5px';
                    eventElement.style.fontSize = '14px';
                    eventsList.appendChild(eventElement);
                });
                dayElement.appendChild(eventsList);
            }

            weekDays.appendChild(dayElement);
        }
    }

    // عرض اليوم
    function renderDayView() {
        const year = currentDate.getFullYear();
        const month = currentDate.getMonth();
        const date = currentDate.getDate();

        // تحديث عنوان اليوم
        dayHeader.textContent = `${dayNames[currentDate.getDay()]}، ${date} ${monthNames[month]} ${year}`;
        periodTitleElement.textContent = `${date} ${monthNames[month]} ${year}`;

        // مسح المحتوى السابق
        timeSlots.innerHTML = '';

        // إضافة الفترات الزمنية
        for (let hour = 8; hour < 20; hour++) {
            const timeSlot = document.createElement('div');
            timeSlot.classList.add('time-slot');
            timeSlot.innerHTML = `
                <div style="font-weight:bold; margin-bottom:5px;">${hour}:00 - ${hour + 1}:00</div>
            `;

            // التحقق من وجود أحداث في هذه الفترة
            const hourEvents = demoEvents.filter(event =>
                event.date.getDate() === date &&
                event.date.getMonth() === month &&
                event.date.getFullYear() === year &&
                event.date.getHours() === hour
            );

            if (hourEvents.length > 0) {
                hourEvents.forEach(event => {
                    const eventElement = document.createElement('div');
                    eventElement.innerHTML = `<strong>${event.time}</strong> - ${event.title}`;
                    eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.3)';
                    eventElement.style.padding = '8px';
                    eventElement.style.margin = '8px 0';
                    eventElement.style.borderRadius = '5px';
                    timeSlot.appendChild(eventElement);
                });
            }

            timeSlots.appendChild(timeSlot);
        }
    }

    // الانتقال إلى فترة زمنية سابقة
    function goToPrevPeriod() {
        switch (currentView) {
            case 'month':
                currentDate.setMonth(currentDate.getMonth() - 1);
                renderMonthView();
                break;
            case 'week':
                currentDate.setDate(currentDate.getDate() - 7);
                renderWeekView();
                break;
            case 'day':
                currentDate.setDate(currentDate.getDate() - 1);
                renderDayView();
                break;
        }
    }

    // الانتقال إلى فترة زمنية تالية
    function goToNextPeriod() {
        switch (currentView) {
            case 'month':
                currentDate.setMonth(currentDate.getMonth() + 1);
                renderMonthView();
                break;
            case 'week':
                currentDate.setDate(currentDate.getDate() + 7);
                renderWeekView();
                break;
            case 'day':
                currentDate.setDate(currentDate.getDate() + 1);
                renderDayView();
                break;
        }
    }

    // الانتقال إلى اليوم الحالي
    function goToToday() {
        currentDate = new Date();
        switch (currentView) {
            case 'month':
                renderMonthView();
                break;
            case 'week':
                renderWeekView();
                break;
            case 'day':
                renderDayView();
                break;
        }
    }

    // إضافة معالجي الأحداث
    prevPeriodButton.addEventListener('click', goToPrevPeriod);
    nextPeriodButton.addEventListener('click', goToNextPeriod);
    todayFloatingBtn.addEventListener('click', goToToday);

    monthViewBtn.addEventListener('click', () => switchView('month'));
    weekViewBtn.addEventListener('click', () => switchView('week'));
    dayViewBtn.addEventListener('click', () => switchView('day'));

    // إخفاء الزر العائم عند التمرير لأسفل
    let lastScrollPosition = 0;
    window.addEventListener('scroll', function () {
        const currentScrollPosition = window.pageYOffset;
        if (currentScrollPosition > lastScrollPosition) {
            // التمرير لأسفل
            todayFloatingBtn.style.transform = 'translateY(100px)';
        } else {
            // التمرير لأعلى
            todayFloatingBtn.style.transform = 'translateY(0)';
        }
        lastScrollPosition = currentScrollPosition;
    });

    // عرض التقويم الأولي
    switchView('month');
});