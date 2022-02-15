using board;

namespace chess
{
    internal class ChessMatch
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public bool Finished { get; private set; }

        public ChessMatch()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            InsertPieces();
        }

        public void PerformMoviment(Position initial, Position final)
        {
            Piece p = Board.RemovePiece(initial);
            p.IncrementMovimentQty();
            Piece CapturedPiece = Board.RemovePiece(final);
            Board.InsertPiece(p, final);
        }

        private void InsertPieces()
        {
            Board.InsertPiece(new Tower(Color.White, Board), new ChessPosition('c', 1).toPosition());
            Board.InsertPiece(new Tower(Color.White, Board), new ChessPosition('c', 2).toPosition());
            Board.InsertPiece(new King(Color.White, Board), new ChessPosition('d', 1).toPosition());
            Board.InsertPiece(new Tower(Color.White, Board), new ChessPosition('d', 2).toPosition());
            Board.InsertPiece(new Tower(Color.White, Board), new ChessPosition('e', 1).toPosition());
            Board.InsertPiece(new Tower(Color.White, Board), new ChessPosition('e', 2).toPosition());

            Board.InsertPiece(new Tower(Color.Black, Board), new ChessPosition('c', 7).toPosition());
            Board.InsertPiece(new Tower(Color.Black, Board), new ChessPosition('c', 8).toPosition());
            Board.InsertPiece(new Tower(Color.Black, Board), new ChessPosition('d', 7).toPosition());
            Board.InsertPiece(new King(Color.Black, Board), new ChessPosition('d', 8).toPosition());
            Board.InsertPiece(new Tower(Color.Black, Board), new ChessPosition('e', 7).toPosition());
            Board.InsertPiece(new Tower(Color.Black, Board), new ChessPosition('e', 8).toPosition());
        }
    }
}
