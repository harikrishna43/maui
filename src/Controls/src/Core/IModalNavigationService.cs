using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Maui.Controls
{
	// But why did you make it internal? 
	// The implementation of these services inside Xamarin.Forms is also internal and
	// we aren't ready to commit to a public navigation set of interfaces yet
	// Hopefully this comment won't exist anymore for v1 .NET MAUI 
	internal interface IModalNavigationService
	{
		IReadOnlyList<Page> ModalStack { get; }
		Task PushModalAsync(Page page, IMauiContext mauiContext);
		Task PushModalAsync(Page page, bool animated, IMauiContext mauiContext);
		Task<Page> PopModalAsync();
		Task<Page> PopModalAsync(bool animated);
	}
}
