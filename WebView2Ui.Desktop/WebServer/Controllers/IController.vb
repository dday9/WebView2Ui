Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Public Interface IController

    Delegate Function Route(data As WebViewRequest) As WebViewResponse
    ReadOnly Property Routes As Dictionary(Of String, Route)
    ReadOnly Property Logger As ILogger

End Interface
