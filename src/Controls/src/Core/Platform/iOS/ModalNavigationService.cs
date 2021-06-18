using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Internals;
using UIKit;

namespace Microsoft.Maui.Controls.Platform
{
	 // TODO: MAUI VALIDATE Appearing disappearing
	internal class ModalNavigationService : IModalNavigationService
	{
		readonly List<Page> _modals;
		bool _appeared = true;
		public ModalNavigationService()
		{
			_modals = new List<Page>();
		}

		// do I really need this anymore?
		static void HandleChildRemoved(object sender, ElementEventArgs e)
		{
			var view = e.Element;
			//view?.DisposeModalAndChildRenderers();
		}


		public IReadOnlyList<Page> ModalStack => _modals;

		public Task<Page> PopModalAsync()
		{
			return PopModalAsync(true);
		}

		public async Task<Page> PopModalAsync(bool animated)
		{
			var modal = _modals.Last();
			_modals.Remove(modal);
			modal.DescendantRemoved -= HandleChildRemoved;

			var controller = (modal.Handler as INativeViewHandler).ViewController;

			//if (_modals.Count >= 1 && controller != null)
				await controller.DismissViewControllerAsync(animated);
			/*
			 * I don't know what this does
			 * else
				await _renderer.DismissViewControllerAsync(animated);*/

			// Yes?
			//modal.DisposeModalAndChildRenderers();

			return modal;
		}

		public Task PushModalAsync(Page modal, IMauiContext mauiContext)
		{
			return PushModalAsync(modal, true, mauiContext);
		}

		public Task PushModalAsync(Page modal, bool animated, IMauiContext mauiContext)
		{
			EndEditing();

			var elementConfiguration = modal as IElementConfiguration<Page>;

			var presentationStyle = elementConfiguration?.On<PlatformConfiguration.iOS>()?.ModalPresentationStyle().ToNativeModalPresentationStyle();

			_modals.Add(modal);

			modal.DescendantRemoved += HandleChildRemoved;

			if (_appeared)
				return PresentModal(modal, animated && animated, mauiContext);

			return Task.FromResult<object>(null);
		}

		async Task PresentModal(Page modal, bool animated, IMauiContext mauiContext)
		{
			modal.ToNative(mauiContext);
			var wrapper = new ModalWrapper(modal.Handler as INativeViewHandler);

			if (_modals.Count > 1)
			{
				var topPage = _modals[_modals.Count - 2];
				var controller = (topPage.Handler as INativeViewHandler).ViewController;
				if (controller != null)
				{
					await controller.PresentViewControllerAsync(wrapper, animated);
					await Task.Delay(5);
					return;
				}
			}

			// One might wonder why these delays are here... well thats a great question. It turns out iOS will claim the 
			// presentation is complete before it really is. It does not however inform you when it is really done (and thus 
			// would be safe to dismiss the VC). Fortunately this is almost never an issue

			var _renderer = mauiContext.Window.RootViewController;
			await _renderer.PresentViewControllerAsync(wrapper, animated);
			await Task.Delay(5);
		}		

		void EndEditing()
		{
			// If any text entry controls have focus, we need to end their editing session
			// so that they are not the first responder; if we don't some things (like the activity indicator
			// on pull-to-refresh) will not work correctly. 

			// The topmost modal on the stack will have the Window; we can use that to end any current
			// editing that's going on 
			if (_modals.Count > 0)
			{
				var uiViewController = (_modals.Last().Handler as INativeViewHandler).ViewController;
				uiViewController?.View?.Window?.EndEditing(true);
				return;
			}


			// TODO MAUI
			// If there aren't any modals, then the platform renderer will have the Window
			//_renderer.View?.Window?.EndEditing(true);
		}
	}
}
