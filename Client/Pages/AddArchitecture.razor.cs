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
    public partial class AddArchitecture
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
            architecture = new Osporting.Server.Models.OSPortDB.Architecture();
        }
        protected bool errorVisible;
        protected Osporting.Server.Models.OSPortDB.Architecture architecture;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await OSPortDBService.CreateArchitecture(architecture);
                DialogService.Close(architecture);
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