using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hangHoas = new List<HangHoa>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(hangHoas);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(hangHoas.Find(x => x.MaHangHoa.Equals(Guid.Parse(id))));
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, HangHoa hanghoaEdit)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(x => x.MaHangHoa.Equals(Guid.Parse(id)));
                if (hanghoa == null)
                {
                    return NotFound();
                }

                if (id != hanghoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }

                // update
                hanghoa.TenHangHoa = hanghoaEdit.TenHangHoa;
                hanghoa.DonGia = hanghoaEdit.DonGia;

                return Ok(hanghoa);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(x => x.MaHangHoa.Equals(Guid.Parse(id)));
                if (hanghoa == null)
                {
                    return NotFound();
                }

               hangHoas.Remove(hanghoa);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Create(HangHoa hangHoaVM)
        {
            var hanghoa = new HangHoa()
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaVM.TenHangHoa,
                DonGia = hangHoaVM.DonGia
            };

            hangHoas.Add(hanghoa);

            return Ok(new
            {
                Success = true,
                Data = hanghoa
            });
        }
    }
}
