#pragma checksum "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/Pages/Workflow/Start.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a96f0e4b297d3350aed3e6bd5d65530e9e457afd"
// <auto-generated/>
#pragma warning disable 1591
namespace zeebe_poc.Pages.Workflow
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using zeebe_poc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/_Imports.razor"
using zeebe_poc.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/workflow/start")]
    public partial class Start : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Starting Point</h1>\n\n");
            __builder.AddMarkupContent(1, "<p>Starting Point of Workflow</p>\n\n");
            __builder.OpenElement(2, "button");
            __builder.AddAttribute(3, "class", "btn btn-primary");
            __builder.AddAttribute(4, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 9 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/Pages/Workflow/Start.razor"
                                          NavigateToApply

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(5, "Let Start");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "/Users/ukaratas/Documents/GitHub/zeebe-simple-poc/src/zeebe-poc/Pages/Workflow/Start.razor"
       

    private void NavigateToApply()
    {
        NavigationManager.NavigateTo("/workflow/apply");
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
    }
}
#pragma warning restore 1591
