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
    [Route("odata/OSPortDB/TestPcs")]
    public partial class TestPcsController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public TestPcsController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.TestPc> GetTestPcs()
        {
            var items = this.context.TestPcs.AsQueryable<Osporting.Server.Models.OSPortDB.TestPc>();
            this.OnTestPcsRead(ref items);

            return items;
        }

        partial void OnTestPcsRead(ref IQueryable<Osporting.Server.Models.OSPortDB.TestPc> items);

        partial void OnTestPcGet(ref SingleResult<Osporting.Server.Models.OSPortDB.TestPc> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/TestPcs(TestPCID={TestPCID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.TestPc> GetTestPc(int key)
        {
            var items = this.context.TestPcs.Where(i => i.TestPCID == key);
            var result = SingleResult.Create(items);

            OnTestPcGet(ref result);

            return result;
        }
        partial void OnTestPcDeleted(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcDeleted(Osporting.Server.Models.OSPortDB.TestPc item);

        [HttpDelete("/odata/OSPortDB/TestPcs(TestPCID={TestPCID})")]
        public IActionResult DeleteTestPc(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.TestPcs
                    .Where(i => i.TestPCID == key)
                    .Include(i => i.HardwarePlannings)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.TestPc>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTestPcDeleted(item);
                this.context.TestPcs.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTestPcDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestPcUpdated(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcUpdated(Osporting.Server.Models.OSPortDB.TestPc item);

        [HttpPut("/odata/OSPortDB/TestPcs(TestPCID={TestPCID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTestPc(int key, [FromBody]Osporting.Server.Models.OSPortDB.TestPc item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TestPcs
                    .Where(i => i.TestPCID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.TestPc>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTestPcUpdated(item);
                this.context.TestPcs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestPcs.Where(i => i.TestPCID == key);
                
                this.OnAfterTestPcUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/TestPcs(TestPCID={TestPCID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTestPc(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.TestPc> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TestPcs
                    .Where(i => i.TestPCID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.TestPc>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTestPcUpdated(item);
                this.context.TestPcs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestPcs.Where(i => i.TestPCID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestPcCreated(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcCreated(Osporting.Server.Models.OSPortDB.TestPc item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.TestPc item)
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

                this.OnTestPcCreated(item);
                this.context.TestPcs.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestPcs.Where(i => i.TestPCID == item.TestPCID);

                

                this.OnAfterTestPcCreated(item);

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
