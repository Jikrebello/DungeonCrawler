using SFML.Graphics;

namespace DungeonCrawler
{
    public class Tile
    {
        public TILE Type { get; set; }
        int _columnIndex;
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }
        int _rowIndex;
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }
        public Sprite Sprite { get; set; }
        public int H { get; set; }
        public int G { get; set; }
        public int F { get; set; }
        public Tile ParentNode { get; set; }

        public Tile()
        {
            Sprite = new Sprite();
        }
    }
}
