Imports NetEduApp.Common
Imports NetEduApp.Emulators.Network
Imports NetEduApp.Models

Namespace ViewModels
    Public Class NetworkViewModel

        Private names As HashSet(Of String)
        Private _SelectedDevice As VisualLabElement

#Region "New()"
        Public Sub New()
            'NavigationService = CType(App.Current, App).NavigationService
            Lab = New Laboratory
            CreateHubCommand = New RelayCommand(AddressOf CreateHubAction)
            CreateSwitchCommand = New RelayCommand(AddressOf CreateSwitchAction)
            CreateRouterCommand = New RelayCommand(AddressOf CreateRouterAction)
            CreateComputerCommand = New RelayCommand(AddressOf CreateComputerAction)
            CreateLinkCommand = New RelayCommand(AddressOf CreateEthernetLinkAction)

            EditCommand = New RelayCommand(AddressOf EditAction, AddressOf CanEditPredicate)
            DeleteCommand = New RelayCommand(AddressOf DeleteAction, AddressOf CanDeletePredicate)
        End Sub
#End Region

#Region "Properties"
        Public Property SelectedDevice As VisualLabElement
            Get
                Return _SelectedDevice
            End Get
            Set(value As VisualLabElement)
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
        Public Property CreateHubCommand As ICommand
        Public Property CreateSwitchCommand As ICommand
        Public Property CreateRouterCommand As ICommand
        Public Property CreateComputerCommand As ICommand

        Public Property CreateLinkCommand As ICommand
        Public Property EditCommand As RelayCommand
        Public Property DeleteCommand As RelayCommand
#End Region

#Region "Create device commands"
        Private Sub CreateHubAction()
            Lab.NewHub()
        End Sub
        Private Sub CreateSwitchAction()
            Lab.NewSwitch()
        End Sub
        Private Sub CreateRouterAction()
            Lab.NewRouter()
        End Sub
        Private Sub CreateComputerAction()
            Lab.NewComputer()
        End Sub
#End Region

#Region "Create link commands"
        Private Sub CreateEthernetLinkAction()
            Lab.NewEthernetLink()
        End Sub

        Private Sub CreateCoaxialLinkAction()
            Lab.NewCoaxialLink()
        End Sub
        Private Sub CreateEthernetCrossoverLinkAction()
            Lab.NewEthernetCrossoverLink()
        End Sub

        Private Sub CreateOpticalFiberLinkAction()
            Lab.NewOpticalFiberLink()
        End Sub

        Private Sub CreateSerialLinkAction()
            Lab.NewSerialLink()
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

    End Class
End Namespace
