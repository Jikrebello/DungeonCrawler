namespace DungeonCrawler
{
    /// <summary>
    /// Game States
    /// </summary>
    public enum GAME_STATE
    {
        Main_Menu,
        Playing,
        Game_Over
    }

    /// <summary>
    /// Spawnable Items
    /// </summary>
    public enum ITEM
    {
        Gem,
        Gold,
        Heart,
        Potion,
        Key,
        Count
    }

    /// <summary>
    /// Enemy Types
    /// </summary>
    public enum ENEMY
    {
        Slime,
        Humanoid,
        Count
    }

    /// <summary>
    /// Animation States
    /// </summary>
    public enum ANIMATION_STATE
    {
        Walk_Up,
        Walk_Down,
        Walk_Right,
        Walk_Left,
        Idle_Up,
        Idle_Down,
        Idle_Right,
        Idle_Left,
        Count
    }

    /// <summary>
    /// Tiles
    /// </summary>
    public enum TILE
    {
        Wall_Single,
        Wall_Top,
        Wall_Top_T,
        Wall_Top_Right,
        Wall_Top_Left,
        Wall_Top_End,
        Wall_Side,
        Wall_Side_Right_End,
        Wall_Side_Left_End,
        Wall_Side_Right_T,
        Wall_Side_Left_T,
        Wall_Bottom_Right,
        Wall_Bottom_Left,
        Wall_Bottom_End,
        Wall_Bottom_T,
        Wall_Intersection,
        Wall_Door_Locked,
        Wall_Door_Unlocked,
        Wall_Entrance,
        Floor,
        Floor_Alt,
        Empty,
        Count
    }

    /// <summary>
    /// Game Views
    /// </summary>
    public enum VIEW
    {
        Main,
        UI,
        Count
    }
}
