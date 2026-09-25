using Raylib_cs;

namespace RobotFactory;

public sealed class HudRenderer
{
    public void Draw(SimulationState state, int screenWidth, int screenHeight)
    {
        Raylib.DrawRectangle(20, 20, 365, 116, new Color(12, 17, 23, 235));
        Raylib.DrawRectangleLines(20, 20, 365, 116, new Color(76, 201, 240, 255));
        Raylib.DrawText("MINI FACTORY CELL", 38, 34, 24, Color.RayWhite);
        Raylib.DrawText($"Trang thai: {GetStatus(state)}", 38, 70, 18, GetStatusColor(state));
        Raylib.DrawText($"Da xu ly: {state.BoxesProcessed} hop", 38, 98, 18, Color.LightGray);

        DrawRobotPanel(state, screenWidth);
        Raylib.DrawText("SPACE: chay/dung   M: manual   E: E-stop   R: reset", 20, screenHeight - 34, 18, Color.LightGray);
    }

    private static void DrawRobotPanel(SimulationState state, int screenWidth)
    {
        int panelX = screenWidth - 290;
        Raylib.DrawRectangle(panelX, 20, 260, 250, new Color(12, 17, 23, 235));
        Raylib.DrawRectangleLines(panelX, 20, 260, 250, new Color(239, 131, 84, 255));
        Raylib.DrawText("ROBOT CONTROL", panelX + 15, 34, 18, Color.Orange);
        Raylib.DrawRectangle(panelX + 15, 48, 105, 24, state.ManualMode ? Color.Orange : Color.DarkGray);
        Raylib.DrawRectangle(panelX + 135, 48, 105, 24, state.EmergencyStop ? Color.Gold : Color.Maroon);
        Raylib.DrawText("AUTO / MANUAL", panelX + 22, 55, 10, Color.RayWhite);
        Raylib.DrawText("E-STOP", panelX + 164, 55, 10, Color.RayWhite);
        Raylib.DrawText($"MODE: {(state.ManualMode ? "MANUAL" : "AUTO")}", panelX + 15, 86, 16, Color.LightGray);
        Raylib.DrawText($"STATUS: {GetStatus(state)}", panelX + 15, 108, 16, GetStatusColor(state));
        Raylib.DrawText("J1-J6: chon truc", panelX + 15, 132, 13, Color.Gray);
        Raylib.DrawText("LEFT/RIGHT hoac W/S", panelX + 15, 148, 13, Color.Gray);

        for (int index = 0; index < state.JointAngles.Length; index++)
        {
            int y = 164 + index * 15;
            if (index == state.SelectedAxis)
            {
                Raylib.DrawRectangle(panelX + 12, y - 2, 235, 15, new Color(58, 43, 38, 255));
            }
            Color color = index == state.SelectedAxis ? Color.Orange : Color.LightGray;
            Raylib.DrawText($"J{index + 1}: {state.JointAngles[index] * 57.2958f,6:0.0} deg", panelX + 15, y, 13, color);
        }
    }

    private static string GetStatus(SimulationState state) => state.EmergencyStop ? "E-STOP" : state.Running ? "DANG CHAY" : "TAM DUNG";

    private static Color GetStatusColor(SimulationState state) => state.EmergencyStop ? Color.Red : state.Running ? Color.Lime : Color.Gold;
}
