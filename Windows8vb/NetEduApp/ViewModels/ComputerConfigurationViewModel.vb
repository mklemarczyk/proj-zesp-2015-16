Imports NetEduApp.Emulators.Network
Imports NetEduApp.Models
Imports NetEduApp.Mvvm

Namespace ViewModels
    Public Class ComputerConfigurationViewModel

        Private computer As VisualLabElement

        Private _Name As String
        Private _Address As String
        Private _Netmask As String
        Private _DefaultGateway As String

        Public Sub New(computer As VisualLabElement)
            Me.computer = computer
            'Me.ApplySettingsCommand = New DelegateCommand(AddressOf ApplySettingsAction, AddressOf CanApplySettings)
        End Sub

        Private Sub ApplySettingsAction()
            'computer.Settings = IpAddress
            'computer.Settings = Netmask
            'computer.Settings = DefaultGateway
        End Sub

        Private Function CanApplySettings() As Boolean
            Dim ValidAddress As NetIpAddress = Nothing
            Dim ValidNetmask As NetIpAddress = Nothing
            Dim ValidDefaultGateway As NetIpAddress = Nothing
            If Not String.IsNullOrWhiteSpace(Name) Then
                If NetIpAddress.TryParse(Address, ValidAddress) AndAlso NetIpAddress.TryParse(Address, ValidNetmask) AndAlso NetIpAddress.TryParse(Address, ValidDefaultGateway) Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Property ApplySettingsCommand As ICommand

        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                'RaisePropertyChanged()
            End Set
        End Property
        Public Property Address As String
            Get
                Return _Address
            End Get
            Set(value As String)
                _Address = value
                'RaisePropertyChanged()
            End Set
        End Property
        Public Property Netmask As String
            Get
                Return _Netmask
            End Get
            Set(value As String)
                _Netmask = value
                'RaisePropertyChanged()
            End Set
        End Property
        Public Property DefaultGateway As String
            Get
                Return _DefaultGateway
            End Get
            Set(value As String)
                _DefaultGateway = value
                'RaisePropertyChanged()
            End Set
        End Property
    End Class
End Namespace
