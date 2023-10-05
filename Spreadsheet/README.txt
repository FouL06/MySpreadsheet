SPREADSHEET C# APPLICATION
	- CS 3500 Fall 2021
	- Author: Ashton Foulger

Introduction:
	- Recreation of Microsofts Excel using C# custom api's and Formula evaluations. Program includes basic features that any user would find in excel.
	  editing of a cell, formulas, cell dependency, and error reporting. Additional features for this program include the ablility to change the cells
	  text color to (Red,Green, or Black) this allows the user to determine if a cell is bad, good, or wants to set a cell color back to the default color.
	  The program will also color and fill any cell that caused an error with the error code, and mark the text as red for readability.

Technology Stack:
	- C#
	- Windows Form Application (GUI)

Requirements:
	- Windows Computer or Windows OS (VM)

Features:
	- (9/18/21) Implemented feature to allow the user to press enter on the cell selected which allow the cell to be evaluated and move the selection down
	  by one cell in its column. This allows for easier movement and selection of cells when inputing mass data.

	- (9/19/21) Implemented Dialog boxes for the internal documentation on how to use the program and its features,
	  this is to allow the user to get a better understanding of how features work. Also provides the ablility to gain access to documenation even without internet connection.

	- (9/19/21) Implemented feature to allow the user to change the cells text color to (Red) for errors or a bad data cell in the spreadsheet. In addition to this, the user
	  can also change the cells text color to green to indicate a positive or good cell in the data. In order for the user to revert these changes if made, also implented a button
	  to revert the color back to default(Black) color.

General Use:
	- All files saved in this program are of the file extension (.sprd) no other files can be read into the program.
	- Changes made to the spreadsheet that are not saved will prompt you to save the spreadsheet to a file unless otherwise cancled.
