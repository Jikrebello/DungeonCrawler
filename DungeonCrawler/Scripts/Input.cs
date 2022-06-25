using SFML.Window;

namespace DungeonCrawler
{
    public class Input
    {
        public static bool IsKeyPressed(KEY keycode)
        {
            switch (keycode)
            {
                case KEY.Key_Left:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Left)
                        || Keyboard.IsKeyPressed(key: Keyboard.Key.A)
                        || Joystick.GetAxisPosition(joystick: 0, axis: Joystick.Axis.X) < -40
                    )
                    {
                        return true;
                    }
                    break;

                case KEY.Key_Right:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Right)
                        || Keyboard.IsKeyPressed(key: Keyboard.Key.D)
                        || Joystick.GetAxisPosition(joystick: 0, axis: Joystick.Axis.X) > 40
                    )
                    {
                        return true;
                    }
                    break;

                case KEY.Key_Up:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Up)
                        || Keyboard.IsKeyPressed(key: Keyboard.Key.W)
                        || Joystick.GetAxisPosition(joystick: 0, axis: Joystick.Axis.Y) < -40
                    )
                    {
                        return true;
                    }
                    break;

                case KEY.Key_Down:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Down)
                        || Keyboard.IsKeyPressed(key: Keyboard.Key.S)
                        || Joystick.GetAxisPosition(joystick: 0, axis: Joystick.Axis.Y) > 40
                    )
                    {
                        return true;
                    }
                    break;

                case KEY.Key_Attack:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Space)
                        || Mouse.IsButtonPressed(button: Mouse.Button.Left)
                    )
                    {
                        return true;
                    }
                    break;

                case KEY.Key_Escape:
                    if (
                        Keyboard.IsKeyPressed(key: Keyboard.Key.Escape)
                        || Joystick.IsButtonPressed(joystick: 0, button: 7)
                    )
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
