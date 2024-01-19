using System.Collections.Generic;
using UnityEngine;

namespace ColyseusDemo.Multiplayer
{
    public class MessagesNames
    {
        public const string Spawn = "spawn";
        public const string Login = "login";
        public const string TurnEnded = "turnReady";

        private const string InvalidNameErrorText = "Error: Invalid message name. Check if the message name is spelled correctly.";
        private const string InvalidDataErrorText = "Error: Invalid message data. Check that the data type of the data being sent is correct.";

        private static Dictionary<string, object> MessagesDataExamples = new Dictionary<string, object>()
        {   { Spawn, new Dictionary<string, object>(){} },
            { Login, "string" },
            { TurnEnded, 10 },
        };

        public static bool DetermineMessageCorrectness(string messageName, object messageData)
        {
            bool isCorrectName = DetermineMessageNameCorrectness(messageName);

            if (isCorrectName)
            {
                var messageExample = MessagesDataExamples[messageName];
                var keySuitableTypeName = messageExample.GetType().ToString();
                var messageDataTypeName = messageData.GetType().ToString();

                if (keySuitableTypeName != messageDataTypeName)
                {
                    Debug.LogError(InvalidDataErrorText);

                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Debug.LogError(InvalidNameErrorText);

                return false;
            }
        }

        private static bool DetermineMessageNameCorrectness(string messageName)
        {
            bool isCorrectKey = MessagesDataExamples.ContainsKey(messageName);

            return isCorrectKey;
        }
    }
}