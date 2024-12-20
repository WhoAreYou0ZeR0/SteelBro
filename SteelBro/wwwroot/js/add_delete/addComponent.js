// Функция для добавления нового компонента
async function addComponent() {
    const form = document.getElementById('add-component-form');
    const formData = new FormData(form);

    const requestData = {
        ComponentName: formData.get('nameComp'),
        Specifications: formData.get('description'),
        Quantity: parseInt(formData.get('quantity')),
        Price: parseFloat(formData.get('amount'))
    };

    try {
        const response = await fetch('/Technician/AddComponent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData),
        });

        const result = await response.json();

        if (result.success) {
            alert(result.message);
            form.reset(); // Очищаем форму
            window.location.href = '/Technician/Components'; // Перенаправляем на страницу компонентов
        } else {
            alert(`Ошибка: ${result.message}`);
        }
    } catch (error) {
        console.error('Ошибка при добавлении компонента:', error);
        alert('Не удалось добавить компонент. Попробуйте снова.');
    }
}

// Привязываем функцию к форме
document.getElementById('add-component-form').addEventListener('submit', function (e) {
    e.preventDefault();
    addComponent();
});