using Microsoft.Data.SqlClient;

namespace SteelBro.Models
{
    public class BackupService
    {
        private readonly string _connectionString;

        public BackupService(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Метод создания резервных копий
        public void BackupDatabase(string backupPath)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string backupQuery = $@"
                    BACKUP DATABASE [SteelBro]
                    TO DISK = '{backupPath}'
                    WITH FORMAT,
                    NAME = 'Full Backup of SteelBro';";

                using (SqlCommand command = new SqlCommand(backupQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        // Метод для ротации резервных копий
        public void DeleteOldBackups(string backupDirectory, int daysToKeep)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(backupDirectory);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.LastWriteTime < DateTime.Now.AddDays(-daysToKeep))
                {
                    file.Delete();
                }
            }
        }
    }
}
