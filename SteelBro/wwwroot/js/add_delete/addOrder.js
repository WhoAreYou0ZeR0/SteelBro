// Инициализация обработчиков событий
document.addEventListener('DOMContentLoaded', () => {
    const addComponentButton = document.getElementById('add-component-btn');
    addComponentButton.addEventListener('click', addComponent);

    const addOrderForm = document.getElementById('add-order-form');
    addOrderForm.addEventListener('submit', handleSubmit);

    // Получаем список комплектующих из data-* атрибута
    const componentsData = JSON.parse(document.getElementById('components-data').getAttribute('data-components'));
    window.components = componentsData; // Сохраняем список комплектующих в глобальной переменной
});

let componentCounter = 1;

// Функция для добавления нового комплектующего
function addComponent() {
    componentCounter++;
    const componentsSection = document.getElementById('components-section');

    const newComponentRow = document.createElement('div');
    newComponentRow.className = 'component-row';
    newComponentRow.innerHTML = `
        <div class="form-group">
            <label for="component-${componentCounter}">Комплектующее</label>
            <select id="component-${componentCounter}" name="SelectedComponents[${componentCounter}].ComponentId" required>
                <option value="">Выберите комплектующее</option>
                ${window.components.map(component => `<option value="${component.componentId}">${component.componentName}</option>`).join('')}
            </select>
        </div>

        <div class="form-group">
            <label for="quantity-${componentCounter}">Количество</label>
            <input type="number" id="quantity-${componentCounter}" name="SelectedComponents[${componentCounter}].Quantity" min="1" required />
        </div>
    `;

    componentsSection.appendChild(newComponentRow);
}

// Обработчик отправки формы
async function handleSubmit(e) {
    e.preventDefault();

    const formData = new FormData(e.target);
    const requestData = {
        ClientId: formData.get('ClientId'),
        ServiceId: formData.get('ServiceId'),
        WorkerId: formData.get('WorkerId'),
        SelectedComponents: []
    };

    // Добавляем комплектующие
    for (let i = 0; i <= componentCounter; i++) {
        const componentId = formData.get(`SelectedComponents[${i}].ComponentId`);
        const quantity = formData.get(`SelectedComponents[${i}].Quantity`);

        if (componentId && quantity) {
            requestData.SelectedComponents.push({
                ComponentId: parseInt(componentId),
                Quantity: parseInt(quantity)
            });
        }
    }

    try {
        const response = await fetch('/Manager/AddOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            window.location.href = '/Manager/Clients';
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при добавлении заказа:', error);
        alert('Не удалось добавить заказ. Попробуйте снова.');
    }
}