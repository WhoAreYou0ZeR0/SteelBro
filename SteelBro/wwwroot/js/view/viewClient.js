// Инициализация обработчиков событий
document.addEventListener('DOMContentLoaded', () => {
    const addOrderButton = document.getElementById('addOrder');
    addOrderButton.addEventListener('click', addOrder);
});

// Функция для добавления заказа
function addOrder() {
    // Получаем ClientId из data-* атрибута
    const clientId = document.querySelector('[data-client-id]').getAttribute('data-client-id');

    // Перенаправляем на страницу добавления заказа с передачей ClientId
    window.location.href = `/Manager/AddOrder?clientId=${clientId}`;
}