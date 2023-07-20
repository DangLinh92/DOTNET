using CarMNS.Application.Interfaces;
using CarMNS.Data.Entities;
using CarMNS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarMNS.Application.Implementation
{
    public class DriverAndCarService : BaseService, IDriverAndCarService
    {
        private IUnitOfWork _unitOfWork;
        private IRespository<CAR, int> _carRepository;
        private IRespository<LAI_XE, int> _LxeRepository;
        private IRespository<LAI_XE_CAR, int> _LxeCarRepository;
        public DriverAndCarService(IUnitOfWork unitOfWork, IRespository<CAR, int> carRepository, IRespository<LAI_XE, int> lxeRepository, IRespository<LAI_XE_CAR, int> lxeCarRepository, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _carRepository = carRepository;
            _LxeRepository = lxeRepository;
            _LxeCarRepository = lxeCarRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public CAR AddCarr(CAR car)
        {
            _carRepository.Add(car);
            Save();
            return car;
        }

        public LAI_XE AddLaiXe(LAI_XE lx)
        {
            _LxeRepository.Add(lx);
            Save();
            return lx;
        }

        public LAI_XE_CAR AddLaiXeCar(LAI_XE_CAR lxeCar)
        {
            _LxeCarRepository.Add(lxeCar);
            Save();
            return lxeCar;
        }

        public void DeleteCarr(int id)
        {
            _carRepository.Remove(id);
            Save();
        }

        public void DeleteLaiXeCar(int id)
        {
            _LxeCarRepository.Remove(id);
            Save();
        }

        public void Delete_LaiXe(int id)
        {
            _LxeRepository.Remove(id);
            Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<CAR> GetAllCar()
        {
           return _carRepository.FindAll().ToList();
        }

        public List<LAI_XE> GetAllLaiXe()
        {
            return _LxeRepository.FindAll(x=>x.LAI_XE_CAR).ToList();
        }

        public CAR GetCarByBienSo(string bienso)
        {
           return _carRepository.FindAll(x => x.BienSoXe == bienso).FirstOrDefault();
        }

        public CAR GetCarById(int id)
        {
            return _carRepository.FindById(id);
        }

        public LAI_XE GetLaiXeById(int id)
        {
            return _LxeRepository.FindById(id);
        }

        public LAI_XE_CAR GetLaiXeCarById(int id)
        {
           return _LxeCarRepository.FindById(id);
        }

        public List<LAI_XE_CAR> LaixeCars()
        {
            return _LxeCarRepository.FindAll().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public CAR UpdateCarr(CAR car)
        {
            _carRepository.Update(car);
            Save();
            return car;
        }

        public LAI_XE UpdateLaiXe(LAI_XE lx)
        {
            _LxeRepository.Update(lx);
            Save();
            return lx;
        }

        public LAI_XE_CAR UpdateLaiXeCar(LAI_XE_CAR lxeCar)
        {
            _LxeCarRepository.Update(lxeCar);
            Save();
            return lxeCar;
        }
    }
}
