using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameUtility 
{
    public static bool IsTooCloseToOtherObject(List<GameEntityAbs> instantiatedGEs,  Vector3 testPosition, 
        float testObjectRadius)
    {
        for (int i = 0; i < instantiatedGEs.Count; i++)
        {
            GameEntityAbs existingObject = instantiatedGEs[i];
            float distToExistingObject = Vector3.Distance(testPosition, existingObject.gameObject.transform.position);

            float existingObjectRadius = existingObject.GetRadius();

            float combinedObjectRadius = testObjectRadius + existingObjectRadius;

            if (distToExistingObject < combinedObjectRadius + 2.0f)
            {
                return true;                 
            }
        }
        return false;
    }
    public static bool HasLineOfSightToOtherShip(List<GameEntityAbs> instantiatedGEs, Vector3 shipPosition, 
        Dictionary<ulong, Player> otherShips)
    {
        foreach (var kvp in otherShips)
        {
            var ship = kvp.Value;
            if(!ship)
                continue;
            if (HasLineOfSightToOtherObject(instantiatedGEs, shipPosition, ship.transform.position, 3.5f))
            {
                return true;
            }
        }
        return false;
    }
    private static bool HasLineOfSightToOtherObject(List<GameEntityAbs> instantiatedGEs, Vector3 shipPosition, 
        Vector3 otherPosition, float testRadius)
    {
        Vector3 toOtherPosition = otherPosition - shipPosition;
        Vector3 linePerp = Vector3.Cross(toOtherPosition, Vector3.forward).normalized;

        for (int i = 0; i < instantiatedGEs.Count; i++)
        {
            var existingObject = instantiatedGEs[i];
            if( existingObject is Player)
            {
                continue;
            }

            float existingObjectRadius = existingObject.GetRadius();
            float combinedRadius = existingObjectRadius + testRadius;

            float distToLine = Mathf.Abs(Vector3.Dot(existingObject.transform.position - shipPosition, linePerp));

            if( distToLine < combinedRadius )
            {
                float distAlongLine = Vector3.Dot(existingObject.transform.position - shipPosition, toOtherPosition.normalized);

                if( distAlongLine >= 0.0f && distAlongLine <= toOtherPosition.magnitude)
                {
                    return false;
                }
            }

            if( Vector3.Distance(shipPosition,existingObject.transform.position) < testRadius)
            {
                return false;
            }
        }

        return true;
    }
    
    public static bool IsTooCloseToOtherObject(Vector3 testPosition, GameEntityAbs testObject, 
        List<GameEntityAbs> instantiatedGEs)
    {   
        float testObjectRadius = testObject.GetScale();

        for (int i = 0; i < instantiatedGEs.Count; i++)
        {
            var existingObject = instantiatedGEs[i];
            float distToExistingObject = Vector3.Distance(testPosition, existingObject.transform.position);

            float existingObjectRadius = existingObject.GetRadius();

            float combinedObjectRadius = testObjectRadius + existingObjectRadius;

            if (distToExistingObject < combinedObjectRadius + 2.0f)
            {
                return true;                 
            }
        }

        return false;
    }

    public static byte[] ToByteArray(object source)
    {
        var formatter = new BinaryFormatter();
        using var stream = new MemoryStream();
        formatter.Serialize(stream, source);                
        return stream.ToArray();
    }
    
    public static T FromByteArray<T>(byte[] bytes)
    {
        var binaryFormatter = new BinaryFormatter();
        using var ms = new MemoryStream(bytes);
        object obj = binaryFormatter.Deserialize(ms);
        return (T)obj;
    }
}
