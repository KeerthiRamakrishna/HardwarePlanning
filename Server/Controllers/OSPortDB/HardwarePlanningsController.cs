using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Osporting.Server.Controllers.OSPortDB
{
    [Route("odata/OSPortDB/HardwarePlannings")]
    public partial class HardwarePlanningsController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public HardwarePlanningsController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.HardwarePlanning> GetHardwarePlannings()
        {
            var items = this.context.HardwarePlannings.AsQueryable<Osporting.Server.Models.OSPortDB.HardwarePlanning>();
            this.OnHardwarePlanningsRead(ref items);

            return items;
        }

        partial void OnHardwarePlanningsRead(ref IQueryable<Osporting.Server.Models.OSPortDB.HardwarePlanning> items);

        partial void OnHardwarePlanningGet(ref SingleResult<Osporting.Server.Models.OSPortDB.HardwarePlanning> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/HardwarePlannings(HardwarePlanningID={HardwarePlanningID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.HardwarePlanning> GetHardwarePlanning(int key)
        {
            var items = this.context.HardwarePlannings.Where(i => i.HardwarePlanningID == key);
            var result = SingleResult.Create(items);

            OnHardwarePlanningGet(ref result);

            return result;
        }
        partial void OnHardwarePlanningDeleted(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningDeleted(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        [HttpDelete("/odata/OSPortDB/HardwarePlannings(HardwarePlanningID={HardwarePlanningID})")]
        public IActionResult DeleteHardwarePlanning(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.HardwarePlannings
                    .Where(i => i.HardwarePlanningID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.HardwarePlanning>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnHardwarePlanningDeleted(item);
                this.context.HardwarePlannings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterHardwarePlanningDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnHardwarePlanningUpdated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningUpdated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        [HttpPut("/odata/OSPortDB/HardwarePlannings(HardwarePlanningID={HardwarePlanningID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutHardwarePlanning(int key, [FromBody]Osporting.Server.Models.OSPortDB.HardwarePlanning item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.HardwarePlannings
                    .Where(i => i.HardwarePlanningID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.HardwarePlanning>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnHardwarePlanningUpdated(item);
                this.context.HardwarePlannings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HardwarePlannings.Where(i => i.HardwarePlanningID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Derivative,TestPc");
                this.OnAfterHardwarePlanningUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/HardwarePlannings(HardwarePlanningID={HardwarePlanningID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchHardwarePlanning(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.HardwarePlanning> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.HardwarePlannings
                    .Where(i => i.HardwarePlanningID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.HardwarePlanning>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnHardwarePlanningUpdated(item);
                this.context.HardwarePlannings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HardwarePlannings.Where(i => i.HardwarePlanningID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Derivative,TestPc");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnHardwarePlanningCreated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningCreated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.HardwarePlanning item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnHardwarePlanningCreated(item);
                this.context.HardwarePlannings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.HardwarePlannings.Where(i => i.HardwarePlanningID == item.HardwarePlanningID);

                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Derivative,TestPc");

                this.OnAfterHardwarePlanningCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
