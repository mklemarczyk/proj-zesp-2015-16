Imports NetEduApp.Models
Imports NetEduApp.ViewModels
Imports Windows.UI.Input
' The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

''' <summary>
''' A basic page that provides characteristics common to most applications.
''' </summary>
Public NotInheritable Class NetworkLabPage
    Inherits Page

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

        Me.prop.Visibility = Visibility.Collapsed
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


	Private inAction As Boolean
	Private pickedContol As FrameworkElement
	Private pickedData As VisualLabElement
	Private activeLink As VisualLabLink
	Private lastPosition As Point?

    Private Sub Grid_PointerReleased(sender As Object, e As PointerRoutedEventArgs)
        e.Handled = True
        If pickedData IsNot Nothing And pickedData IsNot FakeVisualLabElement.Fake Then
            _defaultViewModel.SelectedDevice = pickedData
        Else
            _defaultViewModel.SelectedDevice = Nothing
        End If
        If inAction = True And pickedContol IsNot Nothing Then
            AddHandler pickedContol.PointerPressed, AddressOf Image_PointerPressed
            Me.inAction = False
            Me.pickedContol = Nothing
            Me.pickedData = Nothing
			Me.lastPosition = Nothing

			Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

		End If
    End Sub

    Private Sub Grid_PointerMoved(sender As Object, e As PointerRoutedEventArgs)
        e.Handled = True
		If inAction = True And pickedData IsNot Nothing Then
			Dim position As PointerPoint = e.GetCurrentPoint(Me.netItemsControl)
			Me.pickedData.Position = New Point(position.Position.X - 35, position.Position.Y - 35)
		End If
	End Sub

	Private Sub Grid_PointerExited(sender As Object, e As PointerRoutedEventArgs)
		e.Handled = True
		If inAction = True And pickedData IsNot Nothing Then
			If lastPosition IsNot Nothing Then
				Me.pickedData.Position = lastPosition
				AddHandler pickedContol.PointerPressed, AddressOf Image_PointerPressed
			Else
				If pickedData IsNot FakeVisualLabElement.Fake Then
					netItemsControl.Items.Remove(pickedData)
				End If
			End If
		End If
		Me.inAction = False
		Me.pickedContol = Nothing
		Me.pickedData = Nothing
		Me.lastPosition = Nothing

		Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

	End Sub

	Private Sub Image_PointerPressed(sender As Object, e As PointerRoutedEventArgs)
        If inAction = False Then
            If TypeOf sender Is FrameworkElement Then
                Dim control = CType(sender, FrameworkElement)
                If TypeOf control.DataContext Is VisualLabElement Then
                    Me.inAction = True
                    Me.pickedContol = control
                    Me.pickedData = CType(control.DataContext, VisualLabElement)
                    Me.lastPosition = Me.pickedData.Position
					RemoveHandler control.PointerPressed, AddressOf Image_PointerPressed

					Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1)

				End If
            End If
        Else
            If activeLink IsNot Nothing Then
                If TypeOf sender Is FrameworkElement Then
                    Dim control = CType(sender, FrameworkElement)
                    If TypeOf control.DataContext Is VisualLabElement Then
                        Dim pickedData = CType(control.DataContext, VisualLabElement)
                        If activeLink.ItemA Is FakeVisualLabElement.Fake Then
                            activeLink.ItemA = pickedData
                        Else
                            If activeLink.ItemB Is FakeVisualLabElement.Fake Then
                                activeLink.ItemB = pickedData
                            End If
                            inAction = False
                            activeLink = Nothing
                            pickedData = Nothing
							_defaultViewModel.Lab.Devices.Remove(FakeVisualLabElement.Fake)

							Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

						End If
                    End If
                End If
            End If
        End If
    End Sub

	Private Sub Image_DoubleTapped(sender As Object, e As DoubleTappedRoutedEventArgs)
        'If TypeOf sender Is FrameworkElement Then
        '    Dim control = CType(sender, FrameworkElement)
        '    If TypeOf control.DataContext Is Computer Then
        '        e.Handled = True
        '        Dim conf = New ComputerConfig
        '        prop.DataContext = New ViewModels.ComputerConfigurationViewModel(CType(control.DataContext, VisualLabElement))
        '        prop.Content = conf
        '        prop.Visibility = Visibility.Visible
        '        AddHandler conf.IsEnabledChanged, AddressOf Conf_IsEnabledChanged
        '    End If
        '    If TypeOf control.DataContext Is Router Then
        '        e.Handled = True
        '        Dim conf = New RouterConfig
        '        prop.DataContext = New ViewModels.ComputerConfigurationViewModel(CType(control.DataContext, VisualLabElement))
        '        prop.Content = conf
        '        prop.Visibility = Visibility.Visible
        '        AddHandler conf.IsEnabledChanged, AddressOf Conf_IsEnabledChanged
        '    End If
        'End If
    End Sub

	Private Sub Conf_IsEnabledChanged(sender As Object, e As DependencyPropertyChangedEventArgs)
        'If TypeOf sender Is ComputerConfig Then
        '    Dim conf = CType(sender, ComputerConfig)
        '    RemoveHandler conf.IsEnabledChanged, AddressOf Conf_IsEnabledChanged
        '    prop.Content = Nothing
        '    prop.DataContext = Nothing
        '    prop.Visibility = Visibility.Collapsed
        'End If
        'If TypeOf sender Is RouterConfig Then
        '    Dim conf = CType(sender, RouterConfig)
        '    RemoveHandler conf.IsEnabledChanged, AddressOf Conf_IsEnabledChanged
        '    prop.Content = Nothing
        '    prop.DataContext = Nothing
        '    prop.Visibility = Visibility.Collapsed
        'End If
    End Sub

	Private Sub TextBlock_DoubleTapped(sender As Object, e As DoubleTappedRoutedEventArgs)

    End Sub

    Private Sub MenuFlyoutItem_Click(sender As Object, e As RoutedEventArgs)
        If activeLink IsNot Nothing Then
            _defaultViewModel.Lab.Devices.Remove(FakeVisualLabElement.Fake)
            _defaultViewModel.Lab.Links.Remove(activeLink)
            activeLink.ItemA = Nothing
            activeLink.ItemB = Nothing
            activeLink = Nothing
            pickedData = Nothing
        End If
        If inAction = False And activeLink Is Nothing Then
            inAction = True
            activeLink = New EthernetLink
            activeLink.ItemA = FakeVisualLabElement.Fake
            activeLink.ItemB = FakeVisualLabElement.Fake
            pickedData = FakeVisualLabElement.Fake
            _defaultViewModel.Lab.Devices.Add(FakeVisualLabElement.Fake)
			_defaultViewModel.Lab.Links.Add(activeLink)

			Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Cross, 1)

		End If
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
End Class
