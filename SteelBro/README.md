# Проект SteelBro

Добро пожаловать в проект SteelBro! Этот проект представляет собой веб-приложение, разработанное на ASP.NET Core, с поддержкой резервного копирования базы данных и ротации резервных копий.

## Настройка окружения

Для запуска проекта вам потребуется настроить сервер SQL Server и подключить его к приложению. Вот пошаговая инструкция:

### 1. Установка SQL Server

1. Убедитесь, что у вас установлен SQL Server. Если он не установлен, скачайте и установите его с [официального сайта Microsoft](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
2. Установите SQL Server Management Studio (SSMS) для удобного управления базой данных. Скачать SSMS можно [здесь](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

### 2. Создание базы данных

1. Откройте SQL Server Management Studio (SSMS).
2. Подключитесь к вашему серверу SQL Server.
3. Создайте новую базу данных с именем `SteelBro`. Для этого выполните следующий SQL-запрос, который находится в \project\SteelBro\SteelBro\DatabaseTables
4. Настройте файл config.ini, который находится в \project\SteelBro\SteelBro

Вот как выглядит confi.ini:
[Database]
Server=YourServer

5. Замените YourServer на название вашего сервера

6. Можно запускать