let clients = [];

// Инициализация
document.addEventListener('DOMContentLoaded', () => {
    const searchButton = document.getElementById('find_btn');
    searchButton.addEventListener('click', searchClient);
    loadClients();
});

// Загрузка клиентов
async function loadClients(clientName = '') {
    const response = await fetch(`/Manager/GetClients?clientName=${clientName}`);
    clients = await response.json();

    const tableBody = document.getElementById('clients-table-body');
    tableBody.innerHTML = '';

    clients.forEach(client => {
        const row = document.createElement('tr');
        row.addEventListener('click', () => viewClient(client.clientId));
        row.innerHTML = `
                <td>${client.clientId}</td>
                <td>${client.fullName}</td>
                <td>${client.phoneNumber}</td>
                <td>${client.email}</td>
            `;
        tableBody.appendChild(row);
    });
}

// Поиск по клиенту
function searchClient() {
    const clientName = document.getElementById('client-search').value;
    loadClients(clientName);
}

// Просмотр клиента
function viewClient(clientId) {
    window.location.href = `/Manager/ViewClient/${clientId}`;
}