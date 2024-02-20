Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.Models
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Namespace WebView2Ui.Domain.Database.ORM

    Public Class InMemoryDatabase
        Implements IDatabase

        Sub New(_connectionString As String, _logger As ILogger)
            ConnectionString = _connectionString
            Logger = _logger
            Examples = New List(Of ExampleModel)()
        End Sub

        Public Property ConnectionString As String Implements IDatabase.ConnectionString
        Public Property Logger As ILogger Implements IDatabase.Logger

        ' tables
        Public Property Examples As List(Of ExampleModel) Implements IDatabase.Examples

    End Class

End Namespace
