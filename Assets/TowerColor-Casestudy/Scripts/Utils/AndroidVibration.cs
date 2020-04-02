using UnityEngine;

public class AndroidVibration : MonoBehaviour {

#if !UNITY_EDITOR && UNITY_ANDROID
	private static readonly AndroidJavaObject Vibrator =
	new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
	.GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
	.Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.

#endif
	static AndroidVibration()
	{
		if (Application.isEditor) Handheld.Vibrate();
	}

	public static void Vibrate(long milliseconds)
	{
		if (Application.isEditor) {
			return;

		}
#if !UNITY_EDITOR && UNITY_ANDROID
			Vibrator.Call("vibrate", milliseconds);
#endif
	}

	public static void Vibrate(long[] pattern, int repeat)
	{
		if (Application.isEditor)
		{
			return;
		}
#if !UNITY_EDITOR && UNITY_ANDROID
			Vibrator.Call("vibrate", pattern, repeat);
#endif
	}

}