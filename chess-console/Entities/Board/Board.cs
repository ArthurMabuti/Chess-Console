namespace board
{
    internal class Board
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        private Piece[,] Pieces;

        public Board(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[lines, columns];
        }

        // Allow other classes to access the position of pieces
        public Piece piece(int line, int column)
        {
            return Pieces[line, column];
        }

        //Overcharge from Method piece() / Do the same thing but optimized
        public Piece piece(Position pos)
        {
            return Pieces[pos.Line, pos.Column];
        }

        // This method is needed to validate if exist a piece in that space before moving another piece
        public bool OccupiedSpace(Position pos)
        {
            PositionValidator(pos);
            return piece(pos) != null;
        }

        public void InsertPiece(Piece p, Position pos)
        {
            if (OccupiedSpace(pos))
            {
                throw new BoardException("Position already occupied");
            }

            Pieces[pos.Line, pos.Column] = p; // In position "pos.Line, pos.Column" insert Piece 'p'
            p.Position = pos; // Position of Piece 'p' is equal to "pos"
        }

        public Piece RemovePiece(Position pos)
        {
            if(piece(pos) == null)
            {
                return null;
            }
            Piece aux = piece(pos);
            aux.Position = null;
            Pieces[pos.Line, pos.Column] = null;
            return aux;
        }

        // This method exist just to be used in PositionValidar()
        public bool ValidPosition(Position pos)
        {
            if (pos.Line < 0 || pos.Line >= Lines || pos.Column < 0 || pos.Column >= Columns)
            {
                return false;
            }
            return true;
        }

        public void PositionValidator(Position pos)
        {
            if (!ValidPosition(pos))
            {
                throw new BoardException("Invalid Position!");
            }
        }
    }
}
