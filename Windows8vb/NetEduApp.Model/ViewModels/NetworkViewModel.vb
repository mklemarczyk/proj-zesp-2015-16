Imports NetEduApp.Model.Common
Imports NetEduApp.Model.Services
Imports Windows.Security.Cryptography
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Streams

Namespace ViewModels
    Public Class NetworkViewModel
        Inherits ViewModelBase

        Private names As HashSet(Of String)
        Private _SelectedDevice As DeviceViewModel
        Private _EditDevice As DeviceViewModel
        Private _ActiveLab As Laboratory

#Region "New()"
        Public Sub New()
            Lab = New Laboratory

            LoadCommand = New RelayCommand(AddressOf LoadAction)
            SaveCommand = New RelayCommand(AddressOf SaveAction)

            CreateComputerCommand = New RelayCommand(AddressOf CreateComputerAction)
            CreateHubCommand = New RelayCommand(AddressOf CreateHubAction)
            CreateRouterCommand = New RelayCommand(AddressOf CreateRouterAction)

            CreateEthernetLinkCommand = New RelayCommand(AddressOf CreateEthernetLinkAction)

            EditCommand = New RelayCommand(AddressOf EditAction, AddressOf CanEditPredicate)
            DeleteCommand = New RelayCommand(AddressOf DeleteAction, AddressOf CanDeletePredicate)
            SendPacketCommand = New RelayCommand(AddressOf SendPacketAction)

            ApplyActionCommand = New RelayCommand(AddressOf Grid_PointerReleased)
            CancelActionCommand = New RelayCommand(AddressOf Grid_PointerExited)
            MoveCommand = New RelayCommand(Of Point)(AddressOf Grid_PointerMoved)
            StartMoveCommand = New RelayCommand(Of Object)(AddressOf Image_PointerPressed)
            InterfaceSelectCommand = New RelayCommand(Of Object)(AddressOf Button_InterfaceSelected)
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
                    EditDevice = Nothing
                    EditCommand.RaiseCanExecuteChanged()
                    DeleteCommand.RaiseCanExecuteChanged()
                    RaisePropertyChanged("SelectedDevice")
                End If
            End Set
        End Property

        Public Property EditDevice As DeviceViewModel
            Get
                Return _EditDevice
            End Get
            Set(value As DeviceViewModel)
                If _EditDevice IsNot value Then
                    _EditDevice = value
                    EditCommand.RaiseCanExecuteChanged()
                    DeleteCommand.RaiseCanExecuteChanged()
                    RaisePropertyChanged("EditDevice")
                End If
            End Set
        End Property

        Public Property ActiveLab As Laboratory
            Get
                Return _ActiveLab
            End Get
            Set(value As Laboratory)
                If _ActiveLab IsNot value Then
                    _ActiveLab = value
                    RaisePropertyChanged(NameOf(ActiveLab))
                End If
            End Set
        End Property

        Public Property Lab As Laboratory
#End Region

#Region "Commands"
        Public Property LoadCommand As RelayCommand
        Public Property SaveCommand As RelayCommand

        Public Property CreateComputerCommand As ICommand
        Public Property CreateHubCommand As ICommand
        Public Property CreateRouterCommand As ICommand

        Public Property CreateEthernetLinkCommand As ICommand

        Public Property EditCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
        Public Property SendPacketCommand As RelayCommand
#End Region

#Region "Create device commands"
        Private Sub CreateComputerAction()
            Lab.NewComputer()
        End Sub
        Private Sub CreateHubAction()
            Lab.NewHub()
        End Sub
        Private Sub CreateRouterAction()
            Lab.NewRouter()
        End Sub
#End Region

#Region "Load/Save actions"
        Private Async Sub LoadAction()
            Try
                Dim jsonString As String

                Dim fileOpenPicker As FileOpenPicker = New FileOpenPicker()
                fileOpenPicker.ViewMode = PickerViewMode.Thumbnail
                fileOpenPicker.FileTypeFilter.Add(".netproj")
                fileOpenPicker.CommitButtonText = "Wczytaj projekt"
                fileOpenPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                Dim file = Await fileOpenPicker.PickSingleFileAsync()

                If file IsNot Nothing Then
                    jsonString = Await FileIO.ReadTextAsync(file)

                    Services.ExportService.InterprateJson(jsonString, Me.Lab)

                    RaisePropertyChanged(NameOf(Lab))
                    NotificationService.ShowToast("Projekt wczytano z pliku")
                End If
            Catch ex As Exception
                Me.Lab.Dispose()
                Me.Lab = New Laboratory()
                RaisePropertyChanged(NameOf(Lab))
                NotificationService.ShowToast("Niepowodzenie\nNie można wczytać pliku")
            End Try
        End Sub
        Private Async Sub SaveAction()
            Try
                Dim jsonString As String = String.Empty
                Services.ExportService.GenerateJson(Me.Lab, jsonString)

                Dim fileSavePicker = New FileSavePicker()
                fileSavePicker.FileTypeChoices.Add("Projekt sieci", New String() {".netproj"})
                fileSavePicker.CommitButtonText = "Zapisz projekt"
                fileSavePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                Dim file = Await fileSavePicker.PickSaveFileAsync()

                If file IsNot Nothing Then
                    Await FileIO.WriteTextAsync(file, jsonString)
                    NotificationService.ShowToast("Projekt zapisano do pliku")
                End If
            Catch ex As Exception
                NotificationService.ShowToast("Niepowodzenie\nNie można zapisać pliku")
            End Try
        End Sub
#End Region

#Region "Create link commands"
        Private Sub CreateEthernetLinkAction()
            MenuFlyoutItem_Click(GetType(EthernetLinkViewModel))
        End Sub
#End Region

#Region "Edit action"
        Private Sub EditAction()
            If EditDevice Is Nothing Then
                EditDevice = SelectedDevice
            Else
                EditDevice = Nothing
            End If
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

#Region "Send packet action"
        Private Sub SendPacketAction()
            If ActiveLab Is Nothing Then
                ActiveLab = Lab
            Else
                ActiveLab = Nothing
            End If
        End Sub
#End Region

        ''' <summary>
        ''' Is in action
        ''' </summary>
        Private currentAction As LabAction
        ''' <summary>
        ''' Picked data
        ''' </summary>
        Private pickedData As DeviceViewModel
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
        Public Property InterfaceSelectCommand As ICommand
#End Region

        Private Sub ResetState()
            If Me.currentAction <> LabAction.Idle Then
                If Me.currentAction = LabAction.CreateLink Then
                    Me.Lab.Devices.Remove(FakeDeviceViewModel.Fake)
                    Me.Lab.Links.Remove(activeLink)
                    Me.activeLink.ItemA = Nothing
                    Me.activeLink.ItemB = Nothing
                    Me.activeLink = Nothing
                ElseIf Me.currentAction = LabAction.MoveDevice Then
                    Me.pickedData.Position = Me.lastPosition
                    Me.lastPosition = Nothing
                End If

                Me.pickedData = Nothing
                Me.currentAction = LabAction.Idle

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)
            End If
        End Sub

        Private Sub Grid_PointerReleased()
            If Me.currentAction = LabAction.MoveDevice Then
                Me.currentAction = LabAction.Idle

                Me.pickedData = Nothing
                Me.lastPosition = Nothing

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)

            End If
        End Sub

        Private Sub Grid_PointerMoved(cursorPosition As Point)
            If Me.currentAction = LabAction.MoveDevice Then
                If cursorPosition.X >= 0 AndAlso cursorPosition.Y >= 0 Then
                    Me.pickedData.Position = New Point(cursorPosition.X - 35, cursorPosition.Y - 35)
                End If
            ElseIf Me.currentAction = LabAction.CreateLink Then
                If cursorPosition.X >= 0 AndAlso cursorPosition.Y >= 0 Then
                    FakeDeviceViewModel.Fake.Position = New Point(cursorPosition.X - 35, cursorPosition.Y - 35)
                End If
            End If
        End Sub

        Private Sub Grid_PointerExited()
            If pickedData IsNot Nothing Then
                If pickedData.IsInterfacesVisible Then
                    AddHandler pickedData.PropertyChanged, AddressOf ResetIfFlyoutClosed
                Else
                    ResetState()
                End If
            Else
                ResetState()
            End If
        End Sub

        Private Sub ResetIfFlyoutClosed(s As Object, e As PropertyChangedEventArgs)
            If pickedData IsNot Nothing Then
                RemoveHandler pickedData.PropertyChanged, AddressOf ResetIfFlyoutClosed
            End If
            ResetState()
        End Sub

        Private Sub Image_PointerPressed(sender As Object)
            If Me.currentAction = LabAction.Idle Then

                Me.currentAction = LabAction.MoveDevice
                Dim args = CType(sender, PointerRoutedEventArgs)
                Dim control = CType(args.OriginalSource, FrameworkElement)
                Me.pickedData = CType(control.DataContext, DeviceViewModel)
                Me.lastPosition = Me.pickedData.Position
                Me.SelectedDevice = Me.pickedData

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1)

            ElseIf Me.currentAction = LabAction.CreateLink Then
                Dim args = CType(sender, PointerRoutedEventArgs)
                Dim control = CType(args.OriginalSource, FrameworkElement)
                Dim pickedData = CType(control.DataContext, DeviceViewModel)

                Me.pickedData = pickedData

                If activeLink.ItemA Is FakeDeviceViewModel.Fake Then
                    pickedData.IsInterfacesVisible = True
                Else
                    If activeLink.ItemB Is FakeDeviceViewModel.Fake Then
                        pickedData.IsInterfacesVisible = True
                    End If
                End If
            End If
        End Sub

        Private Sub Button_InterfaceSelected(sender As Object)
            If pickedData IsNot Nothing Then
                RemoveHandler pickedData.PropertyChanged, AddressOf ResetIfFlyoutClosed
            End If

            If Me.currentAction = LabAction.CreateLink Then
                If activeLink.ItemA Is FakeDeviceViewModel.Fake Then
                    activeLink.ItemA = pickedData
                    activeLink.InterfA = DirectCast(sender, String)
                    pickedData.IsInterfacesVisible = False
                Else
                    If activeLink.ItemB Is FakeDeviceViewModel.Fake Then
                        activeLink.ItemB = pickedData
                        activeLink.InterfB = DirectCast(sender, String)
                        pickedData.IsInterfacesVisible = False

                        activeLink.ItemA.RemoveInterface(activeLink.InterfA)
                        activeLink.ItemB.RemoveInterface(activeLink.InterfB)

                        Me.currentAction = LabAction.Idle
                        activeLink = Nothing
                        Lab.Devices.Remove(FakeDeviceViewModel.Fake)

                        Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1)
                    End If
                End If
            End If
        End Sub

        Private Sub MenuFlyoutItem_Click(linkType As Type)
            ResetState()
            If Me.currentAction = LabAction.Idle Then

                Me.currentAction = LabAction.CreateLink
                activeLink = Activator.CreateInstance(linkType)
                activeLink.ItemA = FakeDeviceViewModel.Fake
                activeLink.ItemB = FakeDeviceViewModel.Fake
                pickedData = FakeDeviceViewModel.Fake
                Lab.Devices.Add(FakeDeviceViewModel.Fake)
                Lab.Links.Add(activeLink)

                Window.Current.CoreWindow.PointerCursor = New Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Cross, 1)

            End If
        End Sub

        Private Enum LabAction
            Idle
            MoveDevice
            CreateLink
        End Enum
    End Class
End Namespace
