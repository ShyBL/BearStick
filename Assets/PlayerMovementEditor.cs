using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementEditor : MonoBehaviour
{
    public PlayerMovementStats stats; // Reference to the ScriptableObject
    public TMP_InputField maxWalkSpeedField;
    public TMP_InputField groundAccelerationField;
    public TMP_InputField groundDecelerationField;
    public TMP_InputField airAccelerationField;
    public TMP_InputField airDecelerationField;
    public TMP_InputField maxRunSpeedField;
    //public TMP_InputField groundLayerField; // Assuming it's represented as a string
    public TMP_InputField groundDetectionRayLengthField;
    public TMP_InputField headDetectionRayLengthField;
    public TMP_InputField headWidthField;
    public TMP_InputField jumpHeightField;
    public TMP_InputField jumpHeightCompensationFactorField;
    public TMP_InputField timeTillJumpApexField;
    public TMP_InputField gravityOnReleaseMultiplierField;
    public TMP_InputField maxFallSpeedField;
    public TMP_InputField numberOfJumpsAllowedField;
    public TMP_InputField timeForUpwardsCancelField;
    public TMP_InputField apexThresholdField;
    public TMP_InputField apexHangTimeField;
    public TMP_InputField jumpBufferTimeField;
    public TMP_InputField jumpCoyoteTimeField;
    // public InputField debugShowIsGroundedBoxField;
    // public InputField debugShowHeadBumpBoxField;
    // public InputField showWalkJumpArcField;
    // public InputField showRunJumpArcField;
    // public InputField stopOnCollisionField;
    // public InputField drawRightField;
    // public InputField arcResolutionField;
    // public InputField visualizationStepsField;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize input fields with the current ScriptableObject values
        maxWalkSpeedField.text = stats.MaxWalkSpeed.ToString();
        groundAccelerationField.text = stats.GroundAcceleration.ToString();
        groundDecelerationField.text = stats.GroundDeceleration.ToString();
        airAccelerationField.text = stats.AirAcceleration.ToString();
        airDecelerationField.text = stats.AirDeceleration.ToString();
        maxRunSpeedField.text = stats.MaxRunSpeed.ToString();
        //groundLayerField.text = LayerMask.LayerToName(stats.GroundLayer); // Convert LayerMask to string for UI
        groundDetectionRayLengthField.text = stats.GroundDetectionRayLength.ToString();
        headDetectionRayLengthField.text = stats.HeadDetectionRayLength.ToString();
        headWidthField.text = stats.HeadWidth.ToString();
        jumpHeightField.text = stats.JumpHeight.ToString();
        jumpHeightCompensationFactorField.text = stats.JumpHeightCompensationFactor.ToString();
        timeTillJumpApexField.text = stats.TimeTillJumpApex.ToString();
        gravityOnReleaseMultiplierField.text = stats.GravityOnReleaseMultiplier.ToString();
        maxFallSpeedField.text = stats.MaxFallSpeed.ToString();
        numberOfJumpsAllowedField.text = stats.NumberOfJumpsAllowed.ToString();
        timeForUpwardsCancelField.text = stats.TimeForUpwardsCancel.ToString();
        apexThresholdField.text = stats.ApexThreshold.ToString();
        apexHangTimeField.text = stats.ApexHangTime.ToString();
        jumpBufferTimeField.text = stats.JumpBufferTime.ToString();
        jumpCoyoteTimeField.text = stats.JumpCoyoteTime.ToString();
        // debugShowIsGroundedBoxField.text = stats.DebugShowIsGroundedBox.ToString();
        // debugShowHeadBumpBoxField.text = stats.DebugShowHeadBumpBox.ToString();
        // showWalkJumpArcField.text = stats.ShowWalkJumpArc.ToString();
        // showRunJumpArcField.text = stats.ShowRunJumpArc.ToString();
        // stopOnCollisionField.text = stats.StopOnCollision.ToString();
        // drawRightField.text = stats.DrawRight.ToString();
        // arcResolutionField.text = stats.ArcResolution.ToString();
        // visualizationStepsField.text = stats.VisualizationSteps.ToString();

        // Add listeners to handle changes from the input fields
        maxWalkSpeedField.onValueChanged.AddListener(value => OnFieldChanged("MaxWalkSpeed", value));
        groundAccelerationField.onValueChanged.AddListener(value => OnFieldChanged("GroundAcceleration", value));
        groundDecelerationField.onValueChanged.AddListener(value => OnFieldChanged("GroundDeceleration", value));
        airAccelerationField.onValueChanged.AddListener(value => OnFieldChanged("AirAcceleration", value));
        airDecelerationField.onValueChanged.AddListener(value => OnFieldChanged("AirDeceleration", value));
        maxRunSpeedField.onValueChanged.AddListener(value => OnFieldChanged("MaxRunSpeed", value));
        //groundLayerField.onValueChanged.AddListener(value => OnFieldChanged("GroundLayer", value)); // Handle LayerMask as a string
        groundDetectionRayLengthField.onValueChanged.AddListener(value => OnFieldChanged("GroundDetectionRayLength", value));
        headDetectionRayLengthField.onValueChanged.AddListener(value => OnFieldChanged("HeadDetectionRayLength", value));
        headWidthField.onValueChanged.AddListener(value => OnFieldChanged("HeadWidth", value));
        jumpHeightField.onValueChanged.AddListener(value => OnFieldChanged("JumpHeight", value));
        jumpHeightCompensationFactorField.onValueChanged.AddListener(value => OnFieldChanged("JumpHeightCompensationFactor", value));
        timeTillJumpApexField.onValueChanged.AddListener(value => OnFieldChanged("TimeTillJumpApex", value));
        gravityOnReleaseMultiplierField.onValueChanged.AddListener(value => OnFieldChanged("GravityOnReleaseMultiplier", value));
        maxFallSpeedField.onValueChanged.AddListener(value => OnFieldChanged("MaxFallSpeed", value));
        numberOfJumpsAllowedField.onValueChanged.AddListener(value => OnFieldChanged("NumberOfJumpsAllowed", value));
        timeForUpwardsCancelField.onValueChanged.AddListener(value => OnFieldChanged("TimeForUpwardsCancel", value));
        apexThresholdField.onValueChanged.AddListener(value => OnFieldChanged("ApexThreshold", value));
        apexHangTimeField.onValueChanged.AddListener(value => OnFieldChanged("ApexHangTime", value));
        jumpBufferTimeField.onValueChanged.AddListener(value => OnFieldChanged("JumpBufferTime", value));
        jumpCoyoteTimeField.onValueChanged.AddListener(value => OnFieldChanged("JumpCoyoteTime", value));
        // debugShowIsGroundedBoxField.onValueChanged.AddListener(value => OnFieldChanged("DebugShowIsGroundedBox", value));
        // debugShowHeadBumpBoxField.onValueChanged.AddListener(value => OnFieldChanged("DebugShowHeadBumpBox", value));
        // showWalkJumpArcField.onValueChanged.AddListener(value => OnFieldChanged("ShowWalkJumpArc", value));
        // showRunJumpArcField.onValueChanged.AddListener(value => OnFieldChanged("ShowRunJumpArc", value));
        // stopOnCollisionField.onValueChanged.AddListener(value => OnFieldChanged("StopOnCollision", value));
        // drawRightField.onValueChanged.AddListener(value => OnFieldChanged("DrawRight", value));
        // arcResolutionField.onValueChanged.AddListener(value => OnFieldChanged("ArcResolution", value));
        // visualizationStepsField.onValueChanged.AddListener(value => OnFieldChanged("VisualizationSteps", value));
    }

    private void OnFieldChanged(string propertyName, string value)
    {
        if (float.TryParse(value, out float newValue))
        {
            // Use reflection to set the property on the ScriptableObject
            PropertyInfo property = typeof(PlayerMovementStats).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(stats, newValue);
                stats.CalculateValues(); // Call this to recalculate gravity and velocity if necessary
            }
            else
            {
                Debug.LogWarning($"Property {propertyName} not found or not writable.");
            }
        }
        else if (bool.TryParse(value, out bool boolValue)) // Handle boolean properties
        {
            PropertyInfo property = typeof(PlayerMovementStats).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(stats, boolValue);
            }
            else
            {
                Debug.LogWarning($"Property {propertyName} not found or not writable.");
            }
        }
    }
}
