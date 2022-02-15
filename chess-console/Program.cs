using System;
using chess_console;
using board;
using chess;

try
{
    
    Board board = new Board(8, 8);

    board.InsertPiece(new Tower(Color.White, board), new Position(0, 0));
    board.InsertPiece(new Tower(Color.White, board), new Position(1, 3));
    board.InsertPiece(new King(Color.White, board), new Position(2, 4));

    board.InsertPiece(new King(Color.Black, board), new Position(3, 5));

    Screen.WriteBoard(board);

    Console.ReadLine();
    
}
catch (BoardException e)
{
    Console.WriteLine("Board error: " + e.Message);
}
catch(Exception e)
{
    Console.WriteLine("Unexpected error: " + e.Message);
}
