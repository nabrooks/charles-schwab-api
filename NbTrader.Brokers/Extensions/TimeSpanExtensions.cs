﻿namespace NbTrader.Brokers.Extensions
{
    /// <summary>
    /// TimeSpan extension methods
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Extension Methods

        #region DaysRemainder

        /// <summary>
        /// Days in the TimeSpan minus the months and years
        /// </summary>
        /// <param name="Span">TimeSpan to get the days from</param>
        /// <returns>The number of days minus the months and years that the TimeSpan has</returns>
        public static int DaysRemainder(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Day - 1;
        }

        #endregion

        #region Months

        /// <summary>
        /// Months in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the months from</param>
        /// <returns>The number of months that the TimeSpan has</returns>
        public static int Months(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Month - 1;
        }

        #endregion

        #region Years

        /// <summary>
        /// Years in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the years from</param>
        /// <returns>The number of years that the TimeSpan has</returns>
        public static int Years(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Year - 1;
        }

        #endregion

        #region ToStringFull

        /// <summary>
        /// Converts the input to a string in this format:
        /// (Years) years, (Months) months, (DaysRemainder) days, (Hours) hours, (Minutes) minutes, (Seconds) seconds
        /// </summary>
        /// <param name="Input">Input TimeSpan</param>
        /// <returns>The TimeSpan as a string</returns>
        public static string ToStringFull(this TimeSpan Input)
        {
            string Result = "";
            string Splitter = "";
            if (Input.Years() > 0) { Result += Input.Years() + " year" + (Input.Years() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Months() > 0) { Result += Splitter + Input.Months() + " month" + (Input.Months() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.DaysRemainder() > 0) { Result += Splitter + Input.DaysRemainder() + " day" + (Input.DaysRemainder() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Hours > 0) { Result += Splitter + Input.Hours + " hour" + (Input.Hours > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Minutes > 0) { Result += Splitter + Input.Minutes + " minute" + (Input.Minutes > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Seconds > 0) { Result += Splitter + Input.Seconds + " second" + (Input.Seconds > 1 ? "s" : ""); Splitter = ", "; }
            return Result;
        }

        #endregion

        #endregion
    }
}
