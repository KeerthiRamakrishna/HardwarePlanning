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
    [Route("odata/OSPortDB/Statuses")]
    public partial class StatusesController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public StatusesController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.Status> GetStatuses()
        {
            var items = this.context.Statuses.AsQueryable<Osporting.Server.Models.OSPortDB.Status>();
            this.OnStatusesRead(ref items);

            return items;
        }

        partial void OnStatusesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Status> items);

        partial void OnStatusGet(ref SingleResult<Osporting.Server.Models.OSPortDB.Status> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/Statuses(StatusID={StatusID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.Status> GetStatus(int key)
        {
            var items = this.context.Statuses.Where(i => i.StatusID == key);
            var result = SingleResult.Create(items);

            OnStatusGet(ref result);

            return result;
        }
        partial void OnStatusDeleted(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusDeleted(Osporting.Server.Models.OSPortDB.Status item);

        [HttpDelete("/odata/OSPortDB/Statuses(StatusID={StatusID})")]
        public IActionResult DeleteStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Status>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStatusDeleted(item);
                this.context.Statuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusUpdated(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusUpdated(Osporting.Server.Models.OSPortDB.Status item);

        [HttpPut("/odata/OSPortDB/Statuses(StatusID={StatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStatus(int key, [FromBody]Osporting.Server.Models.OSPortDB.Status item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Status>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == key);
                
                this.OnAfterStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/Statuses(StatusID={StatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStatus(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.Status> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Status>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusCreated(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusCreated(Osporting.Server.Models.OSPortDB.Status item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.Status item)
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

                this.OnStatusCreated(item);
                this.context.Statuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == item.StatusID);

                

                this.OnAfterStatusCreated(item);

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
