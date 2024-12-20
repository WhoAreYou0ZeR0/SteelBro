// Глобальная переменная для хранения работников
let workers = [];

// Инициализация
document.addEventListener('DOMContentLoaded', () => {
    const searchButton = document.getElementById('find_btn');
    searchButton.addEventListener('click', searchWorker);
    loadWorkers();
});

// Загрузка работников
async function loadWorkers(workerName = '') {
    const response = await fetch(`/Manager/GetWorkers?workerName=${workerName}`);
    workers = await response.json();

    const tableBody = document.getElementById('workers-table-body');
    tableBody.innerHTML = '';

    workers.forEach(worker => {
        const row = document.createElement('tr');
        row.addEventListener('click', () => viewWorker(worker.workerId));
        row.innerHTML = `
            <td>${worker.workerId}</td>
            <td>${worker.fullName}</td>
            <td>${worker.phoneNumber}</td>
            <td>${worker.email}</td>
            <td>${worker.postName}</td>
            <td>${worker.roleName}</td>
        `;
        tableBody.appendChild(row);
    });
}

// Поиск по работнику
function searchWorker() {
    const workerName = document.getElementById('worker-search').value;
    loadWorkers(workerName);
}

// Просмотр работника
function viewWorker(workerId) {
    window.location.href = `/Manager/ViewWorker/${workerId}`;
}