Namespace WebView2Ui.Domain.Services.Loggers
    Public Interface ILogger

        Property ApplicationName As String

        Sub LogError(description As String)
        Sub LogInformation(description As String)
        Sub LogWarning(description As String)

    End Interface
End Namespace
