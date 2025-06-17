document.addEventListener('DOMContentLoaded', function() {
    const cardsWrapper = document.querySelector('.testimonial-cards-wrapper');
    const cards = document.querySelector('.testimonial-cards');
    const dots = document.querySelectorAll('.carousel-dots .dot');
    const cardWidth = document.querySelector('.testimonial-card').offsetWidth;
    const gap = 20; // Same as your gap in CSS
    let currentIndex = 0;
    const visibleCards = 3; // Number of cards visible at once
    
    function updateCarousel() {
        const transformValue = -currentIndex * (cardWidth + gap);
        cards.style.transform = `translateX(${transformValue}px)`;
        
        // Update dots
        dots.forEach((dot, index) => {
            dot.classList.toggle('active', index === currentIndex);
        });
    }
    
    function moveSlide(direction) {
        const totalCards = document.querySelectorAll('.testimonial-card').length;
        currentIndex += direction;
        
        // Boundary checks
        if (currentIndex < 0) {
            currentIndex = 0;
        } else if (currentIndex > totalCards - visibleCards) {
            currentIndex = totalCards - visibleCards;
        }
        
        updateCarousel();
    }
    
    function goToSlide(index) {
        currentIndex = index;
        updateCarousel();
    }
    
    // Button event listeners
    document.querySelector('.carousel-btn.left').addEventListener('click', () => moveSlide(-1));
    document.querySelector('.carousel-btn.right').addEventListener('click', () => moveSlide(1));
    
    // Dot event listeners
    dots.forEach((dot, index) => {
        dot.addEventListener('click', () => goToSlide(index));
    });
    
    // Initialize
    updateCarousel();
    
    // Responsive adjustments
    window.addEventListener('resize', function() {
        const newCardWidth = document.querySelector('.testimonial-card').offsetWidth;
        if (newCardWidth !== cardWidth) {
            updateCarousel();
        }
    });
});