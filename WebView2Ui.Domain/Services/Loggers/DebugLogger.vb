Namespace WebView2Ui.Domain.Services.Loggers
    Public Class DebugLogger
        Implements ILogger

        Public Property ApplicationName As String Implements ILogger.ApplicationName

        Public Sub LogError(description As String) Implements ILogger.LogError
            WriteDebugLog(description, "Error")
        End Sub

        Public Sub LogInformation(description As String) Implements ILogger.LogInformation
            WriteDebugLog(description, "Information")
        End Sub

        Public Sub LogWarning(description As String) Implements ILogger.LogWarning
            WriteDebugLog(description, "Warning")
        End Sub

        Private Sub WriteDebugLog(description As String, logType As String)
            Debug.WriteLine(logType)
            Debug.WriteLine(description)
        End Sub

    End Class
End Namespace
