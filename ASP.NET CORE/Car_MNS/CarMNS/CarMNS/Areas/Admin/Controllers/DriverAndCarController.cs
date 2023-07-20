using CarMNS.Application.Interfaces;
using CarMNS.Data.Entities;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarMNS.Areas.Admin.Controllers
{
    public class DriverAndCarController : AdminBaseController
    {
        private IDriverAndCarService _DriverAndCarService;
        public DriverAndCarController(IDriverAndCarService driverAndCarService)
        {
            _DriverAndCarService = driverAndCarService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Thông tin xe
        [HttpGet]
        public object GetXeAll(DataSourceLoadOptions loadOptions)
        {
            var lstXe = _DriverAndCarService.GetAllCar();
            return DataSourceLoader.Load(lstXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostCar(string values)
        {
            var xe = new CAR();
            JsonConvert.PopulateObject(values, xe);
            xe = _DriverAndCarService.AddCarr(xe);
            return Ok(xe);
        }

        [HttpPut]
        public IActionResult PutCar(int key, string values)
        {
            var xe = _DriverAndCarService.GetCarById(key);

            if (xe != null)
            {
                JsonConvert.PopulateObject(values, xe);
                xe = _DriverAndCarService.UpdateCarr(xe);
                return Ok(xe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteCar(int key)
        {
            _DriverAndCarService.DeleteCarr(key);
        }
        #endregion

        #region Thông tin lái xe
        [HttpGet]
        public object GetLaiXeAll(DataSourceLoadOptions loadOptions)
        {
            var lstLXe = _DriverAndCarService.GetAllLaiXe();
            return DataSourceLoader.Load(lstLXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostLaixe(string values)
        {
            var lxe = new LAI_XE();
            JsonConvert.PopulateObject(values, lxe);
            lxe = _DriverAndCarService.AddLaiXe(lxe);
            return Ok(lxe);
        }

        [HttpPut]
        public IActionResult PutLaixe(int key, string values)
        {
            var Lxe = _DriverAndCarService.GetLaiXeById(key);

            if (Lxe != null)
            {
                JsonConvert.PopulateObject(values, Lxe);
                Lxe = _DriverAndCarService.UpdateLaiXe(Lxe);
                return Ok(Lxe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteLaixe(int key)
        {
            _DriverAndCarService.Delete_LaiXe(key);
        }
        #endregion

        #region Thông tin lái xe - xe
        [HttpGet]
        public object GetAllLXeAndXe(DataSourceLoadOptions loadOptions)
        {
            var lstLXe = _DriverAndCarService.LaixeCars();
            return DataSourceLoader.Load(lstLXe, loadOptions);
        }

        [HttpPost]
        public IActionResult PostLxeXe(string values)
        {
            var lxe = new LAI_XE_CAR();
            JsonConvert.PopulateObject(values, lxe);
            lxe = _DriverAndCarService.AddLaiXeCar(lxe);
            return Ok(lxe);
        }

        [HttpPut]
        public IActionResult PutLxeXe(int key, string values)
        {
            var Lxe = _DriverAndCarService.GetLaiXeCarById(key);

            if (Lxe != null)
            {
                JsonConvert.PopulateObject(values, Lxe);
                Lxe = _DriverAndCarService.UpdateLaiXeCar(Lxe);
                return Ok(Lxe);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public void DeleteLxeXe(int key)
        {
            _DriverAndCarService.DeleteLaiXeCar(key);
        }
        #endregion
    }
}
