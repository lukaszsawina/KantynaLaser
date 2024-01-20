using System.Net.Mail;

namespace KantynaLaser.IntegrationTests.Helper;

public static class SuffixGenerator
{
    public static string GetRandomSuffix(this string baseString)
    {
        string[] parts = baseString.Split('@');

        if (parts.Length == 2)
        {
            string suffix = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();

            return $"{parts[0]}_{suffix}@{parts[1]}";
        }

        return baseString;
    }
}
