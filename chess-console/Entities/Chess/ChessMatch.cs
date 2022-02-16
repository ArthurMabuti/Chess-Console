using board;
using System.Collections.Generic;

namespace chess
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool Finished { get; private set; }
        private HashSet<Piece> Piece;
        private HashSet<Piece> Captured;
        public bool Check { get; private set; }
        public Piece VulnerableEnPassant { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            Check = false;
            VulnerableEnPassant = null;
            Piece = new HashSet<Piece>();
            Captured = new HashSet<Piece>();
            InsertPieces();
        }

        public Piece PerformMoviment(Position initial, Position final)
        {
            Piece p = Board.RemovePiece(initial);
            p.IncrementMovimentQty();
            Piece CapturedPiece = Board.RemovePiece(final);
            Board.InsertPiece(p, final);
            if (CapturedPiece != null)
            {
                Captured.Add(CapturedPiece);
            }

            // #SpecialPlay Castling - King's Side
            if (p is King && final.Column == initial.Column + 2)
            {
                Position initialR = new Position(initial.Line, initial.Column + 3);
                Position finalR = new Position(initial.Line, initial.Column + 1);
                Piece R = Board.RemovePiece(initialR);
                R.IncrementMovimentQty();
                Board.InsertPiece(R, finalR);
            }
            // #SpecialPlay Castling - Queen's Side
            if (p is King && final.Column == initial.Column - 2)
            {
                Position initialR = new Position(initial.Line, initial.Column - 4);
                Position finalR = new Position(initial.Line, initial.Column - 1);
                Piece R = Board.RemovePiece(initialR);
                R.IncrementMovimentQty();
                Board.InsertPiece(R, finalR);
            }

            // #SpecialPlay En Passant
            if (p is Pawn)
            {
                if (initial.Column != final.Column && CapturedPiece == null)
                {
                    Position posP;
                    if (p.Color == Color.White)
                    {
                        posP = new Position(final.Line + 1, final.Column);
                    }
                    else
                    {
                        posP = new Position(final.Line - 1, final.Column);
                    }
                    CapturedPiece = Board.RemovePiece(posP);
                    Captured.Add(CapturedPiece);
                }
            }
            return CapturedPiece;
        }

        public void UndoMoviment(Position initial, Position final, Piece capturedPiece)
        {
            Piece p = Board.RemovePiece(final);
            p.DecrementMovimentQty();
            if (capturedPiece != null)
            {
                Board.InsertPiece(capturedPiece, final);
                Captured.Remove(capturedPiece);
            }
            Board.InsertPiece(p, initial);

            // #SpecialPlay Castling - King's Side
            if (p is King && final.Column == initial.Column + 2)
            {
                Position initialR = new Position(initial.Line, initial.Column + 3);
                Position finalR = new Position(initial.Line, initial.Column + 1);
                Piece R = Board.RemovePiece(finalR);
                R.DecrementMovimentQty();
                Board.InsertPiece(R, initialR);
            }

            // #SpecialPlay Castling - Queen's Side
            if (p is King && final.Column == initial.Column - 2)
            {
                Position initialR = new Position(initial.Line, initial.Column - 4);
                Position finalR = new Position(initial.Line, initial.Column - 1);
                Piece R = Board.RemovePiece(finalR);
                R.DecrementMovimentQty();
                Board.InsertPiece(R, initialR);
            }
            // #SpecialPlay En Passant
            if (p is Pawn)
            {
                if (initial.Column != final.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = Board.RemovePiece(final);
                    Position posP;
                    if (p.Color == Color.White)
                    {
                        posP = new Position(3, final.Column);
                    }
                    else
                    {
                        posP = new Position(4, final.Column);
                    }
                    Board.InsertPiece(pawn, posP);
                }
            }

        }

        public void MakeMove(Position initial, Position final)
        {
            Piece CapturedPiece = PerformMoviment(initial, final);

            if (InCheck(CurrentPlayer))
            {
                UndoMoviment(initial, final, CapturedPiece);
                throw new BoardException("You can't put yourself in Check!");
            }

            Piece p = Board.Piece(final);

            // #SpecialPlay Promotion
            if (p is Pawn)
            {
                if ((p.Color == Color.White && final.Line == 0) || (p.Color == Color.Black && final.Line == 7))
                {
                    p = Board.RemovePiece(final);
                    Piece.Remove(p);
                    Piece queen = new Queen(p.Color, Board);
                    Board.InsertPiece(queen, final);
                    Piece.Add(queen);
                }
            }

            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (TestCheckMate(Opponent(CurrentPlayer)))
            {
                Finished = true;
            }
            else
            {
                Turn++;
                AlternatePlayer();
            }


            // #SpecialPlay En Passant
            if (p is Pawn && (final.Line == initial.Line - 2 || final.Line == initial.Line + 2))
            {
                VulnerableEnPassant = p;
            }
            else
            {
                VulnerableEnPassant = null;
            }
        }

        public void ValidateInitialPosition(Position pos)
        {
            if (Board.Piece(pos) == null)
            {
                throw new BoardException("Don't exist piece in that position!");
            }
            if (CurrentPlayer != Board.Piece(pos).Color)
            {
                throw new BoardException("The piece in that position isn't yours!");
            }
            if (!Board.Piece(pos).ExistAllowedMoviment())
            {
                throw new BoardException("Don't have any available moves with that piece!");
            }
        }

        public void ValidateFinalPosition(Position initial, Position final)
        {
            if (!Board.Piece(initial).PossibleMoviment(final))
            {
                throw new BoardException("Invalid moviment!");
            }
        }

        private void AlternatePlayer()
        {
            if (CurrentPlayer == Color.White)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }

        public HashSet<Piece> CapturedPieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Captured)
            {
                if (x.Color == color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in Piece)
            {
                if (x.Color == color)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(CapturedPieces(color));
            return aux;
        }

        private Color Opponent(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        private Piece King(Color color)
        {
            foreach (Piece x in InGamePieces(color))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool InCheck(Color color)
        {
            Piece K = King(color);
            if (K == null)
            {
                throw new BoardException("Don't have a " + color + " king in the board");
            }
            foreach (Piece x in InGamePieces(Opponent(color)))
            {
                bool[,] mat = x.AllowedMoviment();
                if (mat[K.Position.Line, K.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TestCheckMate(Color color)
        {
            if (!InCheck(color))
            {
                return false;
            }
            foreach (Piece x in InGamePieces(color))
            {
                bool[,] mat = x.AllowedMoviment();
                for (int i = 0; i < Board.Lines; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position initial = x.Position;
                            Position final = new Position(i, j);
                            Piece capturedPiece = PerformMoviment(initial, new Position(i, j));
                            bool TestCheck = InCheck(color);
                            UndoMoviment(initial, final, capturedPiece);
                            if (!TestCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void InsertNewPiece(char column, int line, Piece piece)
        {
            Board.InsertPiece(piece, new ChessPosition(column, line).toPosition());
            Piece.Add(piece);
        }

        private void InsertPieces()
        {
            // White ones
            InsertNewPiece('a', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('b', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('c', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('d', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('e', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('f', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('g', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('h', 2, new Pawn(Color.White, Board, this));
            InsertNewPiece('a', 1, new Rook(Color.White, Board));
            InsertNewPiece('h', 1, new Rook(Color.White, Board));
            InsertNewPiece('b', 1, new Knight(Color.White, Board));
            InsertNewPiece('g', 1, new Knight(Color.White, Board));
            InsertNewPiece('c', 1, new Bishop(Color.White, Board));
            InsertNewPiece('f', 1, new Bishop(Color.White, Board));
            InsertNewPiece('d', 1, new Queen(Color.White, Board));
            InsertNewPiece('e', 1, new King(Color.White, Board, this));

            InsertNewPiece('a', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('b', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('c', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('d', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('e', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('f', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('g', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('h', 7, new Pawn(Color.Black, Board, this));
            InsertNewPiece('a', 8, new Rook(Color.Black, Board));
            InsertNewPiece('h', 8, new Rook(Color.Black, Board));
            InsertNewPiece('b', 8, new Knight(Color.Black, Board));
            InsertNewPiece('g', 8, new Knight(Color.Black, Board));
            InsertNewPiece('c', 8, new Bishop(Color.Black, Board));
            InsertNewPiece('f', 8, new Bishop(Color.Black, Board));
            InsertNewPiece('d', 8, new Queen(Color.Black, Board));
            InsertNewPiece('e', 8, new King(Color.Black, Board, this));
        }
    }
}
