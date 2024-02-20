Imports System.Reflection
Imports Microsoft.VisualBasic.ApplicationServices
Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.ORM
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Namespace My
    Partial Friend Class MyApplication

        Public Property DebuggerLogger As ILogger
        Public Property Logger As ILogger
        Public Property Database As IDatabase

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            Dim applicationName = Assembly.GetExecutingAssembly().GetName().Name
            DebuggerLogger = LoggerModule.CreateLogger(applicationName, "debugger")
            Logger = LoggerModule.CreateLogger(applicationName, "console")
            Database = OrmModule.CreateDatabase(applicationName, String.Empty, Logger, String.Empty)
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim message = e.Exception?.Message
            Try
                Logger.LogError(message)
            Catch ex As Exception
                ' HACK: if the logger fails, fallback on the debugging logger which will never fail
                DebuggerLogger.LogError(message)
            End Try
        End Sub

    End Class
End Namespace
