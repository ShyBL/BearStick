using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerMovementEditorWindow : EditorWindow
{
    private PlayerMovementStats playerMovementStats;
    private PlayerMovementStatsData savedPlayerMovementStatsData;
    #region Serialized Stuff

    private SerializedObject serializedPlayerMovementStats;
    private SerializedProperty maxWalkSpeed;
    private SerializedProperty groundAcceleration;
    private SerializedProperty groundDeceleration;
    private SerializedProperty airAcceleration;
    private SerializedProperty airDeceleration;
    private SerializedProperty maxRunSpeed;

    //private SerializedProperty groundLayer;
    private SerializedProperty groundDetectionRayLength;
    private SerializedProperty headDetectionRayLength;
    private SerializedProperty headWidth;

    private SerializedProperty jumpHeight;
    private SerializedProperty jumpHeightCompensationFactor;
    private SerializedProperty timeTillJumpApex;

    private SerializedProperty gravityOnReleaseMultiplier;
    private SerializedProperty maxFallSpeed;
    private SerializedProperty numberOfJumpsAllowed;

    private SerializedProperty timeForUpwardsCancel;
    private SerializedProperty apexThreshold;
    private SerializedProperty apexHangTime;

    private SerializedProperty jumpBufferTime;
    private SerializedProperty jumpCoyoteTime;

    private SerializedProperty debugShowIsGroundedBox;
    private SerializedProperty debugShowHeadBumpBox;

    private SerializedProperty showWalkJumpArc;
    private SerializedProperty showRunJumpArc;
    private SerializedProperty stopOnCollision;
    private SerializedProperty drawRight;
    private SerializedProperty arcResolution;
    private SerializedProperty visualizationSteps;
    #endregion
    private string saveFilePath;

    [MenuItem("Window/Player Movement Stats Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerMovementEditorWindow>("Player Movement Stats Editor");
    }

    private void OnEnable()
    {
        saveFilePath = Application.persistentDataPath + "/PlayerMovementStatsData.dat";
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Movement Stats Editor", EditorStyles.boldLabel);

        playerMovementStats = (PlayerMovementStats)EditorGUILayout.ObjectField("Player Movement Stats", playerMovementStats, typeof(PlayerMovementStats), false);

        if (playerMovementStats != null)
        {
            // If the ScriptableObject is selected, serialize it to modify
            if (serializedPlayerMovementStats == null || serializedPlayerMovementStats.targetObject != playerMovementStats)
            {
                serializedPlayerMovementStats = new SerializedObject(playerMovementStats);

                maxWalkSpeed = serializedPlayerMovementStats.FindProperty("MaxWalkSpeed");
                groundAcceleration = serializedPlayerMovementStats.FindProperty("GroundAcceleration");
                groundDeceleration = serializedPlayerMovementStats.FindProperty("GroundDeceleration");
                airAcceleration = serializedPlayerMovementStats.FindProperty("AirAcceleration");
                airDeceleration = serializedPlayerMovementStats.FindProperty("AirDeceleration");
                maxRunSpeed = serializedPlayerMovementStats.FindProperty("MaxRunSpeed");

                //groundLayer = serializedPlayerMovementStats.FindProperty("GroundLayer");
                groundDetectionRayLength = serializedPlayerMovementStats.FindProperty("GroundDetectionRayLength");
                headDetectionRayLength = serializedPlayerMovementStats.FindProperty("HeadDetectionRayLength");
                headWidth = serializedPlayerMovementStats.FindProperty("HeadWidth");

                jumpHeight = serializedPlayerMovementStats.FindProperty("JumpHeight");
                jumpHeightCompensationFactor = serializedPlayerMovementStats.FindProperty("JumpHeightCompensationFactor");
                timeTillJumpApex = serializedPlayerMovementStats.FindProperty("TimeTillJumpApex");

                gravityOnReleaseMultiplier = serializedPlayerMovementStats.FindProperty("GravityOnReleaseMultiplier");
                maxFallSpeed = serializedPlayerMovementStats.FindProperty("MaxFallSpeed");
                numberOfJumpsAllowed = serializedPlayerMovementStats.FindProperty("NumberOfJumpsAllowed");

                timeForUpwardsCancel = serializedPlayerMovementStats.FindProperty("TimeForUpwardsCancel");
                apexThreshold = serializedPlayerMovementStats.FindProperty("ApexThreshold");
                apexHangTime = serializedPlayerMovementStats.FindProperty("ApexHangTime");

                jumpBufferTime = serializedPlayerMovementStats.FindProperty("JumpBufferTime");
                jumpCoyoteTime = serializedPlayerMovementStats.FindProperty("JumpCoyoteTime");

                debugShowIsGroundedBox = serializedPlayerMovementStats.FindProperty("DebugShowIsGroundedBox");
                debugShowHeadBumpBox = serializedPlayerMovementStats.FindProperty("DebugShowHeadBumpBox");

                showWalkJumpArc = serializedPlayerMovementStats.FindProperty("ShowWalkJumpArc");
                showRunJumpArc = serializedPlayerMovementStats.FindProperty("ShowRunJumpArc");
                stopOnCollision = serializedPlayerMovementStats.FindProperty("StopOnCollision");
                drawRight = serializedPlayerMovementStats.FindProperty("DrawRight");
                arcResolution = serializedPlayerMovementStats.FindProperty("ArcResolution");
                visualizationSteps = serializedPlayerMovementStats.FindProperty("VisualizationSteps");
            }

            serializedPlayerMovementStats.Update();

            // Display and modify the values in the editor window
            EditorGUILayout.PropertyField(maxWalkSpeed);
            EditorGUILayout.PropertyField(groundAcceleration);
            EditorGUILayout.PropertyField(groundDeceleration);
            EditorGUILayout.PropertyField(airAcceleration);
            EditorGUILayout.PropertyField(airDeceleration);
            EditorGUILayout.PropertyField(maxRunSpeed);

            //EditorGUILayout.PropertyField(groundLayer);
            EditorGUILayout.PropertyField(groundDetectionRayLength);
            EditorGUILayout.PropertyField(headDetectionRayLength);
            EditorGUILayout.PropertyField(headWidth);

            EditorGUILayout.PropertyField(jumpHeight);
            EditorGUILayout.PropertyField(jumpHeightCompensationFactor);
            EditorGUILayout.PropertyField(timeTillJumpApex);

            EditorGUILayout.PropertyField(gravityOnReleaseMultiplier);
            EditorGUILayout.PropertyField(maxFallSpeed);
            EditorGUILayout.PropertyField(numberOfJumpsAllowed);

            EditorGUILayout.PropertyField(timeForUpwardsCancel);
            EditorGUILayout.PropertyField(apexThreshold);
            EditorGUILayout.PropertyField(apexHangTime);

            EditorGUILayout.PropertyField(jumpBufferTime);
            EditorGUILayout.PropertyField(jumpCoyoteTime);

            EditorGUILayout.PropertyField(debugShowIsGroundedBox);
            EditorGUILayout.PropertyField(debugShowHeadBumpBox);

            EditorGUILayout.PropertyField(showWalkJumpArc);
            EditorGUILayout.PropertyField(showRunJumpArc);
            EditorGUILayout.PropertyField(stopOnCollision);
            EditorGUILayout.PropertyField(drawRight);
            EditorGUILayout.PropertyField(arcResolution);
            EditorGUILayout.PropertyField(visualizationSteps);

            serializedPlayerMovementStats.ApplyModifiedProperties();
            EditorUtility.SetDirty(playerMovementStats);
            
            if (GUILayout.Button("Save Original Values"))
            {
                SaveOriginalValues();
            }

            if (GUILayout.Button("Load Original Values"))
            {
                LoadOriginalValues();
            }
        }
    }

    private void SaveOriginalValues()
    {
        savedPlayerMovementStatsData = new PlayerMovementStatsData(playerMovementStats);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        bf.Serialize(file, savedPlayerMovementStatsData);
        file.Close();

        Debug.Log("Original values saved.");
    }

    private void LoadOriginalValues()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            savedPlayerMovementStatsData = (PlayerMovementStatsData)bf.Deserialize(file);
            file.Close();

            savedPlayerMovementStatsData.ApplyTo(playerMovementStats);
            serializedPlayerMovementStats.Update();
            Debug.Log("Original values loaded.");
        }
        else
        {
            Debug.LogWarning("No saved original values found.");
        }
    }
}

[System.Serializable]
public class PlayerMovementStatsData
{
    public float MaxWalkSpeed;
    public float GroundAcceleration;
    public float GroundDeceleration;
    public float AirAcceleration;
    public float AirDeceleration;
    public float MaxRunSpeed;

    //public LayerMask GroundLayer;
    public float GroundDetectionRayLength;
    public float HeadDetectionRayLength;
    public float HeadWidth;

    public float JumpHeight;
    public float JumpHeightCompensationFactor;
    public float TimeTillJumpApex;

    public float GravityOnReleaseMultiplier;
    public float MaxFallSpeed;
    public int NumberOfJumpsAllowed;

    public float TimeForUpwardsCancel;
    public float ApexThreshold;
    public float ApexHangTime;

    public float JumpBufferTime;
    public float JumpCoyoteTime;

    public bool DebugShowIsGroundedBox;
    public bool DebugShowHeadBumpBox;

    public bool ShowWalkJumpArc;
    public bool ShowRunJumpArc;
    public bool StopOnCollision;
    public bool DrawRight;
    public int ArcResolution;
    public int VisualizationSteps;

    public PlayerMovementStatsData(PlayerMovementStats stats)
    {
        MaxWalkSpeed = stats.MaxWalkSpeed;
        GroundAcceleration = stats.GroundAcceleration;
        GroundDeceleration = stats.GroundDeceleration;
        AirAcceleration = stats.AirAcceleration;
        AirDeceleration = stats.AirDeceleration;
        MaxRunSpeed = stats.MaxRunSpeed;

        //GroundLayer = stats.GroundLayer;
        GroundDetectionRayLength = stats.GroundDetectionRayLength;
        HeadDetectionRayLength = stats.HeadDetectionRayLength;
        HeadWidth = stats.HeadWidth;

        JumpHeight = stats.JumpHeight;
        JumpHeightCompensationFactor = stats.JumpHeightCompensationFactor;
        TimeTillJumpApex = stats.TimeTillJumpApex;

        GravityOnReleaseMultiplier = stats.GravityOnReleaseMultiplier;
        MaxFallSpeed = stats.MaxFallSpeed;
        NumberOfJumpsAllowed = stats.NumberOfJumpsAllowed;

        TimeForUpwardsCancel = stats.TimeForUpwardsCancel;
        ApexThreshold = stats.ApexThreshold;
        ApexHangTime = stats.ApexHangTime;

        JumpBufferTime = stats.JumpBufferTime;
        JumpCoyoteTime = stats.JumpCoyoteTime;

        DebugShowIsGroundedBox = stats.DebugShowIsGroundedBox;
        DebugShowHeadBumpBox = stats.DebugShowHeadBumpBox;

        ShowWalkJumpArc = stats.ShowWalkJumpArc;
        ShowRunJumpArc = stats.ShowRunJumpArc;
        StopOnCollision = stats.StopOnCollision;
        DrawRight = stats.DrawRight;
        ArcResolution = stats.ArcResolution;
        VisualizationSteps = stats.VisualizationSteps;
    }

    public void ApplyTo(PlayerMovementStats stats)
    {
        stats.MaxWalkSpeed = MaxWalkSpeed;
        stats.GroundAcceleration = GroundAcceleration;
        stats.GroundDeceleration = GroundDeceleration;
        stats.AirAcceleration = AirAcceleration;
        stats.AirDeceleration = AirDeceleration;
        stats.MaxRunSpeed = MaxRunSpeed;

       // stats.GroundLayer = GroundLayer;
        stats.GroundDetectionRayLength = GroundDetectionRayLength;
        stats.HeadDetectionRayLength = HeadDetectionRayLength;
        stats.HeadWidth = HeadWidth;

        stats.JumpHeight = JumpHeight;
        stats.JumpHeightCompensationFactor = JumpHeightCompensationFactor;
        stats.TimeTillJumpApex = TimeTillJumpApex;

        stats.GravityOnReleaseMultiplier = GravityOnReleaseMultiplier;
        stats.MaxFallSpeed = MaxFallSpeed;
        stats.NumberOfJumpsAllowed = NumberOfJumpsAllowed;

        stats.TimeForUpwardsCancel = TimeForUpwardsCancel;
        stats.ApexThreshold = ApexThreshold;
        stats.ApexHangTime = ApexHangTime;

        stats.JumpBufferTime = JumpBufferTime;
        stats.JumpCoyoteTime = JumpCoyoteTime;

        stats.DebugShowIsGroundedBox = DebugShowIsGroundedBox;
        stats.DebugShowHeadBumpBox = DebugShowHeadBumpBox;

        stats.ShowWalkJumpArc = ShowWalkJumpArc;
        stats.ShowRunJumpArc = ShowRunJumpArc;
        stats.StopOnCollision = StopOnCollision;
        stats.DrawRight = DrawRight;
        stats.ArcResolution = ArcResolution;
        stats.VisualizationSteps = VisualizationSteps;
    }
}
