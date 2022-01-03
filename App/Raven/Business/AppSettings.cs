namespace App.Business
{
    public interface AppSettings
    {
        public static bool FirstBoot
        {
            get
            {
                return DBInterface.GetFirstBoot();
            }
            set
            {
                DBInterface.SetFirstBoot(value);
            }
        }
        public static string DefaultCurrency
        {
            get
            {
                return DBInterface.GetDefaultCurrency();
            }
            set
            {
                DBInterface.SetDefaultCurrency(value);
            }
        }
        public static string DefaultLanguage
        {
            get
            {
                return DBInterface.GetDefaultLanguage();
            }
            set
            {
                DBInterface.SetDefaultLanguage(value);
            }
        }
        public static int AutoSavePeriod
        {
            get
            {
                return DBInterface.GetAutoSavePeriod();
            }
            set
            {
                DBInterface.SetAutoSavePeriod(value);
            }
        }
        public static bool DarkMode
        {
            get
            {
                return DBInterface.GetDarkMode();
            }
            set
            {
                DBInterface.SetDarkMode(value);
            }
        }
    }
}
