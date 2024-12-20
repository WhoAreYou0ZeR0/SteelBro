// Инициализация
document.addEventListener('DOMContentLoaded', () => {
    const searchButton = document.getElementById('find_btn');
    searchButton.addEventListener('click', searchClient);
    loadStatuses();
    loadOrders();
});

let orders = [];
let statuses = [];

// Загрузка статусов
async function loadStatuses() {
    const response = await fetch('/Technician/GetStatuses');
    statuses = await response.json();

    const statusFilter = document.getElementById('status-filter');

    statuses.forEach(status => {
        const option = document.createElement('option');
        option.value = status.statusName;
        option.textContent = status.statusName;
        statusFilter.appendChild(option);
    });
}

// Загрузка заказов
async function loadOrders(clientName = '', statusName = 'all') {
    const response = await fetch(`/Technician/GetOrders?clientName=${clientName}&statusName=${statusName}`);
    orders = await response.json();

    const tableBody = document.getElementById('orders-table-body');
    tableBody.innerHTML = '';

    orders.forEach(order => {
        const row = document.createElement('tr');
        row.addEventListener('click', () => viewOrder(order.orderId));
        row.innerHTML = `
            <td>${order.orderId}</td>
            <td>${order.clientName}</td>
            <td>${order.serviceName}</td>
            <td>${order.statusName}</td>
            <td>${new Date(order.orderDate).toLocaleDateString()}</td>
            <td>${order.exDate ? new Date(order.exDate).toLocaleDateString() : 'Не указано'}</td>
        `;
        tableBody.appendChild(row);
    });
}

// Поиск по клиенту
function searchClient() {
    const clientName = document.getElementById('client-search').value;
    const statusName = document.getElementById('status-filter').value;
    loadOrders(clientName, statusName);
}

// Просмотр заказа
function viewOrder(orderId) {
    window.location.href = `/Technician/ViewOrder/${orderId}`;
}
