using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using NetEduApp.Model.ViewModels;
using SUTest = NetEduApp.Model.ViewModels.NetworkViewModel;

namespace NetEduApp.Model.Tests {
	[TestClass]
	public class TC_NetworkViewModel {
		[UITestMethod]
		public void CreateHubCommandTest( ) {
			SUTest viewModel = new SUTest( );

			bool collectionChanged = false;
			viewModel.Lab.Devices.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => {
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(1, e.NewItems.Count);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsInstanceOfType(e.NewItems[0], typeof(HubViewModel));
				collectionChanged = true;
			};

			viewModel.CreateHubCommand.Execute(null);

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(collectionChanged);
		}

		[UITestMethod]
		public void CreateComputerCommandTest( ) {
			SUTest viewModel = new SUTest( );

			bool collectionChanged = false;
			viewModel.Lab.Devices.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => {
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(1, e.NewItems.Count);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsInstanceOfType(e.NewItems[0], typeof(ComputerViewModel));
				collectionChanged = true;
			};

			viewModel.CreateComputerCommand.Execute(null);

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(collectionChanged);
		}

		[UITestMethod]
		public void CreateRouterCommandTest( ) {
			SUTest viewModel = new SUTest( );

			bool collectionChanged = false;
			viewModel.Lab.Devices.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => {
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(1, e.NewItems.Count);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsInstanceOfType(e.NewItems[0], typeof(RouterViewModel));
				collectionChanged = true;
			};

			viewModel.CreateRouterCommand.Execute(null);

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(collectionChanged);
		}

		[UITestMethod]
		public void CreateEthernetLinkCommandTest( ) {
			SUTest viewModel = new SUTest( );

			bool collectionChanged = false;
			viewModel.Lab.Links.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => {
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.AreEqual(1, e.NewItems.Count);
				Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsInstanceOfType(e.NewItems[0], typeof(EthernetLinkViewModel));
				collectionChanged = true;
			};

			viewModel.CreateEthernetLinkCommand.Execute(null);

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(collectionChanged);
		}

		[UITestMethod]
		public void SelectedDeviceChangedTest( ) {
			SUTest viewModel = new SUTest( );

			bool canExecuteChanged = false;
			viewModel.EditCommand.CanExecuteChanged += (object sender, EventArgs e) => {
				canExecuteChanged = true;
			};

			viewModel.SelectedDevice = new ComputerViewModel(new Laboratory());

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(canExecuteChanged);
		}

		[UITestMethod]
		public void EditCommandTest_NoSelectedDevice( ) {
			SUTest viewModel = new SUTest( );

			viewModel.SelectedDevice = null;

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsFalse(viewModel.EditCommand.CanExecute(null));
		}

		[UITestMethod]
		public void EditCommandTest_SelectedDevice( ) {
			SUTest viewModel = new SUTest( );

			viewModel.SelectedDevice = new ComputerViewModel(new Laboratory( ));

			Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert.IsTrue(viewModel.EditCommand.CanExecute(null));
		}

	}
}
