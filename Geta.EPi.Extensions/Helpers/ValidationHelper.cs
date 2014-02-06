using System.Net.Mail;
namespace Geta.EPi.Cms.Helpers
{
	public static class ValidationHelper
	{
		public static bool IsValidEmail(string email)
		{
			try
			{
				new MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}