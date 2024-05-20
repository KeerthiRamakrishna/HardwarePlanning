using System.Collections.Immutable;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Osporting.Server.Models.OSPortDB;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;

namespace Osporting.Client.Pages
{
    public partial class Index
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
        protected SecurityService Security { get; set; }

        [Inject]
        public OSPortDBService OSPortDBService { get; set; }

        List<Appointment> appointments { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await OSPortDBService.GetHardwarePlannings(expand: "Architecture,Derivative,Person,TestPc");
                //IList<Appointment> appointments = (IList<Appointment>)result.Value.Select(x => new { start = x.StartDate , end = x.EndDate , Text = x.HWevaluationBoard }).ToList();
                //List<Appointment> appointments = (result.Value).ToList();

                var list1 = ((List<Appointment>)result.Value.Select(x => new Appointment { Start = x.StartDate, End = x.EndDate, Text =  x.HWevaluationBoard + "-" + x.Person.PersonFirstName + "- StartDate:" + x.StartDate + "- EndDate:" + x.EndDate }).ToList());
                appointments = list1;

                //count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load HardwarePlannings" });
            }
        }

        public class Appointment
        {
            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }

            public string Text { get; set; }
        }

        RadzenScheduler<Appointment> scheduler;
       // EventConsole console;
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        Month startMonth = Month.January;

    //    IList<Appointment> appointments = new List<Appointment>
    //{
    //    new Appointment { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2), Text = "Birthday" },
    //    new Appointment { Start = DateTime.Today.AddDays(-11), End = DateTime.Today.AddDays(-10), Text = "Day off" },
    //    new Appointment { Start = DateTime.Today.AddDays(-10), End = DateTime.Today.AddDays(-8), Text = "Work from home" },
    //    new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(12), Text = "Online meeting" },
    //    new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(13), Text = "Skype call" },
    //    new Appointment { Start = DateTime.Today.AddHours(14), End = DateTime.Today.AddHours(14).AddMinutes(30), Text = "Dentist appointment" },
    //    new Appointment { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(12), Text = "Vacation" },
    //};

        async Task StartMonthChange()
        {
            await scheduler.Reload();
        }

        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

            // Draw a line for new year if start month is not January
            if ((args.View.Text == "Planner" || args.View.Text == "Timeline") && args.Start.Month == 12 && startMonth != Month.January)
            {
                args.Attributes["style"] = "border-bottom: thick double var(--rz-base-600);";
            }
        }

        //async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        //{
        //    console.Log($"SlotSelect: Start={args.Start} End={args.End}");

        //    if (args.View.Text != "Year")
        //    {
        //        Appointment data = await DialogService.OpenAsync<AddAppointmentPage>("Add Appointment",
        //            new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

        //        if (data != null)
        //        {
        //            appointments.Add(data);
        //            // Either call the Reload method or reassign the Data property of the Scheduler
        //            await scheduler.Reload();
        //        }
        //    }
        //}

        //async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<Appointment> args)
        //{
        //    console.Log($"AppointmentSelect: Appointment={args.Data.Text}");

        //    await DialogService.OpenAsync<EditAppointmentPage>("Edit Appointment", new Dictionary<string, object> { { "Appointment", args.Data } });

        //    await scheduler.Reload();
        //}

        void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<Appointment> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.Text == "Birthday")
            {
                args.Attributes["style"] = "background: red";
            }
        }
    }
}