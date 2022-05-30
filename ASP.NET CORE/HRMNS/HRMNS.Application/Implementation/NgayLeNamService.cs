using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class NgayLeNamService : BaseService, INgayLeNamService
    {
        private IRespository<NGAY_LE_NAM, string> _ngaylenamRepository;
        private IRespository<NGAY_NGHI_BU_LE_NAM, int> _nghiburesponsitory;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NgayLeNamService(IRespository<NGAY_LE_NAM, string> respository, IRespository<NGAY_NGHI_BU_LE_NAM, int> nghiburesponsitory, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _ngaylenamRepository = respository;
            _nghiburesponsitory = nghiburesponsitory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public NgayLeNamViewModel Add(NgayLeNamViewModel ngayleVm)
        {
            ngayleVm.DateCreated = GetUserId();
            var entity = _mapper.Map<NGAY_LE_NAM>(ngayleVm);
            _ngaylenamRepository.Add(entity);
            return ngayleVm;
        }

        public void Delete(string id)
        {
            _ngaylenamRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<NgayLeNamViewModel> GetAll(string keyword)
        {
            if (keyword == "")
            {
                return _mapper.Map<List<NgayLeNamViewModel>>(_ngaylenamRepository.FindAll(x => x.Id.Contains(DateTime.Now.Year.ToString())).OrderBy(x => x.Id));
            }
            else
            {
                DateTime time;
                try
                {
                    time = DateTime.Parse(keyword);
                }
                catch (Exception)
                {

                    time = DateTime.Now;
                }

                return _mapper.Map<List<NgayLeNamViewModel>>(_ngaylenamRepository.FindAll(x => x.Id.Contains(time.Year.ToString())).OrderBy(x => x.Id));
            }
        }

        public NgayLeNamViewModel GetById(string id, params Expression<Func<NGAY_LE_NAM, object>>[] includeProperties)
        {
            return _mapper.Map<NgayLeNamViewModel>(_ngaylenamRepository.FindById(id, includeProperties));
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<NgayLeNamViewModel> Search(string condition, string param)
        {
            throw new NotImplementedException();
        }

        public void Update(NgayLeNamViewModel ngayleVm)
        {
            ngayleVm.DateCreated = GetUserId();
            ngayleVm.DateModified = GetUserId();
            var entity = _mapper.Map<NGAY_LE_NAM>(ngayleVm);
            _ngaylenamRepository.Update(entity);
        }

        // ngay le roi vao 
        public void UpdateNghiBu()
        {
            _nghiburesponsitory.RemoveMultiple(_nghiburesponsitory.FindAll().ToList());
            Save();

            var lst = GetAll("");
            string nextDay = "";
            bool isBreak = true;
            for (int i = 0; i < lst.Count; i++)
            {
                nextDay = DateTime.Parse(lst[i].Id).AddDays(1).ToString("yyyy-MM-dd");

                if (DateTime.Parse(lst[i].Id).DayOfWeek == 0) // chu nhat
                {
                    if (!lst.Any(x => x.Id == nextDay))
                    {
                        if (_nghiburesponsitory.FindSingle(x => x.NgayNghiBu == nextDay) == null)
                        {
                            NGAY_NGHI_BU_LE_NAM nghibuEntity = new NGAY_NGHI_BU_LE_NAM();
                            nghibuEntity.NgayNghiBu = nextDay;
                            nghibuEntity.NoiDungNghi = "Nghi bù ngày chủ nhật : " + lst[i].Id;
                            nghibuEntity.UserCreated = GetUserId();
                            _nghiburesponsitory.Add(nghibuEntity);
                            isBreak = true;
                        }
                    }
                    else
                    {
                        isBreak = false;
                    }
                }
                else
                {
                    if (!isBreak && !lst.Any(x => x.Id == nextDay))
                    {
                        if (_nghiburesponsitory.FindSingle(x => x.NgayNghiBu == nextDay) == null)
                        {
                            NGAY_NGHI_BU_LE_NAM nghibuEntity = new NGAY_NGHI_BU_LE_NAM();
                            nghibuEntity.NgayNghiBu = nextDay;
                            nghibuEntity.NoiDungNghi = "Nghi bù ngày chủ nhật : " + lst[i].Id;
                            nghibuEntity.UserCreated = GetUserId();
                            _nghiburesponsitory.Add(nghibuEntity);
                            isBreak = true;
                        }
                    }
                }
            }
            Save();
        }
    }
}
