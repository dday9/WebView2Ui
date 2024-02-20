Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.Models
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Namespace WebView2Ui.Domain.Database.ORM
    Public Interface IDatabase

        Property ConnectionString As String
        Property Logger As ILogger

        ' tables
        Property Examples As List(Of ExampleModel)

    End Interface
End Namespace
