using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using NetEduApp.Model.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;
using SUTest = NetEduApp.Model.Common.NavigationHelper;

namespace NetEduApp.Model.Tests {
	[TestClass]
	public class TC_NavigationHelper {
		[UITestMethod]
		public void CanGoBackTest_PageWithoutBack( ) {
			var page = PageWithoutBack( );

			SUTest navigation = new SUTest(page);

			IsFalse(navigation.CanGoBack( ));
		}

		[UITestMethod]
		public void CanGoBackTest_PageWithBack( ) {
			var page = PageWithBack( );

			SUTest navigation = new SUTest(page);

			IsTrue(navigation.CanGoBack( ));
		}

		[UITestMethod]
		public void CanGoBackTest_PageWithBackAndForward( ) {
			var page = PageWithBackAndForward( );

			SUTest navigation = new SUTest(page);

			IsTrue(navigation.CanGoBack( ));
		}

		[UITestMethod]
		public void CanGoForwardTest_PageWithoutForward( ) {
			var page = PageWithoutForward( );

			SUTest navigation = new SUTest(page);

			IsFalse(navigation.CanGoForward( ));
		}

		[UITestMethod]
		public void CanGoForwardTest_PageWithForward( ) {
			var page = PageWithForward( );

			SUTest navigation = new SUTest(page);

			IsTrue(navigation.CanGoForward( ));
		}

		[UITestMethod]
		public void CanGoForwardTest_PageWithBackAndForward( ) {
			var page = PageWithBackAndForward( );

			SUTest navigation = new SUTest(page);

			IsTrue(navigation.CanGoForward( ));
		}

		[UITestMethod]
		public void OnNavigatedToTest_FirstTo( ) {
			var frame = new Frame( );
			Page page = null;
			NavigationEventArgs eventObject = null;

			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
				eventObject = e;
			};
			frame.Navigate(typeof(Page));

			SUTest navigation = new SUTest(page);

			// Act
			bool loadStateRaised = false;
			navigation.LoadState += (object sender, LoadStateEventArgs e) => {
				IsNull(e.PageState);
				loadStateRaised = true;
			};

			navigation.OnNavigatedTo(eventObject);

			IsTrue(loadStateRaised);
		}

		[UITestMethod]
		public void OnNavigatedFromTest_NothingSaved( ) {
			var frame = new Frame( );
			Page page = null;
			NavigationEventArgs eventObject = null;

			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
				eventObject = e;
			};
			frame.Navigate(typeof(Page));

			SUTest navigation = new SUTest(page);
			navigation.OnNavigatedTo(eventObject);

			// Act
			bool saveStateRaised = false;
			navigation.SaveState += (object sender, SaveStateEventArgs e) => {
				AreEqual(0, e.PageState.Count);
				saveStateRaised = true;
			};

			navigation.OnNavigatedFrom(eventObject);

			IsTrue(saveStateRaised);
		}

		[UITestMethod]
		public void OnNavigatedToTest_WithParameter( ) {
			var frame = new Frame( );
			Page page = null;
			NavigationEventArgs eventObject = null;

			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
				eventObject = e;
			};
			frame.Navigate(typeof(Page), "Test param");

			SUTest navigation = new SUTest(page);

			// Act
			bool loadStateRaised = false;
			navigation.LoadState += (object sender, LoadStateEventArgs e) => {
				AreEqual(e.NavigationParameter, "Test param");
				loadStateRaised = true;
			};

			navigation.OnNavigatedTo(eventObject);

			IsTrue(loadStateRaised);
		}


		#region PreparingMethods

		public Page PageWithoutBack( ) {
			var frame = new Frame( );
			Page page = null;
			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
			};
			frame.Navigate(typeof(Page));

			return page;
		}

		public Page PageWithoutForward( ) {
			var frame = new Frame( );
			Page page = null;
			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
			};
			frame.Navigate(typeof(Page));

			return page;
		}

		public Page PageWithBack( ) {
			var frame = new Frame( );
			Page page = null;
			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
			};
			frame.Navigate(typeof(Page));
			frame.Navigate(typeof(Page));

			return page;
		}

		public Page PageWithForward( ) {
			var frame = new Frame( );
			Page page = null;
			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
			};
			frame.Navigate(typeof(Page));
			frame.Navigate(typeof(Page));
			frame.GoBack( );

			return page;
		}

		public Page PageWithBackAndForward( ) {
			var frame = new Frame( );
			Page page = null;
			frame.Navigated += (object sender, NavigationEventArgs e) => {
				page = e.Content as Page;
			};
			frame.Navigate(typeof(Page));
			frame.Navigate(typeof(Page));
			frame.Navigate(typeof(Page));
			frame.GoBack( );

			return page;
		}

		#endregion
	}
}
