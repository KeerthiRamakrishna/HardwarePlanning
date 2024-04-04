using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Osporting.Server.Data;

namespace Osporting.Server
{
    public partial class OSPortDBService
    {
        OSPortDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly OSPortDBContext context;
        private readonly NavigationManager navigationManager;

        public OSPortDBService(OSPortDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportArchitecturesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportArchitecturesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnArchitecturesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Architecture> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.Architecture>> GetArchitectures(Query query = null)
        {
            var items = Context.Architectures.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnArchitecturesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnArchitectureGet(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnGetArchitectureByArchitectureId(ref IQueryable<Osporting.Server.Models.OSPortDB.Architecture> items);


        public async Task<Osporting.Server.Models.OSPortDB.Architecture> GetArchitectureByArchitectureId(int architectureid)
        {
            var items = Context.Architectures
                              .AsNoTracking()
                              .Where(i => i.ArchitectureID == architectureid);

 
            OnGetArchitectureByArchitectureId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnArchitectureGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnArchitectureCreated(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureCreated(Osporting.Server.Models.OSPortDB.Architecture item);

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> CreateArchitecture(Osporting.Server.Models.OSPortDB.Architecture architecture)
        {
            OnArchitectureCreated(architecture);

            var existingItem = Context.Architectures
                              .Where(i => i.ArchitectureID == architecture.ArchitectureID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Architectures.Add(architecture);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(architecture).State = EntityState.Detached;
                throw;
            }

            OnAfterArchitectureCreated(architecture);

            return architecture;
        }

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> CancelArchitectureChanges(Osporting.Server.Models.OSPortDB.Architecture item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnArchitectureUpdated(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureUpdated(Osporting.Server.Models.OSPortDB.Architecture item);

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> UpdateArchitecture(int architectureid, Osporting.Server.Models.OSPortDB.Architecture architecture)
        {
            OnArchitectureUpdated(architecture);

            var itemToUpdate = Context.Architectures
                              .Where(i => i.ArchitectureID == architecture.ArchitectureID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(architecture);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterArchitectureUpdated(architecture);

            return architecture;
        }

        partial void OnArchitectureDeleted(Osporting.Server.Models.OSPortDB.Architecture item);
        partial void OnAfterArchitectureDeleted(Osporting.Server.Models.OSPortDB.Architecture item);

        public async Task<Osporting.Server.Models.OSPortDB.Architecture> DeleteArchitecture(int architectureid)
        {
            var itemToDelete = Context.Architectures
                              .Where(i => i.ArchitectureID == architectureid)
                              .Include(i => i.HardwarePlannings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnArchitectureDeleted(itemToDelete);


            Context.Architectures.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterArchitectureDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/derivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/derivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/derivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/derivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDerivativesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Derivative> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.Derivative>> GetDerivatives(Query query = null)
        {
            var items = Context.Derivatives.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDerivativesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDerivativeGet(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnGetDerivativeByDerivativeId(ref IQueryable<Osporting.Server.Models.OSPortDB.Derivative> items);


        public async Task<Osporting.Server.Models.OSPortDB.Derivative> GetDerivativeByDerivativeId(int derivativeid)
        {
            var items = Context.Derivatives
                              .AsNoTracking()
                              .Where(i => i.DerivativeID == derivativeid);

 
            OnGetDerivativeByDerivativeId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDerivativeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDerivativeCreated(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeCreated(Osporting.Server.Models.OSPortDB.Derivative item);

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> CreateDerivative(Osporting.Server.Models.OSPortDB.Derivative derivative)
        {
            OnDerivativeCreated(derivative);

            var existingItem = Context.Derivatives
                              .Where(i => i.DerivativeID == derivative.DerivativeID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Derivatives.Add(derivative);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(derivative).State = EntityState.Detached;
                throw;
            }

            OnAfterDerivativeCreated(derivative);

            return derivative;
        }

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> CancelDerivativeChanges(Osporting.Server.Models.OSPortDB.Derivative item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDerivativeUpdated(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeUpdated(Osporting.Server.Models.OSPortDB.Derivative item);

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> UpdateDerivative(int derivativeid, Osporting.Server.Models.OSPortDB.Derivative derivative)
        {
            OnDerivativeUpdated(derivative);

            var itemToUpdate = Context.Derivatives
                              .Where(i => i.DerivativeID == derivative.DerivativeID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(derivative);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDerivativeUpdated(derivative);

            return derivative;
        }

        partial void OnDerivativeDeleted(Osporting.Server.Models.OSPortDB.Derivative item);
        partial void OnAfterDerivativeDeleted(Osporting.Server.Models.OSPortDB.Derivative item);

        public async Task<Osporting.Server.Models.OSPortDB.Derivative> DeleteDerivative(int derivativeid)
        {
            var itemToDelete = Context.Derivatives
                              .Where(i => i.DerivativeID == derivativeid)
                              .Include(i => i.HardwarePlannings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDerivativeDeleted(itemToDelete);


            Context.Derivatives.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDerivativeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportHardwarePlanningsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/hardwareplannings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/hardwareplannings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportHardwarePlanningsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/hardwareplannings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/hardwareplannings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnHardwarePlanningsRead(ref IQueryable<Osporting.Server.Models.OSPortDB.HardwarePlanning> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.HardwarePlanning>> GetHardwarePlannings(Query query = null)
        {
            var items = Context.HardwarePlannings.AsQueryable();

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.Derivative);
            items = items.Include(i => i.Status);
            items = items.Include(i => i.Person);
            items = items.Include(i => i.TestPc);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnHardwarePlanningsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHardwarePlanningGet(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnGetHardwarePlanningByHardwarePlanningId(ref IQueryable<Osporting.Server.Models.OSPortDB.HardwarePlanning> items);


        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> GetHardwarePlanningByHardwarePlanningId(int hardwareplanningid)
        {
            var items = Context.HardwarePlannings
                              .AsNoTracking()
                              .Where(i => i.HardwarePlanningID == hardwareplanningid);

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.Status);
            items = items.Include(i => i.Derivative);
            items = items.Include(i => i.TestPc);
 
            OnGetHardwarePlanningByHardwarePlanningId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnHardwarePlanningGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnHardwarePlanningCreated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningCreated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> CreateHardwarePlanning(Osporting.Server.Models.OSPortDB.HardwarePlanning hardwareplanning)
        {
            OnHardwarePlanningCreated(hardwareplanning);

            var existingItem = Context.HardwarePlannings
                              .Where(i => i.HardwarePlanningID == hardwareplanning.HardwarePlanningID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.HardwarePlannings.Add(hardwareplanning);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(hardwareplanning).State = EntityState.Detached;
                throw;
            }

            OnAfterHardwarePlanningCreated(hardwareplanning);

            return hardwareplanning;
        }

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> CancelHardwarePlanningChanges(Osporting.Server.Models.OSPortDB.HardwarePlanning item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnHardwarePlanningUpdated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningUpdated(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> UpdateHardwarePlanning(int hardwareplanningid, Osporting.Server.Models.OSPortDB.HardwarePlanning hardwareplanning)
        {
            OnHardwarePlanningUpdated(hardwareplanning);

            var itemToUpdate = Context.HardwarePlannings
                              .Where(i => i.HardwarePlanningID == hardwareplanning.HardwarePlanningID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(hardwareplanning);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterHardwarePlanningUpdated(hardwareplanning);

            return hardwareplanning;
        }

        partial void OnHardwarePlanningDeleted(Osporting.Server.Models.OSPortDB.HardwarePlanning item);
        partial void OnAfterHardwarePlanningDeleted(Osporting.Server.Models.OSPortDB.HardwarePlanning item);

        public async Task<Osporting.Server.Models.OSPortDB.HardwarePlanning> DeleteHardwarePlanning(int hardwareplanningid)
        {
            var itemToDelete = Context.HardwarePlannings
                              .Where(i => i.HardwarePlanningID == hardwareplanningid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnHardwarePlanningDeleted(itemToDelete);


            Context.HardwarePlannings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterHardwarePlanningDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPeopleToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/people/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPeopleToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/people/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPeopleRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Person> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.Person>> GetPeople(Query query = null)
        {
            var items = Context.People.AsQueryable();

            //items = items.Include(i => i.Person1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPeopleRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonGet(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnGetPersonByPersonId(ref IQueryable<Osporting.Server.Models.OSPortDB.Person> items);


        public async Task<Osporting.Server.Models.OSPortDB.Person> GetPersonByPersonId(int personid)
        {
            var items = Context.People
                              .AsNoTracking()
                              .Where(i => i.PersonID == personid);

            //items = items.Include(i => i.Person1);
 
            OnGetPersonByPersonId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPersonGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPersonCreated(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonCreated(Osporting.Server.Models.OSPortDB.Person item);

        public async Task<Osporting.Server.Models.OSPortDB.Person> CreatePerson(Osporting.Server.Models.OSPortDB.Person person)
        {
            OnPersonCreated(person);

            var existingItem = Context.People
                              .Where(i => i.PersonID == person.PersonID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.People.Add(person);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(person).State = EntityState.Detached;
                throw;
            }

            OnAfterPersonCreated(person);

            return person;
        }

        public async Task<Osporting.Server.Models.OSPortDB.Person> CancelPersonChanges(Osporting.Server.Models.OSPortDB.Person item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPersonUpdated(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonUpdated(Osporting.Server.Models.OSPortDB.Person item);

        public async Task<Osporting.Server.Models.OSPortDB.Person> UpdatePerson(int personid, Osporting.Server.Models.OSPortDB.Person person)
        {
            OnPersonUpdated(person);

            var itemToUpdate = Context.People
                              .Where(i => i.PersonID == person.PersonID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(person);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPersonUpdated(person);

            return person;
        }

        partial void OnPersonDeleted(Osporting.Server.Models.OSPortDB.Person item);
        partial void OnAfterPersonDeleted(Osporting.Server.Models.OSPortDB.Person item);

        public async Task<Osporting.Server.Models.OSPortDB.Person> DeletePerson(int personid)
        {
            var itemToDelete = Context.People
                              .Where(i => i.PersonID == personid)
                              //.Include(i => i.People1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPersonDeleted(itemToDelete);


            Context.People.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPersonDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStatusesRead(ref IQueryable<Osporting.Server.Models.OSPortDB.Status> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.Status>> GetStatuses(Query query = null)
        {
            var items = Context.Statuses.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStatusGet(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnGetStatusByStatusId(ref IQueryable<Osporting.Server.Models.OSPortDB.Status> items);


        public async Task<Osporting.Server.Models.OSPortDB.Status> GetStatusByStatusId(int statusid)
        {
            var items = Context.Statuses
                              .AsNoTracking()
                              .Where(i => i.StatusID == statusid);

 
            OnGetStatusByStatusId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStatusCreated(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusCreated(Osporting.Server.Models.OSPortDB.Status item);

        public async Task<Osporting.Server.Models.OSPortDB.Status> CreateStatus(Osporting.Server.Models.OSPortDB.Status status)
        {
            OnStatusCreated(status);

            var existingItem = Context.Statuses
                              .Where(i => i.StatusID == status.StatusID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Statuses.Add(status);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(status).State = EntityState.Detached;
                throw;
            }

            OnAfterStatusCreated(status);

            return status;
        }

        public async Task<Osporting.Server.Models.OSPortDB.Status> CancelStatusChanges(Osporting.Server.Models.OSPortDB.Status item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStatusUpdated(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusUpdated(Osporting.Server.Models.OSPortDB.Status item);

        public async Task<Osporting.Server.Models.OSPortDB.Status> UpdateStatus(int statusid, Osporting.Server.Models.OSPortDB.Status status)
        {
            OnStatusUpdated(status);

            var itemToUpdate = Context.Statuses
                              .Where(i => i.StatusID == status.StatusID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(status);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStatusUpdated(status);

            return status;
        }

        partial void OnStatusDeleted(Osporting.Server.Models.OSPortDB.Status item);
        partial void OnAfterStatusDeleted(Osporting.Server.Models.OSPortDB.Status item);

        public async Task<Osporting.Server.Models.OSPortDB.Status> DeleteStatus(int statusid)
        {
            var itemToDelete = Context.Statuses
                              .Where(i => i.StatusID == statusid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStatusDeleted(itemToDelete);


            Context.Statuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTestPcsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/testpcs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/testpcs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTestPcsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/osportdb/testpcs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/osportdb/testpcs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTestPcsRead(ref IQueryable<Osporting.Server.Models.OSPortDB.TestPc> items);

        public async Task<IQueryable<Osporting.Server.Models.OSPortDB.TestPc>> GetTestPcs(Query query = null)
        {
            var items = Context.TestPcs.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTestPcsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTestPcGet(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnGetTestPcByTestPcid(ref IQueryable<Osporting.Server.Models.OSPortDB.TestPc> items);


        public async Task<Osporting.Server.Models.OSPortDB.TestPc> GetTestPcByTestPcid(int testpcid)
        {
            var items = Context.TestPcs
                              .AsNoTracking()
                              .Where(i => i.TestPCID == testpcid);

 
            OnGetTestPcByTestPcid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTestPcGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTestPcCreated(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcCreated(Osporting.Server.Models.OSPortDB.TestPc item);

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> CreateTestPc(Osporting.Server.Models.OSPortDB.TestPc testpc)
        {
            OnTestPcCreated(testpc);

            var existingItem = Context.TestPcs
                              .Where(i => i.TestPCID == testpc.TestPCID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TestPcs.Add(testpc);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(testpc).State = EntityState.Detached;
                throw;
            }

            OnAfterTestPcCreated(testpc);

            return testpc;
        }

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> CancelTestPcChanges(Osporting.Server.Models.OSPortDB.TestPc item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTestPcUpdated(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcUpdated(Osporting.Server.Models.OSPortDB.TestPc item);

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> UpdateTestPc(int testpcid, Osporting.Server.Models.OSPortDB.TestPc testpc)
        {
            OnTestPcUpdated(testpc);

            var itemToUpdate = Context.TestPcs
                              .Where(i => i.TestPCID == testpc.TestPCID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(testpc);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTestPcUpdated(testpc);

            return testpc;
        }

        partial void OnTestPcDeleted(Osporting.Server.Models.OSPortDB.TestPc item);
        partial void OnAfterTestPcDeleted(Osporting.Server.Models.OSPortDB.TestPc item);

        public async Task<Osporting.Server.Models.OSPortDB.TestPc> DeleteTestPc(int testpcid)
        {
            var itemToDelete = Context.TestPcs
                              .Where(i => i.TestPCID == testpcid)
                              .Include(i => i.HardwarePlannings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTestPcDeleted(itemToDelete);


            Context.TestPcs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTestPcDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}