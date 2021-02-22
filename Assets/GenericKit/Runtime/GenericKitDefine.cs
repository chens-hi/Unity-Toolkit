namespace GenericKit.Runtime
{
	/// <summary>
	/// 关于工具箱的定义
	/// </summary>
	public static class GenericKitDefine
	{
		private const string _MENU_PATH = "Assets/Tool Kit/";
		private const int _MENU_PRIORITY = 9000;

		public const string MENU_PATH_SELECTION_COPY_PATH = _MENU_PATH + "Selection/Copy Path";
		public const int MENU_PRIORITY_SELECTION_COPY_PATH = _MENU_PRIORITY + 101;
		public const string MENU_PATH_SELECTION_DEBUG_PATH = _MENU_PATH + "Selection/Debug Path";
		public const int MENU_PRIORITY_SELECTION_DEBUG_PATH = _MENU_PRIORITY + 102;
		
		public const string MENU_PATH_QUICKLY_FIND_MATCH = _MENU_PATH + "Find References/Quickly Find Match";
		public const int MENU_PRIORITY_QUICKLY_FIND_MATCH = _MENU_PRIORITY + 111;
		public const string MENU_PATH_PRECISE_FIND_MATCH = _MENU_PATH + "Find References/Precise Find Match";
		public const int MENU_PRIORITY_PRECISE_FIND_MATCH = _MENU_PRIORITY + 112;
	}
}