// Sample appointment data
const appointmentsData = {
    previous: [
        {
            id: 1,
            type: 'consultations',
            therapistName: 'Dr Bahaa mahmoud',
            therapistTitle: 'Psychiatrist',
            date: '2025-03-06',
            time: '04:57 PM',
            duration: '60 Min',
            status: 'Completed',
            image: 'images/bahaa.webp',
            prescription: 'Antidepressants - Take once daily before bed',
            notes: 'Follow-up required in 2 weeks'
        },
        {
            id: 2,
            type: 'support groups',
            therapistName: 'Dr Noha Ahmed',
            therapistTitle: 'Psychologist',
            date: '2025-01-23',
            time: '07:34 PM',
            duration: '90 Min',
            status: 'Completed',
            image: 'images/noha.webp'
        },
        {
            id: 3,
            type: 'programs',
            therapistName: 'Dr Ola Mostafa',
            therapistTitle: 'Mental Health Specialist',
            date: '2025-02-15',
            time: '02:00 PM',
            duration: '45 Min',
            status: 'Completed',
            image: 'images/ola.png'
        }
    ],
    current: [
        {
            id: 4,
            type: 'consultations',
            therapistName: 'Dr Mohamed Ali',
            therapistTitle: 'Psychiatrist',
            date: '2025-05-10',
            time: '10:00 AM',
            duration: '60 Min',
            status: 'Scheduled',
            image: 'images/mohamed.png'
        },
        {
            id: 5,
            type: 'support groups',
            therapistName: 'Dr Germeen Nabil',
            therapistTitle: 'Psychologist',
            date: '2025-05-12',
            time: '03:00 PM',
            duration: '90 Min',
            status: 'Scheduled',
            image: 'images/germeen.jpeg'
        },
        {
            id: 6,
            type: 'programs',
            therapistName: 'Dr Fatima AlYoussef',
            therapistTitle: 'Mental Health Specialist',
            date: '2025-05-15',
            time: '11:00 AM',
            duration: '45 Min',
            status: 'Scheduled',
            image: 'images/fatima.jpg'
        }
    ]
};

// Language translations
const translations = {
    en: {
        tabButtons: {
            previous: "Previous",
            current: "Current"
        },
        filterButtons: {
            all: "All",
            consultations: "Consultations",
            supportGroups: "Support Groups",
            programs: "Programs"
        },
        status: {
            completed: "Completed",
            scheduled: "Scheduled",
            cancelled: "Cancelled"
        },
        actions: {
            bookAgain: "Book Again",
            leaveReview: "Leave Review",
            reschedule: "Reschedule",
            cancel: "Cancel"
        },
        review: {
            title: "Write a Review",
            communication: "Communication",
            understanding: "Understanding of the situation",
            solutions: "Providing effective solutions",
            overall: "Overall experience",
            placeholder: "Write your review here...",
            submit: "Submit Review"
        },
        noAppointments: {
            text: "No {filter} found",
            sub: "Check back later for upcoming appointments"
        },
        appointmentDetails: {
            title: "Title",
            type: "Type",
            date: "Date",
            time: "Time",
            duration: "Duration",
            status: "Status",
            prescription: "Medical Prescription",
            notes: "Notes"
        },
        welcome: "nou",
        searchPlaceholder: "Search...",
        languageToggle: "العربية"
    },
    ar: {
        tabButtons: {
            previous: "السابقة",
            current: "الحالية"
        },
        filterButtons: {
            all: "الكل",
            consultations: "الاستشارات",
            supportGroups: "مجموعات الدعم",
            programs: "البرامج"
        },
        status: {
            completed: "مكتملة",
            scheduled: "مجدولة",
            cancelled: "ملغاة"
        },
        actions: {
            bookAgain: "احجز مرة أخرى",
            leaveReview: "اترك تقييماً",
            reschedule: "إعادة جدولة",
            cancel: "إلغاء"
        },
        review: {
            title: "اكتب تقييماً",
            communication: "التواصل",
            understanding: "فهم الحالة",
            solutions: "تقديم حلول فعالة",
            overall: "التجربة العامة",
            placeholder: "اكتب تقييمك هنا...",
            submit: "إرسال التقييم"
        },
        noAppointments: {
            text: "لا توجد {filter}",
            sub: "تحقق لاحقًا من المواعيد القادمة"
        },
        appointmentDetails: {
            title: "اللقب",
            type: "النوع",
            date: "التاريخ",
            time: "الوقت",
            duration: "المدة",
            status: "الحالة",
            prescription: "الوصفة الطبية",
            notes: "ملاحظات"
        },
        welcome: "نو",
        searchPlaceholder: "بحث...",
        languageToggle: "English"
    }
};

// Current state
let currentTab = 'previous';
let currentAppointment = null;
let currentLang = 'en'; // Default language

// DOM Elements
const tabButtons = document.querySelectorAll('.tab-btn');
const appointmentsList = document.querySelector('.appointments-list');
const reviewModal = document.querySelector('#reviewModal');
const reviewForm = document.querySelector('#reviewForm');
const modalClose = document.querySelector('.modal-close');
const stars = document.querySelectorAll('.star');
const appointmentIdInput = document.querySelector('#appointmentId');
const languageToggle = document.querySelector('.arabic-link a');
const searchInput = document.querySelector('.search-bar input');
const usernameElements = document.querySelectorAll('.username');

// Function to update UI language
function updateLanguage(lang) {
    document.documentElement.lang = lang;
    document.documentElement.dir = lang === 'ar' ? 'rtl' : 'ltr';
    // Update tab buttons
    tabButtons.forEach(button => {
        const tabType = button.dataset.tab;
        button.textContent = translations[lang].tabButtons[tabType];
    });

    // Update review modal
    document.querySelector('.modal-title').textContent = translations[lang].review.title;
    document.querySelectorAll('.rating-group label').forEach((label, index) => {
        const ratingTypes = ['communication', 'understanding', 'solutions', 'overall'];
        label.textContent = translations[lang].review[ratingTypes[index]];
    });
    reviewForm.querySelector('textarea').placeholder = translations[lang].review.placeholder;
    reviewForm.querySelector('button[type="submit"]').textContent = translations[lang].review.submit;
    
    // Update username
    usernameElements.forEach(element => {
        element.textContent = translations[lang].welcome;
    });
}

// Rating state
const ratings = {
    communication: 0,
    understanding: 0,
    solutions: 0,
    overall: 0
};

// Add new DOM elements for details modal
const detailsModal = document.createElement('div');
detailsModal.className = 'details-modal';
detailsModal.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
detailsModal.innerHTML = `
    <div class="details-modal-content">
        <button class="details-modal-close" aria-label="Close details">&times;</button>
        <div class="details-content"></div>
    </div>
`;
document.body.appendChild(detailsModal);

// Function to show appointment details
function showAppointmentDetails(appointment) {
    detailsModal.style.display = 'block';
    document.body.style.overflow = 'hidden';

    const t = translations[currentLang];
    const detailsContent = detailsModal.querySelector('.details-content');
    detailsContent.innerHTML = `
        <div class="appointment-details">
            <img src="${appointment.image}" alt="${appointment.therapistName}" class="therapist-image">            <h2>${appointment.therapistName}</h2>
            <p><strong>${t.appointmentDetails.title}:</strong> ${appointment.therapistTitle}</p>
            <p><strong>${t.appointmentDetails.date}:</strong> ${appointment.date}</p>
            <p><strong>${t.appointmentDetails.time}:</strong> ${appointment.time}</p>
            <p><strong>${t.appointmentDetails.duration}:</strong> ${appointment.duration}</p>
            <p><strong>${t.appointmentDetails.status}:</strong> <span class="status-badge ${appointment.status.toLowerCase()}">${translateStatus(appointment.status)}</span></p>
            ${appointment.prescription ? `
                <div class="prescription-section">
                    <h3>${t.appointmentDetails.prescription}</h3>
                    <p>${appointment.prescription}</p>
                </div>
            ` : ''}
            ${appointment.notes ? `
                <div class="notes-section">
                    <h3>${t.appointmentDetails.notes}</h3>
                    <p>${appointment.notes}</p>
                </div>
            ` : ''}
        </div>
    `;
}


// Close modal when clicking the close button
const closeButton = detailsModal.querySelector('.details-modal-close');
closeButton.addEventListener('click', () => {
    detailsModal.style.display = 'none';
    document.body.style.overflow = '';
});

// Close modal when clicking outside
detailsModal.addEventListener('click', (e) => {
    if (e.target === detailsModal) {
        detailsModal.style.display = 'none';
        document.body.style.overflow = '';
    }
});

// Event Listeners
tabButtons.forEach(button => {
    button.addEventListener('click', () => {
        tabButtons.forEach(btn => btn.classList.remove('active'));
        button.classList.add('active');
        currentTab = button.dataset.tab;
        displayAppointments();
    });
});

// Filter buttons removed

// Star rating functionality
stars.forEach(star => {
    star.addEventListener('click', () => {
        const value = parseInt(star.dataset.value);
        const ratingType = star.parentElement.dataset.rating;
        const starsContainer = star.parentElement;

        // Update visual state
        starsContainer.querySelectorAll('.star').forEach(s => {
            s.classList.toggle('active', parseInt(s.dataset.value) <= value);
        });

        // Update rating value
        ratings[ratingType] = value;
    });
});

// Modal functionality
modalClose.addEventListener('click', () => {
    reviewModal.classList.remove('active');
    resetReviewForm();
});

reviewForm.addEventListener('submit', (e) => {
    e.preventDefault();

    const review = {
        appointmentId: parseInt(appointmentIdInput.value),
        ratings,
        comment: reviewForm.querySelector('textarea').value
    };

    // In a real application, this would be sent to a server
    console.log('Submitting review:', review);

    // Close modal and reset form
    reviewModal.classList.remove('active');
    resetReviewForm();

    // Show success message
    alert('Thank you for your review!');
});

// Function to reset review form
function resetReviewForm() {
    ratings.communication = 0;
    ratings.understanding = 0;
    ratings.solutions = 0;
    ratings.overall = 0;

    stars.forEach(star => star.classList.remove('active'));
    reviewForm.querySelector('textarea').value = '';
    currentAppointment = null;
}

// Function to handle booking action
function handleBooking(appointment) {
    // Get the first name and handle special cases
    let formattedName = appointment.therapistName.split(' ')[1].toLowerCase();

    // Handle special case for Sheimaa/Shimaa
    if (formattedName === 'sheimaa') {
        formattedName = 'shimaa';
    }

    window.location.href = `booknow-${formattedName}.html`;
}

// Function to display appointments
function displayAppointments() {
    const appointments = appointmentsData[currentTab];
    // Show all appointments since we removed filter options
    const filteredAppointments = appointments;

    const t = translations[currentLang];

    if (filteredAppointments.length === 0) {
        let noAppointmentsText = t.noAppointments.text;
        // Replace filter placeholder with "appointments"
        noAppointmentsText = noAppointmentsText.replace('{filter}', currentLang === 'ar' ? 'مواعيد' : 'appointments');


        appointmentsList.innerHTML = `
            <div class="no-appointments">
                <i class="fas fa-calendar-times"></i>
                <p>${noAppointmentsText}</p>
                <p class="no-appointments-sub">${t.noAppointments.sub}</p>
            </div>
        `;
        return;
    }

    appointmentsList.innerHTML = filteredAppointments.map(appointment => `
        <div class="appointment-card" data-appointment-id="${appointment.id}">
            <div class="appointment-content">
                <div class="therapist-info">
                    <img src="${appointment.image}" alt="${appointment.therapistName}" class="therapist-image">
                    <div class="therapist-details">
                        <h3>${appointment.therapistName}</h3>
                        <p class="therapist-title">${appointment.therapistTitle}</p>
                    </div>
                </div>
                <div class="appointment-time">
                    <div class="time-item">
                        <i class="far fa-calendar"></i>
                        <span>${appointment.date}</span>
                    </div>
                    <div class="time-item">
                        <i class="far fa-clock"></i>
                        <span>${appointment.time}</span>
                    </div>
                    <div class="time-item">
                        <i class="fas fa-hourglass-half"></i>
                        <span>${appointment.duration}</span>
                    </div>                </div>
            </div>
            ${getActionButtons(appointment)}
        </div>
    `).join('');

    // Add click event to the appointment cards
    document.querySelectorAll('.appointment-card').forEach((card, index) => {
        card.addEventListener('click', (e) => {
            // Don't show details if clicking on action buttons
            if (!e.target.classList.contains('action-btn')) {
                const appointment = filteredAppointments[index];
                showAppointmentDetails(appointment);
            }
        });
    });

    // Add event listeners to the newly created buttons
    document.querySelectorAll('.action-btn').forEach(button => {
        button.addEventListener('click', (e) => {
            const appointmentCard = button.closest('.appointment-card');
            const index = Array.from(appointmentsList.children).indexOf(appointmentCard);
            const appointment = filteredAppointments[index];

            if (button.classList.contains('book-again')) {
                handleBooking(appointment);
            } else if (button.classList.contains('review')) {
                currentAppointment = appointment;
                appointmentIdInput.value = appointment.id;
                reviewModal.classList.add('active');
            } else if (button.classList.contains('reschedule')) {
                handleBooking(appointment);
            } else if (button.classList.contains('cancel')) {
                if (confirm('Are you sure you want to cancel this appointment?')) {
                    // In a real application, this would be sent to a server
                    alert('Appointment cancelled successfully');
                    appointment.status = 'Cancelled';
                    displayAppointments();
                }
            }
        });
    });
}

// Helper function to get action buttons based on appointment status
function getActionButtons(appointment) {
    const t = translations[currentLang];

    if (appointment.status === 'Completed') {
        return `
            <div class="appointment-actions">
                <button class="action-btn book-again">${t.actions.bookAgain}</button>
                <button class="action-btn review">${t.actions.leaveReview}</button>
            </div>
        `;
    } else if (appointment.status === 'Scheduled') {
        return `
            <div class="appointment-actions">
                <button class="action-btn reschedule">${t.actions.reschedule}</button>
                <button class="action-btn cancel">${t.actions.cancel}</button>
            </div>
        `;
    }
    return '';
}

displayAppointments();