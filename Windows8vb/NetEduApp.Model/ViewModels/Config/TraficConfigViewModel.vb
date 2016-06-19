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
        Public ReadOnly Property StartSymCommand As RelayCommand
        Public ReadOnly Property StopSymCommand As RelayCommand

        Public Sub New(navigationHelper As NavigationHelper)
            Me._NavigationHelper = navigationHelper

            Me.SaveCommand = New RelayCommand(AddressOf SaveAction, AddressOf CanSave)
            Me.CancelCommand = New RelayCommand(AddressOf CancelAction, AddressOf CanManagePackets)
            Me.NewCommand = New RelayCommand(AddressOf NewAction, AddressOf CanManagePackets)
            Me.DeleteCommand = New RelayCommand(AddressOf DeleteAction, AddressOf CanDelete)
            Me.StartSymCommand = New RelayCommand(AddressOf StartSymAction, AddressOf CanStartSym)
            Me.StopSymCommand = New RelayCommand(AddressOf StopSymAction, AddressOf CanStopSym)
        End Sub

        Private Sub RaiseAllCanExecuteChanged()
            StartSymCommand.RaiseCanExecuteChanged()
            StopSymCommand.RaiseCanExecuteChanged()
            SaveCommand.RaiseCanExecuteChanged()
            CancelCommand.RaiseCanExecuteChanged()
            NewCommand.RaiseCanExecuteChanged()
            DeleteCommand.RaiseCanExecuteChanged()
        End Sub

#Region "Commands"

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
                newPacket.Protocol = Protocol
                Me.Packets.Add(newPacket)
            End If
            Me.Lab.TestPackets = Packets
            Me.OnPacketsChanged()
        End Sub

        Protected Overridable Function CanSave() As Boolean
            Dim ipAddress, targetAddress As NetIpAddress
            Return CanManagePackets() AndAlso
                NetIpAddress.TryParse(Me.SourceAddress, ipAddress) AndAlso
                NetIpAddress.TryParse(Me.TargetAddress, targetAddress) AndAlso
                Not String.IsNullOrEmpty(Protocol)
        End Function

        Protected Overridable Sub CancelAction()
            SelectedPacket = Nothing
        End Sub

        Protected Sub NewAction()
            Me.SelectedPacket = Nothing
        End Sub

        Protected Sub DeleteAction()
            Me.Packets.Remove(Me.SelectedPacket)
            Me.Lab.TestPackets = Packets
            Me.OnPacketsChanged()
        End Sub

        Protected Function CanDelete() As Boolean
            Return CanManagePackets() AndAlso
                Me.SelectedPacket IsNot Nothing
        End Function

        Dim SimulationRunning As Boolean

        Protected Sub StartSymAction()
            SimulationRunning = True
            Me.OnSymStarted()
        End Sub

        Protected Function CanStartSym() As Boolean
            Return Packets IsNot Nothing AndAlso
                Packets.Count > 0 AndAlso
                Not CanStopSym()
        End Function

        Protected Sub StopSymAction()
            SimulationRunning = False
            Me.OnSymStopped()
        End Sub

        Protected Function CanStopSym() As Boolean
            Return SimulationRunning
        End Function

        Protected Function CanManagePackets() As Boolean
            Return Not CanStopSym()
        End Function

#End Region

#Region "Navigation properties"

        Public ReadOnly Property NavigationHelper As NavigationHelper
            Get
                Return _NavigationHelper
            End Get
        End Property

        Public Property Lab As Laboratory
            Get
                Return _Lab
            End Get
            Set(value As Laboratory)
                _Lab = value
                OnLabChanged()
            End Set
        End Property

        Public Property Packets As ObservableCollection(Of PacketViewModel)
            Get
                Return Me._Packets
            End Get
            Set(value As ObservableCollection(Of PacketViewModel))
                Me._Packets = value
                Me.OnPacketsChanged()
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

#End Region

#Region "Packet properties"
        Public Property SourceAddress As String
            Get
                Return Me._IpAddress
            End Get
            Set(value As String)
                Me._IpAddress = value
                Me.OnPacketChanged()
                Me.RaisePropertyChanged(NameOf(SourceAddress))
            End Set
        End Property

        Public Property TargetAddress As String
            Get
                Return Me._TargetAddress
            End Get
            Set(value As String)
                Me._TargetAddress = value
                Me.OnPacketChanged()
                Me.RaisePropertyChanged(NameOf(TargetAddress))
            End Set
        End Property

        Public Property Protocol As String
            Get
                Return Me._Protocol
            End Get
            Set(value As String)
                Me._Protocol = value
                Me.OnPacketChanged()
                Me.RaisePropertyChanged(NameOf(Protocol))
            End Set
        End Property
#End Region

#Region "Events"

        Protected Overridable Sub OnLabChanged()
            If Lab IsNot Nothing Then
                Packets = New ObservableCollection(Of PacketViewModel)(Lab.TestPackets)
            Else
                Packets = Nothing
            End If
            SelectedPacket = Nothing
            Me.OnPacketChanged()
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

            Me.OnPacketChanged()
            Me.NewCommand.RaiseCanExecuteChanged()
            Me.DeleteCommand.RaiseCanExecuteChanged()
            Me.RaisePropertyChanged(NameOf(SelectedPacket))
        End Sub

        Protected Overridable Sub OnPacketsChanged()
            RaiseAllCanExecuteChanged()
        End Sub

        Protected Overridable Sub OnPacketChanged()
            Me.SaveCommand.RaiseCanExecuteChanged()
        End Sub

        Protected Overridable Sub OnSymStarted()
            RaiseAllCanExecuteChanged()
        End Sub

        Protected Overridable Sub OnSymStopped()
            RaiseAllCanExecuteChanged()
        End Sub
#End Region

    End Class
End Namespace
