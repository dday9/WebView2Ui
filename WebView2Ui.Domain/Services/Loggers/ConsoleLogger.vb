Namespace WebView2Ui.Domain.Services.Loggers
    Public Class ConsoleLogger
        Implements ILogger

        Public Property ApplicationName As String Implements ILogger.ApplicationName

        Public Sub LogError(description As String) Implements ILogger.LogError
            WriteConsoleLog(description, "Error")
        End Sub

        Public Sub LogInformation(description As String) Implements ILogger.LogInformation
            WriteConsoleLog(description, "Information")
        End Sub

        Public Sub LogWarning(description As String) Implements ILogger.LogWarning
            WriteConsoleLog(description, "Warning")
        End Sub

        Private Sub WriteConsoleLog(description As String, logType As String)
            Console.WriteLine(logType)
            Console.WriteLine(description)
        End Sub

    End Class
End Namespace
