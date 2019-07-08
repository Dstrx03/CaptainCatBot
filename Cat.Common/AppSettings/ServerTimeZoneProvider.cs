﻿using System;
using TimeZoneConverter;

namespace Cat.Common.AppSettings
{
    public class ServerTimeZoneProvider
    {
        public static string Win()
        {
            return TimeZoneInfo.Local.Id;
        }

        public static string Iana()
        {
            return TZConvert.WindowsToIana(Win());
        }
    }
}
