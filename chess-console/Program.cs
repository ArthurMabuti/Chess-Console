using System;
using chess_console;
using board;
using chess;

try
{
    ChessMatch chessMatch = new ChessMatch();
    
    while (!chessMatch.Finished)
    {
        Console.Clear();
        Screen.WriteBoard(chessMatch.Board);

        Console.WriteLine();
        Console.Write("Origem: ");
        Position initial = Screen.ReadChessPosition().toPosition();
        Console.Write("Destino: ");
        Position final = Screen.ReadChessPosition().toPosition();

        chessMatch.PerformMoviment(initial, final);
    }

}
catch (BoardException e)
{
    Console.WriteLine("Board error: " + e.Message);
}
catch(Exception e)
{
    Console.WriteLine("Unexpected error: " + e.Message);
}
