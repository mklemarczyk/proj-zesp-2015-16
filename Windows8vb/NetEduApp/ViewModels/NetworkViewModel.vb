Imports NetEduApp.Common
Imports NetEduApp.Emulators.Network
Imports NetEduApp.Models
Imports Windows.UI.Input

Namespace ViewModels
    Public Class NetworkViewModel

        Private names As HashSet(Of String)
        Private _SelectedDevice As DeviceViewModel

#Region "New()"
        Public Sub New()
            'NavigationService = CType(App.Current, App).NavigationService
            Lab = New Laboratory

            CreateAccessPointCommand = New RelayCommand(AddressOf CreateAccessPointAction)
            CreateBridgeCommand = New RelayCommand(AddressOf CreateBridgeAction)
            CreateComputerCommand = New RelayCommand(AddressOf CreateComputerAction)
            CreateHubCommand = New RelayCommand(AddressOf CreateHubAction)
            CreateNotebookCommand = New RelayCommand(AddressOf CreateNotebookAction)
            CreateRepeaterCommand = New RelayCommand(AddressOf CreateRepeaterAction)
            CreateRouterCommand = New RelayCommand(AddressOf CreateRouterAction)
            CreateSwitchCommand = New RelayCommand(AddressOf CreateSwitchAction)

            CreateCoaxialLinkCommand = New RelayCommand(AddressOf CreateCoaxialLinkAction)
            CreateEthernetCrossoverLinkCommand = New RelayCommand(AddressOf CreateEthernetCrossoverLinkAction)
            CreateEthernetLinkCommand = New RelayCommand(AddressOf CreateEthernetLinkAction)
            CreateOpticalFiberLinkCommand = New RelayCommand(AddressOf CreateOpticalFiberLinkAction)
            CreateSerialLinkCommand = New RelayCommand(AddressOf CreateSerialLinkAction)

            EditCommand = New RelayCommand(AddressOf EditAction, AddressOf CanEditPredicate)
            DeleteCommand = New RelayCommand(AddressOf DeleteAction, AddressOf CanDeletePredicate)

            ApplyActionCommand = New RelayCommand(AddressOf Grid_PointerReleased)
            CancelActionCommand = New RelayCommand(AddressOf Grid_PointerExited)
            MoveCommand = New RelayCommand(Of Point)(AddressOf Grid_PointerMoved)
            StartMoveCommand = New RelayCommand(Of Object)(AddressOf Image_PointerPressed)
        End Sub
#End Region

#Region "Properties"
        Public Property SelectedDevice As DeviceViewModel
            Get
                Return _SelectedDevice
            End Get
            Set(value As DeviceViewModel)
                If _SelectedDevice IsNot value Then
                    If _SelectedDevice IsNot Nothing Then
                        _SelectedDevice.IsSelected = False
                    End If
                    _SelectedDevice = value
                    If _SelectedDevice IsNot Nothing Then
                        _SelectedDevice.IsSelected = True
                    End If
                    EditCommand.RaiseCanExecuteChanged()
                    DeleteCommand.RaiseCanExecuteChanged()
                End If
            End Set
        End Property

        Public Property Lab As Laboratory
#End Region

#Region "Commands"
        Public Property CreateAccessPointCommand As ICommand
        Public Property CreateBridgeCommand As ICommand
        Public Property CreateComputerCommand As ICommand
        Public Property CreateHubCommand As ICommand
        Public Property CreateNotebookCommand As ICommand
        Public Property CreateRepeaterCommand As ICommand
        Public Property CreateRouterCommand As ICommand
        Public Property CreateSwitchCommand As ICommand

        Public Property CreateCoaxialLinkCommand As ICommand
        Public Property CreateEthernetCrossoverLinkCommand As ICommand
        Public Property CreateEthernetLinkCommand As ICommand
        Public Property CreateOpticalFiberLinkCommand As ICommand
        Public Property CreateSerialLinkCommand As ICommand

        Public Property EditCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
#End Region

#Region "Create device commands"
        Private Sub CreateAccessPointAction()
            Lab.NewAccessPoint()
        End Sub
        Private Sub CreateBridgeAction()
            Lab.NewBridge()
        End Sub
        Private Sub CreateComputerAction()
            Lab.NewComputer()
        End Sub
        Private Sub CreateHubAction()
            Lab.NewHub()
        End Sub
        Private Sub CreateNotebookAction()
            Lab.NewNotebook()
        End Sub
        Private Sub CreateRepeaterAction()
            Lab.NewRepeater()
        End Sub
        Private Sub CreateRouterAction()
            Lab.NewRouter()
        End Sub
        Private Sub CreateSwitchAction()
            Lab.NewSwitch()
        End Sub
#End Region

#Region "Create link commands"
        Private Sub CreateCoaxialLinkAction()
            MenuFlyoutItem_Click(GetType(CoaxialLinkViewModel))
        End Sub
        Private Sub CreateEthernetCrossoverLinkAction()
            MenuFlyoutItem_Click(GetType(EthernetCrossoverLinkViewModel))
        End Sub
        Private Sub CreateEthernetLinkAction()
            MenuFlyoutItem_Click(GetType(EthernetLinkViewModel))
        End Sub
        Private Sub CreateOpticalFiberLinkAction()
            MenuFlyoutItem_Click(GetType(OpticalFiberLinkViewModel))
        End Sub
        Private Sub CreateSerialLinkAction()
            MenuFlyoutItem_Click(GetType(SerialLinkViewModel))
        End Sub
#End Region

#Region "Edit action"
        Private Sub EditAction()

        End Sub
        Private Function CanEditPredicate() As Boolean
            Return SelectedDevice IsNot Nothing
        End Function
#End Region

#Region "Delete device action"
        Private Sub DeleteAction()
            Lab.RemoveDevice(SelectedDevice)
            SelectedDevice = Nothing
        End Sub
        Private Function CanDeletePredicate() As Boolean
            Return SelectedDevice IsNot Nothing
        End Function
#End Region

        ''' <summary>
        ''' Is in action
        ''' </summary>
        Private inAction As Boolean
        ''' <summary>
        ''' Picked control
        ''' </summary>
        Private pickedContol As FrameworkElement
        ''' <summary>
        ''' Picked data
        ''' </summary>
        Private pickedData As DeviceViewModel = SelectedDevice
        ''' <summary>
        ''' New active link
        ''' </summary>
        Private activeLink As LinkViewModel
        ''' <summary>
        ''' Last valid position of picked control
        ''' </summary>
        Private lastPosition As Point?

#Region "Commands"
        Public Property CancelActionCommand As ICommand
        Public Property ApplyActionCommand As ICommand
        Public Property MoveCommand As ICommand
        Public Property StartMoveCommand As ICommand
#End Region

        Private Sub Grid_PointerReleased()
            If pickedData IsNot Nothing And pickedData IsNot FakeDeviceViewModel.Fake Then
                SelectedDevice = pickedData
            Else
                SelectedDevice = Nothing
            End If
            If inAction = True And pickedContol IsNot Nothing Then
                Me.inAction = False
                Me.pickedContol = Nothing
                Me.pickedData = Nothing
                Me.lastPosition = Nothing

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

            End If
        End Sub

        Private Sub Grid_PointerMoved(cursorPosition As Point)
            If inAction = True And pickedData IsNot Nothing Then
                If cursorPosition.X >= 0 AndAlso cursorPosition.Y >= 0 Then
                    Me.pickedData.Position = New Point(cursorPosition.X - 35, cursorPosition.Y - 35)
                End If
            End If
        End Sub

        Private Sub Grid_PointerExited()
            If inAction = True And pickedData IsNot Nothing Then
                If lastPosition IsNot Nothing Then
                    Me.pickedData.Position = lastPosition
                    '''AddHandler pickedContol.PointerPressed, AddressOf Image_PointerPressed
                Else
                    If pickedData IsNot FakeDeviceViewModel.Fake Then
                        Lab.Devices.Remove(pickedData)
                    End If
                End If
            End If
            Me.inAction = False
            Me.pickedContol = Nothing
            Me.pickedData = Nothing
            Me.lastPosition = Nothing
            If activeLink IsNot Nothing Then
                Me.Lab.Links.Remove(Me.activeLink)
                Me.activeLink = Nothing
            End If

            Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

        End Sub

        Private Sub Image_PointerPressed(sender As Object)
            If inAction = False Then
                Dim kk = CType(sender, PointerRoutedEventArgs)
                Dim control = CType(kk.OriginalSource, FrameworkElement)
                If TypeOf control.DataContext Is DeviceViewModel Then
                    Me.inAction = True
                    Me.pickedContol = control
                    Me.pickedData = CType(control.DataContext, DeviceViewModel)
                    Me.lastPosition = Me.pickedData.Position

                    Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1)

                End If
            Else
                If activeLink IsNot Nothing Then
                    Dim kk = CType(sender, PointerRoutedEventArgs)
                    Dim control = CType(kk.OriginalSource, FrameworkElement)
                    If TypeOf control.DataContext Is DeviceViewModel Then
                        Dim pickedData = CType(control.DataContext, DeviceViewModel)
                        If activeLink.ItemA Is FakeDeviceViewModel.Fake Then
                            activeLink.ItemA = pickedData
                        Else
                            If activeLink.ItemB Is FakeDeviceViewModel.Fake Then
                                activeLink.ItemB = pickedData
                            End If
                            inAction = False
                            activeLink = Nothing
                            pickedData = Nothing
                            Lab.Devices.Remove(FakeDeviceViewModel.Fake)

                            Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub MenuFlyoutItem_Click(linkType As Type)
            If activeLink IsNot Nothing Then
                Lab.Devices.Remove(FakeDeviceViewModel.Fake)
                Lab.Links.Remove(activeLink)
                activeLink.ItemA = Nothing
                activeLink.ItemB = Nothing
                activeLink = Nothing
                pickedData = Nothing
            End If
            If inAction = False And activeLink Is Nothing Then
                inAction = True
                activeLink = Activator.CreateInstance(linkType)
                activeLink.ItemA = FakeDeviceViewModel.Fake
                activeLink.ItemB = FakeDeviceViewModel.Fake
                pickedData = FakeDeviceViewModel.Fake
                Lab.Devices.Add(FakeDeviceViewModel.Fake)
                Lab.Links.Add(activeLink)

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Cross, 1)

            End If
        End Sub

    End Class
End Namespace
