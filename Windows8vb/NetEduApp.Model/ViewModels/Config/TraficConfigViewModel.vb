Imports NetEduApp.Model.Common
Imports NetEduApp.Simulator

Namespace ViewModels.Config
    Public Class TraficConfigViewModel
        Inherits ViewModelBase

        Private _Lab As Laboratory
        Private _NavigationHelper As NavigationHelper

        Private _Packets As ObservableCollection(Of PacketViewModel)
        Private _SelectedPacket As PacketViewModel

        Private _IpAddress As String = String.Empty
        Private _TargetAddress As String = String.Empty
        Private _Protocol As String = Nothing

        Public ReadOnly Property SaveCommand As RelayCommand
        Public ReadOnly Property CancelCommand As RelayCommand

        Public ReadOnly Property NewCommand As RelayCommand
        Public ReadOnly Property DeleteCommand As RelayCommand

        Public Sub New(navigationHelper As NavigationHelper)
            Me._NavigationHelper = navigationHelper
            Me.SaveCommand = New RelayCommand(AddressOf SaveAction, AddressOf CanSave)
            Me.CancelCommand = New RelayCommand(AddressOf CancelAction)

            NewCommand = New RelayCommand(AddressOf NewAction)
            DeleteCommand = New RelayCommand(AddressOf DeleteAction, AddressOf CanDelete)
        End Sub

        Protected Overridable Sub OnLabChanged()
            If Lab IsNot Nothing Then
                Packets = New ObservableCollection(Of PacketViewModel)(Lab.TestPackets)
            Else
                Packets = Nothing
            End If
            SelectedPacket = Nothing
            Me.SaveCommand.RaiseCanExecuteChanged()
            Me.RaisePropertyChanged(NameOf(Lab))
        End Sub

        Protected Overridable Sub OnSelectedPacketChanged()
            If SelectedPacket IsNot Nothing Then
                Me.SourceAddress = SelectedPacket.SourceAddress.ToString()
                Me.TargetAddress = SelectedPacket.TargetAddress.ToString()
            Else
                Me.SourceAddress = String.Empty
                Me.TargetAddress = String.Empty
            End If

            Me.SaveCommand.RaiseCanExecuteChanged()
            Me.NewCommand.RaiseCanExecuteChanged()
            Me.DeleteCommand.RaiseCanExecuteChanged()
            Me.RaisePropertyChanged(NameOf(SelectedPacket))
        End Sub

        Protected Overridable Sub SaveAction()
            If SelectedPacket IsNot Nothing Then
                Dim sourceAddress, targetAddress As NetIpAddress
                NetIpAddress.TryParse(Me.SourceAddress, sourceAddress)
                NetIpAddress.TryParse(Me.TargetAddress, targetAddress)
                Me.SelectedPacket.SourceAddress = sourceAddress
                Me.SelectedPacket.TargetAddress = targetAddress
                Me.SelectedPacket = Nothing
            Else
                Dim newPacket As PacketViewModel = New PacketViewModel
                Dim sourceAddress, targetAddress As NetIpAddress
                NetIpAddress.TryParse(Me.SourceAddress, sourceAddress)
                NetIpAddress.TryParse(Me.TargetAddress, targetAddress)
                newPacket.SourceAddress = sourceAddress
                newPacket.TargetAddress = targetAddress
                Me.Packets.Add(newPacket)
            End If
            NavigationHelper.GoBack()
        End Sub

        Protected Overridable Function CanSave() As Boolean
            Dim ipAddress, targetAddress As NetIpAddress
            Return NetIpAddress.TryParse(Me.SourceAddress, ipAddress) AndAlso
                NetIpAddress.TryParse(Me.TargetAddress, targetAddress) AndAlso
                Not String.IsNullOrEmpty(Protocol)
        End Function

        Protected Overridable Sub CancelAction()
            SelectedPacket = Nothing
            OnLabChanged()
            NavigationHelper.GoBack()
        End Sub

        Protected Sub NewAction()
            Me.SelectedPacket = Nothing
        End Sub

        Protected Sub DeleteAction()
            Me.Packets.Remove(Me.SelectedPacket)
        End Sub

        Protected Function CanDelete() As Boolean
            Return Me.SelectedPacket IsNot Nothing
        End Function

        Public Property Lab As Laboratory
            Get
                Return _Lab
            End Get
            Set(value As Laboratory)
                If value IsNot Lab Then
                    _Lab = value
                    OnLabChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property NavigationHelper As NavigationHelper
            Get
                Return _NavigationHelper
            End Get
        End Property

        Public Property Packets As ObservableCollection(Of PacketViewModel)
            Get
                Return Me._Packets
            End Get
            Set(value As ObservableCollection(Of PacketViewModel))
                Me._Packets = value
                Me.SaveCommand.RaiseCanExecuteChanged()
                Me.RaisePropertyChanged(NameOf(Packets))
            End Set
        End Property

        Public Property SelectedPacket As PacketViewModel
            Get
                Return Me._SelectedPacket
            End Get
            Set(value As PacketViewModel)
                Me._SelectedPacket = value
                Me.OnSelectedPacketChanged()
            End Set
        End Property

        Public Property SourceAddress As String
            Get
                Return Me._IpAddress
            End Get
            Set(value As String)
                Me._IpAddress = value
                Me.SaveCommand.RaiseCanExecuteChanged()
                Me.RaisePropertyChanged(NameOf(SourceAddress))
            End Set
        End Property

        Public Property TargetAddress As String
            Get
                Return Me._TargetAddress
            End Get
            Set(value As String)
                Me._TargetAddress = value
                Me.SaveCommand.RaiseCanExecuteChanged()
                Me.RaisePropertyChanged(NameOf(TargetAddress))
            End Set
        End Property

        Public Property Protocol As String
            Get
                Return Me._Protocol
            End Get
            Set(value As String)
                Me._Protocol = value
                Me.SaveCommand.RaiseCanExecuteChanged()
                Me.RaisePropertyChanged(NameOf(Protocol))
            End Set
        End Property

    End Class
End Namespace
