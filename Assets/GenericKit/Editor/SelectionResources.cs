using System.Text;
using GenericKit.Runtime;
using UnityEditor;
using UnityEngine;

namespace GenericKit.Editor
{
	/// <summary>
	/// 选中相关资源
	/// </summary>
	public static class SelectionResources
	{
		/// <summary>
		/// 拷贝路径
		/// </summary>
		[MenuItem(GenericKitDefine.MENU_PATH_SELECTION_COPY_PATH, false, GenericKitDefine.MENU_PRIORITY_SELECTION_COPY_PATH)]
		private static void CopyPath()
		{
			var sb = new StringBuilder();
			foreach (var target in Selection.objects)
			{
				sb.AppendLine(AssetDatabase.GetAssetPath(target));
			}
			GUIUtility.systemCopyBuffer = sb.ToString();
		}

		/// <summary>
		/// 打印路径
		/// </summary>
		[MenuItem(GenericKitDefine.MENU_PATH_SELECTION_DEBUG_PATH, false, GenericKitDefine.MENU_PRIORITY_SELECTION_DEBUG_PATH)]
		private static void DebugPath()
		{
			foreach (var target in Selection.objects)
			{
				Debug.Log(AssetDatabase.GetAssetPath(target), target);
			}
		}

		/// <summary>
		/// 验证选择
		/// </summary>
		/// <returns>状态</returns>
		[MenuItem(GenericKitDefine.MENU_PATH_SELECTION_COPY_PATH, true)]
		[MenuItem(GenericKitDefine.MENU_PATH_SELECTION_DEBUG_PATH, true)]
		private static bool VerifySelection()
		{
			return Selection.objects.Length > 0;
		}
	}
}