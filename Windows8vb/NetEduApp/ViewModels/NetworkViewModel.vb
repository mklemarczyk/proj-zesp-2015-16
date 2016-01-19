﻿Imports NetEduApp.Emulators.Network
Imports NetEduApp.Models

Namespace ViewModels
    Public Class NetworkViewModel

        Private names As HashSet(Of String)

        Public Sub New()
            'NavigationService = CType(App.Current, App).NavigationService
            'Lab = New Laboratory
            'CreateHubCommand = New DelegateCommand(AddressOf CreateHubAction)
            'CreateSwitchCommand = New DelegateCommand(AddressOf CreateSwitchAction)
            'CreateRouterCommand = New DelegateCommand(AddressOf CreateRouterAction)
            'CreateComputerCommand = New DelegateCommand(AddressOf CreateComputerAction)
            'CreateLinkCommand = New DelegateCommand(AddressOf CreateLinkAction)

            'EditCommand = New DelegateCommand(AddressOf EditAction, AddressOf CanEditPredicate)
            'DeleteCommand = New DelegateCommand(AddressOf DeleteAction, AddressOf CanDeletePredicate)
        End Sub

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
        Private Sub CreateLinkAction()
            Lab.NewEthernetLink()
        End Sub

        Private Sub EditAction()

        End Sub
        Private Function CanEditPredicate() As Boolean
            Return SelectedDevice IsNot Nothing
        End Function

        Private Sub DeleteAction()
            Lab.RemoveDevice(SelectedDevice)
            SelectedDevice = Nothing
        End Sub
        Private Function CanDeletePredicate() As Boolean
            Return SelectedDevice IsNot Nothing
        End Function

        Public Property CreateHubCommand As ICommand
        Public Property CreateSwitchCommand As ICommand
        Public Property CreateRouterCommand As ICommand
        Public Property CreateComputerCommand As ICommand

        Public Property CreateLinkCommand As ICommand
        Public Property EditCommand As ICommand
        Public Property DeleteCommand As ICommand

        Private _SelectedDevice As VisualLabElement
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
                    'EditCommand.RaiseCanExecuteChanged()
                    'DeleteCommand.RaiseCanExecuteChanged()
                End If
            End Set
        End Property

        Public Property Lab As Laboratory
    End Class
End Namespace
