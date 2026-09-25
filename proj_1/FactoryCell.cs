using System.Numerics;
using Raylib_cs;

namespace RobotFactory;

public static class FactoryCell
{
    public static void Draw(SimulationState state)
    {
        DrawFloor();
        DrawConveyor(state.ConveyorPosition);
        DrawControlStation(state.Running, state.EmergencyStop);
        DrawSafetyCell();
    }

    private static void DrawFloor()
    {
        Raylib.DrawPlane(new Vector3(0, -0.05f, 0), new Vector2(24, 18), new Color(35, 43, 50, 255));
        Raylib.DrawGrid(24, 1.0f);
        Raylib.DrawCube(new Vector3(0, -0.01f, -2.4f), 21, 0.03f, 5.8f, new Color(50, 58, 64, 255));
        Raylib.DrawCube(new Vector3(0, 0.02f, -2.4f), 5.4f, 0.025f, 5.1f, new Color(64, 72, 76, 255));
        Raylib.DrawCubeWires(new Vector3(0, 0.03f, -2.4f), 5.4f, 0.03f, 5.1f, new Color(239, 196, 93, 255));
    }

    private static void DrawConveyor(float boxPosition)
    {
        Raylib.DrawCube(new Vector3(0, 0.35f, 0), 18, 0.5f, 2.8f, new Color(48, 58, 66, 255));
        Raylib.DrawCube(new Vector3(0, 0.65f, 0), 17.7f, 0.12f, 2.48f, new Color(24, 30, 35, 255));
        Raylib.DrawCube(new Vector3(0, 0.82f, -1.28f), 18.2f, 0.16f, 0.14f, new Color(121, 132, 137, 255));
        Raylib.DrawCube(new Vector3(0, 0.82f, 1.28f), 18.2f, 0.16f, 0.14f, new Color(121, 132, 137, 255));

        for (float x = -8; x <= 8; x += 1.25f)
        {
            Raylib.DrawCylinder(new Vector3(x, 0.76f, 0), 0.18f, 0.18f, 2.25f, 18, new Color(96, 108, 114, 255));
            Raylib.DrawCube(new Vector3(x, 0.86f, 0), 0.035f, 0.025f, 2.15f, Color.SkyBlue);
        }

        Raylib.DrawCube(new Vector3(-8.9f, 0.9f, 0), 0.3f, 1.35f, 3.1f, Color.DarkGray);
        Raylib.DrawCube(new Vector3(8.9f, 0.9f, 0), 0.3f, 1.35f, 3.1f, Color.DarkGray);
        DrawSensor(new Vector3(-5.2f, 1.08f, -1.05f), Color.Lime);
        DrawSensor(new Vector3(5.2f, 1.08f, -1.05f), Color.Red);
        DrawPallet(new Vector3(-6.9f, 1.02f, 0));
        DrawBox(new Vector3(boxPosition, 1.08f, 0));
    }

    private static void DrawBox(Vector3 position)
    {
        Raylib.DrawCube(position, 0.95f, 0.5f, 0.95f, Color.Orange);
        Raylib.DrawCube(position + new Vector3(0, 0.255f, 0), 0.08f, 0.02f, 0.96f, new Color(248, 218, 125, 255));
        Raylib.DrawCube(position + new Vector3(0, 0, -0.48f), 0.42f, 0.22f, 0.015f, new Color(250, 237, 184, 255));
        Raylib.DrawCubeWires(position, 0.95f, 0.5f, 0.95f, Color.DarkBrown);
    }

    private static void DrawSensor(Vector3 position, Color color)
    {
        Raylib.DrawCube(position, 0.18f, 0.32f, 0.18f, Color.Black);
        Raylib.DrawSphere(position + new Vector3(0, 0.2f, 0), 0.07f, color);
    }

    private static void DrawPallet(Vector3 position)
    {
        Raylib.DrawCube(position, 1.3f, 0.12f, 1.2f, new Color(118, 76, 42, 255));
        for (float z = -0.42f; z <= 0.42f; z += 0.42f)
        {
            Raylib.DrawCube(position + new Vector3(0, 0.08f, z), 1.15f, 0.06f, 0.12f, new Color(178, 115, 56, 255));
        }
    }

    private static void DrawControlStation(bool running, bool emergencyStop)
    {
        Vector3 position = new(5.5f, 1.1f, -3.2f);
        Raylib.DrawCube(position, 2.2f, 2.2f, 1.2f, new Color(38, 45, 55, 255));
        Raylib.DrawCube(new Vector3(5.5f, 2.45f, -3.2f), 1.3f, 0.8f, 0.12f, Color.SkyBlue);
        Raylib.DrawCubeWires(position, 2.2f, 2.2f, 1.2f, Color.LightGray);
        Raylib.DrawCube(new Vector3(5.5f, 2.45f, -3.82f), 1.0f, 0.16f, 0.08f, Color.Black);
        Raylib.DrawSphere(new Vector3(5.15f, 2.45f, -3.9f), 0.08f, running ? Color.Lime : Color.DarkGray);
        Raylib.DrawSphere(new Vector3(5.5f, 2.45f, -3.9f), 0.08f, running ? Color.Gold : Color.DarkGray);
        Raylib.DrawSphere(new Vector3(5.85f, 2.45f, -3.9f), 0.08f, emergencyStop ? Color.Red : Color.DarkGray);
    }

    private static void DrawSafetyCell()
    {
        Color postColor = new(214, 176, 58, 255);
        Color railColor = new(232, 194, 75, 255);
        Vector3[] posts =
        [
            new(-2.9f, 1.75f, -5.0f), new(2.9f, 1.75f, -5.0f),
            new(-2.9f, 1.75f, -0.3f), new(2.9f, 1.75f, -0.3f)
        ];
        foreach (Vector3 post in posts)
        {
            Raylib.DrawCylinder(post, 0.08f, 0.08f, 3.5f, 12, postColor);
            Raylib.DrawSphere(post + new Vector3(0, 1.8f, 0), 0.12f, Color.Red);
        }
        DrawLink(posts[0], posts[1], 0.06f, railColor);
        DrawLink(posts[2], posts[3], 0.06f, railColor);
        DrawLink(posts[0] + new Vector3(0, 1.4f, 0), posts[2] + new Vector3(0, 1.4f, 0), 0.06f, railColor);
        DrawLink(posts[1] + new Vector3(0, 1.4f, 0), posts[3] + new Vector3(0, 1.4f, 0), 0.06f, railColor);
        DrawLink(posts[0] + new Vector3(0, 2.7f, 0), posts[1] + new Vector3(0, 2.7f, 0), 0.06f, railColor);
        DrawLink(posts[2] + new Vector3(0, 2.7f, 0), posts[3] + new Vector3(0, 2.7f, 0), 0.06f, railColor);
    }

    private static void DrawLink(Vector3 start, Vector3 end, float thickness, Color color)
    {
        Raylib.DrawCylinderEx(start, end, thickness, thickness, 12, color);
    }
}
