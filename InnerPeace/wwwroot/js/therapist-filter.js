document.addEventListener('DOMContentLoaded', function() {

    const specializationSelect = document.querySelector('select[data-filter="specialization"]');
    const languageCheckboxes = document.querySelectorAll('input[data-filter="language"]');
    const durationRadios = document.querySelectorAll('input[data-filter="duration"]');
    const genderRadios = document.querySelectorAll('input[data-filter="gender"]');
    const feesRange = document.getElementById('feeRange');
    const educationCheckboxes = document.querySelectorAll('input[data-filter="education"]');
    const resetButton = document.querySelector('.reset-button');
    const therapistCards = document.querySelectorAll('.therapist-card');
    
    function matchesFilters(card) {
        if (specializationSelect.value && !card.dataset.specialization.includes(specializationSelect.value)) {
            return false;
        }

        const selectedLanguages = Array.from(languageCheckboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value);
        if (selectedLanguages.length > 0) {
            const cardLanguages = card.dataset.language.split(',');
            if (!selectedLanguages.some(lang => cardLanguages.includes(lang))) {
                return false;
            }
        }
        
        const selectedDuration = document.querySelector('input[data-filter="duration"]:checked').value;
        if (selectedDuration !== 'all' && !card.dataset.duration.includes(selectedDuration)) {
            return false;
        }
        
        const selectedGender = document.querySelector('input[data-filter="gender"]:checked').value;
        if (selectedGender !== 'all' && card.dataset.gender !== selectedGender) {
            return false;
        }

        const maxFees = parseInt(feesRange.value);
        if (parseInt(card.dataset.fees) > maxFees) {
            return false;
        }

        const selectedEducation = Array.from(educationCheckboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value);
        if (selectedEducation.length > 0 && !selectedEducation.includes(card.dataset.education)) {
            return false;
        }

        return true;
    }
    
    function applyFilters() {
        therapistCards.forEach(card => {
            if (matchesFilters(card)) {
                card.style.display = 'flex';
            } else {
                card.style.display = 'none';
            }
        });
    }
    
    specializationSelect.addEventListener('change', applyFilters);
    languageCheckboxes.forEach(cb => cb.addEventListener('change', applyFilters));
    durationRadios.forEach(radio => radio.addEventListener('change', applyFilters));
    genderRadios.forEach(radio => radio.addEventListener('change', applyFilters));
    feesRange.addEventListener('input', applyFilters);
    educationCheckboxes.forEach(cb => cb.addEventListener('change', applyFilters));
    
    resetButton.addEventListener('click', function() {
        specializationSelect.value = '';
        languageCheckboxes.forEach(cb => cb.checked = false);
        document.querySelector('input[data-filter="duration"][value="all"]').checked = true;
        document.querySelector('input[data-filter="gender"][value="all"]').checked = true;
        feesRange.value = 700;
        document.getElementById('rangeValue').textContent = '700';
        educationCheckboxes.forEach(cb => cb.checked = false);
        
        therapistCards.forEach(card => card.style.display = 'flex');
    });
}); 