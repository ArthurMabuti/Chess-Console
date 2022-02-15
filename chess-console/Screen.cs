using board;
using chess;

namespace chess_console
{
    internal class Screen
    {
        public static void WriteMatch(ChessMatch match)
        {
            WriteBoard(match.Board);
            Console.WriteLine();
            WriteCapturedPieces(match);
            Console.WriteLine();
            Console.WriteLine("Turno: " + match.Turn);
            Console.WriteLine("Waiting a move from: " + match.CurrentPlayer);
            if (match.Check)
            {
                Console.WriteLine("XEQUE!");
            } 
        }

        public static void WriteCapturedPieces(ChessMatch match)
        {
            Console.WriteLine("Captured Pieces:");
            Console.Write("White: ");
            WriteGroup(match.CapturedPieces(Color.White));
            Console.WriteLine();
            Console.Write("Black: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteGroup(match.CapturedPieces(Color.Black));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void WriteGroup(HashSet<Piece> group)
        {
            Console.Write("[");
            foreach(Piece x in group)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void WriteBoard(Board board)
        {
            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    WritePiece(board.piece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void WriteBoard(Board board, bool[,] availablePositions)
        {
            ConsoleColor origBackground = Console.BackgroundColor;
            ConsoleColor altBackground = ConsoleColor.DarkGray;

            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if (availablePositions[i, j])
                    {
                        Console.BackgroundColor = altBackground;
                    }
                    else
                    {
                        Console.BackgroundColor = origBackground;
                    }
                    WritePiece(board.piece(i, j));
                    Console.BackgroundColor = origBackground;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = origBackground;
        }

        public static ChessPosition ReadChessPosition()
        {
            string s = Console.ReadLine();
            char column = s[0];
            int line = int.Parse(s[1] + "");
            return new ChessPosition(column, line);
        }

        public static void WritePiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (piece.Color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
