Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.Models
Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.ORM

Namespace WebView2Ui.Domain.Services

    Public Class ExampleService

        Public Sub New(database As IDatabase)
            _database = database
        End Sub

        Private ReadOnly _database As IDatabase

        Public Function GetRecordById(id As Guid) As ExampleModel
            Dim record = _database.Examples.FirstOrDefault(Function(item) item.Id.Equals(id))
            Return record
        End Function

        Public Function GetAll() As IEnumerable(Of ExampleModel)
            Dim records = _database.Examples
            Return records
        End Function

        Public Function Create(model As ExampleModel) As ExampleModel
            model.Id = Guid.NewGuid()
            _database.Examples.Add(model)
            Return model
        End Function

        Public Function Update(model As ExampleModel) As ExampleModel
            Dim record = GetRecordById(model.Id)
            If (record Is Nothing) Then
                Throw New ArgumentOutOfRangeException(NameOf(model), $"The example does not exist with id: {model.Id}")
            End If
            Dim index = _database.Examples.IndexOf(record)
            _database.Examples.Item(index).ExampleName = model.ExampleName
            _database.Examples.Item(index).OrdinalIndex = model.OrdinalIndex

            Return model
        End Function

    End Class

End Namespace
