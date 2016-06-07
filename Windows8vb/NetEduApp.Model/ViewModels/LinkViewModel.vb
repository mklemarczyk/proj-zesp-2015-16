Namespace ViewModels
    Public MustInherit Class LinkViewModel
        Implements INotifyPropertyChanged

        Private _ItemA As DeviceViewModel
        Private _ItemB As DeviceViewModel

        Public Property Color As Brush
        Public Property LineStyle As DoubleCollection


        Protected Sub RaisePropertyChanged(PropertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
        End Sub

        Public Property ItemA As DeviceViewModel
            Get
                Return _ItemA
            End Get
            Set(value As DeviceViewModel)
                If value IsNot _ItemA Then
                    _ItemA = value
                    RaisePropertyChanged("ItemA")
                End If
            End Set
        End Property
        Public Property ItemB As DeviceViewModel
            Get
                Return _ItemB
            End Get
            Set(value As DeviceViewModel)
                If value IsNot _ItemB Then
                    _ItemB = value
                    RaisePropertyChanged("ItemB")
                End If
            End Set
        End Property

		Public Property InterfA As String
		Public Property InterfB As String
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
