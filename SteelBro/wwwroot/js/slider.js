let slideIndex = 1;
showSlides(slideIndex);

// Автоматическая прокрутка каждые 3 секунды
setInterval(() => {
    plusSlides(1);
}, 3000);

// Функция для переключения слайдов
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Функция для переключения на конкретный слайд
function currentSlide(n) {
    showSlides(slideIndex = n);
}

// Основная функция для отображения слайдов
function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("dot");

    // Проверка на выход за пределы слайдов
    if (n > slides.length) {
        slideIndex = 1;
    }
    if (n < 1) {
        slideIndex = slides.length;
    }

    // Скрываем все слайды
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }

    // Убираем активный класс у всех точек
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    // Показываем текущий слайд и добавляем активный класс к соответствующей точке
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}

// Инициализация обработчиков событий для кнопок и точек
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('prev').addEventListener('click', () => plusSlides(-1));
    document.getElementById('next').addEventListener('click', () => plusSlides(1));

    document.getElementById('dot1').addEventListener('click', () => currentSlide(1));
    document.getElementById('dot2').addEventListener('click', () => currentSlide(2));
    document.getElementById('dot3').addEventListener('click', () => currentSlide(3));
});