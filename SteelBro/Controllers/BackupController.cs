using Microsoft.AspNetCore.Mvc;
using SteelBro.Models;

namespace SteelBro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : Controller
    {
        private readonly BackupService _backupService;

        public BackupController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _backupService = new BackupService(connectionString);
        }

        [HttpPost("manual")]
        public IActionResult CreateManualBackup()
        {
            string backupPath = "C:\\Backup\\SteelBro_ManualBackup.bak";
            try
            {
                // Удаляем старые резервные копии
                _backupService.DeleteOldBackups("C:\\Backup", 30); 

                _backupService.BackupDatabase(backupPath);
                return Ok($"Manual backup created successfully at {backupPath}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating manual backup: {ex.Message}");
            }
        }
    }
}
