'Zachary Christensen
'RCET 2265
'Fall 2023
'Stans Grocery
'https://github.com/Minidude140/StansGrocery.git

Option Strict On
Option Explicit On

Imports System.Threading

Public Class StansGroceryForm
    Dim currentInventory(1000, 3) As String
    Dim inventoryNames As New List(Of String)
    Dim inventoryLocation As New List(Of String)
    Dim inventoryCategory As New List(Of String)
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

    ''' <summary>
    ''' Loads Grocery.txt and adds contents to three lists
    ''' </summary>
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
                'add inventory items into lists
                inventoryNames.Add(currentName)
                inventoryLocation.Add(currentLocation)
                inventoryCategory.Add(currentCategory)
            Loop
            FileClose(fileNumber)
            ''Need to fix directory location when file dialog opens

        Catch ioexception As io.ioexception
            With openfiledialog
                .reset()
                .InitialDirectory = "..\"
                .filename = ""
                .defaultext = ".txt"
                .addextension = True
                .filter = "txt files (*.txt)|*.txt|all files (*.*)|*.*"
                .showdialog()
                Me.filename = filename
            End With
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Adds current inventory names, location, and category lists into current Inventory Array
    ''' </summary>
    Sub AddInventoryListToArray()
        ReDim currentInventory((inventoryNames.Count - 1), 2)
        For i = 0 To (inventoryNames.Count - 1)
            currentInventory(i, 0) = inventoryNames(i)
            currentInventory(i, 1) = inventoryLocation(i)
            currentInventory(i, 2) = inventoryCategory(i)
            'System.Threading.Thread.Sleep(1000)
        Next
        inventoryNames.Clear()
        inventoryLocation.Clear()
        inventoryCategory.Clear()
    End Sub

    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()
        LoadInventoryFile()
        AddInventoryListToArray()
        'DisplayLabel.Text = testString
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
