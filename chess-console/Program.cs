using System;
using chess_console;
using board;
using chess;

ChessPosition position = new ChessPosition('a', 1);
ChessPosition position1 = new ChessPosition('c', 7);


Console.WriteLine(position);
Console.WriteLine(position.toPosition());
Console.WriteLine(position1.toPosition());

/*
try
{
    
    Board board = new Board(8, 8);

    board.InsertPiece(new Tower(Color.White, board), new Position(0, 0));
    board.InsertPiece(new Tower(Color.White, board), new Position(1, 3));
    board.InsertPiece(new King(Color.White, board), new Position(2, 4));

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
*/