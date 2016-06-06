Imports NetEduApp.Model.Common

Namespace ViewModels
	Public MustInherit Class DeviceViewModel
		Inherits ViewModelBase
		Implements IDisposable

		Private _Position As Point
		Private _Name As String
		Private _IsSelected As Boolean
		Private _IsInterfacesVisible As Boolean
		Friend ParentLab As Laboratory
		Friend Settings As Dictionary(Of String, String)
		Protected MustOverride Function GetNamePattern() As String
		Public MustOverride ReadOnly Property ImagePath As String

		Public ReadOnly Property Interfaces As List(Of String) = New List(Of String)
		Public ReadOnly Property VisibleInterfaces As ObservableCollection(Of String) = New ObservableCollection(Of String)

		Public Sub New(lab As Laboratory)
			If lab IsNot Nothing Then
				ParentLab = lab
				Name = GetNewName()
				ParentLab.RegisterMyName(Me, Nothing)
			End If
		End Sub

		Protected Overridable Function GetNewName() As String
			Dim pattern = GetNamePattern()
			Dim i = 0
			Dim newName = String.Format(pattern, i)
			While ParentLab.IsNameExist(newName)
				i = i + 1
				newName = String.Format(pattern, i)
			End While
			Return newName
		End Function

		Public Sub Dispose() Implements IDisposable.Dispose
			ParentLab = Nothing
		End Sub

		Public Property Position As Point
			Get
				Return _Position
			End Get
			Set(value As Point)
				_Position = value
				RaisePropertyChanged()
			End Set
		End Property

		Public Property Name As String
			Get
				Return _Name
			End Get
			Set(value As String)
				If Not ParentLab.IsNameExist(value) Then
					ParentLab.RegisterMyName(Me, _Name)
					_Name = value
					RaisePropertyChanged()
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
					RaisePropertyChanged()
				End If
			End Set
		End Property

		Public Property IsInterfacesVisible As Boolean
			Get
				Return _IsInterfacesVisible
			End Get
			Set(value As Boolean)
				If Not _IsInterfacesVisible = value Then
					_IsInterfacesVisible = value
					Debug.WriteLine("{0}", value)
					RaisePropertyChanged()
				End If
			End Set
		End Property

		Friend Sub RemoveInterface(interfaceName As String)
			VisibleInterfaces.Remove(interfaceName)
			RaisePropertyChanged("Interfaces")
		End Sub
	End Class
End Namespace
