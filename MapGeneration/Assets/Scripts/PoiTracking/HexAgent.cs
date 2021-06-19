using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAgent : MonoBehaviour
{
    [SerializeField] MessageBroadcastOptions _broadcastOption = MessageBroadcastOptions.LocalOnly;

    private readonly List<HexBase> _currentHexes = new List<HexBase>();

    public HexBase[] OverlappingHexes => _currentHexes.ToArray();
    public HexBase CurrentHex => _currentHexes.Count == 0 ? null : _currentHexes[_currentHexes.Count - 1];


    public void EnterHex(HexBase hex)
    {
        _currentHexes.Add(hex);
        BroadcastMessageUtility(gameObject, "OnEnterHex", hex, _broadcastOption, SendMessageOptions.DontRequireReceiver);
    }

    public void LeaveHex(HexBase hex)
    {
        if (_currentHexes.Remove(hex))
            BroadcastMessageUtility(gameObject, "OnLeaveHex", hex, _broadcastOption, SendMessageOptions.DontRequireReceiver);
    }


    /// <summary>
    /// Send a message locally, or broadcast it upwards, downwards or from the root.
    /// </summary>
    /// <param name="transmitter">The GameObject that sends the message.</param>
    /// <param name="methodName">The name of the method to call.</param>
    /// <param name="value">An optional parameter value to pass to the caleld method.</param>
    /// <param name="broadcastOptions">The mode for transmitting the message.</param>
    /// <param name="sendOptions">Should an error be raised if the method does not exist?</param>
    public static void BroadcastMessageUtility(GameObject transmitter, string methodName, object value, MessageBroadcastOptions broadcastOptions = MessageBroadcastOptions.BroadcastFromRoot, SendMessageOptions sendOptions = SendMessageOptions.DontRequireReceiver)
    {
        // TODO: move this to some static utility class
        switch (broadcastOptions)
        {
            case MessageBroadcastOptions.LocalOnly:
                transmitter.SendMessage(methodName, value, sendOptions);
                break;
            case MessageBroadcastOptions.Downwards:
                transmitter.BroadcastMessage(methodName, value, sendOptions);
                break;
            case MessageBroadcastOptions.Upwards:
                transmitter.SendMessageUpwards(methodName, value, sendOptions);
                break;
            case MessageBroadcastOptions.UpAndDown:
                transmitter.SendMessageUpwards(methodName, value, sendOptions);
                transmitter.BroadcastMessage(methodName, value, sendOptions);
                break;
            case MessageBroadcastOptions.BroadcastFromRoot:
            default:
                transmitter.transform.root.BroadcastMessage(methodName, value, sendOptions);
                break;
        }
    }
}
