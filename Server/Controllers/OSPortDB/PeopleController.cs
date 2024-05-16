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
    [Route("odata/OSPortDB/People")]
    public partial class PeopleController : ODataController
    {
        private Osporting.Server.Data.OSPortDBContext context;

        public PeopleController(Osporting.Server.Data.OSPortDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Osporting.Server.Models.OSPortDB.Person> GetPeople()
        {
            var items = this.context.People.AsQueryable<Osporting.Server.Models.OSPortDB.Person>();
            this.OnPeopleRead(ref items);

            return items;
        }

        partial void OnPeopleRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Person> items);

        partial void OnPersonGet(ref SingleResult<Osporting.Server.Models.OSPortDB.Person> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/OSPortDB/People(PersonID={PersonID})")]
        public SingleResult<Osporting.Server.Models.OSPortDB.Person> GetPerson(int key)
        {
            var items = this.context.People.Where(i => i.PersonID == key);
            var result = SingleResult.Create(items);

            OnPersonGet(ref result);

            return result;
        }
        partial void OnPersonDeleted(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonDeleted(Osporting.Server.Models.OSPortDB.Person item);

        [HttpDelete("/odata/OSPortDB/People(PersonID={PersonID})")]
        public IActionResult DeletePerson(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.People
                    .Where(i => i.PersonID == key)
                    //.Include(i => i.People1)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Person>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPersonDeleted(item);
                this.context.People.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPersonDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPersonUpdated(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonUpdated(Osporting.Server.Models.OSPortDB.Person item);

        [HttpPut("/odata/OSPortDB/People(PersonID={PersonID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPerson(int key, [FromBody]Osporting.Server.Models.OSPortDB.Person item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.People
                    .Where(i => i.PersonID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Person>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPersonUpdated(item);
                this.context.People.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.People.Where(i => i.PersonID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Person");
                this.OnAfterPersonUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/OSPortDB/People(PersonID={PersonID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPerson(int key, [FromBody]Delta<Osporting.Server.Models.OSPortDB.Person> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.People
                    .Where(i => i.PersonID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Osporting.Server.Models.OSPortDB.Person>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnPersonUpdated(item);
                this.context.People.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.People.Where(i => i.PersonID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Person");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPersonCreated(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonCreated(Osporting.Server.Models.OSPortDB.Person item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Osporting.Server.Models.OSPortDB.Person item)
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

                this.OnPersonCreated(item);
                this.context.People.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.People.Where(i => i.PersonID == item.PersonID);

                Request.QueryString = Request.QueryString.Add("$expand", "Person");

                this.OnAfterPersonCreated(item);

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
