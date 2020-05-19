using CRM.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface ILogService
    {
        Task<LogDTO> AddLog(LogDTO logDTO);
        Task<LogDTO> GetLog(int LogId);
        Task<IEnumerable<LogDTO>> GetLogs();
        Task<IEnumerable<LogDTO>> GetUserLogs(string UserId);
    }
}
