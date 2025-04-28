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

    // عناصر الـ Modal
    const modal = document.getElementById('event-modal');
    const closeModal = document.querySelector('.close-modal');
    const modalTitle = document.getElementById('modal-title');
    const modalDate = document.getElementById('modal-date');
    const modalTime = document.getElementById('modal-time');
    const modalLocation = document.getElementById('modal-location');
    const modalDescription = document.getElementById('modal-description');
    const modalEditBtn = document.getElementById('edit-event');
    const modalDeleteBtn = document.getElementById('delete-event');

   
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
    let demoEvents = [];

    function editEvent(id) {
        window.location.href = `/MyEvents/Edit/${id}`
    }
    function deleteEvent(id) {
        window.location.href = `/MyEvents/Delete/${id}`
    }
    async function fetchEvents() {
        try {
            const response = await fetch('/MyEvents/GetDatesToCalender');
            if (!response.ok) {
                throw new Error('خطأ في جلب الأحداث');
            }
            const data = await response.json();

            // تحويل التواريخ إلى كائنات Date حقيقية
            demoEvents = data.map(event => ({
                id: event.id,
                title: event.name,
                location: event.placeName,
                description: event.discription,
                date: new Date(event.startDate),
                time: event.startTime
            }));

            // بعد ما يتم الجلب بنجاح، اعرض التقويم
            switchView('month');
            console.log(demoEvents);
        } catch (error) {
            console.error('فشل في جلب الأحداث:', error);
        }
    }

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

                if (dayEvents.length > 1) {
                    const eventCount = document.createElement('div');
                    eventCount.classList.add('event-count');
                    eventCount.textContent = dayEvents.length;
                    dayElement.appendChild(eventCount);
                }

                // إضافة tooltip للأحداث
                const tooltip = createEventTooltip(dayEvents);
                dayElement.appendChild(tooltip);

                // إظهار/إخفاء tooltip عند التمرير
                dayElement.addEventListener('mouseenter', () => {
                    tooltip.style.visibility = 'visible';
                    tooltip.style.opacity = '1';
                });

                dayElement.addEventListener('mouseleave', () => {
                    tooltip.style.visibility = 'hidden';
                    tooltip.style.opacity = '0';
                });

                // النقر لعرض تفاصيل الحدث
                dayElement.addEventListener('click', (e) => {
                    e.stopPropagation();
                    if (dayEvents.length === 1) {
                        showEventDetails(dayEvents[0]);
                    } else {
                        // إذا كان هناك أكثر من حدث في اليوم، عرض قائمة للاختيار
                        const choice = prompt(`هناك ${dayEvents.length} أحداث في هذا اليوم. أدخل رقم الحدث الذي تريد عرضه:\n\n${dayEvents.map((event, idx) => `${idx + 1}: ${event.time} - ${event.title}`).join('\n')
                            }\n\nأدخل رقم الحدث:`);

                        if (choice && choice >= 1 && choice <= dayEvents.length) {
                            showEventDetails(dayEvents[choice - 1]);
                        }
                    }
                });
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
                dayElement.classList.add('has-events');
                dayEvents.forEach(event => {
                    const eventElement = document.createElement('div');
                    eventElement.innerHTML = `<strong>${event.time}</strong> - ${event.title}`;
                    eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.2)';
                    eventElement.style.padding = '5px';
                    eventElement.style.margin = '5px 0';
                    eventElement.style.borderRadius = '5px';
                    eventElement.style.fontSize = '14px';
                    eventElement.style.cursor = 'pointer';

                    // إضافة تأثير hover
                    eventElement.addEventListener('mouseenter', () => {
                        eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.5)';
                    });

                    eventElement.addEventListener('mouseleave', () => {
                        eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.2)';
                    });

                    // النقر لعرض تفاصيل الحدث
                    eventElement.addEventListener('click', () => {
                        showEventDetails(event);
                    });

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
                parseInt(event.time.split(':')[0]) === hour
            );

            if (hourEvents.length > 0) {
                timeSlot.classList.add('has-event');
                hourEvents.forEach(event => {
                    const eventElement = document.createElement('div');
                    eventElement.innerHTML = `<strong>${event.time}</strong> - ${event.title}`;
                    eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.3)';
                    eventElement.style.padding = '8px';
                    eventElement.style.margin = '8px 0';
                    eventElement.style.borderRadius = '5px';
                    eventElement.style.cursor = 'pointer';

                    // إضافة تأثير hover
                    eventElement.addEventListener('mouseenter', () => {
                        eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.6)';
                    });

                    eventElement.addEventListener('mouseleave', () => {
                        eventElement.style.backgroundColor = 'rgba(156, 136, 255, 0.3)';
                    });

                    // النقر لعرض تفاصيل الحدث
                    eventElement.addEventListener('click', () => {
                        showEventDetails(event);
                    });

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

    // دالة لإنشاء عنصر تلميح (tooltip) للأحداث
    function createEventTooltip(events) {
        const tooltip = document.createElement('div');
        tooltip.className = 'event-tooltip';

        const eventsList = events.map(event =>
            `<div class="event-tooltip-item">${event.time} - ${event.title}</div>`
        ).join('');

        tooltip.innerHTML = eventsList;
        return tooltip;
    }

    // دالة لعرض تفاصيل الحدث في Modal
    function showEventDetails(event) {
        modalTitle.textContent = event.title;
        modalDate.textContent = `${event.date.getDate()}/${event.date.getMonth() + 1}/${event.date.getFullYear()}`;
        modalTime.textContent = event.time;
        modalLocation.textContent = event.location;
        modalDescription.textContent = event.description;
        modalEditBtn.onclick = () => {
            window.location.href = `/MyEvents/Edit/${event.id}`
        }
        modalDeleteBtn.onclick = () => {
            window.location.href = `/MyEvents/Delete/${event.id}`
        }
        modal.style.display = 'block';
    }

    // إغلاق Modal عند النقر على X
    closeModal.addEventListener('click', () => {
        modal.style.display = 'none';
    });

    // إغلاق Modal عند النقر خارج المحتوى
    window.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.style.display = 'none';
        }
    });

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
    fetchEvents();
});