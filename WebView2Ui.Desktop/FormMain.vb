Imports Microsoft.Web.WebView2.Core
Imports Newtonsoft.Json
Imports System.IO
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services
Imports Microsoft.Web.WebView2.WinForms

Public Class FormMain

    Private ReadOnly _controllerMaps As Dictionary(Of String, IController)

    Sub New()
        InitializeComponent()

        _controllerMaps = BuildControllerMaps()
    End Sub

    Private Async Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Await ConfigureWebView2()
        OpenWebPage(WebView2Container, "WebPages", "Examples", "Index.html")
    End Sub

    ' WebView2 specific
    Private Async Sub WebView2Container_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs) Handles WebView2Container.WebMessageReceived
        Await WebView2Container.EnsureCoreWebView2Async(Nothing)

        Dim message = e.WebMessageAsJson()
        Dim request As WebViewRequest = Nothing
        Dim response = WebViewResponse.InternalServerErrorResponse()
        Try
            request = JsonConvert.DeserializeObject(Of WebViewRequest)(message)
        Catch
            response = WebViewResponse.BadRequestResponse("The payload is not a valid request.")
        End Try

        If (request IsNot Nothing) Then
            If (Not _controllerMaps.ContainsKey(request.Controller)) Then
                response = WebViewResponse.NotFoundResponse()
            Else
                Dim controller = _controllerMaps(request.Controller)
                If (Not controller.Routes.ContainsKey(request.Route)) Then
                    response = WebViewResponse.NotFoundResponse()
                Else
                    Dim route = controller.Routes(request.Route)
                    response = route(request)
                End If
            End If
        End If

        Await WebView2Container.CoreWebView2.ExecuteScriptAsync($"dispatchMessageReceivedEvent({request.ToJson()}, {response.ToJson()});")
    End Sub

    Private Async Function ConfigureWebView2() As Task
        Await WebView2Container.EnsureCoreWebView2Async(Nothing)
        Dim webAssets = AssertApplicationDirectoryPath("WebAssets")
        WebView2Container.CoreWebView2.SetVirtualHostNameToFolderMapping("app-assets.local", webAssets, CoreWebView2HostResourceAccessKind.Allow)
        WebView2Container.CoreWebView2.Settings.IsWebMessageEnabled = True
    End Function

    Private Sub OpenWebPage(container As WebView2, ParamArray filePathParts() As String)
        Dim fileContentPath = AssertApplicationFilePath(filePathParts)
        Dim fileDirectoryPath = Path.GetDirectoryName(fileContentPath)
        Dim url = New Uri(fileContentPath).ToString()
        container.CoreWebView2.SetVirtualHostNameToFolderMapping("page-assets.local", fileDirectoryPath, CoreWebView2HostResourceAccessKind.Allow)
        container.CoreWebView2.Navigate(url)
    End Sub

    Private Shared Function AssertApplicationFilePath(ParamArray filePathParts() As String) As String
        Dim filePaths = {Application.StartupPath}.Concat(filePathParts).ToArray()
        Dim contentPath = Path.Combine(filePaths)
        If (Not File.Exists(contentPath)) Then
            Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
        End If

        Return contentPath
    End Function

    Private Shared Function AssertApplicationDirectoryPath(ParamArray filePathParts() As String) As String
        Dim paths = {Application.StartupPath}.Concat(filePathParts).ToArray()
        Dim contentPath = Path.Combine(paths)
        If (Not Directory.Exists(contentPath)) Then
            Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
        End If
        Return contentPath
    End Function

    ' WebServer specific
    Private Function BuildControllerMaps() As Dictionary(Of String, IController)
        Dim exampleService = New ExampleService(My.Application.Database)
        Dim examplesController = New ExamplesController(My.Application.Logger, exampleService)
        Return New Dictionary(Of String, IController) From {
            {"Examples", examplesController}
        }
    End Function

End Class
