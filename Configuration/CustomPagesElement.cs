using System.Configuration;

namespace CustomPages.Configuration
{
    public class CustomPagesElement : ConfigurationElement
    {
        internal const string NameParam = "name";
        internal const string SiteNameParam = "sitename";
        internal const string MailGetterParam = "mailgetter";
        internal const string SMTPServerParam = "mailconfigsmtp";
        internal const string SMTPPortParam = "mailconfigsmtpport";
        internal const string MailFromParam = "mailconfigsender";
        internal const string MailPwdParam = "mailconfigpassword";
        internal const string MailDevParam = "maildev";
        internal const string PageFolderPathParam = "folderpath";

        /// <summary>
        /// Name of our custom parameters row, not very usefull, more a key than anything else.
        /// </summary>
        [ConfigurationProperty(NameParam, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NameParam]; }
            set { this[NameParam] = value; }
        }
        [ConfigurationProperty(SiteNameParam)]
        public string SiteName
        {
            get { return (string)this[SiteNameParam]; }
            set { this[SiteNameParam] = value; }
        }
        [ConfigurationProperty(MailGetterParam)]
        public string MailGetter
        {
            get { return (string)this[MailGetterParam]; }
            set { this[MailGetterParam] = value; }
        }
        [ConfigurationProperty(SMTPServerParam)]
        public string SMTPServer
        {
            get { return (string)this[SMTPServerParam]; }
            set { this[SMTPServerParam] = value; }
        }
        [ConfigurationProperty(SMTPPortParam)]
        public int SMTPServerPort
        {
            get { return (int)this[SMTPPortParam]; }
            set { this[SMTPPortParam] = value; }
        }
        [ConfigurationProperty(MailFromParam)]
        public string MailFrom
        {
            get { return (string)this[MailFromParam]; }
            set { this[MailFromParam] = value; }
        }
        [ConfigurationProperty(MailPwdParam)]
        public string MailFromPassword
        {
            get { return (string)this[MailPwdParam]; }
            set { this[MailPwdParam] = value; }
        }
        [ConfigurationProperty(MailGetterParam)]
        public string MailTo
        {
            get { return (string)this[MailGetterParam]; }
            set { this[MailGetterParam] = value; }
        }
        [ConfigurationProperty(MailDevParam)]
        public string MailDev
        {
            get { return (string)this[MailDevParam]; }
            set { this[MailDevParam] = value; }
        }
        /// <summary>
        /// added in v0.5, no need to update your web.config: will automatically return the default old value if absent.
        /// </summary>
        [ConfigurationProperty(PageFolderPathParam)]
        public string PageFolderPath
        {
            get
            {
                string s = (string)this[PageFolderPathParam];
                if (string.IsNullOrEmpty(s))
                {
                    s = DefaultPageFolderPath;
                }
                return s;
            }
            set { this[PageFolderPathParam] = value; }
        }
        private const string DefaultPageFolderPath = "~/Content/pages/";
    }
}
