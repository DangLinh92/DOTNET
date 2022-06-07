using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMNS.Application.Implementation
{
    public class HmrsBackgroundService : BaseService, IBackgroundService
    {
        private IRespository<NHANVIEN_CALAMVIEC, int> _nhanvienClviecRepository;
        private IRespository<DM_CA_LVIEC, string> _dmCalamviecResponsitory;
        private IRespository<SETTING_TIME_CA_LVIEC, int> _settingTimeCalamviec;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger _logger;

        public HmrsBackgroundService(IRespository<NHANVIEN_CALAMVIEC, int> respository, IRespository<DM_CA_LVIEC, string> dmCalamviecRespository, IRespository<SETTING_TIME_CA_LVIEC, int> settingTimeCalamviec, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienClviecRepository = respository;
            _dmCalamviecResponsitory = dmCalamviecRespository;
            _settingTimeCalamviec = settingTimeCalamviec;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DoWork(CancellationToken stoppingToken, ILogger logger)
        {
            _logger = logger;

            // _logger.LogInformation("Background Service Hosted Service is working."+DateTime.Now);
            if (DateTime.Now.ToString("HH:mm:ss") == "01:00:00")
            {
                _logger.LogInformation("DoWork: active" + DateTime.Now.ToString("HH:mm:ss"));
                SettingTimeCaLviec();
            }
            await Task.Delay(new TimeSpan(0, 0, 1));
        }

        private void SettingTimeCaLviec()
        {
            try
            {
                // het thoi gian ca lam viec se chuyen qua Inactive
                var timeCLviec = _settingTimeCalamviec.FindAll().ToList();
                foreach (var item in timeCLviec)
                {
                    if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.NgayKetThuc) > 0 && item.Status != Status.InActive.ToString())
                    {
                        item.Status = Status.InActive.ToString();
                        _settingTimeCalamviec.Update(item);
                        _logger.LogInformation("f:SettingTimeCaLviec: update status = inactive of setting ca lam viec success");
                    }
                    else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.NgayKetThuc) <= 0 &&
                            string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.NgayBatDau) >= 0 &&
                            item.Status != Status.Active.ToString())
                    {
                        item.Status = Status.Active.ToString();
                        _settingTimeCalamviec.Update(item);
                        _logger.LogInformation("f:SettingTimeCaLviec: update status = active of setting ca lam viec success");
                    }
                }

                var nvClviec = _nhanvienClviecRepository.FindAll().ToList();
                foreach (var item in nvClviec)
                {
                    if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.KetThuc_TheoCa) > 0 && item.Status != Status.InActive.ToString())
                    {
                        item.Status = Status.InActive.ToString();
                        _nhanvienClviecRepository.Update(item);
                        _logger.LogInformation("f:SettingTimeCaLviec: update status = inactive of nhan vien ca lam viec success");
                    }
                    else if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.KetThuc_TheoCa) <= 0
                        && string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.BatDau_TheoCa) >= 0 
                        && item.Status != Status.Active.ToString())
                    {
                        item.Status = Status.Active.ToString();
                        _nhanvienClviecRepository.Update(item);
                        _logger.LogInformation("f:SettingTimeCaLviec: update status = inactive of nhan vien ca lam viec success");
                    }
                }

                _unitOfWork.Commit();

                foreach (var item in timeCLviec)
                {
                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }

                foreach (var item in nvClviec)
                {
                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERR: f:SettingTimeCaLviec: " + ex.Message);
            }
        }
    }
}
