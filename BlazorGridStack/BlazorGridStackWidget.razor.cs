using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorGridStack.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGridStack
{
    partial class BlazorGridStackWidget : ComponentBase
    {
        [Inject] IJSRuntime JS { get; set; } = default!;
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public BlazorGridStackWidgetOptions? WidgetOptions { get; set; }
        [Parameter] public EventCallback<BlazorGridStackWidgetOptions> DeleteBtn_OnClick { get; set; }
        [Parameter] public EventCallback<BlazorGridStackWidgetOptions> EditBtn_OnClick { get; set; }
        [Parameter] public bool? IsEditable { get; set; } = null;
        [Parameter] public bool? IsDeletable { get; set; } = null;
        ElementReference DropdownRef;
        bool isShowingCommands = false;

        override protected void OnInitialized()
        {
            if (WidgetOptions == null)
                WidgetOptions = new BlazorGridStackWidgetOptions();
            if (IsEditable != null)
                WidgetOptions.IsEditable = IsEditable.Value;
            if (IsDeletable != null)
                WidgetOptions.IsDeletable = IsDeletable.Value;
        }
        public async Task DeleteClick()
        {
            DeleteBtn_OnClick.InvokeAsync(WidgetOptions);
        }
        async Task EditClick()
        {
            EditBtn_OnClick.InvokeAsync(WidgetOptions);
        }
        public async Task ShowCommands()
        {
            isShowingCommands = !isShowingCommands;

        }
        async Task CommandsOnfocusout()
        {
            isShowingCommands = false;
        }
        [JSInvokable]
        public async Task CloseDropdown()
        {
            isShowingCommands = false;
            StateHasChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (WidgetOptions.IsEditable || WidgetOptions.IsDeletable)
                    await JS.InvokeVoidAsync("registerOutsideClick", DropdownRef, DotNetObjectReference.Create(this));
            }
        }
        //protected override bool ShouldRender()
        //{
        //    if (WidgetOptions !=null && WidgetOptions.ShouldRender)
        //    {
        //        WidgetOptions.ShouldRender = false;
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
