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
    [Route("odata/OSPortDB/Architectures")]
    public partial class ArchitecturesController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public ArchitecturesController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.Architecture> GetArchitectures()
        {
            var items = this.context.Architectures.AsQueryable<Osporting.Server.Models.OSPortDB.Architecture>();
            this.OnArchitecturesRead(ref items);

            return items;
        }

        partial void OnArchitecturesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Architecture> items);

        partial void OnArchitectureGet(ref SingleResult<Osporting.Server.Models.OSPortDB.Architecture> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/Architectures(ArchitectureID={ArchitectureID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.Architecture> GetArchitecture(int key)
        {
            var items = this.context.Architectures.Where(i => i.ArchitectureID == key);
            var result = SingleResult.Create(items);

            OnArchitectureGet(ref result);

            return result;
        }
        partial void OnArchitectureDeleted(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureDeleted(Osporting.Server.Models.OSPortDB.Architecture item);

        [HttpDelete("/odata/OSPortDB/Architectures(ArchitectureID={ArchitectureID})")]
        public IActionResult DeleteArchitecture(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .Include(i => i.HardwarePlannings)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Architecture>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnArchitectureDeleted(item);
                this.context.Architectures.Remove(item);
                this.context.SaveChanges();
                this.OnAfterArchitectureDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArchitectureUpdated(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureUpdated(Osporting.Server.Models.OSPortDB.Architecture item);

        [HttpPut("/odata/OSPortDB/Architectures(ArchitectureID={ArchitectureID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutArchitecture(int key, [FromBody]Osporting.Server.Models.OSPortDB.Architecture item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Architecture>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnArchitectureUpdated(item);
                this.context.Architectures.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == key);
                
                this.OnAfterArchitectureUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/Architectures(ArchitectureID={ArchitectureID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchArchitecture(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.Architecture> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Architecture>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnArchitectureUpdated(item);
                this.context.Architectures.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArchitectureCreated(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureCreated(Osporting.Server.Models.OSPortDB.Architecture item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.Architecture item)
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

                this.OnArchitectureCreated(item);
                this.context.Architectures.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == item.ArchitectureID);

                

                this.OnAfterArchitectureCreated(item);

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
