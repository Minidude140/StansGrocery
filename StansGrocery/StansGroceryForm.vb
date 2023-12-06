﻿'Zachary Christensen
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
        FilterComboBox.SelectedIndex = -1
    End Sub

    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click,
                                                                           ExitContextStripItem.Click,
                                                                           ExitStripMenuItem.Click
        'Close the program
        Me.Close()
    End Sub

    Private Sub AboutStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutStripMenuItem.Click
        'Display the About form to the user
        AboutForm.Show()
    End Sub
End Class
