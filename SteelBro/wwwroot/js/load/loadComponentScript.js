document.addEventListener('DOMContentLoaded', () => {
    // Найти кнопку "Найти" и добавить обработчик
    const searchButton = document.getElementById('search-component-btn');
    searchButton.addEventListener('click', searchComponent);

    // Найти кнопку "Добавить" и добавить обработчик
    const addButton = document.getElementById('add-component-btn');
    addButton.addEventListener('click', addComponent);

    loadComponents();
});


let components = [];

// Загрузка компонентов
async function loadComponents(componentName = '') {
    const response = await fetch(`/Technician/GetComponents?componentName=${componentName}`);
    components = await response.json();

    const tableBody = document.getElementById('components-table-body');
    tableBody.innerHTML = '';

    components.forEach(component => {
        const row = document.createElement('tr');
        row.addEventListener('click', () => viewComponent(component.componentId));
        row.innerHTML = `
            <td>${component.componentId}</td>
            <td>${component.componentName}</td>
            <td>${component.quantity}</td>
            <td>${component.price}</td>
        `;
        tableBody.appendChild(row);
    });
}

// Поиск по названию
function searchComponent() {
    const componentName = document.getElementById('component-search').value;
    loadComponents(componentName);
}

// Просмотр компонента
function viewComponent(componentId) {
    window.location.href = `/Technician/ViewComponent/${componentId}`;
}

// Добавление нового компонента
function addComponent() {
    window.location.href = '/Technician/AddComponent';
}

