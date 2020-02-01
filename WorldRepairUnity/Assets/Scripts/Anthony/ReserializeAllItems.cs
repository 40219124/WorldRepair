using System.IO;
using UnityEditor;
using UnityEngine;

public class ReserializeAllItems : MonoBehaviour
{
#if UNITY_2017_1_OR_NEWER
	[MenuItem("Toolkit/Reserialize")]
	public static void Reserialize()
	{
		AssetDatabase.ForceReserializeAssets();
	}
#endif

#if UNITY_5_4_OR_NEWER
	[MenuItem("Toolkit/Delete Empty Directories")]
	public static void Run()
	{
		Process(Application.dataPath);
	}

	public static bool Process(string dir)
	{
		string[] subdirs = Directory.GetDirectories(dir);

		int deletedCount = 0;
		foreach (string subdir in subdirs)
		{
			if (Process(subdir))
			{
				deletedCount++;
			}
		}

		if (deletedCount >= subdirs.Length)
		{
			// optimisation - don't query the file list if we have subdirs
			string[] files = Directory.GetFiles(dir);

			bool delete = false;
			if (files.Length == 0)
			{
				delete = true;
			}
			else if ((files.Length == 1) && files[0].EndsWith("/.DS_Store"))
			{
				File.Delete(Path.Combine(dir, ".DS_Store"));
				delete = true;
			}

			if (delete)
			{
				// empty, remove
				Directory.Delete(dir, recursive: false);

				// remove meta
				File.Delete(dir + ".meta");

				return true;
			}
		}

		return false;
	}
#endif
}
