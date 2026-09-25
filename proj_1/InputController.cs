using System.Numerics;
using Raylib_cs;

namespace RobotFactory;

public sealed class InputController
{
    private const int PanelX = 990;

    public void Update(SimulationState state, float deltaTime)
    {
        HandlePanelClick(state);

        if (Raylib.IsKeyPressed(KeyboardKey.Space) && !state.EmergencyStop)
        {
            state.Running = !state.Running;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.M))
        {
            state.ManualMode = !state.ManualMode;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.E))
        {
            state.EmergencyStop = !state.EmergencyStop;
            state.Running = !state.EmergencyStop;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            state.Reset();
        }

        for (int index = 0; index < 6; index++)
        {
            if (Raylib.IsKeyPressed((KeyboardKey)((int)KeyboardKey.One + index)))
            {
                state.SelectedAxis = index;
            }
        }

        if (state.ManualMode && !state.EmergencyStop)
        {
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.S))
            {
                state.MoveSelectedAxis(-deltaTime * 1.8f);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.W))
            {
                state.MoveSelectedAxis(deltaTime * 1.8f);
            }
        }
    }

    private static void HandlePanelClick(SimulationState state)
    {
        if (!Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            return;
        }

        Vector2 mouse = Raylib.GetMousePosition();
        if (mouse.X >= PanelX + 15 && mouse.X <= PanelX + 120 && mouse.Y >= 48 && mouse.Y <= 72)
        {
            state.ManualMode = !state.ManualMode;
            return;
        }
        if (mouse.X >= PanelX + 135 && mouse.X <= PanelX + 240 && mouse.Y >= 48 && mouse.Y <= 72)
        {
            state.EmergencyStop = !state.EmergencyStop;
            state.Running = !state.EmergencyStop;
            return;
        }

        for (int index = 0; index < 6; index++)
        {
            int y = 164 + index * 15;
            if (mouse.X >= PanelX + 12 && mouse.X <= PanelX + 247 && mouse.Y >= y - 2 && mouse.Y <= y + 13)
            {
                state.SelectedAxis = index;
                return;
            }
        }
    }
}
