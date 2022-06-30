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
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ILogger _logger;


        public HmrsBackgroundService(IRespository<NHANVIEN_CALAMVIEC, int> respository, IRespository<HR_NHANVIEN, string> nhanvienRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _nhanvienClviecRepository = respository;
            _nhanvienRepository = nhanvienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DoWork(CancellationToken stoppingToken, ILogger logger)
        {
            _logger = logger;
            _logger.LogInformation("f:SettingTimeCaLviec: begin update status");
            SettingTimeCaLviec();
            await Task.Delay(new TimeSpan(0, 0, 1));
            _logger.LogInformation("f:SettingTimeCaLviec: end update status");
        }

        private void SettingTimeCaLviec()
        {
            try
            {
                // het thoi gian ca lam viec se chuyen qua Inactive
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
                        _logger.LogInformation("f:SettingTimeCaLviec: update status = active of nhan vien ca lam viec success");
                    }
                }

                // update nghi viec nhan vien
                var lstNv = _nhanvienRepository.FindAll();

                foreach (var item in lstNv)
                {
                    if (!string.IsNullOrEmpty(item.NgayNghiViec))
                    {
                        if (string.Compare(DateTime.Now.ToString("yyyy-MM-dd"), item.NgayNghiViec) >= 0)
                        {
                            item.Status = Status.InActive.ToString();
                            _nhanvienRepository.Update(item);
                        }
                    }
                }

                _unitOfWork.Commit();

                foreach (var item in nvClviec)
                {
                    ((Data.EF.EFUnitOfWork)_unitOfWork).DBContext().Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }

                foreach (var item in lstNv)
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
