namespace MinMax.Scripts {
    public struct Coordinate {

        public int X;
        public int Y;
        
        public Coordinate TopRight => new Coordinate(X + 1, Y + 1);
        public Coordinate BottomRight => new Coordinate(X + 1, Y - 1);
        public Coordinate BottomLeft => new Coordinate(X - 1, Y - 1);
        public Coordinate TopLeft => new Coordinate(X - 1, Y + 1);
        
        public Coordinate TopRightJump => new Coordinate(X + 2, Y + 2);
        public Coordinate BottomRightJump => new Coordinate(X + 2, Y - 2);
        public Coordinate BottomLeftJump => new Coordinate(X - 2, Y - 2);
        public Coordinate TopLeftJump => new Coordinate(X - 2, Y + 2);

        public Coordinate(int x, int y) {
            X = x;
            Y = y;
        }
        
    }
}