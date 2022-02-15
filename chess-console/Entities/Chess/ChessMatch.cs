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

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            Check = false;
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
        }

        public void MakeMove(Position initial, Position final)
        {
            Piece CapturedPiece = PerformMoviment(initial, final);

            if (InCheck(CurrentPlayer))
            {
                UndoMoviment(initial, final, CapturedPiece);
                throw new BoardException("You can't put yourself in Check!");
            }
            if (InCheck(Opponent(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            Turn++;
            AlternatePlayer();
        }

        public void ValidateInitialPosition(Position pos)
        {
            if (Board.piece(pos) == null)
            {
                throw new BoardException("Don't exist piece in that position!");
            }
            if (CurrentPlayer != Board.piece(pos).Color)
            {
                throw new BoardException("The piece in that position isn't yours!");
            }
            if (!Board.piece(pos).ExistAllowedMoviment())
            {
                throw new BoardException("Don't have any available moves with that piece!");
            }
        }

        public void ValidateFinalPosition(Position initial, Position final)
        {
            if (!Board.piece(initial).CanMoveTo(final))
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
            if(K == null)
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

        public void InsertNewPiece(char column, int line, Piece piece)
        {
            Board.InsertPiece(piece, new ChessPosition(column, line).toPosition());
            Piece.Add(piece);
        }

        private void InsertPieces()
        {
            InsertNewPiece('c', 1, new Tower(Color.White, Board));
            InsertNewPiece('c', 2, new Tower(Color.White, Board));
            InsertNewPiece('d', 1, new King(Color.White, Board));
            InsertNewPiece('d', 2, new Tower(Color.White, Board));
            InsertNewPiece('e', 1, new Tower(Color.White, Board));
            InsertNewPiece('e', 2, new Tower(Color.White, Board));

            InsertNewPiece('c', 7, new Tower(Color.Black, Board));
            InsertNewPiece('c', 8, new Tower(Color.Black, Board));
            InsertNewPiece('d', 7, new Tower(Color.Black, Board));
            InsertNewPiece('d', 8, new King(Color.Black, Board));
            InsertNewPiece('e', 7, new Tower(Color.Black, Board));
            InsertNewPiece('e', 8, new Tower(Color.Black, Board));
        }
    }
}
