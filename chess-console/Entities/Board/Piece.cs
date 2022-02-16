namespace board
{
    internal abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovimentQty { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Color color, Board board)
        {
            Position = null;
            Color = color;
            Board = board;
            MovimentQty = 0;
        }

        public void DecrementMovimentQty()
        {
            MovimentQty--;
        }

        public void IncrementMovimentQty()
        {
            MovimentQty++;
        }

        public bool ExistAllowedMoviment()
        {
            bool[,] mat = AllowedMoviment();
            for (int i = 0; i < Board.Lines; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }

        public bool PossibleMoviment(Position pos)
        {
            return AllowedMoviment()[pos.Line, pos.Column];
        }

        public abstract bool[,] AllowedMoviment();
    }
}
