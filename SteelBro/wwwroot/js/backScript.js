document.addEventListener('DOMContentLoaded', () => {

    const searchButton = document.getElementById('back');
    searchButton.addEventListener('click', goBack);
});

// Функция для возврата на предыдущую страницу
function goBack() {
    window.history.back();
}