Imports System.Runtime.Versioning
Namespace WebView2Ui.Domain.Services.Loggers
    <SupportedOSPlatform("windows")>
    Public Class WindowsEventLogger
        Implements ILogger

        Public Property ApplicationName As String Implements ILogger.ApplicationName

        Public Sub LogError(description As String) Implements ILogger.LogError
            WriteWindowsEventLog(description, "Error")
        End Sub

        Public Sub LogInformation(description As String) Implements ILogger.LogInformation
            WriteWindowsEventLog(description, "Information")
        End Sub

        Public Sub LogWarning(description As String) Implements ILogger.LogWarning
            WriteWindowsEventLog(description, "Warning")
        End Sub

        Private Sub WriteWindowsEventLog(description As String, logType As String)
            Dim logName = "Application"
            AssertEventLogSource(ApplicationName, logName)

            Using windowsEventLog = New EventLog(logName)
                windowsEventLog.Source = ApplicationName
                windowsEventLog.WriteEntry(description, logType)
            End Using
        End Sub

        Private Shared Sub AssertEventLogSource(applicationName As String, logName As String)
            If (String.IsNullOrWhiteSpace(applicationName)) Then
                Throw New ArgumentNullException(NameOf(applicationName))
            End If
            If (String.IsNullOrWhiteSpace(logName)) Then
                Throw New ArgumentNullException(NameOf(logName))
            End If

            If (Not EventLog.SourceExists(applicationName)) Then
                EventLog.CreateEventSource(applicationName, logName)
            End If
        End Sub

    End Class
End Namespace
