using Microsoft.AspNetCore.Http;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Infrastructure.Interfaces;
using OPERATION_MNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OPERATION_MNS.Application.Implementation
{
    public class WaitimeService : BaseService, IWaitimeService
    {
        private IRespository<INVENTORY_ACTUAL, int> _InventoryActualRepository;
        private IUnitOfWork _unitOfWork;
        public WaitimeService(IRespository<INVENTORY_ACTUAL, int> InventoryActualRepository,
                              IUnitOfWork unitOfWork,
                              IHttpContextAccessor httpContextAccessor)
        {
            _InventoryActualRepository = InventoryActualRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        List<string> arrWall = new List<string>() { "OP30500", "OP30700", "OP31000", "OP34000", "OP35000", "OP36000", "OP37000", "OP37500", "OP38000", "OP39000" };
        List<string> arrRoof = new List<string>() { "OP39500", "OP40000", "OP40500", "OP41000", "OP43000", "OP44000", "OP45000", "OP46000", "OP46500", "OP47000", "OP47100", "OP48000", "OP48500", "OP49000", "OP50000", "OP51000" };
        List<string> arrPlate = new List<string>() { "OP52500", "OP53000", "OP55000", "OP56000", "OP56500", "OP57000", "OP58000", "OP59000" };
        List<string> arrSeed = new List<string>() { "OP52000" };
        List<string> arrPlated = new List<string>() { "OP59500", "OP60000", "OP61000", "OP65000", "OP62000", "OP62500", "OP63000", "OP64000", "OP69000", "OP69500", "OP70000" };
        List<string> arrProbe = new List<string>() { "OP70500", "OP71000", "OP72000" };
        List<string> arrDicing = new List<string>() {"OP75000","OPS0300","OPT1500","OP76000","OPT1000","OP77000","OPT2000","OP78000","OPT3000","OP79000","OP01100","OP02000"};
        List<string> arrChipInspection = new List<string>() { "OP05000", "OP04000" };
        List<string> arrReel = new List<string>() { "OP06000", "OP06500", "OP07000", "OP06700", "OP08000", "OP10000", "OP09000", "OP11000" };

        public List<OperationWaitimeSheet> GetWaitTime_WLP1()
        {
            return GenerateWaitTime();
        }

        private List<OperationWaitimeSheet> GenerateWaitTime()
        {
            List<OperationWaitimeSheet> sheets = new List<OperationWaitimeSheet>();

            OperationWaitimeSheet grid_Wall = new OperationWaitimeSheet() { GridName = "grid_Wall" };
            OperationWaitimeSheet grid_Roof = new OperationWaitimeSheet() { GridName = "grid_Roof" };
            OperationWaitimeSheet grid_Plate = new OperationWaitimeSheet() { GridName = "grid_Plate" };
            OperationWaitimeSheet grid_Seed = new OperationWaitimeSheet() { GridName = "grid_Seed" };
            OperationWaitimeSheet grid_Plateted = new OperationWaitimeSheet() { GridName = "grid_Plateted" };
            OperationWaitimeSheet grid_Probe = new OperationWaitimeSheet() { GridName = "grid_Probe" };
            OperationWaitimeSheet grid_Dicing = new OperationWaitimeSheet() { GridName = "grid_Dicing" };
            OperationWaitimeSheet grid_Inspection = new OperationWaitimeSheet() { GridName = "grid_Inspection" };
            OperationWaitimeSheet grid_ReelPacking = new OperationWaitimeSheet() { GridName = "grid_ReelPacking" };

            ResultDB resultDB = _InventoryActualRepository.ExecProceduce2("PKG_BUSINESS@GET_WAITE_TIME", new Dictionary<string, string>());

            if (resultDB.ReturnInt == 0)
            {
                DataTable table = resultDB.ReturnDataSet.Tables[0];
                WaittimeViewModel waittime;
                foreach (DataRow row in table.Rows)
                {
                    waittime = new WaittimeViewModel()
                    {
                        CassetteID = row["Cassette ID"].NullString(),
                        Material = row["Material"].NullString(),
                        OperationName = row["Operation Name"].NullString(),
                        OperationID = row["Operation ID"].NullString(),
                        Status = row["Status"].NullString(),
                        StayDay = decimal.Parse(row["Stay Day"].IfNullIsZero()),
                        Id = row["Cassette ID"].NullString() + row["Material"].NullString() + row["Operation Name"].NullString() + row["Status"].NullString()
                    };

                    if (arrWall.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Wall.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrRoof.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Roof.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrPlate.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Plate.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrSeed.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Seed.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrPlated.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Plateted.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrProbe.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Probe.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrDicing.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Dicing.lstWaittimeViewModel.Add(waittime);
                    }

                    if (arrChipInspection.Contains(row["Operation ID"].NullString()))
                    {
                        grid_Inspection.lstWaittimeViewModel.Add(waittime);
                    }

                    //if (arrReel.Contains(row["Operation ID"].NullString()))
                    //{
                    //    grid_ReelPacking.lstWaittimeViewModel.Add(waittime);
                    //}
                }

                if(resultDB.ReturnDataSet.Tables.Count > 1)
                {
                    DataTable tableReel = resultDB.ReturnDataSet.Tables[1];
                    foreach (DataRow row in tableReel.Rows)
                    {
                        waittime = new WaittimeViewModel()
                        {
                            LotId = row["Lot ID"].NullString(),
                            Material = row["Material"].NullString(),
                            OperationName = row["Operation Name"].NullString(),
                            OperationID = row["Operation ID"].NullString(),
                            Status = row["Status"].NullString(),
                            StayDay = decimal.Parse(row["Stay Day"].IfNullIsZero()),
                            Id = row["Lot ID"].NullString() + row["Material"].NullString() + row["Operation Name"].NullString() + row["Status"].NullString()
                        };

                        if (arrReel.Contains(row["Operation ID"].NullString()))
                        {
                            grid_ReelPacking.lstWaittimeViewModel.Add(waittime);
                        }
                    }
                }

            }

            sheets.AddRange(new List<OperationWaitimeSheet>() { grid_Wall, grid_Roof, grid_Plate, grid_Seed, grid_Plateted, grid_Probe, grid_Dicing, grid_Inspection, grid_ReelPacking });
            return sheets;
        }

    }
}
