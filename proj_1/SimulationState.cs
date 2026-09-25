namespace RobotFactory;

public sealed class SimulationState
{
    public bool Running { get; set; } = true;
    public bool ManualMode { get; set; }
    public bool EmergencyStop { get; set; }
    public int SelectedAxis { get; set; }
    public float Speed { get; set; } = 1.0f;
    public float ConveyorPosition { get; private set; } = -7.5f;
    public float RobotCycle { get; private set; }
    public int BoxesProcessed { get; private set; }
    public float[] JointAngles { get; } = new float[6];

    public void Reset()
    {
        Running = true;
        ManualMode = false;
        EmergencyStop = false;
        SelectedAxis = 0;
        Speed = 1.0f;
        ConveyorPosition = -7.5f;
        RobotCycle = 0;
        BoxesProcessed = 0;
        Array.Clear(JointAngles);
    }

    public void Update(float deltaTime)
    {
        if (!Running || EmergencyStop)
        {
            return;
        }

        ConveyorPosition += deltaTime * 2.2f * Speed;
        if (ConveyorPosition > 7.5f)
        {
            ConveyorPosition = -7.5f;
        }

        RobotCycle += deltaTime * Speed;
        if (!ManualMode)
        {
            UpdateAutomaticPose();
        }

        if (RobotCycle >= 4.2f)
        {
            RobotCycle = 0;
            BoxesProcessed++;
        }
    }

    public void MoveSelectedAxis(float amount)
    {
        if (ManualMode && !EmergencyStop)
        {
            JointAngles[SelectedAxis] = Math.Clamp(JointAngles[SelectedAxis] + amount, -MathF.PI, MathF.PI);
        }
    }

    private void UpdateAutomaticPose()
    {
        JointAngles[0] = MathF.Sin(RobotCycle * 1.1f) * 0.55f;
        JointAngles[1] = MathF.Sin(RobotCycle * 1.7f) * 0.35f;
        JointAngles[2] = MathF.Cos(RobotCycle * 1.7f) * 0.45f;
        JointAngles[3] = MathF.Sin(RobotCycle * 2.1f) * 0.8f;
        JointAngles[4] = MathF.Cos(RobotCycle * 1.4f) * 0.5f;
        JointAngles[5] = MathF.Sin(RobotCycle * 2.8f) * 1.1f;
    }
}
