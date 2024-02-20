Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.ORM
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Namespace WebView2Ui.Domain.Database.ORM
    Public Module OrmModule

        Public Function CreateDatabase(applicationName As String, connectionString As String, logger As ILogger, databaseType As String) As IDatabase
            Dim database As IDatabase

            ' if we had different database types, we'd setup an if/then jsut like in LoggerModule.CreateLogger
            connectionString = applicationName
            database = New InMemoryDatabase(connectionString, logger)

            Return database
        End Function

    End Module
End Namespace
