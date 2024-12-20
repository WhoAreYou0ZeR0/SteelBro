// Функция для инициализации обработчиков событий
function initializeEventListeners() {
    // Получаем WorkerId из data-* атрибута
    const workerId = document.querySelector('[data-worker-id]').getAttribute('data-worker-id');

    // Обработчик для кнопки обновления роли
    document.getElementById('update-role-btn')?.addEventListener('click', function () {
        updateRole(workerId, 'role-select');
    });

    // Обработчик для кнопки обновления должности
    document.getElementById('update-post-btn')?.addEventListener('click', function () {
        updatePost(workerId, 'post-select');
    });
}

// Обновление роли
async function updateRole(workerId, roleSelectId) {
    const newRole = document.getElementById(roleSelectId).value;

    const response = await fetch('/Manager/UpdateWorkerRole', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            WorkerId: workerId,
            NewRole: newRole
        })
    });

    const result = await response.json();
    if (result.success) {
        alert(result.message);
        document.getElementById('role').innerText = result.updatedRole;
    } else {
        alert(result.message);
    }
}

// Обновление должности
async function updatePost(workerId, postSelectId) {
    const newPost = document.getElementById(postSelectId).value;

    const response = await fetch('/Manager/UpdateWorkerPost', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            WorkerId: workerId,
            NewPost: newPost
        })
    });

    const result = await response.json();
    if (result.success) {
        alert(result.message);
        document.getElementById('post').innerText = result.updatedPost;
    } else {
        alert(result.message);
    }
}

// Вызов функции инициализации после загрузки DOM
document.addEventListener('DOMContentLoaded', initializeEventListeners);