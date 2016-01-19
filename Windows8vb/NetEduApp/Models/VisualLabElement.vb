Namespace Models
    Public MustInherit Class VisualLabElement
        Implements INotifyPropertyChanged
        Implements IDisposable

        Private _Position As Point
        Private _Name As String
        Private _IsSelected As Boolean

        Protected MustOverride Function GetNames() As ICollection(Of String)
        Protected MustOverride Function GetNamePattern() As String
        Public MustOverride ReadOnly Property ImagePath As String

        Public Sub New()
            Name = GetNewName()
        End Sub

        Protected Overridable Function GetNewName() As String
            Dim pattern = GetNamePattern()
            Dim names = GetNames()
            Dim i = 0
            Dim newName = String.Format(pattern, i)
            While names.Contains(newName)
                i = i + 1
                newName = String.Format(pattern, i)
            End While
            Return newName
        End Function

        Protected Sub RaisePropertyChanged(PropertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dim Names = GetNames()
            If Names.Contains(Name) Then
                Names.Remove(Name)
            End If
        End Sub

        Public Property Position As Point
            Get
                Return _Position
            End Get
            Set(value As Point)
                _Position = value
                RaisePropertyChanged("Position")
            End Set
        End Property

        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                Dim names = GetNames()
                If Not names.Contains(value) Then
                    names.Remove(_Name)
                    names.Add(value)
                    _Name = value
                    RaisePropertyChanged("Name")
                End If
            End Set
        End Property

        Public Property IsSelected As Boolean
            Get
                Return _IsSelected
            End Get
            Set(value As Boolean)
                If Not _IsSelected = value Then
                    _IsSelected = value
                    RaisePropertyChanged("IsSelected")
                End If
            End Set
        End Property
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
