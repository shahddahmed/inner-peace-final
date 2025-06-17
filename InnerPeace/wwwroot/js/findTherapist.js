document.addEventListener("DOMContentLoaded", function() {
    // DOM Elements
    const steps = document.querySelectorAll('.step');
    const optionButtons = document.querySelectorAll('.option-button');
    const nextBtn = document.getElementById('nextBtn');
    const prevBtn = document.getElementById('prevBtn');
    const stepTitle = document.getElementById('step-title');
    const progressBar = document.querySelector('.progress-bar > #progress'); // More specific selector

    // State
    let currentStep = 0;
    const selectedOptions = Array(steps.length).fill().map(() => []);

    // Initialize
    updateDisplay();

    // Option Selection Handler
    optionButtons.forEach(button => {
        button.addEventListener('click', function() {
            const step = this.closest('.step');
            if (!step) return; // Safety check

            const stepIndex = parseInt(step.dataset.step) - 1;
            const value = this.dataset.value;

            // Step 1: Multiple selections allowed
            if (stepIndex === 0) {
                this.classList.toggle('selected');
                if (this.classList.contains('selected')) {
                    selectedOptions[stepIndex].push(value);
                } else {
                    selectedOptions[stepIndex] = selectedOptions[stepIndex].filter(v => v !== value);
                }
            }
            // Steps 2-3: Single selection only
            else {
                // Clear all selections in current step
                const buttons = step.querySelectorAll('.option-button');
                buttons.forEach(btn => btn.classList.remove('selected'));

                // Select current button
                this.classList.add('selected');
                selectedOptions[stepIndex] = [value];
            }

            updateNavButtons();
        });
    });

    // Navigation Functions
    nextBtn.addEventListener('click', nextStep);
    prevBtn.addEventListener('click', prevStep);

    function nextStep() {
        if (currentStep < steps.length - 1) {
            currentStep++;
            updateDisplay();
        } else {
            // Form submission
            const formattedSelections = selectedOptions.map((arr, index) =>
                index === 0 ? arr : arr[0] // Keep array for step 1, single value for others
            );
            let specialization = formattedSelections[0].join(",");
            let gender = formattedSelections[2];
            
            let url = `/therapist?specializationFilter=${encodeURIComponent(specialization)}`;

            if (gender === "male" || gender === "female") {
                url += `&gender=${encodeURIComponent(gender)}`;
            }
            
            window.location.href = url;
        }
    }

    function prevStep() {
        if (currentStep > 0) {
            currentStep--;
            updateDisplay();
            // Restore selections when going back
            const currentSelections = selectedOptions[currentStep];
            if (currentSelections.length > 0) {
                const currentStepEl = steps[currentStep];
                currentStepEl.querySelectorAll('.option-button').forEach(btn => {
                    if (currentSelections.includes(btn.dataset.value)) {
                        btn.classList.add('selected');
                    }
                });
            }
        }
    }

    function updateDisplay() {
        // Update step visibility
        steps.forEach((step, index) => {
            step.classList.toggle('active', index === currentStep);
        });

        // Update progress
        if (progressBar) {
            progressBar.style.width = `${((currentStep + 1) / steps.length) * 100}%`;
        }

        // Update title
        const stepTitles = [
            "Tell us what you're going through?",
            "How long have you been dealing with these issues?",
            "What is the therapist gender that you prefer?"
        ];
        stepTitle.textContent = `Step ${currentStep + 1}: ${stepTitles[currentStep] || ''}`;

        updateNavButtons();
    }

    function updateNavButtons() {
        if (!prevBtn || !nextBtn) return;

        prevBtn.disabled = currentStep === 0;
        nextBtn.disabled = selectedOptions[currentStep].length === 0;
        nextBtn.textContent = currentStep === steps.length - 1 ? 'Submit' : 'Next';
    }
});