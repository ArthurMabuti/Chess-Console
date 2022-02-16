using board;

namespace chess
{
    internal class King : Piece
    {
        private ChessMatch Match;

        public King(Color color, Board board, ChessMatch match)
            : base(color, board)
        {
            Match = match;
        }

        public override string ToString()
        {
            return "K";
        }

        private bool CanMove(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p == null || p.Color != Color;
        }

        private bool TestRookCastling(Position pos)
        {
            Piece p = Board.Piece(pos);
            return p != null && p is Rook && p.Color == Color && p.MovimentQty == 0;
        }

        public override bool[,] AllowedMoviment()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];

            Position pos = new Position(0, 0);

            // Up
            pos.SetValues(Position.Line - 1, Position.Column);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Up-Right
            pos.SetValues(Position.Line - 1, Position.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Right
            pos.SetValues(Position.Line, Position.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Down-Right
            pos.SetValues(Position.Line + 1, Position.Column + 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Down
            pos.SetValues(Position.Line + 1, Position.Column);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Down-Left
            pos.SetValues(Position.Line + 1, Position.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Left
            pos.SetValues(Position.Line, Position.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // Up - Left
            pos.SetValues(Position.Line - 1, Position.Column - 1);
            if (Board.ValidPosition(pos) && CanMove(pos))
            {
                mat[pos.Line, pos.Column] = true;
            }

            // #SpecialPlay Castling
            if(MovimentQty==0 && !Match.Check)
            {
                // #SpecialPlay Castling - King's Side
                Position posR1 = new Position(Position.Line, Position.Column + 3);
                if (TestRookCastling(posR1))
                {
                    Position p1 = new Position(Position.Line, Position.Column + 1);
                    Position p2 = new Position(Position.Line, Position.Column + 2);
                    if(Board.Piece(p1) == null && Board.Piece(p2) == null)
                    {
                        mat[Position.Line, Position.Column + 2] = true;
                    }
                }

                // #SpecialPlay Castling - Queen's Side
                Position posR2 = new Position(Position.Line, Position.Column - 4);
                if (TestRookCastling(posR1))
                {
                    Position p1 = new Position(Position.Line, Position.Column - 1);
                    Position p2 = new Position(Position.Line, Position.Column - 2);
                    Position p3 = new Position(Position.Line, Position.Column - 3);
                    if (Board.Piece(p1) == null && Board.Piece(p2) == null && Board.Piece(p3) == null)
                    {
                        mat[Position.Line, Position.Column - 2] = true;
                    }
                }
            }

            return mat;
        }
    }
}
