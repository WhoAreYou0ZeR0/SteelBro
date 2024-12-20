using IniParser;
using IniParser.Model;

namespace SteelBro.Models
{
    public static class IniConfigReader
    {
        public static string GetConnectionString(string iniFilePath)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFilePath);

            // Получаем значение сервера из секции [Database]
            string server = data["Database"]["Server"];

            // Формируем строку подключения
            return $"Server={server};Database=SteelBro;Trusted_Connection=True;TrustServerCertificate=True;";
        }
    }

}
