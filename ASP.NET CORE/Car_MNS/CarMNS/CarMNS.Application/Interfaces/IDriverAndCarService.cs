using CarMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.Interfaces
{
    public interface IDriverAndCarService : IDisposable
    {
        List<CAR> GetAllCar();
        CAR UpdateCarr(CAR car);
        CAR AddCarr(CAR car);
        void DeleteCarr(int id);
        CAR GetCarById(int id);
        CAR GetCarByBienSo(string bienso);

        List<LAI_XE> GetAllLaiXe();
        LAI_XE UpdateLaiXe(LAI_XE lx);
        LAI_XE AddLaiXe(LAI_XE lx);
        void Delete_LaiXe(int id);
        LAI_XE GetLaiXeById(int id);

        List<LAI_XE_CAR> LaixeCars();
        LAI_XE_CAR AddLaiXeCar(LAI_XE_CAR lxeCar);
        LAI_XE_CAR UpdateLaiXeCar(LAI_XE_CAR lxeCar);
        void DeleteLaiXeCar(int id);
        LAI_XE_CAR GetLaiXeCarById(int id);

        void Save();
    }
}
