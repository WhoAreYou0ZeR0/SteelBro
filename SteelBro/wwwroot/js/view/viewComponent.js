// Инициализация обработчиков событий
document.addEventListener('DOMContentLoaded', () => {
    const updateQuantityButton = document.getElementById('update-quantity-btn');
    const updatePriceButton = document.getElementById('update-price-btn');
    const deleteComponentButton = document.getElementById('delete-component-btn');

    updateQuantityButton.addEventListener('click', updateComponentQuantity);
    updatePriceButton.addEventListener('click', updateComponentPrice);
    deleteComponentButton.addEventListener('click', deleteComponent);
});

// Функция для изменения количества компонента
async function updateComponentQuantity() {
    const componentId = document.querySelector('[data-component-id]').getAttribute('data-component-id');
    const newQuantity = document.getElementById('component-quantity').value;

    try {
        const response = await fetch('/Technician/UpdateComponentQuantity', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ componentId, newQuantity }),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            document.getElementById('quantity').innerText = result.updatedQuantity;
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при изменении количества:', error);
        alert('Не удалось обновить количество. Попробуйте снова.');
    }
}

// Функция для изменения цены компонента
async function updateComponentPrice() {
    const componentId = document.querySelector('[data-component-id]').getAttribute('data-component-id');
    const newPrice = document.getElementById('component-price').value;

    try {
        const response = await fetch('/Technician/UpdateComponentPrice', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ componentId, newPrice }),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            document.getElementById('price').innerText = result.updatedPrice;
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при изменении цены:', error);
        alert('Не удалось обновить цену. Попробуйте снова.');
    }
}

// Функция для удаления компонента
async function deleteComponent() {
    const componentId = document.querySelector('[data-component-id]').getAttribute('data-component-id');

    if (!confirm('Вы уверены, что хотите удалить этот компонент?')) {
        return;
    }

    try {
        const response = await fetch('/Technician/DeleteComponent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ componentId }),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            window.location.href = '/Technician/Components'; // Перенаправляем на страницу компонентов
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при удалении компонента:', error);
        alert('Не удалось удалить компонент. Попробуйте снова.');
    }
}