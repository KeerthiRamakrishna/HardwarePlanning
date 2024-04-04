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
    [Route("odata/OSPortDB/Derivatives")]
    public partial class DerivativesController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public DerivativesController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.Derivative> GetDerivatives()
        {
            var items = this.context.Derivatives.AsQueryable<Osporting.Server.Models.OSPortDB.Derivative>();
            this.OnDerivativesRead(ref items);

            return items;
        }

        partial void OnDerivativesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Derivative> items);

        partial void OnDerivativeGet(ref SingleResult<Osporting.Server.Models.OSPortDB.Derivative> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/Derivatives(DerivativeID={DerivativeID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.Derivative> GetDerivative(int key)
        {
            var items = this.context.Derivatives.Where(i => i.DerivativeID == key);
            var result = SingleResult.Create(items);

            OnDerivativeGet(ref result);

            return result;
        }
        partial void OnDerivativeDeleted(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeDeleted(Osporting.Server.Models.OSPortDB.Derivative item);

        [HttpDelete("/odata/OSPortDB/Derivatives(DerivativeID={DerivativeID})")]
        public IActionResult DeleteDerivative(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Derivatives
                    .Where(i => i.DerivativeID == key)
                    .Include(i => i.HardwarePlannings)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Derivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDerivativeDeleted(item);
                this.context.Derivatives.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDerivativeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDerivativeUpdated(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeUpdated(Osporting.Server.Models.OSPortDB.Derivative item);

        [HttpPut("/odata/OSPortDB/Derivatives(DerivativeID={DerivativeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDerivative(int key, [FromBody]Osporting.Server.Models.OSPortDB.Derivative item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Derivatives
                    .Where(i => i.DerivativeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Derivative>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDerivativeUpdated(item);
                this.context.Derivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Derivatives.Where(i => i.DerivativeID == key);
                
                this.OnAfterDerivativeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/Derivatives(DerivativeID={DerivativeID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDerivative(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.Derivative> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Derivatives
                    .Where(i => i.DerivativeID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Derivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnDerivativeUpdated(item);
                this.context.Derivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Derivatives.Where(i => i.DerivativeID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDerivativeCreated(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeCreated(Osporting.Server.Models.OSPortDB.Derivative item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.Derivative item)
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

                this.OnDerivativeCreated(item);
                this.context.Derivatives.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Derivatives.Where(i => i.DerivativeID == item.DerivativeID);

                

                this.OnAfterDerivativeCreated(item);

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
