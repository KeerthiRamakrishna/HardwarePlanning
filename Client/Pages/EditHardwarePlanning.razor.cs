using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Osporting.Server.Models.OSPortDB;

namespace Osporting.Client.Pages
{
    public partial class EditHardwarePlanning
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

        [Parameter]
        public int HardwarePlanningID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hardwarePlanning = await OSPortDBService.GetHardwarePlanningByHardwarePlanningId(hardwarePlanningId:HardwarePlanningID);
        }
        protected bool errorVisible;
        protected Osporting.Server.Models.OSPortDB.HardwarePlanning hardwarePlanning;

        protected IEnumerable<Osporting.Server.Models.OSPortDB.Architecture> architecturesForArchitectureID;

        protected IEnumerable<Osporting.Server.Models.OSPortDB.Derivative> derivativesForDerivativeID;
        
        protected IEnumerable<Osporting.Server.Models.OSPortDB.Status> statusForStatusID;

        protected IEnumerable<Osporting.Server.Models.OSPortDB.TestPc> testPcsForTestPCID;

        protected int architecturesForArchitectureIDCount;
        protected Osporting.Server.Models.OSPortDB.Architecture architecturesForArchitectureIDValue;
        protected async Task architecturesForArchitectureIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetArchitectures(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(ArchitectureName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                architecturesForArchitectureID = result.Value.AsODataEnumerable();
                architecturesForArchitectureIDCount = result.Count;

                if (!object.Equals(hardwarePlanning.ArchitectureID, null))
                {
                    var valueResult = await OSPortDBService.GetArchitectures(filter: $"ArchitectureID eq {hardwarePlanning.ArchitectureID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        architecturesForArchitectureIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Architecture" });
            }
        }
        protected Osporting.Server.Models.OSPortDB.Person person;
        protected IEnumerable<Osporting.Server.Models.OSPortDB.Person> peopleForPersonID;
        protected int peopleForPersonIDCount;
        protected Osporting.Server.Models.OSPortDB.Person peopleForPersonIDValue;
        protected async Task peopleForPersonIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetPeople(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(PersonFirstName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                peopleForPersonID = result.Value.AsODataEnumerable();
                peopleForPersonIDCount = result.Count;

                if (!object.Equals(hardwarePlanning.PersonID, null))
                {
                    var valueResult = await OSPortDBService.GetPeople(filter: $"PersonID eq {hardwarePlanning.PersonID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        peopleForPersonIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Person" });
            }
        }

        protected int statusForStatusIDCount;
        protected Osporting.Server.Models.OSPortDB.Status statusForStatusIDValue;
        protected async Task statusForStatusIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetStatuses(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(StatusName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                statusForStatusID = result.Value.AsODataEnumerable();
                statusForStatusIDCount = result.Count;

                if (!object.Equals(hardwarePlanning.StatusID, null))
                {
                    var valueResult = await OSPortDBService.GetStatuses(filter: $"StatusID eq {hardwarePlanning.StatusID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        statusForStatusIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Status" });
            }
        }

        protected int derivativesForDerivativeIDCount;
        protected Osporting.Server.Models.OSPortDB.Derivative derivativesForDerivativeIDValue;
        protected async Task derivativesForDerivativeIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetDerivatives(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(DerivativeName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                derivativesForDerivativeID = result.Value.AsODataEnumerable();
                derivativesForDerivativeIDCount = result.Count;

                if (!object.Equals(hardwarePlanning.DerivativeID, null))
                {
                    var valueResult = await OSPortDBService.GetDerivatives(filter: $"DerivativeID eq {hardwarePlanning.DerivativeID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        derivativesForDerivativeIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Derivative" });
            }
        }

        protected int testPcsForTestPCIDCount;
        protected Osporting.Server.Models.OSPortDB.TestPc testPcsForTestPCIDValue;
        protected async Task testPcsForTestPCIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetTestPcs(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(TestPCName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                testPcsForTestPCID = result.Value.AsODataEnumerable();
                testPcsForTestPCIDCount = result.Count;

                if (!object.Equals(hardwarePlanning.TestPCID, null))
                {
                    var valueResult = await OSPortDBService.GetTestPcs(filter: $"TestPCID eq {hardwarePlanning.TestPCID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        testPcsForTestPCIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TestPc" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await OSPortDBService.UpdateHardwarePlanning(hardwarePlanningId:HardwarePlanningID, hardwarePlanning);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(hardwarePlanning);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            hardwarePlanning = await OSPortDBService.GetHardwarePlanningByHardwarePlanningId(hardwarePlanningId:HardwarePlanningID);
        }
    }
}