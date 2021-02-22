using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GenericKit.Runtime;
using UnityEditor;
using UnityEngine;

namespace GenericKit.Editor
{
	/// <summary>
	/// 查找引用资源
	/// </summary>
	public static class FindReferences
	{
		/// <summary>
		/// 快速匹配扩展列表
		/// </summary>
		private static readonly List<string> _quicklyMatchExtensionList = new List<string>
		{
			".guiskin",
			".prefab",
			".unity",
			".mat",
			".asset"
		};
		/// <summary>
		/// 精准匹配依赖集合
		/// </summary>
		private static readonly Dictionary<string, HashSet<string>> _preciseMatchDependencyMap = new Dictionary<string, HashSet<string>>();

		/// <summary>
		/// 快速查找匹配
		/// </summary>
		[MenuItem(GenericKitDefine.MENU_PATH_QUICKLY_FIND_MATCH, false, GenericKitDefine.MENU_PRIORITY_QUICKLY_FIND_MATCH)]
		private static void QuicklyMatch()
		{
			if (EditorSettings.serializationMode == SerializationMode.ForceText)
			{
				var index = 0;
				var array = AssetDatabase.GetAllAssetPaths().Where(path => _quicklyMatchExtensionList.Contains(Path.GetExtension(path).ToLower())).ToArray();
				var targetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
				var targetGuid = AssetDatabase.AssetPathToGUID(targetPath);

				Debug.Log($"开始匹配资源: {targetPath}");
				EditorApplication.update = delegate
				{
					var over = false;
					if (index >= 0 && index < array.Length)
					{
						var assetPath = array[index++];
						if (EditorUtility.DisplayCancelableProgressBar("匹配资源中", assetPath, index * 1.0f / array.Length))
						{
							over = true;

							Debug.Log("终止匹配资源...");
						}
						else
						{
							if (Regex.IsMatch(File.ReadAllText(assetPath), targetGuid))
							{
								Debug.Log(assetPath, AssetDatabase.LoadMainAssetAtPath(assetPath));
							}
						}
					}
					else
					{
						over = true;

						Debug.Log("结束匹配资源...");
					}

					if (over)
					{
						EditorUtility.ClearProgressBar();
						EditorApplication.update = null;
					}
				};
			}
			else
			{
				if (EditorUtility.DisplayDialog("温馨提示", "此查找资源引用需要将编辑器中的序列化模式改为文本, 是否转换呢?", "确定", "取消"))
				{
					EditorSettings.serializationMode = SerializationMode.ForceText;
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
		}

		/// <summary>
		/// 精准查找匹配
		/// </summary>
		[MenuItem(GenericKitDefine.MENU_PATH_PRECISE_FIND_MATCH, false, GenericKitDefine.MENU_PRIORITY_PRECISE_FIND_MATCH)]
		private static void PreciseMatch()
		{
			var index = 0;
			var array = AssetDatabase.GetAllAssetPaths();
			var targetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

			Debug.Log($"开始匹配资源: {targetPath}");
			EditorApplication.update = delegate
			{
				var over = false;
				if (index >= 0 && index < array.Length)
				{
					var assetPath = array[index++];
					if (!_preciseMatchDependencyMap.ContainsKey(assetPath) || !AssetDatabase.GetAssetDependencyHash(assetPath).isValid)
					{
						_preciseMatchDependencyMap[assetPath] = new HashSet<string>(AssetDatabase.GetDependencies(assetPath, false));
					}
					
					if (EditorUtility.DisplayCancelableProgressBar("匹配资源中", assetPath, index * 1.0f / array.Length))
					{
						over = true;

						Debug.Log("终止匹配资源...");
					}
					else
					{
						if (_preciseMatchDependencyMap.TryGetValue(assetPath, out var target) && target.Contains(targetPath))
						{
							Debug.Log(assetPath, AssetDatabase.LoadMainAssetAtPath(assetPath));
						}
					}
				}
				else
				{
					over = true;

					Debug.Log("结束匹配资源...");
				}

				if (over)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
				}
			};
		}

		/// <summary>
		/// 验证匹配
		/// </summary>
		/// <returns>状态</returns>
		[MenuItem(GenericKitDefine.MENU_PATH_QUICKLY_FIND_MATCH, true)]
		[MenuItem(GenericKitDefine.MENU_PATH_PRECISE_FIND_MATCH, true)]
		private static bool VerifyMatch()
		{
			return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(Selection.activeObject));
		}
	}
}