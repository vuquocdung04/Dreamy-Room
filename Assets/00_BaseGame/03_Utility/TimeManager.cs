public class TimeManager
{
    public static System.DateTime GetCurrentTime()
    {
        return System.DateTime.Now;
    }

    public static bool HasDayPassed(System.DateTime oldTime, System.DateTime currentTime)
    {
        System.DateTime replaceOldTime = new System.DateTime(oldTime.Year, oldTime.Month, oldTime.Day);
        System.DateTime replaceCurrentTime = new System.DateTime(currentTime.Year, currentTime.Month, currentTime.Day);

        if (replaceOldTime < replaceCurrentTime)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Lấy ngày bắt đầu của tuần chứa 'time'. Mặc định tuần bắt đầu vào thứ Hai.
    /// </summary>
    public static System.DateTime GetStartOfWeek(System.DateTime time, System.DayOfWeek startOfWeek = System.DayOfWeek.Monday)
    {
        // Sử dụng time.Date để loại bỏ phần giờ, phút, giây
        System.DateTime date = time.Date; 
        // Tính toán số ngày cần trừ đi để về đầu tuần
        int diff = (7 + (int)date.DayOfWeek - (int)startOfWeek) % 7;
        return date.AddDays(-diff);
    }

    public static System.DateTime GetStartOfDay(System.DateTime time)
    {
        return new  System.DateTime(time.Year, time.Month, time.Day);
    }

    public static long CalculateTime(System.DateTime oldTime, System.DateTime newTime)
    {
        System.TimeSpan diff = newTime - oldTime;
        long result = diff.Days * 24 * 60 * 60 +  diff.Hours * 60 * 60 + diff.Minutes * 60 + diff.Seconds;
        return result;
    }
}