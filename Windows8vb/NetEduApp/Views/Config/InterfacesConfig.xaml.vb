
Namespace Views.Config
	' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

	''' <summary>
	''' An empty page that can be used on its own or navigated to within a Frame.
	''' </summary>
	Public NotInheritable Class InterfacesConfig
		Inherits Page

#Region "Navigation"
        ''' <summary>
        ''' NavigationHelper is used on each page to aid in navigation and 
        ''' process lifetime management
        ''' </summary>
        Public ReadOnly Property NavigationHelper As Model.Common.NavigationHelper
            Get
                Return Me._navigationHelper
            End Get
        End Property
		Private _navigationHelper As Model.Common.NavigationHelper

		''' <summary>
		''' This can be changed to a strongly typed view model.
		''' </summary>
		Public ReadOnly Property DefaultViewModel As Model.ViewModels.Config.InterfacesConfigViewModel
			Get
				Return Me._defaultViewModel
			End Get
		End Property
		Private _defaultViewModel

		Public Sub New()
			InitializeComponent()

			Me._navigationHelper = New Model.Common.NavigationHelper(Me)
			Me._defaultViewModel = New Model.ViewModels.Config.InterfacesConfigViewModel(Me._navigationHelper)

			AddHandler Me._navigationHelper.LoadState, AddressOf NavigationHelper_LoadState
			AddHandler Me._navigationHelper.SaveState, AddressOf NavigationHelper_SaveState
		End Sub

        ''' <summary>
        ''' Populates the page with content passed during navigation.  Any saved state is also
        ''' provided when recreating a page from a prior session.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event; typically <see cref="NavigationHelper"/>
        ''' </param>
        ''' <param name="e">Event data that provides both the navigation parameter passed to
        ''' <see cref="Frame.Navigate"/> when this page was initially requested and
        ''' a dictionary of state preserved by this page during an earlier
        ''' session.  The state will be null the first time a page is visited.</param>
        Private Sub NavigationHelper_LoadState(sender As Object, e As Model.Common.LoadStateEventArgs)
			Me.DefaultViewModel.Device = Me.DataContext
		End Sub

        ''' <summary>
        ''' Preserves state associated with this page in case the application is suspended or the
        ''' page is discarded from the navigation cache.  Values must conform to the serialization
        ''' requirements of <see cref="Model.Common.SuspensionManager.SessionState"/>.
        ''' </summary>
        ''' <param name="sender">
        ''' The source of the event; typically <see cref="NavigationHelper"/>
        ''' </param>
        ''' <param name="e">Event data that provides an empty dictionary to be populated with 
        ''' serializable state.</param>
        Private Sub NavigationHelper_SaveState(sender As Object, e As Model.Common.SaveStateEventArgs)
			Me.DefaultViewModel.Device = Nothing
		End Sub

#Region "NavigationHelper registration"

		''' The methods provided in this section are simply used to allow
		''' NavigationHelper to respond to the page's navigation methods.
		''' 
		''' Page specific logic should be placed in event handlers for the  
		''' <see cref="Model.Common.NavigationHelper.LoadState"/>
		''' and <see cref="Model.Common.NavigationHelper.SaveState"/>.
		''' The navigation parameter is available in the LoadState method 
		''' in addition to page state preserved during an earlier session.

		Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
			_navigationHelper.OnNavigatedTo(e)
		End Sub

		Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
			_navigationHelper.OnNavigatedFrom(e)
		End Sub

#End Region
#End Region

	End Class
End Namespace
