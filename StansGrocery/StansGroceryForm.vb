'Zachary Christensen
'RCET 2265
'Fall 2023
'Stans Grocery
'https://github.com/Minidude140/StansGrocery.git

Option Strict On
Option Explicit On

Imports System.Threading

'TODO
'[~]load grocery list, split, and clean up strings
'[~]Create array from lists and clear lists

'[]Make splash screen
'[]Create display sub that updates formated display label 

'[~]Update list box sub that updates from given array
'[~]Combo Box should contain either aisle numbers or category depending on radio buttons
'[]Filter list box from combo box selection
'[]Filter from search


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
        FilterComboBox.SelectedItem = "Show All"
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
                'add inventory items into lists if they are not empty
                If currentName <> "" Then
                    inventoryNames.Add(currentName)
                    inventoryLocation.Add(currentLocation)
                    inventoryCategory.Add(currentCategory)
                End If
            Loop
            FileClose(fileNumber)
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
        'sort lists alphabetically
        'inventoryNames.Sort()
        'inventoryLocation.Sort()
        'inventoryCategory.Sort()
    End Sub

    ''' <summary>
    ''' Adds current inventory names, location, and category lists into current Inventory Array.
    ''' Clears lists.
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

    ''' <summary>
    ''' Displays the every first element in the array into the display list box
    ''' </summary>
    ''' <param name="listBoxArray"></param>
    Sub UpdateDisplayListBox(listBoxArray As String(,))
        'loop from 0 to the length of a specified dimension of the array
        For i = 0 To (listBoxArray.GetLength(0) - 1)
            'add the names of the array to the list box
            DisplayListBox.Items.Add(listBoxArray(i, 0))
        Next
        DisplayListBox.Sorted = True
    End Sub

    ''' <summary>
    ''' Fills Filter Combo Box with Aisle Numbers or Categories depending on filter radio buttons
    ''' </summary>
    Sub UpdateFilterComboBox()
        FilterComboBox.Items.Clear()
        If FilterByAisleRadioButton.Checked Then
            Dim allAisleNumbers As New List(Of Integer)
            'add show all option
            FilterComboBox.Items.Add("Show All")
            'fill combo box with aisle numbers
            For i = 0 To (currentInventory.GetLength(0) - 1)
                Select Case True
                    Case currentInventory(i, 1) = "",
                        allAisleNumbers.Contains(CInt(currentInventory(i, 1)))

                    Case Else
                        allAisleNumbers.Add(CInt(currentInventory(i, 1)))
                        'FilterComboBox.Items.Add(currentInventory(i, 1))
                End Select
            Next
            'sort aisle numbers
            allAisleNumbers.Sort()
            'update combo box
            For i = 0 To (allAisleNumbers.Count - 1)
                FilterComboBox.Items.Add(allAisleNumbers(i))
            Next
        Else
            'add show all option
            FilterComboBox.Items.Add("Show All")
            'fill combo box with categories
            For i = 0 To (currentInventory.GetLength(0) - 1)
                Select Case True
                    Case currentInventory(i, 2) = "",
                        FilterComboBox.Items.Contains(currentInventory(i, 2))
                    Case Else
                        FilterComboBox.Items.Add(currentInventory(i, 2))
                End Select
            Next
        End If

    End Sub

    ''' <summary>
    ''' Updates display label containing item, location, and category depeding on selected list box item
    ''' </summary>
    Sub UpdateDisplayLabel()
        Dim selectedItemName As String
        Dim selectedItemLocation As String
        Dim selectedItemCategory As String
        Dim itemArrayLocation As Integer = findArrayLocation(DisplayListBox.SelectedItem.ToString)

        selectedItemName = currentInventory(itemArrayLocation, 0)
        selectedItemLocation = currentInventory(itemArrayLocation, 1)
        selectedItemCategory = currentInventory(itemArrayLocation, 2)

        Select Case True
            Case selectedItemCategory = "" Or selectedItemLocation = ""
                DisplayLabel.Text = $"Stan's Grocery has {selectedItemName}, but it cannot be located."
            Case Else
                DisplayLabel.Text = $"{selectedItemName} is found on aisle {selectedItemLocation} with {selectedItemCategory}."
        End Select
    End Sub

    ''' <summary>
    ''' Returns array index for the list item given
    ''' </summary>
    ''' <param name="itemName"></param>
    ''' <returns></returns>
    Function findArrayLocation(itemName As String) As Integer
        Dim itemlocation As Integer
        Dim currentItemName As String
        For i = 0 To (currentInventory.GetLength(0) - 1)
            currentItemName = currentInventory(i, 0)
            If currentItemName = itemName Then
                itemlocation = i
            End If
        Next
        Return itemlocation
    End Function

    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetDefaults()
        LoadInventoryFile()
        AddInventoryListToArray()
        UpdateDisplayListBox(currentInventory)
        UpdateFilterComboBox()
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

    Private Sub FilterByAisleRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles FilterByAisleRadioButton.CheckedChanged
        'if the filter option is changed update combo box
        UpdateFilterComboBox()
        FilterComboBox.SelectedItem = "Show All"
    End Sub

    Private Sub DisplayListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DisplayListBox.SelectedIndexChanged
        UpdateDisplayLabel()
    End Sub
End Class
