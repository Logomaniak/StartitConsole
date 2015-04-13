
Imports System.Threading.Tasks
Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization

Public Class _default
    Inherits System.Web.UI.Page

    Private otp As String = Nothing
    Private uri As String = "https://s2sc-stage.desktopmate.net/api/otp/verify/:"
    Private DM_ApiKey As String = "7a7a4933-ad1b-4758-9fb9-95782c495dd6-stage"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.otp = Request("otp")
        If Len(Trim(otp)) > 0 Then
            getRequest()
        End If

        postRequest()

    End Sub


    Public Sub getRequest()

        ' Create a request for the URL. 
        Dim request As WebRequest = WebRequest.Create(uri & Me.otp)
        ' If required by the server, set the credentials.
        'request.Credentials = CredentialCache.DefaultCredentials
        request.Headers.Add("Authorization", DM_ApiKey)
        request.Method = "GET"
        ' Get the response.
        Dim response As WebResponse = request.GetResponse()
        ' Display the status.
        '"{"status":{"returnCode":400,"returnMessage":"one time password verify, ERROR - Invalid OTP","S2SCversion":"1.23.151001501","serverTime":"2015-04-10T17:21:13.089Z"},"payload":{"err":"Invalid OTP"}}"
        Using reader = New StreamReader(response.GetResponseStream())
            Dim js As New JavaScriptSerializer()
            Dim objText = reader.ReadToEnd()
            Dim myobj As MyObject = CType(js.Deserialize(objText, GetType(MyObject)), MyObject)
        End Using

        response.Close()

    End Sub


    Public Sub postRequest()

        Dim myremotepost As New RemotePost()
        myremotepost.Url = "http://web.fleet2track.it/aut.asp"
        myremotepost.Add("user", "admin")
        myremotepost.Add("pwd", "admincapri")
        myremotepost.Post()

    End Sub

    Public Class RemotePost
        Private Inputs As New System.Collections.Specialized.NameValueCollection()

        Public Url As String = ""
        Public Method As String = "post"
        Public FormName As String = "form1"

        Public Sub Add(ByVal name As String, ByVal value As String)
            Inputs.Add(name, value)
        End Sub

        Public Sub Post()
            System.Web.HttpContext.Current.Response.Clear()

            System.Web.HttpContext.Current.Response.Write("<html><head>")

            System.Web.HttpContext.Current.Response.Write(String.Format("</head><body onload=""document.{0}.submit()"">", FormName))

            System.Web.HttpContext.Current.Response.Write(String.Format("<form name=""{0}"" method=""{1}"" action=""{2}"" >", FormName, Method, Url))
            For i As Integer = 0 To Inputs.Keys.Count - 1
                System.Web.HttpContext.Current.Response.Write(String.Format("<input name=""{0}"" type=""hidden"" value=""{1}"">", Inputs.Keys(i), Inputs(Inputs.Keys(i))))
            Next i
            System.Web.HttpContext.Current.Response.Write("</form>")
            System.Web.HttpContext.Current.Response.Write("</body></html>")
            System.Web.HttpContext.Current.Response.End()
        End Sub
    End Class


    Public Class Status
        Public Property returnCode As Integer
        Public Property returnMessage As String
        Public Property S2SCversion As String
        Public Property serverTime As DateTime
    End Class

    Public Class Payload
        Public Property username As String
        Public Property email As String
        Public Property givenName As String
        Public Property familyName As String
        Public Property pictureUrl As String
        Public Property birthdate As Date
        Public Property gender As String
        Public Property jobTitle As String
        Public Property phoneNumber As String
        Public Property skypeAccount As String
        Public Property roles As New ArrayList
        Public Property policies As New ArrayList

    End Class


    Public Class MyObject

        Public Property status As New Status
        Public Property payload As New Payload

    End Class


End Class