using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Osporting.Server.Data;

namespace Osporting.Server.Controllers
{
    public partial class ExportOSPortDBController : ExportController
    {
        private readonly OSPortDBContext context;
        private readonly OSPortDBService service;

        public ExportOSPortDBController(OSPortDBContext context, OSPortDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/OSPortDB/architectures/csv")]
        [HttpGet("/export/OSPortDB/architectures/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArchitecturesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetArchitectures(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/architectures/excel")]
        [HttpGet("/export/OSPortDB/architectures/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArchitecturesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetArchitectures(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/derivatives/csv")]
        [HttpGet("/export/OSPortDB/derivatives/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDerivativesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/derivatives/excel")]
        [HttpGet("/export/OSPortDB/derivatives/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDerivativesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/hardwareplannings/csv")]
        [HttpGet("/export/OSPortDB/hardwareplannings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHardwarePlanningsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetHardwarePlannings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/hardwareplannings/excel")]
        [HttpGet("/export/OSPortDB/hardwareplannings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHardwarePlanningsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetHardwarePlannings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/people/csv")]
        [HttpGet("/export/OSPortDB/people/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPeopleToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPeople(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/people/excel")]
        [HttpGet("/export/OSPortDB/people/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPeopleToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPeople(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/statuses/csv")]
        [HttpGet("/export/OSPortDB/statuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/statuses/excel")]
        [HttpGet("/export/OSPortDB/statuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/testpcs/csv")]
        [HttpGet("/export/OSPortDB/testpcs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestPcsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTestPcs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/OSPortDB/testpcs/excel")]
        [HttpGet("/export/OSPortDB/testpcs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestPcsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTestPcs(), Request.Query, false), fileName);
        }
    }
}
