using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Osporting.Client.Pages
{
    public partial class AddPerson
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public OSPortDBService OSPortDBService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            person = new Osporting.Server.Models.OSPortDB.Person();
        }
        protected bool errorVisible;
        protected Osporting.Server.Models.OSPortDB.Person person;

        protected IEnumerable<Osporting.Server.Models.OSPortDB.Person> peopleForPersonID;


        protected int peopleForPersonIDCount;
        protected Osporting.Server.Models.OSPortDB.Person peopleForPersonIDValue;
        protected async Task peopleForPersonIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetPeople(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(PersonFirstName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                peopleForPersonID = result.Value.AsODataEnumerable();
                peopleForPersonIDCount = result.Count;

                if (!object.Equals(person.PersonID, null))
                {
                    var valueResult = await OSPortDBService.GetPeople(filter: $"PersonID eq {person.PersonID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        peopleForPersonIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Person" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await OSPortDBService.CreatePerson(person);
                DialogService.Close(person);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}