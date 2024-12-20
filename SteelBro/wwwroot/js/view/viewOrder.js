// Инициализация обработчиков событий
document.addEventListener('DOMContentLoaded', () => {
    const changeStatusButton = document.getElementById('change-status-btn');
    changeStatusButton.addEventListener('click', changeStatus);
});

// Функция для изменения статуса заказа
async function changeStatus() {
    const orderId = document.querySelector('[data-order-id]').getAttribute('data-order-id');
    const newStatus = document.getElementById('order-status').value;

    try {
        const response = await fetch('/Manager/UpdateOrderStatus', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ orderId, newStatus }),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            document.getElementById('current-status').innerText = result.updatedStatus;

            // Обновляем строку таблицы в форме списка заказов
            const orderRow = document.querySelector(`#orders-table-body tr[data-order-id="${orderId}"]`);
            if (orderRow) {
                orderRow.querySelector('.order-status').innerText = result.updatedStatus;
            }
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при изменении статуса:', error);
        alert('Не удалось обновить статус. Попробуйте снова.');
    }
}