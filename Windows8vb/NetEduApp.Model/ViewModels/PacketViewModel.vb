Imports NetEduApp.Model.Common
Imports NetEduApp.Simulator

Namespace ViewModels
    Public Class PacketViewModel
        Inherits ViewModelBase

        Private _SourceAddress As NetIpAddress
        Private _TargetAddress As NetIpAddress
        Private _Protocol As String
        Private _Status As String = "Nie wysłany"

        Public Property SourceAddress As NetIpAddress
            Get
                Return _SourceAddress
            End Get
            Set(value As NetIpAddress)
                _SourceAddress = value
                RaisePropertyChanged()
            End Set
        End Property

        Public Property TargetAddress As NetIpAddress
            Get
                Return _TargetAddress
            End Get
            Set(value As NetIpAddress)
                _TargetAddress = value
                RaisePropertyChanged()
            End Set
        End Property

        Public Property Protocol As String
            Get
                Return _Protocol
            End Get
            Set(value As String)
                _Protocol = value
                RaisePropertyChanged()
            End Set
        End Property

        Public Property Status As String
            Get
                Return _Status
            End Get
            Set(value As String)
                _Status = value
                RaisePropertyChanged()
            End Set
        End Property

    End Class
End Namespace
