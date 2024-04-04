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
    public partial class HardwarePlannings
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

        protected IEnumerable<Osporting.Server.Models.OSPortDB.HardwarePlanning> hardwarePlannings;

        protected RadzenDataGrid<Osporting.Server.Models.OSPortDB.HardwarePlanning> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await OSPortDBService.GetHardwarePlannings(filter: $@"(contains(ProgramIncrement,""{search}"") or contains(HWAssetNo,""{search}"") or contains(HWevaluationBoard,""{search}"") or contains(MCU,""{search}"") or contains(StartWeek,""{search}"") or contains(EndWeek,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Architecture,Derivative,TestPc,Status,Person", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                hardwarePlannings = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load HardwarePlannings" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddHardwarePlanning>("Add HardwarePlanning", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Osporting.Server.Models.OSPortDB.HardwarePlanning args)
        {
            await DialogService.OpenAsync<EditHardwarePlanning>("Edit HardwarePlanning", new Dictionary<string, object> { {"HardwarePlanningID", args.HardwarePlanningID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Osporting.Server.Models.OSPortDB.HardwarePlanning hardwarePlanning)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await OSPortDBService.DeleteHardwarePlanning(hardwarePlanningId:hardwarePlanning.HardwarePlanningID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete HardwarePlanning"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await OSPortDBService.ExportHardwarePlanningsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "Architecture,Derivative,TestPc",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "HardwarePlannings");
            }

            if (args == null || args.Value == "xlsx")
            {
                await OSPortDBService.ExportHardwarePlanningsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "Architecture,Derivative,TestPc",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "HardwarePlannings");
            }
        }
    }
}