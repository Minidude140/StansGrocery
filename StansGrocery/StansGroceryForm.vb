'Zachary Christensen
'RCET 2265
'Fall 2023
'Stans Grocery
'https://github.com/Minidude140/StansGrocery.git

Option Strict On
Option Explicit On


Public Class StansGroceryForm
    'Custom Methods

    ''' <summary>
    ''' Sets the default Filters and Texts
    ''' </summary>
    Sub SetDefaults()
        FilterByAisleRadioButton.Checked = True
        DisplayLabel.Text = "No Item Selected."
        FilterComboBox.SelectedIndex = 0
    End Sub

    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()
    End Sub

End Class
