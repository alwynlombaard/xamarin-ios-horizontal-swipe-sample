using UIKit;

namespace HorizontalSwipe
{
	public static class IOSTheme
	{

		public static UIFont ThemeFontBold(int size)
		{
			return UIFont.FromName("Roboto-Bold", size);
		}

		public static UIFont ThemeFontRegular(int size)
		{
			return UIFont.FromName("Roboto-Regular", size);
		}

		public static UIFont ThemeFontLight(int size)
		{
			return UIFont.FromName("Roboto-Light", size);
		}

		public static UIFont ThemeFontMedium(int size)
		{
			return UIFont.FromName("Roboto-Medium", size);
		}

		public static UIColor JGLightPink => UIColor.FromRGB(172, 41, 181); //0xac29b5

	}
}
