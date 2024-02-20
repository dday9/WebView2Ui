Imports System.Runtime.InteropServices

Namespace WebView2Ui.Domain.Services.Loggers
    Public Module LoggerModule

        Public Function CreateLogger(applicationName As String, loggerType As String) As ILogger
            Dim logger As ILogger

            If (loggerType.Equals("windows", StringComparison.OrdinalIgnoreCase)) Then
                If (Not RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Then
                    Throw New ArgumentOutOfRangeException(NameOf(loggerType), "The current operating system is not Windows.")
                End If
                logger = New WindowsEventLogger()
            ElseIf (loggerType.Equals("console", StringComparison.OrdinalIgnoreCase)) Then
                logger = New ConsoleLogger
            Else
                logger = New DebugLogger
            End If

            logger.ApplicationName = applicationName

            Return logger
        End Function


    End Module
End Namespace
