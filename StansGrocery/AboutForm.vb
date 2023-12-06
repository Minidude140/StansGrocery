Public Class AboutForm
    Private Sub AboutCloseButton_Click(sender As Object, e As EventArgs) Handles AboutCloseButton.Click
        'Close the about form return to Stan's Grocery Form
        StansGroceryForm.Show()
        Me.Close()
    End Sub
End Class