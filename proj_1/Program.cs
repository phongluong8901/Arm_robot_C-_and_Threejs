using System.Numerics;
using Raylib_cs;

namespace RobotFactory;

internal static class Program
{
    private const int ScreenWidth = 1280;
    private const int ScreenHeight = 720;

    private static void Main()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Mini Factory Cell");
        Raylib.SetTargetFPS(60);

        var camera = new Camera3D
        {
            Position = new Vector3(12, 10, 14),
            Target = new Vector3(0, 0.8f, -0.8f),
            Up = Vector3.UnitY,
            FovY = 45,
            Projection = CameraProjection.Perspective
        };

        var state = new SimulationState();
        var input = new InputController();
        var hud = new HudRenderer();

        while (!Raylib.WindowShouldClose())
        {
            float deltaTime = Raylib.GetFrameTime();
            input.Update(state, deltaTime);
            state.Update(deltaTime);
            Raylib.UpdateCamera(ref camera, CameraMode.Orbital);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(new Color(22, 28, 36, 255));
            Raylib.BeginMode3D(camera);
            FactoryCell.Draw(state);
            RobotArm.Draw(new Vector3(0, 0.85f, -1.9f), state.JointAngles);
            Raylib.EndMode3D();
            hud.Draw(state, ScreenWidth, ScreenHeight);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
