using System;
using chess_console;
using board;
using chess;

try
{
    ChessMatch match = new ChessMatch();

    while (!match.Finished)
    {
        try
        {
            Console.Clear();
            Screen.WriteBoard(match.Board);
            Console.WriteLine();
            Console.WriteLine("Turn: " + match.Turn);
            Console.WriteLine("Waiting a move: " + match.CurrentPlayer);

            Console.WriteLine();
            Console.Write("Origem: ");
            Position initial = Screen.ReadChessPosition().toPosition();
            match.ValidateInitialPosition(initial);

            bool[,] AvailablePositions = match.Board.piece(initial).AllowedMoviment();

            Console.Clear();
            Screen.WriteBoard(match.Board, AvailablePositions);

            Console.WriteLine();
            Console.Write("Destino: ");
            Position final = Screen.ReadChessPosition().toPosition();
            match.ValidateFinalPosition(initial, final);

            match.MakeMove(initial, final);
        }
        catch (BoardException e)
        {
            Console.WriteLine("Move error: " + e.Message);
            Console.ReadLine();
        }
    }

}
catch (BoardException e)
{
    Console.WriteLine("Board error: " + e.Message);
}
catch (Exception e)
{
    Console.WriteLine("Unexpected error: " + e.Message);
}
