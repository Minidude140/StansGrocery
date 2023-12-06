﻿'Zachary Christensen
'RCET 2265
'Fall 2023
'Stans Grocery
'https://github.com/Minidude140/StansGrocery.git

Option Strict On
Option Explicit On


Public Class StansGroceryForm
    Dim currentInventory() As String
    Dim fileName As String = "..\..\Grocery.txt"
    Dim testString As String

    'Custom Methods

    ''' <summary>
    ''' Sets the default Filters and Texts
    ''' </summary>
    Sub SetDefaults()
        FilterByAisleRadioButton.Checked = True
        DisplayLabel.Text = "No Item Selected."
        FilterComboBox.SelectedIndex = -1
    End Sub

    Sub LoadInventoryFile()
        Dim fileNumber As Integer = FreeFile()
        Dim currentLine As String = ""
        Dim currentFields() As String
        Dim currentName As String
        Dim currentLocation As String
        Dim currentCategory As String


        Try
            FileOpen(fileNumber, Me.fileName, OpenMode.Input)
            Do Until EOF(fileNumber)
                'Input the whole line
                currentLine = LineInput(fileNumber)
                'replace extra " with empty string
                currentLine = Replace(currentLine, Chr(34), "")
                'Split Line into 3 fields
                currentFields = Split(currentLine, ",")
                'Clean up each field of extra characters
                currentName = Replace(currentFields(0), "$$ITM", "")
                currentLocation = Replace(currentFields(1), "##LOC", "")
                currentCategory = Replace(currentFields(2), "%%CAT", "")
                '**Need to Add to Current Inventory array**
            Loop
            FileClose(fileNumber)
        Catch ex As Exception

        End Try
    End Sub
    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()
        LoadInventoryFile()
        DisplayLabel.Text = testString
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
