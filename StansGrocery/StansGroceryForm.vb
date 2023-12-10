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
'[~]Create display sub that updates formated display label 

'[~]Update list box sub that updates from given array
'[~]Combo Box should contain either aisle numbers or category depending on radio buttons
'[~]Filter list box from combo box selection
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
    End Sub

    ''' <summary>
    ''' Adds current inventory names, location, and category lists into current Inventory Array.
    ''' Clears lists.
    ''' </summary>
    Sub AddInventoryListToArray()
        'Resize current inventory array to list size
        ReDim currentInventory((inventoryNames.Count - 1), 2)
        'iterate through lists and add them to the two dimensional array of current inventory
        For i = 0 To (inventoryNames.Count - 1)
            currentInventory(i, 0) = inventoryNames(i)
            currentInventory(i, 1) = inventoryLocation(i)
            currentInventory(i, 2) = inventoryCategory(i)
        Next
        'clear lists to free memory
        inventoryNames.Clear()
        inventoryLocation.Clear()
        inventoryCategory.Clear()
    End Sub

    ''' <summary>
    ''' Displays filtered contents to the list box (show all, selected aisle, or selected category)
    ''' </summary>
    ''' <param name="listBoxArray"></param>
    Sub UpdateDisplayListBox()
        DisplayListBox.Items.Clear()
        Dim selectedFilter As String = FilterComboBox.SelectedItem.ToString
        Select Case True
            Case selectedFilter = "Show All"
                'loop from 0 to the length of a specified dimension of the array
                For i = 0 To (currentInventory.GetLength(0) - 1)
                    'add the names of the array to the list box
                    DisplayListBox.Items.Add(currentInventory(i, 0))
                Next
            Case FilterByAisleRadioButton.Checked = True
                'display only items from selected aisle
                Dim selectedAisle As String = selectedFilter
                For i = 0 To (currentInventory.GetLength(0) - 1)
                    'if item is in selected aisle add to list box
                    If selectedAisle = currentInventory(i, 1) Then
                        DisplayListBox.Items.Add(currentInventory(i, 0))
                    End If
                Next
            Case FilterByCategoryRadioButton.Checked = True
                'display only item from selected category
                Dim selectedCategory As String = selectedFilter
                For i = 0 To (currentInventory.GetLength(0) - 1)
                    'if item is in selected category add to list
                    If selectedCategory = currentInventory(i, 2) Then
                        DisplayListBox.Items.Add(currentInventory(i, 0))
                    End If
                Next
        End Select
        'sort list box alphabetically
        DisplayListBox.Sorted = True
    End Sub

    ''' <summary>
    ''' Fills Filter Combo Box with Aisle Numbers or Categories depending on filter radio buttons
    ''' </summary>
    Sub UpdateFilterComboBox()
        'clear any previous items in the combo box
        FilterComboBox.Items.Clear()

        If FilterByAisleRadioButton.Checked Then
            'Fill Combo box with aisle numbers
            Dim allAisleNumbers As New List(Of Integer)
            'add show all option
            FilterComboBox.Items.Add("Show All")
            'loop through inventory and add aisle numbers to list
            For i = 0 To (currentInventory.GetLength(0) - 1)
                Select Case True
                    Case currentInventory(i, 1) = "",
                        allAisleNumbers.Contains(CInt(currentInventory(i, 1)))
                        'Do nothing as aisle is already accounted for or empty
                    Case Else
                        'add aisle number to list
                        allAisleNumbers.Add(CInt(currentInventory(i, 1)))
                End Select
            Next
            'sort aisle numbers
            allAisleNumbers.Sort()
            'reverse aisle numbers into descending order
            allAisleNumbers.Reverse()
            'update combo box
            For i = 0 To (allAisleNumbers.Count - 1)
                FilterComboBox.Items.Add(allAisleNumbers(i))
            Next
        Else
            'Fill combo box with categories
            'add show all option
            FilterComboBox.Items.Add("Show All")
            'loop through inventory and add categories to combo box
            For i = 0 To (currentInventory.GetLength(0) - 1)
                Select Case True
                    Case currentInventory(i, 2) = "",
                        FilterComboBox.Items.Contains(currentInventory(i, 2))
                        'Do nothing as category is already accounted for or empty
                    Case Else
                        'add Category to list
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
        'find the array element containing the selected item
        Dim itemArrayLocation As Integer = findArrayIndex(DisplayListBox.SelectedItem.ToString)
        'set temp strings for the name, location, and category of selected item
        selectedItemName = currentInventory(itemArrayLocation, 0)
        selectedItemLocation = currentInventory(itemArrayLocation, 1)
        selectedItemCategory = currentInventory(itemArrayLocation, 2)
        'update display label accordingly
        Select Case True
            Case selectedItemCategory = "" Or selectedItemLocation = ""
                'Item name is available, but the location or category is missing
                DisplayLabel.Text = $"Stan's Grocery has {selectedItemName}, but it cannot be located."
            Case Else
                'Item name is available and location is known
                DisplayLabel.Text = $"{selectedItemName} is found on aisle {selectedItemLocation} with {selectedItemCategory}."
        End Select
    End Sub

    ''' <summary>
    ''' Returns array index for the list item given
    ''' </summary>
    ''' <param name="itemName"></param>
    ''' <returns></returns>
    Function findArrayIndex(itemName As String) As Integer
        Dim itemlocation As Integer
        Dim currentItemName As String
        'loop through inventory and check if any names match given parameter
        For i = 0 To (currentInventory.GetLength(0) - 1)
            'set compare string to next array element
            currentItemName = currentInventory(i, 0)
            'compare current array element with given parameter
            If currentItemName = itemName Then
                'if equal item array location is equal to iteration count
                itemlocation = i
            End If
        Next
        'return array index 
        Return itemlocation
    End Function

    Sub FilterBySearch()
        Dim searchString As String = Me.SeachTextBox.Text
        Dim matchFound As Boolean = False

        DisplayListBox.Items.Clear()
        For i = 0 To (currentInventory.GetLength(0) - 1)
            'set variables for current Array item
            Dim currentName As String = currentInventory(i, 0)
            Dim currentLocation As String = currentInventory(i, 1)
            Dim currentCategory As String = currentInventory(i, 2)

            Select Case searchString
                Case currentName
                    'search string is current name
                    matchFound = True
                    DisplayListBox.Items.Add(currentName)
                Case currentLocation
                    'search string is current location
                    matchFound = True
                    DisplayListBox.Items.Add(currentName)
                Case currentCategory
                    matchFound = True
                    DisplayListBox.Items.Add(currentName)
                    'search string is current category
                Case Else
                    'search string was not found
            End Select
        Next
        'only display if match was not fund
        If matchFound = True Then
        Else
            DisplayListBox.Items.Clear()
            DisplayLabel.Text = $"Sorry not match for {searchString} was found."
        End If
    End Sub

    'Event Handlers
    Private Sub StansGroceryForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadInventoryFile()
        AddInventoryListToArray()
        SetDefaults()
        UpdateDisplayListBox()
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

    Private Sub FilterComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilterComboBox.SelectedIndexChanged
        UpdateDisplayListBox()
    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        FilterBySearch()
    End Sub
End Class
