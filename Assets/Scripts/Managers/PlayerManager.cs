using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        InputUser user = playerInput.user;

        if (playerInput.devices.Count == 0)
        {
            Debug.LogError($"No device found {playerInput.playerIndex}.");
            return;
        }

        InputDevice device = playerInput.devices[0];
        string deviceName = device.name.ToLower();
        string controlScheme;

        if (deviceName.Contains("switch"))
        {
            controlScheme = "Switch";
        }
        else if (deviceName.Contains("keyboard"))
        {
            controlScheme = "Keyboard";
        }
        else
        {
            controlScheme = "Gamepad";
        }

        Debug.Log($"Device detect : {deviceName}, Selected schema : {controlScheme}");

        try
        {
            playerInput.SwitchCurrentControlScheme(controlScheme, device);
        }
        catch (System.ArgumentException)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad", device);
        }

        user.UnpairDevices();

        InputUser.PerformPairingWithDevice(device, user, InputUserPairingOptions.None);
        playerInput.neverAutoSwitchControlSchemes = true;

        Camera camera = playerInput.GetComponentInChildren<Camera>();
        if (camera != null)
        {
            int playerCount = PlayerInputManager.instance.playerCount;
            if (playerCount == 2)
            {
                if (playerInput.playerIndex == 0)
                    camera.rect = new Rect(0, 0, 0.5f, 1);
                else
                    camera.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
        }

        playerInput.transform.position = GetSpawnPosition(playerInput.playerIndex);
        Debug.Log($"Player {playerInput.playerIndex} spawn at {playerInput.transform.position}");
    }

    private Vector3 GetSpawnPosition(int playerIndex)
    {
        Vector3[] spawnPoints = new Vector3[]
        {
            new Vector3(-2, 2, 0),
            new Vector3(2, 2, 0),
            new Vector3(-2, 2, 2),
            new Vector3(2, 2, 2)
        };
        return spawnPoints[playerIndex % spawnPoints.Length];
    }
}
