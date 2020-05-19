using AutoMapper;
using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class LogService : ILogService
    {
        ApiContext db;
        public LogService(ApiContext context)
        {
            db = context;
        }
        public async Task<LogDTO> AddLog(LogDTO logDTO)
        {
            Log log = new Log
            {
                Action = logDTO.Action,
                UserId = logDTO.UserId
            };
            await db.Logs.AddAsync(log);
            await db.SaveChangesAsync();
            return logDTO;
        }

        public async Task<LogDTO> GetLog(int LogId)
        {
            var Logs = await GetLogs();
            return Logs.Where(p => p.Id == LogId).FirstOrDefault();
        }

        public async Task<IEnumerable<LogDTO>> GetLogs()
        {
            return await MapRange(await db.Logs.OrderByDescending(p => p.CreatedDate).ToListAsync());
        }

        public async Task<IEnumerable<LogDTO>> GetUserLogs(string UserId)
        {
            var Logs = await GetLogs();
            return Logs.Where(p => p.UserId == UserId);
        }
        public async Task<LogDTO> Map(Log Log)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Log, LogDTO>()).CreateMapper();
            LogDTO LogDTO = mapper.Map<LogDTO>(Log);
            return LogDTO;
        }
        public async Task<IEnumerable<LogDTO>> MapRange(IEnumerable<Log> Logs)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Log, LogDTO>()).CreateMapper();
            IEnumerable<LogDTO> LogDTO = mapper.Map<IEnumerable<LogDTO>>(Logs);
            return LogDTO;
        }
    }
}
