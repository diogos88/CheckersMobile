Public Class Point
    Property X As Integer = 0
    Property Y As Integer = 0

    Public Sub New(x As Integer, y As Integer)
        Me.X = x
        Me.Y = y
    End Sub

    Public Overrides Function ToString() As String
        Return X & " " & Y
    End Function
End Class
