using System.Numerics;
using Raylib_cs;

namespace RobotFactory;

public static class RobotArm
{
    private static readonly Color Shell = new(42, 54, 64, 255);
    private static readonly Color JointColor = new(239, 131, 84, 255);
    private static readonly Color LinkColor = new(196, 59, 54, 255);
    private static readonly Color ForearmColor = new(239, 196, 93, 255);

    public static void Draw(Vector3 basePosition, float[] angles)
    {
        float yaw = angles[0];
        float shoulderAngle = angles[1] - 0.35f;
        float elbowAngle = angles[2] + 0.8f;
        Vector3 direction = new(MathF.Sin(yaw), 0, MathF.Cos(yaw));
        Vector3 armDirection = new(MathF.Sin(yaw + shoulderAngle), 0, MathF.Cos(yaw + shoulderAngle));
        Vector3 forearmDirection = new(MathF.Sin(yaw + shoulderAngle + elbowAngle), 0, MathF.Cos(yaw + shoulderAngle + elbowAngle));
        Vector3 turret = basePosition + new Vector3(0, 0.75f, 0);
        Vector3 shoulder = turret + new Vector3(0, 0.8f, 0);
        Vector3 elbow = shoulder + armDirection * 1.45f + new Vector3(0, 0.25f, 0);
        Vector3 wrist = elbow + forearmDirection * 1.25f + new Vector3(0, -0.15f, 0);
        Vector3 tool = wrist + forearmDirection * 0.42f + new Vector3(0, -0.18f, 0);

        Raylib.DrawCylinder(basePosition, 1.0f, 1.0f, 0.45f, 32, Shell);
        Raylib.DrawCylinder(basePosition + new Vector3(0, 0.25f, 0), 0.75f, 0.75f, 0.12f, 32, Color.SkyBlue);
        Raylib.DrawCylinder(turret, 0.58f, 0.58f, 0.7f, 24, Color.DarkBlue);
        Raylib.DrawCylinder(turret + new Vector3(0, 0.38f, 0), 0.48f, 0.48f, 0.08f, 24, Color.LightGray);

        DrawJoint(turret, 0.42f, JointColor);
        DrawJoint(shoulder, 0.34f, JointColor);
        DrawJoint(elbow, 0.3f, JointColor);
        DrawJoint(wrist, 0.22f, Color.Gold);

        DrawLink(shoulder - direction * 0.22f, elbow - direction * 0.22f, 0.34f, Shell);
        DrawLink(shoulder + direction * 0.22f, elbow + direction * 0.22f, 0.34f, Shell);
        DrawLink(shoulder - direction * 0.22f, elbow - direction * 0.22f, 0.25f, LinkColor);
        DrawLink(shoulder + direction * 0.22f, elbow + direction * 0.22f, 0.25f, LinkColor);
        DrawLink(elbow - direction * 0.18f, wrist - direction * 0.18f, 0.29f, Shell);
        DrawLink(elbow + direction * 0.18f, wrist + direction * 0.18f, 0.29f, Shell);
        DrawLink(elbow - direction * 0.18f, wrist - direction * 0.18f, 0.21f, ForearmColor);
        DrawLink(elbow + direction * 0.18f, wrist + direction * 0.18f, 0.21f, ForearmColor);
        DrawLink(wrist, tool, 0.17f, Color.LightGray);
        DrawLink(shoulder + direction * 0.34f + new Vector3(0, 0.18f, 0), elbow + direction * 0.34f + new Vector3(0, 0.18f, 0), 0.045f, Color.Black);
        DrawLink(elbow + direction * 0.3f + new Vector3(0, 0.18f, 0), wrist + direction * 0.3f + new Vector3(0, 0.18f, 0), 0.04f, Color.Black);

        Raylib.DrawCylinder(tool, 0.22f, 0.22f, 0.3f, 18, Color.DarkGray);
        Raylib.DrawCylinder(tool + new Vector3(0, 0.16f, 0), 0.28f, 0.28f, 0.08f, 18, JointColor);
        Vector3 jawOffset = new(MathF.Cos(yaw) * 0.18f, 0, -MathF.Sin(yaw) * 0.18f);
        DrawLink(tool + jawOffset, tool + jawOffset + forearmDirection * 0.32f + new Vector3(0, -0.1f, 0), 0.07f, Color.Yellow);
        DrawLink(tool - jawOffset, tool - jawOffset + forearmDirection * 0.32f + new Vector3(0, -0.1f, 0), 0.07f, Color.Yellow);
    }

    private static void DrawJoint(Vector3 position, float radius, Color color)
    {
        Raylib.DrawSphere(position, radius, color);
        Raylib.DrawCylinder(position - new Vector3(0, radius * 0.7f, 0), radius * 0.72f, radius * 0.72f, radius * 0.3f, 20, Shell);
        Raylib.DrawCylinder(position + new Vector3(0, radius * 0.7f, 0), radius * 0.72f, radius * 0.72f, radius * 0.12f, 20, Color.LightGray);
    }

    private static void DrawLink(Vector3 start, Vector3 end, float thickness, Color color)
    {
        Raylib.DrawCylinderEx(start, end, thickness, thickness, 16, color);
    }
}
