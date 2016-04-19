Imports NetEduApp.Models
Imports NetEduApp.ViewModels
Imports NetEduApp.Views.Config
Imports Windows.UI.Input
' The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

''' <summary>
''' A basic page that provides characteristics common to most applications.
''' </summary>
Public NotInheritable Class NetworkLabPage
	Inherits Page

#Region "Navigation"
	''' <summary>
	''' NavigationHelper is used on each page to aid in navigation and 
	''' process lifetime management
	''' </summary>
	Public ReadOnly Property NavigationHelper As Common.NavigationHelper
		Get
			Return Me._navigationHelper
		End Get
	End Property
	Private _navigationHelper As Common.NavigationHelper

    ''' <summary>
    ''' This can be changed to a strongly typed view model.
    ''' </summary>
    Public ReadOnly Property DefaultViewModel As NetworkViewModel
		Get
			Return Me._defaultViewModel
		End Get
	End Property
	Private _defaultViewModel As New NetworkViewModel()

	Public Sub New()
		InitializeComponent()
		Me._navigationHelper = New Common.NavigationHelper(Me)
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
    Private Sub NavigationHelper_LoadState(sender As Object, e As Common.LoadStateEventArgs)

	End Sub

    ''' <summary>
    ''' Preserves state associated with this page in case the application is suspended or the
    ''' page is discarded from the navigation cache.  Values must conform to the serialization
    ''' requirements of <see cref="Common.SuspensionManager.SessionState"/>.
    ''' </summary>
    ''' <param name="sender">
    ''' The source of the event; typically <see cref="NavigationHelper"/>
    ''' </param>
    ''' <param name="e">Event data that provides an empty dictionary to be populated with 
    ''' serializable state.</param>
    Private Sub NavigationHelper_SaveState(sender As Object, e As Common.SaveStateEventArgs)

	End Sub

#Region "NavigationHelper registration"

	''' The methods provided in this section are simply used to allow
	''' NavigationHelper to respond to the page's navigation methods.
	''' 
	''' Page specific logic should be placed in event handlers for the  
	''' <see cref="Common.NavigationHelper.LoadState"/>
	''' and <see cref="Common.NavigationHelper.SaveState"/>.
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

    Private Sub TextBlock_DoubleTapped(sender As Object, e As DoubleTappedRoutedEventArgs)

    End Sub

    Private Sub Page_Drop(sender As Object, e As DragEventArgs)
        'Dim items = Await e.DataView.GetStorageItemsAsync()
        'If items.Count > 0 Then
        '    Dim storageFile = CType(items(0), StorageFile)

        'End If
        'e.Handled = True
    End Sub

	Private Sub Page_DragOver(sender As Object, e As DragEventArgs)
        'AndAlso (Await e.DataView.GetStorageItemsAsync()).Count = 1
        'If e.AcceptedOperation = DataTransfer.DataPackageOperation.None Then
        '    e.AcceptedOperation = DataTransfer.DataPackageOperation.Link
        '    e.DragUIOverride.Caption = "Open"
        '    e.Handled = True
        'End If
    End Sub

	Private Sub CommandBar_Closed(sender As Object, e As Object)
		DirectCast(sender, CommandBar).IsOpen = True
	End Sub

    Private Sub prop_DataContextChanged(sender As FrameworkElement, args As DataContextChangedEventArgs)
        If prop IsNot Nothing Then
            If args.NewValue IsNot Nothing Then
                prop.Visibility = Visibility.Visible
                prop.Navigate(GetType(ConfigMenu))
                prop.BackStack.Clear()
            Else
                prop.Visibility = Visibility.Collapsed
            End If
        End If
    End Sub
End Class
