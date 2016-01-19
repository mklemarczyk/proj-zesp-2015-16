Namespace Models
    Public MustInherit Class VisualLabLink
        Implements INotifyPropertyChanged

        Private _ItemA As VisualLabElement
        Private _ItemB As VisualLabElement

        Public Property Color As SolidColorBrush


        Protected Sub RaisePropertyChanged(PropertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
        End Sub

        Public Property ItemA As VisualLabElement
            Get
                Return _ItemA
            End Get
            Set(value As VisualLabElement)
                If value IsNot _ItemA Then
                    _ItemA = value
                    RaisePropertyChanged("ItemA")
                End If
            End Set
        End Property
        Public Property ItemB As VisualLabElement
            Get
                Return _ItemB
            End Get
            Set(value As VisualLabElement)
                If value IsNot _ItemB Then
                    _ItemB = value
                    RaisePropertyChanged("ItemB")
                End If
            End Set
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
