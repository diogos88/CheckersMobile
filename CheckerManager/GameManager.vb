'CheckersMobile

'Copyright (C) 2014 Diogo Soares, Frédéric Hanna, David Desrochers and Étienne Chevalier

'This file is part of CheckersMobile.

'CheckersMobile is free software: you can redistribute it and/or modify it under the terms of the
'GNU General Public License as published by the Free Software Foundation, either version 2 of the
'License, or (at your option) any later version.

'CheckersMobile is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
'without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

'See the GNU General Public License for more details. You should have received a copy of the GNU
'General Public License along with CheckersMobile. If not, see <http://www.gnu.org/licenses/>.

'Authors: Diogo Soares
'         Frédéric Hanna
'         David Desrochers
'         Étienne Chevalier


Imports System.Linq

Public Class GameManager
   Public Const CHECKER_NONE = 0
   Public Const MAN_PLAYER_A = 1
   Public Const MAN_PLAYER_B = 2
   Public Const KING_PLAYER_A = 10
   Public Const KING_PLAYER_B = 20
   Public Const PLAYER_A = 0
   Public Const PLAYER_B = 1
   Private ReadOnly m_board(7, 7) As Integer
   Private m_player As Integer = PLAYER_1
   Private m_isThreeRows As Integer = 0
   Public Property now As Date = Date.Now

   Public Shared Property PLAYER_1 As Integer = PLAYER_A
   Public Shared Property PLAYER_2 As Integer = PLAYER_B
   Public Shared Property MAN_PLAYER_1 As Integer = MAN_PLAYER_A
   Public Shared Property MAN_PLAYER_2 As Integer = MAN_PLAYER_B
   Public Shared Property KING_PLAYER_1 As Integer = KING_PLAYER_A
   Public Shared Property KING_PLAYER_2 As Integer = KING_PLAYER_B

   Public ReadOnly Property boardSizeX As Integer
      Get
         Return m_board.GetUpperBound(0) - m_board.GetLowerBound(0) + 1
      End Get
   End Property
   Public ReadOnly Property boardSizeY As Integer
      Get
         Return m_board.GetUpperBound(1) - m_board.GetLowerBound(1) + 1
      End Get
   End Property
   Public ReadOnly Property board1D As Integer()
      Get
         Dim b1D(boardSizeX * boardSizeY - 1) As Integer
         For Y As Integer = 0 To boardSizeY - 1
            For X As Integer = 0 To boardSizeX - 1
               b1D(Y * boardSizeX + X) = m_board(X, Y)
            Next
         Next
         Return b1D
      End Get
   End Property
   Public Function getCell1D(X As Integer, Y As Integer) As Integer
      Return Y * boardSizeX + X
   End Function
   Public Function getCell2D(Index As Integer) As Point
      Dim Y As Integer = Math.Floor(Index / boardSizeX)
      Dim X As Integer = Index Mod boardSizeY
      Return New Point(X, Y)
   End Function
   <Obsolete("To delete, the manager doesn't care about the colors")> _
   Public Function getCellColor(Index As Integer) As Integer ' 0 is red
      Dim P As Point = getCell2D(Index)
      Return (Index - (P.Y Mod 2)) Mod 2
   End Function
   ''' <summary>
   ''' Param0: Y
   ''' Param1: X
   ''' </summary>
   Public ReadOnly Property board2D As Integer(,)
      Get
         Return m_board.Clone
      End Get
   End Property
   Public ReadOnly Property yourcolor As Integer ' 0 is red
      Get
         Return m_player
      End Get
   End Property
   Public ReadOnly Property morebuttons As Integer
      Get
         Return m_isThreeRows
      End Get
   End Property
   Public Property turnPlayer As Integer = PLAYER_1

   Public Sub CreateBoard(player As Integer, numCheckers As Integer)
      Debug.Assert(numCheckers = 8 OrElse numCheckers = 12)
      Debug.Assert(player = 0 OrElse player = 1)
      m_player = player
      m_isThreeRows = IIf(numCheckers = 12, True, False)

      If m_player = PLAYER_A Then
         PLAYER_1 = PLAYER_A
         PLAYER_2 = PLAYER_B
         MAN_PLAYER_1 = MAN_PLAYER_A
         MAN_PLAYER_2 = MAN_PLAYER_B
         KING_PLAYER_1 = KING_PLAYER_A
         KING_PLAYER_2 = KING_PLAYER_B

         turnPlayer = PLAYER_1
      Else
         PLAYER_1 = PLAYER_B
         PLAYER_2 = PLAYER_A
         MAN_PLAYER_1 = MAN_PLAYER_B
         MAN_PLAYER_2 = MAN_PLAYER_A
         KING_PLAYER_1 = KING_PLAYER_B
         KING_PLAYER_2 = KING_PLAYER_A

         turnPlayer = PLAYER_2
      End If

      For x As Integer = 0 To boardSizeX - 1
         For y As Integer = 0 To boardSizeY - 1
            m_board(x, y) = CHECKER_NONE
         Next
      Next
      m_board(0, 0) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(2, 0) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(4, 0) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(6, 0) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(1, 1) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(3, 1) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(5, 1) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      m_board(7, 1) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      If numCheckers = 12 Then
         m_board(0, 2) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
         m_board(2, 2) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
         m_board(4, 2) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
         m_board(6, 2) = IIf(m_player = PLAYER_1, MAN_PLAYER_2, MAN_PLAYER_1)
      End If
      m_board(7, 7) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(5, 7) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(3, 7) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(1, 7) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(6, 6) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(4, 6) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(2, 6) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      m_board(0, 6) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      If numCheckers = 12 Then
         m_board(7, 5) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
         m_board(5, 5) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
         m_board(3, 5) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
         m_board(1, 5) = IIf(m_player = PLAYER_1, MAN_PLAYER_1, MAN_PLAYER_2)
      End If
   End Sub
   Public Function Move(SrcX As Integer, SrcY As Integer, DestX As Integer, DestY As Integer) As Boolean
      Debug.Assert(SrcX >= 0 AndAlso SrcX < 8 AndAlso SrcY >= 0 AndAlso SrcY < 8)
      Debug.Assert(DestX >= 0 AndAlso DestX < 8 AndAlso DestY >= 0 AndAlso DestY < 8)
      Dim CellSrc As Point = New Point(SrcX, SrcY)
      Dim CellDest As Point = New Point(DestX, DestY)

      Dim Jumped As Boolean = False

      Dim moveDone As Boolean = myking(CellSrc, CellDest, Jumped)
      moveDone = moveDone OrElse myman(CellSrc, CellDest, Jumped)

      If moveDone Then
         If Jumped Then
            turnPlayer = determinateNextPlayer(CellDest)
         Else
            turnPlayer = IIf(turnPlayer = PLAYER_1, PLAYER_2, PLAYER_1)
         End If
      End If
      checkmyking()
      Return moveDone
   End Function
   Private Sub checkmyking()
      If m_player = PLAYER_1 Then
         For x As Integer = 0 To boardSizeX - 1 Step 2
            If m_board(x, 0) = MAN_PLAYER_1 Then
               m_board(x, 0) = KING_PLAYER_1
            End If
         Next
      Else
         For x As Integer = 1 To boardSizeX - 1 Step 2
            If m_board(x, 7) = MAN_PLAYER_1 Then
               m_board(x, 7) = KING_PLAYER_1
            End If
         Next
      End If
   End Sub
   Private Sub checkenemyking()
      If m_player = PLAYER_1 Then
         For x As Integer = 1 To boardSizeX - 1 Step 2
            If m_board(x, 7) = MAN_PLAYER_2 Then
               m_board(x, 7) = KING_PLAYER_2
            End If
         Next
      Else
         For x As Integer = 0 To boardSizeX - 1 Step 2
            If m_board(x, 0) = MAN_PLAYER_2 Then
               m_board(x, 0) = KING_PLAYER_2
            End If
         Next
      End If
   End Sub
   Private Function myman(CellSrc As Point, CellDest As Point, ByRef Jumped As Boolean) As Boolean
      Dim moveDone As Boolean = False

      Jumped = False

      If m_board(CellSrc.X, CellSrc.Y) = MAN_PLAYER_1 Then
         Dim src As Integer = getCell1D(CellSrc.X, CellSrc.Y)
         Dim dest As Integer = getCell1D(CellDest.X, CellDest.Y)
         Dim moveRight As Integer = src - 7
         Dim moveLeft As Integer = src - 9
         Dim moveRightJump As Integer = src - 14
         Dim moveLeftJump As Integer = src - 18
         If dest = moveRight Or dest = moveLeft Then
            If m_board(CellDest.X, CellDest.Y) = CHECKER_NONE Then
               m_board(CellDest.X, CellDest.Y) = MAN_PLAYER_1
               m_board(CellSrc.X, CellSrc.Y) = CHECKER_NONE
               'enemyinfo()
               moveDone = True
            End If
         ElseIf dest = moveRightJump Or dest = moveLeftJump Then
            Dim cellEnemy As Point = Nothing
            Select Case dest
               Case moveRightJump
                  cellEnemy = getCell2D(moveRight)
               Case moveLeftJump
                  cellEnemy = getCell2D(moveLeft)
            End Select
            If cellEnemy IsNot Nothing AndAlso m_board(cellEnemy.X, cellEnemy.Y) = MAN_PLAYER_2 OrElse m_board(cellEnemy.X, cellEnemy.Y) = KING_PLAYER_2 Then
               m_board(CellDest.X, CellDest.Y) = MAN_PLAYER_1
               m_board(CellSrc.X, CellSrc.Y) = CHECKER_NONE
               m_board(cellEnemy.X, cellEnemy.Y) = CHECKER_NONE
               moveDone = True
               Jumped = True
            End If
         End If
         checkmyking()
      End If
      Return moveDone
   End Function
   Public Function determinateNextPlayer(finalCell As Point) As Integer
      Dim theNextPlayer As Integer = IIf(turnPlayer = PLAYER_1, PLAYER_2, PLAYER_1)
      Dim manEnemy As Integer
      Dim kingEnemy As Integer
      If (turnPlayer = PLAYER_1) Then
         manEnemy = MAN_PLAYER_2
         kingEnemy = KING_PLAYER_2
      Else
         manEnemy = MAN_PLAYER_1
         kingEnemy = KING_PLAYER_1
      End If
      Dim src As Integer = getCell1D(finalCell.X, finalCell.Y)
      Dim moveRight As Integer = src + IIf(turnPlayer = PLAYER_1, -7, 7)
      Dim moveLeft As Integer = src + IIf(turnPlayer = PLAYER_1, -9, 9)
      Dim moveRightJump As Integer = moveRight + IIf(turnPlayer = PLAYER_1, -7, 7)
      Dim moveLeftJump As Integer = moveLeft + IIf(turnPlayer = PLAYER_1, -9, 9)
      Dim cellMoveRight As Point = getCell2D(moveRight)
      Dim cellMoveLeft As Point = getCell2D(moveLeft)
      Dim cellMoveRightJump As Point = getCell2D(moveRightJump)
      Dim cellMoveLeftJump As Point = getCell2D(moveLeftJump)
      If (isCell1DValid(moveRight) And isCell1DValid(moveRightJump)) AndAlso isCellValid(cellMoveRight) AndAlso isCellValid(cellMoveRightJump) Then
         If m_board(cellMoveRightJump.X, cellMoveRightJump.Y) = CHECKER_NONE AndAlso (m_board(cellMoveRight.X, cellMoveRight.Y) = manEnemy OrElse m_board(cellMoveRight.X, cellMoveRight.Y) = kingEnemy) Then
            theNextPlayer = turnPlayer
         End If
      End If
      If (isCell1DValid(moveLeft) And isCell1DValid(moveLeftJump)) AndAlso isCellValid(cellMoveLeft) AndAlso isCellValid(cellMoveLeftJump) Then
         If m_board(cellMoveLeftJump.X, cellMoveLeftJump.Y) = CHECKER_NONE AndAlso (m_board(cellMoveLeft.X, cellMoveLeft.Y) = manEnemy OrElse m_board(cellMoveLeft.X, cellMoveLeft.Y) = kingEnemy) Then
            theNextPlayer = turnPlayer
         End If
      End If
      Return theNextPlayer
   End Function

   Private Function isCell1DValid(cell As Integer) As Boolean
      Dim cell2D As Point = getCell2D(cell)

      Return cell >= 0 AndAlso cell < boardSizeX * boardSizeY AndAlso (isOdd(cell) = isOdd(cell2D.Y))
   End Function

   Private Function isOdd(value As Integer) As Boolean
      If ((value = 0) OrElse (value Mod 2) = 0) Then
         Return False
      Else
         Return True
      End If
   End Function

   Private Function myking(CellSrc As Point, CellDest As Point, ByRef Jumped As Boolean) As Boolean
      Dim moveDone As Boolean = False
      Jumped = False
      If m_board(CellSrc.X, CellSrc.Y) = KING_PLAYER_1 Then
         Dim src As Integer = getCell1D(CellSrc.X, CellSrc.Y)
         Dim dest As Integer = getCell1D(CellDest.X, CellDest.Y)
         Dim move1 As Integer = src - 7
         Dim move2 As Integer = src - 9
         Dim move3 As Integer = src + 7
         Dim move4 As Integer = src + 9
         Dim move5 As Integer = move1 - 7
         Dim move6 As Integer = move2 - 9
         Dim move7 As Integer = move3 + 7
         Dim move8 As Integer = move4 + 9
         If dest = move1 Or dest = move2 Or dest = move3 Or dest = move4 Then
            If m_board(CellDest.X, CellDest.Y) = CHECKER_NONE Then
               m_board(CellDest.X, CellDest.Y) = KING_PLAYER_1
               m_board(CellSrc.X, CellSrc.Y) = CHECKER_NONE
               moveDone = True
            End If
         ElseIf dest = move5 Or dest = move6 Or dest = move7 Or dest = move8 Then
            Dim cellEnemy As Point = Nothing
            Select Case dest
               Case move5
                  cellEnemy = getCell2D(move1)
               Case move6
                  cellEnemy = getCell2D(move2)
               Case move7
                  cellEnemy = getCell2D(move3)
               Case move8
                  cellEnemy = getCell2D(move4)
            End Select
            If cellEnemy IsNot Nothing AndAlso m_board(cellEnemy.X, cellEnemy.Y) = MAN_PLAYER_2 OrElse m_board(cellEnemy.X, cellEnemy.Y) = KING_PLAYER_2 Then
               m_board(CellDest.X, CellDest.Y) = KING_PLAYER_1
               m_board(CellSrc.X, CellSrc.Y) = CHECKER_NONE
               m_board(cellEnemy.X, cellEnemy.Y) = CHECKER_NONE
               Jumped = True
               moveDone = True
            End If
         End If
      End If
      Return moveDone
   End Function
   Private Function isCellValid(cell As Point) As Boolean
      Return cell.X >= 0 AndAlso cell.X < boardSizeX AndAlso cell.Y >= 0 AndAlso cell.Y < boardSizeY
   End Function
   Public Sub enemyinfo()
      Dim theBoard1D As Integer() = board1D()
      Dim enemies1D As List(Of Integer) = Enumerable.Range(0, theBoard1D.Count).Where(Function(i) (theBoard1D(i) = MAN_PLAYER_2) OrElse (theBoard1D(i) = KING_PLAYER_2)).ToList()
      Dim enemies2D As New List(Of Point)(enemies1D.Count)
      enemies2D.AddRange(From enemy1D In enemies1D Select getCell2D(enemy1D))

      Dim jumped As Boolean = False
      Dim jumpedCell As New Point(-1, -1)
      Dim alreadyMoved As Boolean = False
      Dim possibleMoves As New List(Of Point)

      For enemyIndex As Integer = 0 To enemies1D.Count - 1
         Dim enemy1D As Integer = enemies1D(enemyIndex)
         Dim enemy2D As Point = enemies2D(enemyIndex)

         Dim moveRight = enemy1D + 9
         Dim moveLeft = enemy1D + 7
         Dim moveRightJump = moveRight + 9
         Dim moveLeftJump = moveLeft + 7
         Try
            If isCell1DValid(moveRight) AndAlso isCell1DValid(moveRightJump) AndAlso (theBoard1D(moveRight) = MAN_PLAYER_1 OrElse theBoard1D(moveRight) = KING_PLAYER_1) AndAlso (theBoard1D(moveRightJump) = CHECKER_NONE) Then
               m_board(getCell2D(moveRightJump).X, getCell2D(moveRightJump).Y) = m_board(enemy2D.X, enemy2D.Y)
               m_board(getCell2D(moveRight).X, getCell2D(moveRight).Y) = CHECKER_NONE
               m_board(enemy2D.X, enemy2D.Y) = CHECKER_NONE
               alreadyMoved = True
               jumped = True
               jumpedCell = getCell2D(moveRightJump)
               Exit For
            ElseIf isCell1DValid(moveLeft) AndAlso isCell1DValid(moveLeftJump) AndAlso (theBoard1D(moveLeft) = MAN_PLAYER_1 OrElse theBoard1D(moveLeft) = KING_PLAYER_1) AndAlso (theBoard1D(moveLeftJump) = CHECKER_NONE) Then
               m_board(getCell2D(moveLeftJump).X, getCell2D(moveLeftJump).Y) = m_board(enemy2D.X, enemy2D.Y)
               m_board(getCell2D(moveLeft).X, getCell2D(moveLeft).Y) = CHECKER_NONE
               m_board(enemy2D.X, enemy2D.Y) = CHECKER_NONE
               alreadyMoved = True
               jumped = True
               jumpedCell = getCell2D(moveLeftJump)
               Exit For
            ElseIf m_board(enemy2D.X, enemy2D.Y) = KING_PLAYER_2 Then
               moveRight = enemy1D - 9
               moveLeft = enemy1D - 7
               moveRightJump = moveRight - 9
               moveLeftJump = moveLeft - 7
               If isCell1DValid(moveRight) AndAlso isCell1DValid(moveRightJump) AndAlso (theBoard1D(moveRight) = MAN_PLAYER_1 OrElse theBoard1D(moveRight) = KING_PLAYER_1) AndAlso (theBoard1D(moveRightJump) = CHECKER_NONE) Then
                  m_board(getCell2D(moveRightJump).X, getCell2D(moveRightJump).Y) = KING_PLAYER_2
                  m_board(getCell2D(moveRight).X, getCell2D(moveRight).Y) = CHECKER_NONE
                  m_board(enemy2D.X, enemy2D.Y) = CHECKER_NONE
                  alreadyMoved = True
                  jumped = True
                  jumpedCell = getCell2D(moveRightJump)
                  Exit For
               ElseIf isCell1DValid(moveLeft) AndAlso isCell1DValid(moveLeftJump) AndAlso (theBoard1D(moveLeft) = MAN_PLAYER_1 OrElse theBoard1D(moveLeft) = KING_PLAYER_1) AndAlso (theBoard1D(moveLeftJump) = CHECKER_NONE) Then
                  m_board(getCell2D(moveLeftJump).X, getCell2D(moveLeftJump).Y) = KING_PLAYER_2
                  m_board(getCell2D(moveLeft).X, getCell2D(moveLeft).Y) = CHECKER_NONE
                  m_board(enemy2D.X, enemy2D.Y) = CHECKER_NONE
                  alreadyMoved = True
                  jumped = True
                  jumpedCell = getCell2D(moveLeftJump)
                  Exit For
               End If
            End If
            If isCell1DValid(moveRight) AndAlso (theBoard1D(moveRight) = CHECKER_NONE) Then
               possibleMoves.Add(New Point(enemy1D, moveRight))
            ElseIf isCell1DValid(moveLeft) AndAlso (theBoard1D(moveLeft) = CHECKER_NONE) Then
               possibleMoves.Add(New Point(enemy1D, moveLeft))
            End If
         Catch ex As Exception
         End Try
      Next
      If Not alreadyMoved Then
         Dim value As Integer = Math.Min(CInt(Int(((possibleMoves.Count - 1) * Rnd() + 1))), possibleMoves.Count - 1)

         If possibleMoves.Count > 0 AndAlso isCell1DValid(possibleMoves.Item(value).X) AndAlso isCell1DValid(possibleMoves.Item(value).Y) Then
            Dim fromCell As Point = getCell2D(possibleMoves.Item(value).X)
            Dim toCell As Point = getCell2D(possibleMoves.Item(value).Y)
            m_board(toCell.X, toCell.Y) = m_board(fromCell.X, fromCell.Y)
            m_board(fromCell.X, fromCell.Y) = CHECKER_NONE
         End If
      End If

      checkenemyking()

      If jumped Then
         turnPlayer = determinateNextPlayer(jumpedCell)
      Else
         turnPlayer = IIf(turnPlayer = PLAYER_1, PLAYER_2, PLAYER_1)
      End If

   End Sub

   Public Function CheckWinner() As Integer
      Dim TotalPlayer1 As Integer = 0
      Dim TotalPlayer2 As Integer = 0

      For Y As Integer = 0 To boardSizeY - 1
         For X As Integer = 0 To boardSizeX - 1
            If m_board(X, Y) = MAN_PLAYER_1 OrElse m_board(X, Y) = KING_PLAYER_1 Then
               TotalPlayer1 += 1
            End If
            If m_board(X, Y) = MAN_PLAYER_2 OrElse m_board(X, Y) = KING_PLAYER_2 Then
               TotalPlayer2 += 1
            End If
         Next
      Next

      If TotalPlayer1 = 0 Then
         Return PLAYER_2
      ElseIf TotalPlayer2 = 0 Then
         Return PLAYER_1
      Else
         Return -1
      End If

   End Function

End Class

